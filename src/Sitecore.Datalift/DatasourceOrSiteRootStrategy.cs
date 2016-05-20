using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift
{
    public class DatasourceOrSiteRootStrategy : BaseStrategy
    {
        public override Item Resolve(string datasourceString, string templateIdentifier, Item contextItem)
        {
            Assert.ArgumentNotNull(contextItem, nameof(contextItem));

            Template t = null;
            if (!string.IsNullOrWhiteSpace(templateIdentifier))
            {
                t = GetTemplate(templateIdentifier, contextItem.Database);
                Assert.IsNotNull(t, $"Template: {templateIdentifier}");
            }

            Log.Debug($"Action Item resolving. Strategy: {GetType().FullName}, Database: {Context.Database.Name}, Language: {Context.Language.Name}, Expected Base Template: {t.FullName}, Context Item: {contextItem.Paths.FullPath}");

            Item actionItem;

            if (!string.IsNullOrWhiteSpace(datasourceString))
            {
                var datasourceItem = contextItem.Database.GetItem(datasourceString);

                if (datasourceItem == null || datasourceItem.Versions.Count == 0)
                {
                    Log.Debug(datasourceItem == null ? $"Datasource: {datasourceString} resolved to NULL" : $"Datasource: {datasourceString} resolved to {datasourceItem.Paths.FullPath} but has 0 versions");
                    datasourceItem = null;
                }

                actionItem = datasourceItem;
            }
            else
            {
                var siteRoot = contextItem.Database.GetItem(Context.Site.StartPath);
                if (siteRoot == null || siteRoot.Versions.Count == 0)
                {
                    Log.Debug(siteRoot == null ? $"Site root: {Context.Site.StartPath} resolved to NULL" : $"Site root: {Context.Site.StartPath} resolved but has 0 versions");
                    siteRoot = null;
                }

                actionItem = siteRoot;
            }

            // WTF resharper?  
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (actionItem != null && t != null)
            {
                if (!InheritsTemplate(actionItem, t))
                {
                    Log.Debug($"Item: {actionItem.Paths.FullPath} does not implement {t.FullName}");
                    actionItem = null;
                }
            }

            return actionItem;
        }
    }
}