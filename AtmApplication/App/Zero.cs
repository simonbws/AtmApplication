﻿using AtmApplication.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.App
{
    class Zero
    {
        static void Main(string[] args)
        {
            AppView.Welcome();
            AtmApplication atmApplication = new AtmApplication();
            atmApplication.InitUsers();
            atmApplication.CheckCardNumberAndPassword();
            atmApplication.Welcome();
            //int cardNumber = Validation.Convert<int>("Your card number");
            //Console.WriteLine($"Your card number is {cardNumber}");

            Utility.PressEnterToProcess();
        }
    }
}
