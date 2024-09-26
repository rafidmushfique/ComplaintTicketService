using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public partial class TblUserWiseBusinessAndPlantCode
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
    }
}
