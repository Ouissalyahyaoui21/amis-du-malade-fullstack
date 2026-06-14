namespace AmisDuMaladeApp.Constants;

public static class AppConstants
{
#if WINDOWS || MACCATALYST
    public const string BaseUrl = "http://localhost:5113/";
#elif ANDROID
    // محاكي Android Emulator  → 10.0.2.2 يتوجه تلقائيًا لـ localhost الجهاز
    // جهاز حقيقي على Wi-Fi   → غيّر السطر التالي إلى IP الـ PC مثلاً: http://192.168.1.100:5113/
    public const string BaseUrl = "http://10.0.2.2:5113/";
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
