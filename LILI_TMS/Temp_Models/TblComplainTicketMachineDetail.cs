using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblComplainTicketMachineDetail
    {
        public int Id { get; set; }
        public string TicketNo { get; set; }
        public string MachineCode { get; set; }

        public virtual TblMachineSetup MachineCodeNavigation { get; set; }
        public virtual TblComplainTicket TicketNoNavigation { get; set; }
    }
}
