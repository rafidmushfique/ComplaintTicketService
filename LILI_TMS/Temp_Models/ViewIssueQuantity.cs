using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class ViewIssueQuantity
    {
        public long? Id { get; set; }
        public string RequisitionNo { get; set; }
        public string MaterialCode { get; set; }
        public decimal? IssueQuantityDc { get; set; }
    }
}
