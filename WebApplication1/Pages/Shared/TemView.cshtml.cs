using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HanaToolUtilities.Models;
using WebApplication1.Data;

namespace HanaToolUtilities.Pages.Shared
{
    public class TemViewModel : PageModel
    {
        private readonly WebApplication1.Data.WebApplication1Context _context;

        public TemViewModel(WebApplication1.Data.WebApplication1Context context)
        {
            _context = context;
        }

        public TemModel TemModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemModel = await _context.TemModel.FirstOrDefaultAsync(m => m.iD == id);

            if (TemModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
