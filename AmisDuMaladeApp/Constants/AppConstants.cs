namespace AmisDuMaladeApp.Constants;

public static class AppConstants
{
#if WINDOWS || MACCATALYST
    public const string BaseUrl = "http://localhost:5113/";
#else
    // Android Emulator → 10.0.2.2 | Real device on Wi-Fi → change to 192.168.1.X
    public const string BaseUrl = "http://10.0.2.2:5113/";
#endif
    public const string TokenKey        = "auth_token";
    public const string LanguageKey     = "app_language";
    public const string DefaultLanguage = "ar";
    public const string WhatsAppNumber  = "213561000000";  // رقم جمعية أصدقاء المريض
}
