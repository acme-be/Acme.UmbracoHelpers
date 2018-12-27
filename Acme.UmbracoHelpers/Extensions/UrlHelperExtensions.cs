// -----------------------------------------------------------------------
//  <copyright file="UrlHelperExtensions.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Acme.UmbracoHelpers.Images;

    /// <summary>
    /// Add extensions to the url helper
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Get the path with adding a crc to invalidate client cache
        /// </summary>
        /// <param name="helper">The current UrlHelper</param>
        /// <param name="path">The path of the file</param>
        /// <returns> path with adding a crc to invalidate client cache</returns>
        public static string GetPathWithCrc(this UrlHelper helper, string path)
        {
            var physicalPath = helper.RequestContext.HttpContext.Server.MapPath(path);
            var fileInfo = new FileInfo(physicalPath);

            if (!fileInfo.Exists)
            {
                return path;
            }

            if (path.IndexOf("?", StringComparison.Ordinal) == -1)
            {
                return path + "?v=" + fileInfo.LastWriteTimeUtc.Ticks;
            }

            return path + "&v=" + fileInfo.LastWriteTimeUtc.Ticks;
        }

        /// <summary>
        /// Get the path with adding a crc to invalidate client cache, add the format web p if available
        /// </summary>
        /// <param name="helper">The current UrlHelper</param>
        /// <param name="path">The path of the file</param>
        /// <returns> path with adding a crc to invalidate client cache</returns>
        public static string GetUrlWithCrcAndWebpFormat(this UrlHelper helper, string path)
        {
            var url = helper.GetPathWithCrc(path);

            if (WebpHelper.IsWebpSupported(HttpContext.Current, false))
            {
                url = $"{url}{(url.Contains("?") ? "&" : "?")}format=webp";
            }

            return url;
        }
    }
}