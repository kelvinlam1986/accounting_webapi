using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.ViewModels
{
    public class ReceiptBatchViewModel
    {
        public string ReceiptBatchNo { get; set; }
        public DateTime ReceiptBatchDate { get; set; }
        public string DescriptionInVietNamese { get; set; }
        public bool BatchStatus { get; set; }
    }
}
