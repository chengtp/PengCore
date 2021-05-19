using DDD.Util.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.Upload
{
    /// <summary>
    /// remote upload parameter
    /// </summary>
    public class RemoteUploadParameter
    {
        /// <summary>
        /// upload files
        /// </summary>
        public List<UploadFile> Files
        {
            get; set;
        }

        /// <summary>
        /// upload file parameter name
        /// </summary>
        public const string REQUEST_KEY = "upload_file_option";
    }
}
