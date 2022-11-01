using System;
using System.Runtime.ConstrainedExecution;
using Banks.Models;
using Banks.Tools;

namespace Banks.Interfaces
{
    public abstract class Decorator : IBankAccount
    {
        private IBankAccount _bankAccount;

        public Decorator(IBankAccount bankAccount)
        {
            this._bankAccount = bankAccount;
        }

        public string Type => _bankAccount.Type;
        public int Id => _bankAccount.Id;
        public Client ClientAccOwner => _bankAccount.ClientAccOwner;

        public double AvailableMoney => _bankAccount.AvailableMoney;
        public double PendingMoney => _bankAccount.PendingMoney;
        public void SetBankAccount(IBankAccount bankAccount)
        {
            this._bankAccount = bankAccount;
        }

        public virtual void SkipSomeDays(int days)
        {
            this._bankAccount?.SkipSomeDays(days);
        }

        public virtual bool DecreaseMoney(double money)
        {
            return this._bankAccount?.DecreaseMoney(money) ?? false;
        }

        public virtual void IncreaseMoney(double money)
        {
            this._bankAccount?.IncreaseMoney(money);
        }
    }
}