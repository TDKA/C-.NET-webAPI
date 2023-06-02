
namespace ExoAPI.Interfaces;
using ExoAPI.Models;

using Microsoft.AspNetCore.Mvc;

public interface IStudentRepository
    {
        Task<List<Student>> GetStudents();

        Task<Student>GetStudentById(int studentId);
        void AddStudent(Student student);
        Task<bool> Delete(int studentId);

        // void UpdateStudentInfo(Student student, int studentId);


        //Courses
        Task<bool> AddCourse(int studentId, Course course);
        Task<List<Course>> GetStudentCourses(int studentId);
        Task<bool> DeleteCourse(int studentId, int courseId);
    // Task<IActionResult> UpdateStudentInfo(int id, Student student);
}