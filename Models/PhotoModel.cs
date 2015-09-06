using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Photo.Models
{
    public class AlbomModel
    {
        [Required]
        [StringLength(150)]
        public string AlbumName { get; set; }

        [StringLength(150)]
        public string AlbumFace { get; set; }
    }

    public class AlbumIdModel
    {
        public string AlbumId { get; set; }
    }
    public class PhotoModel
    {
        public string PhotoPath { get; set; }
        public int AlbumId { get; set; }
    }
}