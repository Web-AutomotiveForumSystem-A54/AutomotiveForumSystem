using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Repositories;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;


namespace AutomotiveForumSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				});

			var configuration = builder.Configuration;

            // Configure JWT authentication
            var key = Encoding.ASCII.GetBytes("my-super-secret-key");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

			// Http Session
			builder.Services.AddSession(options =>
			{
				// With IdleTimeout you can change the number of seconds after which the session expires.
				// The seconds reset every time you access the session.
				// This only applies to the session, not the cookie.
				options.IdleTimeout = TimeSpan.FromHours(1);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireClaim("admin", "True");
                });
            });
            // Configure JWT authentication
            // test commit
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutomotiveForumSystemAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
			builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            builder.Services.AddScoped<ITagsService, TagsService>();
			builder.Services.AddScoped<ITagsRepository, TagsRepository>();

            builder.Services.AddScoped<ICommentsService, CommentsService>();
            builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();

            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();

            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddScoped<ICategoryModelMapper, CategoryModelMapper>();
            builder.Services.AddScoped<ICommentModelMapper, CommentModelMapper>();
            builder.Services.AddScoped<IPostModelMapper, PostModelMapper>();
            var secretKey = configuration["JwtSettings:SecretKey"];

			builder.Services.AddScoped<IAuthManager>(provider =>
                new AuthManager(
                    provider.GetRequiredService<IUsersService>(),
                    secretKey
                ));

			builder.Services.AddScoped<ILikesService, LikesService>();
			builder.Services.AddScoped<ILikesRepository, LikesRepository>();


			builder.Services.AddTransient<JwtService>(provider => new JwtService(secretKey));

            builder.Services.AddScoped<IUserMapper, UserMapper>();

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                // A connection string for establishing a connection to the locally installed SQL Server.
                // Set serverName to your local instance; databaseName is the name of the database
                string connectionString = @"Server=localhost\SQLEXPRESS;Database=AutomotiveForum;Trusted_Connection=True;";

                // Configure the application to use the locally installed SQL Server.
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });

            var app = builder.Build();

            app.UseRouting();

			// Enables session
			app.UseSession();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutomotiveForumSystem");
            });

			app.UseAuthentication();
            app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});

			app.UseStaticFiles();

			app.Run();
        }
    }
}