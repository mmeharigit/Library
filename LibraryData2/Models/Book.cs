using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryData2.Models
{
   public class Book:LibraryAsset
    {
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string DewyIndexNumber { get; set; }
    }
}
