using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblComplainDepartmentApproval
    {
        public TblComplainDepartmentApproval()
        {
            TblComplainDepartmentApprovalSmsdetails = new HashSet<TblComplainDepartmentApprovalSmsdetail>();

        }

        public int Id { get; set; }
        public string ApprovalNo { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApproverCode { get; set; }
        public string TicketNo { get; set; }
        public string ServiceDepartmentCode { get; set; }
        public string ApproverComments { get; set; }
        public string StatusCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public virtual TblEmployeeSetup ApproverCodeNavigation { get; set; }
        public virtual TblDepartment ServiceDepartmentCodeNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
        public virtual ICollection<TblComplainDepartmentApprovalSmsdetail> TblComplainDepartmentApprovalSmsdetails { get; set; }



        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public string ComplainTypeName { get; set; }

        [NotMapped]
        public string[] MachineCode { get; set; }

        [NotMapped]
        public string TicketDate { get; set; }
        //[NotMapped]
        //public string DepartmentCode { get; set; }
        //[NotMapped]
        //public string ComplainTypeCode { get; set; }

        //[NotMapped]
        //public string SeverityLevelCode { get; set; }

        //[NotMapped]
        //public string ComplainDetails { get; set; }
        //[NotMapped]
        //public string Comments { get; set; }



        











    }
}
