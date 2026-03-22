using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.WorkflowDefinitions.Commands.Configurations;

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
            var result = await _mediator.Send(new CreateGridColumnCommand(dto));
            return Ok(new EntityResponse<bool>(result, "Tạo cột thành công."));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] GridColumnConfigDto dto)
        {
            var result = await _mediator.Send(new UpdateGridColumnCommand(id, dto));
            return Ok(new EntityResponse<bool>(result, "Cập nhật cột thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGridColumnCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa cột thành công."));
        }

        [HttpPut("reorder")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Reorder([FromBody] List<ReorderItemDto> orderings)
        {
            var result = await _mediator.Send(new ReorderGridColumnsCommand(orderings));
            return Ok(new EntityResponse<bool>(result, "Cập nhật thứ tự cột thành công."));
        }
    }
}