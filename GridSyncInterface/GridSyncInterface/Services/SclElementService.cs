using System.Text.Json;
using GridSyncInterface.Data;
using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Models;
using GridSyncInterface.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace GridSyncInterface.Services
{
    /// <summary>
    /// Handles CRUD for all SCL model elements.
    /// Every mutating method:
    ///   1. Verifies the user has access to the project.
    ///   2. Calls <see cref="IConflictResolutionService.CheckAsync"/> (lock + optimistic concurrency).
    ///   3. Persists the change.
    ///   4. Writes an audit entry.
    /// </summary>
    public interface ISclElementService
    {
        // Substation hierarchy
        Task<SubstationDto>        GetSubstationAsync(int projectId, int id);
        Task<IEnumerable<SubstationDto>> GetSubstationsAsync(int projectId);
        Task<SubstationDto>        CreateSubstationAsync(int projectId, int userId, CreateSubstationRequest req);
        Task<SubstationDto>        UpdateSubstationAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateSubstationRequest> env);
        Task                       DeleteSubstationAsync(int projectId, int userId, int id);

        Task<VoltageLevelDto>      GetVoltageLevelAsync(int projectId, int id);
        Task<IEnumerable<VoltageLevelDto>> GetVoltageLevelsAsync(int projectId, int substationId);
        Task<VoltageLevelDto>      CreateVoltageLevelAsync(int projectId, int userId, CreateVoltageLevelRequest req);
        Task<VoltageLevelDto>      UpdateVoltageLevelAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateVoltageLevelRequest> env);
        Task                       DeleteVoltageLevelAsync(int projectId, int userId, int id);

        Task<BayDto>               GetBayAsync(int projectId, int id);
        Task<IEnumerable<BayDto>>  GetBaysAsync(int projectId, int voltageLevelId);
        Task<BayDto>               CreateBayAsync(int projectId, int userId, CreateBayRequest req);
        Task<BayDto>               UpdateBayAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateBayRequest> env);
        Task                       DeleteBayAsync(int projectId, int userId, int id);

        // IED hierarchy
        Task<IEDDto>               GetIEDAsync(int projectId, int id);
        Task<IEnumerable<IEDDto>>  GetIEDsAsync(int projectId);
        Task<IEDDto>               CreateIEDAsync(int projectId, int userId, CreateIEDRequest req);
        Task<IEDDto>               UpdateIEDAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateIEDRequest> env);
        Task                       DeleteIEDAsync(int projectId, int userId, int id);

        Task<LDeviceDto>           GetLDeviceAsync(int projectId, int id);
        Task<IEnumerable<LDeviceDto>> GetLDevicesAsync(int projectId, int serverId);
        Task<LDeviceDto>           CreateLDeviceAsync(int projectId, int userId, CreateLDeviceRequest req);
        Task<LDeviceDto>           UpdateLDeviceAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateLDeviceRequest> env);
        Task                       DeleteLDeviceAsync(int projectId, int userId, int id);

        Task<LogicalNodeDto>        GetLogicalNodeAsync(int projectId, int id);
        Task<IEnumerable<LogicalNodeDto>> GetLogicalNodesAsync(int projectId, int ldeviceId);
        Task<LogicalNodeDto>        CreateLogicalNodeAsync(int projectId, int userId, CreateLogicalNodeRequest req);
        Task<LogicalNodeDto>        UpdateLogicalNodeAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateLogicalNodeRequest> env);
        Task                        DeleteLogicalNodeAsync(int projectId, int userId, int id);

        Task<ConductingEquipmentDto>  GetConductingEquipmentAsync(int projectId, int id);
        Task<IEnumerable<ConductingEquipmentDto>> GetConductingEquipmentsAsync(int projectId, int bayId);
        Task<ConductingEquipmentDto>  CreateConductingEquipmentAsync(int projectId, int userId, CreateConductingEquipmentRequest req);
        Task<ConductingEquipmentDto>  UpdateConductingEquipmentAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateConductingEquipmentRequest> env);
        Task                          DeleteConductingEquipmentAsync(int projectId, int userId, int id);

        Task<ConnectivityNodeDto>     GetConnectivityNodeAsync(int projectId, int id);
        Task<IEnumerable<ConnectivityNodeDto>> GetConnectivityNodesAsync(int projectId, int bayId);
        Task<ConnectivityNodeDto>     CreateConnectivityNodeAsync(int projectId, int userId, CreateConnectivityNodeRequest req);
        Task<ConnectivityNodeDto>     UpdateConnectivityNodeAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateConnectivityNodeRequest> env);
        Task                          DeleteConnectivityNodeAsync(int projectId, int userId, int id);

        Task<PowerTransformerDto>     GetPowerTransformerAsync(int projectId, int id);
        Task<IEnumerable<PowerTransformerDto>> GetPowerTransformersAsync(int projectId);
        Task<PowerTransformerDto>     CreatePowerTransformerAsync(int projectId, int userId, CreatePowerTransformerRequest req);
        Task<PowerTransformerDto>     UpdatePowerTransformerAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreatePowerTransformerRequest> env);
        Task                          DeletePowerTransformerAsync(int projectId, int userId, int id);

        Task<DOIDto>                  GetDOIAsync(int projectId, int id);
        Task<IEnumerable<DOIDto>>     GetDOIsAsync(int projectId, int? logicalNodeId, int? ln0Id);
        Task<DOIDto>                  CreateDOIAsync(int projectId, int userId, CreateDOIRequest req);
        Task<DOIDto>                  UpdateDOIAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateDOIRequest> env);
        Task                          DeleteDOIAsync(int projectId, int userId, int id);

        Task<DAIDto>                  GetDAIAsync(int projectId, int id);
        Task<IEnumerable<DAIDto>>     GetDAIsAsync(int projectId, int doiId);
        Task<DAIDto>                  CreateDAIAsync(int projectId, int userId, CreateDAIRequest req);
        Task<DAIDto>                  UpdateDAIAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateDAIRequest> env);
        Task                          DeleteDAIAsync(int projectId, int userId, int id);

        Task<DataSetDto>              GetDataSetAsync(int projectId, int id);
        Task<IEnumerable<DataSetDto>> GetDataSetsAsync(int projectId);
        Task<DataSetDto>              CreateDataSetAsync(int projectId, int userId, CreateDataSetRequest req);
        Task<DataSetDto>              UpdateDataSetAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateDataSetRequest> env);
        Task                          DeleteDataSetAsync(int projectId, int userId, int id);

        Task<SubNetworkDto>           GetSubNetworkAsync(int projectId, int id);
        Task<IEnumerable<SubNetworkDto>> GetSubNetworksAsync(int projectId);
        Task<SubNetworkDto>           CreateSubNetworkAsync(int projectId, int userId, CreateSubNetworkRequest req);
        Task<SubNetworkDto>           UpdateSubNetworkAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateSubNetworkRequest> env);
        Task                          DeleteSubNetworkAsync(int projectId, int userId, int id);

        Task<LNodeTypeDto>            GetLNodeTypeAsync(int projectId, int id);
        Task<IEnumerable<LNodeTypeDto>> GetLNodeTypesAsync(int projectId);
        Task<LNodeTypeDto>            CreateLNodeTypeAsync(int projectId, int userId, CreateLNodeTypeRequest req);
        Task<LNodeTypeDto>            UpdateLNodeTypeAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateLNodeTypeRequest> env);
        Task                          DeleteLNodeTypeAsync(int projectId, int userId, int id);

        Task<ConnectedAPDto>            GetConnectedAPAsync(int projectId, int id);
        Task<IEnumerable<ConnectedAPDto>> GetConnectedAPsAsync(int projectId, int subNetworkId);
        Task<ConnectedAPDto>            CreateConnectedAPAsync(int projectId, int userId, CreateConnectedAPRequest req);
        Task<ConnectedAPDto>            UpdateConnectedAPAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateConnectedAPRequest> env);
        Task                            DeleteConnectedAPAsync(int projectId, int userId, int id);

        Task<DOTypeDto>                 GetDOTypeAsync(int projectId, int id);
        Task<IEnumerable<DOTypeDto>>    GetDOTypesAsync(int projectId);
        Task<DOTypeDto>                 CreateDOTypeAsync(int projectId, int userId, CreateDOTypeRequest req);
        Task<DOTypeDto>                 UpdateDOTypeAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateDOTypeRequest> env);
        Task                            DeleteDOTypeAsync(int projectId, int userId, int id);

        Task<DATypeDto>                 GetDATypeAsync(int projectId, int id);
        Task<IEnumerable<DATypeDto>>    GetDATypesAsync(int projectId);
        Task<DATypeDto>                 CreateDATypeAsync(int projectId, int userId, CreateDATypeRequest req);
        Task<DATypeDto>                 UpdateDATypeAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateDATypeRequest> env);
        Task                            DeleteDATypeAsync(int projectId, int userId, int id);

        Task<EnumTypeDto>               GetEnumTypeAsync(int projectId, int id);
        Task<IEnumerable<EnumTypeDto>>  GetEnumTypesAsync(int projectId);
        Task<EnumTypeDto>               CreateEnumTypeAsync(int projectId, int userId, CreateEnumTypeRequest req);
        Task<EnumTypeDto>               UpdateEnumTypeAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateEnumTypeRequest> env);
        Task                            DeleteEnumTypeAsync(int projectId, int userId, int id);

        // ReportControl
        Task<ReportControlDto>              GetReportControlAsync(int projectId, int id);
        Task<IEnumerable<ReportControlDto>> GetReportControlsAsync(int projectId, int? ln0Id, int? logicalNodeId);
        Task<ReportControlDto>              CreateReportControlAsync(int projectId, int userId, CreateReportControlRequest req);
        Task<ReportControlDto>              UpdateReportControlAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateReportControlRequest> env);
        Task                                DeleteReportControlAsync(int projectId, int userId, int id);

        // GSEControl (GOOSE Control Block)
        Task<GSEControlDto>              GetGSEControlAsync(int projectId, int id);
        Task<IEnumerable<GSEControlDto>> GetGSEControlsAsync(int projectId, int ln0Id);
        Task<GSEControlDto>              CreateGSEControlAsync(int projectId, int userId, CreateGSEControlRequest req);
        Task<GSEControlDto>              UpdateGSEControlAsync(int projectId, int userId, int id, SclUpdateEnvelope<CreateGSEControlRequest> env);
        Task                             DeleteGSEControlAsync(int projectId, int userId, int id);
    }

    public class SclElementService : ISclElementService
    {
        private readonly SclDbContext _db;
        private readonly IAuditService _audit;
        private readonly IConflictResolutionService _conflict;

        public SclElementService(SclDbContext db, IAuditService audit, IConflictResolutionService conflict)
        {
            _db      = db;
            _audit   = audit;
            _conflict = conflict;
        }

        // ????????????????????????????????????????????????????????????????????
        // Substation
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<SubstationDto>> GetSubstationsAsync(int projectId)
        {
            var sclId = await GetSclIdAsync(projectId);
            var items = await _db.Substations.Where(s => s.SclId == sclId).ToListAsync();
            return items.Select(MapSubstation);
        }

        public async Task<SubstationDto> GetSubstationAsync(int projectId, int id)
            => MapSubstation(await LoadSubstationAsync(projectId, id));

        public async Task<SubstationDto> CreateSubstationAsync(int projectId, int userId, CreateSubstationRequest req)
        {
            var sclId = await GetSclIdAsync(projectId);
            var entity = new Substation { SclId = sclId, Name = req.Name, Desc = req.Desc };
            _db.Substations.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "Substation", entity.Id, "Create", null, Ser(req));
            return MapSubstation(entity);
        }

        public async Task<SubstationDto> UpdateSubstationAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateSubstationRequest> env)
        {
            var entity = await LoadSubstationAsync(projectId, id);
            await _conflict.CheckAsync(projectId, userId, "Substation", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name;
            entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "Substation", id, "Update", old, Ser(entity), env.Comment);
            return MapSubstation(entity);
        }

        public async Task DeleteSubstationAsync(int projectId, int userId, int id)
        {
            var entity = await LoadSubstationAsync(projectId, id);
            await _conflict.CheckAsync(projectId, userId, "Substation", id, null, entity);
            _db.Substations.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "Substation", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // VoltageLevel
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<VoltageLevelDto>> GetVoltageLevelsAsync(int projectId, int substationId)
        {
            var items = await _db.VoltageLevels.Where(v => v.SubstationId == substationId).ToListAsync();
            return items.Select(MapVoltageLevel);
        }

        public async Task<VoltageLevelDto> GetVoltageLevelAsync(int projectId, int id)
            => MapVoltageLevel(await LoadAsync<VoltageLevel>(_db.VoltageLevels, id));

        public async Task<VoltageLevelDto> CreateVoltageLevelAsync(int projectId, int userId, CreateVoltageLevelRequest req)
        {
            var entity = new VoltageLevel
            {
                SubstationId     = req.SubstationId,
                Name             = req.Name, Desc = req.Desc,
                NomFreq          = req.NomFreq, NumPhases = req.NumPhases,
                VoltageValue     = req.VoltageValue,
                VoltageMultiplier = req.VoltageMultiplier
            };
            _db.VoltageLevels.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "VoltageLevel", entity.Id, "Create", null, Ser(req));
            return MapVoltageLevel(entity);
        }

        public async Task<VoltageLevelDto> UpdateVoltageLevelAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateVoltageLevelRequest> env)
        {
            var entity = await LoadAsync<VoltageLevel>(_db.VoltageLevels, id);
            await _conflict.CheckAsync(projectId, userId, "VoltageLevel", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.Desc = env.Payload.Desc;
            entity.NomFreq = env.Payload.NomFreq; entity.NumPhases = env.Payload.NumPhases;
            entity.VoltageValue = env.Payload.VoltageValue; entity.VoltageMultiplier = env.Payload.VoltageMultiplier;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "VoltageLevel", id, "Update", old, Ser(entity), env.Comment);
            return MapVoltageLevel(entity);
        }

        public async Task DeleteVoltageLevelAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<VoltageLevel>(_db.VoltageLevels, id);
            await _conflict.CheckAsync(projectId, userId, "VoltageLevel", id, null, entity);
            _db.VoltageLevels.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "VoltageLevel", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // Bay
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<BayDto>> GetBaysAsync(int projectId, int voltageLevelId)
        {
            var items = await _db.Bays.Where(b => b.VoltageLevelId == voltageLevelId).ToListAsync();
            return items.Select(MapBay);
        }

        public async Task<BayDto> GetBayAsync(int projectId, int id)
            => MapBay(await LoadAsync<Bay>(_db.Bays, id));

        public async Task<BayDto> CreateBayAsync(int projectId, int userId, CreateBayRequest req)
        {
            var entity = new Bay { VoltageLevelId = req.VoltageLevelId, Name = req.Name, Desc = req.Desc };
            _db.Bays.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "Bay", entity.Id, "Create", null, Ser(req));
            return MapBay(entity);
        }

        public async Task<BayDto> UpdateBayAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateBayRequest> env)
        {
            var entity = await LoadAsync<Bay>(_db.Bays, id);
            await _conflict.CheckAsync(projectId, userId, "Bay", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "Bay", id, "Update", old, Ser(entity), env.Comment);
            return MapBay(entity);
        }

        public async Task DeleteBayAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<Bay>(_db.Bays, id);
            await _conflict.CheckAsync(projectId, userId, "Bay", id, null, entity);
            _db.Bays.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "Bay", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // IED
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<IEDDto>> GetIEDsAsync(int projectId)
        {
            var sclId = await GetSclIdAsync(projectId);
            var items = await _db.IEDs.Where(i => i.SclId == sclId).ToListAsync();
            return items.Select(MapIED);
        }

        public async Task<IEDDto> GetIEDAsync(int projectId, int id)
            => MapIED(await LoadAsync<IED>(_db.IEDs, id));

        public async Task<IEDDto> CreateIEDAsync(int projectId, int userId, CreateIEDRequest req)
        {
            var sclId = await GetSclIdAsync(projectId);
            var entity = new IED
            {
                SclId = sclId, Name = req.Name, IedType = req.IedType,
                Manufacturer = req.Manufacturer, ConfigVersion = req.ConfigVersion, Desc = req.Desc
            };
            _db.IEDs.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "IED", entity.Id, "Create", null, Ser(req));
            return MapIED(entity);
        }

        public async Task<IEDDto> UpdateIEDAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateIEDRequest> env)
        {
            var entity = await LoadAsync<IED>(_db.IEDs, id);
            await _conflict.CheckAsync(projectId, userId, "IED", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.IedType = env.Payload.IedType;
            entity.Manufacturer = env.Payload.Manufacturer;
            entity.ConfigVersion = env.Payload.ConfigVersion; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "IED", id, "Update", old, Ser(entity), env.Comment);
            return MapIED(entity);
        }

        public async Task DeleteIEDAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<IED>(_db.IEDs, id);
            await _conflict.CheckAsync(projectId, userId, "IED", id, null, entity);
            _db.IEDs.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "IED", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // LDevice
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<LDeviceDto>> GetLDevicesAsync(int projectId, int serverId)
        {
            var items = await _db.LDevices.Where(l => l.ServerId == serverId).ToListAsync();
            return items.Select(MapLDevice);
        }

        public async Task<LDeviceDto> GetLDeviceAsync(int projectId, int id)
            => MapLDevice(await LoadAsync<LDevice>(_db.LDevices, id));

        public async Task<LDeviceDto> CreateLDeviceAsync(int projectId, int userId, CreateLDeviceRequest req)
        {
            var entity = new LDevice { ServerId = req.ServerId, Inst = req.Inst, LdName = req.LdName, Desc = req.Desc };
            _db.LDevices.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LDevice", entity.Id, "Create", null, Ser(req));
            return MapLDevice(entity);
        }

        public async Task<LDeviceDto> UpdateLDeviceAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateLDeviceRequest> env)
        {
            var entity = await LoadAsync<LDevice>(_db.LDevices, id);
            await _conflict.CheckAsync(projectId, userId, "LDevice", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Inst = env.Payload.Inst; entity.LdName = env.Payload.LdName; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LDevice", id, "Update", old, Ser(entity), env.Comment);
            return MapLDevice(entity);
        }

        public async Task DeleteLDeviceAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<LDevice>(_db.LDevices, id);
            await _conflict.CheckAsync(projectId, userId, "LDevice", id, null, entity);
            _db.LDevices.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LDevice", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // LogicalNode
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<LogicalNodeDto>> GetLogicalNodesAsync(int projectId, int ldeviceId)
        {
            var items = await _db.LogicalNodes.Where(l => l.LDeviceId == ldeviceId).ToListAsync();
            return items.Select(MapLogicalNode);
        }

        public async Task<LogicalNodeDto> GetLogicalNodeAsync(int projectId, int id)
            => MapLogicalNode(await LoadAsync<LogicalNode>(_db.LogicalNodes, id));

        public async Task<LogicalNodeDto> CreateLogicalNodeAsync(int projectId, int userId, CreateLogicalNodeRequest req)
        {
            var entity = new LogicalNode
            {
                LDeviceId = req.LDeviceId, Prefix = req.Prefix,
                LnClass = req.LnClass, Inst = req.Inst, LnType = req.LnType, Desc = req.Desc
            };
            _db.LogicalNodes.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LogicalNode", entity.Id, "Create", null, Ser(req));
            return MapLogicalNode(entity);
        }

        public async Task<LogicalNodeDto> UpdateLogicalNodeAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateLogicalNodeRequest> env)
        {
            var entity = await LoadAsync<LogicalNode>(_db.LogicalNodes, id);
            await _conflict.CheckAsync(projectId, userId, "LogicalNode", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Prefix = env.Payload.Prefix; entity.LnClass = env.Payload.LnClass;
            entity.Inst = env.Payload.Inst; entity.LnType = env.Payload.LnType; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LogicalNode", id, "Update", old, Ser(entity), env.Comment);
            return MapLogicalNode(entity);
        }

        public async Task DeleteLogicalNodeAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<LogicalNode>(_db.LogicalNodes, id);
            await _conflict.CheckAsync(projectId, userId, "LogicalNode", id, null, entity);
            _db.LogicalNodes.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LogicalNode", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // ConductingEquipment
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<ConductingEquipmentDto>> GetConductingEquipmentsAsync(int projectId, int bayId)
        {
            var items = await _db.ConductingEquipments.Where(c => c.BayId == bayId).ToListAsync();
            return items.Select(MapConductingEquipment);
        }

        public async Task<ConductingEquipmentDto> GetConductingEquipmentAsync(int projectId, int id)
            => MapConductingEquipment(await LoadAsync<ConductingEquipment>(_db.ConductingEquipments, id));

        public async Task<ConductingEquipmentDto> CreateConductingEquipmentAsync(int projectId, int userId,
            CreateConductingEquipmentRequest req)
        {
            var entity = new ConductingEquipment
            {
                BayId = req.BayId, Name = req.Name, EquipmentType = req.EquipmentType,
                Virtual = req.Virtual, Desc = req.Desc
            };
            _db.ConductingEquipments.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConductingEquipment", entity.Id, "Create", null, Ser(req));
            return MapConductingEquipment(entity);
        }

        public async Task<ConductingEquipmentDto> UpdateConductingEquipmentAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateConductingEquipmentRequest> env)
        {
            var entity = await LoadAsync<ConductingEquipment>(_db.ConductingEquipments, id);
            await _conflict.CheckAsync(projectId, userId, "ConductingEquipment", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.EquipmentType = env.Payload.EquipmentType;
            entity.Virtual = env.Payload.Virtual; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConductingEquipment", id, "Update", old, Ser(entity), env.Comment);
            return MapConductingEquipment(entity);
        }

        public async Task DeleteConductingEquipmentAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<ConductingEquipment>(_db.ConductingEquipments, id);
            await _conflict.CheckAsync(projectId, userId, "ConductingEquipment", id, null, entity);
            _db.ConductingEquipments.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConductingEquipment", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // ConnectivityNode
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<ConnectivityNodeDto>> GetConnectivityNodesAsync(int projectId, int bayId)
        {
            var items = await _db.ConnectivityNodes.Where(c => c.BayId == bayId).ToListAsync();
            return items.Select(MapConnectivityNode);
        }

        public async Task<ConnectivityNodeDto> GetConnectivityNodeAsync(int projectId, int id)
            => MapConnectivityNode(await LoadAsync<ConnectivityNode>(_db.ConnectivityNodes, id));

        public async Task<ConnectivityNodeDto> CreateConnectivityNodeAsync(int projectId, int userId,
            CreateConnectivityNodeRequest req)
        {
            var entity = new ConnectivityNode
            {
                BayId = req.BayId, Name = req.Name, PathName = req.PathName, Desc = req.Desc
            };
            _db.ConnectivityNodes.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConnectivityNode", entity.Id, "Create", null, Ser(req));
            return MapConnectivityNode(entity);
        }

        public async Task<ConnectivityNodeDto> UpdateConnectivityNodeAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateConnectivityNodeRequest> env)
        {
            var entity = await LoadAsync<ConnectivityNode>(_db.ConnectivityNodes, id);
            await _conflict.CheckAsync(projectId, userId, "ConnectivityNode", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.PathName = env.Payload.PathName; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConnectivityNode", id, "Update", old, Ser(entity), env.Comment);
            return MapConnectivityNode(entity);
        }

        public async Task DeleteConnectivityNodeAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<ConnectivityNode>(_db.ConnectivityNodes, id);
            await _conflict.CheckAsync(projectId, userId, "ConnectivityNode", id, null, entity);
            _db.ConnectivityNodes.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConnectivityNode", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // PowerTransformer
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<PowerTransformerDto>> GetPowerTransformersAsync(int projectId)
        {
            var items = await _db.PowerTransformers.ToListAsync();
            return items.Select(MapPowerTransformer);
        }

        public async Task<PowerTransformerDto> GetPowerTransformerAsync(int projectId, int id)
            => MapPowerTransformer(await LoadAsync<PowerTransformer>(_db.PowerTransformers, id));

        public async Task<PowerTransformerDto> CreatePowerTransformerAsync(int projectId, int userId,
            CreatePowerTransformerRequest req)
        {
            var entity = new PowerTransformer
            {
                EquipmentContainerId = req.EquipmentContainerId, Name = req.Name,
                Virtual = req.Virtual, Desc = req.Desc
            };
            _db.PowerTransformers.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "PowerTransformer", entity.Id, "Create", null, Ser(req));
            return MapPowerTransformer(entity);
        }

        public async Task<PowerTransformerDto> UpdatePowerTransformerAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreatePowerTransformerRequest> env)
        {
            var entity = await LoadAsync<PowerTransformer>(_db.PowerTransformers, id);
            await _conflict.CheckAsync(projectId, userId, "PowerTransformer", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.Virtual = env.Payload.Virtual; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "PowerTransformer", id, "Update", old, Ser(entity), env.Comment);
            return MapPowerTransformer(entity);
        }

        public async Task DeletePowerTransformerAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<PowerTransformer>(_db.PowerTransformers, id);
            await _conflict.CheckAsync(projectId, userId, "PowerTransformer", id, null, entity);
            _db.PowerTransformers.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "PowerTransformer", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // DOI
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<DOIDto>> GetDOIsAsync(int projectId, int? logicalNodeId, int? ln0Id)
        {
            var q = _db.DOIs.AsQueryable();
            if (logicalNodeId.HasValue) q = q.Where(d => d.LogicalNodeId == logicalNodeId);
            if (ln0Id.HasValue)         q = q.Where(d => d.LN0Id == ln0Id);
            return (await q.ToListAsync()).Select(MapDOI);
        }

        public async Task<DOIDto> GetDOIAsync(int projectId, int id)
            => MapDOI(await LoadAsync<DOI>(_db.DOIs, id));

        public async Task<DOIDto> CreateDOIAsync(int projectId, int userId, CreateDOIRequest req)
        {
            var entity = new DOI
            {
                LogicalNodeId = req.LogicalNodeId, LN0Id = req.LN0Id, Name = req.Name,
                Ix = req.Ix, AccessControl = req.AccessControl, Desc = req.Desc
            };
            _db.DOIs.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DOI", entity.Id, "Create", null, Ser(req));
            return MapDOI(entity);
        }

        public async Task<DOIDto> UpdateDOIAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateDOIRequest> env)
        {
            var entity = await LoadAsync<DOI>(_db.DOIs, id);
            await _conflict.CheckAsync(projectId, userId, "DOI", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.Ix = env.Payload.Ix;
            entity.AccessControl = env.Payload.AccessControl; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DOI", id, "Update", old, Ser(entity), env.Comment);
            return MapDOI(entity);
        }

        public async Task DeleteDOIAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<DOI>(_db.DOIs, id);
            await _conflict.CheckAsync(projectId, userId, "DOI", id, null, entity);
            _db.DOIs.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DOI", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // DAI
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<DAIDto>> GetDAIsAsync(int projectId, int doiId)
        {
            var items = await _db.DAIs.Where(d => d.DOIId == doiId).ToListAsync();
            return items.Select(MapDAI);
        }

        public async Task<DAIDto> GetDAIAsync(int projectId, int id)
            => MapDAI(await LoadAsync<DAI>(_db.DAIs, id));

        public async Task<DAIDto> CreateDAIAsync(int projectId, int userId, CreateDAIRequest req)
        {
            var entity = new DAI
            {
                DOIId = req.DOIId, SDIId = req.SDIId, Name = req.Name,
                SAddr = req.SAddr, ValKind = req.ValKind, Ix = req.Ix,
                ValImport = req.ValImport, Desc = req.Desc
            };
            _db.DAIs.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DAI", entity.Id, "Create", null, Ser(req));
            return MapDAI(entity);
        }

        public async Task<DAIDto> UpdateDAIAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateDAIRequest> env)
        {
            var entity = await LoadAsync<DAI>(_db.DAIs, id);
            await _conflict.CheckAsync(projectId, userId, "DAI", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.SAddr = env.Payload.SAddr;
            entity.ValKind = env.Payload.ValKind; entity.Ix = env.Payload.Ix;
            entity.ValImport = env.Payload.ValImport; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DAI", id, "Update", old, Ser(entity), env.Comment);
            return MapDAI(entity);
        }

        public async Task DeleteDAIAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<DAI>(_db.DAIs, id);
            await _conflict.CheckAsync(projectId, userId, "DAI", id, null, entity);
            _db.DAIs.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DAI", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // DataSet
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<DataSetDto>> GetDataSetsAsync(int projectId)
        {
            var sclId = await GetSclIdAsync(projectId);
            var items = await _db.DataSets.ToListAsync();
            return items.Select(MapDataSet);
        }

        public async Task<DataSetDto> GetDataSetAsync(int projectId, int id)
            => MapDataSet(await LoadAsync<DataSet>(_db.DataSets, id));

        public async Task<DataSetDto> CreateDataSetAsync(int projectId, int userId, CreateDataSetRequest req)
        {
            var entity = new DataSet { LN0Id = req.LN0Id, LogicalNodeId = req.LogicalNodeId, Name = req.Name, Desc = req.Desc };
            _db.DataSets.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DataSet", entity.Id, "Create", null, Ser(req));
            return MapDataSet(entity);
        }

        public async Task<DataSetDto> UpdateDataSetAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateDataSetRequest> env)
        {
            var entity = await LoadAsync<DataSet>(_db.DataSets, id);
            await _conflict.CheckAsync(projectId, userId, "DataSet", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DataSet", id, "Update", old, Ser(entity), env.Comment);
            return MapDataSet(entity);
        }

        public async Task DeleteDataSetAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<DataSet>(_db.DataSets, id);
            await _conflict.CheckAsync(projectId, userId, "DataSet", id, null, entity);
            _db.DataSets.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DataSet", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // SubNetwork
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<SubNetworkDto>> GetSubNetworksAsync(int projectId)
        {
            var items = await _db.SubNetworks.ToListAsync();
            return items.Select(MapSubNetwork);
        }

        public async Task<SubNetworkDto> GetSubNetworkAsync(int projectId, int id)
            => MapSubNetwork(await LoadAsync<SubNetwork>(_db.SubNetworks, id));

        public async Task<SubNetworkDto> CreateSubNetworkAsync(int projectId, int userId, CreateSubNetworkRequest req)
        {
            var entity = new SubNetwork
            {
                CommunicationId = req.CommunicationId, Name = req.Name,
                NetworkType = req.NetworkType, Desc = req.Desc
            };
            _db.SubNetworks.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "SubNetwork", entity.Id, "Create", null, Ser(req));
            return MapSubNetwork(entity);
        }

        public async Task<SubNetworkDto> UpdateSubNetworkAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateSubNetworkRequest> env)
        {
            var entity = await LoadAsync<SubNetwork>(_db.SubNetworks, id);
            await _conflict.CheckAsync(projectId, userId, "SubNetwork", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.Name = env.Payload.Name; entity.NetworkType = env.Payload.NetworkType; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "SubNetwork", id, "Update", old, Ser(entity), env.Comment);
            return MapSubNetwork(entity);
        }

        public async Task DeleteSubNetworkAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<SubNetwork>(_db.SubNetworks, id);
            await _conflict.CheckAsync(projectId, userId, "SubNetwork", id, null, entity);
            _db.SubNetworks.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "SubNetwork", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // LNodeType
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<LNodeTypeDto>> GetLNodeTypesAsync(int projectId)
        {
            var items = await _db.LNodeTypes.ToListAsync();
            return items.Select(MapLNodeType);
        }

        public async Task<LNodeTypeDto> GetLNodeTypeAsync(int projectId, int id)
            => MapLNodeType(await LoadAsync<LNodeType>(_db.LNodeTypes, id));

        public async Task<LNodeTypeDto> CreateLNodeTypeAsync(int projectId, int userId, CreateLNodeTypeRequest req)
        {
            var entity = new LNodeType
            {
                DataTypeTemplatesId = req.DataTypeTemplatesId, SclId = req.SclId,
                LnClass = req.LnClass, IedType = req.IedType ?? string.Empty, Desc = req.Desc
            };
            _db.LNodeTypes.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LNodeType", entity.Id, "Create", null, Ser(req));
            return MapLNodeType(entity);
        }

        public async Task<LNodeTypeDto> UpdateLNodeTypeAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateLNodeTypeRequest> env)
        {
            var entity = await LoadAsync<LNodeType>(_db.LNodeTypes, id);
            await _conflict.CheckAsync(projectId, userId, "LNodeType", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.SclId = env.Payload.SclId; entity.LnClass = env.Payload.LnClass;
            entity.IedType = env.Payload.IedType ?? string.Empty; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LNodeType", id, "Update", old, Ser(entity), env.Comment);
            return MapLNodeType(entity);
        }

        public async Task DeleteLNodeTypeAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<LNodeType>(_db.LNodeTypes, id);
            await _conflict.CheckAsync(projectId, userId, "LNodeType", id, null, entity);
            _db.LNodeTypes.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "LNodeType", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // ConnectedAP
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<ConnectedAPDto>> GetConnectedAPsAsync(int projectId, int subNetworkId)
        {
            var items = await _db.ConnectedAPs.Where(c => c.SubNetworkId == subNetworkId).ToListAsync();
            return items.Select(MapConnectedAP);
        }

        public async Task<ConnectedAPDto> GetConnectedAPAsync(int projectId, int id)
            => MapConnectedAP(await LoadAsync<ConnectedAP>(_db.ConnectedAPs, id));

        public async Task<ConnectedAPDto> CreateConnectedAPAsync(int projectId, int userId, CreateConnectedAPRequest req)
        {
            var entity = new ConnectedAP
            {
                SubNetworkId = req.SubNetworkId, IedName = req.IedName,
                ApName = req.ApName, RedProt = req.RedProt, Desc = req.Desc
            };
            _db.ConnectedAPs.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConnectedAP", entity.Id, "Create", null, Ser(req));
            return MapConnectedAP(entity);
        }

        public async Task<ConnectedAPDto> UpdateConnectedAPAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateConnectedAPRequest> env)
        {
            var entity = await LoadAsync<ConnectedAP>(_db.ConnectedAPs, id);
            await _conflict.CheckAsync(projectId, userId, "ConnectedAP", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.IedName = env.Payload.IedName; entity.ApName = env.Payload.ApName;
            entity.RedProt = env.Payload.RedProt; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConnectedAP", id, "Update", old, Ser(entity), env.Comment);
            return MapConnectedAP(entity);
        }

        public async Task DeleteConnectedAPAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<ConnectedAP>(_db.ConnectedAPs, id);
            await _conflict.CheckAsync(projectId, userId, "ConnectedAP", id, null, entity);
            _db.ConnectedAPs.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ConnectedAP", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // DOType
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<DOTypeDto>> GetDOTypesAsync(int projectId)
        {
            var items = await _db.DOTypes.ToListAsync();
            return items.Select(MapDOType);
        }

        public async Task<DOTypeDto> GetDOTypeAsync(int projectId, int id)
            => MapDOType(await LoadAsync<DOType>(_db.DOTypes, id));

        public async Task<DOTypeDto> CreateDOTypeAsync(int projectId, int userId, CreateDOTypeRequest req)
        {
            var entity = new DOType
            {
                DataTypeTemplatesId = req.DataTypeTemplatesId, SclId = req.SclId,
                Cdc = req.Cdc, IedType = req.IedType ?? string.Empty, Desc = req.Desc
            };
            _db.DOTypes.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DOType", entity.Id, "Create", null, Ser(req));
            return MapDOType(entity);
        }

        public async Task<DOTypeDto> UpdateDOTypeAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateDOTypeRequest> env)
        {
            var entity = await LoadAsync<DOType>(_db.DOTypes, id);
            await _conflict.CheckAsync(projectId, userId, "DOType", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.SclId = env.Payload.SclId; entity.Cdc = env.Payload.Cdc;
            entity.IedType = env.Payload.IedType ?? string.Empty; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DOType", id, "Update", old, Ser(entity), env.Comment);
            return MapDOType(entity);
        }

        public async Task DeleteDOTypeAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<DOType>(_db.DOTypes, id);
            await _conflict.CheckAsync(projectId, userId, "DOType", id, null, entity);
            _db.DOTypes.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DOType", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // DAType
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<DATypeDto>> GetDATypesAsync(int projectId)
        {
            var items = await _db.DATypes.ToListAsync();
            return items.Select(MapDAType);
        }

        public async Task<DATypeDto> GetDATypeAsync(int projectId, int id)
            => MapDAType(await LoadAsync<DAType>(_db.DATypes, id));

        public async Task<DATypeDto> CreateDATypeAsync(int projectId, int userId, CreateDATypeRequest req)
        {
            var entity = new DAType
            {
                DataTypeTemplatesId = req.DataTypeTemplatesId, SclId = req.SclId,
                IedType = req.IedType ?? string.Empty, Desc = req.Desc
            };
            _db.DATypes.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DAType", entity.Id, "Create", null, Ser(req));
            return MapDAType(entity);
        }

        public async Task<DATypeDto> UpdateDATypeAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateDATypeRequest> env)
        {
            var entity = await LoadAsync<DAType>(_db.DATypes, id);
            await _conflict.CheckAsync(projectId, userId, "DAType", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.SclId = env.Payload.SclId;
            entity.IedType = env.Payload.IedType ?? string.Empty; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DAType", id, "Update", old, Ser(entity), env.Comment);
            return MapDAType(entity);
        }

        public async Task DeleteDATypeAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<DAType>(_db.DATypes, id);
            await _conflict.CheckAsync(projectId, userId, "DAType", id, null, entity);
            _db.DATypes.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "DAType", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // EnumType
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<EnumTypeDto>> GetEnumTypesAsync(int projectId)
        {
            var items = await _db.EnumTypes.ToListAsync();
            return items.Select(MapEnumType);
        }

        public async Task<EnumTypeDto> GetEnumTypeAsync(int projectId, int id)
            => MapEnumType(await LoadAsync<EnumType>(_db.EnumTypes, id));

        public async Task<EnumTypeDto> CreateEnumTypeAsync(int projectId, int userId, CreateEnumTypeRequest req)
        {
            var entity = new EnumType
            {
                DataTypeTemplatesId = req.DataTypeTemplatesId, SclId = req.SclId, Desc = req.Desc
            };
            _db.EnumTypes.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "EnumType", entity.Id, "Create", null, Ser(req));
            return MapEnumType(entity);
        }

        public async Task<EnumTypeDto> UpdateEnumTypeAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateEnumTypeRequest> env)
        {
            var entity = await LoadAsync<EnumType>(_db.EnumTypes, id);
            await _conflict.CheckAsync(projectId, userId, "EnumType", id, env.RowVersion, entity);
            var old = Ser(entity);
            entity.SclId = env.Payload.SclId; entity.Desc = env.Payload.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "EnumType", id, "Update", old, Ser(entity), env.Comment);
            return MapEnumType(entity);
        }

        public async Task DeleteEnumTypeAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<EnumType>(_db.EnumTypes, id);
            await _conflict.CheckAsync(projectId, userId, "EnumType", id, null, entity);
            _db.EnumTypes.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "EnumType", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // Private helpers
        // ????????????????????????????????????????????????????????????????????
        private async Task<int> GetSclIdAsync(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId)
                ?? throw new KeyNotFoundException($"Project {projectId} not found.");
            return project.SclId ?? throw new InvalidOperationException("Project has no SCL root.");
        }

        private async Task<Substation> LoadSubstationAsync(int projectId, int id)
        {
            var sclId = await GetSclIdAsync(projectId);
            return await _db.Substations.FirstOrDefaultAsync(s => s.Id == id && s.SclId == sclId)
                ?? throw new KeyNotFoundException($"Substation {id} not found in project {projectId}.");
        }

        private static async Task<T> LoadAsync<T>(IQueryable<T> set, int id) where T : SclBaseElement
        {
            var entity = await set.FirstOrDefaultAsync(e => e.Id == id);
            return entity ?? throw new KeyNotFoundException($"{typeof(T).Name} {id} not found.");
        }

        private static string Ser(object? obj)
            => obj == null ? "{}" : JsonSerializer.Serialize(obj);

        // ?? Mappers ??????????????????????????????????????????????????????????
        private static SubstationDto MapSubstation(Substation e) => new()
        {
            Id = e.Id, Name = e.Name, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static VoltageLevelDto MapVoltageLevel(VoltageLevel e) => new()
        {
            Id = e.Id, SubstationId = e.SubstationId ?? 0, Name = e.Name, Desc = e.Desc,
            NomFreq = e.NomFreq, NumPhases = e.NumPhases, VoltageValue = e.VoltageValue,
            VoltageMultiplier = e.VoltageMultiplier,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static BayDto MapBay(Bay e) => new()
        {
            Id = e.Id, VoltageLevelId = e.VoltageLevelId ?? 0, Name = e.Name, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static IEDDto MapIED(IED e) => new()
        {
            Id = e.Id, Name = e.Name, IedType = e.IedType,
            Manufacturer = e.Manufacturer, ConfigVersion = e.ConfigVersion, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static LDeviceDto MapLDevice(LDevice e) => new()
        {
            Id = e.Id, ServerId = e.ServerId, Inst = e.Inst, LdName = e.LdName, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static LogicalNodeDto MapLogicalNode(LogicalNode e) => new()
        {
            Id = e.Id, LDeviceId = e.LDeviceId, Prefix = e.Prefix,
            LnClass = e.LnClass, Inst = e.Inst, LnType = e.LnType, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static ConductingEquipmentDto MapConductingEquipment(ConductingEquipment e) => new()
        {
            Id = e.Id, BayId = e.BayId, Name = e.Name, EquipmentType = e.EquipmentType,
            Virtual = e.Virtual, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static ConnectivityNodeDto MapConnectivityNode(ConnectivityNode e) => new()
        {
            Id = e.Id, BayId = e.BayId, Name = e.Name, PathName = e.PathName, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static PowerTransformerDto MapPowerTransformer(PowerTransformer e) => new()
        {
            Id = e.Id, EquipmentContainerId = e.EquipmentContainerId, Name = e.Name,
            Virtual = e.Virtual, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static DOIDto MapDOI(DOI e) => new()
        {
            Id = e.Id, LogicalNodeId = e.LogicalNodeId, LN0Id = e.LN0Id, Name = e.Name,
            Ix = e.Ix, AccessControl = e.AccessControl, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static DAIDto MapDAI(DAI e) => new()
        {
            Id = e.Id, DOIId = e.DOIId, SDIId = e.SDIId, Name = e.Name,
            SAddr = e.SAddr, ValKind = e.ValKind, Ix = e.Ix, ValImport = e.ValImport, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static DataSetDto MapDataSet(DataSet e) => new()
        {
            Id = e.Id, Name = e.Name, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static SubNetworkDto MapSubNetwork(SubNetwork e) => new()
        {
            Id = e.Id, CommunicationId = e.CommunicationId, Name = e.Name,
            NetworkType = e.NetworkType, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static LNodeTypeDto MapLNodeType(LNodeType e) => new()
        {
            Id = e.Id, SclId = e.SclId, LnClass = e.LnClass, IedType = e.IedType, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static ConnectedAPDto MapConnectedAP(ConnectedAP e) => new()
        {
            Id = e.Id, SubNetworkId = e.SubNetworkId, IedName = e.IedName,
            ApName = e.ApName, RedProt = e.RedProt, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static DOTypeDto MapDOType(DOType e) => new()
        {
            Id = e.Id, SclId = e.SclId, Cdc = e.Cdc, IedType = e.IedType, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static DATypeDto MapDAType(DAType e) => new()
        {
            Id = e.Id, SclId = e.SclId, IedType = e.IedType, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static EnumTypeDto MapEnumType(EnumType e) => new()
        {
            Id = e.Id, SclId = e.SclId, Desc = e.Desc,
            RowVersion = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        // ????????????????????????????????????????????????????????????????????
        // ReportControl
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<ReportControlDto>> GetReportControlsAsync(int projectId, int? ln0Id, int? logicalNodeId)
        {
            var q = _db.ReportControls.AsQueryable();
            if (ln0Id.HasValue)          q = q.Where(r => r.LN0Id == ln0Id);
            if (logicalNodeId.HasValue)  q = q.Where(r => r.LogicalNodeId == logicalNodeId);
            return (await q.ToListAsync()).Select(MapReportControl);
        }

        public async Task<ReportControlDto> GetReportControlAsync(int projectId, int id)
            => MapReportControl(await LoadAsync<ReportControl>(_db.ReportControls, id));

        public async Task<ReportControlDto> CreateReportControlAsync(int projectId, int userId, CreateReportControlRequest req)
        {
            var entity = new ReportControl
            {
                LN0Id         = req.LN0Id,
                LogicalNodeId = req.LogicalNodeId,
                Name          = req.Name,
                DatSet        = req.DatSet,
                RptID         = req.RptID,
                ConfRev       = req.ConfRev,
                Buffered      = req.Buffered,
                BufTime       = req.BufTime,
                Indexed       = req.Indexed,
                IntgPd        = req.IntgPd,
                TrgOpsDchg    = req.TrgOpsDchg,
                TrgOpsQchg    = req.TrgOpsQchg,
                TrgOpsDupd    = req.TrgOpsDupd,
                TrgOpsPeriod  = req.TrgOpsPeriod,
                TrgOpsGi      = req.TrgOpsGi,
                OptSeqNum     = req.OptSeqNum,
                OptTimeStamp  = req.OptTimeStamp,
                OptDataSet    = req.OptDataSet,
                OptReasonCode = req.OptReasonCode,
                OptDataRef    = req.OptDataRef,
                OptEntryID    = req.OptEntryID,
                OptConfigRef  = req.OptConfigRef,
                OptBufOvfl    = req.OptBufOvfl,
                Desc          = req.Desc
            };
            _db.ReportControls.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ReportControl", entity.Id, "Create", null, Ser(req));
            return MapReportControl(entity);
        }

        public async Task<ReportControlDto> UpdateReportControlAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateReportControlRequest> env)
        {
            var entity = await LoadAsync<ReportControl>(_db.ReportControls, id);
            await _conflict.CheckAsync(projectId, userId, "ReportControl", id, env.RowVersion, entity);
            var old = Ser(entity);
            var req = env.Payload;
            entity.Name          = req.Name;
            entity.DatSet        = req.DatSet;
            entity.RptID         = req.RptID;
            entity.ConfRev       = req.ConfRev;
            entity.Buffered      = req.Buffered;
            entity.BufTime       = req.BufTime;
            entity.Indexed       = req.Indexed;
            entity.IntgPd        = req.IntgPd;
            entity.TrgOpsDchg    = req.TrgOpsDchg;
            entity.TrgOpsQchg    = req.TrgOpsQchg;
            entity.TrgOpsDupd    = req.TrgOpsDupd;
            entity.TrgOpsPeriod  = req.TrgOpsPeriod;
            entity.TrgOpsGi      = req.TrgOpsGi;
            entity.OptSeqNum     = req.OptSeqNum;
            entity.OptTimeStamp  = req.OptTimeStamp;
            entity.OptDataSet    = req.OptDataSet;
            entity.OptReasonCode = req.OptReasonCode;
            entity.OptDataRef    = req.OptDataRef;
            entity.OptEntryID    = req.OptEntryID;
            entity.OptConfigRef  = req.OptConfigRef;
            entity.OptBufOvfl    = req.OptBufOvfl;
            entity.Desc          = req.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ReportControl", id, "Update", old, Ser(entity), env.Comment);
            return MapReportControl(entity);
        }

        public async Task DeleteReportControlAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<ReportControl>(_db.ReportControls, id);
            await _conflict.CheckAsync(projectId, userId, "ReportControl", id, null, entity);
            _db.ReportControls.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "ReportControl", id, "Delete", Ser(entity), null);
        }

        // ????????????????????????????????????????????????????????????????????
        // GSEControl (GOOSE Control Block)
        // ????????????????????????????????????????????????????????????????????
        public async Task<IEnumerable<GSEControlDto>> GetGSEControlsAsync(int projectId, int ln0Id)
        {
            var items = await _db.GSEControls.Where(g => g.LN0Id == ln0Id).ToListAsync();
            return items.Select(MapGSEControl);
        }

        public async Task<GSEControlDto> GetGSEControlAsync(int projectId, int id)
            => MapGSEControl(await LoadAsync<GSEControl>(_db.GSEControls, id));

        public async Task<GSEControlDto> CreateGSEControlAsync(int projectId, int userId, CreateGSEControlRequest req)
        {
            var entity = new GSEControl
            {
                LN0Id          = req.LN0Id,
                Name           = req.Name,
                DatSet         = req.DatSet,
                GseType        = req.GseType,
                AppID          = req.AppID,
                FixedOffs      = req.FixedOffs,
                SecurityEnable = req.SecurityEnable,
                Protocol       = req.Protocol,
                ConfRev        = req.ConfRev,
                Desc           = req.Desc
            };
            _db.GSEControls.Add(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "GSEControl", entity.Id, "Create", null, Ser(req));
            return MapGSEControl(entity);
        }

        public async Task<GSEControlDto> UpdateGSEControlAsync(int projectId, int userId, int id,
            SclUpdateEnvelope<CreateGSEControlRequest> env)
        {
            var entity = await LoadAsync<GSEControl>(_db.GSEControls, id);
            await _conflict.CheckAsync(projectId, userId, "GSEControl", id, env.RowVersion, entity);
            var old = Ser(entity);
            var req = env.Payload;
            entity.Name           = req.Name;
            entity.DatSet         = req.DatSet;
            entity.GseType        = req.GseType;
            entity.AppID          = req.AppID;
            entity.FixedOffs      = req.FixedOffs;
            entity.SecurityEnable = req.SecurityEnable;
            entity.Protocol       = req.Protocol;
            entity.ConfRev        = req.ConfRev;
            entity.Desc           = req.Desc;
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "GSEControl", id, "Update", old, Ser(entity), env.Comment);
            return MapGSEControl(entity);
        }

        public async Task DeleteGSEControlAsync(int projectId, int userId, int id)
        {
            var entity = await LoadAsync<GSEControl>(_db.GSEControls, id);
            await _conflict.CheckAsync(projectId, userId, "GSEControl", id, null, entity);
            _db.GSEControls.Remove(entity);
            await _db.SaveChangesAsync();
            await _audit.LogAsync(projectId, userId, "GSEControl", id, "Delete", Ser(entity), null);
        }

        // ?? Mappers (new) ???????????????????????????????????????????????????
        private static ReportControlDto MapReportControl(ReportControl e) => new()
        {
            Id             = e.Id,
            LN0Id          = e.LN0Id,
            LogicalNodeId  = e.LogicalNodeId,
            Name           = e.Name,
            DatSet         = e.DatSet,
            RptID          = e.RptID,
            ConfRev        = e.ConfRev,
            Buffered       = e.Buffered,
            BufTime        = e.BufTime,
            Indexed        = e.Indexed,
            IntgPd         = e.IntgPd,
            TrgOpsDchg     = e.TrgOpsDchg,
            TrgOpsQchg     = e.TrgOpsQchg,
            TrgOpsDupd     = e.TrgOpsDupd,
            TrgOpsPeriod   = e.TrgOpsPeriod,
            TrgOpsGi       = e.TrgOpsGi,
            OptSeqNum      = e.OptSeqNum,
            OptTimeStamp   = e.OptTimeStamp,
            OptDataSet     = e.OptDataSet,
            OptReasonCode  = e.OptReasonCode,
            OptDataRef     = e.OptDataRef,
            OptEntryID     = e.OptEntryID,
            OptConfigRef   = e.OptConfigRef,
            OptBufOvfl     = e.OptBufOvfl,
            Desc           = e.Desc,
            RowVersion     = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };

        private static GSEControlDto MapGSEControl(GSEControl e) => new()
        {
            Id             = e.Id,
            LN0Id          = e.LN0Id,
            Name           = e.Name,
            DatSet         = e.DatSet,
            GseType        = e.GseType,
            AppID          = e.AppID,
            FixedOffs      = e.FixedOffs,
            SecurityEnable = e.SecurityEnable,
            Protocol       = e.Protocol,
            ConfRev        = e.ConfRev,
            Desc           = e.Desc,
            RowVersion     = e.RowVersion != null ? Convert.ToBase64String(e.RowVersion) : string.Empty
        };
    }
}
