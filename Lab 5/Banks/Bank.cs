#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

abstract class Bank : IBank
{
    protected abstract string _name { get; set; }
    private List<Client> _clients = new List<Client>();
    private List<IAccount> _accounts = new List<IAccount>();
    protected abstract double _limit { get; set; }
    protected abstract double _limitWhileDoubt { get; set; }
    protected abstract double _percentOfBonus { get; set; }
    protected abstract double _commision { get; set; }

    public Bank()
    {
        Banks.AddBank(this);
    }

    public void AddMoney(IAccount where, double amount)
    {
        var accountInList = _accounts.Find(acc => acc.Id == where.Id);
        if (accountInList == null)
            throw new InnerAccountOrClientException($"Account with id = {where.Id} was not found in bank {_name}");
        accountInList.Add(amount);
    }

    public void AddNewClient(Client client)
    {
        var clientInList = _clients.Find(cli => cli == client);
        if (clientInList != null)
            throw new InnerAccountOrClientException($"Client with name = {client.Name} was found in bank {_name}");
        _clients.Add(client);
    }

    public double GetAvailableMoney(IAccount where)
    {
        var accountInList = _accounts.Find(acc => acc.Id == where.Id);
        if (accountInList == null)
            throw new InnerAccountOrClientException($"Account with id = {where.Id} was not found in bank {_name}");
        return accountInList.CheckMoney();
    }

    public ImmutableList<Client> GetClients() => _clients.ToImmutableList();

    public IAccount OpenNewAccountDeposit(Client client, DateTime timeOfEnd, double startMoney)
    {
        var clientInList = _clients.Find(cli => cli == client);
        if (clientInList == null)
            throw new InnerAccountOrClientException($"Client with name = {client.Name} was not found in bank {_name}");
        _accounts.Add(new Deposit(clientInList, GetPercentOfDeposit(startMoney), startMoney, timeOfEnd, _limitWhileDoubt));
        return _accounts.Last();
    }

    protected abstract double GetPercentOfDeposit(double startMoney);

    public IAccount OpenNewAccountDebitAccount(Client client, double startMoney = 0)
    {
        var clientInList = _clients.Find(cli => cli == client);
        if (clientInList == null)
            throw new InnerAccountOrClientException($"Client with name = {client.Name} was not found in bank {_name}");
        _accounts.Add(new DebitAccount(clientInList, _percentOfBonus, _limitWhileDoubt, startMoney));
        return _accounts.Last();
    }

    public IAccount OpenNewAccountCreditAccount(Client client, double startMoney = 0)
    {
        var clientInList = _clients.Find(cli => cli == client);
        if (clientInList == null)
            throw new InnerAccountOrClientException($"Client with name = {client.Name} was not found in bank {_name}");
        _accounts.Add(new CreditAccount(clientInList, _commision, _limit, _limitWhileDoubt, startMoney));
        return _accounts.Last();
    }

    public void SetAdress(Client client, Adress adress)
    {
        var clientInList = _clients.Find(cli => cli == client);
        if (clientInList == null)
            throw new InnerAccountOrClientException($"Client with name = {client.Name} was not found in bank {_name}");
        clientInList.SetAdress(adress);
    }

    public void SetPassportNumber(Client client, int passportNumber)
    {
        var clientInList = _clients.Find(cli => cli == client);
        if (clientInList == null)
            throw new InnerAccountOrClientException($"Client with name = {client.Name} was not found in bank {_name}");
        clientInList.SetPassportNumber(passportNumber);
    }

    public void TakeofMoney(IAccount where, double amount)
    {
        var accountInList = _accounts.Find(acc => acc.Id == where.Id);
        if (accountInList == null)
            throw new InnerAccountOrClientException($"Account with id = {where.Id} was not found in bank {_name}");
        accountInList.TakeOff(amount);
    }

