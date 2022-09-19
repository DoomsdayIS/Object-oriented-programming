using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Shops.Models;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopService
    {
        public List<Shop> Shops { get; private set; } = new ();

        public Shop CreateShop(string name, string address)
        {
            var shop = new Shop(name, address);
            Shops.Add(shop);
            return shop;
        }

        public Product CreateProduct(string name)
        {
            return new Product(name);
        }

        public Shop FindBetterOffer(Customer customer, Dictionary<Product, int> order)
        {
            return Shops.Where(s => s.CheckOrderPrice(customer, order) != null)
                .OrderBy(s => s.CheckOrderPrice(customer, order)).ToList().FirstOrDefault();
        }
    }
}