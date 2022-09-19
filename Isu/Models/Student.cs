namespace Isu.Models
{
    public class Student
    {
        private static int _lastId = 1;
        public Student(string name)
        {
            Name = name;
            Id = _lastId;
            _lastId++;
        }

        public string Name { get; private set; }
        public int Id { get; private set; }
        public Group Group { get; private set; }

        public void ChangeGroup(Group group)
        {
            Group = group;
        }
    }
}