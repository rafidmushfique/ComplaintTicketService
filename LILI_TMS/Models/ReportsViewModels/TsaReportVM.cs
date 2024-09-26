using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LILI_TMS.Models.ReportsViewModels
{
    public class TsaReportVM
    {
        [Display(Name = "Sl No")]
        public int SlNo { get; set; }

        [Display(Name = "TSA Name")]
        public string TSAName { get; set; }

        [Display(Name = "TSA Code")]
        public string TSACode { get; set; }

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
        public int Quantity { get; set; }

        [Display(Name = "Action Type")]
        public string ActionTypeName { get; set; }

        [Display(Name = "Action Date")]
        public DateTime ActionDate { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Original File Name")]
        public string OriginalFileName { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Comments")]
        public string Description { get; set; }


    }
}
