using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class DTOExternalLogin
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
    }
}
