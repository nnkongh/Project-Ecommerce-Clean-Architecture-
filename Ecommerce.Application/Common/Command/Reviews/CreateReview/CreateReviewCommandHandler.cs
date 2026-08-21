using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Reviews.CreateReview
{
    public sealed class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewModel>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IProductRepository productRepo, IReviewRepository reviewRepo, IUnitOfWork uow, IMapper mapper)
        {
            _productRepo = productRepo;
            _reviewRepo = reviewRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<ReviewModel>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<ReviewModel>(new Error("", $"Sản phẩm với ID {request.ProductId} không tồn tại"));
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return Result.Failure<ReviewModel>(new Error("", "Đánh giá phải trong khoảng từ 1 đến 5 sao"));
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return Result.Failure<ReviewModel>(new Error("", "Nội dung đánh giá không được để trống"));
            }

            var review = Review.Create(request.UserId, request.ProductId, request.Rating, request.Content);
            await _reviewRepo.AddAsync(review);
            await _uow.SaveChangesAsync(cancellationToken);

            var saved = await _reviewRepo.GetByIdWithUserAsync(review.Id);
            var mapped = _mapper.Map<ReviewModel>(saved);
            return Result.Success(mapped);
        }
    }
}
