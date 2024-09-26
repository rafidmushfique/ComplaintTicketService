using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblComplainTicket
    {
        public TblComplainTicket()
        {
            TblComplainDepartmentApprovalSmsdetails = new HashSet<TblComplainDepartmentApprovalSmsdetail>();
            TblComplainDepartmentApprovals = new HashSet<TblComplainDepartmentApproval>();
            TblComplainTicketApproverSmsdetails = new HashSet<TblComplainTicketApproverSmsdetail>();
            TblComplainTicketImageDetails = new HashSet<TblComplainTicketImageDetail>();
            TblComplainTicketMachineDetails = new HashSet<TblComplainTicketMachineDetail>();
            TblServiceDepartmentTicketAssignments = new HashSet<TblServiceDepartmentTicketAssignment>();
            TblTicketAssigneeInfoPartsDetails = new HashSet<TblTicketAssigneeInfoPartsDetail>();
            TblTicketAssigneeInfoSmsdetails = new HashSet<TblTicketAssigneeInfoSmsdetail>();
            TblTicketAssigneeInfos = new HashSet<TblTicketAssigneeInfo>();
        }

        public int Id { get; set; }
        public string TicketNo { get; set; }
        public DateTime TicketDate { get; set; }
        public string DepartmentCode { get; set; }
        public string ComplainTypeCode { get; set; }
        public string ComplainDetails { get; set; }
        public string Comments { get; set; }
        public string SeverityLevelCode { get; set; }
        public string StatusCode { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public virtual TblSeverityLevel SeverityLevelCodeNavigation { get; set; }
        public virtual TblStatus StatusCodeNavigation { get; set; }
        public virtual ICollection<TblComplainDepartmentApprovalSmsdetail> TblComplainDepartmentApprovalSmsdetails { get; set; }
        public virtual ICollection<TblComplainDepartmentApproval> TblComplainDepartmentApprovals { get; set; }
        public virtual ICollection<TblComplainTicketApproverSmsdetail> TblComplainTicketApproverSmsdetails { get; set; }
        public virtual ICollection<TblComplainTicketImageDetail> TblComplainTicketImageDetails { get; set; }
        public virtual ICollection<TblComplainTicketMachineDetail> TblComplainTicketMachineDetails { get; set; }
        public virtual ICollection<TblServiceDepartmentTicketAssignment> TblServiceDepartmentTicketAssignments { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoPartsDetail> TblTicketAssigneeInfoPartsDetails { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoSmsdetail> TblTicketAssigneeInfoSmsdetails { get; set; }
        public virtual ICollection<TblTicketAssigneeInfo> TblTicketAssigneeInfos { get; set; }
    }
}
