using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfferWebSystem.EFnClass;
using OfferWebSystem.Models;
using System.IO;

namespace OfferWebSystem.Controllers
{
    [SessionExpire]
    [CustomAuthorize(Roles = "administrator")]
    public class CategoryController : AppBaseController
    {
        // GET: Category
        public List<OfferCategory> oCategoryList = null;
        public OfferCategory oOfferCategory = null;
        private static string postImagePath = "/Content/ImageCategory/";
        public ActionResult Index()
        {
            oCurrentUser = (SysUser)Session["User"];
            oCategoryList = oDBContext.OfferCategories.ToList();
            return View(oCategoryList);
        }

        [HttpGet]
        public ActionResult NewCategory()
        {
            oCurrentUser = (SysUser)Session["User"];
            var newCategory = new OfferCategory();
            newCategory.IsActive = false;

            var CategoryInfo = new CategoryDetailModel();
            CategoryInfo.IsNew = true;
            CategoryInfo.Category = newCategory;

            ViewBag.CategoryList = oDBContext.OfferCategories.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.CategoryName.ToString()
            });

            return PartialView("_categoryDetailPartial", CategoryInfo);
        }

        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {

            var CategoryInfo = new CategoryDetailModel();
            CategoryInfo.IsNew = false;
            CategoryInfo.Category = oDBContext.OfferCategories.FirstOrDefault(t => t.ID == id);

            ViewBag.CategoryList = oDBContext.OfferCategories.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.CategoryName.ToString()
            });

            return PartialView("_categoryDetailPartial", CategoryInfo);
        }


        [HttpPost]
        public ActionResult InsertCategory(CategoryDetailModel oCategoryInfo, HttpPostedFileBase ctrlCatIcon)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oCurrentUser = (SysUser)Session["User"];
                    var uploadFileName = "";
                    var existCategory = oDBContext.OfferCategories.FirstOrDefault(t => t.ID == oCategoryInfo.Category.ID);
                    if (existCategory == null)
                    {
                        OfferCategory oOfferCategory = oCategoryInfo.Category;
                        oOfferCategory.IsActive = true;
                        if (ctrlCatIcon != null)
                        {
                            uploadFileName = DateTime.Now.ToString("yyddMMhhmmssfff") + "_" + Path.GetFileName(ctrlCatIcon.FileName).Replace(" ", "").ToLower().Trim();
                            oOfferCategory.CategoryIcon = postImagePath + uploadFileName;
                            ctrlCatIcon.SaveAs(Path.Combine(Server.MapPath(postImagePath), uploadFileName));
                        }
                        oOfferCategory.CreatedBy = oCurrentUser.ID;
                        oOfferCategory.CreatedOn = DateTime.Now;
                        oDBContext.OfferCategories.Add(oOfferCategory);
                        oDBContext.SaveChanges();
                        TempData["SuccessMsg"] = "Data Saved Successfully";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Data already Exists!!!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error Occured Due to " + ExceptionMsg(ex);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCategory(CategoryDetailModel oCategoryInfo, string chkIsActive, HttpPostedFileBase ctrlCatIcon)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oCurrentUser = (SysUser)Session["User"];
                    var uploadFileName = "";
                    var existCategory = oDBContext.OfferCategories.FirstOrDefault(t => t.ID == oCategoryInfo.Category.ID);
                    if (existCategory != null)
                    {
                        existCategory.CategoryName = oCategoryInfo.Category.CategoryName;
                        if (ctrlCatIcon != null)
                        {
                            uploadFileName = DateTime.Now.ToString("yyddMMhhmmssfff") + "_" + Path.GetFileName(ctrlCatIcon.FileName).Replace(" ", "").ToLower().Trim();
                            existCategory.CategoryIcon = postImagePath + uploadFileName;
                            ctrlCatIcon.SaveAs(Path.Combine(Server.MapPath(postImagePath), uploadFileName));
                        }
                        existCategory.IsActive = (!string.IsNullOrEmpty(chkIsActive) && chkIsActive.Contains("on")) ? true : false;
                        existCategory.ModifiedBy = oCurrentUser.ID;
                        existCategory.ModifiedOn = DateTime.Now;
                        oDBContext.SaveChanges();
                        TempData["SuccessMsg"] = "Data Update Successfully";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Data Not Found!!!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error Occured Due to " + ExceptionMsg(ex);
            }
            return RedirectToAction("Index");
        }

    }
}