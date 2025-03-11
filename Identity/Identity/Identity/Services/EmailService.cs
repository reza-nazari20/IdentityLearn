using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    // سرویس ارسال ایمیل که برای ارسال پیام‌های الکترونیکی به کاربران استفاده می‌شود
    public class EmailService
    {
        // متد اصلی برای ارسال ایمیل که آدرس کاربر، متن پیام و موضوع را دریافت می‌کند
        public Task Execute(string UserEmail, string Body, string Subject)
        {
            // ایجاد یک نمونه از کلاس SmtpClient برای برقراری ارتباط با سرور SMTP
            SmtpClient client = new SmtpClient();

            // تنظیم پورت ارتباطی با سرور SMTP (پورت استاندارد 587)
            // پورت 587 یک درگاه ارتباطی استاندارد برای ارسال ایمیل است که امکان رمزگذاری TLS را فراهم می‌کند
            // این پورت برای "ارسال ایمیل" (SMTP Submission) استفاده می‌شود و اکثر سرویس‌دهندگان ایمیل آن را پشتیبانی می‌کنند
            client.Port = 587;

            // تنظیم آدرس سرور SMTP جیمیل
            // این آدرس مشخص می‌کند که برنامه باید به کدام سرور ایمیل متصل شود
            // "smtp.gmail.com" آدرس سرور ارسال ایمیل گوگل است که برای ارسال ایمیل از طریق حساب‌های Gmail استفاده می‌شود
            // هر سرویس ایمیل آدرس SMTP مخصوص به خود را دارد (مثلاً برای Outlook از "smtp.office365.com" استفاده می‌شود)
            client.Host = "smtp.gmail.com";

            // فعال‌سازی SSL برای ارتباط امن با سرور
            client.EnableSsl = true;

            // تنظیم زمان انتظار برای پاسخ سرور (به میلی‌ثانیه)
            client.Timeout = 1000000;

            // تنظیم روش تحویل پیام به صورت شبکه
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            // عدم استفاده از اعتبارنامه‌های پیش‌فرض
            client.UseDefaultCredentials = false;

            //لینک گرفتن رمز عبور جدید برای سرویس ارسال ایمیل
            //https://myaccount.google.com/apppasswords
            // تنظیم اعتبارنامه‌های لازم برای احراز هویت با سرور SMTP
            client.Credentials = new NetworkCredential("pr.rezanazarii@gmail.com", "bzhn leqw piad wxvz");

            // ایجاد پیام ایمیل با مشخص کردن فرستنده، گیرنده، موضوع و متن پیام
            MailMessage message = new MailMessage("pr.rezanazarii@gmail.com", UserEmail, Subject, Body);

            // فعال‌سازی پشتیبانی از HTML در متن پیام
            message.IsBodyHtml = true;

            // تنظیم کدگذاری متن پیام به UTF8 برای پشتیبانی از کاراکترهای فارسی
            message.BodyEncoding = UTF8Encoding.UTF8;

            // درخواست اعلان تحویل موفقیت‌آمیز پیام
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            // ارسال پیام از طریق سرور SMTP
            client.Send(message);

            // بازگرداندن یک Task تکمیل شده به عنوان نتیجه
            return Task.CompletedTask;
        }
    }
}
