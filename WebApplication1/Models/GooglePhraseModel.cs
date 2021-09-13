using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class GooglePhraseModel
    {

        public int iD { get ;  set;}
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This is required")]
        public string txtFromText { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "This is required")]
        public string txtToText { get; set; }         
    }
}
