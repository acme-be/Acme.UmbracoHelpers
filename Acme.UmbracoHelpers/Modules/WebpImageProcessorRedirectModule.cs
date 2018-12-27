// -----------------------------------------------------------------------
//  <copyright file="WebpImageProcessorRedirectModule.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    using Acme.Core.Extensions;

    /// <summary>
    /// Http module that can redirect to the webp format of a file (301)
    /// </summary>
    public class WebpImageProcessorRedirectModule : IHttpModule
    {
        /// <summary>
        /// Availables extensions for webp
        /// </summary>
        private static readonly HashSet<string> ImageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png" };

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public void Init(HttpApplication context)
        {
            context.BeginRequest += this.ContextBeginRequest;
        }

        /// <summary>
        /// Check if request cann be served as webp
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">thi argument</param>
        private void ContextBeginRequest(object sender, EventArgs e)
        {
            if (this.IsWebpSupported(HttpContext.Current) && !this.IsFormatSpecified(HttpContext.Current))
            {
                var webpUrl = $"{HttpContext.Current.Request.Url.PathAndQuery}{(HttpContext.Current.Request.Url.PathAndQuery.Contains("?") ? "&" : "?")}format=webp";
                HttpContext.Current.Response.RedirectPermanent(webpUrl);
            }
        }

        /// <summary>
        /// Determines if webp is enabled on this query
        /// </summary>
        /// <param name="context">The context to check</param>
        /// <returns>True if webp is enabled</returns>
        private bool IsFormatSpecified(HttpContext context)
        {
            return context.Request.QueryString["format"] != null;
        }

        /// <summary>
        /// Determines if webp is supported on this query
        /// </summary>
        /// <param name="context">The context to check</param>
        /// <returns>True if webp can be used</returns>
        private bool IsWebpSupported(HttpContext context)
        {
            context.ThrowIfNull(nameof(context));

            var physicalPath = context.Server.MapPath(context.Request.FilePath);

            if (!File.Exists(physicalPath))
            {
                return false;
            }

            if (!ImageExtensions.Contains(Path.GetExtension(context.Request.FilePath)))
            {
                return false;
            }

            return context.Request.AcceptTypes?.Contains("image/webp") ?? false;
        }
    }
}