using System;

class CreditAccount : IAccount
{
    public int Id { get; }
    public Client Client { get; }
    private double _money;
    private double _commission;
    private DateTime _lastDoCommission;
    private double _limit;
    private double _limitWhileDoubt;

    public CreditAccount(Client client, double commission, double limit, double limitWhileDoubt, double money = 0)
    {
        Id = new Random().Next();
        Client = client;
        _commission = commission;
        _limit = limit;
        _money = money;
        _lastDoCommission = DateTime.Now;
        _limitWhileDoubt = limitWhileDoubt;
    }

    public void Add(double amount)
    {
        DoCommission();
        if (amount <= 0)
            throw new AddMoneyException($"Can't add lower or 0 money to account. Account id = {Id}");
        _money += amount;
        GlobalTransaction.AddTransaction(this, amount, TypeOfTransaction.Add);
    }

    public double CheckMoney()
    {
        DoCommission();
        return _money;
    }

    public void TakeOff(double amount)
    {
        DoCommission();
        if (Client.IsDoubtful() && _limitWhileDoubt < amount)
            throw new LimitDoubtException($"Your accout is under doubt. Your limit is {_limitWhileDoubt}");
        if (amount <= 0)
            throw new TakeOfException($"Can't take of lower or 0 money to account. Account id = {Id}");
        if (_money - amount < -_limit)
            throw new NotEnoughMoneyException($"Not enought money to take. Available = {_money}");
        _money -= amount;
        GlobalTransaction.AddTransaction(this, amount, TypeOfTransaction.TakeOf);
    }

    public void TransferMoney(IAccount where, double amount)
    {
        DoCommission();
        if (Client.IsDoubtful() && _limitWhileDoubt < amount)
            throw new LimitDoubtException($"Your accout is under doubt. Your limit is {_limitWhileDoubt}");
        if (where.Id == Id)
            throw new SelfTransferException($"Self-transfer. id = {Id}");
        if (_money - amount < -_limit)
            throw new NotEnoughMoneyException($"Not enought money to transfer. Available = {_money}");
        where.Add(amount);
        _money -= amount;
        GlobalTransaction.AddTransaction(this, where, amount, TypeOfTransaction.Transfer);
    }

    private void DoCommission()
    {
        if (_money >= 0)
            return;
        if (_lastDoCommission.Day >= DateTime.Now.Day)
            return;
        _money -= _money * _commission / 100.0;
        _lastDoCommission = DateTime.Now;
    }

}