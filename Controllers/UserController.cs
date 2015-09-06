using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Security;
using Photo.Models;
using System.Data.Entity;
using Photo.PhotoManipulation;
using System.Web.UI.WebControls;

namespace Photo.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        private PhotoDbContext data = new PhotoDbContext();
        private static FileWork fw = new FileWork();

        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LogIn(Models.LoginModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.Email, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login Data is incorrect.");
                }
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Models.UserModel user, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                using (var db = new PhotoDbContext())
                {
                    var crypto = new SimpleCrypto.PBKDF2();

                    var encrpPass = crypto.Compute(user.Password);

                    var sysUser = db.Users.Create();

                    sysUser.Email = user.Email;
                    sysUser.FirstName = user.FirstName;
                    sysUser.LastName = user.LastName;
                    sysUser.Password = encrpPass;
                    sysUser.PasswordSalt = crypto.Salt;
                    if (fileUpload == null)
                    {
                        sysUser.UserFace = "\\Content\\assets\\all.jpg";
                    }
                    else
                    {
                        FileWork file = new FileWork();
                        string Extension = file.FileExtension(fileUpload.FileName);
                        string path = "\\UserImage\\" + User.Identity.Name;
                        fileUpload.SaveAs(Path.Combine(Server.MapPath("/UserImage/" + User.Identity.Name + "/"), "Photo." + Extension));
                        sysUser.UserFace = path + "\\"+"Photo." + Extension;
                    }
                    db.Users.Add(sysUser);
                    db.SaveChanges();
                    FileWork fw = new FileWork();
                    fw.CreateDirectory("D://Photo//Photo//UserImage//"+user.Email);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect");
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult UserPage(string userId)
        {
            List<Users> user = data.Users.ToList();
            var findUser = user.Where(m => m.Email == userId);
            ViewBag.UserName = findUser;
            return View();
        }

        [HttpGet]
        [Authorize()]
        public ActionResult CreateAlbum()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateAlbum(Models.AlbomModel user, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                using (var db = new PhotoDbContext())
                {
                    var album = db.Albums.Create();
                    fw.CreateDirectory("D://Photo//Photo//UserImage//" + User.Identity.Name + "//" + user.AlbumName);
                    album.AlbumName = user.AlbumName;
                    if (fileUpload != null)
                    {
                        FileWork file= new FileWork();
                        string Extension = file.FileExtension(fileUpload.FileName);
                        string path="\\UserImage\\"+User.Identity.Name+"\\"+user.AlbumName;
                        fileUpload.SaveAs(Path.Combine(Server.MapPath("/UserImage/"+User.Identity.Name+"/"+user.AlbumName+"/"), user.AlbumName + "." + Extension));
                        album.AlbumFace = path + "\\" + user.AlbumName + "." + Extension;
                    }
                    else
                        album.AlbumFace = "";
                    album.User = User.Identity.Name;
                    db.Albums.Add(album);
                    db.SaveChanges();
                    fw.GetName(user.AlbumName);
                    return RedirectToAction("AddPhoto", "User");
                }
            }
            else
            {
                ModelState.AddModelError("", "Data is incorrect");
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult AddPhoto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPhoto(IEnumerable<HttpPostedFileBase> fileUpload)
        {
            fw.SaveOnServer(fileUpload, User.Identity.Name);
            return RedirectToAction("AlbumPage", "User" , new {userId=User.Identity.Name});
           
        }
        [HttpPost]
        public ActionResult UpdatePhoto(IEnumerable<HttpPostedFileBase> fileUpload, int albumId)
        {
            List<Albums> album = data.Albums.ToList();
            foreach (var b in album)
            {
                if (b.AlbumId == albumId) fw.GetName(b.AlbumName);
            }
            fw.SaveOnServer(fileUpload, User.Identity.Name);
            return RedirectToAction("AlbumPage", "User", new { userId = User.Identity.Name });

        }
        [HttpGet]
        public ActionResult AlbumPage(string userId)
        {
            List<Albums> album = data.Albums.ToList();
            if (album == null)
                return RedirectToAction("Index");
            else
            {
                var findAlbums = album.Where(m => m.User == userId);
                ViewBag.UsersAlbum = findAlbums.ToList();
                return View();
            }
        }
        private bool IsValid(string email, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool isValid = false;
            using (var db = new PhotoDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        isValid = true;
                    }
                }
            }


            return isValid;
        }
    }
}
