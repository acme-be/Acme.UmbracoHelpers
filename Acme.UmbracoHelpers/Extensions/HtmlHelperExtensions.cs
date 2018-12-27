// -----------------------------------------------------------------------
//  <copyright file="HtmlHelperExtensions.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Extensions
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The html helper extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Gets the absolute URL.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="path">The path.</param>
        /// <returns>The absolute url in string</returns>
        public static string GetAbsoluteUrl(this HtmlHelper helper, string path)
        {
            var absoluteUri = new Uri(HttpContext.Current.Request.Url, path);
            return absoluteUri.ToString();
        }
    }
}