using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfferWebSystem.EFnClass;
using System.IO;

namespace OfferWebSystem.Controllers
{
    [SessionExpire]

    public class UserController : AppBaseController
    {
        private static string postImagePath = "/Content/ImagePost/";
        // GET: User
        [CustomAuthorize(Roles = "administrator")]
        public ActionResult Index()
        {
            List<SysUser> oUserList = new List<SysUser>();
            oUserList = oDBContext.SysUsers.ToList();
            return View(oUserList);
        }

        public ActionResult UserProfile()
        {
            var userSessionProfile = (SysUser)Session["User"];
            var userProfile = oDBContext.SysUsers.FirstOrDefault(t => t.ID == userSessionProfile.ID);
            ViewBag.UserTypeList = oDBContext.SysEnums.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.EnumName.ToString()
            });
            return View(userProfile);
        }

        [CustomAuthorize(Roles = "administrator")]
        [HttpGet]
        public ActionResult UpdateUser(int id)
        {

            var userInfo = new SysUser();

            userInfo = oDBContext.SysUsers.FirstOrDefault(t => t.ID == id);
            var enumFeature = Convert.ToInt32(Enumaretion.DBEnumType.SysUserType);
            ViewBag.UserTypeList = oDBContext.SysEnums.Where(t => t.EnumType == enumFeature && t.IsActive == true).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.EnumName.ToString()
            });

            return PartialView("_userDetailPartial", userInfo);
        }


        public ActionResult UpdateUserProfile(SysUser oUpdateUser, HttpPostedFileBase ctrlPostImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = (SysUser)Session["User"];
                    var userUpdate = oDBContext.SysUsers.FirstOrDefault(t => t.UserId == oUpdateUser.UserId);
                    var uploadFileName = "";
                    if (userUpdate != null)
                    {
                        userUpdate.Email = oUpdateUser.Email;
                        userUpdate.CompanyName = oUpdateUser.CompanyName;
                        userUpdate.ContactNo = oUpdateUser.ContactNo;
                        userUpdate.Website = oUpdateUser.Website;
                        if (ctrlPostImage != null)
                        {
                            uploadFileName = DateTime.Now.ToString("yyddMMhhmmssfff") + "_Profile_" + Path.GetFileName(ctrlPostImage.FileName);
                            userUpdate.CompanyLogoUrl = postImagePath + uploadFileName;
                        }
                        userUpdate.ModifiedBy = currentUser.ID;
                        userUpdate.ModifiedOn = DateTime.Now;
                        oDBContext.SaveChanges();
                        if (ctrlPostImage != null)
                            ctrlPostImage.SaveAs(Path.Combine(Server.MapPath(postImagePath), uploadFileName));
                        TempData["SuccessMsg"] = "Data Saved Successfully";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "User Doesn't Exists!!!";
                        //return View("UserProfile", oUpdateUser);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error Occured Due to " + ExceptionMsg(ex);
            }
            //return View("UserProfile",oUpdateUser);
            return RedirectToAction("UserProfile");
        }


        [HttpGet]
        public ActionResult GetUserByUserTypeSearchKey(string[] UserType, string SearchWord)
        {
            oCurrentUser = (SysUser)Session["User"];
            SearchWord = SearchWord.Trim().ToLower();
            var searchOutletList = new List<SysUser>();
            searchOutletList = oDBContext.SysUsers.ToList();
            if (searchOutletList != null && searchOutletList.Count > 0)
            {
                if (UserType != null && UserType.Count() > 0)
                {
                    if (UserType[0].Contains(','))
                        UserType = UserType[0].Split(',');
                    searchOutletList = searchOutletList.Where(t => UserType.Contains(t.UserType.ToString())).ToList();
                }

                if (!string.IsNullOrEmpty(SearchWord))
                    searchOutletList = searchOutletList.Where(t => t.UserId.ToLower().Contains(SearchWord) || t.CompanyName.ToLower().Contains(SearchWord) || t.ContactNo.ToLower().Contains(SearchWord) || t.Website.ToLower().Contains(SearchWord) || t.Email.ToLower().Contains(SearchWord)).ToList();
            }

            return PartialView("_userListPartial", searchOutletList);
        }
    }
}