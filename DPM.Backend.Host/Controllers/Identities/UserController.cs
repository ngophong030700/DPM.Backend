using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.DTOs.Identity;
using Shared.Application.BaseClass;
using Shared.Domain.Exceptions;

using Identity.Application.Users.Commands.CreateUser;
using Identity.Application.Users.Commands.UpdateUser;
using Identity.Application.Users.Commands.DeleteUser;
using Identity.Application.Users.Commands.Login;
using Identity.Application.Users.Queries.GetUser;

namespace IED.VTVMS.Host.Controllers.Identity
{
    [ApiController]
    [Authorize]
    [Route("v1/api/user")]
    [Tags("User (Người dùng)")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===================== AUTH =====================

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(EntityResponse<UserLoginResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserLoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new EntityResponse<UserLoginResultDto>(result, "Đăng nhập thành công."));
        }

        // ===================== CREATE =====================

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var result = await _mediator.Send(new CreateUserCommand(dto));
            return Ok(new EntityResponse<ViewDetailUserDto>(result!, "Tạo user thành công."));
        }

        // ===================== UPDATE =====================

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailUserDto>(result!, "Cập nhật user thành công."));
        }

        // ===================== DELETE =====================

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa user thành công."));
        }

        // ===================== GET LIST =====================

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListUserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] PagingRequest request)
        {
            var result = await _mediator.Send(new GetListUserQuery(request));

            return Ok(new EntityResponse<List<ViewListUserDto>>(
                result.Items,
                "Lấy danh sách user thành công.",
                result.TotalItems
            ));
        }

        // ===================== GET DETAIL =====================

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (result == null)
                throw new NotFoundException("User không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailUserDto>(
                result,
                "Lấy chi tiết user thành công."
            ));
        }

        // ===================== FORM METADATA =====================

        [HttpGet("form-metadata")]
        [ProducesResponseType(typeof(EntityResponse<UserFormMetadataDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFormMetadata()
        {
            var result = await _mediator.Send(new GetUserFormMetadataQuery());

            return Ok(new EntityResponse<UserFormMetadataDto>(
                result,
                "Lấy dữ liệu form user thành công."
            ));
        }
    }
}