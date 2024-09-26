using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class ViewBom
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long PlantId { get; set; }
        public string BusinessCode { get; set; }
        public long BomusageTypeId { get; set; }
        public long TechnicalTypeId { get; set; }
        public string Bomtext { get; set; }
        public string Bomtype { get; set; }
        public string ProductCode { get; set; }
        public decimal BatchSize { get; set; }
        public decimal StandardOutput { get; set; }
        public string AlternativeUnit { get; set; }
        public decimal ConversionValue { get; set; }
        public decimal AltUnitBatchSize { get; set; }
        public decimal AltUnitStandardOutput { get; set; }
        public int AlternativeBom { get; set; }
        public string IsActive { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }
    }
}
