using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hirportal.Models
{
    public class ArchiveViewModel
    {
        [Required(ErrorMessage = "A keresett kifejezés hiányzik")] // feltételek a validáláshoz
        public String SearchTerm { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
