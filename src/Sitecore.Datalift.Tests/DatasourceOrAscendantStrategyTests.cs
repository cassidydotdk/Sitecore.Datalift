using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    public class DatasourceOrAscendantStrategyTests
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

            SitecoreFaker.Instance.MakeItem("contentpage-1", "news", section1);
            SitecoreFaker.Instance.MakeItem("contentpage-2", "news", section1);
            SitecoreFaker.Instance.MakeItem("contentpage-3", "news", section1);
            SitecoreFaker.Instance.MakeItem("contentpage-4", "news", section2);
            SitecoreFaker.Instance.MakeItem("contentpage-5", "news", section2);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            SitecoreFaker.Instance.CleanDatabases();
            Assert.IsTrue(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content") == null);
        }

        [TestMethod]
        public void DatasourceOrAscendantStrategyTests_test_valid_datasource()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrAscendantStrategy();
            var authorId = SitecoreFaker.Instance.MyDatabase.GetTemplateIdFromName("author");

            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "author");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TemplateID == authorId);
        }

        [TestMethod]
        public void DatasourceOrAscendantStrategyTests_test_valid_datasource_wrong_template()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrAscendantStrategy();

            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy", home, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrAscendantStrategyTests_test_invalid_datasource_test()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrAscendantStrategy();

            var home2 = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016");
            var result = strategy.Resolve("/fakesitecore/content/home/news-2016/mark-cassidy!!111!!!oneoneone!", home2, "news");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrAscendantStrategyTests_test_ascendancy_no_template()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home");
            var strategy = new TestableDatasourceOrAscendantStrategy();

            var result = strategy.Resolve(null, home, "dafuq-this-doesnt-exist");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DatasourceOrAscendantStrategyTests_test_ascendancy_section_fallbacks()
        {
            var home = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/section-1/contentpage-1");
            var section1 = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/section-1");

            var strategy = new TestableDatasourceOrAscendantStrategy();

            var result = strategy.Resolve(null, home, "site root");
            Assert.IsTrue(result.ID == section1.ID);

            var content = SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content");
            result = strategy.Resolve(null, home, "content");
            Assert.IsTrue(result.ID == content.ID);
        }
    }
}