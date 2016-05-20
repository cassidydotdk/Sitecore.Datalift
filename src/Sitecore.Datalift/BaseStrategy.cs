using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;

namespace Sitecore.Datalift
{
    public abstract class BaseStrategy : IDataliftStrategy
    {
        public abstract Item Resolve(string datasourceString, string templateIdentifier, Item contextItem);

        protected virtual Template GetTemplate(string templateIdentifier, Database database)
        {
            return TemplateManager.GetTemplate(templateIdentifier, database);
        }

        protected virtual bool InheritsTemplate(Item candidate, Template template)
        {
            var t = TemplateManager.GetTemplate(candidate);
            return t.InheritsFrom(template.ID);
        }
    }
}