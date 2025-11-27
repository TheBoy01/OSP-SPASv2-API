using System.Security.Cryptography;
using System.Text;

namespace OSP.Common.Service.OperationContract
{
    public class CommonService
    {
        public int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }

        public int GetAgewithDeath(DateTime dateOfBirth,DateTime dateOfDeath)
        {
            

            DateTime zeroTime = new DateTime(1, 1, 1);

            DateTime a = new DateTime(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
            DateTime b = new DateTime(dateOfDeath.Year, dateOfDeath.Month, dateOfDeath.Day);

            TimeSpan span = b - a;
            // because we start at year 1 for the Gregorian 
            // calendar, we must subtract a year here.
            int years = (zeroTime + span).Year - 1;
            return years;
        }

        public async Task<TblResponse> CopyFile(List<IFormFile> files, string PasteTopath)
        {
            try
            {
                TblResponse response = new TblResponse();
                response = null;
                long size = files.Sum(f => f.Length);

                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    string _fileName = System.IO.Path.GetFileName(formFile.FileName);
                    string _FileToPaste = Path.Combine(PasteTopath, _fileName);
                    int _FileNameIndex = 0;
                    // Get file path to be uploaded
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
                    //var path = Path.Combine(UploadingPath, ReferenceNo);
                    var _filePath = Path.GetFullPath(formFile.FileName);
                    //var filePath = "C:\\Wa\\sample\\" + fileName;

                    string _File = string.Empty;
                    //Check file if name is already saved
                    _File = _FileToPaste;

                    //Create index to remain unique file name
                    while (File.Exists(_File))
                    {
                        _File = _FileToPaste;
                        _FileNameIndex += 1;
                        string index = "(" + _FileNameIndex + ")";
                        _File = _FileToPaste.Insert(_FileToPaste.LastIndexOf('.'), index);
                    }

                    _FileToPaste = _File; 
                    // Create a new local file and copy contents of uploaded file
                    //using (var localFile = System.IO.File.OpenWrite(filePath))
                    //using (var uploadedFile = formFile.OpenReadStream())
                    //{
                    //    uploadedFile.CopyTo(localFile);
                    //} 
                    File.Copy(formFile.Name, _FileToPaste);
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void CopyTemplate(string fileName, string pathOutBox, string template)
        {
            string source = template;

            string destination = pathOutBox + fileName;

            File.Copy(source, destination);
        } 

    }
}
