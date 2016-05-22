using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    [DatasourceOrSelf("content")]
    public class MyDsosController : DataliftController
    {
        protected override IDataliftAttribute GetStrategyAttribute()
        {
            var att = base.GetStrategyAttribute();
            if (att != null)
            {
                Assert.IsInstanceOfType(att.Strategy, typeof(DatasourceOrSelfStrategy));
                att.Strategy = new TestableDatasourceOrSelfStrategy();
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