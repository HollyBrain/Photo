using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Photo.Models;

namespace Photo.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Album/
        private PhotoDbContext data = new PhotoDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OpenAlbum(int id, string album)
        {
            List<Photos> photo = data.Photos.ToList();
            var findPhoto=photo.Where(a => a.AlbumId == id);
            ViewBag.Photos = findPhoto;
            List<Albums> albums=data.Albums.ToList();
            List<Users> user = data.Users.ToList();
            var findCreated = albums.Where(m => m.AlbumId == id);
            var filterUser = "";
            foreach (var b in findCreated)
            {
                filterUser = b.User;
            }
            var findUser = user.Where(m => m.Email == filterUser);
            ViewBag.UserName = findUser;
            return View();
        }
        [HttpGet]
        public ActionResult DeletePhoto(int id, int album, string path)
        {
            List <Albums> al= data.Albums.ToList();
            foreach (var b in al)
            {
                if (b.AlbumId == album)
                {
                    if (b.User == User.Identity.Name)
                    {
                        Photos photo = data.Photos.Find(id);
                        System.IO.File.Delete(Server.MapPath(path));
                        data.Photos.Remove(photo);
                        data.SaveChanges();
                    }
                    else return View();
                }
            }
            return RedirectToAction("OpenAlbum", "Album", new {id=album, album=User.Identity.Name});
        }
    }
}
