using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Review.GetReviewsByProductId
{
    public sealed class GetReviewsByProductIdHandler : IRequestHandler<GetReviewsByProductIdQuery, Result<IReadOnlyList<ReviewModel>>>
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;

        public GetReviewsByProductIdHandler(IReviewRepository reviewRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ReviewModel>>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepo.GetAllReviewsByProductIdAsync(request.ProductId);
            var mapped = _mapper.Map<IReadOnlyList<ReviewModel>>(reviews);
            return Result.Success(mapped);
        }
    }
}
