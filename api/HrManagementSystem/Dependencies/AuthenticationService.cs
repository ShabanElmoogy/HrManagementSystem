using HrManagementSystem.Entities.BasicEntities;
using HrManagementSystem.Services.BasicServices.AuthService;
using Microsoft.Extensions.Options;

namespace HrManagementSystem.Dependencies
{
    public static class AuthenticationService
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services
            RegisterServices(services);

            // Configure Identity
            ConfigureIdentity(services);

            // Configure JWT
            var jwtSettings = ConfigureJwtOptions(services, configuration);

            // Configure Authentication
            ConfigureAuthentication(services, configuration, jwtSettings);

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        private static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        private static JwtOptions ConfigureJwtOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(nameof(JwtOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var jwtSettings = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
            {
                throw new InvalidOperationException("JWT configuration is missing or invalid");
            }

            return jwtSettings;
        }

        private static void ConfigureAuthentication(
            IServiceCollection services,
            IConfiguration configuration,
            JwtOptions jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };

                // Configure JWT for SignalR
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        // If the request is for SignalR hubs
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs") || path.StartsWithSegments("/hub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            .AddGoogle(options =>
            {
                var googleConfig = configuration.GetSection("ExternalLogin:Google");

                options.ClientId = googleConfig["ClientId"]
                    ?? throw new InvalidOperationException("Google ClientId is not configured");

                options.ClientSecret = googleConfig["ClientSecret"]
                    ?? throw new InvalidOperationException("Google ClientSecret is not configured");

                options.CallbackPath = "/signin-google";
            });
        }
    }
}