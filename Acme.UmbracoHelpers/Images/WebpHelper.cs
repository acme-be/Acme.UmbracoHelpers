// -----------------------------------------------------------------------
//  <copyright file="WebpHelper.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Images
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    using Acme.Core.Extensions;

    /// <summary>
    /// Provides some help for webp
    /// </summary>
    internal static class WebpHelper
    {
        /// <summary>
        /// Availables extensions for webp
        /// </summary>
        private static readonly HashSet<string> ImageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png" };

        /// <summary>
        /// Determines if webp is enabled on this query
        /// </summary>
        /// <param name="context">The context to check</param>
        /// <returns>True if webp is enabled</returns>
        public static bool IsFormatSpecified(HttpContext context)
        {
            return context.Request.QueryString["format"] != null;
        }

        /// <summary>
        /// Determines if webp is supported on this query
        /// </summary>
        /// <param name="context">The context to check</param>
        /// <param name="checkFile">Perform a file check, default : true</param>
        /// <returns>True if webp can be used</returns>
        public static bool IsWebpSupported(HttpContext context, bool checkFile = true)
        {
            context.ThrowIfNull(nameof(context));

            if (checkFile)
            {
                var physicalPath = context.Server.MapPath(context.Request.FilePath);

                if (!File.Exists(physicalPath))
                {
                    return false;
                }

                if (!ImageExtensions.Contains(Path.GetExtension(context.Request.FilePath)))
                {
                    return false;
                }
            }

            return context.Request.AcceptTypes?.Contains("image/webp") ?? false;
        }
    }
}