using CustomerAPI.Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<DbApiContext>(opt => opt.UseInMemoryDatabase("MemoryDb"));

        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

           
            //SeedDatabase(app);
        }


        public static void SeedDatabase(IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DbApiContext>();

                // Seed the database.

                var testUser1 = new CustomerAPI.Entities.User
                {
                    //Id = 1,
                    FirstName = "zeyad",
                    SureName = "abdelwahab"
                };

                context.Users.Add(testUser1);

                var testAccount1 = new CustomerAPI.Entities.Account
                {
                   // Id = 1,
                    UserId = testUser1.UserId,
                    FirstName = "zeyad",
                    SureName = "abdelwahab",
                    Balance = 1
                };

                context.Accounts.Add(testAccount1);

                context.SaveChanges();

                var transaction1 = new CustomerAPI.Entities.Transaction
                {
                    //Id = 1,
                    TransactionType = Entities.TransactionType.CheckBalance,
                    Account = testAccount1
                };
                var transaction2 = new CustomerAPI.Entities.Transaction
                {
                   // Id = 2,
                    TransactionType = Entities.TransactionType.Deposit,
                    Account = testAccount1
                };
                context.Transactions.Add(transaction1);
                context.Transactions.Add(transaction2);
                context.SaveChanges();

                var users = context.Accounts
                    .Include(u => u.Transactions)
                    .Include(u => u.User)
                    .ToList();
            }

        }
    }
}
