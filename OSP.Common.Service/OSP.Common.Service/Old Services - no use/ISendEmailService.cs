namespace OSP.Common.Service.ServiceContract
{
    public interface ISendEmailService<TEntity> where TEntity : class
    {
        TEntity SendEmail(string email);
   


        //Task<bool> SendMailAsync(TblSendEmail mailData);
        //Task<bool> SendHTMLMailAsync(TblSendEmail mailData);
        //Task<bool> SendMailWithAttachmentsAsync(TblSendEmail mailData);

    }
}
