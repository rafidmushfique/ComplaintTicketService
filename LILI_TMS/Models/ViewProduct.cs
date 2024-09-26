using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class ViewProduct
    {
        public string ProductCode { get; set; }
        public string Smscode { get; set; }
        public string ProductName { get; set; }
        public string PackSize { get; set; }
        public string BrandCode { get; set; }
        public string ProductCategory { get; set; }
        public string GroupCode { get; set; }
        public string Pccc { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? DistDiscount { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Mrp { get; set; }
        public string Business { get; set; }
        public string Active { get; set; }
        public string DiscountStatus { get; set; }
        public string BusiSumGroupCode { get; set; }
        public decimal? Carton { get; set; }
        public decimal? RatePerCarton { get; set; }
        public DateTime? EffectedDate { get; set; }
        public string SubBusinessCode { get; set; }
        public string ProductName1 { get; set; }
        public string Show { get; set; }
        public string PrincipalCode { get; set; }
        public string ReportGroupCode { get; set; }
        public string Smsorder { get; set; }
        public string StorageType { get; set; }
    }
}
