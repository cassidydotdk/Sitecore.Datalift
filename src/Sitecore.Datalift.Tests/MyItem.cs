using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class MyItem : Item
    {
        public MyItem(ID itemId, ItemData data, Database database, Item parentItem) : base(itemId, data, database)
        {
            Parent = parentItem;
        }

        public new Item Parent { get; set; }
    }
}