using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shops.Models;
using Shops.Tools;
using Shops.Services;
using NUnit.Framework;

namespace Shops.Tests
{
    public class ShopsTests
    {
        public class Tests
    {

        private ShopService _shopService;
        private Customer _customer1;
        private Customer _customer2;
        private Shop _shop1;
        private Shop _shop2;
        private Product _product1;
        private Product _product2;
        private Product _product3;
        [SetUp]
        public void Setup()
        {
            _shopService = new ShopService();
            _customer1 = new Customer("Ivan", 322);
            _customer2 = new Customer("Prepod", 777);
            
        }

        [Test]
        public void CreateShopAddProducts()
        {
            _shop1 = _shopService.CreateShop("shop1","Moskow");
            _product1 = _shopService.CreateProduct("shokoladka");
            _product2 = _shopService.CreateProduct("balalayka");
            _shop1.AddProducts(new Dictionary<Product, (int count, int price)>
            {
                {_product1, (1, 50)},
                {_product2, (1, 500)}
            });
            
            Assert.AreEqual(_shop1.Products[_product1].count, 1);
            Assert.AreEqual(_shop1.Products[_product2].count, 1);
            Assert.AreEqual(_shop1.Products[_product1].price, 50);
            Assert.AreEqual(_shop1.Products[_product2].price, 500);
        }

        [Test]
        public void ChangePrice()
        {
            _shop1 = _shopService.CreateShop("shop1","Moskow");
            _product1 = _shopService.CreateProduct("shokoladka");
            _product2 = _shopService.CreateProduct("balalayka");
            _shop1.AddProducts(new Dictionary<Product, (int count, int price)>
            {
                {_product1, (1, 50)},
                {_product2, (1, 500)}
            });
            _shop1.ChangeProductPrice(_product1,200);
            _shop1.ChangeProductPrice(_product2,666);
            Assert.AreEqual(_shop1.Products[_product1].price, 200);
            Assert.AreEqual(_shop1.Products[_product2].price, 666);
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            _shop1 = _shopService.CreateShop("shop1","Moskow");
            _shop2 = _shopService.CreateShop("shop2","SPB");
            _product1 = _shopService.CreateProduct("shokoladka");
            _product3 = _shopService.CreateProduct("shokoladka");
            _product2 = _shopService.CreateProduct("balalayka");
            _shop1.AddProducts(new Dictionary<Product, (int count, int price)>
            {
                {_product1, (1, 50)},
                {_product2, (1, 500)}
            });
            _shop2.AddProducts(new Dictionary<Product, (int count, int price)>
            {
                {_product3, (1, 250)},
            });
            Shop shop3 = _shopService.FindBetterOffer(_customer2, new Dictionary<Product, int>
            {
                {_product1, 1}
            });
            Assert.AreEqual(shop3.Id,_shop1.Id);

        }

        [Test]
        public void PurshaceSomeProductsInTHeShop_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                _shop1 = _shopService.CreateShop("shop1","Moskow");
                _product1 = _shopService.CreateProduct("shokoladka");
                _product2 = _shopService.CreateProduct("balalayka");
                _shop1.AddProducts(new Dictionary<Product, (int count, int price)>
                {
                    {_product1, (1, 50)},
                    {_product2, (1, 500)}
                });
                _shop1.Buy(_customer1,new Dictionary<Product, int>
                {
                    {_product1, 3},
                    {_product2, 2}
                });
            });
        }
    }

    }
}