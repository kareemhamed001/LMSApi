using DataAccessLayer.Interfaces;
using ILanguageService = BusinessLayer.Interfaces.ILanguageService;

namespace BusinessLayer.Services
{
    public class LanguageService(ILanguageRepository languageRepository) : ILanguageService
    {
        private readonly ILanguageRepository languageRepository = languageRepository;
        public async Task<Language> CreateLanguageAsync(LanguageRequest request)
        {
            Language language = new Language
            {
                Name = request.Name,
                Code = request.Code
            };
            
            return await languageRepository.CreateLanguageAsync(language);
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
