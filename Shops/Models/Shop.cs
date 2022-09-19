using System.Collections.Generic;
using System.Linq;
using Shops.Services;
using Shops.Tools;

namespace Shops.Models
{
    public class Shop
    {
        private static int _lastId = 1;

        public Shop(string name, string address)
        {
            Name = name;
            Address = address;
            Id = _lastId;
            _lastId++;
        }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public string Address { get; private set; }

        public Dictionary<Product, (int count, int price)> Products { get; private set; } = new ();

        public void AddProducts(Dictionary<Product, (int count, int price)> newProducts)
        {
            foreach (var supply in newProducts)
            {
                var existingProduct = Products
                    .FirstOrDefault(p => p.Key.Name == supply.Key.Name);
                if (existingProduct.Key == null)
                {
                    Products.Add(supply.Key, supply.Value);
                }
                else
                {
                    Products[existingProduct.Key] =
                        (supply.Value.count + existingProduct.Value.count, existingProduct.Value.price);
                }
            }
        }

        public void ChangeProductPrice(Product product, int price)
        {
            var existingProduct = Products
                .FirstOrDefault(p => p.Key.Name == product.Name);
            Products[existingProduct.Key] =
                (existingProduct.Value.count, price);
        }

        public void Buy(Customer customer, Dictionary<Product, int> order)
        {
            int orderSum = 0;
            foreach (var product in order)
            {
                var existingProduct = Products
                    .FirstOrDefault(p => p.Key.Name == product.Key.Name);
                if (existingProduct.Key == null)
                {
                    throw new ShopException($"There is not {product.Key.Name} in this shop");
                }
                else if (existingProduct.Value.count < product.Value)
                {
                    throw new ShopException($"There is not enough {product.Key.Name} in this shop");
                }
                else
                {
                    orderSum += existingProduct.Value.price * product.Value;
                    Products[existingProduct.Key] =
                        (existingProduct.Value.count - product.Value, existingProduct.Value.price);
                }
            }

            customer.IncreseMoneyCount(orderSum);
        }

        public int? CheckOrderPrice(Customer customer, Dictionary<Product, int> order)
        {
            int orderPrice = 0;
            foreach (var product in order)
            {
                var existingProduct = Products
                    .FirstOrDefault(p => p.Key.Name == product.Key.Name);
                if (existingProduct.Key == null)
                {
                    return null;
                }
                else if (existingProduct.Value.count < product.Value)
                {
                    return null;
                }
                else
                {
                    orderPrice += existingProduct.Value.price * product.Value;
                }
            }

            if (customer.AmountOfMoney < orderPrice)
            {
                return null;
            }
            else
            {
                return orderPrice;
            }
        }
    }
}