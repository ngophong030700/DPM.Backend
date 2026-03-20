using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.WorkflowDefinitions.Commands.CreateWorkflowDefinition;
using Workflow.Application.WorkflowDefinitions.Commands.UpdateWorkflowDefinition;
using Workflow.Application.WorkflowDefinitions.Commands.DeleteWorkflowDefinition;
using Workflow.Application.WorkflowDefinitions.Queries.GetWorkflowDefinitionById;
using Workflow.Application.WorkflowDefinitions.Queries.GetWorkflowDefinitionList;
using Workflow.Application.WorkflowDefinitions.Queries;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-definition")]
    [Tags("Workflow Definition (Tab 1: Thông tin quy trình)")]
    public class WorkflowDefinitionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWorkflowDefinitionQueryService _queryService;

        public WorkflowDefinitionController(
            IMediator mediator,
            IWorkflowDefinitionQueryService queryService)
        {
            _mediator = mediator;
            _queryService = queryService;
        }

        [HttpGet("form-metadata")]
        [ProducesResponseType(typeof(EntityResponse<WorkflowDefinitionMetadataDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFormMetadata()
        {
            var result = await _queryService.GetDefinitionMetadataAsync();
            return Ok(new EntityResponse<WorkflowDefinitionMetadataDto>(result, "Lấy metadata thành công."));
        }

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListWorkflowDefinitionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] PagingRequest request)
        {
            var result = await _mediator.Send(new GetWorkflowDefinitionListQuery(request));
            return Ok(new EntityResponse<List<ViewListWorkflowDefinitionDto>>(
                result.Items,
                "Lấy danh sách quy trình thành công.",
                result.TotalItems
            ));
        }

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailWorkflowDefinitionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetWorkflowDefinitionByIdQuery(id));

            if (result == null)
                throw new NotFoundException("Quy trình không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailWorkflowDefinitionDto>(
                result,
                "Lấy chi tiết quy trình thành công."
            ));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailWorkflowDefinitionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateWorkflowDefinitionDto dto)
        {
            var result = await _mediator.Send(new CreateWorkflowDefinitionCommand(dto));
            return Ok(new EntityResponse<ViewDetailWorkflowDefinitionDto>(result!, "Tạo quy trình thành công."));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailWorkflowDefinitionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkflowDefinitionDto dto)
        {
            var result = await _mediator.Send(new UpdateWorkflowDefinitionCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailWorkflowDefinitionDto>(result!, "Cập nhật quy trình thành công."));
        }
    }
}