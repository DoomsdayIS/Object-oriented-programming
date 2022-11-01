using System.Collections.Generic;

namespace Banks.Interfaces
{
    public interface IBuilder
    {
        void Reset();
        void BuildStep1(string s1);
        void BuildStep2(string s2);
        void BuildStep3(string s3);
    }
}