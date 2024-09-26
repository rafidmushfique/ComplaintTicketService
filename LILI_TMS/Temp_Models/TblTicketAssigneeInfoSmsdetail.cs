using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblTicketAssigneeInfoSmsdetail
    {
        public int Id { get; set; }
        public string JobNo { get; set; }
        public string TicketNo { get; set; }
        public string ApproverCode { get; set; }
        public string Smsto { get; set; }
        public string Smsmessage { get; set; }
        public byte Try { get; set; }
        public string Status { get; set; }
        public DateTime? SentDate { get; set; }
        public long? ReceiveId { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ReplyMessage { get; set; }
        public bool IsSmsreceiver { get; set; }

        public virtual TblEmployeeSetup ApproverCodeNavigation { get; set; }
        public virtual TblTicketAssigneeInfo JobNoNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
