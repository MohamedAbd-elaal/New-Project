using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC_FILE_VIEW_.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MVC_FILE_VIEW_.ViewModel;
using MVC_FILE_VIEW_.Security;
using Microsoft.AspNetCore.Authentication.Google;


namespace MVC_FILE_VIEW_
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            this._config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //controll password option by configureor addIdentity
            //services.Configure<IdentityOptions>(option =>
            //{
            //    option.Password.RequiredLength = 5;
            //    option.Password.RequiredUniqueChars = 2;

            //});
            // for run identity
            services.AddIdentity<ApplicationUser, IdentityRole>(
                option =>
                {
                    option.Password.RequiredLength = 7;
                    option.Password.RequiredUniqueChars = 1;
                    // for confirmeEmail to be required
                    option.SignIn.RequireConfirmedEmail = true;
                    //change Email Provider Token (X)
                    option.Tokens.ChangeEmailTokenProvider = "CustomEmailConfirmation";
                    //For LockOut Email
                    //time to can Access Wrong
                    option.Lockout.MaxFailedAccessAttempts = 5;
                    //Time if u fail to login 5 Times
                    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })

                    // to run our sql database
                    .AddEntityFrameworkStores<dbcontextclass>()
                    //this token for Email confirmation and reset pass (All Token We have) in Regiter Method
                    .AddDefaultTokenProviders()
                    //Add Custom Token Provider for Just Email not Other Tokens (X)
                    .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");
             
            services.AddMvc(options =>
            {
                var Policy = new AuthorizationPolicyBuilder()
                                 // any option want user and go to login page and to go login page we want user so it is infinit loop
                                 // and only action IEnumerable load beacause it has allowanonymous attribute
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(Policy));
            }).AddXmlSerializerFormatters();
            // look to Editrole folder(Autherize(role vs claim)-9 folder) to know more about
            //[Authorize(role="") there is anthor way it simillar claim [Authorize(policy="") 
            //for autherize claims see DeleteRole Method
            // (RequireClaim,RequireRole,RequireAssertion) all these method built in Requirement
            services.AddAuthorization(option =>
            { option.AddPolicy("DeleteRolePolicy",
                policy => policy.RequireClaim("Delete Role"));
                //option.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireClaim("Edit Role","true"));

                //if we want to add role and claim with us
                //option.AddPolicy("EditRolePolicy",
                //    policy=>policy.RequireAssertion(context=>context.User.IsInRole("user")&& context.User.HasClaim(claim=>claim.Type=="Edit Role"&&claim.Value=="true")||
                //    context.User.IsInRole("SuperAdmin"))                                
                //    );

                //if we want to add role and claim by function 
                option.AddPolicy("EditRolePolicy",
                    policy => policy.RequireAssertion(context => AuthorizeAccess(context)));

                /// for custom handler 
                option.AddPolicy("EditRolePolicy1",
                   (policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement())));
                // the option of not go to anthor handler if there is an error in a Existing Handler and get an error page
              //  option.InvokeHandlersAfterFailure = false;
            });
            //to run custom Handler 
            services.AddScoped<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddScoped<IAuthorizationHandler, SuperAdminHandler>();
            //run db
            services.AddDbContextPool<dbcontextclass>(
                option => option.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false).AddXmlSerializerFormatters();
            services.AddScoped<IEmployee, ISqlEmployee>();//<type interface,implmentation>
            //in Microsoft.AspNetCore.Authentication.Google
            services.AddAuthentication().AddGoogle(
                //https://console.cloud.google.com/apis/credentials/oauthclient/629960033939-4ne01mbdoif4db1rvp6qudvm9kvo0qnd.apps.googleusercontent.com?project=employee-mgmt-sts-338701&supportedpurview=project
                option => {       option.ClientId = "629960033939-4ne01mbdoif4db1rvp6qudvm9kvo0qnd.apps.googleusercontent.com";
                                 option.ClientSecret = "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";
                          }
                )
                .AddFacebook(option =>
                {
                    option.AppId = "269578741911118";
                    option.AppSecret = "c1bfba03c1967271204e9ddd475d5380";
                });

            // Set token life span to 5 hours(valid for 5h)
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                               o.TokenLifespan = TimeSpan.FromHours(5));
            // Changes token lifespan of just the Email Confirmation Token type (X)
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(options=>options.TokenLifespan=TimeSpan.FromDays(3));
            //Register the class that contains purpose strings with the asp.net core dependency injection container.
            //This allows us to inject an instance of this class into any controller throughout our application. 
            services.AddSingleton<DataProtectionPurposeStrings>();
        }
        // this is the function
        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("user") && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                context.User.IsInRole("SuperAdmin");
       }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //solve error with url in html pages
            else
            {
                app.UseExceptionHandler("/Error");//for handler the (throw new Exception in Details2)
                // app.UseStatusCodePages();
               // app.UseStatusCodePagesWithRedirects("/Error/{0}");//("controller/(statuscode(int like 404 not found)))
               // this is the best and if u want see diffrence see network in in inspect of pages
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            // for identity
            app.UseAuthentication();
            // app.UseMvcWithDefaultRoute();
            //it is how to search in html (?)=>mean optional not nessary
            //Conventional Routing
            app.UseMvc(Routes => {Routes.MapRoute("default", "{controller=home}/{action=IEnumerable}/{id?}"); });
           //Attribute Routing
            //app.UseMvc();
            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