    public void TransferMoney(IAccount from, Client where, double amount)
    {
        var accountInList = _accounts.Find(acc => acc.Id == from.Id);
        if (accountInList == null)
            throw new InnerAccountOrClientException($"Account with id = {from.Id} was not found in bank {_name}");
        var clientInList = _clients.Find(cli => cli == where);
        if (clientInList == null)
            throw new InnerAccountOrClientException($"Client with name = {where.Name} was not found in bank {_name}");
        var accountOfClient = FindAccountOfClient(where);
        if (accountOfClient == null)
        {
            accountOfClient = FindAccountOfClientInAnotherBank(where);
            if (accountOfClient == null)
                throw new InnerAccountOrClientException($"Account of client with name = {where.Name} was not found in bank {_name}");
        }
        accountInList.TransferMoney(accountOfClient, amount);
    }

    public void TransferMoney(IAccount from, IAccount where, double amount)
    {
        var accountInListFrom = _accounts.Find(acc => acc.Id == from.Id);
        if (accountInListFrom == null)
            throw new InnerAccountOrClientException($"Account with id = {from.Id} was not found in bank {_name}");
        var accountInListWhere = _accounts.Find(acc => acc.Id == where.Id);
        if (accountInListWhere == null)
        {
            accountInListWhere = FindAccountOfAccountInAnotherBank(where);
            if (accountInListWhere == null)
                throw new InnerAccountOrClientException($"Account with id = {where.Id} was not found in all banks");
        }
        accountInListFrom.TransferMoney(accountInListWhere, amount);
    }

    private IAccount? FindAccountOfAccountInAnotherBank(IAccount account)
    {
        IAccount? accountFound = null;
        var allBanks = Banks.GetAll();
        foreach (var bank in allBanks)
        {
            if (bank as Bank != null)
            {
                accountFound = FindAccountOfAccountInAnotherBank(account, (bank as Bank)._accounts);
                break;
            }
        }
        return accountFound;
    }

    private IAccount? FindAccountOfAccountInAnotherBank(IAccount account, List<IAccount> accounts)
    {
        IAccount? accountFound = null;
        foreach (var account1 in accounts)
        {
            if (account1.Id == account.Id)
                accountFound = account1;
        }
        return accountFound;
    }

    private IAccount? FindAccountOfClientInAnotherBank(Client client)
    {
        IAccount? accountFound = null;
        var allBanks = Banks.GetAll();
        foreach (var bank in allBanks)
        {
            if (bank as Bank != null)
            {
                accountFound = FindAccountOfClientInAnotherBank(client, (bank as Bank)._accounts);
                break;
            }
        }
        return accountFound;
    }

    private IAccount? FindAccountOfClientInAnotherBank(Client client, List<IAccount> accounts)
    {
        IAccount? accountFound = null;
        foreach (var account in accounts)
        {
            if (account.Client == client)
                accountFound = account;
        }
        return accountFound;
    }

    private IAccount? FindAccountOfClient(Client client)
    {
        IAccount? accountFound = null;
        foreach (var account in _accounts)
        {
            if (account.Client == client)
                accountFound = account;
        }
        return accountFound;
    }

    public void CancelOperation(IAccount from, IAccount where, double amount, TypeOfTransaction type)
    {
        var accountInListFrom = _accounts.Find(acc => acc.Id == from.Id);
        if (accountInListFrom == null)
            throw new InnerAccountOrClientException($"Account with id = {from.Id} was not found in bank {_name}");
        var accountInListWhere = _accounts.Find(acc => acc.Id == where.Id);
        if (accountInListWhere == null)
            throw new InnerAccountOrClientException($"Account with id = {where.Id} was not found in bank {_name}");
        GlobalTransaction.CancelTransaction(accountInListFrom, accountInListWhere, amount, type);
    }

    public void CancelOperation(IAccount from, double amount, TypeOfTransaction type)
    {
        var accountInListFrom = _accounts.Find(acc => acc.Id == from.Id);
        if (accountInListFrom == null)
            throw new InnerAccountOrClientException($"Account with id = {from.Id} was not found in bank {_name}");
        GlobalTransaction.CancelTransaction(accountInListFrom, amount, type);
    }

    private ImmutableList<IAccount> GetAccounts() => _accounts.ToImmutableList();
    
        public void UpdateAllAccounts()
    {
        foreach (var account in _accounts)
        {
            account.CheckMoney();
        }
    }

    public void FutureInThePast(int days) //Сколько дней промотать
    {
        for (int i = 0; i < days; i++)
        {
            UpdateAllAccounts();
        }
    }
}
