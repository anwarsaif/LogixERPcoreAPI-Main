using DocumentFormat.OpenXml.Drawing.Charts;
using Logix.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace Logix.Application.Common
{
    /// <summary>
    /// Session abstraction intended for MVC usage (DevExpress report designer uses this).
    /// Note: This interface is not designed for API endpoints; API code should not rely on server-side MVC session state.
    /// </summary>
    public interface IMvcSession
    {
        /// <summary>
        /// Clears all values from the current session.
        /// </summary>
        /// <returns>True when the clear succeeds; false when an exception is thrown.</returns>
        bool ClearSession();

        /// <summary>
        /// Retrieves a typed value from session by key.
        /// </summary>
        /// <typeparam name="T">The expected type of the stored value.</typeparam>
        /// <param name="key">Session key.</param>
        /// <returns>The stored value converted to <typeparamref name="T"/> or default(T) if not present.</returns>
        T GetData<T>(string key);

        /// <summary>
        /// Adds or updates a value in session under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the value to store.</typeparam>
        /// <param name="key">Session key.</param>
        /// <param name="value">Value to store.</param>
        void AddData<T>(string key, T value);

        /// <summary>
        /// Convenience method to populate the main set of session values used throughout the MVC application.
        /// Stores user, employee, facility, language and other context values in session.
        /// </summary>
        /// <param name="userId">Logged-in user identifier.</param>
        /// <param name="empId">Employee identifier.</param>
        /// <param name="groupId">User's group id.</param>
        /// <param name="facilityId">Facility identifier.</param>
        /// <param name="finYear">Financial year identifier.</param>
        /// <param name="language">Language id for localization.</param>
        /// <param name="branches">Comma-separated branch identifiers.</param>
        /// <param name="calendartype">Calendar type string (e.g., Gregorian/Hijri).</param>
        /// <param name="branchId">Current branch id.</param>
        /// <param name="FinyearGregorian">Gregorian financial year (int).</param>
        /// <param name="LocationId">Location identifier.</param>
        /// <param name="DeptId">Department identifier.</param>
        /// <param name="SalesType">Sales type code.</param>
        /// <param name="periodId">Active period identifier.</param>
        /// <param name="IsAzureAuthenticated">Flag indicating Azure AD authentication state.</param>
        /// <param name="isAgree">Flag indicating user agreement/consent state.</param>
        void SetMainData(long userId, int empId, int groupId, long facilityId, long finYear, int language, string branches, string calendartype, long branchId, int FinyearGregorian, int LocationId, int DeptId, int SalesType, long periodId, int IsAzureAuthenticated = 0, bool isAgree = true);

        /// <summary>
        /// The current logged-in user's id stored in session.
        /// </summary>
        long UserId { get; }

        /// <summary>
        /// Employee id associated with the current user session.
        /// </summary>
        int EmpId { get; }

        long FacilityId { get; }
        long FinYear { get; }
        int GroupId { get; }
        int Language { get; }
        long PeriodId { get; }
        string Branches { get; }
        string OldBaseUrl { get; }
        string CoreBaseUrl { get; }
        string CalendarType { get; }
        long BranchId { get; }
        int FinyearGregorian { get; }
        int DeptId { get; }
        int LocationId { get; }
        int SalesType { get; }
        int IsAzureAuthenticated { get; }
        bool isAgree { get; }
    }

    public class MvcSession : IMvcSession
    {
        private readonly ISession _session;

        // Each property reads a typed value from the ASP.NET Core session store.
        // The helper extension methods (.GetData<T>) perform the (de)serialization.
        public long UserId => _session.GetData<long>("UserId");
        public int EmpId => _session.GetData<int>("EmpId");
        public long FacilityId => _session.GetData<long>("FacilityId");
        public long FinYear => _session.GetData<long>("FinYear");
        public int GroupId => _session.GetData<int>("GroupId");
        public int Language => _session.GetData<int>("Language");
        public long PeriodId => _session.GetData<long>("Period");
        public string Branches => _session.GetData<string>("Branches");
        public string OldBaseUrl => _session.GetData<string>("OldBaseUrl");
        public string CoreBaseUrl => _session.GetData<string>("CoreBaseUrl");
        public string CalendarType => _session.GetData<string>("CalendarType");
        public long BranchId => _session.GetData<long>("BranchId");
        public int FinyearGregorian => _session.GetData<int>("FinyearGregorian");
        public int DeptId => _session.GetData<int>("DeptId");
        public int LocationId => _session.GetData<int>("LocationId");
        public int SalesType => _session.GetData<int>("SalesType");
        public int IsAzureAuthenticated => _session.GetData<int>("IsAzureAuthenticated");
        public bool isAgree => _session.GetData<bool>("isAgree");

        /// <summary>
        /// Constructs a session wrapper using the provided IHttpContextAccessor.
        /// The implementation is defensive: when the accessor or HttpContext is null the internal _session remains null.
        /// </summary>
        public MvcSession(IHttpContextAccessor httpContextAccessor)
        {
            // Defensive null-checks to avoid NullReferenceException during construction in non-HTTP contexts.
            if (httpContextAccessor != null)
            {
                if (httpContextAccessor.HttpContext != null)
                {
                    _session = httpContextAccessor.HttpContext.Session;
                }
            }
        }

        /// <summary>
        /// Retrieves a typed value from session using the shared extension method.
        /// </summary>
        public T GetData<T>(string key)
        {
            // Inline comment: delegate to the session extension which handles lookup and deserialization.
            return _session.GetData<T>(key);
        }

        /// <summary>
        /// Adds or updates a typed value into the session using the shared extension method.
        /// </summary>
        public void AddData<T>(string key, T value)
        {
            // Inline comment: serialize and store the value in the session store.
            _session.AddData<T>(key, value);
        }

        /// <summary>
        /// Sets the main application-related session values in a single call.
        /// The order of AddData calls mirrors the expected keys consumed elsewhere in the MVC app.
        /// </summary>
        public void SetMainData(long userId, int empId, int groupId, long facilityId, long finYear, int language, string branches, string calendartype, long branchId, int FinyearGregorian, int LocationId, int DeptId, int SalesType, long periodId, int IsAzureAuthenticated = 0, bool isAgree = true)
        {
            // Store each important context value in session for later retrieval by views/reports.
            _session.AddData<long>("UserId", userId);
            _session.AddData<int>("EmpId", empId);
            _session.AddData<int>("GroupId", groupId);
            _session.AddData<long>("FacilityId", facilityId);
            _session.AddData<long>("FinYear", finYear);
            _session.AddData<int>("Language", language);
            _session.AddData<long>("PeriodId", periodId);
            _session.AddData<string>("Branches", branches);
            _session.AddData<long>("BranchId", branchId);
            _session.AddData<int>("FinyearGregorian", FinyearGregorian);
            // Note: CalendarType parameter is provided as 'calendartype', but original code wrote CalendarType (property)
            // Keep the intended behavior: store the provided calendartype argument.
            _session.AddData<string>("CalendarType", calendartype);
            _session.AddData<int>("LocationId", LocationId);
            _session.AddData<int>("DeptId", DeptId);
            _session.AddData<int>("SalesType", SalesType);
            _session.AddData<int>("IsAzureAuthenticated", IsAzureAuthenticated);
            _session.AddData<bool>("isAgree", isAgree);

            // The commented-out OldBaseUrl/CoreBaseUrl were intentionally left as comments in the original.
            //_session.AddData<string>("OldBaseUrl", oldBaseUrl);
            //_session.AddData<string>("CoreBaseUrl", coreBaseUrl);
        }

        /// <summary>
        /// Clears the session and returns a boolean status. Exceptions are swallowed and false is returned
        /// to keep callers simple (this mirrors the previous behavior).
        /// </summary>
        public bool ClearSession()
        {
            try
            {
                // Main statement: clear all session keys.
                _session.Clear();
                return true;
            }
            catch (Exception)
            {
                // Inline note: swallowing exceptions keeps calling code simple but hides failure reasons.
                // Consider logging the exception if visibility is desired.
                return false;
            }
        }
    }
}
