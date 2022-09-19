using Shops.Services;
using Shops.Tools;

namespace Shops.Models
{
    public class Product
    {
        public Product(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}