using System.Collections.Generic;

namespace IsuExtra.Models
{
    public class Teacher
    {
        public Teacher(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public List<Lesson> Lessons { get; } = new ();

        public void AddLesson(Lesson lesson)
        {
            Lessons.Add(lesson);
        }
    }
}