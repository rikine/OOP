using System;

class Deposit : IAccount
{
    public int Id { get; }
    public Client Client { get; }
    private double _money;
    private double _bonus;
    private double _percent;
    private double _limitWhileDoubt;
    private DateTime _lastDoBonus;
    private DateTime _dateOfEnd;
    private bool wasEnd;

    public Deposit(Client client, double percent, double money, DateTime dateOfEnd, double limitWhileDoubt)
    {
        Id = new Random().Next();
        Client = client;
        _percent = percent;
        _money = money;
        _dateOfEnd = dateOfEnd;
        _bonus = 0;
        _lastDoBonus = DateTime.Now;
        wasEnd = false;
        _limitWhileDoubt = limitWhileDoubt;
    }

    public void Add(double amount)
    {
        DoBonus();
        if (amount <= 0)
            throw new AddMoneyException($"Can't add lower or 0 money to account. Account id = {Id}");
        _money += amount;
        GlobalTransaction.AddTransaction(this, amount, TypeOfTransaction.Add);
    }

    public double CheckMoney()
    {
        DoBonus();
        return _money;
    }

    public void TakeOff(double amount)
    {
        DoBonus();
        if (Client.IsDoubtful() && _limitWhileDoubt < amount)
            throw new LimitDoubtException($"Your accout is under doubt. Your limit is {_limitWhileDoubt}");
        if (amount <= 0)
            throw new TakeOfException($"Can't take of lower or 0 money to account. Account id = {Id}");
        if (!wasEnd)
            throw new TakeOfException($"Can't take of money. Deposit doesn't end. Date of end = {_dateOfEnd}");
        if (_money - amount < 0)
            throw new NotEnoughMoneyException($"Not enought money to take. Available = {_money}");
        _money -= amount;
        GlobalTransaction.AddTransaction(this, amount, TypeOfTransaction.TakeOf);
    }

    public void TransferMoney(IAccount where, double amount)
    {
        DoBonus();
        if (Client.IsDoubtful() && _limitWhileDoubt < amount)
            throw new LimitDoubtException($"Your accout is under doubt. Your limit is {_limitWhileDoubt}");
        if (_money - amount < 0)
            throw new NotEnoughMoneyException($"Not enought money to transfer. Available = {_money}");
        if (!wasEnd)
            throw new TakeOfException($"Can't transfer money. Deposit doesn't end. Date of end = {_dateOfEnd}");
        if (where.Id == Id)
            throw new SelfTransferException($"Self-transfer. id = {Id}");
        where.Add(amount);
        _money -= amount;
        GlobalTransaction.AddTransaction(this, where, amount, TypeOfTransaction.Transfer);
    }

    private void DoBonus()
    {
        if (_lastDoBonus.Day >= DateTime.Now.Day)
            return;
        if (_lastDoBonus.Month < DateTime.Now.Month || (_lastDoBonus.Month == 12 && DateTime.Now.Month == 1))
        {
            _money += _bonus;
            _bonus = 0;
        }
        if (_dateOfEnd >= DateTime.Now)
        {
            if (wasEnd)
                return;
            _money += _bonus;
            _bonus = 0;
            wasEnd = true;
        }
        _bonus += (_money + _bonus) * _percent / 100.0 / 365.0;
        _lastDoBonus = DateTime.Now;
    }

}