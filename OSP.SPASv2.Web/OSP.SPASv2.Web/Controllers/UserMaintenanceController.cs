using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SPASv2.Controllers
{
    public class UserMaintenanceController : Controller
    {
        // GET: UserMaintenanceController
        public ActionResult Index()
        {
            return View();


        }

        // GET: UserMaintenanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserMaintenanceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMaintenanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: UserMaintenanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserMaintenanceController/Edit/5
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

        // GET: UserMaintenanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserMaintenanceController/Delete/5
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
    }
}
