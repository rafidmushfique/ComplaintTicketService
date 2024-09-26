using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class ViewMaterial
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string BusinessCode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string BaseUnit { get; set; }
        public long MaterialTypeId { get; set; }
        public long? SubCategoryId { get; set; }
        public string SubBusinessCode { get; set; }
    }
}
