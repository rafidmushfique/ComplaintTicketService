﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Models
{
    public partial class TblComplainType
    {
        public int Id { get; set; }
        public string ComplainTypeCode { get; set; }
        public string ComplainTypeName { get; set; }
        public string DepartmentCode { get; set; }
        public string Comments { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        [NotMapped]
        public string DepartmentName { get; set; }

        public virtual TblDepartment DepartmentCodeNavigation { get; set; }
    }
}
