using System;
using Shops;
using Shops.Details;
using System.Collections.Generic;

namespace OOP_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var sm = new ShopManager();
            sm.CreateProduct("Clock");
            sm.CreateProduct("Watch");
            sm.CreateProduct("Laptop");
            sm.CreateProduct("Ipad");
            sm.CreateProduct("Drive");
            sm.CreateProduct("Backpack");
            sm.CreateProduct("Cat");
            sm.CreateProduct("Dog");
            sm.CreateProduct("Cat");
            sm.CreateProduct("ChocoPie");

            sm.CreateShop("ReStore", "USA, California");
            sm.CreateShop("AnimalPlanet", "London");
            sm.CreateShop("MVideo", "Russia, Moscow");

            var shops = sm.GetShops();
            var products = sm.GetProducts();

            Console.WriteLine("\n1-2 Задание");
            Console.WriteLine(sm.ToString());

            Console.WriteLine("\n3 Задание");
            foreach (var shop in shops)
            {
                foreach (var prod in products)
                {
                    var price = new Random().Next(10, 90000);
                    if (price % 3 == 0 || price % 2 == 0)
                        sm.Add(shop, prod, price / 9, price);
                }
                Console.WriteLine(shop);
            }
            Console.WriteLine("\nДобавление без цены");
            foreach (var shop in shops)
            {
                foreach (var prod in products)
                {
                    var qty = new Random().Next(1, 50000);
                    if (qty % 3 == 0 || qty % 2 == 0)
                    {
                        try
                        {
                            sm.Add(shop, prod, qty);
                        }
                        catch (System.Exception)
                        {
                            continue;
                        }
                    }
                }
                Console.WriteLine(shop);
            }

            Console.WriteLine("\n4 Задание");
            Console.WriteLine("Ищем самый дешевый магазин с товаром ChocoPie");
            try
            {
                var shop1 = sm.ShopWithCheapestProduct(products[9]);
                Console.WriteLine(shop1);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\n5 Задание");
            Console.WriteLine("Покупаем в 1 магазине на 200к");
            var lstOfProducts = sm.MaxBuyByPrice(shops[0], 200000);
            Console.WriteLine("ID Name\tQuantity");
            foreach (var item in lstOfProducts)
            {
                Console.WriteLine($"{item.product}\t{item.qty}");
            }

            Console.WriteLine("\n6 Задание");
            Console.WriteLine("Пытаемся купить ChocoPie в кол-ве 2000 шт. из 3 магазина.\nМагазин до:");
            Console.WriteLine(shops[2]);
            if (sm.TryBuy(shops[2], products[9], 2000, out double totalPrice))
            {
                Console.WriteLine($"Успешно. Итоговая цена: {totalPrice}");
                Console.WriteLine(shops[2]);
            }
            else
            {
                Console.WriteLine("Невозможно купить.");
            }

            Console.WriteLine("\n7 Задание");
            Console.WriteLine("Ищем магазин с самым дешевым набором: 3000 ChocoPie, 1500 Clock и 15 Dog");
            var listOfProducts = new List<(Product product, int qty)>();
            listOfProducts.Add((products[9], 3000));
            listOfProducts.Add((products[0], 1500));
            listOfProducts.Add((products[7], 15));
            try
            {
                var shop2 = sm.CheapestShopForShipment(listOfProducts);
                Console.WriteLine(shop2);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
