using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblTicketAssigneeInfo
    {
        public TblTicketAssigneeInfo()
        {
            TblTicketAssigneeInfoPartsDetails = new HashSet<TblTicketAssigneeInfoPartsDetail>();
            TblTicketAssigneeInfoSmsdetails = new HashSet<TblTicketAssigneeInfoSmsdetail>();
        }

        public int Id { get; set; }
        public string JobNo { get; set; }
        public DateTime JobDate { get; set; }
        public string TicketNo { get; set; }
        public string ServiceDepartmentCode { get; set; }
        public string JobExecutedByCode { get; set; }
        public string Comments { get; set; }
        public string StatusCode { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public virtual TblEmployeeSetup JobExecutedByCodeNavigation { get; set; }
        public virtual TblDepartment ServiceDepartmentCodeNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoPartsDetail> TblTicketAssigneeInfoPartsDetails { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoSmsdetail> TblTicketAssigneeInfoSmsdetails { get; set; }
    }
}
