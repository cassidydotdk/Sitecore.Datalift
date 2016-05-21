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
        public static Database GetDatabase(string name)
        {
            var db = (Database) ReflectionUtil.CreateObject(typeof (Database), new object[] {name});
            var n = new MyDatabase(db);

            MethodBase originalMethod = db.GetType().GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis, new[] {typeof (string)}, null);
            MethodBase newMethod = n.GetType().GetMethod("GetItem", BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] {typeof (string)}, null);
            MethodUtil.ReplaceMethod(newMethod, originalMethod);
            return db;
        }

        public static Item GetItem(ID templateId, Item parentItem)
        {
            var id = ID.NewID;

            var def = new ItemDefinition(id, "fake", templateId, ID.Null);
            var fields = new FieldList();

            var data = new ItemData(def, Language.Parse("en"), new Version(1), fields);
            var db = GetDatabase("unittest");
            return new MyItem(id, data, db, parentItem);
        }
    }
}