namespace todoApp.Common
{
    public static class CommonMessage
    {
        public static string? MessageKey { get; set; }
        public static string? TitleKey { get; set; }
        public static bool? EnableCancelBtn { get; set; } = false;

        public static string MessageNo1(string value)
        {
            return "パスワードは " + value + "を含める必要があります。";
        }

        public static string MessageNo2(string value1, string value2)
        {
            return "パスワードは " + value1 + "文字 " + value2 + "である必要があります。";
        }

        public static string MessageNo3()
        {
            return "既に使用中のメールアドレスです。";
        }

        public static string MessageNo4()
        {
            return "メールアドレスまたはパスワードが正しくありません。";
        }

        /*
        * value1: 名前
        * value2: 数
        */
        public static string MessageNo5(string value1, string value2)
        {
            return $"{value1}は最大{value2}個まで生成できます。";
        }

        public static void PrintMessage(string title, string message, bool enableCancelBtn)
        {
            TitleKey = title;
            MessageKey = message;
            EnableCancelBtn = enableCancelBtn;
        }
    }
}
