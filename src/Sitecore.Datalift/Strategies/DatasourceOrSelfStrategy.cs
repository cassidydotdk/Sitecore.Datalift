using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift.Strategies
{
    public class DatasourceOrSelfStrategy : BaseStrategy
    {
        public override Item Resolve([CanBeNull] string datasourceString, [NotNull] Item contextItem, [CanBeNull] string templateIdentifier = null)
        {
            Assert.ArgumentNotNull(contextItem, nameof(contextItem));

            var actionItem = contextItem;

            if (!string.IsNullOrWhiteSpace(datasourceString))
            {
                var datasourceItem = contextItem.Database.GetItem(datasourceString);

#if !DEBUG
                if (datasourceItem == null || datasourceItem.Versions.Count == 0)
                {
                    datasourceItem = null;
                }
#endif

                // Right here, is a potential source of controversy. If a datasource has been defined BUT it leads us nowhere, should we fall back to context item?
                // I say not; fallback should only happen if no datasource has been defined at all. That's how one would expect it to work.
                // If you feel differently, fork and add an "else" clause here. That will do the trick. Or discuss. @cassidydotdk on Slack.
                actionItem = datasourceItem;
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