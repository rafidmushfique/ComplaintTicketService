using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LILI_TMS.Models.ReportsViewModels
{
    public class TsaWiseAssignReportVM
    {
        [Display(Name = "Sl No")]
        public int SlNo { get; set; }

        [Display(Name = "TSA Code")]
        public string TSACode { get; set; }

        [Display(Name = "TSA Name")]
        public string TSAName { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Area Name")]
        public string AreaName { get; set; }

        [Display(Name = "Region Name")]
        public string RegionName { get; set; }

        [Display(Name = "Tool Code")]
        public string ToolCode { get; set; }

        [Display(Name = "Tool Name")]
        public string ToolName { get; set; }

        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }

        [Display(Name = "Action Type")]
        public string ActionTypeName { get; set; }

        [Display(Name = "Action Date")]
        public DateTime ActionDate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }


    }
}
