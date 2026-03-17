using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;
using Identity.Application.Users.Queries;

namespace Identity.Application.Users.Queries.GetUser
{
    public class GetListUserQueryHandler
        : IRequestHandler<GetListUserQuery, PagingResponse<ViewListUserDto>>
    {
        private readonly IUserQueryService _userQueryService;

        public GetListUserQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<PagingResponse<ViewListUserDto>> Handle(
            GetListUserQuery request,
            CancellationToken cancellationToken)
        {
            return await _userQueryService.GetListUserAsync(request.Request);
        }
    }
}