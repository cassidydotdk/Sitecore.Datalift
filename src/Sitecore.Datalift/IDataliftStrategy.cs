using Sitecore.Data.Items;

namespace Sitecore.Datalift
{
    public interface IDataliftStrategy
    {
        Item Resolve(string datasourceString, Item contextItem, string templateIdentifier);
    }
}