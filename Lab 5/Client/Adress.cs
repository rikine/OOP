struct Adress
{
    public string Country { get; }
    public string Region { get; }
    public string City { get; }
    public int House { get; }

    public Adress(string country, string region, string city, int house)
    {
        Country = country;
        Region = region;
        City = city;
        House = house;
    }
}