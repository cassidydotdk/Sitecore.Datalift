﻿using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift
{
    public class DatasourceOrAscendantStrategy : BaseStrategy
    {
        public override Item Resolve([NotNull] string datasourceString, [NotNull] Item contextItem, [NotNull] string templateIdentifier)
        {
            // This strategy makes no sense, without knowing the base template we're after
            Assert.ArgumentNotNull(templateIdentifier, nameof(templateIdentifier));
            Assert.ArgumentNotNull(contextItem, nameof(contextItem));

            Item actionItem;

            if (!string.IsNullOrWhiteSpace(datasourceString))
            {
                var datasourceItem = contextItem.Database.GetItem(datasourceString);

                if (datasourceItem == null || datasourceItem.Versions.Count == 0)
                {
                    datasourceItem = null;
                }

                if (!InheritsTemplate(datasourceItem, templateIdentifier))
                    datasourceItem = null;

                actionItem = datasourceItem;
            }
            else
            {
                actionItem = contextItem;

                while (!InheritsTemplate(actionItem, templateIdentifier))
                {
                    actionItem = actionItem.Parent;
                    if (actionItem == null)
                        break;
                }
            }

            return actionItem;
        }
    }
}