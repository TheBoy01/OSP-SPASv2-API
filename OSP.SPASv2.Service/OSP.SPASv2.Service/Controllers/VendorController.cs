using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using OSP.SPASv2.Domain;
using OSP.SPASv2.Service.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _1SP.SPASv2.Service.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        ServiceUnit _ServiceUnit;

        public VendorController()
        {
            _ServiceUnit = new ServiceUnit();
        }

        // GET: api/<VendorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {  
            return new string[] { "value1", "value2" };
        }

        // GET api/<VendorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VendorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VendorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VendorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("GenerateNewVendorCode")]
        public async Task<string> GenerateNewPRNo(string LatestVendorCode)
        {
            var str = await _ServiceUnit.VendorMaintenanceService.GenerateVendorCode(LatestVendorCode);

            return str;
        }

        //[Authorize]
        //[HttpPost("SearchVendor")]
        //public async Task<ActionResult> SearchVendor(TblVendor tblVendor)
        //{
        //    IEnumerable<TblVendor> Vendorlist = new List<TblVendor>{
        //        new TblVendor() { VendorID = "1", Name = "JDC"} ,
        //        new TblVendor() { VendorID = "2", Name = "BDO" } ,

        //        new TblVendor() { VendorID = "5", Name = "INK SALES"  }

        //    };


        //    return Ok(Vendorlist);
        //}


        //public async Task<ActionResult<IEnumerable<TblVendor>>> Company_Bind(string CompanyID)
        //{
        //    SampleWaServiceClient client1 = new SampleWaServiceClient();
        //    IEnumerable<ServiceReference1.RefBranch> _refloc;

        //    _refloc = await client1.GetBranchAsync();

        //    List<SelectListItem> Branchlist = new List<SelectListItem>();
        //    foreach (var item in _refloc)
        //    {
        //        Branchlist.Add(new SelectListItem { Text = item.BranchDesc.ToString(), Value = item.Branchcode.ToString() });
        //    }
        //    //return Json(citylist, JsonRequestBehavior.AllowGet);
        //    // return Json((new SelectList(Branchlist, "Branchcode", "BranchDesc")));
        //    //return new List<RefBranch>(_refloc);

        //    return Ok(new SelectList(_refloc, "BranchDesc", "Branchcode"));



        //}
    }
}
