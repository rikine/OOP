namespace Shops.Details
{
    class Product
    {
        public uint id { get; }
        public string name { get; }

        public Product(uint id, string name) { this.id = id; this.name = name; }

        public override string ToString()
        {
            return $"{id.ToString()} {name}";
        }
    }
}