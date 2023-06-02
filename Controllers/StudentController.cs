

using ExoAPI.Context;
using ExoAPI.Interfaces;
using ExoAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExoAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class StudentController: ControllerBase {
    
    private readonly IStudentRepository _repo;
    private readonly ContextDB _context;
        public StudentController(IStudentRepository repo, ContextDB context)
        {
            _repo = repo;
            _context = context;
        }
        // GET: api/[controller]
        [Route("getstudents")]
        [HttpGet]
        public Task<List<Student>> GetStudent()
        {
    
                var students = _repo.GetStudents();
                if(students == null) {
                    StatusCode(500);
                }
                return students;
            
        }
        //         // get STUDENT by id
        [Route("getStudentById/{id:int}")]
        [HttpGet]
        public async Task<Student> GetStudentById(int id ){
                
            var student =  await _repo.GetStudentById(id);

            if(student == null ){
                StatusCode(500);
            }
            return student;
        }


        //Add Student
        [Route("addStudent")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Student student)
        {
            if(student == null)
            {
                return NotFound();
            }
            try
            {
                _repo.AddStudent(student);
                return Ok("Value Added");
            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE STUDENT
        [Route("removestudent/{id:int}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteStudent(int id) 
        {
            bool result = await _repo.Delete(id);

            if(result != true) {
                NotFound();
            }
            return Ok();
        }

        //Update Student
        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateStudentInfo(int id, Student student)
        // {
        //     if (id != student.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        //         await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, JsonPatchDocument<Student> studentUpdates) {
            
            var student =  await _repo.GetStudentById(id);

            studentUpdates.ApplyTo(student);
            _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Add Course
        [HttpGet]
        [Route("getStudentCourses/{id:int}")]

        public async Task<List<Course>> GetStudentCourses( int id) {
       
                var result = await _repo.GetStudentCourses(id);
                if(result == null) {
                      NotFound();
                }
                return result;
        }
        //Add Course
        [HttpPost]
        [Route("addcourse/{id:int}")]
        public async Task<IActionResult> AddCourse(int id, Course crs)
        {
            try
            {
                bool res = await _repo.AddCourse(id, crs);

                if (res)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(304);
                }

            }
            catch (Exception err)
            {
                return StatusCode(500, err);
            }
        }

        //Delete Course
    [Route("removecourse/{idStudent:int}/{idCourse:int}")]
    [HttpDelete]
    public async Task<IActionResult> RemoveStudent(int idStudent, int idCourse)
    {
    
            bool res = await _repo.DeleteCourse(idStudent, idCourse);

            if (res)
            {
                Console.WriteLine(res);
                return Ok();
            }
            else
            {
                Console.WriteLine(res);
                return StatusCode(304);
            }

        

    }
    
}
