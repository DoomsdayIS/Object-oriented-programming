using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class Bank
    {
        public Bank(
            string name,
            double interestOnBalance,
            int creditCommission,
            int creditLimit,
            int unauthorizedLimit,
            int id,
            DateTime currentDay)
        {
            Name = name;
            InterestOnBalance = interestOnBalance;
            CreditCommission = creditCommission;
            CreditLimit = creditLimit;
            UnauthorizedLimit = unauthorizedLimit;
            Id = id + 1;
            CurrentDay = currentDay;
        }

        public List<IBankAccount> BankAccounts { get; } = new ();
        public string Name { get; private set; }
        public int Id { get; }
        public double InterestOnBalance { get; private set; }
        public int CreditCommission { get; private set; }
        public int CreditLimit { get; private set; }
        public int UnauthorizedLimit { get; private set; }
        public DateTime CurrentDay { get; private set; }

        public List<(double, int?)> DepositPercentPoint { get; private set; }
            = new List<(double, int?)> { (3, 50000), (3.5, 100000), (4, null) };

        public void AddDepositPercents(List<(double, int?)> deposit)
        {
            DepositPercentPoint = deposit;
        }

        public void SkipSomeDays(int days)
        {
            foreach (IBankAccount bankAccount in BankAccounts)
            {
                bankAccount.SkipSomeDays(days);
            }

            CurrentDay = CurrentDay.AddDays(days);
        }

        public IBankAccount OpenAccount(Client client, string accountType, int? dayOnDeposit)
        {
            IBankAccountFactory bankAccountFactory = AccountType(accountType);
            IBankAccount newBankAccount = bankAccountFactory.CreateBankAccount(client, this, dayOnDeposit);
            BankAccounts.Add(newBankAccount);
            return newBankAccount;
        }

        public void ChangeInterestOnBalance(double newInterest)
        {
            InterestOnBalance = newInterest;
            BankAccounts.Where(a => a.Type == "debit" && a.ClientAccOwner.Notification != null)
                .ToList().ForEach(a => a.ClientAccOwner.Notification.Notify(a.ClientAccOwner));
        }

        public void ChangeCreditCommission(int newCreditCommission)
        {
            CreditCommission = newCreditCommission;
            BankAccounts.Where(a => a.Type == "credit" && a.ClientAccOwner.Notification != null)
                .ToList().ForEach(a => a.ClientAccOwner.Notification.Notify(a.ClientAccOwner));
        }

        public void ChangeCreditLimit(int newCreditLimit)
        {
            CreditLimit = newCreditLimit;
            BankAccounts.Where(a => a.Type == "credit" && a.ClientAccOwner.Notification != null)
                .ToList().ForEach(a => a.ClientAccOwner.Notification.Notify(a.ClientAccOwner));
        }

        public void ChangeUnauthorizedLimit(int newUnauthorizedLimit)
        {
            UnauthorizedLimit = newUnauthorizedLimit;
            BankAccounts.Where(a => !a.ClientAccOwner.Confirmed && a.ClientAccOwner.Notification != null)
                .ToList().ForEach(a => a.ClientAccOwner.Notification.Notify(a.ClientAccOwner));
        }

        private static IBankAccountFactory AccountType(string accountType)
        {
            return accountType switch
            {
                "debit" => new DebitAccountFactory(),
                "credit" => new CreditAccountFactory(),
                "deposit" => new DepositAccountFactory(),
                _ => new DebitAccountFactory()
            };
        }
    }
}