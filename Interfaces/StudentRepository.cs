
using ExoAPI.Context;
using ExoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExoAPI.Interfaces;
public class StudentRepository : IStudentRepository
    {
        private ContextDB _mainContext;
        public StudentRepository(ContextDB context)
        {
            _mainContext = context;
        }
        //Get All Students;
        public async Task<List<Student>> GetStudents() {

            return await _mainContext.Students
                            .Include(student => student.Courses)
                            .ToListAsync();
        }
        //Get Student By ID:
        public async Task<Student> GetStudentById(int id) 
        {
            Student student = _mainContext.Students.Include(std => std.Courses).FirstOrDefault(el => el.Id == id);
            if(student == null) {
                throw new ArgumentNullException(nameof(student));
            }

            return student;
        }
        //ADD Student
        public async  void AddStudent(Student student)
        {
            if(student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            await _mainContext.Students.AddAsync(student);
            await _mainContext.SaveChangesAsync();

        }
        //Delete Student By ID
        public async Task<bool> Delete(int StudentId)
            {
                bool result = false;

                List<Student> students = _mainContext.Students
                                        .Include(std => std.Courses)
                                        .Where(el => el.Id == StudentId)
                                        .ToList();

                if (students.Count() == 1)
                {
                    Student student = students[0];

                    if (student != null)
                    {
                        if (student.Courses != null)
                        {
                            foreach (Course course in student.Courses)
                            {
                                _mainContext.Courses.Remove(course);
                            }
                        }

                        _mainContext.Students.Remove(student);
                        await _mainContext.SaveChangesAsync();

                        result = true;
                    }
                }
                return result;
            }
        
        //Update student:
        // public async void UpdateStudentInfo(Student student, int studentId)
        // {
                
        //     if(student == null)
        //     {
        //         throw new ArgumentNullException();
        //     }

        //     //Student exist ?
        //     Student? studentExist = _mainContext.Students.FirstOrDefault(std => std.Id == studentId);

        //     if(studentExist == null)
        //     {
        //         throw new ArgumentNullException();
        //     }

        //     _mainContext.Entry(student).State = EntityState.Modified;

        //     await _mainContext.SaveChangesAsync();

        // }
        /////////////// COURSES /////////////////////////
        //Get all Courses of a student:

        public async Task<List<Course>> GetStudentCourses(int StudentId) {

        return await _mainContext.Courses.Where(crs => crs.Id == StudentId)
                            .ToListAsync();
        }
        
        //Add Course to a specific Student
        public async Task<bool> AddCourse(int StudentId, Course course)
        {
            bool result = false;

            Student student = _mainContext.Students.Find(StudentId);

            if(student != null )
            {
                if (student.Courses == null)
                {
                    student.Courses = new List<Course>();
                }
                student.Courses.Add(course);
                _mainContext.SaveChangesAsync();

                if (course.Id != 0)
                {
                    result = true;
                }
            }
            return result;
        }


        //Delete Course:

        public async Task<bool> DeleteCourse(int studentId, int courseId) 
        {
            bool result = false;
            Course? course =  _mainContext.Courses.Find(courseId);
            Student? student = _mainContext.Students.Find(studentId);

            //If course / student exist
            if(course != null && student != null) {

                //if student->courses exists and if the specific course exists --> then remove
                if(student.Courses != null && student.Courses.Where(crs => crs.Id == courseId).Any())
                {
                    student.Courses.Remove(course);
                    _mainContext.Courses.Remove(course);

                    await _mainContext.SaveChangesAsync();
                    result = true;
                }
            }

            return result;
        }
    }

