using POEM.Model.Model;
using System.Web;

namespace POEMPricing.Managers
{
    public static class CurrentUserManager
    {
        private const string CurrentUserKey = "CurrentUser";

        private static UserSession User
        {
            get
            {
                return HttpContext.Current?.Items[CurrentUserKey] as UserSession;
            }
        }

        public static int LoginId
        {
            get
            {
                return User?.LoginId ?? 0;
            }
        }

        public static string FullName
        {
            get
            {
                return User?.FullName ?? string.Empty;
            }
        }
    }
}