
namespace DataAccessLayer.Repositories
{
    public class LanguageRepository(AppDbContext appDbContext) : ILanguageRepository
    {
        private readonly AppDbContext appDbContext = appDbContext;
        public async Task<Language> CreateLanguageAsync(Language language)
        {
            appDbContext.Languages.Add(language);
            await appDbContext.SaveChangesAsync();
            return language;
        }

        public Task DeleteLanguageAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Language> GetLanguageByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Language> UpdateLanguageAsync(int id, Language request)
        {
            appDbContext.Languages.Update(request);
            await appDbContext.SaveChangesAsync();
            return request;
        }
    }
}
