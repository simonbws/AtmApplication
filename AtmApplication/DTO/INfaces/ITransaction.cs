using AtmApplication.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.Domain.INfaces
{
    public interface ITransaction
    {
        void InsertTrasaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc);
        void ShowTransaction();
    }
}
