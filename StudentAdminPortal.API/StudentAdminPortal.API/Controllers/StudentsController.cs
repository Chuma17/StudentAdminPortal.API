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
        private readonly IImageRepository _imageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetStudentsAsync();

            return Ok(_mapper.Map<List<StudentDto>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudentAsync")]
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

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        //UpdateStudentRequest is the object containing all the properties that the user will be allowed to change
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.Exists(studentId))
            {
                //Update Details
                var updatedStudent = await _studentRepository.UpdateStudent(studentId, _mapper.Map<Student>(request));

                if (updatedStudent != null)
                {
                    return Ok(_mapper.Map<Student>(updatedStudent));
                }
            }

            return NotFound();

        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepository.Exists(studentId))
            {
                var student = await _studentRepository.DeleteStudentAsync(studentId);
                return Ok(_mapper.Map<Student>(student));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = await _studentRepository.AddStudentAsync(_mapper.Map<Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id },
                _mapper.Map<Student>(student));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            if (await _studentRepository.Exists(studentId))
            {
                var fileName = Guid.NewGuid() + Path.GetFileName(profileImage.FileName);
                var fileImagePath = await _imageRepository.Upload(profileImage, fileName);
                if(await _studentRepository.UpdateProfileImage(studentId, fileImagePath))
                {
                    return Ok(fileImagePath);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
            }

            return NotFound();
        }
    }
}
