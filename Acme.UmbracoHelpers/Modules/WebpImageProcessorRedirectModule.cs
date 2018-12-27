// -----------------------------------------------------------------------
//  <copyright file="WebpImageProcessorRedirectModule.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Modules
{
    using System;
    using System.Linq;
    using System.Web;

    using Acme.UmbracoHelpers.Images;

    /// <summary>
    /// Http module that can redirect to the webp format of a file (301)
    /// </summary>
    public class WebpImageProcessorRedirectModule : IHttpModule
    {
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
            if (WebpHelper.IsWebpSupported(HttpContext.Current) && !WebpHelper.IsFormatSpecified(HttpContext.Current))
            {
                var webpUrl = $"{HttpContext.Current.Request.Url.PathAndQuery}{(HttpContext.Current.Request.Url.PathAndQuery.Contains("?") ? "&" : "?")}format=webp";
                HttpContext.Current.Response.RedirectPermanent(webpUrl);
            }
        }
    }
}