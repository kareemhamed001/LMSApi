using AutoMapper;
using LMSApi.App.Atrributes;

namespace LMSApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {

        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;

        public LanguagesController(ILanguageService languageService, IMapper mapper)
        {
            _languageService = languageService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [CheckPermission("Language.index")]
        public async Task<ActionResult> GetLanguageById(int id)
        {
            var languageEntity = await _languageService.GetLanguageByIdAsync(id);
            if (languageEntity == null) return NotFound();
            var languageDto = _mapper.Map<LanguageRequest>(languageEntity);

            return Ok(languageDto);
        }

        [HttpGet]
        [CheckPermission("Language.AllLanguage")]
        public async Task<ActionResult<IEnumerable<LanguageRequest>>> GetAllLanguages()
        {
            var languageEntities = await _languageService.GetAllLanguagesAsync();
            var languageDtos = _mapper.Map<IEnumerable<LanguageRequest>>(languageEntities);

            return Ok(languageDtos);
        }

        [HttpPost]
        //[CheckPermission("Language.createLanguage")]
        public async Task<ActionResult> CreateLanguage(LanguageRequest languageDto)
        {

            Language language = await _languageService.CreateLanguageAsync(languageDto);

            return Ok(ApiResponseFactory.Create(language, "Language created successfully", 201, true));
        }

        [HttpPut("{id}")]
        [CheckPermission("Language.updateLanguage")]
        public async Task<ActionResult> UpdateLanguage(int id, LanguageRequest languageDto)
        {

            await _languageService.UpdateLanguageAsync(id, languageDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [CheckPermission("Language.deleteLanguage")]
        public async Task<ActionResult> DeleteLanguage(int id)
        {
            await _languageService.DeleteLanguageAsync(id);

            return NoContent();
        }
    }
}
