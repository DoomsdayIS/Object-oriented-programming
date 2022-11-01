using System;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class DebitAccount : IBankAccount
    {
        public DebitAccount(Client client, Bank bank)
        {
            ClientAccOwner = client;
            BankAccOwner = bank;
            CreationDate = bank.CurrentDay;
            Id = bank.BankAccounts.LastOrDefault()?.Id + 1 ?? 1;
        }

        public DateTime CreationDate { get; private set; }
        public string Type => "debit";
        public int Id { get; private set; }
        public Client ClientAccOwner { get; private set; }
        public Bank BankAccOwner { get; private set; }

        public double AvailableMoney { get; private set; } = 0;
        public double PendingMoney { get; private set; } = 0;

        public bool DecreaseMoney(double money)
        {
            if (money > AvailableMoney)
            {
                return false;
            }

            if (!ClientAccOwner.Confirmed && money > BankAccOwner.UnauthorizedLimit)
            {
                return false;
            }

            AvailableMoney -= money;
            return true;
        }

        public void IncreaseMoney(double money)
        {
            AvailableMoney += money;
        }

        public void SkipSomeDays(int days)
        {
            DateTime startDate = BankAccOwner.CurrentDay;
            for (int i = 0; i < days; i++)
            {
                if (startDate.Day == CreationDate.Day)
                {
                    AvailableMoney += PendingMoney;
                    PendingMoney = 0;
                }

                PendingMoney += AvailableMoney * ((BankAccOwner.InterestOnBalance / 365) / 100);
                startDate = startDate.AddDays(1);
            }
        }
    }
}