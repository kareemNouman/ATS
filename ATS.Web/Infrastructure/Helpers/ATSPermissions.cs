using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ATS.Web.Infrastructure.Helpers
{
    public class ATSPermissions
    {
        public const string CustomerRead = "Customer-Read";
        public const string CustomerCreate = "Customer-Create";
        public const string CustomerUpdate = "Customer-Update";



        public const string EmployeeRead = "Employee-Read";
        public const string EmployeeCreate = "Employee-Create";
        public const string EmployeeUpdate = "Employee-Update";


        public const string MeterRead = "Meter-Read";
        public const string MeterCreate = "Meter-Create";
        public const string MeterUpdate = "Meter-Update";

        public const string AddGracePeriod = "AddGracePeriod";
        public const string ExtentGracePeriod = "ExtentGracePeriod";


        public const string State = "State";
        public const string City = "City";
        public const string BillPlan = "BillPlan";
        public const string Charges = "Charges";
        public const string ConnectionCategory = "ConnectionCategory";
        public const string Deduction = "Deduction";
        public const string Department = "Department";
        public const string Discount = "Discount";
        public const string Group = "Group";
        public const string MeterType = "MeterType";
        public const string Zone = "Zone";
        public const string Tax = "Tax";
        public const string SahalMaster = "SahalMaster";
        public const string EDahabMaster = "EDahabMaster";
        public const string OtherCharges = "OtherCharges";
        public const string Role = "Role";



        public const string EmailTemplate = "EmailTemplate";
        public const string SMSTemplate = "SMSTemplate";
        public const string AddSponsors = "AddSponsors";


        public const string ManualBilling = "ManualBilling";
        public const string SingleBill = "SingleBill";
        public const string BulkUpload = "BulkUpload";
        public const string BillGeneration = "BillGeneration";

        public const string Payment = "Payment";
        public const string PaymentEdit = "PaymentEdit";
        public const string PaymentConfirmation = "PaymentConfirmation";
        public const string PaymentDiscount = "PaymentDiscount";

        public const string AuditLog = "AuditLog";

        public const string Permission = "Permission";

        public const string Reports = "Reports";

        public const string EmployeePaymentRead = "EmployeePayment-Read";
        public const string EmployeePaymentCreate = "EmployeePayment-Create";


        public const string ChangePassword = "ChangePassword";

        public const string Page = "Page";
        public const string EditBill = "EditBill";
        public const string MergeMeter = "MergeMeter";

        public const string EmployeeChangePassword = "EmployeeChangePassword";


        #region Reports

        public const string ReceivableReport = "ReceivableReport";

        public const string SalaryReport = "SalaryReport";

        public const string BillsReport = "BillsReport";

        public const string ConsumptionReport = "ConsumptionReport";

        public const string BillPaymentReport = "BillPaymentReport";

        public const string MeterReaderReport = "MeterReaderReport";

        public const string AccountDetailReport = "AccountDetailReport";


        public const string ConnectionReport = "ConnectionReport";


        public const string ConnectionandDisconnectionReport = "ConnectionandDisconnectionReport";

        public const string ReaderPerformanceReport = "ReaderPerformanceReport";

        public const string IncreaseDecreaseReport = "IncreaseDecreaseReport";

        public const string AccountantReport = "AccountantReport";

        public const string CustomerStatementReport = "CustomerStatementReport";

        public const string SummaryReport = "SummaryReport";

        public const string BulkInvoiceReport = "BulkInvoiceReport";


        public const string OtherChargesReport = "OtherChargesReport";

        public const string BillGenerationReport = "BillGenerationReport";


        public const string BillDiscountReport = "BillDiscountReport";

        public const string PaymentDiscountReport = "PaymentDiscountReport";


        public const string CustomerSummaryReport = "CustomerSummaryReport";
        public const string NetbalanceReprot = "Netbalancereport";

        #endregion



        //public async Task<List<string>> GetPermissions()
        //{
        //    var permissions = HttpContext.Current.Session["permissions"] as List<string>;

        //    if (permissions != null)
        //        return permissions;

        //    var response = await new MastersService().GetPermissions();

        //    permissions = response.Data as List<string>;
        //    permissions = permissions ?? new List<string>();

        //    HttpContext.Current.Session["permissions"] = permissions;
        //    return permissions;
        //}

    }
}