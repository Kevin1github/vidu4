using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidu4.Controllers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _context;

        public StudentsController(StudentContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            if (students.Count == 0)
            {
                return NotFound(new { message = "No students found" });
            }
            return Ok(students);
        }

        // GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            {
                var students = await _context.Students.ToListAsync();
                return Ok(students); // Trả về mảng rỗng nếu không có dữ liệu
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }
            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name) || string.IsNullOrWhiteSpace(student.Class))
            {
                return BadRequest(new { message = "Invalid student data. Name and Class are required." });
            }

            if (!string.IsNullOrWhiteSpace(student.Photo))
            {
                if (!student.Photo.StartsWith("data:image"))
                {
                    return BadRequest(new { message = "Invalid image format. Must be Base64 encoded." });
                }
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/students/UpdatePhoto/{id}
        [HttpPut("UpdatePhoto/{id}")]
        public async Task<IActionResult> UpdatePhoto(int id, [FromBody] PhotoUpdateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Photo))
            {
                return BadRequest(new { message = "Image cannot be empty." });
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            string base64ImageString = request.Photo;

            // Nếu chuỗi không bắt đầu bằng "data:image", kiểm tra xem có phải Base64 hợp lệ không và thêm prefix mặc định
            if (!base64ImageString.StartsWith("data:image"))
            {
                try
                {
                    // Kiểm tra xem chuỗi có phải Base64 hợp lệ không
                    Convert.FromBase64String(base64ImageString);
                    // Nếu hợp lệ, thêm prefix mặc định (ở đây giả sử ảnh PNG)
                    base64ImageString = "data:image/png;base64," + base64ImageString;
                }
                catch
                {
                    return BadRequest(new { message = "Invalid image format. Must be Base64 encoded." });
                }
            }

            student.Photo = base64ImageString;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Photo updated successfully!", student });
        }
    }
}