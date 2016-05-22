using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Datalift.Strategies;

namespace Sitecore.Datalift.Tests
{
    public class TestableDatasourceOrSiteRootStrategy : DatasourceOrSiteRootStrategy
    {
        public TestableDatasourceOrSiteRootStrategy(string contextSiteStartPath) : base(contextSiteStartPath)
        {
        }

        protected override bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            ID id = SitecoreFaker.Instance.MyDatabase.GetTemplateIdFromName(templateIdentifier);
            return candidate.TemplateID == id;
        }
    }
}