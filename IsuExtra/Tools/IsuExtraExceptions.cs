using System;

namespace IsuExtra.Tools
{
    public class IsuExtraExceptions : Exception
    {
        public IsuExtraExceptions()
        {
        }

        public IsuExtraExceptions(string message)
            : base(message)
        {
        }

        public IsuExtraExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}