using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class AspNetRoleClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string RoleId { get; set; }
    }
}
