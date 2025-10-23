using Microsoft.AspNetCore.Builder;
using System.Globalization;

namespace Logix.Application.Common
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Returns the list of cultures supported by the application/localization resources.
        /// </summary>
        /// <returns>List of supported <see cref="CultureInfo"/> instances.</returns>
        IList<CultureInfo> GetSupportedCultures();

        /// <summary>
        /// Configures request localization middleware on the provided application builder.
        /// </summary>
        /// <param name="app">The application builder to configure.</param>
        void ConfigureLocalization(IApplicationBuilder app);

        /// <summary>
        /// Retrieves a localized string from a named resource file.
        /// </summary>
        /// <param name="key">The resource key to lookup.</param>
        /// <param name="resource">The resource file or category name.</param>
        /// <param name="culture">Optional culture to use; when omitted the current request culture is used.</param>
        /// <returns>The localized string if found; otherwise the key or an empty string depending on implementation.</returns>
        string GetLocalizedResource(string key, string resource, CultureInfo culture = default);

        /// <summary>
        /// Retrieves a localized message by key from the application's messages resource.
        /// </summary>
        /// <param name="key">Message key.</param>
        /// <param name="culture">Optional culture override.</param>
        /// <returns>Localized message string.</returns>
        string GetMessagesResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from the 'Main' resource set.
        /// </summary>
        string GetMainResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from the Accounting (ACC) resource set.
        /// </summary>
        string GetAccResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from common/shared resources.
        /// </summary>
        string GetCommonResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from the HR module resources.
        /// </summary>
        string GetHrResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from the Project Management (PM) module resources.
        /// </summary>
        string GetPMResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from the Sales (SAL) module resources.
        /// </summary>
        string GetSALResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Generic resource accessor for resource set 1 (legacy/placeholder).
        /// </summary>
        string GetResource1(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from core/shared application resources.
        /// </summary>
        string GetCoreResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from inventory-related resources.
        /// </summary>
        string GetInventoryResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from purchasing-related resources.
        /// </summary>
        string GetPUResource(string key, CultureInfo culture = default);

        /// <summary>
        /// Retrieves localized values from SS resources (screen-specific or subsystem resources).
        /// </summary>
        string GetSSResources(string key, CultureInfo culture = default);
    }
}
