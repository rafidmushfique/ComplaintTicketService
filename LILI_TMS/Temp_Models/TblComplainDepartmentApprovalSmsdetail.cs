using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblComplainDepartmentApprovalSmsdetail
    {
        public int Id { get; set; }
        public string ApprovalNo { get; set; }
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

        public virtual TblComplainDepartmentApproval ApprovalNoNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
