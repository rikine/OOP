class Client
{
    public string Name { get; }
    public string Surname { get; }
    private Adress? _adress;
    private int? _passportNumber;

    public Client(string name, string surname)
    {
        Name = name;
        Surname = surname;
    }

    public void SetAdress(Adress adress) => _adress = adress;

    public Adress GetAdress() => _adress.GetValueOrDefault();

    public void SetPassportNumber(int passportNumber) => _passportNumber = passportNumber;

    public int GetPassportNumber() => _passportNumber.GetValueOrDefault();

    public bool IsDoubtful() => _adress.HasValue ? _passportNumber.HasValue ? true : false : false;
}