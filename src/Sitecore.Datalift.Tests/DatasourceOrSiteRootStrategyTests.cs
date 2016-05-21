using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    public class DatasourceOrSiteRootStrategyTests
    {
        [ClassInitialize]
        public static void Initialise(TestContext context)
        {
            var root = SitecoreFaker.Instance.MakeItem("fakesitecore", "root", null);
            var content = SitecoreFaker.Instance.MakeItem("content", "content", root);
            var home = SitecoreFaker.Instance.MakeItem("home", "site root", content);
            var news2016 = SitecoreFaker.Instance.MakeItem("news-2016", "category", home);
            var news2017 = SitecoreFaker.Instance.MakeItem("news-2017", "category", home);
            var section1 = SitecoreFaker.Instance.MakeItem("section-1", "site root", home);
            var section2 = SitecoreFaker.Instance.MakeItem("section-2", "site root", home);
            var authCassidy = SitecoreFaker.Instance.MakeItem("mark-cassidy", "author", news2016);
            var authFigy = SitecoreFaker.Instance.MakeItem("kam-figy", "author", news2016);

            var authReynolds = SitecoreFaker.Instance.MakeItem("mike-reynolds", "author", news2017);
            var authScherrer = SitecoreFaker.Instance.MakeItem("daniel-scherrer", "author", news2017);

            SitecoreFaker.Instance.MakeItem("news-1", "news", authCassidy);
            SitecoreFaker.Instance.MakeItem("news-2", "news", authScherrer);
            SitecoreFaker.Instance.MakeItem("news-3", "news", authFigy);
            SitecoreFaker.Instance.MakeItem("news-4", "news", authReynolds);
            SitecoreFaker.Instance.MakeItem("news-5", "news", news2016);
            SitecoreFaker.Instance.MakeItem("news-6", "news", news2017);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_valid_datasource()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");
            var authorId = SitecoreFaker.Instance.MyDatabase.GetTemplateIdFromName("author");

            // The DS is valid, and is has no template to go on. It has no choice but to just return the DS
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == authorId);

            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "author");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == authorId);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_valid_datasource_wrong_template()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_invalid_datasource_test()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home);
            Assert.IsNull(result);

            var home2 = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016");
            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home2, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_blank_datasource_returns_null_with_wrong_template_for_site_root_item()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve(null, home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_blank_datasource_returns_site_root_item_with_correct_template_for_site_root_item()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var section1 = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/section-1");
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve(null, section1, "site root");
            Assert.IsTrue(result.ID == home.ID);
        }
    }
}