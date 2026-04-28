using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// Substation-section CRUD:
    ///   Substations ? VoltageLevels ? Bays ? ConductingEquipment / ConnectivityNodes
    ///   Also PowerTransformers.
    /// All routes are scoped under /api/projects/{projectId}/...
    /// </summary>
    [Route("api/projects/{projectId:int}")]
    [Authorize]
    public class SubstationController : ApiBaseController
    {
        private readonly ISclElementService _scl;
        public SubstationController(ISclElementService scl) => _scl = scl;

        // ?? Substations ???????????????????????????????????????????????????????
        [HttpGet("substations")]
        public async Task<IActionResult> GetSubstations(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetSubstationsAsync(projectId)));

        [HttpGet("substations/{id:int}")]
        public async Task<IActionResult> GetSubstation(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetSubstationAsync(projectId, id)));

        [HttpPost("substations")]
        public async Task<IActionResult> CreateSubstation(int projectId, [FromBody] CreateSubstationRequest req)
            => await ExecuteAsync(async () =>
            {
                var s = await _scl.CreateSubstationAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetSubstation), new { projectId, id = s.Id }, s);
            });

        [HttpPut("substations/{id:int}")]
        public async Task<IActionResult> UpdateSubstation(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateSubstationRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateSubstationAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("substations/{id:int}")]
        public async Task<IActionResult> DeleteSubstation(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteSubstationAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? VoltageLevels ?????????????????????????????????????????????????????
        [HttpGet("substations/{substationId:int}/voltagelevels")]
        public async Task<IActionResult> GetVoltageLevels(int projectId, int substationId)
            => await ExecuteAsync(async () => Ok(await _scl.GetVoltageLevelsAsync(projectId, substationId)));

        [HttpGet("voltagelevels/{id:int}")]
        public async Task<IActionResult> GetVoltageLevel(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetVoltageLevelAsync(projectId, id)));

        [HttpPost("voltagelevels")]
        public async Task<IActionResult> CreateVoltageLevel(int projectId, [FromBody] CreateVoltageLevelRequest req)
            => await ExecuteAsync(async () =>
            {
                var vl = await _scl.CreateVoltageLevelAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetVoltageLevel), new { projectId, id = vl.Id }, vl);
            });

        [HttpPut("voltagelevels/{id:int}")]
        public async Task<IActionResult> UpdateVoltageLevel(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateVoltageLevelRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateVoltageLevelAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("voltagelevels/{id:int}")]
        public async Task<IActionResult> DeleteVoltageLevel(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteVoltageLevelAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? Bays ??????????????????????????????????????????????????????????????
        [HttpGet("voltagelevels/{voltageLevelId:int}/bays")]
        public async Task<IActionResult> GetBays(int projectId, int voltageLevelId)
            => await ExecuteAsync(async () => Ok(await _scl.GetBaysAsync(projectId, voltageLevelId)));

        [HttpGet("bays/{id:int}")]
        public async Task<IActionResult> GetBay(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetBayAsync(projectId, id)));

        [HttpPost("bays")]
        public async Task<IActionResult> CreateBay(int projectId, [FromBody] CreateBayRequest req)
            => await ExecuteAsync(async () =>
            {
                var b = await _scl.CreateBayAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetBay), new { projectId, id = b.Id }, b);
            });

        [HttpPut("bays/{id:int}")]
        public async Task<IActionResult> UpdateBay(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateBayRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateBayAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("bays/{id:int}")]
        public async Task<IActionResult> DeleteBay(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteBayAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? ConductingEquipment ???????????????????????????????????????????????
        [HttpGet("bays/{bayId:int}/conductingequipments")]
        public async Task<IActionResult> GetConductingEquipments(int projectId, int bayId)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetConductingEquipmentsAsync(projectId, bayId)));

        [HttpGet("conductingequipments/{id:int}")]
        public async Task<IActionResult> GetConductingEquipment(int projectId, int id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetConductingEquipmentAsync(projectId, id)));

        [HttpPost("conductingequipments")]
        public async Task<IActionResult> CreateConductingEquipment(int projectId,
            [FromBody] CreateConductingEquipmentRequest req)
            => await ExecuteAsync(async () =>
            {
                var ce = await _scl.CreateConductingEquipmentAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetConductingEquipment), new { projectId, id = ce.Id }, ce);
            });

        [HttpPut("conductingequipments/{id:int}")]
        public async Task<IActionResult> UpdateConductingEquipment(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateConductingEquipmentRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateConductingEquipmentAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("conductingequipments/{id:int}")]
        public async Task<IActionResult> DeleteConductingEquipment(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteConductingEquipmentAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? ConnectivityNode ??????????????????????????????????????????????????
        [HttpGet("bays/{bayId:int}/connectivitynodes")]
        public async Task<IActionResult> GetConnectivityNodes(int projectId, int bayId)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetConnectivityNodesAsync(projectId, bayId)));

        [HttpGet("connectivitynodes/{id:int}")]
        public async Task<IActionResult> GetConnectivityNode(int projectId, int id)
            => await ExecuteAsync(async () =>
                Ok(await _scl.GetConnectivityNodeAsync(projectId, id)));

        [HttpPost("connectivitynodes")]
        public async Task<IActionResult> CreateConnectivityNode(int projectId,
            [FromBody] CreateConnectivityNodeRequest req)
            => await ExecuteAsync(async () =>
            {
                var cn = await _scl.CreateConnectivityNodeAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetConnectivityNode), new { projectId, id = cn.Id }, cn);
            });

        [HttpPut("connectivitynodes/{id:int}")]
        public async Task<IActionResult> UpdateConnectivityNode(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateConnectivityNodeRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateConnectivityNodeAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("connectivitynodes/{id:int}")]
        public async Task<IActionResult> DeleteConnectivityNode(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteConnectivityNodeAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? PowerTransformer ??????????????????????????????????????????????????
        [HttpGet("powertransformers")]
        public async Task<IActionResult> GetPowerTransformers(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetPowerTransformersAsync(projectId)));

        [HttpGet("powertransformers/{id:int}")]
        public async Task<IActionResult> GetPowerTransformer(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetPowerTransformerAsync(projectId, id)));

        [HttpPost("powertransformers")]
        public async Task<IActionResult> CreatePowerTransformer(int projectId,
            [FromBody] CreatePowerTransformerRequest req)
            => await ExecuteAsync(async () =>
            {
                var pt = await _scl.CreatePowerTransformerAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetPowerTransformer), new { projectId, id = pt.Id }, pt);
            });

        [HttpPut("powertransformers/{id:int}")]
        public async Task<IActionResult> UpdatePowerTransformer(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreatePowerTransformerRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdatePowerTransformerAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("powertransformers/{id:int}")]
        public async Task<IActionResult> DeletePowerTransformer(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeletePowerTransformerAsync(projectId, CurrentUserId, id);
                return NoContent();
            });
    }
}
