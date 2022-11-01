using Banks.Interfaces;

namespace Banks.Models
{
    public class Replenish : IOperation
    {
        public Replenish(double money, IBankAccount bankAccount)
        {
            Money = money;
            BankAccount = bankAccount;
        }

        public double Money { get; private set; }
        public IBankAccount BankAccount { get; private set; }
        public bool Execute()
        {
            BankAccount.IncreaseMoney(Money);
            return true;
        }

        public bool Cancel()
        {
            return BankAccount.DecreaseMoney(Money);
        }
    }
}