using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfferWebSystem.EFnClass;
using OfferWebSystem.Models;
using System.Web.Security;
using System.Threading.Tasks;
using System.IO;

namespace OfferWebSystem.Controllers
{
    public class SignInController : AppBaseController
    {
        //
        // GET: /SignIn/
        SysUser oSysUser = null;

        [AllowAnonymous]

        public ActionResult Index()
        {
            if (IsSessionExist())
                return RedirectToAction("Index", "Dashboard");
            else {
                Session.Clear();
                return View();
            }
        }

        public ActionResult ForgetPassword()
        {
            Session.Clear();
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            Session.Clear();
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignInSubmit(LogIn objSignIn)
        {
            if (ModelState.IsValid)
            {

                oSysUser = oDBContext.SysUsers.FirstOrDefault(t => t.UserId == objSignIn.USERID && t.Password == objSignIn.USERPASSWORD.Trim());
                if (Membership.ValidateUser(objSignIn.USERID, objSignIn.USERPASSWORD))
                {
                    FormsAuthentication.Authenticate(objSignIn.USERID, objSignIn.USERPASSWORD);
                    Session["User"] = oSysUser;
                    Session["UserRole"] = oSysUser.SysEnum.EnumName.ToLower().Trim();
                    oCurrentUserType = oSysUser.SysEnum.EnumName.ToLower().Trim();
                    oSysAdmin = (oSysUser.SysEnum.EnumName.ToLower().Trim().Contains("administrator")) ? true : false;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["SignMsg"] = "Invalid User ID / Password !!!";
                    return View("Index");
                }

            }

            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult ForgetPasswordSubmit(ForgetPassword objFgtPwd)
        {
            if (ModelState.IsValid)
            {

                oSysUser = oDBContext.SysUsers.FirstOrDefault(t => t.UserId == objFgtPwd.USERID && t.Email == objFgtPwd.USEREMAIL.Trim());
                if (oSysUser != null)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Server.MapPath("~/Content/EmailTemplate/ForgetPasswordEmailBody.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{UserID}", oSysUser.UserId);
                    body = body.Replace("{Email}", oSysUser.Email);
                    body = body.Replace("{Password}", oSysUser.Password);
                    SendEmail.SendMail("Forget Password", objFgtPwd.USEREMAIL, body);
                    TempData["SignMsgSuccess"] = "Please Check Inbox/Spam/Junk of your mailbox.";
                    return View("Index");
                }
                else
                {
                    TempData["SignMsg"] = "Invalid User ID / Email !!!";
                    return View("Index");
                }

            }

            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult SignRegister(RegisterUser objRegisterUser)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var userRegister = oDBContext.SysUsers.FirstOrDefault(t => t.UserId == objRegisterUser.USERID);
                    if (userRegister == null)
                    {
                        int eType = Convert.ToInt32(Enumaretion.DBEnumType.SysUserType);
                        var standardUserType = oDBContext.SysEnums.FirstOrDefault(t => t.EnumType == eType && t.EnumName.ToLower().Contains("standard"));
                        SysUser oSysUser = new SysUser();
                        oSysUser.UserId = objRegisterUser.USERID;
                        oSysUser.Password = objRegisterUser.USERPASSWORD;
                        oSysUser.Email = objRegisterUser.USEREMAIL;
                        oSysUser.CompanyName = objRegisterUser.USERCOMPANY;
                        oSysUser.IsActive = true;
                        oSysUser.UserType = (standardUserType != null) ? standardUserType.ID : 2;
                        oSysUser.CreatedBy = 0;
                        oSysUser.CreatedOn = DateTime.Now;

                        oDBContext.SysUsers.Add(oSysUser);
                        oDBContext.SaveChanges();
                    }
                    else
                    {
                        TempData["SignMsg"] = "User already Exists!!!";
                        return View("Register");
                    }


                }
            }
            catch (Exception ex)
            {
                TempData["SignMsg"] = "Error Occured Due to " + ExceptionMsg(ex);
            }
            return View("Index");
        }
        public ActionResult LogOut(string Msg)
        {
            Session.Clear();
            return View("Index");
        }

        public ActionResult NotAuthorized()
        {
            TempData["SignMsg"] = "Not Authorized. Please Sign in again...";
            Session.Clear();
            return View("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult IsUserIDExists(string USERID)
        {

            var user = oDBContext.SysUsers.FirstOrDefault(t => t.UserId == USERID);

            return Json(user == null);
        }

    }
}
