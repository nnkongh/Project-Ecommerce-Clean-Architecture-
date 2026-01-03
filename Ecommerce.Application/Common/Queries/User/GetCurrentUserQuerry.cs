using Ecommerce.Application.DTOs.Models;
using MediatR;

namespace Ecommerce.Application.Common.Queries.User
{
    public class GetCurrentUserQuerry : IRequest<UserModel>
    {
    }
}
