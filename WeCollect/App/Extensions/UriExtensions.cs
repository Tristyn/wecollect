using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WeCollect.App.Extensions
{
    public static class UriExtensions
    {
        public static string GetDirectory(this Uri uri)
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).DirectoryName;
        }
    }
}
