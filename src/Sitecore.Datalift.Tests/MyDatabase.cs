using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Datalift.Tests
{
    public class MyDatabase
    {
        private readonly Database _innerDatabase;
        private static List<MyItem> _itemStorage;
        private static Dictionary<string, ID> _templateMap;

        public MyDatabase(Database innerDatabase)
        {
            _innerDatabase = innerDatabase;
            _itemStorage = new List<MyItem>();
            _templateMap = new Dictionary<string, ID>();
        }

        internal Item AddItem(MyItem item, string templateName, ID templateId)
        {
            _itemStorage.Add(item);
            if(!_templateMap.ContainsKey(templateName))
                _templateMap.Add(templateName, templateId);

            return item;
        }

        public ID GetTemplateIdFromName(string templateName)
        {
            if (_templateMap.ContainsKey(templateName))
                return _templateMap[templateName];
            return ID.Null;
        }

        public Item GetItem(ID id)
        {
            return _itemStorage.Find(fi => fi.ID == id);
        }

        public Item GetItem(string identifier)
        {
            return _itemStorage.Find(fi => fi.FullPath.Equals(identifier));
        }
    }
}