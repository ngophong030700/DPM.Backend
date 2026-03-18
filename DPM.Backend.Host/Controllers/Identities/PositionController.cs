using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.DTOs.Identity;
using Shared.Application.BaseClass;
using Shared.Domain.Exceptions;

using Identity.Application.Positions.Commands.CreatePosition;
using Identity.Application.Positions.Commands.UpdatePosition;
using Identity.Application.Positions.Commands.DeletePosition;
using Identity.Application.Positions.Queries.GetPosition;

namespace IED.VTVMS.Host.Controllers.Identity
{
    [ApiController]
    [Authorize]
    [Route("v1/api/position")]
    [Tags("Position (Chức vụ)")]
    public class PositionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PositionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===================== CREATE =====================

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailPositionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreatePositionDto dto)
        {
            var result = await _mediator.Send(new CreatePositionCommand(dto));
            return Ok(new EntityResponse<ViewDetailPositionDto>(result!, "Tạo chức vụ thành công."));
        }

        // ===================== UPDATE =====================

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailPositionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePositionDto dto)
        {
            var result = await _mediator.Send(new UpdatePositionCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailPositionDto>(result!, "Cập nhật chức vụ thành công."));
        }

        // ===================== DELETE =====================

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePositionCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa chức vụ thành công."));
        }

        // ===================== GET LIST =====================

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListPositionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] string? keyword)
        {
            var result = await _mediator.Send(new GetListPositionQuery(keyword));
            return Ok(new EntityResponse<IEnumerable<ViewListPositionDto>>(result, "Lấy danh sách chức vụ thành công."));
        }

        // ===================== GET DETAIL =====================

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailPositionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetPositionByIdQuery(id));

            if (result == null)
                throw new NotFoundException("Chức vụ không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailPositionDto>(result, "Lấy chi tiết chức vụ thành công."));
        }
    }
}
