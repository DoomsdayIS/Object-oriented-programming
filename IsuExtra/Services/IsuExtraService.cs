using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IsuExtra.Models;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class IsuExtraService
    {
        private readonly List<Classroom> _classrooms = new ();
        private readonly List<Course> _courseList = new ();
        private readonly List<Group> _groupList = new ();
        private readonly List<Teacher> _teachers = new ();

        public Group AddGroup(string name)
        {
            var group = new Group(name);
            _groupList.Add(group);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            var student = new Student(name);
            group.AddStudent(student);
            return student;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _groupList)
            {
                Student student = group.GetStudent(id);
                if (student != null) return student;
            }

            throw new IsuExtraExceptions("There is no student with this id");
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
                    return group.Students.Select(s => s as Student).ToList();
                }
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var courseStudents = new List<Student>();
            foreach (Group group in _groupList)
            {
                if (group.Number.Course == courseNumber.Course)
                {
                    courseStudents.AddRange(group.Students.Select(s => s as Student).ToList());
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
            var groups = new List<Group>();
            foreach (Group group in _groupList)
            {
                if (group.Number.Course == courseNumber.Course)
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

            throw new IsuExtraExceptions("There is no group with this name");
        }

        public Course AddCourse(string name, Department department, int streamsCount = 1, int studentStreamLimit = 25)
        {
            var course = new Course(name, department, streamsCount, studentStreamLimit);
            _courseList.Add(course);

            return course;
        }

        public Classroom AddClassroom(int classroomNumber)
        {
            var classroom = new Classroom(classroomNumber);
            _classrooms.Add(classroom);
            return classroom;
        }

        public Teacher RegisterTeacher(string teacherName)
        {
            var teacher = new Teacher(teacherName);
            _teachers.Add(teacher);
            return teacher;
        }

        public Lesson CreateLesson(
            DayOfWeek weekDay,
            string startTimeString,
            string endTimeString,
            Classroom classroom,
            Teacher teacher)
        {
            if (!DateTime.TryParse(
                startTimeString,
                new CultureInfo("ru-RU"),
                DateTimeStyles.NoCurrentDateDefault,
                out DateTime startTime))
                throw new IsuExtraExceptions("Incorrect lesson start time format!");
            if (!DateTime.TryParse(
                endTimeString,
                new CultureInfo("ru-RU"),
                DateTimeStyles.NoCurrentDateDefault,
                out DateTime endTime))
                throw new IsuExtraExceptions("Incorrect lesson end time format!");
            var lesson = new Lesson(weekDay, startTime, endTime, classroom, teacher);
            return lesson;
        }

        public void AddLessonToGroupSchedule(Lesson lesson, Group group)
        {
            group.AddLesson(lesson);
        }

        public void AddLessonToStreamSchedule(Lesson lesson, Stream stream)
        {
            stream.AddLesson(lesson);
        }

        public bool PickCourseForStudent(Course course, Student student)
        {
            return course.AddStudent(student);
        }

        public List<Student> GetStudentsWithoutCourses(Group group)
        {
            return group.GetStudentWithoutCourses();
        }

        public void CancelCoursePick(Course course, Student student)
        {
            course.CancelCoursePick(student);
        }
    }
}