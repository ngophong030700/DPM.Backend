using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Shared.Domain.Exceptions;
using Workflow.Application.MasterDataSources.Commands.AddMasterDataValue;
using Workflow.Application.MasterDataSources.Commands.UpdateMasterDataValue;
using Workflow.Application.MasterDataSources.Commands.DeleteMasterDataValue;
using Workflow.Application.MasterDataSources.Commands.CreateMasterDataSource;
using Workflow.Application.MasterDataSources.Commands.UpdateMasterDataSource;
using Workflow.Application.MasterDataSources.Commands.DeleteMasterDataSource;
using Workflow.Application.MasterDataSources.Queries.GetMasterDataSourceById;
using Workflow.Application.MasterDataSources.Queries.GetMasterDataSourceList;
using Workflow.Application.MasterDataSources.Queries.GetMasterDataValues;

namespace DPM.Backend.Host.Controllers.Workflows
{
    [ApiController]
    [Authorize]
    [Route("v1/api/master-data-source")]
    [Tags("Master Data Source (Nguồn dữ liệu danh mục)")]
    public class MasterDataSourceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MasterDataSourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ===================== SOURCE =====================

        [HttpPost("create")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailMasterDataSourceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateMasterDataSourceDto dto)
        {
            var result = await _mediator.Send(new CreateMasterDataSourceCommand(dto));
            return Ok(new EntityResponse<ViewDetailMasterDataSourceDto>(result!, "Tạo nguồn dữ liệu thành công."));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailMasterDataSourceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMasterDataSourceDto dto)
        {
            var result = await _mediator.Send(new UpdateMasterDataSourceCommand(id, dto));
            return Ok(new EntityResponse<ViewDetailMasterDataSourceDto>(result!, "Cập nhật nguồn dữ liệu thành công."));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteMasterDataSourceCommand(id));
            return Ok(new EntityResponse<bool>(result, "Xóa nguồn dữ liệu thành công."));
        }

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewListMasterDataSourceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] PagingRequest request)
        {
            var result = await _mediator.Send(new GetMasterDataSourceListQuery(request));
            return Ok(new EntityResponse<List<ViewListMasterDataSourceDto>>(
                result.Items,
                "Lấy danh sách nguồn dữ liệu thành công.",
                result.TotalItems
            ));
        }

        [HttpGet("get-detail/{id}")]
        [ProducesResponseType(typeof(EntityResponse<ViewDetailMasterDataSourceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetMasterDataSourceByIdQuery(id));

            if (result == null)
                throw new NotFoundException("Nguồn dữ liệu không tồn tại hoặc đã bị xóa.");

            return Ok(new EntityResponse<ViewDetailMasterDataSourceDto>(
                result,
                "Lấy chi tiết nguồn dữ liệu thành công."
            ));
        }

        // ===================== VALUES (ROWS) =====================

        [HttpGet("{sourceId}/values")]
        [ProducesResponseType(typeof(EntityResponse<List<ViewMasterDataValueDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetValues(int sourceId, [FromQuery] PagingRequest request)
        {
            var result = await _mediator.Send(new GetMasterDataValuesQuery(sourceId, request));
            return Ok(new EntityResponse<List<ViewMasterDataValueDto>>(
                result.Items,
                "Lấy danh sách dòng dữ liệu thành công.",
                result.TotalItems
            ));
        }

        [HttpPost("{sourceId}/add-value")]
        [ProducesResponseType(typeof(EntityResponse<ViewMasterDataValueDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddValue(int sourceId, [FromBody] CreateMasterDataValueDto dto)
        {
            var result = await _mediator.Send(new AddMasterDataValueCommand(sourceId, dto));
            return Ok(new EntityResponse<ViewMasterDataValueDto>(result!, "Thêm dòng dữ liệu thành công."));
        }

        [HttpPut("update-value/{valueId}")]
        [ProducesResponseType(typeof(EntityResponse<ViewMasterDataValueDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateValue(int valueId, [FromBody] UpdateMasterDataValueDto dto)
        {
            var result = await _mediator.Send(new UpdateMasterDataValueCommand(valueId, dto));
            return Ok(new EntityResponse<ViewMasterDataValueDto>(result!, "Cập nhật dòng dữ liệu thành công."));
        }

        [HttpDelete("delete-value/{valueId}")]
        [ProducesResponseType(typeof(EntityResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteValue(int valueId)
        {
            var result = await _mediator.Send(new DeleteMasterDataValueCommand(valueId));
            return Ok(new EntityResponse<bool>(result, "Xóa dòng dữ liệu thành công."));
        }
    }
}