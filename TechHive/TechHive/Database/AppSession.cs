namespace TechHive.Database
{
    public static class AppSession
    {
        public static int LoggedInUserId { get; set; }
        public static int LoggedInSellerId { get; set; }

        public static bool IsSeller => LoggedInSellerId > 0;
        public static bool IsUser => LoggedInUserId > 0;

        public static void ClearSession()
        {
            LoggedInUserId = 0;
            LoggedInSellerId = 0;
        }
    }
}
