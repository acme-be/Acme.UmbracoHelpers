// -----------------------------------------------------------------------
//  <copyright file="PublishedContentExtensions.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Extensions
{
    using System;
    using System.Linq;

    using Umbraco.Core.Models;
    using Umbraco.Web;

    /// <summary>
    /// The published content extensions.
    /// </summary>
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Gets the fallback property value.
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>The property value</returns>
        public static T GetFallbackPropertyValue<T>(this IPublishedContent content, params string[] propertyNames)
        {
            var value = default(T);

            foreach (var propertyName in propertyNames)
            {
                value = content.GetPropertyValue<T>(propertyName);

                if (value != null && !value.Equals(default(T)))
                {
                    return value;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets the name of the title or.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The title or name</returns>
        public static string GetTitleOrName(this IPublishedContent content)
        {
            if (!content.HasProperty("title"))
            {
                return content.Name;
            }

            var title = content.GetPropertyValue<string>("title");

            if (string.IsNullOrWhiteSpace(title))
            {
                return content.Name;
            }

            return title;
        }
    }
}