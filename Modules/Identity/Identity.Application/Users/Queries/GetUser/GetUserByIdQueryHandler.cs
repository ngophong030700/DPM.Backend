using MediatR;
using Shared.Application.DTOs.Identity;
using Identity.Application.Users.Queries;

namespace Identity.Application.Users.Queries.GetUser
{
    public class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, ViewDetailUserDto?>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUserByIdQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<ViewDetailUserDto?> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _userQueryService.GetUserByIdAsync(request.Id);
        }
    }
}