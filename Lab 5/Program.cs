using System;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            var tinkoff = new Tinkoff();
            var nikita = new Client("Nikita", "Shestakov");
            tinkoff.AddNewClient(nikita);
            var NikitaDebitAccount = tinkoff.OpenNewAccountDebitAccount(nikita);
            tinkoff.AddMoney(NikitaDebitAccount, 10000);
            tinkoff.CancelOperation(NikitaDebitAccount, 10000, TypeOfTransaction.Add);
            var sberbank = new Sberbank();
            sberbank.AddNewClient(nikita);
            var NikitaDebitAccountSber = sberbank.OpenNewAccountDebitAccount(nikita, 100000);
            sberbank.TransferMoney(NikitaDebitAccountSber, NikitaDebitAccount, 10000);
        }
    }
}