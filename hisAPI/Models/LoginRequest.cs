using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class LoginRequest
    {
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}