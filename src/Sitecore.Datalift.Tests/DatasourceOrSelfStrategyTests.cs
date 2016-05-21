using System;
using System.Security.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.Datalift.Tests
{
    [TestClass]
    [ReflectionPermission(SecurityAction.Demand)]
    public class DatasourceOrSelfStrategyTests
    {
        [TestInitialize]
        public void Initialise()
        {
            var db = SitecoreFaker.GetDatabase("unittest");
            db.GetItem("I wanna cuz I wanna!");
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
