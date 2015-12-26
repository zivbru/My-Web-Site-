using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class Picture
    {
        public Picture()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public string FileName { get; set; }

        public PictureType PictureType { get; set; }
        public int PictureTypeId { get; set; }

        public virtual Artist Artist { get; set; }
        public int ArtistId { get; set; }

        [DataType(DataType.Currency)]
        public int Price { get; set; }


        public virtual Genre Genre { get; set; }
        public int GenreId { get; set; }

        [StringLength(6000, MinimumLength = 3)]
        public string Description { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
