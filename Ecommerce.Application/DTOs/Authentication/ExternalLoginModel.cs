using Ecommerce.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Authentication
{
    public class ExternalLoginModel
    {
        //public string Code { get; set; }
        public string RedirectUri { get; set; }
        public string Provider { get; set; }


    }
}
