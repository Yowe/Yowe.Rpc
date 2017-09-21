using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vice.Repository;

namespace Vice.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public ActionResult Index()
        {
           var result=  unitOfWork.ArticleRepository.Get();
            return View();
        }

    }
}
