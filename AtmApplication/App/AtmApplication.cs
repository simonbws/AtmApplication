
using AtmApplication.Domain.Entities;
using AtmApplication.Domain.Enumerators;
using AtmApplication.Domain.INfaces;
using AtmApplication.UI;
using ConsoleTables;
using System.Linq;

namespace AtmApplication
{  
    public class AtmApplication : IULogin, IUserActions, ITransaction
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _TransactionList;
        private const decimal minTakeValue = 500; //minimumKeptAmount
        private readonly AppView view;

        public AtmApplication()
        {
            view = new AppView();
        }


        public void Go()
        {
            AppView.Welcome();
            CheckCardNumberAndPassword();
            AppView.WelcomeCstmr(selectedAccount.FullName);
            while (true)
            {
                AppView.ApplicationMenu();
                ProcessMenu();
            }
        }
        public void InitUsers()
        {
            userAccountList = new List<UserAccount>
            {
                 new UserAccount { Id = 1, FullName = "Szymon Bywalec", AccountNumber = 321321, CardNumber = 323232, CardPin = 432234, AccountBalance = 55000.00m, isLocked = false },
                 new UserAccount { Id = 2, FullName = "Berdasz Barabasz", AccountNumber = 321322, CardNumber = 322322, CardPin = 233242, AccountBalance = 60000.00m, isLocked = false },
                 new UserAccount { Id = 3, FullName = "Gardes Szaresz", AccountNumber = 321323, CardNumber = 333333, CardPin = 512352, AccountBalance = 8000.00m, isLocked = true },
            };
            _TransactionList = new List<Transaction>();
        }
        
