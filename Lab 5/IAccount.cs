interface IAccount
{
    int Id { get; }
    Client Client { get; }
    void TakeOff(double amount);
    void Add(double amount);
    void TransferMoney(IAccount where, double amount);
    double CheckMoney();
}