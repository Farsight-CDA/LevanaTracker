using Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPITemplate.Api.Configuration;
using MinimalAPITemplate.Api.Persistence;
using MinimalAPITemplate.Api.Services;
using MinimalAPITemplate.Api.Services.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace MinimalAPITemplate.Api;
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
        _ = services.AddApplication(Configuration, options =>
        {
            options.UseServiceLevels = true;
            options.ValidateServiceLevelsOnInitialize = true;
            options.IgnoreIServiceWithoutLifetime = false;
        },
        Program.Assembly);

        _ = services.AddDbContextPool<AppDbContext>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<DatabaseOptions>();
            _ = options.UseNpgsql(dbOptions.ConnectionString);
        }, 8);

        _ = services.AddValidatorsFromAssemblyContaining<Startup>(includeInternalTypes: true, filter: x => true);

        _ = services.AddResponseCaching();
        _ = services.AddCors();

        _ = services.AddSingleton(provider =>
        {
            var securityOptions = provider.GetRequiredService<SecurityOptions>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityOptions.JWTKey));

            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = securityOptions.JWTIssuer,
                ValidAudience = securityOptions.JWTIssuer,
                IssuerSigningKey = securityKey,
            };
        });

        _ = services.AddSingleton<JwtSecurityTokenHandler>();
        _ = services.AddOptions<JwtBearerOptions>()
            .Configure<TokenValidationParameters>((options, p) => options.TokenValidationParameters = p);

        _ = services.ConfigureOptions<JwtBearerOptionsConfiguration>();

        _ = services.AddSingleton(RandomNumberGenerator.Create());

        _ = services.AddAuthentication()
            .AddJwtBearer(options =>
            {
            });
        _ = services.AddAuthorization();
    }

    public void ConfigurePipeline(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if(env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }

        _ = app.UseCors(policy =>
        {
            _ = policy.AllowAnyHeader();
            _ = policy.AllowAnyMethod();
            _ = policy.AllowAnyOrigin();
        });

        _ = app.UseStaticFiles();

        var options = new RewriteOptions()
            .AddRewrite("^(?!api\\/|Api\\/)(.+)\\/$", "$1.html", true)
            .AddRewrite("^(?!api\\/|Api\\/)(.+)", "$1.html", true);

        _ = app.UseRewriter(options);

        _ = app.UseStaticFiles();

        _ = app.UseAuthentication();

        _ = app.UseRouting();

        _ = app.UseAuthorization();

        _ = app.UseResponseCaching();
    }
}