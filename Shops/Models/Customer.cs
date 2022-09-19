using Shops.Services;
using Shops.Tools;

namespace Shops.Models
{
    public class Customer
    {
        public Customer(string name, int amountOfMoney = 0)
        {
            Name = name;
            AmountOfMoney = amountOfMoney;
        }

        public string Name { get; private set; }
        public int AmountOfMoney { get; private set; }
        public void IncreseMoneyCount(int money)
        {
            if (money > AmountOfMoney)
            {
                throw new ShopException($"Lol u don't have enough money, your order costs{money}");
            }
            else
            {
                AmountOfMoney -= money;
            }
        }
    }
}