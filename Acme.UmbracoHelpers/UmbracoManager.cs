// -----------------------------------------------------------------------
//  <copyright file="UmbracoManager.cs" company="Prism">
//  Copyright (c) Prism. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;

namespace Acme.UmbracoHelpers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    using Acme.Core.Extensions;

    using Newtonsoft.Json;

    using Umbraco.Core;
    using Umbraco.Core.Configuration;
    using Umbraco.Core.Security;
    using Umbraco.Web;
    using Umbraco.Web.Routing;
    using Umbraco.Web.Security;

    /// <summary>
    /// Usefull methods to manage Ubmraco :)
    /// </summary>
    public static class UmbracoManager
    {
        /// <summary>
        /// Gets the umbraco helper.
        /// </summary>
        /// <value>The umbraco helper.</value>
        public static UmbracoHelper UmbracoHelper
        {
            get
            {
                if (!(HttpContext.Current.Items["UmbracoHelper"] is UmbracoHelper helper))
                {
                    EnsureContext();

#pragma warning disable WVN008
                    helper = new UmbracoHelper(UmbracoContext.Current);
#pragma warning restore WVN008
                    HttpContext.Current.Items.Add("UmbracoHelper", helper);
                }

                return helper;
            }
        }

        /// <summary>
        /// Gets the dictionary value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The translated content for the key or key if not found</returns>
        public static string GetDictionaryValue(string key, CultureInfo culture)
        {
            key.ThrowIfNull(nameof(key));

            EnsureContext();

#pragma warning disable WVN010 // Avoid use of IService.
            var dictionaryItem = UmbracoContext.Current.Application.Services.LocalizationService.GetDictionaryItemByKey(key);
#pragma warning restore WVN010 // Avoid use of IService.

            if (dictionaryItem != null)
            {
                var translation = dictionaryItem.Translations.SingleOrDefault(x => x.Language.CultureInfo.Equals(culture));
                if (translation != null)
                {
                    return translation.Value;
                }

                var iso2Lang = culture.Name.Substring(0, 2);
                translation = dictionaryItem.Translations.SingleOrDefault(x => x.Language.CultureInfo.Name.StartsWith(iso2Lang, StringComparison.OrdinalIgnoreCase));
                if (translation != null)
                {
                    return translation.Value;
                }
            }

            return key;
        }

        /// <summary>
        /// Gets the umbraco back office identity.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The umbraco back office identity.</returns>
        public static UmbracoBackOfficeIdentity GetUmbracoBackOfficeIdentity(this HttpContextBase httpContext)
        {
            var ticket = httpContext.GetUmbracoAuthTicket();
            if (ticket?.Expired == false && httpContext.RenewUmbracoAuthTicket())
            {
                try
                {
                    return new UmbracoBackOfficeIdentity(ticket);
                }
                catch (Exception exception) when (exception is FormatException || exception is JsonReaderException)
                {
                    // this will occur if the cookie data is invalid
                    httpContext.UmbracoLogout();
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the user has umbraco back office access with the specified <paramref name="roles" />.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// <c>true</c> if the user has umbraco back office access with the specified <paramref name="roles" />; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool HasUmbracoBackOfficeAccess(this HttpContextBase httpContext, params string[] roles)
        {
            var identity = httpContext.GetUmbracoBackOfficeIdentity();
            if (identity == null)
            {
                return false;
            }

            return roles == null || roles.Length == 0 || roles.All(r => identity.AllowedApplications.Contains(r, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether the specified request is an umbraco request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// <c>true</c> if the specified request is an umbraco request; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUmbracoRequest(this HttpRequestBase request)
        {
            return request.Url?.AbsolutePath.StartsWith("/umbraco", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Ensures the context.
        /// </summary>
        internal static void EnsureContext()
        {
            var contextWrapper = new HttpContextWrapper(HttpContext.Current);
            var webSecurity = new WebSecurity(contextWrapper, ApplicationContext.Current);
            UmbracoContext.EnsureContext(contextWrapper, ApplicationContext.Current, webSecurity, UmbracoConfig.For.UmbracoSettings(), UrlProviderResolver.Current.Providers, false);
        }
    }
}