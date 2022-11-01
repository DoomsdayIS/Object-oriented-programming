using System;
using System.Globalization;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Lesson
    {
        public Lesson(DayOfWeek weekDay, DateTime startTime, DateTime endTime, Classroom classroom, Teacher teacher)
        {
            if (endTime < startTime)
            {
                throw new IsuExtraExceptions("Start time should be earlier than end time!");
            }

            WeekDay = weekDay;
            StartTime = startTime;
            EndTime = endTime;
            LessonAuditorium = classroom;
            classroom.AddLesson(this);
            Teacher = teacher;
            teacher.AddLesson(this);
        }

        public DateTime EndTime { get; }
        public DateTime StartTime { get; }

        public Classroom LessonAuditorium { get; }
        public Teacher Teacher { get; }
        public Group Group { get; private set; }
        public Stream Stream { get; private set; }

        public DayOfWeek WeekDay { get; private set; }
        public void SetGroup(Group group)
        {
            Group = group;
        }

        public void SetStream(Stream stream)
        {
            Stream = stream;
        }
    }
}