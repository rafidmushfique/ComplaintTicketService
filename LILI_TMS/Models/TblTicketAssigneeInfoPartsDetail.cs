using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblTicketAssigneeInfoPartsDetail
    {
        public int Id { get; set; }
        public string JobNo { get; set; }
        public string TicketNo { get; set; }
        public string PartsCode { get; set; }
        public decimal Quantity { get; set; }

        [NotMapped]
        public string PartsName { get; set; }
        public virtual TblTicketAssigneeInfo JobNoNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
