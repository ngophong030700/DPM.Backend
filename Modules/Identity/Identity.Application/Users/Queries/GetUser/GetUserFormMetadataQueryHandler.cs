using MediatR;
using Shared.Application.DTOs.Identity;
using Identity.Application.Users.Queries;

namespace Identity.Application.Users.Queries.GetUser
{
    public class GetUserFormMetadataQueryHandler
        : IRequestHandler<GetUserFormMetadataQuery, UserFormMetadataDto>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUserFormMetadataQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<UserFormMetadataDto> Handle(
            GetUserFormMetadataQuery request,
            CancellationToken cancellationToken)
        {
            return await _userQueryService.GetUserFormMetadataAsync();
        }
    }
}