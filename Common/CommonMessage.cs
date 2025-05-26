namespace todoApp.Common
{
    public static class CommonMessage
    {
        public static string MessageNo1(string value)
        {
            return "비밀번호는 " + value + "를 포함해야 합니다.";
        }

        public static string MessageNo2(string value1, string value2)
        {
            return "비밀번호는 " + value1 + "자 " + value2 + "이어야 합니다.";
        }

        public static string MessageNo3()
        {
            return "이미 사용중인 이메일 입니다.";
        }

        public static string MessageNo4()
        {
            return "이메일 또는 비밀번호가 올바르지 않습니다";
        }
    }
}