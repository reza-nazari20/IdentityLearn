using System.Net;

namespace Identity.Services
{
    // کلاس سرویس برای ارسال پیامک از طریق API کاوه نگار
    public class SmsService
    {
        // متد ارسال پیامک که شماره تلفن و کد تایید را دریافت می‌کند
        public void Send(string PhoneNumber, string Code)
        {
            // ایجاد یک نمونه از کلاینت وب برای ارسال درخواست HTTP
            var client = new WebClient();

            // ساخت آدرس URL برای فراخوانی API کاوه نگار با پارامترهای زیر:
            // به جای "apikey" باید کلید اختصاصی API که از پنل کاوه نگار دریافت کرده‌اید را جایگزین کنید
            // receptor: شماره تلفن گیرنده پیامک
            // token: کد تایید که باید در پیامک ارسال شود
            // template: قالب پیش‌تعریف شده در پنل کاوه نگار به نام VerifyTECHBACKAccount
            // این آدرس به سرویس lookup کاوه نگار متصل می‌شود که امکان ارسال پیامک‌های اعتبارسنجی را فراهم می‌کند
            string url = $"https://api.kavenegar.com/v1/apikey/lookup.json?receptor={PhoneNumber}&token={Code}&template=VerifyTECHBACKAccount";

            // ارسال درخواست GET به API و دریافت پاسخ
            var content = client.DownloadString(url);
        }
    }
}
