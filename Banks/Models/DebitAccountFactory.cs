using Banks.Interfaces;

namespace Banks.Models
{
    public class DebitAccountFactory : IBankAccountFactory
    {
        public IBankAccount CreateBankAccount(Client client, Bank bank, int? daysForDeposit)
        {
            return new DebitAccount(client, bank);
        }
    }
}