using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Models
{
    public class CentralBank
    {
        private static CentralBank _instance;
        private CentralBank() { }

        public List<Bank> Banks { get; } = new ();

        public List<IOperation> Operations { get; } = new ();

        public DateTime CurrentDay { get; private set; } = DateTime.Today;
        public static CentralBank GetInstance()
        {
            return _instance ??= new CentralBank();
        }

        public bool DoOperation(IOperation operation)
        {
            bool value = operation.Execute();
            if (value)
            {
                Operations.Add(operation);
            }

            return value;
        }

        public bool CancelOperation(IOperation operation)
        {
            bool value = Operations.FirstOrDefault(o => o == operation)?.Cancel() ?? false;
            if (value)
            {
                Operations.Remove(operation);
            }

            return value;
        }

        public void SkipSomeDays(int days)
        {
            if (days < 0)
            {
                throw new BanksException("U can't back to the past!");
            }

            foreach (Bank bank in Banks)
            {
                bank.SkipSomeDays(days);
            }

            CurrentDay = CurrentDay.AddDays(days);
        }

        public Bank CreateBank(
            string name,
            double interestOnBalance,
            int creditCommission,
            int creditLimit,
            int unauthorizedLimit)
        {
            var bank = new Bank(
                name,
                interestOnBalance,
                creditCommission,
                creditLimit,
                unauthorizedLimit,
                Banks.LastOrDefault()?.Id ?? 0,
                CurrentDay);
            Banks.Add(bank);
            return bank;
        }
    }
}