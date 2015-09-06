using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photo.PhotoManipulation
{
    public class ManipulationDb
    {
        public void UpdateBd(string photoName, int id)
        {
            Models.PhotoModel user = new Models.PhotoModel();
            using (var db = new PhotoDbContext())
            {
                var photo = db.Photos.Create();
                photo.PhotoPath = photoName;
                photo.AlbumId = id;
                db.Photos.Add(photo);
                db.SaveChanges();
            }
        }
    }
}