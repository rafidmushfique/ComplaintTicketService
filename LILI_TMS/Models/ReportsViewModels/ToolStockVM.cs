using System.ComponentModel;

namespace LILI_TMS.Models.ReportsViewModels
{
    public class ToolStockVM
    {
        [DisplayName("Sl No")]
        public int SlNo { get; set; }
        [DisplayName("Tool Code")]
        public string ToolCode {get;set;}
        [DisplayName("Tool Name")]
        public string ToolName { get;set;}
        [DisplayName("Brand")]
        public string Brand { get;set;}
        [DisplayName("Balance Quantity")]
        public int BalanceQty { get;set;}
        [DisplayName("Action Type")]
        public string ActionType { get;set;}
        [DisplayName("Comments")]
        public string Comments { get;set;}


    }
}
