using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblComplainTicket
    {
        public TblComplainTicket()
        {
            TblComplainDepartmentApprovalSmsdetails = new HashSet<TblComplainDepartmentApprovalSmsdetail>();
            TblComplainDepartmentApprovals = new HashSet<TblComplainDepartmentApproval>();
            TblComplainTicketsApproverSmsdetails = new List<TblComplainTicketApproverSmsdetail>();
            TblComplainTicketsImageDetails = new List<TblComplainTicketImageDetails>();
            TblServiceDepartmentTicketAssignments = new HashSet<TblServiceDepartmentTicketAssignment>();
            TblTicketAssigneeInfoPartsDetails = new HashSet<TblTicketAssigneeInfoPartsDetail>();
            TblTicketAssigneeInfoSmsdetails = new HashSet<TblTicketAssigneeInfoSmsdetail>();
            TblTicketAssigneeInfos = new HashSet<TblTicketAssigneeInfo>();
            TblComplainTicketMachineDetails = new HashSet<TblComplainTicketMachineDetail>();
            ApproverInfoViewModel = new List<ApproverInfoViewModel>();
        }

        public int Id { get; set; }
        public string TicketNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime TicketDate { get; set; }
        public string DepartmentCode { get; set; }
        public string ComplainTypeCode { get; set; }
        public string ComplainDetails { get; set; }
        public string Comments { get; set; }
        public string SeverityLevelCode { get; set; }
        public string StatusCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public virtual TblSeverityLevel SeverityLevelCodeNavigation { get; set; }
        public virtual TblStatus StatusCodeNavigation { get; set; }
        public virtual ICollection<TblComplainDepartmentApprovalSmsdetail> TblComplainDepartmentApprovalSmsdetails { get; set; }
        public virtual ICollection<TblComplainDepartmentApproval> TblComplainDepartmentApprovals { get; set; }
        public virtual List<TblComplainTicketApproverSmsdetail> TblComplainTicketsApproverSmsdetails { get; set; }
        public virtual List<TblComplainTicketImageDetails> TblComplainTicketsImageDetails { get; set; }
        public virtual ICollection<TblServiceDepartmentTicketAssignment> TblServiceDepartmentTicketAssignments { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoPartsDetail> TblTicketAssigneeInfoPartsDetails { get; set; }
        public virtual ICollection<TblTicketAssigneeInfoSmsdetail> TblTicketAssigneeInfoSmsdetails { get; set; }
        public virtual ICollection<TblTicketAssigneeInfo> TblTicketAssigneeInfos { get; set; }
        public virtual ICollection<TblComplainTicketMachineDetail> TblComplainTicketMachineDetails { get; set; }
        [NotMapped]
        public  List<ApproverInfoViewModel> ApproverInfoViewModel { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public string ComplainTypeName { get; set; }
        [NotMapped]
        public IFormFile Attachment { get; set; }
        [NotMapped]
        public string[] MachineCode { get; set; }
        [NotMapped]
        public string VwTicketDate { get; set; }

        [NotMapped]
        public string JobNo { get; set; }
        [NotMapped]
        public DateTime JobDate { get; set; }

        [NotMapped]
        public string ApprovalNo { get; set; }

        [NotMapped]
        public DateTime ApprovalDate { get; set; }
        [NotMapped]
        public string ApproverCode { get; set; }

        [NotMapped]
        public string ApproverComments { get; set; }


        [NotMapped]
        public string ServiceDepartmentCode { get; set; }
        [NotMapped]
        public string ApproverUser { get; set; }
        [NotMapped]
        public string JobExecutedBy { get; set; }


    }
}
