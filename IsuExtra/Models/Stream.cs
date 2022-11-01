using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Stream
    {
        private int _studentLimit;

        public Stream(Course course, int studentLimit)
        {
            Course = course;
            StudentLimit = studentLimit;
        }

        public int StudentLimit
        {
            get => _studentLimit;
            private set
            {
                if (value < 1) throw new IsuExtraExceptions("Student stream limit must be more than 0!");
                _studentLimit = value;
            }
        }

        public List<Student> Students { get; } = new ();
        public Course Course { get; }

        public List<Lesson> Schedule { get; } = new ();
        public void AddLesson(Lesson newLesson)
        {
            if (Schedule.Any(l => (l.WeekDay == newLesson.WeekDay &&
                                   ((l.EndTime >= newLesson.StartTime && l.StartTime <= newLesson.StartTime) ||
                                    (l.EndTime >= newLesson.EndTime && l.StartTime <= newLesson.EndTime)))))
            {
                throw new IsuExtraExceptions("This lesson can't be added in Schedule!");
            }

            Schedule.Add(newLesson);
            newLesson.SetStream(this);
        }

        public bool RemoveStudent(Student student)
        {
            return Students.Remove(student);
        }

        public bool AddStudent(Student student)
        {
            if (Students.Count >= _studentLimit || !CheckScheduleCompatibility(student)) return false;
            if (!student.AddCourse(Course))
            {
                return false;
            }

            Students.Add(student);
            return true;
        }

        public bool CheckScheduleCompatibility(Student student)
        {
            foreach (Lesson lesson in Schedule)
            {
                if ((student.Group as Group)?.Schedule.Any(l => l.WeekDay == lesson.WeekDay &&
                                                                ((l.EndTime >= lesson.StartTime &&
                                                                 l.StartTime <= lesson.StartTime) ||
                                                                 (l.EndTime >= lesson.EndTime &&
                                                                 l.StartTime <= lesson.EndTime))) ??
                    throw new IsuExtraExceptions("You can't do this with old group"))
                {
                    return false;
                }
            }

            return true;
        }

        public Student GetStudent(int id)
        {
            return Students.FirstOrDefault(s => s.Id == id);
        }

        public Student FindStudent(string name)
        {
            return Students.FirstOrDefault(s => s.Name == name);
        }

        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
        }
    }
}