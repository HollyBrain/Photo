using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Providers.Entities;
using System.Web.Mvc;

namespace Photo.PhotoManipulation
{
    public class FileWork
    {
        private string name;
        public string GetName(string Name)
        {
            this.name = Name;
            return this.name;
        }
        public FileWork() 
        {
        }
        PhotoDbContext db = new PhotoDbContext();
        public string FileExtension(string filename)
        {
            int sn = 0; //Symbol Number
            string Extension = null;
            string[] fn = new string[] { filename };
            foreach (string s in fn)
            {
                sn = s.IndexOf(".");
                Extension = s.Substring(sn + 1);
            }
            return Extension;
        }

        public void CreateDirectory(string name)
        {
            Directory.CreateDirectory(name);
        }
        public void SaveFace(IEnumerable<HttpPostedFileBase> fileUpload, string path, string photoName)
        {
            foreach (var file in fileUpload)
            {
                if (file == null) continue;
                string filename = System.IO.Path.GetFileName(file.FileName);
                if (filename != null) file.SaveAs(Path.Combine(path, photoName));
            }
        }
       public void SaveOnServer(IEnumerable<HttpPostedFileBase> fileUpload, string userName)
        {
            IEnumerable<Albums> albums = db.Albums;
            int id = 0;
            int i = 0;
            foreach (var file in fileUpload)
            {
                if (file == null) continue;
                string filename = System.IO.Path.GetFileName(file.FileName);
                string Extension = FileExtension(filename);
                string NewFileName = this.name+ "Photo" + i + "." + Extension;
                ManipulationDb mbd = new ManipulationDb();
                
                foreach (var b in albums)
                {
                    if (b.AlbumName == this.name && b.User == userName)
                    {
                        id = b.AlbumId;
                    }
                }
                string saveFile = "\\UserImage\\" + userName + "\\" + this.name+"\\"+NewFileName;
                mbd.UpdateBd(saveFile, id);
                if (filename != null) file.SaveAs(Path.Combine("D:\\Photo\\Photo\\UserImage"+"\\"+userName+"\\"+this.name, NewFileName));
                i++;
            }
        }
    }
}