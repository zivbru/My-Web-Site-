using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class Artist
    {
        public Artist()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string AboutMe { get; set; }

        public Gender Gender { get; set; }
        public int GenderID { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string Address { get; set; }


       [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string City { get; set; }

        public string UserProfilePic { get; set; }

        [EmailAddress]
        public string Mail { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
    }
}