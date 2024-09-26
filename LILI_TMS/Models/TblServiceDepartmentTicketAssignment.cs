using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblServiceDepartmentTicketAssignment
    {
        public TblServiceDepartmentTicketAssignment()
        {
            TblComplainTicketsImageDetails = new List<TblComplainTicketImageDetails>();
        }
        public int Id { get; set; }
        public string AssignNo { get; set; }
        public DateTime AssignDate { get; set; }
        public string AssignByCode { get; set; }
        public string TicketNo { get; set; }
        public string ServiceDepartmentCode { get; set; }
        public string AssignToCode { get; set; }
        public string AssignerComments { get; set; }
        public string StatusCode { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        [NotMapped]
        public string DepartmentCode { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public string ServiceDepartmentName { get; set; }
        [NotMapped]
        public DateTime TicketDate { get; set; }
        [NotMapped]
        public string ComplainDetails { get; set; }
        [NotMapped]
        public string SeverityLevelName { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        [NotMapped]
        public string ComplainTypeCode { get; set; }
        [NotMapped]
        public string MachineCode { get; set; }

        [NotMapped]
        public string SeverityLevelCode { get; set; }

        [NotMapped]
        public string ApproverCode { get; set; }
        [NotMapped]
        public string ApproverName { get; set; }

        [NotMapped]
        public string ApproverComments { get; set; }

        [NotMapped]
        public string Comments { get; set; }
        [NotMapped]

        public virtual ICollection<TblComplainTicketImageDetails> TblComplainTicketsImageDetails { get; set; }

        public virtual TblEmployeeSetup AssignByCodeNavigation { get; set; }
        public virtual TblEmployeeSetup AssignToCodeNavigation { get; set; }
        public virtual TblDepartment ServiceDepartmentCodeNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
