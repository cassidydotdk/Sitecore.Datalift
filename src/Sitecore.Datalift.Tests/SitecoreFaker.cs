using System.Reflection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Reflection;

namespace Sitecore.Datalift.Tests
{
    public sealed class SitecoreFaker
    {
        // ReSharper disable once InconsistentNaming
        public static readonly SitecoreFaker _instance = new SitecoreFaker();

        private static MethodBase originalMethod;
        private static MethodBase newMethod;
        private static MethodBase originalMethod2;
        private static MethodBase newMethod2;

        static SitecoreFaker()
        {
        }

        private SitecoreFaker()
        {
            Database = (Database) ReflectionUtil.CreateObject(typeof (Database), new object[] {"unittest"});
            MyDatabase = new MyDatabase(Database);

            originalMethod = Database.GetType().GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis, new[] {typeof (string)}, null);
            newMethod = MyDatabase.GetType().GetMethod("GetItem", BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] {typeof (string)}, null);
            MethodUtil.ReplaceMethod(newMethod, originalMethod);

            originalMethod2 = Database.GetType().GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis, new[] {typeof (ID)}, null);
            newMethod2 = MyDatabase.GetType().GetMethod("GetItem", BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] {typeof (ID)}, null);
            MethodUtil.ReplaceMethod(newMethod2, originalMethod2);
        }

        public Database Database { get; }
        public MyDatabase MyDatabase { get; }

        // ReSharper disable once ConvertToAutoProperty
        public static SitecoreFaker Instance => _instance;

        public Item MakeItem(string name, string templateName, Item parentItem)
        {
            var id = ID.NewID;
            var templateId = MyDatabase.GetTemplateIdFromName(templateName);
            if (templateId.IsNull)
                templateId = ID.NewID;

            var def = new ItemDefinition(id, name, templateId, ID.Null);
            var fields = new FieldList();

            var data = new ItemData(def, Language.Parse("en"), new Version(1), fields);

            string fullpath;
            if (parentItem == null)
                fullpath = "/" + name;
            else
                fullpath = ((MyItem) parentItem).FullPath + "/" + name;

            var ni = new MyItem(name, fullpath, id, data, Database, parentItem);
            return MyDatabase.AddItem(ni, templateName, templateId);
        }
    }
}