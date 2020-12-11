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
            var sberbank = new Sberbank();
            sberbank.AddNewClient(nikita);
            var NikitaDebitAccountSber = sberbank.OpenNewAccountDebitAccount(nikita, 100000);
            sberbank.TransferMoney(NikitaDebitAccountSber, NikitaDebitAccount, 10000);
            var nikitaCreditAccount = tinkoff.OpenNewAccountCreditAccount(nikita, 10000);
            tinkoff.FutureInThePast(40);
            sberbank.FutureInThePast(20);
            Console.WriteLine(tinkoff.GetAvailableMoney(NikitaDebitAccount)); //was 20000 became 20014.39
            Console.WriteLine(sberbank.GetAvailableMoney(NikitaDebitAccountSber)); //was 900000 90047.14
        }
    }
}
