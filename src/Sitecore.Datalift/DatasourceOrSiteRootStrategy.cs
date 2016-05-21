using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift
{
    public class DatasourceOrSiteRootStrategy : BaseStrategy
    {
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
                var siteRoot = contextItem.Database.GetItem(Context.Site.StartPath);
                if (siteRoot == null || siteRoot.Versions.Count == 0)
                {
                    siteRoot = null;
                }

                actionItem = siteRoot;
            }

            // WTF resharper?  
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
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