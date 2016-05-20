using Sitecore.Data.Items;

namespace Sitecore.Datalift
{
    public interface IDataliftStrategy
    {
        Item Resolve(string datasourceString, string templateIdentifier, Item contextItem);
    }
}