using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Logix.Application.Common
{
    /// <summary>
    /// Provides access to current execution context data (user, facility, language, etc.) extracted from JWT claims.
    /// Implementations read claims from the current HTTP context and expose them as strongly-typed properties.
    /// </summary>
    public interface ICurrentData
    {
        /// <summary>Identifier of the current user.</summary>
        long UserId { get; }
        /// <summary>Employee id associated with the current user (if any).</summary>
        int EmpId { get; }
        /// <summary>Facility identifier for the current context.</summary>
        long FacilityId { get; }
        /// <summary>Financial year identifier.</summary>
        long FinYear { get; }
        /// <summary>User group identifier.</summary>
        int GroupId { get; }
        /// <summary>Current language identifier.</summary>
        int Language { get; }
        /// <summary>Current period id.</summary>
        long PeriodId { get; }
        /// <summary>Comma-separated branch list the user has access to.</summary>
        string Branches { get; }
        /// <summary>Old base URL for backward compatibility scenarios.</summary>
        string OldBaseUrl { get; }
        /// <summary>Core base URL used by the application.</summary>
        string CoreBaseUrl { get; }
        /// <summary>Calendar type identifier (e.g., Hijri/Gregorian).</summary>
        string CalendarType { get; }
        /// <summary>Current branch identifier.</summary>
        long BranchId { get; }
        /// <summary>Financial year in Gregorian format.</summary>
        int FinyearGregorian { get; }
        /// <summary>Current department id.</summary>
        int DeptId { get; }
        /// <summary>Current location id.</summary>
        int LocationId { get; }
        /// <summary>Sales type for current context.</summary>
        int SalesType { get; }
        /// <summary>Flag indicating whether Azure authentication was used.</summary>
        int IsAzureAuthenticated { get; }
        /// <summary>User agreement flag.</summary>
        bool isAgree { get; }
        /// <summary>Default date format used in the application.</summary>
        string DateFomet { get; }

    }

    /// <summary>
    /// Default implementation of <see cref="ICurrentData"/> that reads values from JWT claims contained
    /// in the current HTTP request. Supports both Authorization header tokens (API) and a "jwt" query parameter (MVC).
    /// </summary>
    public class CurrentData : ICurrentData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal _userClaims;

        /// <summary>
        /// Constructs the current data helper using an <see cref="IHttpContextAccessor"/> to access the request.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor used to read request headers and query string.</param>
        public CurrentData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            // Initialize the cached claims principal on construction to avoid repeated parsing.
            _userClaims = GetUserClaims();
        }

        // Backing cached ClaimsPrincipal used by helper methods.
        private ClaimsPrincipal UserClaims => _userClaims ??= GetUserClaims();

        /// <summary>
        /// Extracts the JWT token from the current request and returns a <see cref="ClaimsPrincipal"/>.
        /// Behavior:
        /// - If the request path starts with "/api" the code checks the Authorization header for a token.
        /// - Otherwise it checks the query string for a "jwt" parameter (useful for MVC pages / DevExpress reports).
        /// If parsing fails or no token is present an empty principal is returned.
        /// </summary>
        /// <returns>A ClaimsPrincipal populated from the JWT claims or an empty principal when no valid token is found.</returns>
        private ClaimsPrincipal GetUserClaims()
        {
            // If this is an API request attempt to read the token from the Authorization header.
            if (_httpContextAccessor.HttpContext?.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) == true)
            {
                var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                // Some tooling (Swagger UI) may supply the token without the "Bearer " prefix, so only check existence here.
                if (string.IsNullOrEmpty(authHeader))
                {
                    // No token provided - return an empty principal so callers receive default values.
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }

                // Remove an optional "Bearer " prefix if present.
                var token = authHeader.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();

                try
                {
                    // Parse the JWT without validating (just extract claims). If parsing fails we fallback to empty principal.
                    var jwtToken = handler.ReadJwtToken(token);
                    return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims));
                }
                catch
                {
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }
            }
            else
            {
                // Non-API requests: look for the token in the query string under the "jwt" parameter (DevExpress reports, etc.).
                string token = string.Empty;
                var query = _httpContextAccessor.HttpContext?.Request.Query;

                StringValues values = ""; // placeholder for TryGetValue out parameter
                var chk = query?.TryGetValue("jwt", out values) ?? false;
                if (chk == true)
                {
                    token = values.FirstOrDefault() ?? "";

                    var handler2 = new JwtSecurityTokenHandler();

                    try
                    {
                        var jwtToken = handler2.ReadJwtToken(token);
                        return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims));
                    }
                    catch
                    {
                        return new ClaimsPrincipal(new ClaimsIdentity());
                    }
                }
                else
                {
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }
            }
        }

        /// <summary>
        /// Helper to read a claim value as string; returns "-1" if the claim is missing.
        /// </summary>
        private string GetClaimValue(string claimType) => UserClaims.FindFirst(claimType)?.Value ?? "-1";

        /// <summary>
        /// Helper to read an integer claim; returns -1 when parsing fails or claim missing.
        /// </summary>
        private int GetClaimValueInt(string claimType) => int.TryParse(GetClaimValue(claimType), out int value) ? value : -1;

        /// <summary>
        /// Helper to read a long claim; returns -1 when parsing fails or claim missing.
        /// </summary>
        private long GetClaimValueLong(string claimType) => long.TryParse(GetClaimValue(claimType), out long value) ? value : -1;

        /// <summary>
        /// Helper to read a boolean claim; returns false when parsing fails or claim missing.
        /// </summary>
        private bool GetClaimValueBool(string claimType) => bool.TryParse(GetClaimValue(claimType), out bool value) && value;

        // Expose strongly-typed properties by reading from claims using the helper methods above.
        public long UserId => GetClaimValueLong("UserId");
        public int EmpId => GetClaimValueInt("EmpId");
        public long FacilityId => GetClaimValueLong("FacilityId");
        public long FinYear => GetClaimValueLong("FinYear");
        public int GroupId => GetClaimValueInt("GroupId");
        public int Language => GetClaimValueInt("Language");
        public string OldBaseUrl => GetClaimValue("OldBaseUrl");
        public string CoreBaseUrl => GetClaimValue("CoreBaseUrl");
        public long PeriodId => GetClaimValueLong("PeriodId");
        public string Branches => GetClaimValue("Branches");
        public string CalendarType => GetClaimValue("CalendarType");
        public long BranchId => GetClaimValueLong("BranchId");
        public int FinyearGregorian => GetClaimValueInt("FinyearGregorian");
        public int DeptId => GetClaimValueInt("DeptId");
        public int LocationId => GetClaimValueInt("LocationId");
        public int SalesType => GetClaimValueInt("SalesType");
        public int IsAzureAuthenticated => GetClaimValueInt("IsAzureAuthenticated");
        public bool isAgree => GetClaimValueBool("isAgree");
        public string DateFomet => "yyyy/MM/dd";

    }
}