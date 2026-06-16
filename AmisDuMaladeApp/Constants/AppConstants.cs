namespace AmisDuMaladeApp.Constants;

public static class AppConstants
{
#if WINDOWS || MACCATALYST
    public const string BaseUrl = "http://localhost:5113/";
#elif ANDROID
#if EMULATOR
    // محاكي: 10.0.2.2 يشير تلقائياً إلى localhost في الجهاز المضيف
    public const string BaseUrl = "http://10.0.2.2:5113/";
#else
    // جهاز حقيقي: IP الحاسوب على شبكة Wi-Fi المحلية (ipconfig → Wi-Fi IPv4)
    // مثال: http://192.168.1.100:5113/
    // تأكد من تشغيل الباك-اند بـ: dotnet run --urls "http://0.0.0.0:5113"
    public const string BaseUrl = "http://192.168.1.12:5113/";
#endif
#else
    public const string BaseUrl = "http://10.0.2.2:5113/";
#endif
    public const string TokenKey        = "auth_token";
    public const string LanguageKey     = "app_language";
    public const string DefaultLanguage = "ar";
    public const string WhatsAppNumber  = "213667476967";  // واتساب جمعية أصدقاء المريض سكيكدة
    public const string PhoneNumber     = "0780422194";    // هاتف الجمعية
    public const string ContactEmail    = "lam_skikda@yahoo.fr";
    public const string FacebookUrl     = "https://www.facebook.com/profile.php?id=100063806393009";
}
