using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class TestableDatasourceOrAscendantStrategy : DatasourceOrAscendantStrategy
    {
        protected override Item GetParent(Item item)
        {
            return ((MyItem) item).MyParent;
        }

        protected override bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            if (candidate == null || string.IsNullOrEmpty(templateIdentifier))
                return false;

            ID id = SitecoreFaker.Instance.MyDatabase.GetTemplateIdFromName(templateIdentifier);
            return candidate.TemplateID == id;
        }
    }
}