        public void CheckCardNumberAndPassword()
        {
            bool CorrectLogin = false;
            while(CorrectLogin == false)
            {
                UserAccount inputAccount = AppView.UserLoginForm();
                AppView.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if(inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.Login++;

                        if(inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if(selectedAccount.isLocked || selectedAccount.Login > 3)
                            {
                                AppView.LockMessage();
                            }
                            else
                            {
                                selectedAccount.Login = 0;
                                CorrectLogin = true;
                                break;
                            }
                        }

                    }
                    if (CorrectLogin == false)
                    {
                        Utility.TypeMessage("\n Invalid Card Number or PIN... Please type correct.", false);
                        selectedAccount.isLocked = selectedAccount.Login == 3;
                        if (selectedAccount.isLocked)
                        {
                            AppView.LockMessage();
                        }
                    }
                    Console.Clear();
                }

            }          

        }
        private void ProcessMenu()
        {
            switch(Validation.Convert<int>("option:"))
            {
                case (int)ApplicationMenuEnum.CheckBalance:
                    CheckBalance();
                    break;
                case (int)ApplicationMenuEnum.Deposit:
                    Deposit();
                    break;
                case (int)ApplicationMenuEnum.DoWithdrawal:
                    DoWithdrawal();
                    break;
                case (int)ApplicationMenuEnum.InTransfer:
                    var internalTransfer = view.InternalTransferForm();
                    ProcessOfInternalTransfer(internalTransfer);
                    break;
                case (int)ApplicationMenuEnum.ShowTransaction:
                    ShowTransaction();
                    break;
                case (int)ApplicationMenuEnum.Logout:
                    AppView.LogOut();
                    Utility.TypeMessage("You has been logged out. Take your card. Thank you");
                    Go();
                    break;
                default:
                    Utility.TypeMessage("Wrong Option. ", false);
                    break;

            }
        }

        public void CheckBalance()
        {
            Utility.TypeMessage($"Your balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void Deposit()
        {
            Console.WriteLine("\nOnly multiples of 500 and 1000 złotych allowed.\n");
            var transAmount = Validation.Convert<int>($"Amount {AppView.curr}");

            //simulate counting
            Console.WriteLine("\nChecking and Counting bank notes");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            //some guard clause
            if(transAmount <= 0)
            {
                Utility.TypeMessage("Value must be greater than zero. Please, try again", false);
                return;
            }
            if(transAmount % 500 !=0)
            {
                Utility.TypeMessage($"Enter deposit amount in multiple of 500 or 1000. Please, try again.", false);
                return;
            }
            if(PreviewBankNotesCount(transAmount) == false)
            {
                Utility.TypeMessage($"You have cancelled your action", false);
                return;
            }

            //bind transaction details to transaction object
            InsertTrasaction(selectedAccount.Id, TransactionType.Deposit, transAmount, "");

            //actualize account balance
            selectedAccount.AccountBalance += transAmount;

            //print Succes message to the screen

            Utility.TypeMessage($"Deposit of {Utility.FormatAmount(transAmount)} was succesfull", true);
        }

        public void DoWithdrawal()
        {
            var transaction_amt = 0;
            var selectedAmount = AppView.ChooseAmount();
            if(selectedAmount == -1)
            {
                DoWithdrawal();
                return;

            }else if(selectedAmount !=0)

            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validation.Convert<int>($"amount {AppView.curr}");
            }

            //input validation
            if(transaction_amt <=0)
            {
                Utility.TypeMessage("Value must be greater than zero. Please try again");
                return;
            }
            if(transaction_amt % 500 !=0)
            {
                Utility.TypeMessage("Only withdrawing an amount in multiples of 500 or 1000 is allowed. Please, try again", false);
                return;
            }
            //Business logic validation

            if(transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.TypeMessage($"Error. Your balance is too low to make withdrawal" +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }

            if((selectedAccount.AccountBalance - transaction_amt < minTakeValue))
            {
                Utility.TypeMessage($"Error. Your balance needs to have" + 
                    $"minimum {Utility.FormatAmount(minTakeValue)}", false);
                return;
            }
            //Bind withdrawal details to transaction objects
            InsertTrasaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
            //update account balance
            selectedAccount.AccountBalance -= transaction_amt;
            //success msg
            Utility.TypeMessage($"Withdrawal has been successfully done " +
                $"{Utility.FormatAmount(transaction_amt)}. ", true);






        }
        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            Console.WriteLine("\nSummary");
            Console.WriteLine("------");
            Console.WriteLine($"{AppView.curr}1000 X {thousandNotesCount} = {1000 * thousandNotesCount}");
            Console.WriteLine($"{AppView.curr}500 X {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validation.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTrasaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            //create a new transaction object
            var transaction = new Transaction()
            {
                TransId = Utility.GetTransactionId(),
                BankAccountId = _UserBankAccountId,
                TransDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };
            // add transaction object to the list
            _TransactionList.Add(transaction);
        }
        private void ProcessOfInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.TypeMessage("Value must be more than zero. Please try again", false);
                return;
            }
            //check sender's accounts balance
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.TypeMessage("Error in transfer. You must have more balance" +
                    $" to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }
            //check the minimum kept amount
            if ((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minTakeValue)
            {
                Utility.TypeMessage($"Transfer failed. Your balance must at least minimum" +
                    $" {Utility.FormatAmount(minTakeValue)}", false);
                return;
            }
            //check reveiver's account number is valid
            var givenBankAccountReceiver = (from userAccount in userAccountList
                                            where userAccount.AccountNumber == internalTransfer.RecipientBankAccountNumber
                                            select userAccount).FirstOrDefault();
            if (givenBankAccountReceiver == null)
            {
                Utility.TypeMessage("Trainsfer failed. Receiver's bank account number is incorrect", false);
                return;
            }
            //check receivers name is correct
            if (givenBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.TypeMessage("Transfer failed. Recipient's bank account name is not match.", false);
                return;
            }

            //add transaction to transactions record - sender
            InsertTrasaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered" +
                $" to {givenBankAccountReceiver.AccountNumber} ({givenBankAccountReceiver.FullName})");
            //update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //add transaction record-reciever
            InsertTrasaction(givenBankAccountReceiver.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from +" +
                $"{selectedAccount.AccountNumber}({selectedAccount.FullName})");
            //update receivers account balance
            givenBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;
            //print succes message
            Utility.TypeMessage($"Transfer has been done successfully " +
                $" {Utility.FormatAmount(internalTransfer.TransferAmount)} to" +
                $" {internalTransfer.RecipientBankAccountName}", true);

        }
        
        public void ShowTransaction()
        {
            var filterListTransaction = _TransactionList.Where(t => t.BankAccountId == selectedAccount.Id).ToList();
            //check if theres a transaction
            if(filterListTransaction.Count <= 0)
            {
                Utility.TypeMessage("You don't have transaction yet", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Date of Transation", "Type", "Description", "Value " + AppView.curr);
                foreach(var transaction in filterListTransaction)
                {
                    table.AddRow(transaction.TransId, transaction.TransDate, transaction.TransactionType, transaction.Description, transaction.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.TypeMessage($"You got {filterListTransaction.Count} transaction(s)", true);
            }

        }
        
    }
}




