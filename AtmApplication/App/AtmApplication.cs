
using AtmApplication.Domain.Entities;
using AtmApplication.Domain.INfaces;
using AtmApplication.UI;

namespace AtmApplication
{  
    public class AtmApplication : IULogin
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;

        public void InitUsers()
        {
            userAccountList = new List<UserAccount>
            {
                 new UserAccount { Id = 1, FullName = "Szymon Bywalec", AccountNumber = 321321, CardNumber = 323232, CardPin = 432234, AccountBalance = 55000.00m, isLocked = false },
                 new UserAccount { Id = 2, FullName = "Berdasz Barabasz", AccountNumber = 321322, CardNumber = 322322, CardPin = 233242, AccountBalance = 60000.00m, isLocked = false },
                 new UserAccount { Id = 3, FullName = "Gardes Szaresz", AccountNumber = 321323, CardNumber = 333333, CardPin = 512352, AccountBalance = 8000.00m, isLocked = true },
            };
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
        public void Welcome()
        {
            Console.WriteLine($"Welcome back, {selectedAccount.FullName}");
        }
        
    }
}




