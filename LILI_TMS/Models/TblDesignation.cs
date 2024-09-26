using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblDesignation
    {
        public TblDesignation()
        {
            TblEmployeeSetups = new HashSet<TblEmployeeSetup>();
        }

        public int Id { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentCode { get; set; }
        public string Comments { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }

        public virtual TblDepartment DepartmentCodeNavigation { get; set; }
        public virtual ICollection<TblEmployeeSetup> TblEmployeeSetups { get; set; }
    }
}
