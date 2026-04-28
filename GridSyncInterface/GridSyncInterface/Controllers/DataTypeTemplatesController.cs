using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// DataTypeTemplates section: LNodeTypes, DOTypes, DATypes, EnumTypes.
    /// All routes scoped under /api/projects/{projectId}/...
    /// </summary>
    [Route("api/projects/{projectId:int}")]
    [Authorize]
    public class DataTypeTemplatesController : ApiBaseController
    {
        private readonly ISclElementService _scl;
        public DataTypeTemplatesController(ISclElementService scl) => _scl = scl;

        // ?? LNodeTypes ???????????????????????????????????????????????????????
        [HttpGet("lnodetypes")]
        public async Task<IActionResult> GetLNodeTypes(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetLNodeTypesAsync(projectId)));

        [HttpGet("lnodetypes/{id:int}")]
        public async Task<IActionResult> GetLNodeType(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetLNodeTypeAsync(projectId, id)));

        [HttpPost("lnodetypes")]
        public async Task<IActionResult> CreateLNodeType(int projectId,
            [FromBody] CreateLNodeTypeRequest req)
            => await ExecuteAsync(async () =>
            {
                var lt = await _scl.CreateLNodeTypeAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetLNodeType), new { projectId, id = lt.Id }, lt);
            });

        [HttpPut("lnodetypes/{id:int}")]
        public async Task<IActionResult> UpdateLNodeType(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateLNodeTypeRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateLNodeTypeAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("lnodetypes/{id:int}")]
        public async Task<IActionResult> DeleteLNodeType(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteLNodeTypeAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? DOTypes ???????????????????????????????????????????????????????????
        [HttpGet("dotypes")]
        public async Task<IActionResult> GetDOTypes(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetDOTypesAsync(projectId)));

        [HttpGet("dotypes/{id:int}")]
        public async Task<IActionResult> GetDOType(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetDOTypeAsync(projectId, id)));

        [HttpPost("dotypes")]
        public async Task<IActionResult> CreateDOType(int projectId,
            [FromBody] CreateDOTypeRequest req)
            => await ExecuteAsync(async () =>
            {
                var dt = await _scl.CreateDOTypeAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetDOType), new { projectId, id = dt.Id }, dt);
            });

        [HttpPut("dotypes/{id:int}")]
        public async Task<IActionResult> UpdateDOType(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateDOTypeRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateDOTypeAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("dotypes/{id:int}")]
        public async Task<IActionResult> DeleteDOType(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteDOTypeAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? DATypes ???????????????????????????????????????????????????????????
        [HttpGet("datypes")]
        public async Task<IActionResult> GetDATypes(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetDATypesAsync(projectId)));

        [HttpGet("datypes/{id:int}")]
        public async Task<IActionResult> GetDAType(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetDATypeAsync(projectId, id)));

        [HttpPost("datypes")]
        public async Task<IActionResult> CreateDAType(int projectId,
            [FromBody] CreateDATypeRequest req)
            => await ExecuteAsync(async () =>
            {
                var dt = await _scl.CreateDATypeAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetDAType), new { projectId, id = dt.Id }, dt);
            });

        [HttpPut("datypes/{id:int}")]
        public async Task<IActionResult> UpdateDAType(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateDATypeRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateDATypeAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("datypes/{id:int}")]
        public async Task<IActionResult> DeleteDAType(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteDATypeAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? EnumTypes ?????????????????????????????????????????????????????????
        [HttpGet("enumtypes")]
        public async Task<IActionResult> GetEnumTypes(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetEnumTypesAsync(projectId)));

        [HttpGet("enumtypes/{id:int}")]
        public async Task<IActionResult> GetEnumType(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetEnumTypeAsync(projectId, id)));

        [HttpPost("enumtypes")]
        public async Task<IActionResult> CreateEnumType(int projectId,
            [FromBody] CreateEnumTypeRequest req)
            => await ExecuteAsync(async () =>
            {
                var et = await _scl.CreateEnumTypeAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetEnumType), new { projectId, id = et.Id }, et);
            });

        [HttpPut("enumtypes/{id:int}")]
        public async Task<IActionResult> UpdateEnumType(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateEnumTypeRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateEnumTypeAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("enumtypes/{id:int}")]
        public async Task<IActionResult> DeleteEnumType(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteEnumTypeAsync(projectId, CurrentUserId, id);
                return NoContent();
            });
    }
}
