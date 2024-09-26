using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblComplainTicketApproverSmsdetail
    {
        public int Id { get; set; }
        public string TicketNo { get; set; }
        public string ApproverCode { get; set; }  
        public string Smsto { get; set; }
        public string Smsmessage { get; set; }
        public byte? Try { get; set; }
        public string Status { get; set; }
        public DateTime? SentDate { get; set; }
        public long? ReceiveId { get; set; }
        public DateTime? ReceiveDate { get; set; }= DateTime.Now;
        public string ReplyMessage { get; set; }
        public bool IsSmsreceiver { get; set; }
        public virtual TblEmployeeSetup ApproverCodeNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
        [NotMapped]
        public string ApproverName { get; set; }
        [NotMapped]
        public string Department { get; set; }
        [NotMapped]
        public string Designation { get; set; }
    }
}
