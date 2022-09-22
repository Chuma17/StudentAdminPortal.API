using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Models;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetStudentsAsync();            
                        
            return Ok(_mapper.Map<List<StudentDto>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            //Fetch student details
            var student = await _studentRepository.GetStudentAsync(studentId);

            //Return student
            if (student == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<StudentDto>(student));
        }
    }
}
