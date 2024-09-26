using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblComplainTicketImageDetail
    {
        public int Id { get; set; }
        public string TicketNo { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Location { get; set; }
        public string DocumentType { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }

        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
