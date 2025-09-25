﻿using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Wishlists.RemoveItemInWishlist
{
    public sealed record RemoveItemWishlistCommand(int productId, int wishlistId) : IRequest<Result>
    {
    }
}
