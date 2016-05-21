using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class TestableDatasourceOrSiteRootStrategy : DatasourceOrSiteRootStrategy
    {
        public TestableDatasourceOrSiteRootStrategy(string contextSiteStartPath) : base(contextSiteStartPath)
        {
        }

        protected override bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            ID id = ID.Null;
            if (MyDatabase.TemplateMap.ContainsKey(templateIdentifier))
                id = MyDatabase.TemplateMap[templateIdentifier];

            return candidate.TemplateID == id;
        }
    }
}