using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblDepartmentType
    {
        public TblDepartmentType()
        {
            TblDepartments = new HashSet<TblDepartment>();
        }

        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<TblDepartment> TblDepartments { get; set; }
    }
}
