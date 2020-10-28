using System;
using System.Collections.Generic;
using Shops.Details;

namespace Shops
{
    class ShopManager
    {
        private List<Product> products = new List<Product>();
        private List<Shop> shops = new List<Shop>();

        public void CreateShop(string name, string location)
        {
            shops.Add(new Shop((uint)shops.Count + 1, name, location));
        }

        public void CreateProduct(string name)
        {
            products.Add(new Product((uint)products.Count + 1, name));
        }

        public void Add(Shop shop, Product product, int qty, double price = -100.0)
        {
            if (shops.Find(x => x.id == shop.id) == null) throw new AddProductException($"Can't add a {product}. {shop.name} {shop.id} doesn't exists.");
            if (products.Find(x => x.id == product.id) == null) throw new AddProductException($"Can't add a {product} because it doesn't exist.");

            if (qty < 1) throw new AddProductException($"Can't add a product with < 1 quantity. Your quantity is {qty}");
            if (price == -100.0)
                shops.Find(x => x.id == shop.id).Add(products.Find(item => item.id == product.id), (uint)qty);
            else
            {
                if (price < 0)
                    throw new AddProductException($"Price can't be below 0.");
                shops.Find(x => x.id == shop.id).Add(products.Find(item => item.id == product.id), (uint)qty, price);
            }
        }

        public bool TryBuy(Shop shop, Product product, int qty, out double totalPrice)
        {
            totalPrice = 0;
            if (shops.Find(item => item.id == shop.id) == null) return false;
            if (products.Find(item => item.id == product.id) == null) return false;

            if (!shops.Find(x => x.id == shop.id).TryBuy(products.Find(item => item.id == product.id), (uint)qty, out double price))
                return false;
            totalPrice = price;
            return true;
        }

        public Shop ShopWithCheapestProduct(Product product)
        {
            double minPrice = double.MaxValue;
            Shop cheapShop = null;

            foreach (var shop in shops)
            {
                if (shop.GetShipments().Find(item => item.id == product.id) != null
                    && shop.GetPrice(product) < minPrice)
                {
                    minPrice = shop.GetPrice(product);
                    cheapShop = shop;
                }
            }

            if (cheapShop == null)
                throw new CheapestShopException($"No product {product} in all shops");
            return cheapShop;
        }

        public List<(Product product, int qty)> MaxBuyByPrice(Shop shop, double maxPrice)
        {
            if (shops.Find(item => item.id == shop.id) == null) throw new AddProductException($"Can't find max buys in {shop.name} {shop.id} because it doesn't exists.");

            var buys = new List<(Product product, int qty)>();
            foreach (var prod in shops.Find(item => item.id == shop.id).GetShipments())
            {
                var price = shop.GetPrice(prod);
                var qty = shop.GetQty(prod);
                var enough = (int)Math.Min((double)qty, Math.Floor(maxPrice / price));
                if (enough >= 1)
                    buys.Add((prod, enough));
            }
            return buys;
        }

        public Shop CheapestShopForShipment(List<(Product product, int qty)> listOfProducts)
        {
            Shop cheapShop = null;
            double cheapPrice = double.MaxValue;

            foreach (var shop in shops)
            {
                double shopPrice = 0.0;
                bool ready = true;
                foreach (var prod in listOfProducts)
                {
                    if (shop.GetShipments().Find(item => item.id == prod.product.id) == null)
                    {
                        ready = false;
                        break;
                    }
                    var prodPrice = shop.GetPrice(prod.product);
                    var prodQty = shop.GetQty(prod.product);
                    if (prodQty < prod.qty)
                    {
                        ready = false;
                        break;
                    }
                    shopPrice += prodPrice * prod.qty;
                }
                if (ready && shopPrice < cheapPrice)
                {
                    cheapPrice = shopPrice;
                    cheapShop = shop;
                }
            }
            if (cheapShop == null)
                throw new AvailableException("No neccesary products in shops");
            return cheapShop;
        }

        public List<Shop> GetShops() { return shops; }

        public List<Product> GetProducts() { return products; }

        public override string ToString()
        {
            var result = "Products\nID\tName\n";
            foreach (var prod in products)
            {
                result += $"{prod.id.ToString()}\t{prod.name}\n";
            }
            result += "Shops\nID\tName\tLocation\n";
            foreach (var shop in shops)
            {
                result += $"{shop.id.ToString()}\t{shop.name}\t{shop.location}\n";
            }
            foreach (var shop in shops)
            {
                result += shop.ToString();
            }
            return result;
        }
    }
}