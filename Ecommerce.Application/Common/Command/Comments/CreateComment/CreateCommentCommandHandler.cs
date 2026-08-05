using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Comments.CreateComment
{
    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentModel>>
    {
        private readonly IProductRepository _productRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(IProductRepository productRepo, ICommentRepository commentRepo, IUnitOfWork uow, IMapper mapper)
        {
            _productRepo = productRepo;
            _commentRepo = commentRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<CommentModel>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<CommentModel>(new Error("", $"Sản phẩm với ID {request.ProductId} không tồn tại"));
            }

            var comment = Comment.Create(request.Content, request.ProductId, request.UserId);
            await _commentRepo.AddAsync(comment);
            await _uow.SaveChangesAsync(cancellationToken);

            var saved = await _commentRepo.GetByIdWithUserAsync(comment.Id);
            var mapped = _mapper.Map<CommentModel>(saved);
            return Result.Success(mapped);
        }
    }
}
