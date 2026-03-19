using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowCategories.Commands.CreateWorkflowCategory;
using Workflow.Application.WorkflowCategories.Commands.UpdateWorkflowCategory;
using Workflow.Application.WorkflowCategories.Commands.DeleteWorkflowCategory;
using Workflow.Application.WorkflowCategories.Queries.GetWorkflowCategoryById;
using Workflow.Application.WorkflowCategories.Queries.GetWorkflowCategoryList;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-category")]
    [Tags("Workflow Category (Danh mục quy trình)")]
    public class WorkflowCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===================== CREATE =====================

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailWorkflowCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateWorkflowCategoryDto dto)
        {
            var result = await _mediator.Send(new CreateWorkflowCategoryCommand(dto));
            return Ok(new EntityResponse<ViewDetailWorkflowCategoryDto>(result!, "Tạo danh mục quy trình thành công."));
        }

        // ===================== UPDATE =====================

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailWorkflowCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkflowCategoryDto dto)
        {
            var result = await _mediator.Send(new UpdateWorkflowCategoryCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailWorkflowCategoryDto>(result!, "Cập nhật danh mục quy trình thành công."));
        }

        // ===================== DELETE =====================

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteWorkflowCategoryCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa danh mục quy trình thành công."));
        }

        // ===================== GET LIST =====================

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListWorkflowCategoryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] PagingRequest request)
        {
            var result = await _mediator.Send(new GetWorkflowCategoryListQuery(request));
            return Ok(new EntityResponse<List<ViewListWorkflowCategoryDto>>(
                result.Items,
                "Lấy danh sách danh mục quy trình thành công.",
                result.TotalItems
            ));
        }

        // ===================== GET DETAIL =====================

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailWorkflowCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetWorkflowCategoryByIdQuery(id));

            if (result == null)
                throw new NotFoundException("Danh mục quy trình không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailWorkflowCategoryDto>(
                result,
                "Lấy chi tiết danh mục quy trình thành công."
            ));
        }
    }
}