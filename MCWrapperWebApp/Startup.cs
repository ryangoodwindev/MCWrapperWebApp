using MCWrapper.CLI.Extensions;
using MCWrapper.RPC.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MCWrapperWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // When using the AddMultiChainCoreServices parameterless constructor
            // BlockchainProfileOptions and RuntimeParamOptions are loaded from 
            // environment variables. Please refer to our help page for this
            // scenario, https://mcwrapper.com/configure-options
            // 
            // IConfiguration or explicit configuration constructors are available as well.
            services.AddMultiChainCoreRpcServices(rpcOptions =>
            {
                rpcOptions.ChainAdminAddress = "1F5WvCAwc9sX6ZMJKE8GRZA6UQQPnxgB8FbU7t";
                rpcOptions.ChainBurnAddress = "1XXXXXXXDXXXXXXX7bXXXXXXVsXXXXXXXCT2tS";
                rpcOptions.ChainPassword = "EmjbtUBfbg2SikAXVnAJSQKmbeZhxbpSopsVKk58zrE";
                rpcOptions.ChainUsername = "multichainrpc";
                // localhost or a remote node's address eg. NewChain@192.168.1.1:7764
                rpcOptions.ChainHostname = "localhost";
                rpcOptions.ChainName = "NewChain";
                rpcOptions.ChainRpcPort = 7764;
                // default is HTTP if this is left empty
                rpcOptions.ChainSslPath = string.Empty;
                rpcOptions.ChainUseSsl = false;
            });

            services.AddMultiChainCoreCliServices(cliOptions =>
            {
                cliOptions.ChainAdminAddress = "1F5WvCAwc9sX6ZMJKE8GRZA6UQQPnxgB8FbU7t";
                cliOptions.ChainBurnAddress = "1XXXXXXXDXXXXXXX7bXXXXXXVsXXXXXXXCT2tS";
                cliOptions.ChainName = "NewChain";
                // if empty default location is used
                cliOptions.ChainDefaultColdNodeLocation = string.Empty;
                cliOptions.ChainDefaultLocation = string.Empty;
                cliOptions.ChainBinaryLocation = string.Empty;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
