using Banks.Interfaces;

namespace Banks.Models
{
    public class Withdraw : IOperation
    {
        public Withdraw(double money, IBankAccount bankAccount)
        {
            Money = money;
            BankAccount = bankAccount;
        }

        public double Money { get; private set; }
        public IBankAccount BankAccount { get; private set; }
        public bool Execute()
        {
            return BankAccount.DecreaseMoney(Money);
        }

        public bool Cancel()
        {
            BankAccount.IncreaseMoney(Money);
            return true;
        }
    }
}