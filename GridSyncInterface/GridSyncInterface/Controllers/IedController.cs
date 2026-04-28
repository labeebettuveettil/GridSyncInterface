using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// IED section CRUD: IED ? AccessPoint ? Server ? LDevice ? LN/LN0 ? DOI/DAI/DataSet.
    /// All routes scoped under /api/projects/{projectId}/...
    /// </summary>
    [Route("api/projects/{projectId:int}")]
    [Authorize]
    public class IedController : ApiBaseController
    {
        private readonly ISclElementService _scl;
        public IedController(ISclElementService scl) => _scl = scl;

        // ?? IED ???????????????????????????????????????????????????????????????
        [HttpGet("ieds")]
        public async Task<IActionResult> GetIEDs(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetIEDsAsync(projectId)));

        [HttpGet("ieds/{id:int}")]
        public async Task<IActionResult> GetIED(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetIEDAsync(projectId, id)));

        [HttpPost("ieds")]
        public async Task<IActionResult> CreateIED(int projectId, [FromBody] CreateIEDRequest req)
            => await ExecuteAsync(async () =>
            {
                var ied = await _scl.CreateIEDAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetIED), new { projectId, id = ied.Id }, ied);
            });

        [HttpPut("ieds/{id:int}")]
        public async Task<IActionResult> UpdateIED(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateIEDRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateIEDAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("ieds/{id:int}")]
        public async Task<IActionResult> DeleteIED(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteIEDAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? LDevice ???????????????????????????????????????????????????????????
        [HttpGet("servers/{serverId:int}/ldevices")]
        public async Task<IActionResult> GetLDevices(int projectId, int serverId)
            => await ExecuteAsync(async () => Ok(await _scl.GetLDevicesAsync(projectId, serverId)));

        [HttpGet("ldevices/{id:int}")]
        public async Task<IActionResult> GetLDevice(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetLDeviceAsync(projectId, id)));

        [HttpPost("ldevices")]
        public async Task<IActionResult> CreateLDevice(int projectId, [FromBody] CreateLDeviceRequest req)
            => await ExecuteAsync(async () =>
            {
                var ld = await _scl.CreateLDeviceAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetLDevice), new { projectId, id = ld.Id }, ld);
            });

        [HttpPut("ldevices/{id:int}")]
        public async Task<IActionResult> UpdateLDevice(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateLDeviceRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateLDeviceAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("ldevices/{id:int}")]
        public async Task<IActionResult> DeleteLDevice(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteLDeviceAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? LogicalNode ???????????????????????????????????????????????????????
        [HttpGet("ldevices/{ldeviceId:int}/logicalnodes")]
        public async Task<IActionResult> GetLogicalNodes(int projectId, int ldeviceId)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetLogicalNodesAsync(projectId, ldeviceId)));

        [HttpGet("logicalnodes/{id:int}")]
        public async Task<IActionResult> GetLogicalNode(int projectId, int id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetLogicalNodeAsync(projectId, id)));

        [HttpPost("logicalnodes")]
        public async Task<IActionResult> CreateLogicalNode(int projectId,
            [FromBody] CreateLogicalNodeRequest req)
            => await ExecuteAsync(async () =>
            {
                var ln = await _scl.CreateLogicalNodeAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetLogicalNode), new { projectId, id = ln.Id }, ln);
            });

        [HttpPut("logicalnodes/{id:int}")]
        public async Task<IActionResult> UpdateLogicalNode(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateLogicalNodeRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateLogicalNodeAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("logicalnodes/{id:int}")]
        public async Task<IActionResult> DeleteLogicalNode(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteLogicalNodeAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? DOI ???????????????????????????????????????????????????????????????
        [HttpGet("dois")]
        public async Task<IActionResult> GetDOIs(int projectId,
            [FromQuery] int? logicalNodeId, [FromQuery] int? ln0Id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetDOIsAsync(projectId, logicalNodeId, ln0Id)));

        [HttpGet("dois/{id:int}")]
        public async Task<IActionResult> GetDOI(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetDOIAsync(projectId, id)));

        [HttpPost("dois")]
        public async Task<IActionResult> CreateDOI(int projectId, [FromBody] CreateDOIRequest req)
            => await ExecuteAsync(async () =>
            {
                var doi = await _scl.CreateDOIAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetDOI), new { projectId, id = doi.Id }, doi);
            });

        [HttpPut("dois/{id:int}")]
        public async Task<IActionResult> UpdateDOI(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateDOIRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateDOIAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("dois/{id:int}")]
        public async Task<IActionResult> DeleteDOI(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteDOIAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? DAI ???????????????????????????????????????????????????????????????
        [HttpGet("dois/{doiId:int}/dais")]
        public async Task<IActionResult> GetDAIs(int projectId, int doiId)
            => await ExecuteAsync(async () => Ok(await _scl.GetDAIsAsync(projectId, doiId)));

        [HttpGet("dais/{id:int}")]
        public async Task<IActionResult> GetDAI(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetDAIAsync(projectId, id)));

        [HttpPost("dais")]
        public async Task<IActionResult> CreateDAI(int projectId, [FromBody] CreateDAIRequest req)
            => await ExecuteAsync(async () =>
            {
                var dai = await _scl.CreateDAIAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetDAI), new { projectId, id = dai.Id }, dai);
            });

        [HttpPut("dais/{id:int}")]
        public async Task<IActionResult> UpdateDAI(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateDAIRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateDAIAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("dais/{id:int}")]
        public async Task<IActionResult> DeleteDAI(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteDAIAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? DataSet ???????????????????????????????????????????????????????????
        [HttpGet("datasets")]
        public async Task<IActionResult> GetDataSets(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetDataSetsAsync(projectId)));

        [HttpGet("datasets/{id:int}")]
        public async Task<IActionResult> GetDataSet(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetDataSetAsync(projectId, id)));

        [HttpPost("datasets")]
        public async Task<IActionResult> CreateDataSet(int projectId, [FromBody] CreateDataSetRequest req)
            => await ExecuteAsync(async () =>
            {
                var ds = await _scl.CreateDataSetAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetDataSet), new { projectId, id = ds.Id }, ds);
            });

        [HttpPut("datasets/{id:int}")]
        public async Task<IActionResult> UpdateDataSet(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateDataSetRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateDataSetAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("datasets/{id:int}")]
        public async Task<IActionResult> DeleteDataSet(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteDataSetAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? ReportControl ???????????????????????????????????????????????????
        [HttpGet("reportcontrols")]
        public async Task<IActionResult> GetReportControls(int projectId,
            [FromQuery] int? ln0Id, [FromQuery] int? logicalNodeId)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetReportControlsAsync(projectId, ln0Id, logicalNodeId)));

        [HttpGet("reportcontrols/{id:int}")]
        public async Task<IActionResult> GetReportControl(int projectId, int id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetReportControlAsync(projectId, id)));

        [HttpPost("reportcontrols")]
        public async Task<IActionResult> CreateReportControl(int projectId,
            [FromBody] CreateReportControlRequest req)
            => await ExecuteAsync(async () =>
            {
                var rc = await _scl.CreateReportControlAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetReportControl), new { projectId, id = rc.Id }, rc);
            });

        [HttpPut("reportcontrols/{id:int}")]
        public async Task<IActionResult> UpdateReportControl(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateReportControlRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateReportControlAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("reportcontrols/{id:int}")]
        public async Task<IActionResult> DeleteReportControl(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteReportControlAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? GSEControl (GOOSE Control Block) ????????????????????????????????
        [HttpGet("ln0s/{ln0Id:int}/gsecontrols")]
        public async Task<IActionResult> GetGSEControls(int projectId, int ln0Id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetGSEControlsAsync(projectId, ln0Id)));

        [HttpGet("gsecontrols/{id:int}")]
        public async Task<IActionResult> GetGSEControl(int projectId, int id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetGSEControlAsync(projectId, id)));

        [HttpPost("gsecontrols")]
        public async Task<IActionResult> CreateGSEControl(int projectId,
            [FromBody] CreateGSEControlRequest req)
            => await ExecuteAsync(async () =>
            {
                var gc = await _scl.CreateGSEControlAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetGSEControl), new { projectId, id = gc.Id }, gc);
            });

        [HttpPut("gsecontrols/{id:int}")]
        public async Task<IActionResult> UpdateGSEControl(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateGSEControlRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateGSEControlAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("gsecontrols/{id:int}")]
        public async Task<IActionResult> DeleteGSEControl(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteGSEControlAsync(projectId, CurrentUserId, id);
                return NoContent();
            });
    }
}
