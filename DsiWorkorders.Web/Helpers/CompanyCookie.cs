using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DsiWorkorders.Web.Helpers
{
    public class CompanyCookie
    {
        private static readonly string CompanyCookieName = "CompanyCookie";

        /// <summary>
        /// to get and set CompanyCookie
        /// </summary>
        public static string SelectedCompany
        {
            //this will return CompanyCookie if cookie exists otherwise null
            get
            {
                try
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[CompanyCookieName];
                    if (cookie != null && string.IsNullOrEmpty(cookie.Value) == false)
                    {
                        return cookie.Value;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }

            //to set CompanyCookie cookie
            set
            {
                try
                {
                    //delete existing cookie             
                    Clear(CompanyCookieName);

                    if (!string.IsNullOrEmpty(value))
                    {
                        //add new cookie
                        HttpCookie cookie = new HttpCookie(CompanyCookieName);

                        //expire cookie after 30 day
                        cookie.Expires = DateTime.Now.AddDays(30);
                        cookie.Path = "/";
                        cookie.Value = value;
                        cookie.HttpOnly = true;

                        //add to cookies
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }

                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Clear cookie by setting expires date to any time in past
        /// </summary>
        /// <param name="key"></param>
        public static void Clear(string key)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            HttpCookie cookie = new HttpCookie(key)
            {
                Expires = DateTime.Now.AddDays(-1) // or any other time in the past
            };
            httpContext.Response.Cookies.Set(cookie);
        }

    }
}