using System.Collections.Generic;
using System.Collections.Immutable;

static class Banks
{
    private static List<IBank> banks = new List<IBank>();

    public static void AddBank(IBank bank) => banks.Add(bank);
    public static ImmutableList<IBank> GetAll() => banks.ToImmutableList();
}