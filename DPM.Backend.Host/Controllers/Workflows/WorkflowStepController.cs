using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.WorkflowDefinitions.Commands.Configurations;
using Workflow.Application.WorkflowDefinitions.Queries;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-step")]
    [Tags("Workflow Step (Tab 4: Quy trình xử lý)")]
    public class WorkflowStepController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWorkflowDefinitionQueryService _queryService;

        public WorkflowStepController(IMediator mediator, IWorkflowDefinitionQueryService queryService)
        {
            _mediator = mediator;
            _queryService = queryService;
        }

        [HttpGet("form-metadata/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<WorkflowStepMetadataDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFormMetadata(int versionId)
        {
            var result = await _queryService.GetStepMetadataAsync(versionId);
            return Ok(new EntityResponse<WorkflowStepMetadataDto>(result, "Lấy metadata thành công."));
        }

        [HttpGet("get-list/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<List<StepConfigDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(int versionId)
        {
            var data = await _queryService.GetStepsByVersionIdAsync(versionId);
            return Ok(new EntityResponse<List<StepConfigDto>>(data, "Lấy cấu hình bước quy trình thành công."));
        }

        [HttpGet("get-edges/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewWorkflowEdgeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEdges(int versionId)
        {
            var data = await _queryService.GetEdgesByVersionIdAsync(versionId);
            return Ok(new EntityResponse<List<ViewWorkflowEdgeDto>>(data, "Lấy danh sách các liên kết (edges) thành công."));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] StepConfigDto dto)
        {
            var result = await _mediator.Send(new CreateStepCommand(dto));
            return Ok(new EntityResponse<bool>(result, "Tạo bước quy trình thành công."));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, [FromBody] StepConfigDto dto)
        {
            dto.Id = id;
            var result = await _mediator.Send(new UpdateStepCommand(id, dto.VersionId, dto));
            return Ok(new EntityResponse<bool>(result, "Cập nhật cấu hình bước thành công."));
        }

        [HttpPut("update-position/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePosition(string id, [FromBody] StepPositionDto pos)
        {
            var result = await _mediator.Send(new UpdateStepPositionCommand(id, pos.VersionId, pos));
            return Ok(new EntityResponse<bool>(result, "Cập nhật tọa độ thành công."));
        }

        [HttpPut("save-canvas/{versionId}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveCanvas(int versionId, [FromBody] SetupWorkflowStepsDto dto)
        {
            var result = await _mediator.Send(new SetupWorkflowStepsCommand(versionId, dto));
            return Ok(new EntityResponse<bool>(result, "Lưu trạng thái canvas thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id, [FromQuery] int versionId)
        {
            var result = await _mediator.Send(new DeleteStepCommand(id, versionId));
            return Ok(new EntityResponse<bool>(result, "Xóa bước quy trình thành công."));
        }
    }

    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-action")]
    [Tags("Workflow Step (Tab 4: Quy trình xử lý - Actions)")]
    public class WorkflowActionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowActionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] StepActionConfigDto dto)
        {
            var result = await _mediator.Send(new CreateActionCommand(dto));
            return Ok(new EntityResponse<bool>(result, "Tạo hành động thành công."));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StepActionConfigDto dto)
        {
            var result = await _mediator.Send(new UpdateActionCommand(id, dto));
            return Ok(new EntityResponse<bool>(result, "Cập nhật hành động thành công."));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteActionCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa hành động thành công."));
        }
    }

    [ApiController]
    [Authorize]
    [Route("v1/api/workflow-step-document-define")]
    [Tags("Workflow Step (Tab 4: Quy trình xử lý - Documents)")]
    public class WorkflowStepDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowStepDocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] StepDocumentConfigDto dto)
        {
            var result = await _mediator.Send(new CreateDocumentCommand(dto));
            return Ok(new EntityResponse<bool>(result, "Thêm yêu cầu tài liệu thành công."));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StepDocumentConfigDto dto)
        {
            var result = await _mediator.Send(new UpdateDocumentCommand(id, dto));
            return Ok(new EntityResponse<bool>(result, "Cập nhật yêu cầu tài liệu thành công."));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDocumentCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa yêu cầu tài liệu thành công."));
        }
    }
}