using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblComplainTicketImageDetails
    {
        public int Id { get; set; }
        public string TicketNo { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Location { get; set; }
        public string DocumentType { get; set; }
        public string Iuser { get; set; } = string.Empty;
        public DateTime Idate { get; set; }= DateTime.Now;
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }

        public virtual TblComplainTicket TicketNoNavigation { get; set; }

    }
}
