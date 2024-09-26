using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblSeverityLevel
    {
        public TblSeverityLevel()
        {
            TblComplainTickets = new HashSet<TblComplainTicket>();
        }

        public int Id { get; set; }
        public string SeverityLevelCode { get; set; }
        public string SeverityLevelName { get; set; }

        public virtual ICollection<TblComplainTicket> TblComplainTickets { get; set; }
    }
}
