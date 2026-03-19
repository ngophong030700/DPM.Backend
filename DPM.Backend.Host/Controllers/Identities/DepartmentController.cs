using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.DTOs.Identity;
using Shared.Application.BaseClass;
using Shared.Domain.Exceptions;

using Identity.Application.Departments.Commands.CreateDepartment;
using Identity.Application.Departments.Commands.UpdateDepartment;
using Identity.Application.Departments.Commands.DeleteDepartment;
using Identity.Application.Departments.Queries.GetDepartment;

namespace IED.VTVMS.Host.Controllers.Identity
{
    [ApiController]
    [Authorize]
    [Route("v1/api/department")]
    [Tags("Department (Phòng ban)")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===================== CREATE =====================

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailDepartmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            var result = await _mediator.Send(new CreateDepartmentCommand(dto));
            return Ok(new EntityResponse<ViewDetailDepartmentDto>(result!, "Tạo phòng ban thành công."));
        }

        // ===================== UPDATE =====================

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailDepartmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            var result = await _mediator.Send(new UpdateDepartmentCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailDepartmentDto>(result!, "Cập nhật phòng ban thành công."));
        }

        // ===================== DELETE =====================

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDepartmentCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa phòng ban thành công."));
        }

        // ===================== GET LIST =====================

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListDepartmentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] PagingRequest request)
        {
            var result = await _mediator.Send(new GetListDepartmentQuery(request));
            return Ok(new EntityResponse<List<ViewListDepartmentDto>>(
                result.Items,
                "Lấy danh sách phòng ban thành công.",
                result.TotalItems
            ));
        }

        // ===================== GET DETAIL =====================

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailDepartmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetDepartmentByIdQuery(id));

            if (result == null)
                throw new NotFoundException("Phòng ban không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailDepartmentDto>(result, "Lấy chi tiết phòng ban thành công."));
        }
    }
}
