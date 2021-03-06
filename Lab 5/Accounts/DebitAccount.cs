using System;

class DebitAccount : IAccount
{
    public int Id { get; }
    public Client Client { get; }
    private double _money;
    private double _bonus;
    private double _percent;
    private DateTime _lastDoBonus;
    private double _limitWhileDoubt;
    private DateTime time;


    public DebitAccount(Client client, double percent, double limitWhileDoubt, double money = 0)
    {
        Id = new Random().Next();
        Client = client;
        _percent = percent;
        _money = money;
        _bonus = 0;
        _lastDoBonus = DateTime.Now;
        _limitWhileDoubt = limitWhileDoubt;
        time = DateTime.Now.AddDays(1);
    }

    public void Add(double amount)
    {
        DoBonus(time = time.AddDays(1));
        if (amount <= 0)
            throw new AddMoneyException($"Can't add lower or 0 money to account. Account id = {Id}");
        _money += amount;
        GlobalTransaction.AddTransaction(this, amount, TypeOfTransaction.Add);
    }

    public void TakeOff(double amount)
    {
        DoBonus(time = time.AddDays(1));
        if (Client.IsDoubtful() && _limitWhileDoubt < amount)
            throw new LimitDoubtException($"Your accout is under doubt. Your limit is {_limitWhileDoubt}");
        if (amount <= 0)
            throw new TakeOfException($"Can't take of lower or 0 money to account. Account id = {Id}");
        if (_money - amount < 0)
            throw new NotEnoughMoneyException($"Not enought money to take. Available = {_money}");
        _money -= amount;
        GlobalTransaction.AddTransaction(this, amount, TypeOfTransaction.TakeOf);
    }

    public void TransferMoney(IAccount where, double amount)
    {
        DoBonus(time = time.AddDays(1));
        if (Client.IsDoubtful() && _limitWhileDoubt < amount)
            throw new LimitDoubtException($"Your accout is under doubt. Your limit is {_limitWhileDoubt}");
        if (where.Id == Id)
            throw new SelfTransferException($"Self-transfer. id = {Id}");
        if (_money - amount < 0)
            throw new NotEnoughMoneyException($"Not enought money to transfer. Available = {_money}");
        where.Add(amount);
        _money -= amount;
        GlobalTransaction.AddTransaction(this, where, amount, TypeOfTransaction.Transfer);
    }

    public double CheckMoney()
    {
        DoBonus(time = time.AddDays(1));
        return _money;
    }

    private void DoBonus(DateTime dateTime)
    {
        // if (_lastDoBonus.Day == DateTime.Now.Day)
        if (_lastDoBonus.Day == dateTime.Day)
            return;
        // if (_lastDoBonus.Month < DateTime.Now.Month || (_lastDoBonus.Month == 12 && DateTime.Now.Month == 1))
        if (_lastDoBonus.Month < dateTime.Month || (_lastDoBonus.Month == 12 && dateTime.Month == 1))
        {
            _money += _bonus;
            _bonus = 0;
        }
        _bonus += (_money + _bonus) * _percent / 100.0 / 365.0;
        // _lastDoBonus = DateTime.Now;
        _lastDoBonus = dateTime;
    }

}
