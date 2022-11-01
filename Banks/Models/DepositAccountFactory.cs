using Banks.Interfaces;

namespace Banks.Models
{
    public class DepositAccountFactory : IBankAccountFactory
    {
        public IBankAccount CreateBankAccount(Client client, Bank bank, int? daysForDeposit)
        {
            return new DepositAccount(client, bank, daysForDeposit);
        }
    }
}