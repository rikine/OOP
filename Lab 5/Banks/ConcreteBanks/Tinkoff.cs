class Tinkoff : Bank
{
    protected override string _name { get; set; } = "Tinkoff";
    protected override double _limit { get; set; } = 100_000;
    protected override double _limitWhileDoubt { get; set; } = 50_000;
    protected override double _percentOfBonus { get; set; } = 1.5;
    protected override double _commision { get; set; } = 5;

    protected override double GetPercentOfDeposit(double startMoney)
    {
        switch (startMoney)
        {
            case < 50_000:
                return 3.0;
            case < 100_000:
                return 3.5;
            default:
                return 4.0;
        }
    }
}