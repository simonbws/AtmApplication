using AtmApplication.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.Domain.Entities
{
    public class Transaction
    {
        public long TransId { get; set; }
        public long BankAccountId { get; set; }
        public DateTime TransDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public Decimal TransactionAmount { get; set; }
    }
}
