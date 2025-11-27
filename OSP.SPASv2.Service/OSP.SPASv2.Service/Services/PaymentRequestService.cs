using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OSP.SPASv2.Domain.View;
using System;
using System.Text.RegularExpressions;


namespace OSP.SPASv2.Service.Services
{
    public class PaymentRequestService
    {
        ServiceUnit _ServiceUnit = new ServiceUnit();

        public async Task<string> GeneratePaymentrequestno(string lastno, string companycode, string branchcode, DateTime auditdate)
        {
            string paymentreqno = "";
            //paymentreqno= lastno.Substring(lastno.Length-6)+ 1;
            if (lastno.Length < 6)
            {
                paymentreqno = lastno;
            }
            else
            {
                paymentreqno = lastno.Substring(lastno.Length - 6);
            }

            paymentreqno = Convert.ToString(Convert.ToInt16(paymentreqno) + 1);
            paymentreqno = companycode + branchcode + auditdate.ToString("yyMM") + '-' + paymentreqno.PadLeft(6, '0');
            //paymentreqno =   companycode +branchcode+'-'+Convert.ToString(Convert.ToInt16(lastno.Substring(lastno.Length-6) + 1).PadLeft(6, '0');
            return await Task.FromResult(paymentreqno.ToUpper());
        }



        public async Task<qryComputeBreakdown> ComputeBreakDown(qryComputeBreakdown _qry)
        {
            try
            {
                qryComputeBreakdown _ComputeBreakDown = new qryComputeBreakdown();
                decimal Gross = _qry.Qty * _qry.Gross;
                decimal NetOfVAT = 0;
                decimal AmountDue = Gross;
                decimal Discount = 0;
                decimal Vat = 0;
                if (!string.IsNullOrEmpty(_qry.Disccode))
                {
                    if (_qry.Disccode.ToUpper() == "001")
                    {
                        AmountDue = Math.Round(Gross - (Gross * (_qry.Discount / 100)), 4,MidpointRounding.AwayFromZero);
                        Vat = Math.Round(AmountDue - (AmountDue / _qry.VatRate), 4,MidpointRounding.AwayFromZero);
                        Discount = Math.Round(Gross * (_qry.Discount / 100),4, MidpointRounding.AwayFromZero);
                        NetOfVAT =AmountDue / _qry.VatRate;
                        //AmountDue = Convert.ToDecimal(string.Format("{0:F2}", Gross - (Gross * (_qry.Discount / 100))));
                        //Vat = Convert.ToDecimal(string.Format("{0:F2}", AmountDue - (AmountDue / _qry.VatRate)));
                        //Discount = Convert.ToDecimal(string.Format("{0:F2}", Gross * (_qry.Discount / 100)));
                        //NetOfVAT = Convert.ToDecimal(string.Format("{0:F2}", AmountDue / _qry.VatRate));

                    }
                    else if (_qry.Disccode.ToUpper() == "002")
                    {
                        AmountDue = Math.Round(( Gross - _qry.Discount),4, MidpointRounding.AwayFromZero);
                        Vat = Math.Round((AmountDue - (AmountDue / _qry.VatRate)),4, MidpointRounding.AwayFromZero);
                        Discount = _qry.Discount;
                        NetOfVAT = AmountDue - Vat;
                    }
                }

                if (!_qry.isVAT)
                {
                    Vat = 0;
                    AmountDue = Gross; //Gross * _qry.Qty;
                    NetOfVAT = AmountDue;
                    _qry.VatRate = 0;
                }

                _ComputeBreakDown.Gross = Gross;
                _ComputeBreakDown.Vat = Vat;
                _ComputeBreakDown.NetOfVAT = NetOfVAT;
                _ComputeBreakDown.Discount = Discount;
                _ComputeBreakDown.AmountDue = AmountDue;
                _ComputeBreakDown.Qty = _qry.Qty;
                _ComputeBreakDown.Disccode = _qry.Disccode;
                _ComputeBreakDown.VatRate = _qry.VatRate;



                return await Task.FromResult(_ComputeBreakDown);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<qryRequisitionHdrComputation> ComputeBreakDownHdr(List<qryRequisitionDtl> tmp)
        {
            try
            {
                qryComputeBreakdown qryComputeBreakdown = new qryComputeBreakdown();

                var qry = new qryRequisitionHdrComputation();
                for (int i = 0; i < tmp.Count; i++)
                {

                    qry.Gross += tmp[i].Gross;
                    qry.Vat += tmp[i].VAT;
                    qry.NetOfVat +=  tmp[i].NetOfVAT;
                    qry.Discount += tmp[i].Discount;
                    qry.TotalTax +=  tmp[i].TotalTax;
                    qry.Deduction += tmp[i].Deduction;


                    qry.AmountDue += tmp[i].TotalAmount;
                }
                qry.AmountDue = qry.AmountDue  - (qry.Deduction + qry.Discount + qry.TotalTax);

                //  IActionResult result = new OkObjectResult(qry);
                return await Task.FromResult(qry);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public async Task<decimal> ComputeTotalAmountItems(IList<tmpPaymentRequestInventory> tmp)
        {
            decimal totalAmountItems = 0;

            foreach (tmpPaymentRequestInventory item in tmp)
            {
                totalAmountItems = totalAmountItems + item.TotalAmt;
            }

            return await Task.FromResult(totalAmountItems);
        }

        public async Task<string> GenerateBatchNo(string LastNo, DateTime AuditDate)
        {
            string BatchReqNo = "";
            //paymentreqno= lastno.Substring(lastno.Length-6)+ 1;
            if (LastNo.Length < 6)
            {
                BatchReqNo = LastNo;
            }
            else
            {
                BatchReqNo = LastNo.Substring(LastNo.Length - 6);
            }

            BatchReqNo = Convert.ToString(Convert.ToInt16(BatchReqNo) + 1);
            BatchReqNo = "BN" + AuditDate.ToString("yyMM") + BatchReqNo.PadLeft(6, '0');//companycode + branchcode + auditdate.ToString("yyMM") + '-' + BatchReqNo.PadLeft(6, '0');
            //paymentreqno =   companycode +branchcode+'-'+Convert.ToString(Convert.ToInt16(lastno.Substring(lastno.Length-6) + 1).PadLeft(6, '0');
            return await Task.FromResult(BatchReqNo.ToUpper());
        }
    }




}

