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
    public class PostController : AppBaseController
    {
        // GET: Post
        private int pageSize = 4;
        private static string postImagePath = "/Content/ImagePost/";
        private static string defaultImageFileName = "discount_post_default.jpg";
        public SysUser oSysCurrentUser = null;

        private List<OffersInfo> GetOffersInfo()
        {
            var offerList = new List<OffersInfo>();
            if (IsCurrentUserAdmin())
                offerList = oDBContext.OffersInfoes.OrderByDescending(t => t.ID).ToList();
            else
                offerList = oDBContext.OffersInfoes.Where(t => t.CreatedBy == oCurrentUser.ID).OrderByDescending(t => t.ID).ToList();
            return offerList;
        }
        public ActionResult Index(int? page)
        {

            var postList = new OfferPostListModel();
            postList.IsUserAdmin = IsCurrentUserAdmin();
            postList.OffersList = GetOffersInfo();
            return View(postList);
        }


        public ActionResult GetPostDetail(string postId)
        {
            oCurrentUser = (SysUser)Session["User"];
            OffersInfo oOffersInfo = new OffersInfo();
            if (!string.IsNullOrEmpty(postId))
            {
                var enumFeature = Convert.ToInt32(Enumaretion.DBEnumType.OfferFeature);
                var lowFeature = oDBContext.SysEnums.FirstOrDefault(t => t.EnumType == enumFeature && t.EnumName.ToLower().Contains("low")).ID;
                oOffersInfo = oDBContext.OffersInfoes.FirstOrDefault(t => t.ID.ToString() == postId);
                ViewBag.CatList = oDBContext.OfferCategories.Where(t => t.IsActive).Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.CategoryName.ToString()
                });
                if (!IsCurrentUserAdmin())
                {
                    ViewBag.LocList = oDBContext.OfferLocOutletMaps.Where(t => t.IsActive && t.SysUser.ID == oCurrentUser.ID).Select(x => x.OfferLocation).Distinct().ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.LocationName.ToString()
                    });
                }
                else
                {
                    ViewBag.LocList = oDBContext.OfferLocOutletMaps.Where(t => t.IsActive).Select(x => x.OfferLocation).Distinct().ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.LocationName.ToString()
                    });
                }

                if (IsCurrentUserAdmin())
                {

                    ViewBag.FeatureList = oDBContext.SysEnums.Where(t => t.EnumType == enumFeature).Select(x => x).Distinct().ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.EnumName.ToString()
                    });
                }
                else
                {
                    ViewBag.FeatureList = oDBContext.SysEnums.Where(t => t.EnumType == enumFeature && t.EnumName.ToLower().Contains("low")).Select(x => x).Distinct().ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.EnumName.ToString()
                    });
                }
            }
            return PartialView("_postDetailPartial", oOffersInfo);
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            oCurrentUser = (SysUser)Session["User"];
            var newPost = new OffersInfo();
            if (Session["User"] != null)
            {
                var enumFeature = Convert.ToInt32(Enumaretion.DBEnumType.OfferFeature);
                var lowFeature = oDBContext.SysEnums.FirstOrDefault(t => t.EnumType == enumFeature && t.EnumName.ToLower().Contains("low")).ID;
                oSysCurrentUser = (SysUser)Session["User"];
                newPost.OfferFeatureVal = lowFeature;
                newPost.IsActive = false;
                newPost.CreatedBy = oSysCurrentUser.ID;
                newPost.CreatedOn = DateTime.Now;
                newPost.OfferStatus = 1; //hardcode for offer status
                newPost.OfferStartDate = DateTime.Now;
                newPost.OfferEndDate = DateTime.Now;

                ViewBag.CatList = oDBContext.OfferCategories.Where(t => t.IsActive).Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.CategoryName.ToString()
                });

                if (!IsCurrentUserAdmin())
                {
                    ViewBag.LocList = oDBContext.OfferLocOutletMaps.Where(t => t.IsActive && t.SysUser.ID == oCurrentUser.ID).Select(x => x.OfferLocation).Distinct().ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.LocationName.ToString()
                    });
                }
                else
                {
                    ViewBag.LocList = oDBContext.OfferLocOutletMaps.Where(t => t.IsActive).Select(x => x.OfferLocation).Distinct().ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.LocationName.ToString()
                    });
                }

                if (IsCurrentUserAdmin())
                {

                    ViewBag.FeatureList = oDBContext.SysEnums.Where(t => t.EnumType == enumFeature).Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.EnumName.ToString()
                    });
                }
                else
                {
                    ViewBag.FeatureList = oDBContext.SysEnums.Where(t => t.EnumType == enumFeature  && t.EnumName.ToLower().Contains("low")).Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.EnumName.ToString()
                    });
                }

                return PartialView("_createPostPartial", newPost);
            }
            else
                return RedirectToAction("SignIn", "SignIn");
        }

        [HttpGet]
        public ActionResult GetAvailableOutletList(int OfferId, int LocId)
        {
            oCurrentUser = (SysUser)Session["User"];
            var availOutletList = new List<CatLocAvailableOutlet>();
            if (!IsCurrentUserAdmin())
            {
                availOutletList = oDBContext.OfferLocOutletMaps.Where(t => t.IsActive && t.LocationID == LocId && t.UserId == oCurrentUser.ID ).Select(t => new CatLocAvailableOutlet
                {
                    offerId = t.OfferAvailOutlets.FirstOrDefault().OfferId,
                    outletLocationId = t.LocationID,
                    outletId = t.ID,
                    outletName = t.OutletName,
                    outletAddress = t.OutletAddress,
                    outletLocation = t.OfferLocation.LocationName,
                    outletIsActive = (t.OfferAvailOutlets.FirstOrDefault(x => x.OfferId == OfferId && x.OutletID == t.ID) != null) ? true : false

                }).ToList();
            }
            else
            {
                var offerUserid = oDBContext.OffersInfoes.FirstOrDefault(t => t.ID == OfferId).SysUser.ID;
                availOutletList = oDBContext.OfferLocOutletMaps.Where(t => t.IsActive && t.LocationID == LocId && t.UserId == offerUserid).Select(t => new CatLocAvailableOutlet
                {
                    offerId = t.OfferAvailOutlets.FirstOrDefault(x => x.OfferId == OfferId).OfferId,
                    outletLocationId = t.LocationID,
                    outletId = t.ID,
                    outletName = t.OutletName,
                    outletAddress = t.OutletAddress,
                    outletLocation = t.OfferLocation.LocationName,
                    outletIsActive = (t.OfferAvailOutlets.FirstOrDefault(x => x.OfferId == OfferId && x.OutletID == t.ID) != null) ? true : false

                }).ToList();

                //availOutletList = oDBContext.OfferAvailOutlets.Where(t => t.IsActive && t.OfferLocOutletMap.LocationID == LocId).Select(t => new CatLocAvailableOutlet
                //{
                //    offerId = t.OfferId,
                //    outletLocationId = t.OfferLocOutletMap.LocationID,
                //    outletId = t.OfferLocOutletMap.ID,
                //    outletName = t.OfferLocOutletMap.OutletName,
                //    outletAddress = t.OfferLocOutletMap.OutletAddress,
                //    outletLocation = t.OfferLocOutletMap.OfferLocation.LocationName,
                //    outletIsActive = true

                //}).ToList();
            }
            return PartialView("_postLocOutletsPartial", availOutletList);
        }

        [HttpGet]
        public ActionResult GetMorePost(int pageNo, string[] CategoryId, string[] LocationId, string SearchWord, string[] PostStatus)
        {
            oCurrentUser = (SysUser)Session["User"];
            var morePostList = GetOffersInfo();
            if (morePostList != null && morePostList.Count > 0)
            {
                if (CategoryId != null && CategoryId.Count() > 0)
                    morePostList = morePostList.Where(t => CategoryId.Contains(t.OfferCat.ToString())).ToList();
                if (LocationId != null && LocationId.Count() > 0)
                    morePostList = morePostList.Where(t => LocationId.Contains(t.OfferLoc.ToString())).ToList();

                if (!string.IsNullOrEmpty(SearchWord))
                    morePostList = morePostList.Where(t => t.PostName.ToLower().Contains(SearchWord) || t.OfferDetails.ToLower().Contains(SearchWord) || t.OfferDiscountAmt.ToLower().Contains(SearchWord) || t.OfferCategory.CategoryName.ToLower().ToLower().Contains(SearchWord) || t.OfferLocation.LocationName.ToLower().Contains(SearchWord)).ToList();

                if (PostStatus != null && PostStatus.Count() == 1)
                {
                    var postIsActive = Convert.ToBoolean(PostStatus[0].ToString());
                    morePostList = morePostList.Where(t => t.IsActive == postIsActive).ToList();
                }

                morePostList = morePostList.Skip(pageSize * pageNo).Take(pageSize).ToList();
            }
            return PartialView("_postListMorePartial", morePostList);
        }

        [HttpGet]
        public ActionResult GetPostByCategoryLocation(string[] CategoryId, string[] LocationId, string SearchWord, string[] PostStatus)
        {
            oCurrentUser = (SysUser)Session["User"];
            SearchWord = SearchWord.Trim().ToLower();
            var searchPostList = new List<OffersInfo>();
            searchPostList = GetOffersInfo();
            if (searchPostList != null && searchPostList.Count > 0)
            {
                if (CategoryId != null && CategoryId.Count() > 0)
                    searchPostList = searchPostList.Where(t => CategoryId.Contains(t.OfferCat.ToString())).ToList();
                if (LocationId != null && LocationId.Count() > 0)
                    searchPostList = searchPostList.Where(t => LocationId.Contains(t.OfferLoc.ToString())).ToList();

                if (!string.IsNullOrEmpty(SearchWord))
                    searchPostList = searchPostList.Where(t => t.PostName.ToLower().Contains(SearchWord) || t.OfferDetails.ToLower().Contains(SearchWord) || t.OfferDiscountAmt.ToLower().Contains(SearchWord) || t.OfferCategory.CategoryName.ToLower().ToLower().Contains(SearchWord) || t.OfferLocation.LocationName.ToLower().Contains(SearchWord)).ToList();
                if (PostStatus != null && PostStatus.Count() == 1)
                {
                    var postIsActive = Convert.ToBoolean(PostStatus[0].ToString());
                    searchPostList = searchPostList.Where(t => t.IsActive == postIsActive).ToList();
                }
            }

            return PartialView("_postListPartial", searchPostList);
        }

        [HttpPost]
        public ActionResult InsertPost(OffersInfo oOffersInfo, HttpPostedFileBase ctrlPostImage, FormCollection newFormCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oSysCurrentUser = (SysUser)Session["User"];
                    var offerNew = oDBContext.OffersInfoes.FirstOrDefault(t => t.ID == oOffersInfo.ID);
                    var uploadFileName = "";
                    if (offerNew == null)
                    {
                        offerNew = new OffersInfo();
                        offerNew.PostName = oOffersInfo.PostName;
                        offerNew.OfferDiscountAmt = oOffersInfo.OfferDiscountAmt;
                        offerNew.OfferStartDate = oOffersInfo.OfferStartDate;
                        offerNew.OfferEndDate = oOffersInfo.OfferEndDate;
                        offerNew.OfferCat = oOffersInfo.OfferCat;
                        offerNew.OfferLoc = oOffersInfo.OfferLoc;
                        offerNew.OfferDetails = oOffersInfo.OfferDetails;
                        offerNew.OfferFeatureVal = oOffersInfo.OfferFeatureVal;
                        if (ctrlPostImage != null)
                        {
                            uploadFileName = DateTime.Now.ToString("yyddMMhhmmssfff") + "_" + Path.GetFileName(ctrlPostImage.FileName).Replace(" ", "").ToLower().Trim();
                            offerNew.OfferImagePath = postImagePath + uploadFileName;
                            ctrlPostImage.SaveAs(Path.Combine(Server.MapPath(postImagePath), uploadFileName));
                        }
                        else
                        {
                            offerNew.OfferImagePath = postImagePath + defaultImageFileName;
                        }
                        offerNew.CreatedBy = oSysCurrentUser.ID;
                        offerNew.CreatedOn = DateTime.Now; ;
                        offerNew.IsActive = oOffersInfo.IsActive;
                        offerNew.OfferStatus = oOffersInfo.OfferStatus;

                        oDBContext.OffersInfoes.Add(offerNew);
                        oDBContext.SaveChanges();

                        SaveAvailableOutlet(newFormCollection, offerNew.ID.ToString());
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
        public ActionResult UpdatePost(OffersInfo oOffersInfo, HttpPostedFileBase ctrlPostImage, FormCollection updateFormCollection, string chkIsActive)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oSysCurrentUser = (SysUser)Session["User"];
                    var offerUpdate = oDBContext.OffersInfoes.FirstOrDefault(t => t.ID == oOffersInfo.ID);
                    var uploadFileName = "";
                    if (offerUpdate != null && SaveAvailableOutlet(updateFormCollection, offerUpdate.ID.ToString()))
                    {
                        offerUpdate.PostName = oOffersInfo.PostName;
                        offerUpdate.OfferDiscountAmt = oOffersInfo.OfferDiscountAmt;
                        offerUpdate.OfferStartDate = oOffersInfo.OfferStartDate;
                        offerUpdate.OfferEndDate = oOffersInfo.OfferEndDate;
                        offerUpdate.OfferCat = oOffersInfo.OfferCat;
                        offerUpdate.OfferLoc = oOffersInfo.OfferLoc;
                        offerUpdate.OfferDetails = oOffersInfo.OfferDetails;
                        if (IsCurrentUserAdmin())
                        {
                            offerUpdate.OfferFeatureVal = oOffersInfo.OfferFeatureVal.Value;
                        }
                        offerUpdate.IsActive = (!string.IsNullOrEmpty(chkIsActive) && chkIsActive.Contains("on")) ? true : false;
                        if (Request.Files["ctrlPostImage"].ContentLength > 0)
                        {
                            if (ctrlPostImage != null)
                            {
                                uploadFileName = DateTime.Now.ToString("yyddMMhhmmssfff") + "_" + Path.GetFileName(ctrlPostImage.FileName).Replace(" ", "").ToLower().Trim();
                                offerUpdate.OfferImagePath = postImagePath + uploadFileName;
                                ctrlPostImage.SaveAs(Path.Combine(Server.MapPath(postImagePath), uploadFileName));
                            }
                            else
                            {
                                offerUpdate.OfferImagePath = postImagePath + defaultImageFileName;
                            }
                        }
                        else {
                            offerUpdate.OfferImagePath = offerUpdate.OfferImagePath;
                        }
                        offerUpdate.ModifiedBy = oSysCurrentUser.ID;
                        offerUpdate.ModifiedOn = DateTime.Now;
                        oDBContext.SaveChanges();

                        TempData["SuccessMsg"] = "Data Updated Successfully";
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

        private bool SaveAvailableOutlet(FormCollection checkedOutlet, string OfferId)
        {

            bool isOpComplete = true;

            //var OfferId = (!string.IsNullOrEmpty(checkedOutlet["ID"].ToList()[0].ToString())) ? checkedOutlet["ID"].ToList()[0].ToString() : "0";
            oDBContext.Database.ExecuteSqlCommand("Delete from OfferAvailOutlet where OfferId = {0}", OfferId);
            oDBContext.SaveChanges();

            var selectedOutletId = checkedOutlet.GetValues("chkAvailOutlet").ToList();
            foreach (string oOutLetId in selectedOutletId)
            {
                OfferAvailOutlet oOfferAvailOutlet = new OfferAvailOutlet();
                oOfferAvailOutlet.OfferId = Convert.ToInt32(OfferId);
                oOfferAvailOutlet.OutletID = Convert.ToInt32(oOutLetId);
                oOfferAvailOutlet.IsActive = true;
                oOfferAvailOutlet.CreatedBy = oSysCurrentUser.ID;
                oOfferAvailOutlet.CreatedOn = DateTime.Now;
                oDBContext.OfferAvailOutlets.Add(oOfferAvailOutlet);
                oDBContext.SaveChanges();
            }
            return isOpComplete;
        }
    }
}