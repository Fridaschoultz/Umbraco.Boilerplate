﻿using System.Collections.Generic;
using System.Linq;
using Camelonta.Boilerplate.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Camelonta.Boilerplate.Classes
{
    public static class ExtensionMethods
    {
        public static List<IPublishedContent> FilterInvalidPages(this IEnumerable<IPublishedContent> pages)
        {
            return pages.Where(p => p.IsVisible() && p.DocumentTypeAlias != "Faqquestion" && p.DocumentTypeAlias != "folder").ToList();
        }

        public static IEnumerable<IPublishedContent> FilterSearchResults(this IEnumerable<IPublishedContent> pages)
        {
            // Remove invalid document types
            pages = pages.Where(page => page.DocumentTypeAlias.ToLower() != "site");

            // Remove pages that doesn't have search engine indexing allowed ("Tillåt EJ sökmotorindexering"-property)
            pages = pages.Where(page => !page.GetPropertyValue<bool>("robotsIndex"));

            return pages;
        }

        /// <summary>
        /// Get page from Content picker ID
        /// </summary>
        /// <param name="helper">UmbracoHelper</param>
        /// <param name="page">Page to get property from</param>
        /// <param name="propertyAlias">Alias of Content picker property</param>
        /// <returns></returns>
        public static IPublishedContent GetIPublishedContent(this UmbracoHelper helper, IPublishedContent page, string propertyAlias)
        {
            int id = page.GetPropertyValue<int>(propertyAlias);
            return helper.TypedContent(id);
        }

        public static Hero GetHero(this IPublishedContent page)
        {
            var imageId = page.GetPropertyValue<string>("heroImage");

            if (string.IsNullOrEmpty(imageId)) return null;

            var umbraco = new UmbracoHelper(UmbracoContext.Current);
            var image = umbraco.TypedMedia(imageId);
            
            if (image != null)
            {
                return new Hero
                {
                    Image = image.Url,
                    Text = page.GetPropertyValue<string>("heroText")
                };
            }

            return null;
        }
    }
}