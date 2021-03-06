﻿using System;
using Sitecore.Datalift.Strategies;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrSiteRootAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrSiteRootAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSiteRootStrategy();
        }

        public DatasourceOrSiteRootAttribute(string templateIdentifier, string siteRootPath)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSiteRootStrategy(siteRootPath);
        }

        public string TemplateIdentifier { get; set; }
        public IDataliftStrategy Strategy { get; set; }
    }
}