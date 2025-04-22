using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Shipping.Models;
using Shipping.UnitOfWork;
using Shipping.Repository.ArabicNamesForRoleClaims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Shipping.Repository.Employee_Repository;
using Shipping.AutoMapperProfiles;
using Shipping.Repository.DeliveryRepo;
using Microsoft.AspNetCore.Hosting;
using Shipping.Repository.MerchantRepository;
using Shipping.Repository.BranchRepository;
using Shipping.CustomAuth.RoleClaimService;
using Shipping.CustomAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Swagger Configuration
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Shipping System API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT Token with 'Bearer' prefix"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
        #endregion

        #region Authentication Configuration
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = Encoding.ASCII.GetBytes("Iti ii iii ii ii iiiiiiii iiiiiiii iiiiii iii ii");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                };
            });

        builder.Services.AddScoped<IRoleClaimService, RoleClaimService>();
        #endregion

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

        builder.Services.AddEndpointsApiExplorer();

        #region Identity Configuration
        builder.Services.AddIdentity<AppUser, UserRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<ShippingContext>();
        #endregion

        #region CORS Configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        #endregion

        #region Dependency Injection
        builder.Services.AddDbContext<ShippingContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Db"));
        });
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddScoped<IUnitOfWork<Order>, UnitOfWork<Order>>();
        builder.Services.AddScoped<IUnitOfWork<WeightSetting>, UnitOfWork<WeightSetting>>();
        builder.Services.AddScoped<IUnitOfWork<SpecialCitiesPrice>, UnitOfWork<SpecialCitiesPrice>>();
        builder.Services.AddScoped<IUnitOfWork<City>, UnitOfWork<City>>();
        builder.Services.AddScoped<IUnitOfWork<Government>, UnitOfWork<Government>>();
        builder.Services.AddScoped<IUnitOfWork<Delivery>, UnitOfWork<Delivery>>();
        builder.Services.AddScoped<IUnitOfWork<Employee>, UnitOfWork<Employee>>();
        builder.Services.AddScoped<IUnitOfWork<WeightSetting>, UnitOfWork<WeightSetting>>();
        builder.Services.AddScoped<IAddArabicNamesForRoleClaims, AddArabicNamesForRoleClaims>();
        builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();
        builder.Services.AddScoped<IUnitOfWork<Merchant>, UnitOfWork<Merchant>>();
        builder.Services.AddScoped<IBranchRepository, BranchRepository>();
        builder.Services.AddScoped<IUnitOfWork<Branch>, UnitOfWork<Branch>>();
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        #endregion

        var app = builder.Build();

        #region Middleware Pipeline
        app.UseCors("AllowAll");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        #endregion

        app.Run();
    }
}
