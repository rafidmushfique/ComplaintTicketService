using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblPart
    {
        public int Id { get; set; }
        public string PartsCode { get; set; }
        public string PartsName { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
    }
}
