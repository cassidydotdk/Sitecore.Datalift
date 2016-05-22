using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    public class DatasourceOrSelfStrategyTests
    {
        [ClassInitialize]
        public static void Initialise(TestContext context)
        {
            SitecoreFaker.Instance.CleanDatabases();
            var root = SitecoreFaker.Instance.MakeItem("fakesitecore", "root", null);
            var content = SitecoreFaker.Instance.MakeItem("content", "content", root);
            var home = SitecoreFaker.Instance.MakeItem("home", "site root", content);
            var news2016 = SitecoreFaker.Instance.MakeItem("news-2016", "category", home);
            var news2017 = SitecoreFaker.Instance.MakeItem("news-2017", "category", home);

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

        [ClassCleanup]
        public static void Cleanup()
        {
            SitecoreFaker.Instance.CleanDatabases();
            Assert.IsTrue(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content") == null);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTests_test_valid_datasource()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");

            var authorTemplateId = SitecoreFaker.Instance.MyDatabase.GetTemplateIdFromName("author");
            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == authorTemplateId, "Result (" + result.Name + ").TemplateID is " + result.TemplateID + ", expected: " + authorTemplateId);

            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "author");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == authorTemplateId);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTests_test_valid_datasource_wrong_template()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTests_test_invalid_datasource_test()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home);
            Assert.IsNull(result);

            home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016");
            result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_falls_back_to_context_item_without_template()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve(null, home);
            Assert.IsTrue(result.ID == home.ID);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_returns_null_with_wrong_template_for_context_item()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve(null, home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_blank_datasource_returns_contextitem_with_correct_template_for_context_item()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve(null, home, "category");
            Assert.IsTrue(result.ID == home.ID);
        }

        [TestMethod]
        public void DatasourceOrSelfStrategyTest_test_correct_datasource_returns_datasource_item_with_correct_template_for_context_item()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016");

            var strategy = new TestableDatasourceOrSelfStrategy();
            var result = strategy.Resolve("/fakesitecore/content/home/news-2017", home, "category");
            Assert.IsFalse(result.ID == home.ID);
            Assert.IsTrue(result.Name == "news-2017");

            result = strategy.Resolve("  ", home, "category");
            Assert.IsTrue(result.ID == home.ID);
            Assert.IsTrue(result.Name == "news-2016");
        }
    }
}