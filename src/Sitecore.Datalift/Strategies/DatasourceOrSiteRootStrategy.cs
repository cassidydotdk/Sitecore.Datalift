using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift.Strategies
{
    public class DatasourceOrSiteRootStrategy : BaseStrategy
    {
        private readonly string _contextSiteStartPath;

        public DatasourceOrSiteRootStrategy()
        {
        }

        public DatasourceOrSiteRootStrategy(string contextSiteStartPath)
        {
            _contextSiteStartPath = contextSiteStartPath;
        }

        public override Item Resolve([NotNull] string datasourceString, [NotNull] Item contextItem, [CanBeNull] string templateIdentifier = null)
        {
            Assert.ArgumentNotNull(contextItem, nameof(contextItem));

            Item actionItem;

            if (!string.IsNullOrWhiteSpace(datasourceString))
            {
                var datasourceItem = contextItem.Database.GetItem(datasourceString);

#if !DEBUG
                if (datasourceItem == null || datasourceItem.Versions.Count == 0)
                {
                    datasourceItem = null;
                }
#endif
                actionItem = datasourceItem;
            }
            else
            {
                Item siteRoot;

                if (!string.IsNullOrEmpty(_contextSiteStartPath))
                    siteRoot = contextItem.Database.GetItem(_contextSiteStartPath);
                else
                    siteRoot = contextItem.Database.GetItem(Context.Site.StartPath);

#if !DEBUG
                if (siteRoot == null || siteRoot.Versions.Count == 0)
                {
                    siteRoot = null;
                }
#endif

                actionItem = siteRoot;
            }

            if (actionItem != null && templateIdentifier != null)
            {
                if (!InheritsTemplate(actionItem, templateIdentifier))
                {
                    actionItem = null;
                }
            }

            return actionItem;
        }
    }
}