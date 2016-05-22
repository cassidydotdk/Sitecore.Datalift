using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    [DatasourceOrSiteRoot("site root")]
    public class MyDsosrController : DataliftController
    {
        protected override IDataliftAttribute GetStrategyAttribute()
        {
            var att = base.GetStrategyAttribute();
            if (att != null)
            {
                Assert.IsInstanceOfType(att.Strategy, typeof(DatasourceOrSiteRootStrategy));
                att.Strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");
            }

            return att;
        }

        public void RunExpectedResult(Item itemToExpect, string datasourceQuery, Item contextItem)
        {
            var result = GetActionItem(datasourceQuery, null, contextItem);
            Assert.IsNotNull(result);
            Assert.IsTrue(itemToExpect.ID == result.ID );
        }

        public void RunExpectedNullResult(string datasourceQuery, Item contextItem)
        {
            var result = GetActionItem(datasourceQuery, null, contextItem, null);
            Assert.IsNull(result);
        }
    }
}