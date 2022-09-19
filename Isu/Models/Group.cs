using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Models
{
    public class Group
    {
        public Group(string name)
        {
            if (name.Length != 5 && name[0] != 'M' && name[1] != '3')
            {
                throw new IsuException("Incorrect group name");
            }

            Name = name;
            Number = new CourseNumber((int)char.GetNumericValue(name[2]));
        }

        protected Group()
        {
        }

        public List<Student> Students { get; private set; } = new ();
        public string Name { get; protected set; }
        public CourseNumber Number { get; protected set; }

        public void AddStudent(Student student)
        {
            if (Students.Count > 25)
            {
                throw new IsuException("Too many students!");
            }

            student.ChangeGroup(this);
            Students.Add(student);
        }

        public Student GetStudent(int id)
        {
            foreach (Student student in Students)
            {
                if (student.Id == id)
                {
                    return student;
                }
            }

            return null;
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in Students)
            {
                if (student.Name == name)
                {
                    return student;
                }
            }

            return null;
        }

        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
        }
    }
}