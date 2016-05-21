using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class MyDatabase
    {
        private readonly Database _innerDatabase;

        public static List<MyItem> MyItems = new List<MyItem>();
        public static Dictionary<string,ID> TemplateMap = new Dictionary<string, ID>();

        public MyDatabase(Database innerDatabase)
        {
            MyItems = new List<MyItem>();
            TemplateMap = new Dictionary<string, ID>();
            _innerDatabase = innerDatabase;
        }

        public Item GetItem(ID id)
        {
            return MyItems.Find(fi => fi.ID == id);
        }

        public Item GetItem(string identifier)
        {
            return MyItems.Find(fi => fi.FullPath.Equals(identifier));
        }
    }
}