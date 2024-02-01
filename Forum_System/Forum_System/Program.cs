using Forum_System.Data;
using Forum_System.Helpers;
using Forum_System.Repositories;
using Forum_System.Repositories.Contracts;
using Forum_System.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

//using Microsoft.OpenApi.Models; swagger thing
using Newtonsoft.Json;
using Forum_System.Services.Contracts;
using Microsoft.Extensions.Configuration;


namespace Forum_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // JWT setup start

            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidIssuer = jwtIssuer,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                };
            });


            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor(); // claim reader

            // JWT setup end

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // frontpage setup

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ForumSystem API", Version = "v1" });

                //authentication for swagger below, adds the "Authenticate" button

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token with Bearer format: bearer[space]token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[]{}
                    }
                });
            });

            //Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".Forum_System.Session";
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                // The connection string has been moved to appsettings.json
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            //Repos
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IThreadRepository, ThreadRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
			builder.Services.AddScoped<ITagRepository, TagRepository>();


			//Services
			builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IThreadService, ThreadService >();
            builder.Services.AddScoped<ICommentService, CommentService>();
			builder.Services.AddScoped<ITagService, TagService>();
			builder.Services.AddScoped<IAuthService, AuthService>();

            //Helpers
            builder.Services.AddScoped<ModelMapper>();

            //Other



            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseSession();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ForumSystem API V1");
                options.RoutePrefix = "api/swagger";
                // options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); 
            });

            app.Run();
        }
    }
}
