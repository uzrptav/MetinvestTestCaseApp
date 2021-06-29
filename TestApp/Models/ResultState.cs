using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class ResultState
    {
        public bool IsSuccess { get; set; }
        public string MessageHeader { get; set; }
        public string MessageText { get; set; }
        public string ResponseBody { get; set; }
    }
}