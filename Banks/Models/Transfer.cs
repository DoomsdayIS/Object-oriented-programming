using Banks.Interfaces;

namespace Banks.Models
{
    public class Transfer : IOperation
    {
        public Transfer(double money, IBankAccount bankAccountFrom, IBankAccount bankAccountTo)
        {
            Money = money;
            BankAccountFrom = bankAccountFrom;
            BankAccountTo = bankAccountTo;
        }

        public double Money { get; private set; }
        public IBankAccount BankAccountFrom { get; private set; }
        public IBankAccount BankAccountTo { get; private set; }
        public bool Execute()
        {
            bool value = BankAccountFrom.DecreaseMoney(Money);
            if (value)
            {
                BankAccountTo.IncreaseMoney(Money);
            }

            return value;
        }

        public bool Cancel()
        {
            bool value = BankAccountTo.DecreaseMoney(Money);
            if (value)
            {
                BankAccountFrom.IncreaseMoney(Money);
            }

            return value;
        }
    }
}