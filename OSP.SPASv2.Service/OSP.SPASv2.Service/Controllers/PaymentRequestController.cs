using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OSP.SPASv2.Service.Model;
using OSP.SPASv2.Service;
using OSP.SPASv2.Domain;

using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Service.Services;
using System.Text.RegularExpressions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSP.SPASv2.Service.Controllers
{


    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestController : ControllerBase
    {

        //private readonly JWTAuthenticationManager jwtAuthenticationManager;
        ServiceUnit _ServiceUnit;

        public PaymentRequestController()
        {
            //this.jwtAuthenticationManager = jwtAuthenticationManager;
            _ServiceUnit = new ServiceUnit();
        }



        //[Authorize]
        //[HttpPost("SearchBranch")]
        //public  IList<RefBranch> SearchBranch(RefBranch refBranch)
        //{
        //    IList<RefBranch> Vendorlist = new List<RefBranch>{
        //        new RefBranch() { Branchcode = "1", BranchDesc = "QUEZON AVE"} ,
        //        new RefBranch() { Branchcode = "2", BranchDesc = "CEBU" } ,

        //        new RefBranch() { Branchcode = "5", BranchDesc = "DAVAO"  }
        //    };


        //    return Vendorlist;
        //}


        [HttpGet("GenerateNewPRNo")]
        public async Task<string> GenerateNewPRNo(string lastno, string companycode, string branchcode, string auditdate)
        {
            var str = await _ServiceUnit.PaymentRequestService.GeneratePaymentrequestno(lastno, companycode, branchcode, Convert.ToDateTime(auditdate));

            return str;
        }

        [HttpGet("ComputeTotalAmountItems")]
        public async Task<decimal> ComputeTotalAmountItems(IList<tmpPaymentRequestInventory> tmp)
        {
            var str = await _ServiceUnit.PaymentRequestService.ComputeTotalAmountItems(tmp);

            return str;
        }


        //[HttpGet("GenerateNewBatchNo")]
        //public async Task<string> GenerateNewPRNo(string lastno, string auditdate)
        //{
        //    var str = await _ServiceUnit.PaymentRequestService.GenerateBatchNo(lastno, Convert.ToDateTime(auditdate));

        //    return str;
        //}

        [HttpGet("ComputeBreakDown")]
        //public async Task<qryComputeBreakdown> ComputeBreakDown(int qty, decimal gross, decimal vatrate, decimal discount, string discountcode)
        public async Task<qryComputeBreakdown> ComputeBreakDown(qryComputeBreakdown _qry)
        {
            try
            {
                //var qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(qty, gross, vatrate, discount, discountcode);
                var qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(_qry);
                return qry;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost("ComputeBreakDown")]
        //public async Task<qryComputeBreakdown> ComputeBreakDown(int qty, decimal gross, decimal vatrate, decimal discount, string discountcode)
        public async Task<TblAPIResponse<qryComputeBreakdown>> ComputeBreakDown1(qryComputeBreakdown _qry)
        {
            try
            {
                //var qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(qty, gross, vatrate, discount, discountcode);
                var qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(_qry);

                return new TblAPIResponse<qryComputeBreakdown>
                {
                    StatusCode = "200", // Assuming a success status code
                    StatusDesc = "Success",
                    Data = qry,
                   
                };

                //return qry;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost("ComputeHdrBreakDown")]
        //public async Task<qryComputeBreakdown> ComputeBreakDown(int qty, decimal gross, decimal vatrate, decimal discount, string discountcode)
        public async Task<qryRequisitionHdrComputation> ComputeHdrBreakDown(List<qryRequisitionDtl> qryDtlList)
        {
            try
            {   
                //var qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(qty, gross, vatrate, discount, discountcode);
                var qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(qryDtlList);
                return qry;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GenerateBatchNo")]
        public async Task<string> GenerateBatchNo(string lastno, DateTime AuditDate)
        {
            var str = await _ServiceUnit.PaymentRequestService.GenerateBatchNo(lastno, AuditDate);

            return str;
        }



    }
}
