using System;
using System.Collections.Generic;
using System.Collections.Immutable;

interface IBank
{
    void AddNewClient(Client client);
    IAccount OpenNewAccountDeposit(Client client, DateTime timeOfEnd, double startMoney);
    IAccount OpenNewAccountDebitAccount(Client client, double startMoney);
    IAccount OpenNewAccountCreditAccount(Client client, double startMoney);
    void TakeofMoney(IAccount where, double amount);
    void AddMoney(IAccount where, double amount);
    void TransferMoney(IAccount from, Client where, double amount);
    void TransferMoney(IAccount from, IAccount where, double amount);
    double GetAvailableMoney(IAccount where);
    void SetAdress(Client client, Adress adress);
    void SetPassportNumber(Client client, int passportNumber);
    ImmutableList<Client> GetClients();
}