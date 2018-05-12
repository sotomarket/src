using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sotomarket.Controllers
{
    public abstract class BaseController : Controller
    {
        public JsonResult SuccessJson()
        {
            return Json(new { success = true });
        }

        public JsonResult FailJson(Exception ex=null)
        {
            return Json(new { success = false, ex });
        }
    }
}