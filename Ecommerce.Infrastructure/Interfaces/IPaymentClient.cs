using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPAY.Models;
using VNPAY.Models.Enums;

namespace Ecommerce.Infrastructure.Interfaces
{
    public interface IPaymentClient
    {
        PaymentUrlDetail CreatePaymentUrl(VnpayPaymentRequest request);

        
        PaymentUrlDetail CreatePaymentUrl(double money, string description, BankCode bankCode = BankCode.ANY);


        VnpayPaymentResult GetPaymentResult(IQueryCollection parameters);


        VnpayPaymentResult GetPaymentResult(HttpRequest httpRequest);
    }
}
