using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class MyItem : Item
    {
        public MyItem(string name, string fullpath, ID itemId, ItemData data, Database database, Item parentItem) : base(itemId, data, database)
        {
            MyName = name;
            MyParent = parentItem;
            FullPath = fullpath;
        }

        public string MyName { get; set; }
        public Item MyParent { get; set; }
        public string FullPath { get; set; }
    }
}