using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Datalift.Strategies;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Datalift
{
    public static class Extensions
    {
        public static IDataliftAttribute GetStrategyAttribute(this IController controller)
        {
            var customAttributes = controller.GetType().GetCustomAttributes(typeof (IDataliftAttribute), false);
            if (customAttributes.Length > 0)
            {
                return customAttributes[0] as IDataliftAttribute;
            }

            return null;
        }

        public static Item GetActionItem(this IController controller, string datasourceString = null, string templateIdentifier = null, Item contextItem = null, IDataliftStrategy strategy = null)
        {
            IDataliftAttribute att = null;
            if (controller != null)
                att = GetStrategyAttribute(controller);

            if (strategy == null && att != null)
            {
                strategy = att.Strategy;
            }

            if (templateIdentifier == null && att != null)
            {
                templateIdentifier = att.TemplateIdentifier;
            }

            if (datasourceString == null)
                datasourceString = RenderingContext.CurrentOrNull?.Rendering.DataSource;

            if (strategy == null)
                strategy = new DatasourceOrSelfStrategy();

            if (contextItem == null)
                contextItem = Context.Item;

            return strategy.Resolve(datasourceString, contextItem, templateIdentifier);
        }

        public static Item GetActionItem(this RenderingContext renderingContext, string templateIdentifier = null, IDataliftStrategy strategy = null)
        {
            return GetActionItem(null, renderingContext.Rendering.DataSource, templateIdentifier, renderingContext.ContextItem, strategy);
        }
    }
}