using System;
using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Course
    {
        public Course(string name, Department department, int streams, int studentLimit)
        {
            Name = name;
            Department = department;
            if (streams < 1) throw new IsuExtraExceptions("It must be at least one stream");

            for (int i = 0; i < streams; i++)
            {
                var stream = new Stream(this, studentLimit);
                Streams.Add(stream);
            }
        }

        public List<Stream> Streams { get; } = new ();
        public string Name { get; }
        public Department Department { get; }

        public bool AddStudent(Student student)
        {
            if ((student.Group as Group)?.Department == Department)
            {
                throw new IsuExtraExceptions("Incorrect Course! Same Department!");
            }

            foreach (Stream stream in Streams)
            {
                if (stream.AddStudent(student))
                {
                    return true;
                }
            }

            return false;
        }

        public void CancelCoursePick(Student student)
        {
            foreach (Stream stream in Streams)
            {
                if (stream.RemoveStudent(student))
                {
                    if (student.CancelCoursePick(this))
                    {
                        return;
                    }
                }
            }

            throw new IsuExtraExceptions("U can't cancel this course pick, cause u r not joining this course!");
        }
    }
}