using System.Collections.Generic;

static class GlobalTransaction
{
    private static List<Transaction> transactions = new List<Transaction>();

    public static void AddTransaction(IAccount from, double amount, TypeOfTransaction type)
    {
        transactions.Add(new Transaction(from, type, amount));
    }

    public static void AddTransaction(IAccount from, IAccount where, double amount, TypeOfTransaction type)
    {
        transactions.Add(new Transaction(from, where, type, amount));
    }

    public static void CancelTransaction(IAccount from, double amount, TypeOfTransaction type)
    {
        var isTransactionInList = transactions.Exists(trans => trans.From.Id == from.Id && trans.Amount == amount && trans.Type == type);
        if (!isTransactionInList)
            throw new NoTransactionWhereFoundException($"No transaction where found");
        var TransactionInList = transactions.Find(trans => trans.From.Id == from.Id && trans.Amount == amount && trans.Type == type);
        if (TransactionInList.Type == TypeOfTransaction.TakeOf)
        {
            from.Add(amount);
            transactions.Remove(TransactionInList);
        }
        else if (TransactionInList.Type == TypeOfTransaction.Add)
        {
            from.TakeOff(amount);
            transactions.Remove(TransactionInList);
        }
        else
        {
            throw new CancelTransactionException($"Error with cancel transaction");
        }
    }

    public static void CancelTransaction(IAccount from, IAccount where, double amount, TypeOfTransaction type)
    {
        var isTransactionInList = transactions.Exists(trans => trans.From.Id == from.Id && trans.Amount == amount && trans.Type == type);
        if (!isTransactionInList)
            throw new NoTransactionWhereFoundException($"No transaction where found");
        var TransactionInList = transactions.Find(trans => trans.From.Id == from.Id && trans.Amount == amount && trans.Type == type);
        if (TransactionInList.Type == TypeOfTransaction.Transfer)
        {
            where.TakeOff(amount);
            from.TakeOff(amount);
            transactions.Remove(TransactionInList);
        }
        else
        {
            throw new CancelTransactionException($"Error with cancel transaction");
        }
    }
}