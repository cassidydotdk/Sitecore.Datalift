using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class MyItem : Item
    {
        public MyItem(string name, string fullpath, ID itemId, ItemData data, Database database, Item parentItem) : base(itemId, data, database)
        {
            Name = name;
            Parent = parentItem;
            FullPath = fullpath;
        }

        public new string Name { get; set; }
        public new Item Parent { get; set; }
        public string FullPath { get; set; }
    }
}