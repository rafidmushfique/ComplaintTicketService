using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblCountry
    {
        public TblCountry()
        {
            //TblSubmissionItemManufacturerCountry = new HashSet<TblSubmissionItem>();
            //TblSubmissionItemSupplierCountry = new HashSet<TblSubmissionItem>();
        }

        public long? Id { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }

        //public ICollection<TblSubmissionItem> TblSubmissionItemManufacturerCountry { get; set; }
        //public ICollection<TblSubmissionItem> TblSubmissionItemSupplierCountry { get; set; }
    }
}
