using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Helpers;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity
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
            services.AddDbContext<DataBaseContext>(p => p.UseSqlServer("Data Source=.; Initial Catalog=DbIdentity; Integrated Security=True; TrustServerCertificate=Yes")); 
            services.AddControllersWithViews();

            // پیکربندی سیستم احراز هویت (Identity) در برنامه
            services.AddIdentity<User, Role>()
               // تعیین ذخیره‌سازی داده‌ها با استفاده از Entity Framework و کلاس DataBaseContext
               .AddEntityFrameworkStores<DataBaseContext>()
               // افزودن ارائه‌دهنده‌های توکن پیش‌فرض برای عملیاتی مانند بازیابی رمز عبور و تأیید ایمیل
               .AddDefaultTokenProviders()
               // فعال‌سازی سیستم نقش‌ها (Roles) با استفاده از کلاس Role تعریف شده در برنامه
               .AddRoles<Role>()
               // استفاده از کلاس سفارشی CustomIdentityError برای نمایش پیام‌های خطا به زبان فارسی
               .AddErrorDescriber<CustomIdentityError>()
               // افزودن اعتبارسنج سفارشی MyPasswordValidator برای بررسی قوی بودن رمزهای عبور
               .AddPasswordValidator<MyPasswordValidator>();

            // پیکربندی تنظیمات هویت (Identity) در برنامه
            //services.Configure<IdentityOptions>(option =>
            //{

            //    // تنظیمات مربوط به کاربر
            //    // تعیین کاراکترهای مجاز برای نام کاربری (فقط حروف a تا d و اعداد 1 تا 3)
            //    option.User.AllowedUserNameCharacters = "abcd123";
            //    // الزام یکتا بودن ایمیل برای کاربران
            //    option.User.RequireUniqueEmail = true;

            //    // تنظیمات مربوط به رمز عبور
            //    // عدم نیاز به وجود عدد در رمز عبور
            //    option.Password.RequireDigit = false;
            //    // عدم نیاز به وجود حروف کوچک در رمز عبور
            //    option.Password.RequireLowercase = false;
            //    // عدم نیاز به وجود کاراکترهای خاص مثل !@#$%^&*()_+ در رمز عبور
            //    option.Password.RequireNonAlphanumeric = false;
            //    // عدم نیاز به وجود حروف بزرگ در رمز عبور
            //    option.Password.RequireUppercase = false;
            //    // حداقل طول رمز عبور: 8 کاراکتر
            //    option.Password.RequiredLength = 8;
            //    // حداقل تعداد کاراکترهای یکتا در رمز عبور: 1 کاراکتر
            //    // یعنی کاربر در رمز عبورش فقط میتواند یک کاراکتر یکتا داشته باشد
            //    option.Password.RequiredUniqueChars = 1;

            //    // تنظیمات مربوط به قفل شدن حساب کاربری
            //    // قفل شدن حساب کاربری پس از 3 بار تلاش ناموفق برای ورود
            //    option.Lockout.MaxFailedAccessAttempts = 3;
            //    // مدت زمان قفل شدن حساب کاربری: 10 میلی‌ثانیه
            //    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(10);

            //    // تنظیمات مربوط به ورود به سیستم
            //    // عدم نیاز به تأیید حساب کاربری برای ورود
            //    option.SignIn.RequireConfirmedAccount = false;
            //    // عدم نیاز به تأیید ایمیل برای ورود
            //    option.SignIn.RequireConfirmedEmail = false;
            //    // عدم نیاز به تأیید شماره تلفن برای ورود
            //    option.SignIn.RequireConfirmedPhoneNumber = false;
            //});

            // پیکربندی تنظیمات کوکی برنامه
            services.ConfigureApplicationCookie(option =>
            {
                // تنظیمات مربوط به کوکی
                // مدت زمان انقضای کوکی: 10 دقیقه
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                // معرفی مسیر صفحه ورود به سیستم
                // در صورتی که کاربر ثبت نام نکرده باشد و به یک اکشن که برای ثبت نام شده ها طراحی شده بخواهد وارد شود به این صفحه ارسال میشود
                option.LoginPath = "/account/login";

                // مسیر صفحه دسترسی غیرمجاز
                // در صورتی که کاربر به یک صفحه دسترسی نداشته باشد به این صفحه ریدایرکت میشود 
                option.AccessDeniedPath = "/Account/AccessDenied";

                // تمدید خودکار مدت زمان کوکی در صورت فعالیت کاربر
                option.SlidingExpiration = true;
            });

            // ثبت سرویس سفارشی AddMyClaims به عنوان پیاده‌سازی IUserClaimsPrincipalFactory<User> در سیستم تزریق وابستگی
            // با طول عمر Scoped (به ازای هر درخواست یک نمونه جدید)
            //services.AddScoped<IUserClaimsPrincipalFactory<User>, AddMyClaims>();

            // ثبت سرویس IClaimsTransformation با پیاده‌سازی AddClaim در محدوده Scoped
            // این سرویس برای اضافه کردن کلیم‌های سفارشی به هویت کاربر در هر درخواست استفاده می‌شود
            services.AddScoped<IClaimsTransformation, AddClaim>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
