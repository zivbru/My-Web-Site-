using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class Genre
    {
        public Genre()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
