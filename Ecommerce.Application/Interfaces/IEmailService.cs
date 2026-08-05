using Ecommerce.Application.DTOs.EmailMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
