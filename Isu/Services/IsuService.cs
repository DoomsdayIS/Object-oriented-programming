using System;
using System.Collections.Generic;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private List<Group> _groupList = new List<Group>();

        public Group AddGroup(string name)
        {
            Group group = new Group(name);
            _groupList.Add(group);

            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            Student student = new Student(name);
            group.AddStudent(student);
            return student;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _groupList)
            {
                Student student = group.GetStudent(id);
                if (student != null)
                {
                    return student;
                }
            }

            throw new IsuException("There is no student with this id");
        }

        public Student FindStudent(string name)
        {
            foreach (Group group in _groupList)
            {
                Student student = group.FindStudent(name);
                if (student != null)
                {
                    return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in _groupList)
            {
                if (group.Name == groupName)
                {
                    return group.Students;
                }
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            List<Student> courseStudents = new List<Student>();
            foreach (Group group in _groupList)
            {
                if (group.Number == courseNumber)
                {
                    courseStudents.AddRange(group.Students);
                }
            }

            return courseStudents;
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in _groupList)
            {
                if (group.Name == groupName)
                {
                    return group;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            List<Group> groups = new List<Group>();
            foreach (Group group in _groupList)
            {
                if (group.Number == courseNumber)
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in _groupList)
            {
                if (group == student.Group)
                {
                    group.DeleteStudent(student);
                    newGroup.AddStudent(student);
                    return;
                }
            }

            throw new IsuException("There is no group with this name");
        }
    }
}