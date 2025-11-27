//using Domain.Models;
using Microsoft.AspNetCore.Mvc;
//using Repository.IRepository;
using SPASv2.Models;

namespace SPASv2.Controllers
{
    public class SubjectController : Controller
    {

        //private readonly IRepositoryUnit _IRepositoryUnit;
        private ILogger<SubjectController> _logger;
        public SubjectController(ILogger<SubjectController> logger)
        {
            _logger = logger;
            //_IRepositoryUnit = IRepositoryUnit;
        }

        public IActionResult Index()
        {
            int s = 2;
            //TblEmployee sampleEmployee = new TblEmployee()
            //{
            //    EmpID = "4",
            //    FirstName = "davee1ee",
            //    LastName = "daveee1ee",
            //    Age = 10
            //};

            //Subject sampleEmployee = new Subject()
            //{
            //    SubjectId = 1,
            //    SubjectName = "davee1ee",
            //    SubjectTerm = "123123",
            //    SubjectCredets = 10
            //};

            //if (SubjectRules.CanDelete("2") == true)
            //{
            //_IRepositoryUnit.ISubjectRepository.Delete(s);
            //_IRepositoryUnit.ISubjectRepository.IUpdate(sampleEmployee);
            //}
            return View();

            // return View(_IRepositoryUnit.ISubjectRepository.GetAllObjects());
          
        }

        public IActionResult Delete(int id)
        {

            //TblEmployee sampleEmployee = new TblEmployee()
            //{
            //    EmpID = "4",
            //    FirstName = "davee1ee",
            //    LastName = "daveee1ee",
            //    Age = 10
            //};

            //if (SubjectRules.CanDelete("2") == true)
            //{
            //_IRepositoryUnit.ISubjectRepository.Delete(id);
            //}
            return RedirectToAction("Index");
        }
    }
}
