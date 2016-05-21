using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    public class DatasourceOrSiteRootStrategyTests
    {
        private static readonly ID _authId = ID.NewID;
        private static readonly ID _cateId = ID.NewID;
        private static readonly ID _newsId = ID.NewID;
        private static readonly ID _siteId = ID.NewID;

        private static Database _db;

        private static Item _home;
        private static Item _section1;
        private static Item _section2;

        [ClassInitialize]
        public static void Initialise(TestContext context)
        {
            var root = SitecoreFaker.MakeItem("fakesitecore", ID.NewID, "root", null);
            var content = SitecoreFaker.MakeItem("content", ID.NewID, "content", root);
            _home = SitecoreFaker.MakeItem("home", _siteId, "site root", content);
            var news2016 = SitecoreFaker.MakeItem("news-2016", _cateId, "category", _home);
            var news2017 = SitecoreFaker.MakeItem("news-2017", _cateId, "category", _home);
            _section1 = SitecoreFaker.MakeItem("section-1", _siteId, "site root", _home);
            _section2 = SitecoreFaker.MakeItem("section-2", _siteId, "site root", _home);
            var authCassidy = SitecoreFaker.MakeItem("mark-cassidy", _authId, "author", news2016);
            var authFigy = SitecoreFaker.MakeItem("kam-figy", _authId, "author", news2016);

            var authReynolds = SitecoreFaker.MakeItem("mike-reynolds", _authId, "author", news2017);
            var authScherrer = SitecoreFaker.MakeItem("daniel-scherrer", _authId, "author", news2017);

            SitecoreFaker.MakeItem("news-1", _newsId, "news", authCassidy);
            SitecoreFaker.MakeItem("news-2", _newsId, "news", authScherrer);
            SitecoreFaker.MakeItem("news-3", _newsId, "news", authFigy);
            SitecoreFaker.MakeItem("news-4", _newsId, "news", authReynolds);
            SitecoreFaker.MakeItem("news-5", _newsId, "news", news2016);
            SitecoreFaker.MakeItem("news-6", _newsId, "news", news2017);

            _db = SitecoreFaker.GetDatabase();
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_valid_datasource()
        {
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            // The DS is valid, and is has no template to go on. It has no choice but to just return the DS
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", _home);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == _authId);

            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", _home, "author");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == _authId);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_valid_datasource_wrong_template()
        {
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", _home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSiteRootStrategyTests_test_invalid_datasource_test()
        {
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", _home);
            Assert.IsNull(result);

            var home2 = _db.GetItem("/fakesitecore/content/home/news-2016");
            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home2, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_returns_null_with_wrong_template_for_site_root_item()
        {
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve(null, _home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_returns_site_root_item_with_correct_template_for_site_root_item()
        {
            var strategy = new TestableDatasourceOrSiteRootStrategy("/fakesitecore/content/home");

            var result = strategy.Resolve(null, _section1, "site root");
            Assert.IsTrue(result.ID == _home.ID);
        }
    }
}