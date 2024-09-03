namespace BusinessLayer.Interfaces
{
    public interface ILanguageService
    {

        public Task<Language> CreateLanguageAsync(LanguageRequest request);
        public Task<Language> GetLanguageByIdAsync(int id);
        public Task<IEnumerable<Language>> GetAllLanguagesAsync();
        public Task<Language> UpdateLanguageAsync(int id, LanguageRequest request);
        public Task DeleteLanguageAsync(int id);
    }
}
