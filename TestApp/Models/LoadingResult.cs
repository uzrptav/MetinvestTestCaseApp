using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class LoadingResult
    {
        public string Entity { get; set; }
        public bool AlreadyExists { get; set; }
        public string ErrorMessage { get; set; }
    }
}