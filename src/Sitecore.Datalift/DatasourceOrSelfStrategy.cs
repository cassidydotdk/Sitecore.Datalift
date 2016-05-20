using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift
{
    public class DatasourceOrSelfStrategy : BaseStrategy
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

            var actionItem = contextItem;

            if (!string.IsNullOrWhiteSpace(datasourceString))
            {
                var datasourceItem = contextItem.Database.GetItem(datasourceString);

                if (datasourceItem == null || datasourceItem.Versions.Count == 0)
                {
                    Log.Debug(datasourceItem == null ? $"Datasource: {datasourceString} resolved to NULL" : $"Datasource: {datasourceString} resolved to {datasourceItem.Paths.FullPath} but has 0 versions");
                    datasourceItem = null;
                }

                // Right here, is a potential source of controversy. If a datasource has been defined BUT it leads us nowhere, should we fall back to context item?
                // I say not; fallback should only happen if no datasource has been defined at all. That's how one would expect it to work.
                // If you feel differently, fork and add an "else" clause here. That will do the trick. Or discuss. @cassidydotdk on Slack.
                actionItem = datasourceItem;
            }

            // WTF resharper?
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (actionItem != null && t != null)
            {
                if (!InheritsTemplate(actionItem, t))
                {
                    Log.Debug($"Item: {actionItem.Paths.FullPath} does not implement template: {t.FullName}");
                    actionItem = null;
                }
            }

            return actionItem;
        }
    }
}