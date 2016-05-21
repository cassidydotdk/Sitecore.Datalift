using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Sitecore.Datalift
{
    public abstract class BaseStrategy : IDataliftStrategy
    {
        public abstract Item Resolve(string datasourceString, Item contextItem, string templateIdentifier = null);

        protected virtual bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            var t = TemplateManager.GetTemplate(candidate);
            var y = TemplateManager.GetTemplate(templateIdentifier, candidate.Database);

            return t.InheritsFrom(y.ID);
        }
    }
}