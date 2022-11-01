using System;
using IsuExtra.Models;
using IsuExtra.Services;
using System.Linq;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class IsuExtraServiceTest
    {
        private IsuExtraService _isu;
        [SetUp]
        public void Setup()
        {
            _isu = new IsuExtraService();
        }
        
        [Test]
        public void AddCourse_CourseAddedSuccessfully()
        {
            Department f1 = Department.SchoolOfPhysicsAndEngineering;
            Course course1 = _isu.AddCourse("LasersTechnology", f1, 3, 2);
            Assert.AreEqual(3,course1.Streams.Count);
            Assert.AreEqual(2,course1.Streams[0].StudentLimit);
        }

        [Test]
        public void AddClassroomAndRegTeacherAndCreateLessonAddLessonToGroupAndStreamSchedule_AllCreatedSuccessfully()
        {
            Department f1 = Department.SchoolOfPhysicsAndEngineering;
            Course course1 = _isu.AddCourse("LasersTechnology", f1, 1, 2);
            Teacher teacher = _isu.RegisterTeacher("Oleg");
            Classroom classroom = _isu.AddClassroom(99);
            Lesson lesson = _isu.CreateLesson(DayOfWeek.Friday, "11:30", "12:00", classroom, teacher);
            Lesson lesson3 = _isu.CreateLesson(DayOfWeek.Friday, "12:30", "13:00", classroom, teacher);
            Group group = _isu.AddGroup("M3212");
            Group group2 = _isu.AddGroup("M3305");
            _isu.AddLessonToGroupSchedule(lesson,group);
            _isu.AddLessonToStreamSchedule(lesson3,course1.Streams[0]);
            Assert.AreEqual(lesson.Teacher, teacher);
            Assert.AreEqual(lesson.LessonAuditorium, classroom);
            Assert.IsTrue(group.Schedule.Contains(lesson));
            Assert.IsTrue(course1.Streams[0].Schedule.Contains(lesson3));
        }

        [Test]
        public void PickCourseForStudentCancelCoursePick_StudentAddedInCourseAndThenCancel()
        {
            Department f1 = Department.SchoolOfPhysicsAndEngineering;
            Course course1 = _isu.AddCourse("LasersTechnology", f1, 1, 2);
            Teacher teacher = _isu.RegisterTeacher("Oleg");
            Classroom classroom = _isu.AddClassroom(99);
            Lesson lesson = _isu.CreateLesson(DayOfWeek.Friday, "11:30", "12:00", classroom, teacher);
            Lesson lesson3 = _isu.CreateLesson(DayOfWeek.Friday, "12:30", "13:00", classroom, teacher);
            Group group = _isu.AddGroup("M3212");
            Group group2 = _isu.AddGroup("M3305");
            _isu.AddLessonToGroupSchedule(lesson,group);
            _isu.AddLessonToStreamSchedule(lesson3,course1.Streams[0]);
            Student student = _isu.AddStudent(group, "Igor");
            Student student2 = _isu.AddStudent(group, "Dimas");
            Student student3 = _isu.AddStudent(group, "Ya");
            Assert.IsTrue(_isu.PickCourseForStudent(course1,student));
            _isu.CancelCoursePick(course1,student);
            Assert.AreEqual(3,_isu.GetStudentsWithoutCourses(group).Count);
        }

        [Test]
        public void PickCourseForStudent_GotFalseBecauseOfGroupSchedule()
        {
            Department f1 = Department.SchoolOfPhysicsAndEngineering;
            Course course1 = _isu.AddCourse("LasersTechnology", f1, 1, 2);
            Teacher teacher = _isu.RegisterTeacher("Oleg");
            Classroom classroom = _isu.AddClassroom(99);
            Lesson lesson = _isu.CreateLesson(DayOfWeek.Friday, "11:30", "12:00", classroom, teacher);
            Lesson lesson3 = _isu.CreateLesson(DayOfWeek.Friday, "11:45", "13:00", classroom, teacher);
            Group group = _isu.AddGroup("M3212");
            Group group2 = _isu.AddGroup("M3305");
            _isu.AddLessonToGroupSchedule(lesson,group);
            _isu.AddLessonToStreamSchedule(lesson3,course1.Streams[0]);
            Student student = _isu.AddStudent(group, "Igor");
            Student student2 = _isu.AddStudent(group, "Dimas");
            Student student3 = _isu.AddStudent(group, "Ya");
            Assert.IsFalse(_isu.PickCourseForStudent(course1,student));
        }

        [Test]
        public void PickCourseForStudent_ThrowException()
        {
            Department f1 = Department.SchoolOfTranslationalInformationTechnologies;
            Course course1 = _isu.AddCourse("LasersTechnology", f1, 1, 2);
            Teacher teacher = _isu.RegisterTeacher("Oleg");
            Classroom classroom = _isu.AddClassroom(99);
            Lesson lesson = _isu.CreateLesson(DayOfWeek.Friday, "11:30", "12:00", classroom, teacher);
            Lesson lesson3 = _isu.CreateLesson(DayOfWeek.Friday, "11:45", "13:00", classroom, teacher);
            Group group = _isu.AddGroup("M3212");
            Group group2 = _isu.AddGroup("M3305");
            _isu.AddLessonToGroupSchedule(lesson,group);
            _isu.AddLessonToStreamSchedule(lesson3,course1.Streams[0]);
            Student student = _isu.AddStudent(group, "Igor");
            Student student2 = _isu.AddStudent(group, "Dimas");
            Student student3 = _isu.AddStudent(group, "Ya");
            Assert.Catch<IsuExtraExceptions>(() =>
            {
                _isu.PickCourseForStudent(course1,student);
            });
        }
    }
}