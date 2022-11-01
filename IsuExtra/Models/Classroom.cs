using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Classroom
    {
        private int _classroomNumber;

        public Classroom(int classroomNumber)
        {
            ClassroomNumber = classroomNumber;
        }

        public int ClassroomNumber
        {
            get => _classroomNumber;
            private set
            {
                if (value is < 1 or > 599)
                    throw new IsuExtraExceptions("Incorrect classroom number, it must be between 100 and 599");

                _classroomNumber = value;
            }
        }

        private List<Lesson> Lessons { get; } = new ();

        public void AddLesson(Lesson lesson)
        {
            Lessons.Add(lesson);
        }
    }
}