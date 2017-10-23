using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfferWebSystem.EFnClass;
using OfferWebSystem.Models;
namespace OfferWebSystem.Controllers
{
    [SessionExpire]
    public class OutletController : AppBaseController
    {
        // GET: Outlet
        public List<OfferLocOutletMap> oOutletList = null;
        public OfferLocOutletMap oOfferLocOutletMap = null;

        private List<OfferLocOutletMap> GetOutletList()
        {
            var outletList = new List<OfferLocOutletMap>();
            if (IsCurrentUserAdmin())
                outletList = oDBContext.OfferLocOutletMaps.OrderByDescending(t => t.ID).ToList();
            else
                outletList = oDBContext.OfferLocOutletMaps.Where(t => t.UserId == oCurrentUser.ID).OrderByDescending(t => t.ID).ToList();
            return outletList;
        }
        public ActionResult Index()
        {
            var outletIndexModel = new OutletListModel();
            outletIndexModel.IsUserAdmin = IsCurrentUserAdmin();
            outletIndexModel.OutletList = GetOutletList();
            return View(outletIndexModel);
        }

        [HttpGet]
        public ActionResult NewOutlet()
        {
            oCurrentUser = (SysUser)Session["User"];
            var newOutlet = new OfferLocOutletMap();
            newOutlet.IsActive = true;
            newOutlet.UserId = oCurrentUser.ID;
            
            var outletInfo = new OutletDetailModel();
            outletInfo.IsNew = true;
            outletInfo.Outlet = newOutlet;

            ViewBag.LocationList = oDBContext.OfferLocations.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.LocationName.ToString()
            });

            return PartialView("_outletDetailPartial", outletInfo);
        }

        [HttpGet]
        public ActionResult UpdateOutlet(int id)
        {

            var outletInfo = new OutletDetailModel();
            outletInfo.IsNew = false;
            outletInfo.Outlet = oDBContext.OfferLocOutletMaps.FirstOrDefault(t => t.ID == id);

            ViewBag.LocationList = oDBContext.OfferLocations.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.LocationName.ToString()
            });

            return PartialView("_outletDetailPartial", outletInfo);
        }


        [HttpPost]
        public ActionResult InsertOutlet(OutletDetailModel oOutletInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oCurrentUser = (SysUser)Session["User"];
                    var existOutlet = oDBContext.OfferLocOutletMaps.FirstOrDefault(t => t.ID == oOutletInfo.Outlet.ID);

                    if (existOutlet == null)
                    {
                        OfferLocOutletMap oOfferLocOutletMap = oOutletInfo.Outlet;
                        oOfferLocOutletMap.IsActive = true;
                        oOfferLocOutletMap.CreatedBy=oCurrentUser.ID;
                        oOfferLocOutletMap.CreatedOn=DateTime.Now;
                        oDBContext.OfferLocOutletMaps.Add(oOfferLocOutletMap);
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
        public ActionResult UpdateOutlet(OutletDetailModel oOutletInfo, string chkIsActive)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oCurrentUser = (SysUser)Session["User"];
                    var existOutlet = oDBContext.OfferLocOutletMaps.FirstOrDefault(t => t.ID == oOutletInfo.Outlet.ID);

                    if (existOutlet != null)
                    {
                        existOutlet.OutletName = oOutletInfo.Outlet.OutletName;
                        existOutlet.OutletAddress = oOutletInfo.Outlet.OutletAddress;
                        existOutlet.LocationID = oOutletInfo.Outlet.LocationID;
                        existOutlet.IsActive = (!string.IsNullOrEmpty(chkIsActive) && chkIsActive.Contains("on")) ? true : false;
                        existOutlet.ModifiedBy = oCurrentUser.ID;
                        existOutlet.ModifiedOn = DateTime.Now;
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


        [HttpGet]
        public ActionResult GetOutletByLocation(string[] LocationId, string SearchWord)
        {
            
            SearchWord = SearchWord.Trim().ToLower();
            var searchOutletList = new List<OfferLocOutletMap>();
            searchOutletList = GetOutletList();
            if (searchOutletList != null && searchOutletList.Count > 0)
            {
                if (LocationId != null && LocationId.Count() > 0)
                {
                    if (LocationId[0].Contains(','))
                        LocationId = LocationId[0].Split(',');
                    searchOutletList = searchOutletList.Where(t => LocationId.Contains(t.LocationID.ToString())).ToList();
                }
                    

                if (!string.IsNullOrEmpty(SearchWord))
                    searchOutletList = searchOutletList.Where(t => t.OutletName.ToLower().Contains(SearchWord) || t.OutletAddress.ToLower().Contains(SearchWord) || t.OfferLocation.LocationName.ToLower().Contains(SearchWord)).ToList();

                //if (page != null)
                //{
                //    searchOutletList = searchOutletList.Skip((page.Value - 1) * 4).ToList();
                //}
            }

            return PartialView("_outletListPartial", searchOutletList);
        }


    }
}