using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.Data.Items;
using System;

using Sitecore.Data.Events;
using Sitecore.Events;
using Sitecore.Web;
using System.Collections.Generic;
using System.Linq;

namespace DictionaryFlushPartialCache
{
    
    public class Clear
    {
        private readonly BaseSiteContextFactory _siteContextFactory;

        public Clear(BaseSiteContextFactory siteContextFactory)
        {
          Assert.ArgumentNotNull((object) siteContextFactory, nameof (siteContextFactory));
          this._siteContextFactory = siteContextFactory;
        }

        public void OnItemEvent(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            Item item = ExtractItem(args);
            if (item != null && item.Paths.FullPath.StartsWith(@"/sitecore/system/Dictionary"))
            {
                Flush();
            }
        }

        private static Item ExtractItem(EventArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            switch (args)
            {
                case ItemSavedRemoteEventArgs savedRemoteEventArgs:
                    return savedRemoteEventArgs.Item;
                case SitecoreEventArgs sitecoreEventArgs:
                    return (Item)Sitecore.Events.Event.ExtractParameter(args, 0);
            }
            return (Item)null;
        }

        private void Flush()
        {
            var sites = GetSitesWithPartialHtml();
            foreach (SiteInfo site in sites)
            {
                var partialcache = Sitecore.Caching.CacheManager.FindCacheByName<string>(site.Name + "[partial html]");
                if (partialcache != null)
                {
                    partialcache.Clear();
                }
            }
        }

        private IEnumerable<SiteInfo> GetSitesWithPartialHtml()
        {
            return this._siteContextFactory.GetSites().Where<SiteInfo>(x => x.CacheHtml && x.EnablePartialHtmlCaheClear);
        }
    }
}
