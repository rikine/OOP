namespace Shops.Details
{
    class Shipment
    {
        public uint qty { get; set; }
        public double price { get; set; }
        public Product product { get; }

        public Shipment(Product product, uint qty, double price)
        {
            this.product = product;
            this.qty = qty;
            this.price = price;
        }
    }
}