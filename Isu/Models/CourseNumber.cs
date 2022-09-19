using System.Dynamic;
using Isu.Tools;

namespace Isu.Models
{
    public class CourseNumber
    {
        private int _course;
        public CourseNumber(int course)
        {
            Course = course;
        }

        public int Course
        {
            get => _course;
            private set
            {
                if (value is >= 5 or < 1)
                {
                    throw new IsuException("Incorrect course number");
                }
                else
                {
                    _course = value;
                }
            }
        }
    }
}