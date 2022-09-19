## Object-oriented-programming
# Лабораторная 0. Isu 
**Цель:** ознакомиться с языком C#, базовыми механизмами ООП. В шаблонном репозитории описаны базовые сущности, требуется реализовать недостающие методы и написать тесты, которые бы проверили корректность работы.

**Предметная область.** Студенты, группы, переводы (хоть где-то), поиск. Группа имеет название (соответсвует шаблону M3XYY, где X - номер курса, а YY - номер группы). Студент может находиться только в одной группе. Система должна поддерживать механизм перевода между группами, добавления в группу и удаление из группы.

Требуется реализовать предоставленный в шаблоне интерфейс:

```csharp
public interface IIsuService
{
    Group AddGroup(GroupName name);
    Student AddStudent(Group group, string name);

    Student GetStudent(int id);
    Student FindStudent(string name);
    List<Student> FindStudents(GroupName groupName);
    List<Student> FindStudents(CourseNumber courseNumber);

    Group FindGroup(GroupName groupName);
    List<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}
```

И протестировать написанный код:

```csharp
[Test]
public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
{
}

[Test]
public void ReachMaxStudentPerGroup_ThrowException()
{
}

[Test]
public void CreateGroupWithInvalidName_ThrowException()
{
}

[Test]
public void TransferStudentToAnotherGroup_GroupChanged()
{
}
```
[Лабораторная работа 0.Isu (реализация)](Isu/)

[Лабораторная работа 0.Isu (тесты)](Isu.Tests/)
