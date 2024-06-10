using RegistrationApp.Database.Extensions;
using RegistrationApp.BusinessLogic.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation.AspNetCore;
using RegistrationApp.Shared.Validators;
using RegistrationApp.Database.Validators;
using FluentValidation;

namespace RegistrationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a WebApplicationBuilder to configure app's services and middleware
            var builder = WebApplication.CreateBuilder(args);

            // Register database services using the connection string from configuration
            builder.Services.AddDatabaseServices(builder.Configuration.GetConnectionString("Database"));

            // Register business logic services
            builder.Services.AddBusinessLogicServices();

            // Configure JWT authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Validate the issuer of the token
                    ValidateAudience = true, // Validate the audience of the token
                    ValidateLifetime = true, // Validate the token's expiration
                    ValidateIssuerSigningKey = true, // Validate the signing key
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer validation parameter
                    ValidAudience = builder.Configuration["Jwt:Audience"], // Audience validation parameter
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Signing key
                    ClockSkew = TimeSpan.FromSeconds(0) // Clock skew tolerance
                };
            });

            // Configure Swagger for API documentation
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT APP", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
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

            // Add services to the container.

            builder.Services.AddControllers();

            // Register FluentValidation for automatic validation
            builder.Services.AddFluentValidationAutoValidation()
                            .AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<PersonDtoValidator>()
                            .AddValidatorsFromAssemblyContaining<PlaceOfResidenceDtoValidator>()
                            .AddValidatorsFromAssemblyContaining<UserDtoValidator>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // Register endpoints API explorer for Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();// Enable Swagger UI
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

            app.UseAuthorization(); // Enable authorization middleware

            app.MapControllers(); // Map controllers to routes

            app.Run(); // Run the application

            //Create the first user, which is Admin automatically
        }
    }
}
