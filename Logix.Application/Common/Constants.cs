using System.IO;

namespace Logix.Application.Common
{
    /// <summary>
    /// Language identifiers used across the application to select localization resources.
    /// </summary>
    public static class Languages
    {
        /// <summary>Arabic language identifier.</summary>
        public const int Ar = 1;

        /// <summary>English language identifier.</summary>
        public const int Eng = 2;
    }
    /// <summary>
    /// Constants related to SignalR hub routing (notification endpoint).
    /// </summary>
    public static class RouteHub
    {
        /// <summary>The route used to connect to the notification hub.</summary>
        public const string Route = "/notify";

    }

    /// <summary>
    /// Common file path constants used by the application for organizing generated files and temporary storage.
    /// Paths use <see cref="Path.DirectorySeparatorChar"/> to be OS-agnostic.
    /// </summary>
    public static class FilesPath
    {
        /// <summary>Temporary files directory name.</summary>
        public const string TempPath = "TempFiles";

        /// <summary>Top-level directory where application files are stored.</summary>
        public const string AllFiles = "AllFiles";

        /// <summary>
        /// Path to store ZATCA QR codes. Uses the system directory separator to build a cross-platform path.
        /// </summary>
        public static readonly string ZacatQrPath = $"AllFiles{Path.DirectorySeparatorChar}QRCode{Path.DirectorySeparatorChar}ZATCA";

        /// <summary>Path to store invoice QR codes.</summary>
        public static readonly string InvoiceQrPath = $"AllFiles{Path.DirectorySeparatorChar}QRCode{Path.DirectorySeparatorChar}Invoice";

        /// <summary>Path to store sales QR codes. (Used in FXA controllers.)</summary>
        public static readonly string SaleQrPath = $"AllFiles{Path.DirectorySeparatorChar}QRCode{Path.DirectorySeparatorChar}Sales"; // use in FXA/FxaAdditionsExclusionController

        /// <summary>Path to store fixed asset QR codes.</summary>
        public static readonly string FixedAssetQrPath = $"AllFiles{Path.DirectorySeparatorChar}QRCode{Path.DirectorySeparatorChar}FixedAssets";

        /// <summary>Path to store barcodes for fixed assets.</summary>
        public static readonly string FixedAssetBarCodePath = $"Files{Path.DirectorySeparatorChar}Barcode{Path.DirectorySeparatorChar}FixedAssets";

        /// <summary>Directory name for database backups.</summary>
        public static readonly string BackupDbPath = $"BackupDB";
    }

    /// <summary>
    /// Common session key names used to store temporary UI or workflow state for the current user session.
    /// Keys are string constants to avoid typos and centralize key names.
    /// </summary>
    public static class SessionKeys
    {
        public const string AddTempFiles = "AddTempFiles";
        public const string EditTempFiles = "EditTempFiles";
        public const string TempFilesAdd = "TempFilesAdd";
        public const string AddTempCustomerFiles = "AddTempCustomerFiles";
        public const string EditTempCustomerFiles = "EditTempCustomerFiles";
        public const string AddSalTransactionItems = "AddSalTransactionItems";
        public const string EditSalTransactionItems = "EditSalTransactionItems";
        public const string AddSalTransactionLocation = "AddSalTransactionLocation";
        public const string EditSalTransactionLocation = "EditSalTransactionLocation";
        public const string CopiesSalTransactionLocation = " CopiesSalTransactionLocation";


        public const string EditCustomerContacts = "EditCustomerContacts";

        public const string AddOpmContractItem = "AddOpmContractItem";
        public const string AddOpmContractPurchaseItem = "AddOpmContractPurchaseItem";

        public const string AddOpmContractLocation = "AddOpmContractLocation";
        public const string AddOpmContractPurchaseLocation = "AddOpmContractPurchaseLocation";

        public const string AddContractEmployee = "AddContractEmployee";
        public const string AddOPMPayroll = "AddOPMPayroll";
        public const string EditOPMPayroll = "EditOPMPayroll";
        public const string AddOPmPurchasesInvoiceitems = "AddOPmPurchasesInvoiceitems";
        public const string EditOPmPurchasesInvoiceitems = "EditOPmPurchasesInvoiceitems";
        public const string CopiesSalTransactionItems = "CopiesSalTransactionItems";
        public const string InvestEmployee = "InvestEmployee";
        public const string SubitemsDto = "SubitemsDto";
        public const string InitialCreditsAdd = "InitialCreditsAdd";
        public const string InitialCreditsEdit = "InitialCreditsEdit";
        public const string InitialCreditsRepetitiont = "InitialCreditsRepetitiont";
        public const string ReinforcementsAdd = "ReinforcementsAdd";
        public const string ReinforcementsEdit = "ReinforcementsEdit";
        public const string CostsitemsAdd = "CostsitemsAdd";
        public const string CostsitemsEdit = "CostsitemsEdit";
    }

    /// <summary>
    /// Types of accounts that a branch may use. Useful for mapping business logic to accounting categories.
    /// </summary>
    public enum BranchAccountType
    {
        None,
        Sales,
        Purchases,
        ReSales,
        RePurchases,
        Customer,
        Supplier,
        Sales_Discount,
        Purchases_Discount,
        Cost_Goods_Sold,
        Inventory_Transfer_Account
    }

    /// <summary>
    /// Data type identifiers used by dynamic fields and metadata to indicate how values should be handled.
    /// </summary>
    public enum DataTypeIdEnum
    {
        String = 1,
        Boolean = 2,
        Numeric = 3,
        Date = 4,
        PickList = 5,
        Longstring = 6,
        Title = 7,
        Time = 8,
        File = 9,
        Table = 10,
        Label = 11,
        Link = 12
    }
    
    /// <summary>
    /// Pagination defaults used throughout the application.
    /// </summary>
    public static class Pagination
    {
        /// <summary>Default page size for paginated endpoints.</summary>
        public const int take = 10;

        /// <summary>Default page size for drop-down list (DDL) endpoints.</summary>
        public const int DDLpageSize = 10;

    }

    /// <summary>
    /// Signal notification channel identifiers used by SignalR or internal messaging to identify message types.
    /// </summary>
    public static class SignalNOTIFICATION
    {
        /// <summary>Channel name used for new notifications.</summary>
        public const string NOTIFICATION_CHANNEL = "new_notification";

        /// <summary>Channel name used for permission-related notification messages.</summary>
        public const string PERMISSION_CHANNEL = "notification_permission";

        /// <summary>Channel name used for system-level notifications.</summary>
        public const string SYSTEM_CHANNEL = "notification_system";

        /// <summary>Channel name used for screen-specific notifications.</summary>
        public const string SCREEN_CHANNEL = "notification_screen";

    }

}
