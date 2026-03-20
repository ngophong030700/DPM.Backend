using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-grid-column")]
    [Tags("Workflow Grid Column (Tab 2: Cột của trường dạng Grid)")]
    public class WorkflowGridColumnController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowGridColumnController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GridColumnConfigDto dto)
        {
            // Placeholder: Call Command
            return Ok(new EntityResponse<bool>(true, "Tạo cột thành công."));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] GridColumnConfigDto dto)
        {
            dto.Id = id;
            return Ok(new EntityResponse<bool>(true, "Cập nhật cột thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(new EntityResponse<bool>(true, "Xóa cột thành công."));
        }

        [HttpPut("reorder")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Reorder([FromBody] List<ReorderItemDto> orderings)
        {
            return Ok(new EntityResponse<bool>(true, "Cập nhật thứ tự cột thành công."));
        }
    }
}