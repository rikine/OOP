struct Transaction
{
    public TypeOfTransaction Type { get; }
    public IAccount From { get; }
    public IAccount Where { get; }

    public double Amount { get; }

    public Transaction(IAccount from, IAccount where, TypeOfTransaction type, double amount)
    {
        From = from;
        Where = where;
        Type = type;
        Amount = amount;
    }

    public Transaction(IAccount from, TypeOfTransaction type, double amount)
    {
        From = from;
        Type = type;
        Amount = amount;
        Where = null;
    }
}