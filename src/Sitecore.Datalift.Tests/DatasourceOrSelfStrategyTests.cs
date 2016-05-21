using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Data;
using Sitecore.Data.Templates;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    public class DatasourceOrSelfStrategyTests
    {
        private Database _db;
        private readonly ID _newsId = ID.NewID;
        private readonly ID _authId = ID.NewID;
        private readonly ID _cateId = ID.NewID;

        [TestInitialize]
        public void Initialise()
        {
            var root = SitecoreFaker.MakeItem("fakesitecore", ID.NewID, "root", null);
            var content = SitecoreFaker.MakeItem("content", ID.NewID, "content", root);
            var home = SitecoreFaker.MakeItem("home", ID.NewID, "site root", content);
            var news2016 = SitecoreFaker.MakeItem("news-2016", _cateId, "category", home);
            var news2017 = SitecoreFaker.MakeItem("news-2017", _cateId, "category", home);

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
        public void DatasourceOrSelfStrategyTests_test_valid_datasource()
        {
            var home = _db.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == _authId);

            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "author");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == _authId);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTests_test_valid_datasource_wrong_template()
        {
            var home = _db.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTests_test_invalid_datasource_test()
        {
            var home = _db.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home);
            Assert.IsNull(result);

            home = _db.GetItem("/fakesitecore/content/home/news-2016");
            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_falls_back_to_context_item_without_template()
        {
            var home = _db.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve(null, home);
            Assert.IsTrue(result.ID == home.ID);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_returns_null_with_wrong_template_for_context_item()
        {
            var home = _db.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve(null, home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_returns_contextitem_with_correct_template_for_context_item()
        {
            var home = _db.GetItem("/fakesitecore/content/home/news-2016");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve(null, home, "category");
            Assert.IsTrue(result.ID == home.ID);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_correct_datasource_returns_datasource_item_with_correct_template_for_context_item()
        {
            var home = _db.GetItem("/fakesitecore/content/home/news-2016");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2017", home, "category");
            Assert.IsFalse(result.ID == home.ID);
            Assert.IsTrue(result.Name == "news-2017");
        }
    }
}