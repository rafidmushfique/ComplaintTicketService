using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblBusinessSetupInfo
    {
        public int Id { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
    }
}
