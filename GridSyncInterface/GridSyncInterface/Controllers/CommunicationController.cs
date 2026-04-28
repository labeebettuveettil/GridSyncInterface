using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// Communication section: SubNetworks and ConnectedAPs.
    /// All routes scoped under /api/projects/{projectId}/...
    /// </summary>
    [Route("api/projects/{projectId:int}")]
    [Authorize]
    public class CommunicationController : ApiBaseController
    {
        private readonly ISclElementService _scl;
        public CommunicationController(ISclElementService scl) => _scl = scl;

        // ?? SubNetworks ???????????????????????????????????????????????????????
        [HttpGet("subnetworks")]
        public async Task<IActionResult> GetSubNetworks(int projectId)
            => await ExecuteAsync(async () => Ok(await _scl.GetSubNetworksAsync(projectId)));

        [HttpGet("subnetworks/{id:int}")]
        public async Task<IActionResult> GetSubNetwork(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetSubNetworkAsync(projectId, id)));

        [HttpPost("subnetworks")]
        public async Task<IActionResult> CreateSubNetwork(int projectId,
            [FromBody] CreateSubNetworkRequest req)
            => await ExecuteAsync(async () =>
            {
                var sn = await _scl.CreateSubNetworkAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetSubNetwork), new { projectId, id = sn.Id }, sn);
            });

        [HttpPut("subnetworks/{id:int}")]
        public async Task<IActionResult> UpdateSubNetwork(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateSubNetworkRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateSubNetworkAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("subnetworks/{id:int}")]
        public async Task<IActionResult> DeleteSubNetwork(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteSubNetworkAsync(projectId, CurrentUserId, id);
                return NoContent();
            });

        // ?? ConnectedAPs ??????????????????????????????????????????????????????
        [HttpGet("subnetworks/{subNetworkId:int}/connectedaps")]
        public async Task<IActionResult> GetConnectedAPs(int projectId, int subNetworkId)
            => await ExecuteAsync(async () => Ok(await _scl.GetConnectedAPsAsync(projectId, subNetworkId)));

        [HttpGet("connectedaps/{id:int}")]
        public async Task<IActionResult> GetConnectedAP(int projectId, int id)
            => await ExecuteAsync(async () => Ok(await _scl.GetConnectedAPAsync(projectId, id)));

        [HttpPost("connectedaps")]
        public async Task<IActionResult> CreateConnectedAP(int projectId,
            [FromBody] CreateConnectedAPRequest req)
            => await ExecuteAsync(async () =>
            {
                var cap = await _scl.CreateConnectedAPAsync(projectId, CurrentUserId, req);
                return CreatedAtAction(nameof(GetConnectedAP), new { projectId, id = cap.Id }, cap);
            });

        [HttpPut("connectedaps/{id:int}")]
        public async Task<IActionResult> UpdateConnectedAP(int projectId, int id,
            [FromBody] SclUpdateEnvelope<CreateConnectedAPRequest> env)
            => await ExecuteAsync(async () =>
                Ok(await _scl.UpdateConnectedAPAsync(projectId, CurrentUserId, id, env)));

        [HttpDelete("connectedaps/{id:int}")]
        public async Task<IActionResult> DeleteConnectedAP(int projectId, int id)
            => await ExecuteAsync(async () =>
            {
                await _scl.DeleteConnectedAPAsync(projectId, CurrentUserId, id);
                return NoContent();
            });
    }
}
