using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.Domain.Entities
{
    public class UserAccount
    {
        public int Id{ get; set; }
        public long CardNumber{ get; set; }
        public int CardPin{ get; set; }
        public long AccountNumber { get; set; }
        public string FullName { get; set; }
        public decimal AccountBalance { get; set; }
        public int Login { get; set; }
        public bool isLocked { get; set; }

        

    }
}
