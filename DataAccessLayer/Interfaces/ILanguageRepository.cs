

namespace DataAccessLayer.Interfaces
{
    public interface ILanguageRepository
    {

        public Task<Language> CreateLanguageAsync(Language request);
        public Task<Language> GetLanguageByIdAsync(int id);
        public Task<IEnumerable<Language>> GetAllLanguagesAsync();
        public Task<Language> UpdateLanguageAsync(int id, Language request);
        public Task DeleteLanguageAsync(int id);
    }
}
