using Ecommerce.WebApi.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [Authorize]
    [Route("order")]
    public class OrderController : ApiController
    {
        public OrderController(ISender sender) : base(sender)
        {
        }
        
    }
}
