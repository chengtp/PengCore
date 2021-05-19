using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Util.Application
{
    /// <summary>
    /// application manager
    /// </summary>
    public static class ApplicationManager
    {
        /// <summary>
        /// current application info
        /// </summary>
        public static ApplicationInfo Current
        {
            get; set;
        }
    }
}
