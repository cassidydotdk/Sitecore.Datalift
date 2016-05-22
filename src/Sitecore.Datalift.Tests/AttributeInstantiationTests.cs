using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    public class AttributeInstantiationTests
    {
        [ClassInitialize]
        public static void Initialise(TestContext context)
        {
            var root = SitecoreFaker.Instance.MakeItem("fakesitecore", "root", null);
            var content = SitecoreFaker.Instance.MakeItem("content", "content", root);
            var home = SitecoreFaker.Instance.MakeItem("home", "site root", content);
            var news2016 = SitecoreFaker.Instance.MakeItem("news-2016", "category", home);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            SitecoreFaker.Instance.CleanDatabases();
            Assert.IsTrue(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content") == null);
        }

        [TestMethod]
        public void AttributeInstantiationTests_Dsos_Tests()
        {
            var dsos = new MyDsosController();
            dsos.RunExpectedResult(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content"), null, SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content"));
            dsos.RunExpectedResult(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content"), "/fakesitecore/content", SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home"));

            dsos.RunExpectedNullResult(null, SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home"));
            dsos.RunExpectedNullResult("/fakesitecore/content/home", SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content"));
        }

        [TestMethod]
        public void AttributeInstantiationTests_Dsosr_Tests()
        {
            var dsosr = new MyDsosrController();
            dsosr.RunExpectedResult(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home"), null, SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016"));
            dsosr.RunExpectedResult(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home"), "/fakesitecore/content/home", SitecoreFaker.Instance.Database.GetItem("/fakesitecore"));

            dsosr.RunExpectedNullResult("/fakesitecore/content/home/news-2016", SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016"));
        }

        [TestMethod]
        public void AttributeInstantiationTests_Dsoa_Tests()
        {
            var dsoa = new MyDsoaController();
            dsoa.RunExpectedResult(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home"), null, SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016"));
            dsoa.RunExpectedResult(SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home"), "/fakesitecore/content/home", SitecoreFaker.Instance.Database.GetItem("/fakesitecore"));

            dsoa.RunExpectedNullResult("/fakesitecore/content/home/news-2016", SitecoreFaker.Instance.Database.GetItem("/fakesitecore/content/home/news-2016"));
        }
    }
}