using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class Gender
    {
        public Gender()
        {
            Artists = new HashSet<Artist>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }
    }
}
