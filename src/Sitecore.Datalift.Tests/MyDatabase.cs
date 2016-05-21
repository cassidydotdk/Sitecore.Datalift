using System;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Datalift.Tests
{
    public class MyDatabase
    {
        private readonly Database _innerDatabase;

        public MyDatabase(Database innerDatabase)
        {
            _innerDatabase = innerDatabase;
        }

        public Item GetItem(string identifier)
        {
            throw new Exception("Someone wants: " + identifier);
        }
    }
}