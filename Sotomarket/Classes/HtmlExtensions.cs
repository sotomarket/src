using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Sotomarket.Classes
{
    public static class HtmlExtensions
    {
        public static bool Allowed(this HtmlHelper htmlHelper, string action, string controller = "")
        {
            ControllerBase controllerBase = string.IsNullOrEmpty(controller) ? htmlHelper.ViewContext.Controller : GetControllerByName(htmlHelper, controller);
            ControllerContext controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerBase);
            ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controllerContext.Controller.GetType());
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controllerContext, action);

            if (actionDescriptor == null)
                return false;

            FilterInfo filters = new FilterInfo(FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor));

            AuthorizationContext authorizationContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (IAuthorizationFilter authorizationFilter in filters.AuthorizationFilters)
            {
                authorizationFilter.OnAuthorization(authorizationContext);
                if (authorizationContext.Result != null)
                    return false;
            }
            return true;
        }

        public static MvcHtmlString AllowedActionLink(this HtmlHelper htmlHelper,string linkText,string actionName)
        {
            if (htmlHelper.Allowed(actionName))
            {
                return htmlHelper.ActionLink(linkText,actionName);
            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString AllowedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues=null, object htmlAttributes=null)
        {
            if (htmlHelper.Allowed(actionName))
            {
                return htmlHelper.ActionLink(linkText,actionName,routeValues,htmlAttributes);
            }
            return new MvcHtmlString("");
        }

        public static ControllerBase GetControllerByName(HtmlHelper htmlHelper, string controllerName)
        {
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
            IController controller = factory.CreateController(htmlHelper.ViewContext.RequestContext, controllerName);
            if (controller == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "The IControllerFactory '{0}' did not return a controller for the name '{1}'.", factory.GetType(), controllerName));
            }
            return (ControllerBase)controller;
        }
    }
}