using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class TestableDatasourceOrSelfStrategy : DatasourceOrSelfStrategy
    {
        protected override bool InheritsTemplate(Item candidate, string templateIdentifier)
        {
            var myItem = (MyItem) candidate;

            ID id = ID.Null;
            if (MyDatabase.TemplateMap.ContainsKey(templateIdentifier))
                id = MyDatabase.TemplateMap[templateIdentifier];

            return candidate.TemplateID == id;
        }
    }
}