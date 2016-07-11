using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttachmentPage.Models
{
    public class Attachment
    {
        public string FileName { set; get; }
        public string FileType { set; get; }
        public bool EnableDelete { set; get; }
    }
}