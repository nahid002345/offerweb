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
    [CustomAuthorize(Roles = "administrator")]
    public class LocationController : AppBaseController
    {
        // GET: Location
        public List<OfferLocation> oLocationList = null;
        public OfferLocation oOfferLocation = null;
        public ActionResult Index()
        {
            oCurrentUser = (SysUser)Session["User"];
            oLocationList = oDBContext.OfferLocations.ToList();
            return View(oLocationList);
        }

        [HttpGet]
        public ActionResult NewLocation()
        {
            oCurrentUser = (SysUser)Session["User"];
            var newLocation = new OfferLocation();
            newLocation.IsActive = false;
            var LocationInfo = new LocationDetailModel();
            LocationInfo.IsNew = true;
            LocationInfo.Location = newLocation;

            ViewBag.LocationList = oDBContext.OfferLocations.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.LocationName.ToString()
            });

            return PartialView("_locationDetailPartial", LocationInfo);
        }

        [HttpGet]
        public ActionResult UpdateLocation(int id)
        {

            var LocationInfo = new LocationDetailModel();
            LocationInfo.IsNew = false;
            LocationInfo.Location = oDBContext.OfferLocations.FirstOrDefault(t => t.ID == id);
            ViewBag.LocationList = oDBContext.OfferLocations.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.LocationName.ToString()
            });
            return PartialView("_locationDetailPartial", LocationInfo);
        }


        [HttpPost]
        public ActionResult InsertLocation(LocationDetailModel oLocationInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oCurrentUser = (SysUser)Session["User"];
                    var existLocation = oDBContext.OfferLocations.FirstOrDefault(t => t.ID == oLocationInfo.Location.ID);
                    if (existLocation == null)
                    {
                        OfferLocation oOfferLocation = oLocationInfo.Location;
                        oOfferLocation.IsActive = true;
                        oOfferLocation.CreatedBy = oCurrentUser.ID;
                        oOfferLocation.CreatedOn = DateTime.Now;
                        oDBContext.OfferLocations.Add(oOfferLocation);
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
        public ActionResult UpdateLocation(LocationDetailModel oLocationInfo, string chkIsActive)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oCurrentUser = (SysUser)Session["User"];
                    var existLocation = oDBContext.OfferLocations.FirstOrDefault(t => t.ID == oLocationInfo.Location.ID);
                    if (existLocation != null)
                    {
                        existLocation.LocationName = oLocationInfo.Location.LocationName;
                        existLocation.ModifiedBy = oCurrentUser.ID;
                        existLocation.ModifiedOn = DateTime.Now;
                        existLocation.IsActive = (!string.IsNullOrEmpty(chkIsActive) && chkIsActive.Contains("on")) ? true : false;
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