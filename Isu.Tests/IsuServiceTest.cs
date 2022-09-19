using System.Linq;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;
        private string _groupName = "M3205";
        private string _newGroupName = "M3212";
        private string _studentName = "Vanya";

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            _isuService.AddGroup(_groupName);
            _isuService.AddStudent(_isuService.FindGroup(_groupName), _studentName);

            
            Group actualGroup = _isuService.FindStudent(_studentName).Group;
            Group expectedGroup = _isuService.FindGroup(_groupName);
            
            Student actualStudent = actualGroup.Students.FirstOrDefault(x => x.Name == _studentName);
            Student expectedStudent = _isuService.FindStudent(_studentName);
            
            
            Assert.AreEqual(expectedGroup, actualGroup);
            Assert.IsTrue(actualStudent != null);
            Assert.AreEqual(expectedStudent, actualStudent);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(_groupName);
                Group group = _isuService.FindGroup(_groupName);
                for (int i = 0; i < 27; i++)
                {
                    _isuService.AddStudent(group, $"a{i}");
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("Nlrlr228");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            _isuService.AddGroup(_groupName);
            _isuService.AddStudent(_isuService.FindGroup(_groupName), _studentName);

            _isuService.AddGroup(_newGroupName);
            _isuService.ChangeStudentGroup(_isuService.FindStudent(_studentName), _isuService.FindGroup(_newGroupName));
                
            Group actualGroup = _isuService.FindStudent(_studentName).Group;
            Group expectedGroup = _isuService.FindGroup(_newGroupName);
            
            Student actualStudent = actualGroup.Students.FirstOrDefault(x => x.Name == _studentName);
            Student expectedStudent = _isuService.FindStudent(_studentName);
            
            
            Assert.AreEqual(expectedGroup, actualGroup);
            Assert.IsTrue(actualStudent != null);
            Assert.AreEqual(expectedStudent, actualStudent);
        }
    }
}