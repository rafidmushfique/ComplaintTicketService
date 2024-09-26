using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblStatus
    {
        public TblStatus()
        {
            TblComplainTicket = new HashSet<TblComplainTicket>();
        }

        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<TblComplainTicket> TblComplainTicket { get; set; }
    }
}
