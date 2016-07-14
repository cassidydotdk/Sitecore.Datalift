using System;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Sitecore.Datalift
{
    public abstract class BaseStrategy : IDataliftStrategy
    {
        public abstract Item Resolve(string datasourceString, Item contextItem, string templateIdentifier = null);

        protected virtual Item GetParent(Item item)
        {
            return item.Parent;
        }

        protected virtual bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            if (candidate == null || string.IsNullOrEmpty(templateIdentifier))
                return false;

            var t = TemplateManager.GetTemplate(candidate);
            var y = TemplateManager.GetTemplate(templateIdentifier, candidate.Database);

            if (y == null)
                throw new Exception($"Invalid Template Identifier: \'{templateIdentifier}\'");

            return t.InheritsFrom(y.ID);
        }
    }
}