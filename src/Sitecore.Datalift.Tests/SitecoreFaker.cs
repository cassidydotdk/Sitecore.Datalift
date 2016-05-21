using System;
using System.Collections.Generic;
using System.Reflection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Reflection;
using Version = Sitecore.Data.Version;

namespace Sitecore.Datalift.Tests
{
    public static class SitecoreFaker
    {
        private static readonly Database Db;

        static SitecoreFaker()
        {
            Db = (Database)ReflectionUtil.CreateObject(typeof(Database), new object[] { "unittest" });
            var mdb = new MyDatabase(Db);

            MethodBase originalMethod = Db.GetType().GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis, new[] { typeof(string) }, null);
            MethodBase newMethod = mdb.GetType().GetMethod("GetItem", BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] { typeof(string) }, null);
            MethodUtil.ReplaceMethod(newMethod, originalMethod);

            originalMethod = Db.GetType().GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis, new[] { typeof(ID) }, null);
            newMethod = mdb.GetType().GetMethod("GetItem", BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] { typeof(ID) }, null);
            MethodUtil.ReplaceMethod(newMethod, originalMethod);
        }

        public static Database GetDatabase()
        {
            return Db;
        }

        public static Item MakeItem(string name, ID templateId, string templateName, Item parentItem)
        {
            var id = ID.NewID;

            var def = new ItemDefinition(id, name, templateId, ID.Null);
            var fields = new FieldList();

            var data = new ItemData(def, Language.Parse("en"), new Version(1), fields);

            string fullpath;
            if (parentItem == null)
                fullpath = "/" + name;
            else
                fullpath = ((MyItem) parentItem).FullPath + "/" + name;

            var ni = new MyItem(name, fullpath, id, data, Db, parentItem);
            MyDatabase.MyItems.Add(ni);
            if(!MyDatabase.TemplateMap.ContainsKey(templateName))
                MyDatabase.TemplateMap.Add(templateName, templateId);
            return ni;
        }
    }
}