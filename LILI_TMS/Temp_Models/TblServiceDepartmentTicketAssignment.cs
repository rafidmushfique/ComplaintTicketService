using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblServiceDepartmentTicketAssignment
    {
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

        public virtual TblEmployeeSetup AssignByCodeNavigation { get; set; }
        public virtual TblEmployeeSetup AssignToCodeNavigation { get; set; }
        public virtual TblDepartment ServiceDepartmentCodeNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
