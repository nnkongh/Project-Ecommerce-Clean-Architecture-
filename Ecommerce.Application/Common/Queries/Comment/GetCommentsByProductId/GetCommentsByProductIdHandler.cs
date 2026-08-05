using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Comment.GetCommentsByProductId
{
    public sealed class GetCommentsByProductIdHandler : IRequestHandler<GetCommentsByProductIdQuery, Result<IReadOnlyList<CommentModel>>>
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IMapper _mapper;

        public GetCommentsByProductIdHandler(ICommentRepository commentRepo, IMapper mapper)
        {
            _commentRepo = commentRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<CommentModel>>> Handle(GetCommentsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepo.GetAllComentsByProductIdAsync(request.ProductId);
            var mapped = _mapper.Map<IReadOnlyList<CommentModel>>(comments);
            return Result.Success(mapped);
        }
    }
}
