using System;

namespace AMS.Model
{
    public class AppSettings
    {
        public AppSettings()
        {
            CookieTimeout = 60;//默认60分钟
            CookieIsPersistent = false;
        }
        public int CookieTimeout { get; set; }

        public bool CookieIsPersistent { get; set; }
    }
}
