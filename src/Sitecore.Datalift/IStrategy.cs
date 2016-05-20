using Sitecore.Data.Items;

namespace Sitecore.Datalift
{
    public interface IStrategy
    {
        Item Resolve(string datasourceString, string templateIdentifier, Item contextItem);
    }
}