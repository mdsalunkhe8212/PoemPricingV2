using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
    public class SharePointImage
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public Stream Content { get; set; }
    }
}