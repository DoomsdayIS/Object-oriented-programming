using System;
using Banks.Interfaces;
using Banks.Models;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        [TestFixture]
        public class BackupJobTest
        {
            private Client _client;
            private Client _client2;
            private CentralBank _centralBank;
            private Bank _bank1;

            [SetUp]
            public void Setup()
            {

                _client = new Client("Ivan", "Sheremet");
                _client2 = new Client("Igor", "Likhachev", "SPB", "88222");
                _centralBank = CentralBank.GetInstance();
                _bank1 = _centralBank
                    .CreateBank("Tin", 3.65, 100, 10000, 600);

            }

            [Test]
            public void CreateDepositAndCredit_CreatedSuccessfully()
            {
                IBankAccount acc2 = _bank1.OpenAccount(_client2, "credit",null);
                IBankAccount acc = _bank1.OpenAccount(_client, "debit", null);
                Assert.AreEqual(2, _bank1.BankAccounts.Count);
                Assert.AreNotEqual(acc.Type, acc2.Type);
                Assert.AreEqual(2, acc.Id);
            }

            [Test]
            public void NotificationSubscribeAndChangeSmth_GetNotification()
            {
                IBankAccount acc2 = _bank1.OpenAccount(_client2, "credit",null);
                IBankAccount acc = _bank1.OpenAccount(_client, "debit",null);
                INotification notification = new BasedNotification();
                _client.SubscribeToNotification(notification);
                _bank1.ChangeCreditCommission(500);
                Assert.AreEqual(500, _bank1.CreditCommission);
            }

            [Test]
            public void TryAllTypesOfOperation_AndCheckResults()
            {
                IBankAccount acc2 = _bank1.OpenAccount(_client2, "credit",null);
                IBankAccount acc = _bank1.OpenAccount(_client, "debit",null);
                var oper = new Withdraw(200, acc2);
                _centralBank.DoOperation(oper);
                var oper1 = new Replenish(20000, acc);
                _centralBank.DoOperation(oper1);
                var oper2 = new Transfer(400, acc, acc2);
                _centralBank.DoOperation(oper2);
                Assert.AreEqual(19600, acc.AvailableMoney);
                Assert.AreEqual(200, acc2.AvailableMoney);
            }

            [Test]
            public void GetFalseByWithdrawALotOfMoney()
            {
                IBankAccount acc2 = _bank1.OpenAccount(_client2, "credit",null);
                IBankAccount acc = _bank1.OpenAccount(_client, "debit",null);
                var oper = new Withdraw(2000000, acc2);
                Assert.AreEqual(false,_centralBank.DoOperation(oper));
                

            }

            [Test] public void CanceleOperation_AndGetMoneyBack()
            {
                IBankAccount acc2 = _bank1.OpenAccount(_client2, "credit",null);
                IBankAccount acc = _bank1.OpenAccount(_client, "debit",null);
                var oper = new Withdraw(200, acc2);
                _centralBank.DoOperation(oper);
                var oper1 = new Replenish(20000, acc);
                _centralBank.DoOperation(oper1);
                var oper2 = new Transfer(400, acc, acc2);
                _centralBank.DoOperation(oper2);
                _centralBank.CancelOperation(oper2);
                Assert.AreEqual(20000, acc.AvailableMoney);
                Assert.AreEqual(-200, acc2.AvailableMoney);
                Assert.AreEqual(2,_centralBank.Operations.Count);
            }
        }
    }
}