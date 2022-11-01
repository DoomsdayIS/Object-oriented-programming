using System.Collections.Generic;

namespace IsuExtra.Models
{
    public class Student : Isu.Models.Student
    {
        public Student(string name)
            : base(name)
        {
        }

        public List<Course> StudentsCourses { get; } = new ();

        public bool AddCourse(Course course)
        {
            if (StudentsCourses.Count >= 2 || StudentsCourses.Contains(course))
            {
                return false;
            }

            StudentsCourses.Add(course);
            return true;
        }

        public bool CancelCoursePick(Course course)
        {
            return StudentsCourses.Remove(course);
        }
    }
}