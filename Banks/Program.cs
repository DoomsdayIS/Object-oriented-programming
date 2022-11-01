using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Models;
using Banks.Tools;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var centralBank = CentralBank.GetInstance();
            var clients = new List<Client>();
            var accounts = new List<IBankAccount>();
            Console.WriteLine(" Hi, Press number __  to \n" +
                              "1 : create a client, \n" +
                              "2 : create bank \n" +
                              "3 : open bank account \n" +
                              "4 : subscribe client to Notification \n" +
                              "5 : Create transaction \n" +
                              "6 : Cancel transaction \n" +
                              "7 : Skip a couple of days \n" +
                              "8 : Change smth in Bank \n" +
                              "9 : Get all info about accounts/banks/clients\n" +
                              "10+ : quit program\n");
            bool isWorking = true;
            while (isWorking)
            {
                Console.WriteLine("Next operation:");
                string s = Console.ReadLine();
                if (!int.TryParse(s, out int number))
                {
                    Console.WriteLine("Incorrect input! Try again!");
                    continue;
                }

                string str;
                List<string> arg;
                switch (number)
                {
                    case 1:
                        Console.WriteLine("Enter first and last name separated by a space");
                        str = Console.ReadLine();
                        var clientBuilder = new ClientBuilder();
                        clientBuilder.BuildStep1(str);
                        Console.WriteLine("Client Created ! Will you enter the address? yes/no");
                        str = Console.ReadLine();
                        switch (str)
                        {
                           case "yes":
                               Console.WriteLine("Enter the address");
                               str = Console.ReadLine();
                               clientBuilder.BuildStep2(str);
                               break;
                        }

                        Console.WriteLine("Will you enter passport number? yes/no");
                        str = Console.ReadLine();
                        switch (str)
                        {
                            case "yes":
                                Console.WriteLine("Enter passport number");
                                str = Console.ReadLine();
                                clientBuilder.BuildStep3(str);
                                break;
                        }

                        clients.Add(clientBuilder.GetClient());
                        break;
                    case 2:
                        Console.WriteLine("Creating a bank ..., Enter seperated by a space: \n" +
                                          "Bank name, interest on balance, " +
                                          "credit commission, credit limit, limit for unauthorized");
                        str = Console.ReadLine();
                        arg = str?.Split(' ').ToList();
                        switch (arg?.Count)
                        {
                            case 5:
                                if (int.TryParse(arg[2], out int creditCommission)
                                    && int.TryParse(arg[3], out int creditLimit)
                                    && int.TryParse(arg[4], out int unauthorizedLimit)
                                    && double.TryParse(arg[1], out double interestOnBalance))
                                {
                                    centralBank.CreateBank(
                                        arg[0],
                                        interestOnBalance,
                                        creditCommission,
                                        creditLimit,
                                        unauthorizedLimit);
                                }

                                break;
                            default:
                                Console.WriteLine("Incorrect input!");
                                break;
                        }

                        break;
                    case 3:
                        Console.WriteLine("Creating an account ..., Enter seperated by a space:\n" +
                                          "client first and last name, bank name and " +
                                          "account type: debit/credit/deposit");
                        str = Console.ReadLine();
                        arg = str?.Split(' ').ToList();
                        if (arg.Count < 4)
                        {
                            break;
                        }

                        Client client = clients
                            .Find(c => c.FirstName == arg?[0] && c.LastName == arg?[1]);
                        Bank bank = centralBank.Banks.Find(b => b.Name == arg?[2]);
                        if (client != null && bank != null && arg.Count == 4)
                        {
                            IBankAccount bankAccount = bank.OpenAccount(client, arg[3], null);
                            accounts.Add(bankAccount);
                            Console.WriteLine("Bank account was created successfully!");
                            break;
                        }

                        if (client != null && bank != null && arg.Count == 5)
                        {
                            if (int.TryParse(arg[4], out int depositDays))
                            {
                                IBankAccount depositAccount = bank.OpenAccount(client, arg[3], depositDays);
                                accounts.Add(depositAccount);
                                Console.WriteLine("Bank account was created successfully!");
                                break;
                            }
                        }

                        Console.WriteLine("You didn't create an account, try again!");

                        break;
                    case 4:
                        Console.WriteLine("Enter seperated by a space: " +
                                          "client first and last name");
                        str = Console.ReadLine();
                        arg = str?.Split(' ').ToList();
                        Client client2 = clients
                            .Find(c => c.FirstName == arg?[0] && c.LastName == arg?[1]);
                        INotification notification = new BasedNotification();
                        if (client2 != null && arg.Count == 2)
                        {
                            client2.SubscribeToNotification(notification);
                            Console.WriteLine("Client was subscribed successfully!");
                            continue;
                        }

                        Console.WriteLine("Sorry u don't subscribe a client, try again!");

                        break;
                    case 5:
                        Console.WriteLine("Choose transaction type: Enter 1 if you want replenish" +
                                          " 2 - withdraw" +
                                          " 3 - transfer");
                        string opType = Console.ReadLine();
                        switch (opType)
                        {
                            case "1":
                                Console.WriteLine("Enter the transaction amount and" +
                                                  " the number of creation of the desired account");
                                str = Console.ReadLine();
                                arg = str?.Split(' ').ToList();
                                if (int.TryParse(arg[0], out int smth1)
                                    && int.TryParse(arg[1], out int smth2))
                                {
                                    smth2 -= 1;
                                    var oper1 = new Replenish(smth1, accounts[smth2]);
                                    bool value1 = centralBank.DoOperation(oper1);
                                    if (value1)
                                    {
                                        Console.WriteLine($"Transaction passed Successfully! " +
                                                          $"Now there is {accounts[smth2].AvailableMoney} " +
                                                          $"money in account!");
                                        continue;
                                    }
                                }

                                Console.WriteLine("Transaction didn't create!");

                                break;
                            case "2":
                                Console.WriteLine("Enter the transaction amount and" +
                                                  " the number of creation of the desired account");
                                str = Console.ReadLine();
                                arg = str?.Split(' ').ToList();
                                if (int.TryParse(arg[0], out int smth3)
                                    && int.TryParse(arg[1], out int smth4))
                                {
                                    smth4 -= 1;
                                    var oper2 = new Withdraw(smth3, accounts[smth4]);
                                    bool value2 = centralBank.DoOperation(oper2);
                                    if (value2)
                                    {
                                        Console.WriteLine($"Transaction passed Successfully! " +
                                                          $"Now there is {accounts[smth4].AvailableMoney} " +
                                                          $"money in account!");
                                    }

                                    continue;
                                }

                                Console.WriteLine("Transaction didn't create!");

                                break;
                            case "3":
                                Console.WriteLine("Enter the transaction amount and" +
                                                  " the number of creation of the  account from and " +
                                                  "the number of creation of the  account to");
                                str = Console.ReadLine();
                                arg = str?.Split(' ').ToList();
                                if (int.TryParse(arg[0], out int smth5)
                                    && int.TryParse(arg[1], out int smth6)
                                    && int.TryParse(arg[2], out int smth7))
                                {
                                    smth6 -= 1;
                                    smth7 -= 1;
                                    var oper3 = new Transfer(smth5, accounts[smth6], accounts[smth7]);
                                    bool value3 = centralBank.DoOperation(oper3);
                                    if (value3)
                                    {
                                        Console.WriteLine($"Transfer passed Successfully! " +
                                                          $"Now there are {accounts[smth6].AvailableMoney} and " +
                                                          $"{accounts[smth7].AvailableMoney} " +
                                                          $"available money!");
                                        continue;
                                    }
                                }

                                Console.WriteLine("Transfer didn't pass!");

                                break;
                            default:
                                Console.WriteLine("Your input was incorrect!");
                                break;
                        }

                        break;
                    case 6:
                        Console.WriteLine("Enter the number(creation order) of the operation you want to cancel");
                        str = Console.ReadLine();
                        if (int.TryParse(str, out int opNumber) && opNumber < centralBank.Operations.Count + 1)
                        {
                            opNumber -= 1;
                            bool value4 = centralBank.CancelOperation(centralBank.Operations[opNumber]);
                            if (value4)
                            {
                                Console.WriteLine("U cancel operation!");
                                continue;
                            }
                        }

                        Console.WriteLine("You didn't cancel operation!");

                        break;
                    case 7:
                        Console.WriteLine("Enter how many days you want to pass");
                        str = Console.ReadLine();
                        if (int.TryParse(str, out int daysCount))
                        {
                            centralBank.SkipSomeDays(daysCount);
                            Console.WriteLine($"You past {daysCount} days!");
                            continue;
                        }

                        Console.WriteLine("Your input was incorrect!");

                        break;
                    case 8:
                        Console.WriteLine("Enter bank name:");
                        string bankName = Console.ReadLine();
                        Bank bank8 = centralBank.Banks.Find(b => b.Name == bankName);
                        if (bank8 != null)
                        {
                            Console.WriteLine("Enter number what u what to change: " +
                                              "1 - interest on balance, " +
                                              " 2 - credit commission, 3 - credit limit, 4 -  limit for unauthorized");
                            str = Console.ReadLine();
                            switch (str)
                            {
                                case "1":
                                    Console.WriteLine("Enter new interest on balance:");
                                    string strInterest = Console.ReadLine();
                                    if (double.TryParse(strInterest, out double newInterestOnBalance))
                                    {
                                        bank8.ChangeInterestOnBalance(newInterestOnBalance);
                                        Console.WriteLine("Changed successfully!");
                                        break;
                                    }

                                    Console.WriteLine("Incorrect interest!");
                                    break;
                                case "2":
                                    Console.WriteLine("Enter new credit commission:");
                                    string strCommission = Console.ReadLine();
                                    Console.WriteLine(strCommission);
                                    if (int.TryParse(strCommission, out int newCreditCommission))
                                    {
                                        bank8.ChangeCreditCommission(newCreditCommission);
                                        Console.WriteLine("Changed successfully!");
                                        break;
                                    }

                                    Console.WriteLine("Incorrect new credit commission!");
                                    break;
                                case "3":
                                    Console.WriteLine("Enter new credit limit:");
                                    string strLimit = Console.ReadLine();
                                    if (int.TryParse(strLimit, out int newCreditLimit))
                                    {
                                        bank8.ChangeCreditLimit(newCreditLimit);
                                        Console.WriteLine("Changed successfully!");
                                        break;
                                    }

                                    Console.WriteLine("Incorrect new credit limit!");
                                    break;
                                case "4":
                                    Console.WriteLine("Enter new unauthorized limit:");
                                    string unauthorized = Console.ReadLine();
                                    if (int.TryParse(unauthorized, out int newUnauthorized))
                                    {
                                        bank8.ChangeUnauthorizedLimit(newUnauthorized);
                                        Console.WriteLine("Changed successfully!");
                                        break;
                                    }

                                    Console.WriteLine("Incorrect new unauthorized limit!");
                                    break;
                                default:
                                    Console.WriteLine("Incorrect input!");
                                    break;
                            }
                        }

                        break;
                    case 9:
                        Console.WriteLine("Info about clients:");
                        foreach (Client clientInfo in clients)
                        {
                            Console.WriteLine($"{clientInfo.FirstName} {clientInfo.LastName} | " +
                                              $"Passport {clientInfo.PassportId} | " +
                                              $"Address {clientInfo.Address} | " +
                                              $"Notification {clientInfo.Notification != null}");
                        }

                        Console.WriteLine("Info about banks and bank accounts:");
                        foreach (Bank bankInfo in centralBank.Banks)
                        {
                            Console.WriteLine($"Name : {bankInfo.Name} ID {bankInfo.Id} | " +
                                              $"Interest on balance {bankInfo.InterestOnBalance} | " +
                                              $"Credit commission {bankInfo.CreditCommission} | " +
                                              $"Credit limit {bankInfo.CreditLimit} | " +
                                              $"Unauthorized Limit {bankInfo.UnauthorizedLimit} | " +
                                              $"Today is {bankInfo.CurrentDay}");
                            foreach (IBankAccount accountInfo in bankInfo.BankAccounts)
                            {
                                Console.WriteLine("Bank accounts:");
                                Console.WriteLine($"ID {accountInfo.Id} | " +
                                                  $"Type {accountInfo.Type} | " +
                                                  $"Amount of money = {accountInfo.AvailableMoney} | " +
                                                  $"Owner {accountInfo.ClientAccOwner.FirstName} " +
                                                  $"{accountInfo.ClientAccOwner.LastName}");
                            }

                            Console.WriteLine("\n");
                        }

                        break;
                    default:
                        isWorking = false;
                        break;
                }
            }
        }
    }
}
