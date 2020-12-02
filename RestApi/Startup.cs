using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace GIC.RestApi
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) 
        { 
            Configuration = configuration; 
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services
            //.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
            //.AddBasicAuthentication(
            //    options =>
            //    {
            //    options.Realm = "GamingInterfaceClient";
            //        options.Events = new BasicAuthenticationEvents
            //        {
            //            OnValidatePrincipal = context => AuthenticationHandler(context)
            //        };
            //    });
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private Task AuthenticationHandler(ValidatePrincipalContext context)
        {
            if (context.Password == Program.key) {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,
                                context.UserName,
                                context.Options.ClaimsIssuer)
                };

                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(new ClaimsIdentity(
                        claims,
                        BasicAuthenticationDefaults.AuthenticationScheme)),
                    new AuthenticationProperties(),
                    BasicAuthenticationDefaults.AuthenticationScheme);

                
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
        }
    }
}
