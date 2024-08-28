using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSApi.App.DTOs;
using LMSApi.App.Interfaces.Class;
using LMSApi.App.Requests.Class;
using AutoMapper;
using LMSApi.App.Attributes;

namespace LMSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IMapper _mapper;

        public ClassController(IClassService classService, IMapper mapper)
        {
            _classService = classService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [CheckPermission("Class.index")]
        public async Task<ActionResult<ClassRequest>> GetClassById(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            if (classEntity == null) return NotFound();
            var classDto = _mapper.Map<ClassRequest>(classEntity);

            return Ok(classDto);
        }

        [HttpGet]
        [CheckPermission("Class.AllClass")]
        public async Task<ActionResult<IEnumerable<ClassRequest>>> GetAllClasses()
        {
            var classEntities = await _classService.GetAllClassesAsync();
            var classDtos = _mapper.Map<IEnumerable<ClassRequest>>(classEntities);

            return Ok(classDtos);
        }

        [HttpPost]
        [CheckPermission("Class.createClass")]
        public async Task<ActionResult> CreateClass(ClassRequest classDto)
        {

            var classEntity = _mapper.Map<Class>(classDto);

            await _classService.CreateClassAsync(classEntity);

            return CreatedAtAction(nameof(GetClassById), new { id = classEntity.Id }, classDto);
        }

        [HttpPut("{id}")]
        [CheckPermission("class.updateClass")]
        public async Task<ActionResult> UpdateClass(int id, ClassRequest classDto)
        {

            var classEntity = _mapper.Map<Class>(classDto);
            classEntity.Id = id;

            await _classService.UpdateClassAsync(id, classEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [CheckPermission("class.deleteClass")]
        public async Task<ActionResult> DeleteClass(int id)
        {
            await _classService.DeleteClassAsync(id);

            return NoContent();
        }

        [HttpGet("{classId}/students")]
        [CheckPermission("class.getStudentsByClassId")]
        public async Task<ActionResult<GetStudentOfClassRequest>> GetStudentsByClassId(int classId)
        {
            var classEntity = await _classService.GetClassByIdAsync(classId);
            if (classEntity == null) return NotFound();

            // Get students for the class
            var students = await _classService.GetStudentsByClassIdAsync(classId);

            // Map the class entity to GetStudentOfClassRequest
            var getStudentOfClassRequest = _mapper.Map<GetStudentOfClassRequest>(classEntity);

            // Map students to StudentDto and set it in the request
            getStudentOfClassRequest.Students = _mapper.Map<List<StudentDto>>(students);

            return Ok(getStudentOfClassRequest);
        }
    }
}