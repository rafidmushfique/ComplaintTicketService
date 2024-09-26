using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblEmployeeSetup
    {
        public TblEmployeeSetup()
        {
            TblComplainDepartmentApprovals = new HashSet<TblComplainDepartmentApproval>();
            TblComplainTicketApproverSmsdetails = new HashSet<TblComplainTicketApproverSmsdetail>();
            TblServiceDepartmentTicketAssignmentAssignByCodeNavigations = new HashSet<TblServiceDepartmentTicketAssignment>();
            TblServiceDepartmentTicketAssignmentAssignToCodeNavigations = new HashSet<TblServiceDepartmentTicketAssignment>();
            TblTicketAssigneeInfoSmsdetails = new HashSet<TblTicketAssigneeInfoSmsdetail>();
            TblTicketAssigneeInfos = new HashSet<TblTicketAssigneeInfo>();
        }

        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentCode { get; set; }
        public string DesignationCode { get; set; }
        public string ContactNo { get; set; }
        public bool IsApprover { get; set; }
        public bool IsSmsreceiver { get; set; }
        public string Comments { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public virtual TblDepartment DepartmentCodeNavigation { get; set; }
        public virtual TblDesignation DesignationCodeNavigation { get; set; }
        public virtual ICollection<TblComplainDepartmentApproval> TblComplainDepartmentApprovals { get; set; }
        public virtual ICollection<TblComplainTicketApproverSmsdetail> TblComplainTicketApproverSmsdetails { get; set; }
        public virtual ICollection<TblServiceDepartmentTicketAssignment> TblServiceDepartmentTicketAssignmentAssignByCodeNavigations { get; set; }
        public virtual ICollection<TblServiceDepartmentTicketAssignment> TblServiceDepartmentTicketAssignmentAssignToCodeNavigations { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoSmsdetail> TblTicketAssigneeInfoSmsdetails { get; set; }
        public virtual ICollection<TblTicketAssigneeInfo> TblTicketAssigneeInfos { get; set; }
    }
}
