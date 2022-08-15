using AtmApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.UI
{
    public class AppView
    {
        internal const string curr = "PL ";
        internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "My ATM App";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n..........Welcome to my Atm Application............\n");

            Console.WriteLine("Insert your ATM Card...");
            Console.WriteLine("Note: Actual ATM machine will accept and validate a physical ATM card, read the card number and validate it");
            Utility.PressEnterToProcess();

        }
        internal static UserAccount UserLoginForm()
        {
            UserAccount temporaryAccount = new UserAccount();

            temporaryAccount.CardNumber = Validation.Convert<long>("Card Number: ");
            temporaryAccount.CardPin = Convert.ToInt32(Utility.SecretInput("Type your PIN number: "));
            return temporaryAccount;
        }
        internal static void LoginProgress()
        {
            Console.WriteLine("Checking card number and password...");
            Utility.PrintDotAnimation();
        }
        internal static void LockMessage()
        {
            Console.Clear();
            Utility.TypeMessage("Account is locked. Please go to the nearest branch to activate your account. Thank you.", true);
            Utility.PressEnterToProcess(); // to keep the screen on
            Environment.Exit(1);    

        }
        internal static void WelcomeCstmr(string FullName)
        {
            Console.WriteLine($"Welcome back, {FullName}");
        }
        internal static void ApplicationMenu()
        {
            Console.Clear();
            Console.WriteLine("-----ATM Application Menu");
            Console.WriteLine("1. Cash                        :");
            Console.WriteLine("2. Cash Deposit                :");
            Console.WriteLine("3. Withdrawal                  :");
            Console.WriteLine("4. Transfer Money              :");
            Console.WriteLine("5. All Transactions            :");
            Console.WriteLine("6. Logout from Application     :");
        }
        internal static void LogOut()
        {
            Console.WriteLine("Thank you for using Application");
            Utility.PrintDotAnimation();
            Console.Clear();

        }
        internal static int ChooseAmount() // SelectAmount
        {
            Console.Write("");
            Console.WriteLine(":1.{0}500      5.{0}10,000", curr);
            Console.WriteLine(":2.{0}1000      6.{0}15,000", curr);
            Console.WriteLine(":3.{0}2000      7.{0}20,000", curr);
            Console.WriteLine(":4.{0}5000      8.{0}40,000", curr);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectedAmount = Validation.Convert<int>("option:");
            switch(selectedAmount)
            {
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.TypeMessage("Wrong input. Please do it again.", false);
                    //ChooseAmount();
                    return -1;
                    break;

            }        
            
        }
        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.RecipientBankAccountNumber = Validation.Convert<long>("recipients account number: ");
            internalTransfer.TransferAmount = Validation.Convert<decimal>($"Value {curr}");
            internalTransfer.RecipientBankAccountName = Utility.GetUserInput("Recipients name: ");
            return internalTransfer;
        }


    }
}
