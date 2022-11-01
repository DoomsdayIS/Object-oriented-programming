using System;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface IBankAccount
    {
        string Type { get; }
        int Id { get; }
        Client ClientAccOwner { get; }
        double AvailableMoney { get; }
        double PendingMoney { get; }
        void SkipSomeDays(int days);
        bool DecreaseMoney(double money);
        void IncreaseMoney(double money);
    }
}