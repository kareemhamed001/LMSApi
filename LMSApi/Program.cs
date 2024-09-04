using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Hangfire;
using LMSApi.App.Filters;
using LMSApi.App.Middlewares;
using LMSApi.App.Options;
using LMSApi.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRespository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<ILessonContentRepository, LessonContentRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IClassService, ClassServices>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<ILessonContentService, LessonContentService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();

builder.Services.AddScoped<PermissionSeeder>();

builder.Services.AddHttpContextAccessor();

JwtOptions? jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
if (jwtOptions == null)
{
    throw new Exception("Jwt options not found");
}
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        };
    });

builder.Services.AddDistributedMemoryCache();
// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();
builder.Services.AddControllers(options => { options.Filters.Add<PermissionCheckFilter>(); });
var app = builder.Build();


await using (var scope = app.Services.CreateAsyncScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
    await seeder.Seed();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/dashboard");
}

app.UseHttpsRedirection();

app.UseMiddleware<LanguageMiddleware>();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
