using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblDepartment
    {
        public TblDepartment()
        {
            TblComplainDepartmentApprovals = new HashSet<TblComplainDepartmentApproval>();
            TblComplainTypes = new HashSet<TblComplainType>();
            TblDesignations = new HashSet<TblDesignation>();
            TblEmployeeSetups = new HashSet<TblEmployeeSetup>();
            TblMachineSetups = new HashSet<TblMachineSetup>();
            TblServiceDepartmentTicketAssignments = new HashSet<TblServiceDepartmentTicketAssignment>();
            TblTicketAssigneeInfos = new HashSet<TblTicketAssigneeInfo>();
        }

        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Comments { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public virtual TblDepartmentType TypeCodeNavigation { get; set; }
        public virtual ICollection<TblComplainDepartmentApproval> TblComplainDepartmentApprovals { get; set; }
        public virtual ICollection<TblComplainType> TblComplainTypes { get; set; }
        public virtual ICollection<TblDesignation> TblDesignations { get; set; }
        public virtual ICollection<TblEmployeeSetup> TblEmployeeSetups { get; set; }
        public virtual ICollection<TblMachineSetup> TblMachineSetups { get; set; }
        public virtual ICollection<TblServiceDepartmentTicketAssignment> TblServiceDepartmentTicketAssignments { get; set; }
        public virtual ICollection<TblTicketAssigneeInfo> TblTicketAssigneeInfos { get; set; }
    }
}
