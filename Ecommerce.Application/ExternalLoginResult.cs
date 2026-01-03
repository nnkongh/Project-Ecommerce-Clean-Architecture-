using Ecommerce.Application.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application
{
    public class ExternalLoginResult 
    {
        public bool Succeeded { get; set; }
        public UserModel? User { get; set; }
        public string ErrorMessage { get; set; }
        public ExternalLoginStatus Status { get; set; }

        public static ExternalLoginResult Success(UserModel user, ExternalLoginStatus status)
        {
            return new ExternalLoginResult
            {
                Succeeded = true,
                User = user,
                Status = status
            };
        }
        public static ExternalLoginResult Failure(string errorMessage)
        {
            return new ExternalLoginResult
            {
                Succeeded = false,
                ErrorMessage = errorMessage,
                Status = ExternalLoginStatus.Failed
            };
        }

    }

    public enum ExternalLoginStatus
    {
        UserCreated,
        UserExisted,
        LinkedToExistingAccount,
        Failed
    }
}
