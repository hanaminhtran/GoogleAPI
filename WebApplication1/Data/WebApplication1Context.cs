using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using HanaToolUtilities.Models;

namespace WebApplication1.Data
{
    public class WebApplication1Context : DbContext
    {
        public WebApplication1Context (DbContextOptions<WebApplication1Context> options)
            : base(options)
        {
        }

        public DbSet<WebApplication1.Models.GooglePhraseModel> GooglePhraseModel { get; set; }

        public DbSet<HanaToolUtilities.Models.TemModel> TemModel { get; set; }
    }
}
