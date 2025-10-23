using Logix.Application.Extensions;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Domain.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Logix.MVC.Helpers
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;
        private readonly IAuthService authService;
        private readonly ConcurrentQueue<HttpContext> _requestQueue;
        private readonly SemaphoreSlim _semaphore;
        private int _language;

        public SessionMiddleware(RequestDelegate next,
            IConfiguration configuration,
            IAuthService authService
            )
        {
            _next = next;
            this.configuration = configuration;
            this.authService = authService;
            _requestQueue = new ConcurrentQueue<HttpContext>();
            _semaphore = new SemaphoreSlim(1);
            _language = 1;
        }

        public string CheckApiToken(HttpContext context)
        {
            string token = string.Empty;
            var chk = context.Request.Headers.TryGetValue("Authorization", out var values);
            if (chk)
            {
                token = values.FirstOrDefault() ?? "";
            }
            token = token.Replace("Bearer ", "");
            return token;
        }

        public string CheckMvcToken(HttpContext context)
        {
            string token = string.Empty;
            var chk = context.Request.Query.TryGetValue("jwt", out var values);
            if (chk)
            {
                token = values.FirstOrDefault() ?? "";
            }

            return token;
        }

        public async Task<bool> ValidateApiToken(string tokenString, HttpContext context)
        {
            try
            {
                //var integrationConfig = configuration.GetSection("IntegrationConfig");
                var secret = configuration["IntegrationConfig:IntegrationTokenKey"];
                var oldBaseUrl = configuration["IntegrationConfig:OldBaseUrl"];
                var coreBaseUrl = configuration["IntegrationConfig:CoreBaseUrl"];
                var key = Encoding.UTF8.GetBytes(secret);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = oldBaseUrl,
                    ValidateAudience = false,
                    ValidAudience = coreBaseUrl,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                };

                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);
                var username = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Username").Value ?? "";
                var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId").Value ?? "";
                var empId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "EmpId").Value ?? "";
                var groupId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "GroupId").Value ?? "";
                var facilityId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FacilityId").Value ?? "";
                var finYear = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FinYear").Value ?? "";
                var periodId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "PeriodId").Value ?? "";
                var language = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Language").Value ?? "";
                var branches = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Branches").Value ?? "";
                var branchId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "BranchId").Value ?? "";
                var currFinYearGregorian = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FinyearGregorian").Value ?? "";
                var CalendarType = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "CalendarType").Value ?? "";
                var LocationId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "LocationId").Value ?? "";
                var DeptId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "DeptId").Value ?? "";
                var SalesType = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "SalesType").Value ?? "";
                //var AzureIDToken = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "AzureIDToken").Value ?? "";
                var isAzureAuthenticated = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "IsAzureAuthenticated").Value ?? "0";
                var isAgree = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "isAgree").Value ?? "1";

                _language = Convert.ToInt32(language);

                //var currentUser = await authService.GetByName(username);
                //if (currentUser.Succeeded && currentUser.Data != null)
                //{
                //    // this will be removed when we resolve the functions that use it.
                //    var session = context.Session;
                //    session.AddData<SysUser>("user", currentUser.Data);
                //    return true;
                //}
                //return false;

                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidateMvcToken(string tokenString, HttpContext context)
        {
            try
            {
                //var integrationConfig = configuration.GetSection("IntegrationConfig");
                var secret = configuration["IntegrationConfig:IntegrationTokenKey"];
                var oldBaseUrl = configuration["IntegrationConfig:OldBaseUrl"];
                var coreBaseUrl = configuration["IntegrationConfig:CoreBaseUrl"];
                var key = Encoding.UTF8.GetBytes(secret);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = oldBaseUrl,
                    ValidateAudience = false,
                    ValidAudience = coreBaseUrl,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                };

                var session = context.Session;
                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);
                var username = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Username").Value ?? "";
                var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId").Value ?? "";
                var empId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "EmpId").Value ?? "";
                var groupId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "GroupId").Value ?? "";
                var facilityId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FacilityId").Value ?? "";
                var finYear = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FinYear").Value ?? "";
                var periodId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "PeriodId").Value ?? "";
                var language = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Language").Value ?? "";
                var branches = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Branches").Value ?? "";
                var branchId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "BranchId").Value ?? "";
                var currFinYearGregorian = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FinyearGregorian").Value ?? "";
                var CalendarType = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "CalendarType").Value ?? "";
                var LocationId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "LocationId").Value ?? "";
                var DeptId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "DeptId").Value ?? "";
                var SalesType = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "SalesType").Value ?? "";
                //var AzureIDToken = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "AzureIDToken").Value ?? "";
                var isAzureAuthenticated = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "IsAzureAuthenticated").Value ?? "0";
                var isAgree = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "isAgree").Value ?? "1";
                session.AddData<long>("UserId", long.Parse(userId));
                session.AddData<int>("EmpId", int.Parse(empId));
                session.AddData<int>("GroupId", int.Parse(groupId));
                session.AddData<long>("FacilityId", long.Parse(facilityId));
                session.AddData<long>("FinYear", long.Parse(finYear));
                session.AddData<long>("PeriodId", long.Parse(periodId));
                session.AddData<int>("Language", int.Parse(language));
                session.AddData<string>("OldBaseUrl", oldBaseUrl);
                session.AddData<string>("CoreBaseUrl", coreBaseUrl);
                session.AddData<string>("Branches", branches);
                session.AddData<string>("BranchId", branchId);
                session.AddData<int>("FinyearGregorian", int.Parse(currFinYearGregorian));
                session.AddData<string>("CalendarType", CalendarType);
                session.AddData<string>("LocationId", LocationId);
                session.AddData<string>("DeptId", DeptId);
                session.AddData<string>("SalesType", SalesType);
                session.AddData<int>("IsAzureAuthenticated", Convert.ToInt32(isAzureAuthenticated));
                session.AddData<bool>("isAgree", Convert.ToBoolean(isAgree));
                var currentUser = await authService.GetByName(username);
                if (currentUser.Succeeded && currentUser.Data != null)
                {
                    session.AddData<SysUser>("user", currentUser.Data);
                    return true;
                }
                return false;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task ProcessRequestAsync(HttpContext context, string token)
        {

            // Bypass SessionMiddleware for SignalR endpoints (/notify, /notifyHub and subpaths)
            if (context.Request.Path.StartsWithSegments("/notify", StringComparison.OrdinalIgnoreCase)
                || context.Request.Path.StartsWithSegments("/notifyHub", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // Perform request processing asynchronously
            /*if (!context.Request.IsHttps)
            {
                // Redirect to the HTTPS equivalent URL
                var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                context.Response.Redirect(httpsUrl);
                return;
            }*/
            // الحصول على نقطة النهاية (endpoint)
            var endpoint = context.GetEndpoint();
            // التحقق مما إذا كانت نقطة النهاية تحتوي على خاصية AllowAnonymous للسماح بالوصول بدون تحقق
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context); return;
            }

            if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
            {
                var api = CheckApiToken(context);
                var session = context.Session;
                session.Clear(); // redhwan 2025_03_05
                if (!string.IsNullOrEmpty(api))
                {
                    var authenticated = await ValidateApiToken(api, context);
                    if (!authenticated)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                else if (!context.Request.Path.StartsWithSegments(ApiConfig.ApiAuthUrl, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                if (_language == 1)
                {
                    CultureInfo.CurrentCulture = new CultureInfo("ar-SA");
                    CultureInfo.CurrentUICulture = new CultureInfo("ar-SA");
                }
                else
                {
                    CultureInfo.CurrentCulture = new CultureInfo("en-US");
                    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                }
            }
            else
            {
                var t = CheckMvcToken(context);
                if (!string.IsNullOrEmpty(t))
                {
                    var authenticated = await ValidateMvcToken(t, context);
                    if (authenticated)
                    {
                    }
                }

                // Check if session middleware is available
                var session = context.Session;
                if (session == null && !context.Request.Path.Equals("/Account/Login", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to the login view if session is not available
                    //context.Response.Redirect("/Account/Login");
                    context.Response.Redirect($"{context.Request.Scheme}://{context.Request.Host}/swagger/index.html");
                    return;
                }
                var user = session.GetData<SysUser>("user");

                var Language = 1; // اللغة الافتراضية
                Language = session.GetData<int>("Language");
                if (Language == 1)
                {
                    CultureInfo.CurrentCulture = new CultureInfo("ar-SA");
                    CultureInfo.CurrentUICulture = new CultureInfo("ar-SA");
                }
                else
                {
                    CultureInfo.CurrentCulture = new CultureInfo("en-US");
                    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                }

                if (user == null && !context.Request.Path.Equals("/Account/Login", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to the login view if session is not available
                    //context.Response.Redirect("/Account/Login");
                    context.Response.Redirect($"{context.Request.Scheme}://{context.Request.Host}/swagger/index.html");
                    return;
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }

        public async Task Invoke(HttpContext context)
        {

            // Fast-path bypass: avoid queue/lock for SignalR and internal health checks
            if (context.Request.Path.StartsWithSegments("/notify", StringComparison.OrdinalIgnoreCase)
                || context.Request.Path.StartsWithSegments("/notifyHub", StringComparison.OrdinalIgnoreCase)
                || context.Request.Headers.ContainsKey("X-Bypass-Session"))
            {
                await _next(context);
                return;
            }
            // Enqueue the current request
            _requestQueue.Enqueue(context);

            // Acquire the semaphore to ensure exclusive access
            await _semaphore.WaitAsync();

            try
            {
                // Dequeue and process each request in a separate thread
                while (_requestQueue.TryDequeue(out var queuedContext))
                {
                    await ProcessRequestAsync(queuedContext, "");
                }
            }
            finally
            {
                // Release the semaphore to allow other requests to be processed
                _semaphore.Release();
            }
            // ====== this code used when publish to server ============
            /*if (!context.Request.IsHttps)
            {
                // Redirect to the HTTPS equivalent URL
                var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                context.Response.Redirect(httpsUrl);
                return;
            }*/
            // Create a list to store the tasks
            /* var tasks = new List<Task>();

             // Process the first request asynchronously
             tasks.Add(ProcessRequestAsync(context, ""));

             // Process the second request asynchronously
             tasks.Add(ProcessRequestAsync(context, ""));

             // Await all tasks to complete
             await Task.WhenAll(tasks);*/
        }

    }
}