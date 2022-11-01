using System;
using System.Collections.Generic;
using IsuExtra.Models;
using IsuExtra.Services;

namespace IsuExtra
{
    internal class Program
    {
        private static void Main()
        {
            var isu = new IsuExtraService();
            Group group = isu.AddGroup("M3212");
            Group group2 = isu.AddGroup("M3305");
            Student student = isu.AddStudent(group, "Igor");
            Student student2 = isu.AddStudent(group, "Dimas");
            Student student3 = isu.AddStudent(group, "Ya");
            Console.WriteLine(isu.GetStudent(3).Name);
            Console.WriteLine(isu.FindStudent("Igor").Name);
            Department f1 = Department.SchoolOfPhysicsAndEngineering;
            Course course1 = isu.AddCourse("PIDORI", f1, 1, 2);
            Teacher teacher = isu.RegisterTeacher("tyanka");
            Classroom classroom = isu.AddClassroom(99);
            Lesson lesson = isu.CreateLesson(DayOfWeek.Friday, "11:30", "12:00", classroom, teacher);
            Lesson lesson2 = isu.CreateLesson(DayOfWeek.Friday, "11:30", "12:00", classroom, teacher);
            Lesson lesson3 = isu.CreateLesson(DayOfWeek.Friday, "12:30", "13:00", classroom, teacher);
            List<Student> students = isu.FindStudents("M3212");
            group = isu.FindGroup("M3212");
            var courseNumber = new CourseNumber(2);
            List<Student> students2 = isu.FindStudents(courseNumber);
            List<Group> groups = isu.FindGroups(courseNumber);
            isu.ChangeStudentGroup(student, group2);
            List<Student> students3 = isu.FindStudents(courseNumber);
            Console.WriteLine(student.Group.Name);
            isu.AddLessonToGroupSchedule(lesson, group);
            isu.AddLessonToStreamSchedule(lesson3, course1.Streams[0]);
            Console.WriteLine(isu.PickCourseForStudent(course1, student2));
            Console.WriteLine(group.Students.Count);
            Console.WriteLine(isu.GetStudentsWithoutCourses(group).Count);
            isu.CancelCoursePick(course1, student2);
            Console.WriteLine(isu.GetStudentsWithoutCourses(group).Count);
            Console.WriteLine(f1);
            string str = "M3213";
            if (!((str.Length == 5)
                && (str[0] >= 'A' && str[0] <= 'Z')
                && (str[1] == '3' || str[1] == '4')
                && (str[2] >= '1' && str[2] <= '4')
                && (str[3] >= '0' && str[3] <= '9')
                && (str[4] >= '0' && str[4] <= '9')))
            {
                Console.WriteLine("YES");
            }

            Group group3 = isu.AddGroup("A3305");
            Group group4 = isu.AddGroup("B3305");
            Console.WriteLine();
        }
    }
}