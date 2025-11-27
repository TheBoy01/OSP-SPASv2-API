//using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Web.APIServices;
using OSP.SPASv2.Web.APIServices.Services;
using OSP.SPASv2.Web.Utility;
//using OSP.SPASv2.Domain;
//using OSP.SPASv2.DomainDummy.Models;
//using OSP.SPASv2.RepositoryUnit;
using SPASv2.Models;
using System.Text.Json;
//using OSP.SPASv2.ServiceUnit;
//using OSP.SPASv2.RepositoryUnit;

namespace SPASv2.Controllers
{
    public class VendorMaintenanceController : Controller
    {
        // TblVendorFoo _TblVendorFoo = new TblVendorFoo();

        // GET: VendorMaintenanceController
        private readonly ILogger<PaymentRequestController> _logger;
        private RepositoryUnit _RepositoryUnit;
        private ServiceUnit _ServiceUnit;
        public string errorMessage = "";

        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;

        private IConfiguration configuration;
        public VendorMaintenanceController(ILogger<PaymentRequestController> logger, IConfiguration _configuration)
        {
            _logger = logger;
            _RepositoryUnit = new RepositoryUnit();
            _ServiceUnit = new ServiceUnit();

            configuration = _configuration;

            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: VendorMaintenanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult VendorItems(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateVendor(TblVendor TblVendor, List<TblVendorAddress> TblVendorAddressList)
        {
            try
            {
                string _VendorCode = string.Empty;
                string _GetLatestVendorCode = string.Empty;
                _GetLatestVendorCode = await _RepositoryUnit.VendorRepository.GetNewGeneratedVendorCode(BaseUrlRepo);
                _VendorCode = await _ServiceUnit.VendorService.GenerateNewVendorCode(_GetLatestVendorCode);
                TblVendor.AuditUser = "MAIN OHARDCODE";
                TblVendor.AuditDate = DateTime.Now;
                TblVendor.VendorCode = _VendorCode; //DateTime.Now.ToString("MMdyyhs");
                TblVendor.Active = true;
                if (string.IsNullOrEmpty(TblVendor.FirstName))
                {
                    TblVendor.FirstName = "";
                    TblVendor.MiddleName = "";
                    TblVendor.LastName = "";
                }
                else
                {
                    TblVendor.DisplayName = TblVendor.FirstName + " " + TblVendor.MiddleName + " " + TblVendor.LastName;
                }

                var response = await _RepositoryUnit.VendorRepository.CreateVendor(BaseUrlRepo, TblVendor);
                response = await _RepositoryUnit.VendorRepository.CreateAddress(BaseUrlRepo, TblVendorAddressList, _VendorCode);

                //if (response.IsSuccessStatusCode)
                //{
                // Handle success
                return Ok();
                //}
                //else
                //{
                //    // Handle error
                //    return BadRequest();
                //}

            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: VendorMaintenanceController/Create
        public async Task<ActionResult> CreateVendor()
        {
            VendorMaintenanceModel _VendorMaintenanceModel =  await LoadReferences();

            return View(_VendorMaintenanceModel);
        }

        public async Task<ActionResult> EditVendor()
        {
            VendorMaintenanceModel _VendorMaintenanceModel = await LoadReferences();

            return View(_VendorMaintenanceModel);
        }

        public async Task<VendorMaintenanceModel> LoadReferences()
        {
            VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();

            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            _VendorMaintenanceModel.VendorTypeList = await _RepositoryUnit.VendorRepository.GetVendorTypeList(BaseUrlRepo);
            _VendorMaintenanceModel.AddressTypeList = await _RepositoryUnit.VendorRepository.GetAddressTypeList(BaseUrlRepo);
            _VendorMaintenanceModel.BankAccTypeList = await _RepositoryUnit.VendorRepository.GetBankAccTypeList(BaseUrlRepo);
            _VendorMaintenanceModel.RefATCList = await _RepositoryUnit.VendorRepository.GetRefATCList(BaseUrlRepo);
            _VendorMaintenanceModel.RefVendorDocsList = await _RepositoryUnit.VendorRepository.GetVendorDocsList(BaseUrlRepo);

            return _VendorMaintenanceModel;
        }

       

        [HttpGet]
        public async Task<IActionResult> VendorList()
        {

            VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();

            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorList("");
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorLists("");

            return View(_VendorMaintenanceModel);
            //return View();
        }

        [HttpGet]
        public async Task<IActionResult> _VendorCreateAddress()
        {
            VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();

            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            _VendorMaintenanceModel.AddressTypeList = await _RepositoryUnit.VendorRepository.GetAddressTypeList(BaseUrlRepo);


            return PartialView(_VendorMaintenanceModel);
        }

        [HttpGet]
        public async Task<IActionResult> _VendorLookUp()
        {
            VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();

            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorLists("");


            return PartialView(_VendorMaintenanceModel);
        }

        // POST: VendorMaintenanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TblVendor _TblVendor)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VendorMaintenanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VendorMaintenanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: VendorMaintenanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VendorMaintenanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> VendorAutoComplete(string VendorName)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();

                //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
                //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorList("");
                _VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorLists("");
                //return Json(new SelectList(result));
                return Json(new SelectList(_VendorMaintenanceModel.qryVendorList, "VendorName", "VendorCode"));
                //return View();
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> BankAutoComplete(string prefix)
        {
            var Banks = await _RepositoryUnit.VendorRepository.GetBankList(BaseUrlRepo);
            var FilteredBanks = (from bank in Banks
                                 where bank.BankName.ToUpper().StartsWith(prefix.ToUpper())
                                 select new
                                 {
                                     label = bank.BankName,
                                     val = bank.BankCode
                                 }).ToList();

            return Json(FilteredBanks);
        }

        public async Task<IActionResult> EditVendorDoc(TblVendorAttached TblVendorAttached, string selected_div, string ItemID)
        {
            try
            {
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();
                _VendorMaintenanceModel.TblVendorAttached = TblVendorAttached;
                _VendorMaintenanceModel.SelectedDiv = selected_div;
                _VendorMaintenanceModel.ItemID = ItemID;

                _VendorMaintenanceModel.RefVendorDocsList = await _RepositoryUnit.VendorRepository.GetVendorDocsList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefVendorDocsList.OrderBy(x => x.DocDesc == selected_docdesc).ToList();
                return  PartialView("_VendorDocList", _VendorMaintenanceModel);
            }
            catch (Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        public async Task<IActionResult> CreateVendorDoc(string selected_div)
        {
            try
            {
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel(); 
                _VendorMaintenanceModel.SelectedDiv = selected_div;

                _VendorMaintenanceModel.RefVendorDocsList = await _RepositoryUnit.VendorRepository.GetVendorDocsList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefVendorDocsList.OrderBy(x => x.DocDesc == selected_docdesc).ToList();
                return PartialView("_VendorDocList", _VendorMaintenanceModel);
            }
            catch (Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        public async Task<IActionResult> CreateVendorRequiredDoc(string selected_div)
        {
            try
            {
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();
                _VendorMaintenanceModel.SelectedDiv = selected_div;

                _VendorMaintenanceModel.RefVendorDocsList = await _RepositoryUnit.VendorRepository.GetVendorDocsList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefVendorDocsList.OrderBy(x => x.DocDesc == selected_docdesc).ToList();
                return PartialView("_VendorDocRequired", _VendorMaintenanceModel);
            }
            catch (Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        public async Task<IActionResult> EditVendorRequiredDoc(TblVendorDocRequired TblVendorDocRequired, string _selected_div, string _ItemID)
        {
            try
            {
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();
                _VendorMaintenanceModel.SelectedDiv = _selected_div;
                _VendorMaintenanceModel.TblVendorDocRequired = TblVendorDocRequired;
                _VendorMaintenanceModel.ItemID = _ItemID;

                _VendorMaintenanceModel.RefVendorDocsList = await _RepositoryUnit.VendorRepository.GetVendorDocsList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefVendorDocsList.OrderBy(x => x.DocDesc == selected_docdesc).ToList();
                return PartialView("_VendorDocRequired", _VendorMaintenanceModel);
            }
            catch (Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        public async Task<IActionResult> CreateVendorATC(string selected_div)
        {
            try
            {
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();
                _VendorMaintenanceModel.SelectedDiv = selected_div;

                _VendorMaintenanceModel.RefATCList = await _RepositoryUnit.VendorRepository.GetRefATCList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefVendorDocsList.OrderBy(x => x.DocDesc == selected_docdesc).ToList();
                return PartialView("_VendorATC", _VendorMaintenanceModel);
            }
            catch (Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        public async Task<IActionResult> CreateVendorAddress(string selected_div)
        {
            try
            {
                VendorMaintenanceModel _VendorMaintenanceModel = new VendorMaintenanceModel();
                _VendorMaintenanceModel.SelectedDiv = selected_div;

                _VendorMaintenanceModel.AddressTypeList = await _RepositoryUnit.VendorRepository.GetAddressTypeList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefRegion = await _RepositoryUnit.VendorRepository.GetRegionList();
                _VendorMaintenanceModel.RefProvince = await _RepositoryUnit.VendorRepository.GetProvinceList(BaseUrlRepo);
                //_VendorMaintenanceModel.RefCity = await _RepositoryUnit.VendorRepository.GetCityList();
                //_VendorMaintenanceModel.RefBrgy = await _RepositoryUnit.VendorRepository.GetBrgyList();

                return PartialView("_VendorAddress", _VendorMaintenanceModel);
            }
            catch (Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<RefCity>>> GetCityListByProvinceCode(string ProvinceCode)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<RefCity> vlist = await _RepositoryUnit.VendorRepository.GetCityByProvinceCode(BaseUrlRepo,ProvinceCode); 

                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                ////return Json(new SelectList(vlist, "VendorName", "VendorCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                //return Json(new { result = false, error = ex.Message });
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<RefBrgy>>> GetBrgyListByCityCode(string CityCode)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<RefBrgy> vlist = await _RepositoryUnit.VendorRepository.GetBrgyListByCityCode(BaseUrlRepo, CityCode);

                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                ////return Json(new SelectList(vlist, "VendorName", "VendorCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                //return Json(new { result = false, error = ex.Message });
                return BadRequest(ex.Message);
            }

        }


    }
}
