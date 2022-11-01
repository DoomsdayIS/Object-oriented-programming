using System;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class CreditAccount : IBankAccount
    {
        public CreditAccount(Client client, Bank bank)
        {
            ClientAccOwner = client;
            BankAccOwner = bank;
            CreationDate = bank.CurrentDay;
            Id = bank.BankAccounts.LastOrDefault()?.Id + 1 ?? 1;
        }

        public DateTime CreationDate { get; private set; }
        public string Type => "credit";
        public int Id { get; private set; }
        public Client ClientAccOwner { get; private set; }
        public Bank BankAccOwner { get; private set; }

        public double AvailableMoney { get; private set; } = 0;
        public double PendingMoney { get; } = 0;

        public bool DecreaseMoney(double money)
        {
            if (AvailableMoney - money < -BankAccOwner.CreditLimit)
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
                if (startDate.Day == CreationDate.Day && AvailableMoney < 0)
                {
                    AvailableMoney -= BankAccOwner.CreditCommission;
                }

                startDate = startDate.AddDays(1);
            }
        }
    }
}