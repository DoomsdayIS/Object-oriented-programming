using System;
using System.Text;
using Banks.Interfaces;

namespace Banks.Models
{
    public class DebitDecorator : Decorator
    {
        public DebitDecorator(IBankAccount bankAccount)
            : base(bankAccount)
        {
        }

        public override void SkipSomeDays(int days)
        {
            base.SkipSomeDays(days);
        }

        public override bool DecreaseMoney(double money)
        {
            bool value = base.DecreaseMoney(money);
            if (value)
            {
                Console.WriteLine($"Amount of Money on this account {this.Id} was decreased!" +
                                  $"Current Amount of Money: {this.AvailableMoney} \n");
            }

            return value;
        }

        public override void IncreaseMoney(double money)
        {
            base.IncreaseMoney(money);
            Console.WriteLine($"Amount of Money on this account {this.Id} was increased!" +
                              $"Current Amount of Money: {this.AvailableMoney} \n");
        }
    }
}