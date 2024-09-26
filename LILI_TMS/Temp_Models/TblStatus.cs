using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblStatus
    {
        public TblStatus()
        {
            TblComplainTickets = new HashSet<TblComplainTicket>();
        }

        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public int StatusOrder { get; set; }

        public virtual ICollection<TblComplainTicket> TblComplainTickets { get; set; }
    }
}
