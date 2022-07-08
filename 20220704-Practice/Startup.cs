using _20220704_Practice.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
using Microsoft.AspNetCore.SignalR;
using _20220704_Practice.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace _20220704_Practice
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
            //services.AddDbContext<UserDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("UserDatabase")));


            string DBConnectionString = Configuration.GetConnectionString("DBContext");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

            services.AddDbContext<UserDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(DBConnectionString, serverVersion)
            // The following three options help with debugging, but should
            // be changed or removed for production.
            //.LogTo(Console.WriteLine, LogLevel.Information)
            //.EnableSensitiveDataLogging()
            //.EnableDetailedErrors()
            );

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "_20220704_Practice", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<JwtHelpers>();

            services
            .AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );

                options.AddPolicy("signalr",
                    builder => builder
                    .SetIsOriginAllowed(hostName => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .       AddJwtBearer(options =>
            {
             // ??�Cʧ���r���ؑ����^������ WWW-Authenticate ���^���@�e���@ʾʧ����??�e�`ԭ��
                options.IncludeErrorDetails = true; // �A�Oֵ�� true���Еr���؄e�P�]

                options.TokenValidationParameters = new TokenValidationParameters
                {
                     // ͸�^�@?���棬�Ϳ��ԏ� "sub" ȡֵ�K�O���o User.Identity.Name
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    // ͸�^�@?���棬�Ϳ��ԏ� "roles" ȡֵ���K��? [Authorize] �Д��ɫ
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                    // һ����?����?�C Issuer
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),

                    // ͨ����̫��Ҫ?�C Audience
                    ValidateAudience = false,
                    //ValidAudience = "JwtAuthDemo", // ��?�C�Ͳ���Ҫ��?

                    // һ����?����?�C Token ����Ч���g
                    ValidateLifetime = true,

                    // ��� Token �а��� key ����Ҫ?�C��һ�㶼ֻ��?�¶���
                    ValidateIssuerSigningKey = false,

                     // "1234567890123456" ��?�� IConfiguration ȡ��
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:SignKey")))
                };
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "_20220704_Practice v1"));
            }


            //app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");
            app.UseCors("signalr");

            app.UseRouting();
            app.UseAuthentication();//��?Ҫ��Authorization֮ǰ����?�C�����ڙ�
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
