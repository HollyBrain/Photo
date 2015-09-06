using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            using (PhotoDbContext data = new PhotoDbContext())
            {
                List<Users> user = data.Users.ToList();
                ViewBag.Name = user.ToList();
                List<Albums> album = data.Albums.ToList();
                ViewBag.Album = album.ToList();
           }
            return View();
        }

    }
}
