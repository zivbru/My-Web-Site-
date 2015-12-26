using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public virtual Picture Picture { get; set; }
        public int PictureId { get; set; }

        
        public string Writer { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string Pic { get; set; }

    }
}
