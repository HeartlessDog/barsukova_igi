using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using libr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace lab6_igi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            String con = "Server = DESKTOP-H9ELFFU; Database = LibrLab6; Trusted_Connection = True; MultipleActiveResultSets = true";
            services.AddDbContext<LibraryContext>(options => options.UseSqlServer(con));
            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvc();
            app.UseDefaultFiles();

        }
    }
}
