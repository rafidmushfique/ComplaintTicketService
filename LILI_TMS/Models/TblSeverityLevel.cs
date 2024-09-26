using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblSeverityLevel
    {
        public TblSeverityLevel()
        {
            TblComplainTicket = new HashSet<TblComplainTicket>();
        }

        public int Id { get; set; }
        public string SeverityLevelCode { get; set; }
        public string SeverityLevelName { get; set; }

        public virtual ICollection<TblComplainTicket> TblComplainTicket { get; set; }
    }
}
