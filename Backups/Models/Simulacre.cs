namespace Backups.Models
{
    public class Simulacre : IObjectable
    {
        public Simulacre(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}