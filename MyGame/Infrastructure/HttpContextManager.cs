using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Infrastructure
{
    public static class HttpContextManager
    {
        private static HttpContextBase customContext;
        public static HttpContextBase Current
        {
            get
            {
                if (customContext != null)
                    return customContext;

                if (HttpContext.Current == null)
                    throw new InvalidOperationException("No HttpContext");

                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        public static void SetCurrentContext(HttpContextBase contextBase)
        {
            customContext = contextBase;
        }
    }
}