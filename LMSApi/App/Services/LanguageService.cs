using LMSApi.App.Interfaces;
using LMSApi.App.Requests;
using LMSApi.Database.Data;
using LMSApi.Database.Enitities;

namespace LMSApi.App.Services
{
    public class LanguageService(AppDbContext appDbContext) : ILanguageService
    {
        private readonly AppDbContext appDbContext = appDbContext;
        public async Task<Language> CreateLanguageAsync(LanguageRequest request)
        {
            Language language = new Language
            {
                Name = request.Name,
                Code = request.Code
            };
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

        public Task<Language> UpdateLanguageAsync(int id, LanguageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
