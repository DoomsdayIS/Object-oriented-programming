namespace Banks.Interfaces
{
    public interface IOperation
    {
        bool Execute();
        bool Cancel();
    }
}