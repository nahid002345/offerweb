using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfferWebSystem.EFnClass;
using OfferWebSystem.Models;

namespace OfferWebSystem.Controllers
{
    
    public class AppBaseController : Controller
    {

        
        //
        // GET: /AppBase/
        public OFFERDBEntities oDBContext = new OFFERDBEntities();
        public SysUser oCurrentUser = null;
        public string oCurrentUserType = string.Empty;
        public bool oSysAdmin;
        
        public int ExpiredTimeoutToken = 15;

        public bool IsCurrentUserAdmin()
        {
            oCurrentUser = (SysUser)Session["User"];
            oSysAdmin = (oCurrentUser.SysEnum.EnumName.ToLower().Trim().Contains("administrator")) ? true : false;
            return oSysAdmin;
        }

        public bool IsSessionExist()
        {
            bool result;
            oCurrentUser = (SysUser)Session["User"];
            result = (oCurrentUser != null) ? true : false;
            return result;
        }
        public string ExceptionMsg(Exception ex)
        {
            return (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
        }


    }
}
