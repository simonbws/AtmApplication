using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.Domain.INfaces
{
    public interface IUserActions
    {
        void CheckBalance();
        void Deposit();
        void DoWithdrawal();


    }
}
