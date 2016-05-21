using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class TestableDatasourceOrSelfStrategy : DatasourceOrSelfStrategy
    {
        protected override bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            ID id = SitecoreFaker.Instance.MyDatabase.GetTemplateIdFromName(templateIdentifier);
            return candidate.TemplateID == id;
        }
    }
}