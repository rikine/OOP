using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops.Details
{
    class Shop
    {
        public uint id { get; }
        public string name { get; }
        public string location { get; }
        private List<Shipment> shipments = new List<Shipment>();

        public Shop(uint id, string name, string location)
        {
            this.id = id;
            this.name = name;
            this.location = location;
        }

        public void Add(Product product, uint qty)
        {
            var shipment = shipments.Find(item => item.product.id == product.id);
            if (shipment == null) throw new AddProductException($"Error on adding a {product}: price isn't known");
            shipment.qty += qty;
        }

        public void Add(Product product, uint qty, double price)
        {
            var shipment = shipments.Find(item => item.product.id == product.id);

            if (shipment == null)
                shipments.Add(new Shipment(product, qty, price));
            else
            {
                shipment.qty += qty;
                shipment.price = price;
            }
        }

        public double Buy(Product product, uint qty)
        {
            var shipment = shipments.Find(item => item.product.id == product.id);

            if (shipment == null) throw new BuyProductException($"No such product {product} in shop {name} {id}");
            if (shipment.qty < qty) throw new BuyProductException($"Not enough {product} in shop {name} {id}");

            var totalPrice = qty * shipment.price;
            shipment.qty -= qty;

            return totalPrice;
        }

        public bool TryBuy(Product product, uint qty, out double totalPrice)
        {
            totalPrice = 0;
            try
            {
                totalPrice = Buy(product, qty);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public List<Product> GetShipments() => (from shipment in shipments
                                                select shipment.product).ToList();

        public double GetPrice(Product product)
        {
            var shipment = shipments.Find(item => item.product.id == product.id);
            if (shipment == null) throw new AvailableException($"No product {product} available.");
            return shipment.price;
        }

        public uint GetQty(Product product)
        {
            var shipment = shipments.Find(item => item.product.id == product.id);
            if (shipment == null) throw new AvailableException($"No product {product} available.");
            return shipment.qty;
        }

        public override string ToString()
        {
            var result = $"\nShop({id.ToString()})\nProducts in shop\nID\tName\tQuantity\tPrice\n";
            foreach (var prod in shipments)
            {
                result += $"{prod.product.id.ToString()}\t{prod.product.name}\t{prod.qty}\t{prod.price}\n";
            }
            return result;
        }
    }
}