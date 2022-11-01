using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Group : Isu.Models.Group
    {
        public Group(string name)
        {
            Department = GetDepartmentFromGroupName(name);
            Name = name;
            Number = new CourseNumber((int)char.GetNumericValue(name[2]));
        }

        public List<Lesson> Schedule { get; } = new ();

        public Department Department { get; }

        public Department GetDepartmentFromGroupName(string name)
        {
            if (!((name.Length == 5)
                  && (name[0] >= 'A' && name[0] <= 'Z')
                  && (name[1] == '3' || name[1] == '4')
                  && (name[2] >= '1' && name[2] <= '4')
                  && (name[3] >= '0' && name[3] <= '9')
                  && (name[4] >= '0' && name[4] <= '9')))
            {
                throw new IsuExtraExceptions("Incorrect group name!");
            }

            return name[0] switch
            {
                'M' => Department.SchoolOfTranslationalInformationTechnologies,
                'A' => Department.SchoolOfLifeSciences,
                'B' => Department.InstituteOfInternationalDevelopmentAndPartnership,
                _ => Department.SchoolOfPhysicsAndEngineering
            };
        }

        public void AddLesson(Lesson newLesson)
        {
            if (Schedule.Any(l => (l.WeekDay == newLesson.WeekDay &&
                                 ((l.EndTime >= newLesson.StartTime && l.StartTime <= newLesson.StartTime) ||
                                  (l.EndTime >= newLesson.EndTime && l.StartTime <= newLesson.EndTime)))))
            {
                throw new IsuExtraExceptions("This lesson can't be added in Schedule!");
            }

            Schedule.Add(newLesson);
            newLesson.SetGroup(this);
        }

        public void AddStudent(Student student)
        {
            base.AddStudent(student);
        }

        public new Student GetStudent(int id)
        {
            return base.GetStudent(id) as Student;
        }

        public new Student FindStudent(string name)
        {
            return base.FindStudent(name) as Student;
        }

        public List<Student> GetStudentWithoutCourses()
        {
            return Students.Select(s => s as Student).
                Where(s => s?.StudentsCourses.Count == (int?)0).ToList();
        }
    }
}