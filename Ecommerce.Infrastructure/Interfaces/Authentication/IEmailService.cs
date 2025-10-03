using Ecommerce.Application.DTOs.EmailMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces.Authentication
{
    public interface IEmailService
    {
        Task SendEmail(Message msg);
    }
}
