using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferAggr.RequestApi.Data;
using TransferAggr.RequestApi.Dtos;
using TransferAggr.RequestApi.Models;

namespace TransferAggr.RequestApi
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
            services.AddDbContext<RequestContext>(opt =>
                opt.UseInMemoryDatabase("InMem"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TransferAggr.RequestApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme="oauth2",
                            Name="Bearer",
                            In=ParameterLocation.Header
                        },
                        new List<string>()
                    }

                });
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Request, RequestCreateDto>();
                cfg.CreateMap<RequestCreateDto, Request>();
                cfg.CreateMap<Request, RequestReadDto>();
                cfg.CreateMap<RequestReadDto, Request>();
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddCors(option =>
            {
                option.AddPolicy("FromWebMvc", builder =>
                {
                    builder.WithOrigins($"{Configuration.GetValue<string>("WebUrl")}");
                });
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer(opt =>
                {
                    opt.Authority = $"{Configuration.GetValue<string>("IdentityUrl")}";
                    opt.TokenValidationParameters.ValidateAudience = false;
                });

            services.AddAuthorization(opt =>
                opt.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RequestContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransferAggr.RequestApi v1"));
            }

            app.UseCors("FromWebMvc");

            context.Database.EnsureCreated();

            app.UseRouting();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
