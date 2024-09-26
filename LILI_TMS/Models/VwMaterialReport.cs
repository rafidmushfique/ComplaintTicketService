using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class VwMaterialReport
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string BusinessCode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string BaseUnit { get; set; }
        public string MaterialType { get; set; }
        public string MaterialCategory { get; set; }
    }
}
