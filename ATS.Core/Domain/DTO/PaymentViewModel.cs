using ATS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Core.Domain.DTO
{
    public class PaymentViewModel
    {
        public Int64 ID { get; set; }
        public decimal? TotalPayable { get; set; }
        public decimal? Pay { get; set; }
        public Nullable<PaymentType> PaymentMode { get; set; }
        public decimal? TotalAmountPaid { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public long? DiscountType { get; set; }
        public string Comment { get; set; }
        public string PaymentAccountNo { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }

        public string Date { get; set; }
        public string MeterNo { get; set; }
        public Nullable<Int64> MeterID { get; set; }
        public string Mobile { get; set; }
        public long? SahalAccountNoID { get; set; }
        public long? EDahabAccountNoID { get; set; }
        public long? CustomerID { get; set; }
        public int TotalRecords { get; set; }
        public string SenderMobile { get; set; }
        public string CashierName { get; set; }
        public decimal? Taxamount { get; set; }
        public decimal? balanceAmount { get; set; }
        public decimal? SumOfTotalAmountPaid { get; set; }
        public decimal? SumOfDiscountAmount { get; set; }
        public decimal? SumOfTaxAmount { get; set; }
        public decimal? SumOfBalaceAmount { get; set; }
    }
}
