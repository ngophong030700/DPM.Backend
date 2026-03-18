using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.DTOs.Identity;
using Shared.Application.BaseClass;
using Shared.Domain.Exceptions;

using Identity.Application.Groups.Commands.CreateGroup;
using Identity.Application.Groups.Commands.UpdateGroup;
using Identity.Application.Groups.Commands.DeleteGroup;
using Identity.Application.Groups.Queries.GetGroup;

namespace IED.VTVMS.Host.Controllers.Identity
{
    [ApiController]
    [Authorize]
    [Route("v1/api/group")]
    [Tags("Group (Nhóm người dùng)")]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===================== CREATE =====================

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto dto)
        {
            var result = await _mediator.Send(new CreateGroupCommand(dto));
            return Ok(new EntityResponse<ViewDetailGroupDto>(result!, "Tạo nhóm thành công."));
        }

        // ===================== UPDATE =====================

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGroupDto dto)
        {
            var result = await _mediator.Send(new UpdateGroupCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailGroupDto>(result!, "Cập nhật nhóm thành công."));
        }

        // ===================== DELETE =====================

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGroupCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa nhóm thành công."));
        }

        // ===================== GET LIST =====================

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListGroupDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] string? keyword)
        {
            var result = await _mediator.Send(new GetListGroupQuery(keyword));
            return Ok(new EntityResponse<IEnumerable<ViewListGroupDto>>(result, "Lấy danh sách nhóm thành công."));
        }

        // ===================== GET DETAIL =====================

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetGroupByIdQuery(id));

            if (result == null)
                throw new NotFoundException("Nhóm không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailGroupDto>(result, "Lấy chi tiết nhóm thành công."));
        }
    }
}
