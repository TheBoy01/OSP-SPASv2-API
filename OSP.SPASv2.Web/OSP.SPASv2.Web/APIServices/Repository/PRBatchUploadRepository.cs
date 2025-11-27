using Microsoft.Reporting.Map.WebForms.BingMaps;
using OSP.SPASv2.Web.Utility;
using System;
using System.Data;
using System.Security.Policy;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class PRBatchUploadRepository
    {
        private string APIBaseURLRepo;
        private string APIBaseURLCommonRepo;

        public PRBatchUploadRepository()
        {
             //APIBaseURLRepo = "http://192.168.23.185/SPASv2Repo/api";
            //APIBaseURLCommonRepo = "http://192.168.23.185/OSPRepo/api";
        }

        public async Task<TblRequisitionhdr> GetLatestPRBatchNo(string URL)
        {
            string requestAddress = URL + "/Rudy/GetLatestBatchNo";

            return await UtilitiesHttpClient<TblRequisitionhdr>.GetJsonlist1(requestAddress); 
        }



        public async Task<TblResponse> CreateBatchPRDtl(TblBatchPRDtl batchDtl)
        {
            string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/CreateBatchPRDtl";

            TblResponse response = await UtilitiesHttpClient<TblBatchPRDtl>.PostAsync(batchDtl, requestAddress);
            return response;
        }
        public async Task<TblResponse> CreateBatchPRHdr(TblBatchPRHdr _TblBatchPRHdr)
        {
            string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/CreateBatchPRHdr";

            TblResponse response = await UtilitiesHttpClient<TblBatchPRHdr>.PostAsync(_TblBatchPRHdr, requestAddress);
            return response;
        }

        public async Task<string> CanCreatePRBatchHdr(BatchUploadParams BatchUploadParams)
        {
            try
            {
                string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/CanCreatePRBatchHdr";

                TblResponse response = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<TblResponse>(BatchUploadParams, requestAddress);
                return response.ErrorMessage;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<BatchUploadParams> CanUploadExcelDetails(string URL, BatchUploadParams _BatchUploadParams)
        {
            try
            {
                //TblResponse resp = new TblResponse();
                //resp.ErrorMessage = "INVALID SOMETHING";
                //resp.Status = "FAILED";
                //return resp;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/CanUploadExcelDetails";
                string requestAddress = URL + "/Rudy/CanUploadExcelDetails";

                BatchUploadParams response = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(_BatchUploadParams, requestAddress);
                return response;
                //TblReawait UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<TblResponse>(_BatchUploadParams, requestAddress);
                //return response.ErrorMessage;

         

               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);


            }

           
        }

        public async Task<BatchUploadParams> CanUploadPayment(string URL, BatchUploadParams _BatchUploadParams)
        {

            try
            {
                //TblResponse resp = new TblResponse();
                //resp.ErrorMessage = "INVALID SOMETHING";
                //resp.Status = "FAILED";
                //return resp;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/CanUploadPayment";
                string requestAddress = URL + "/Rudy/CanUploadPayment";

                BatchUploadParams response = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(_BatchUploadParams, requestAddress);
                return response;
                //TblReawait UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<TblResponse>(_BatchUploadParams, requestAddress);
                //return response.ErrorMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        //public async Task<TblResponse> sampleFormFile(List<IFormFile> Files)
        //{
        //    try
        //    {
        //        string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/sampleFormFile";

        //        TblResponse response = await UtilitiesHttpClient<TblResponse>.PostAsyncFormFile(Files, requestAddress);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
