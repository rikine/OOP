class Sberbank : Bank
{
    protected override string _name { get; set; } = "Sberbank";
    protected override double _limit { get; set; } = 200_000;
    protected override double _limitWhileDoubt { get; set; } = 100_000;
    protected override double _percentOfBonus { get; set; } = 1.0;
    protected override double _commision { get; set; } = 7.5;

    protected override double GetPercentOfDeposit(double startMoney)
    {
        switch (startMoney)
        {
            case < 50_000:
                return 2.0;
            case < 100_000:
                return 2.5;
            default:
                return 3.0;
        }
    }
}