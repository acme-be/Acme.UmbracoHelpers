// -----------------------------------------------------------------------
//  <copyright file="PublishedContentExtensions.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;

    using Acme.UmbracoHelpers.Images;

    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;
    using Umbraco.Web.Models;

    /// <summary>
    /// The published content extensions.
    /// </summary>
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Gets the ImageProcessor Url from the IPublishedContent item. Add the param "format" with webp if supported
        /// </summary>
        /// <param name="mediaItem">The IPublishedContent item.</param>
        /// <param name="width">The width of the output image.</param>
        /// <param name="height">The height of the output image.</param>
        /// <param name="propertyAlias">
        /// Property alias of the property containing the Json data.
        /// </param>
        /// <param name="cropAlias">The crop alias.</param>
        /// <param name="quality">Quality percentage of the output image.</param>
        /// <param name="imageCropMode">The image crop mode.</param>
        /// <param name="imageCropAnchor">The image crop anchor.</param>
        /// <param name="preferFocalPoint">
        /// Use focal point, to generate an output image using the focal point instead of the predefined crop
        /// </param>
        /// <param name="useCropDimensions">
        /// Use crop dimensions to have the output image sized according to the predefined crop sizes, this will override the width
        /// and height parameters&gt;.
        /// </param>
        /// <param name="cacheBuster">
        /// Add a serialised date of the last edit of the item to ensure client cache refresh when updated
        /// </param>
        /// <param name="furtherOptions">The further options.</param>
        /// <param name="ratioMode">Use a dimension as a ratio</param>
        /// <param name="upScale">
        /// If the image should be upscaled to requested dimensions
        /// </param>
        /// <returns>
        /// The <see cref="T:System.String" />.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public static string GetCropUrlWithWebp(this IPublishedContent mediaItem, int? width = null, int? height = null, string propertyAlias = "umbracoFile", string cropAlias = null, int? quality = null, ImageCropMode? imageCropMode = null, ImageCropAnchor? imageCropAnchor = null, bool preferFocalPoint = false, bool useCropDimensions = false, bool cacheBuster = true, string furtherOptions = null, ImageCropRatioMode? ratioMode = null, bool upScale = true)
        {
            var cropUrl = mediaItem.GetCropUrl(width, height, propertyAlias, cropAlias, quality, imageCropMode, imageCropAnchor, preferFocalPoint, useCropDimensions, cacheBuster, furtherOptions, ratioMode, upScale);

            if (WebpHelper.IsWebpSupported(HttpContext.Current, false) && !WebpHelper.IsFormatSpecified(HttpContext.Current))
            {
                cropUrl = $"{cropUrl}{(cropUrl.Contains("?") ? "&" : "?")}format=webp";
            }

            return cropUrl;
        }

        /// <summary>
        /// Get a property called title, or, if not found, get the name of the node.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The title or name</returns>
        public static string GetTitleOrName(this IPublishedContent content)
        {
            if (!content.HasProperty("title"))
            {
                return content.Name;
            }

            var title = content.Value<string>("title");

            if (string.IsNullOrWhiteSpace(title))
            {
                return content.Name;
            }

            return title;
        }
    }
}