using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HanaToolUtilities.Models
{
    public class GoogleWord
    {
        public int iD { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This is required")]
        public string FromLang { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This is required")]
        public string ToLang { get; set; }

        public GoogleWord(string f, string t)
        {
            FromLang = f;
            ToLang = t;
        }
    }
}
