using AtmApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.UI
{
    public static class AppView
    {
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


    }
}
