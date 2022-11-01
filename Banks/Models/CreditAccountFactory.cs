using Banks.Interfaces;

namespace Banks.Models
{
    public class CreditAccountFactory : IBankAccountFactory
    {
        public IBankAccount CreateBankAccount(Client client, Bank bank, int? daysForDeposit)
        {
            return new CreditAccount(client, bank);
        }
    }
}