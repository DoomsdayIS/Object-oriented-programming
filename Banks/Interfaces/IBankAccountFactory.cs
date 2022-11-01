using Banks.Models;

namespace Banks.Interfaces
{
    public interface IBankAccountFactory
    {
        IBankAccount CreateBankAccount(Client client, Bank bank, int? daysForDeposit = null);
    }
}