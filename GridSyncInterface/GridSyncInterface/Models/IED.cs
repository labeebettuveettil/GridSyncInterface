using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GridSyncInterface.Models.Base;
using GridSyncInterface.Models.Enums;

namespace GridSyncInterface.Models
{
    // ?? IED hierarchy ??????????????????????????????????????????????????????

    /// <summary>tIED – Intelligent Electronic Device</summary>
    public class IED : SclUnNaming
    {
        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        [Required, MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? IedType { get; set; }

        [MaxLength(255)]
        public string? Manufacturer { get; set; }

        [MaxLength(255)]
        public string? ConfigVersion { get; set; }

        [MaxLength(10)]
        public string? OriginalSclVersion { get; set; }

        [MaxLength(1)]
        public string? OriginalSclRevision { get; set; }

        public byte OriginalSclRelease { get; set; } = 1;

        [MaxLength(20)]
        public string EngRight { get; set; } = "full";

        [MaxLength(255)]
        public string? Owner { get; set; }

        public IedServices? Services { get; set; }
        public ICollection<AccessPoint> AccessPoints { get; set; } = new List<AccessPoint>();
        public ICollection<KDC> KDCs { get; set; } = new List<KDC>();
    }

    /// <summary>tKDC</summary>
    public class KDC
    {
        [Key]
        public int Id { get; set; }

        public int IedId { get; set; }
        public IED? Ied { get; set; }

        [Required, MaxLength(64)]
        public string IedName { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        public string ApName { get; set; } = string.Empty;
    }

    /// <summary>tServices – IED capability services</summary>
    public class IedServices
    {
        [Key]
        public int Id { get; set; }

        public int IedId { get; set; }
        public IED? Ied { get; set; }

        public int NameLength { get; set; } = 32;

        // Service flags stored as flat columns for performance
        public bool? DynAssociation { get; set; }
        public uint? DynAssociationMax { get; set; }

        public bool? GetDirectory { get; set; }
        public bool? GetDataObjectDefinition { get; set; }
        public bool? DataObjectDirectory { get; set; }
        public bool? GetDataSetValue { get; set; }
        public bool? SetDataSetValue { get; set; }
        public bool? DataSetDirectory { get; set; }
        public bool? ReadWrite { get; set; }
        public bool? TimerActivatedControl { get; set; }
        public bool? GetCBValues { get; set; }
        public bool? GSEDir { get; set; }
        public bool? ConfLdName { get; set; }

        // ConfDataSet
        public uint? ConfDataSetMax { get; set; }
        public uint? ConfDataSetMaxAttributes { get; set; }
        public bool ConfDataSetModify { get; set; } = true;

        // DynDataSet
        public uint? DynDataSetMax { get; set; }
        public uint? DynDataSetMaxAttributes { get; set; }

        // ConfReportControl
        public uint? ConfReportControlMax { get; set; }
        public string ConfReportControlBufMode { get; set; } = "both";
        public bool ConfReportControlBufConf { get; set; }
        public uint? ConfReportControlMaxBuf { get; set; }

        // ConfLogControl
        public uint? ConfLogControlMax { get; set; }

        // ReportSettings
        public string ReportSettingsCbName { get; set; } = "Fix";
        public string ReportSettingsDatSet { get; set; } = "Fix";
        public string ReportSettingsRptID { get; set; } = "Fix";
        public string ReportSettingsOptFields { get; set; } = "Fix";
        public string ReportSettingsBufTime { get; set; } = "Fix";
        public string ReportSettingsTrgOps { get; set; } = "Fix";
        public string ReportSettingsIntgPd { get; set; } = "Fix";
        public bool ReportSettingsResvTms { get; set; }
        public bool ReportSettingsOwner { get; set; }

        // LogSettings
        public string LogSettingsCbName { get; set; } = "Fix";
        public string LogSettingsDatSet { get; set; } = "Fix";
        public string LogSettingsLogEna { get; set; } = "Fix";
        public string LogSettingsTrgOps { get; set; } = "Fix";
        public string LogSettingsIntgPd { get; set; } = "Fix";

        // GSESettings
        public string GSESettingsCbName { get; set; } = "Fix";
        public string GSESettingsDatSet { get; set; } = "Fix";
        public string GSESettingsAppID { get; set; } = "Fix";
        public string GSESettingsDataLabel { get; set; } = "Fix";
        public bool GSESettingsKdaParticipant { get; set; }
        public bool GSESettingsMcSecuritySignature { get; set; }
        public bool GSESettingsMcSecurityEncryption { get; set; }

        // SMVSettings
        public string SMVSettingsCbName { get; set; } = "Fix";
        public string SMVSettingsDatSet { get; set; } = "Fix";
        public string SMVSettingsSvID { get; set; } = "Fix";
        public string SMVSettingsOptFields { get; set; } = "Fix";
        public string SMVSettingsSmpRate { get; set; } = "Fix";
        public bool SMVSettingsSamplesPerSec { get; set; }
        public bool SMVSettingsPdcTimeStamp { get; set; }
        public bool SMVSettingsSynchSrcId { get; set; }
        public string SMVSettingsNofASDU { get; set; } = "Fix";
        public bool SMVSettingsKdaParticipant { get; set; }
        public bool SMVSettingsMcSecuritySignature { get; set; }
        public bool SMVSettingsMcSecurityEncryption { get; set; }

        // GOOSE
        public uint? GOOSEMax { get; set; }
        public bool GOOSEFixedOffs { get; set; }
        public bool GOOSEGoose { get; set; } = true;
        public bool GOOSE_rGOOSE { get; set; }

        // GSSE
        public uint? GSSEMax { get; set; }

        // SMVsc
        public uint? SMVscMax { get; set; }
        public string SMVscDelivery { get; set; } = "multicast";
        public bool SMVscDeliveryConf { get; set; }
        public bool SMVscSv { get; set; } = true;
        public bool SMVsc_rSV { get; set; }

        // FileHandling
        public bool? FileHandling { get; set; }
        public bool FileHandlingMms { get; set; } = true;
        public bool FileHandlingFtp { get; set; }
        public bool FileHandlingFtps { get; set; }

        // ConfLNs
        public bool ConfLNsFixPrefix { get; set; }
        public bool ConfLNsFixLnInst { get; set; }

        // ClientServices
        public bool? ClientServicesGoose { get; set; }
        public bool? ClientServicesGsse { get; set; }
        public bool? ClientServicesBufReport { get; set; }
        public bool? ClientServicesUnbufReport { get; set; }
        public bool? ClientServicesReadLog { get; set; }
        public bool? ClientServicesSv { get; set; }
        public bool? ClientServicesSupportsLdName { get; set; }
        public uint? ClientServicesMaxAttributes { get; set; }
        public uint? ClientServicesMaxReports { get; set; }
        public uint? ClientServicesMaxGOOSE { get; set; }
        public uint? ClientServicesMaxSMV { get; set; }
        public bool? ClientServices_rGOOSE { get; set; }
        public bool? ClientServices_rSV { get; set; }
        public bool? ClientServicesNoIctBinding { get; set; }

        // SupSubscription
        public uint? SupSubscriptionMaxGo { get; set; }
        public uint? SupSubscriptionMaxSv { get; set; }

        // RedProt
        public bool? RedProtHsr { get; set; }
        public bool? RedProtPrp { get; set; }
        public bool? RedProtRstp { get; set; }

        // TimeSyncProt
        public bool? TimeSyncProtSntp { get; set; }
        public bool? TimeSyncProt_iec61850_9_3 { get; set; }
        public bool? TimeSyncProt_c37_238 { get; set; }
        public bool? TimeSyncProtOther { get; set; }

        // CommProt
        public bool? CommProtIpv6 { get; set; }

        // ValueHandling
        public bool? ValueHandlingSetToRO { get; set; }

        // ConfSigRef
        public uint? ConfSigRefMax { get; set; }

        // SettingGroups
        public bool? SettingGroupsSGEdit { get; set; }
        public bool? SettingGroupsSGEditResvTms { get; set; }
        public bool? SettingGroupsConfSG { get; set; }
        public bool? SettingGroupsConfSGResvTms { get; set; }
    }

    /// <summary>tAccessPoint</summary>
    public class AccessPoint : SclUnNaming
    {
        public int IedId { get; set; }
        public IED? Ied { get; set; }

        [Required, MaxLength(32)]
        public string Name { get; set; } = string.Empty;

        public bool Router { get; set; } = false;
        public bool Clock { get; set; } = false;
        public bool Kdc { get; set; } = false;

        public Server? Server { get; set; }
        public ServerAt? ServerAt { get; set; }
        public IedServices? AccessPointServices { get; set; }
        public ICollection<LogicalNode> DirectLNs { get; set; } = new List<LogicalNode>();
        public ICollection<Certificate> GOOSESecurities { get; set; } = new List<Certificate>();
        public ICollection<Certificate> SMVSecurities { get; set; } = new List<Certificate>();
    }

    /// <summary>tServerAt</summary>
    public class ServerAt : SclUnNaming
    {
        public int AccessPointId { get; set; }
        public AccessPoint? AccessPoint { get; set; }

        [Required, MaxLength(32)]
        public string ApName { get; set; } = string.Empty;
    }

    /// <summary>tCertificate</summary>
    public class Certificate : SclNaming
    {
        public int? AccessPointGooseId { get; set; }
        public AccessPoint? AccessPointGoose { get; set; }

        public int? AccessPointSmvId { get; set; }
        public AccessPoint? AccessPointSmv { get; set; }

        public uint? XferNumber { get; set; }

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        // Subject
        [Required, MaxLength(255)]
        public string SubjectCommonName { get; set; } = string.Empty;
        [Required, MaxLength(255)]
        public string SubjectIdHierarchy { get; set; } = string.Empty;

        // IssuerName
        [Required, MaxLength(255)]
        public string IssuerCommonName { get; set; } = string.Empty;
        [Required, MaxLength(255)]
        public string IssuerIdHierarchy { get; set; } = string.Empty;
    }

    /// <summary>tServer</summary>
    public class Server : SclUnNaming
    {
        public int AccessPointId { get; set; }
        public AccessPoint? AccessPoint { get; set; }

        public uint Timeout { get; set; } = 30;

        // Authentication flags
        public bool AuthNone { get; set; } = true;
        public bool AuthPassword { get; set; }
        public bool AuthWeak { get; set; }
        public bool AuthStrong { get; set; }
        public bool AuthCertificate { get; set; }

        public ICollection<LDevice> LDevices { get; set; } = new List<LDevice>();
        public ICollection<Association> Associations { get; set; } = new List<Association>();
    }

    /// <summary>tLDevice – Logical Device</summary>
    public class LDevice : SclUnNaming
    {
        public int ServerId { get; set; }
        public Server? Server { get; set; }

        [Required, MaxLength(64)]
        public string Inst { get; set; } = string.Empty;

        [MaxLength(64)]
        public string? LdName { get; set; }

        public LN0? LN0 { get; set; }
        public ICollection<LogicalNode> LNs { get; set; } = new List<LogicalNode>();
        public string? AccessControl { get; set; }
    }

    /// <summary>tAssociation</summary>
    public class Association
    {
        [Key]
        public int Id { get; set; }

        public int ServerId { get; set; }
        public Server? Server { get; set; }

        [MaxLength(64)]
        public string? AssociationDesc { get; set; }

        [Required, MaxLength(64)]
        public string IedName { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        public string LdInst { get; set; } = string.Empty;

        [MaxLength(11)]
        public string Prefix { get; set; } = string.Empty;

        [Required, MaxLength(4)]
        public string LnClass { get; set; } = string.Empty;

        [Required, MaxLength(12)]
        public string LnInst { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Kind { get; set; } = string.Empty;   // AssociationKind

        [MaxLength(255)]
        public string? AssociationID { get; set; }
    }

    // ?? Logical Node base and subtypes ??????????????????????????????????????

    /// <summary>tAnyLN (abstract base for LN0 and LN)</summary>
    public abstract class AnyLogicalNode : SclUnNaming
    {
        [Required, MaxLength(255)]
        public string LnType { get; set; } = string.Empty;
    }

    /// <summary>tLN0 – Logical Node Zero</summary>
    public class LN0 : AnyLogicalNode
    {
        public int LDeviceId { get; set; }
        public LDevice? LDevice { get; set; }

        public string LnClass { get; set; } = "LLN0";
        public string Inst { get; set; } = string.Empty;

        public ICollection<DataSet> DataSets { get; set; } = new List<DataSet>();
        public ICollection<ReportControl> ReportControls { get; set; } = new List<ReportControl>();
        public ICollection<LogControl> LogControls { get; set; } = new List<LogControl>();
        public ICollection<DOI> DOIs { get; set; } = new List<DOI>();
        public Inputs? Inputs { get; set; }
        public ICollection<SclLog> Logs { get; set; } = new List<SclLog>();

        public ICollection<GSEControl> GSEControls { get; set; } = new List<GSEControl>();
        public ICollection<SampledValueControl> SampledValueControls { get; set; } = new List<SampledValueControl>();
        public SettingControl? SettingControl { get; set; }
    }

    /// <summary>tLN – generic Logical Node</summary>
    public class LogicalNode : AnyLogicalNode
    {
        public int? LDeviceId { get; set; }
        public LDevice? LDevice { get; set; }

        // For LNs directly under AccessPoint
        public int? AccessPointId { get; set; }
        public AccessPoint? AccessPoint { get; set; }

        [MaxLength(11)]
        public string Prefix { get; set; } = string.Empty;

        [Required, MaxLength(4)]
        public string LnClass { get; set; } = string.Empty;

        [Required, MaxLength(12)]
        public string Inst { get; set; } = string.Empty;

        public ICollection<DataSet> DataSets { get; set; } = new List<DataSet>();
        public ICollection<ReportControl> ReportControls { get; set; } = new List<ReportControl>();
        public ICollection<LogControl> LogControls { get; set; } = new List<LogControl>();
        public ICollection<DOI> DOIs { get; set; } = new List<DOI>();
        public Inputs? Inputs { get; set; }
        public ICollection<SclLog> Logs { get; set; } = new List<SclLog>();
    }

    // ?? Dataset & controls ??????????????????????????????????????????????????

    /// <summary>tDataSet</summary>
    public class DataSet : SclUnNaming
    {
        public int? LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public int? LogicalNodeId { get; set; }
        public LogicalNode? LogicalNode { get; set; }

        [Required, MaxLength(32)]
        public string Name { get; set; } = string.Empty;

        public ICollection<FCDA> FCDAs { get; set; } = new List<FCDA>();
    }

    /// <summary>tFCDA – Functional Constrained Data Attribute</summary>
    public class FCDA
    {
        [Key]
        public int Id { get; set; }

        public int DataSetId { get; set; }
        public DataSet? DataSet { get; set; }

        [MaxLength(64)]
        public string? LdInst { get; set; }

        [MaxLength(11)]
        public string Prefix { get; set; } = string.Empty;

        [MaxLength(4)]
        public string? LnClass { get; set; }

        [MaxLength(12)]
        public string? LnInst { get; set; }

        [MaxLength(50)]
        public string? DoName { get; set; }

        [MaxLength(255)]
        public string? DaName { get; set; }

        [Required, MaxLength(5)]
        public string Fc { get; set; } = string.Empty;

        public uint? Ix { get; set; }
    }

    /// <summary>tControl (abstract)</summary>
    public abstract class Control : SclUnNaming
    {
        [Required, MaxLength(32)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? DatSet { get; set; }
    }

    /// <summary>tControlWithTriggerOpt (abstract)</summary>
    public abstract class ControlWithTriggerOpt : Control
    {
        public uint IntgPd { get; set; } = 0;

        // TrgOps
        public bool TrgOpsDchg { get; set; }
        public bool TrgOpsQchg { get; set; }
        public bool TrgOpsDupd { get; set; }
        public bool TrgOpsPeriod { get; set; }
        public bool TrgOpsGi { get; set; } = true;
    }

    /// <summary>tReportControl</summary>
    public class ReportControl : ControlWithTriggerOpt
    {
        public int? LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public int? LogicalNodeId { get; set; }
        public LogicalNode? LogicalNode { get; set; }

        [MaxLength(129)]
        public string? RptID { get; set; }

        public uint ConfRev { get; set; }
        public bool Buffered { get; set; }
        public uint BufTime { get; set; }
        public bool Indexed { get; set; } = true;

        // OptFields
        public bool OptSeqNum { get; set; }
        public bool OptTimeStamp { get; set; }
        public bool OptDataSet { get; set; }
        public bool OptReasonCode { get; set; }
        public bool OptDataRef { get; set; }
        public bool OptEntryID { get; set; }
        public bool OptConfigRef { get; set; }
        public bool OptBufOvfl { get; set; } = true;

        public RptEnabled? RptEnabled { get; set; }
    }

    /// <summary>tRptEnabled</summary>
    public class RptEnabled : SclUnNaming
    {
        public int ReportControlId { get; set; }
        public ReportControl? ReportControl { get; set; }

        public uint Max { get; set; } = 1;

        public ICollection<ClientLN> ClientLNs { get; set; } = new List<ClientLN>();
    }

    /// <summary>tClientLN</summary>
    public class ClientLN
    {
        [Key]
        public int Id { get; set; }

        public int RptEnabledId { get; set; }
        public RptEnabled? RptEnabled { get; set; }

        [MaxLength(255)]
        public string? ClientDesc { get; set; }

        [Required, MaxLength(64)]
        public string IedName { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        public string LdInst { get; set; } = string.Empty;

        [MaxLength(11)]
        public string Prefix { get; set; } = string.Empty;

        [Required, MaxLength(4)]
        public string LnClass { get; set; } = string.Empty;

        [Required, MaxLength(12)]
        public string LnInst { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? ApRef { get; set; }
    }

    /// <summary>tLogControl</summary>
    public class LogControl : ControlWithTriggerOpt
    {
        public int? LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public int? LogicalNodeId { get; set; }
        public LogicalNode? LogicalNode { get; set; }

        [MaxLength(64)]
        public string? LdInst { get; set; }

        [MaxLength(11)]
        public string Prefix { get; set; } = string.Empty;

        [MaxLength(4)]
        public string LnClass { get; set; } = "LLN0";

        [MaxLength(12)]
        public string? LnInst { get; set; }

        [Required, MaxLength(32)]
        public string LogName { get; set; } = string.Empty;

        public bool LogEna { get; set; } = true;
        public bool ReasonCode { get; set; } = true;
        public uint BufTime { get; set; }
    }

    /// <summary>tControlWithIEDName (abstract) – base for GSEControl & SampledValueControl</summary>
    public abstract class ControlWithIEDName : Control
    {
        public uint? ConfRev { get; set; }
        public ICollection<ControlIEDName> IEDNames { get; set; } = new List<ControlIEDName>();
    }

    /// <summary>IEDName child of ControlWithIEDName</summary>
    public class ControlIEDName
    {
        [Key]
        public int Id { get; set; }

        public int? GSEControlId { get; set; }
        public GSEControl? GSEControl { get; set; }

        public int? SVControlId { get; set; }
        public SampledValueControl? SVControl { get; set; }

        [Required, MaxLength(64)]
        public string IedName { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? ApRef { get; set; }

        [MaxLength(64)]
        public string? LdInst { get; set; }

        [MaxLength(11)]
        public string? Prefix { get; set; }

        [MaxLength(4)]
        public string? LnClass { get; set; }

        [MaxLength(12)]
        public string? LnInst { get; set; }
    }

    /// <summary>tGSEControl</summary>
    public class GSEControl : ControlWithIEDName
    {
        public int LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        [MaxLength(10)]
        public string GseType { get; set; } = "GOOSE";

        [Required, MaxLength(129)]
        public string AppID { get; set; } = string.Empty;

        public bool FixedOffs { get; set; }

        [MaxLength(40)]
        public string SecurityEnable { get; set; } = "None";

        // Protocol (R-GOOSE)
        public string? Protocol { get; set; }
    }

    /// <summary>tSampledValueControl</summary>
    public class SampledValueControl : ControlWithIEDName
    {
        public int LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        [Required, MaxLength(129)]
        public string SmvID { get; set; } = string.Empty;

        public bool Multicast { get; set; } = true;
        public uint SmpRate { get; set; }
        public uint NofASDU { get; set; }

        [MaxLength(20)]
        public string SmpMod { get; set; } = "SmpPerPeriod";

        [MaxLength(40)]
        public string SecurityEnable { get; set; } = "None";

        // SmvOpts
        public bool SmvOptRefreshTime { get; set; }
        public bool SmvOptSampleSynchronized { get; set; } = true;
        public bool SmvOptSampleRate { get; set; }
        public bool SmvOptDataSet { get; set; }
        public bool SmvOptSecurity { get; set; }
        public bool SmvOptTimestamp { get; set; }
        public bool SmvOptSynchSourceId { get; set; }

        // Protocol (R-SV)
        public string? Protocol { get; set; }
    }

    /// <summary>tSettingControl</summary>
    public class SettingControl : SclUnNaming
    {
        public int LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public uint NumOfSGs { get; set; } = 1;
        public uint ActSG { get; set; } = 1;
        public ushort? ResvTms { get; set; }
    }

    /// <summary>tInputs</summary>
    public class Inputs : SclUnNaming
    {
        public int? LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public int? LogicalNodeId { get; set; }
        public LogicalNode? LogicalNode { get; set; }

        public ICollection<ExtRef> ExtRefs { get; set; } = new List<ExtRef>();
    }

    /// <summary>tExtRef – External Reference</summary>
    public class ExtRef
    {
        [Key]
        public int Id { get; set; }

        public int InputsId { get; set; }
        public Inputs? Inputs { get; set; }

        [MaxLength(255)]
        public string? ExtRefDesc { get; set; }

        [MaxLength(64)]
        public string? IedName { get; set; }

        [MaxLength(64)]
        public string? LdInst { get; set; }

        [MaxLength(11)]
        public string? Prefix { get; set; }

        [MaxLength(4)]
        public string? LnClass { get; set; }

        [MaxLength(12)]
        public string? LnInst { get; set; }

        [MaxLength(50)]
        public string? DoName { get; set; }

        [MaxLength(255)]
        public string? DaName { get; set; }

        [MaxLength(255)]
        public string? IntAddr { get; set; }

        [MaxLength(10)]
        public string? ServiceType { get; set; }

        [MaxLength(64)]
        public string? SrcLDInst { get; set; }

        [MaxLength(11)]
        public string? SrcPrefix { get; set; }

        [MaxLength(4)]
        public string? SrcLNClass { get; set; }

        [MaxLength(12)]
        public string? SrcLNInst { get; set; }

        [MaxLength(32)]
        public string? SrcCBName { get; set; }

        [MaxLength(10)]
        public string? PServT { get; set; }

        [MaxLength(4)]
        public string? PLN { get; set; }

        [MaxLength(50)]
        public string? PDO { get; set; }

        [MaxLength(255)]
        public string? PDA { get; set; }
    }

    /// <summary>tLog</summary>
    public class SclLog : SclUnNaming
    {
        public int? LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public int? LogicalNodeId { get; set; }
        public LogicalNode? LogicalNode { get; set; }

        [MaxLength(32)]
        public string? LogName { get; set; }
    }

    // ?? DOI / SDI / DAI ?????????????????????????????????????????????????????

    /// <summary>tDOI – Data Object Instance</summary>
    public class DOI : SclUnNaming
    {
        public int? LN0Id { get; set; }
        public LN0? LN0 { get; set; }

        public int? LogicalNodeId { get; set; }
        public LogicalNode? LogicalNode { get; set; }

        [Required, MaxLength(12)]
        public string Name { get; set; } = string.Empty;

        public uint? Ix { get; set; }

        [MaxLength(255)]
        public string? AccessControl { get; set; }

        public ICollection<SDI> SDIs { get; set; } = new List<SDI>();
        public ICollection<DAI> DAIs { get; set; } = new List<DAI>();
    }

    /// <summary>tSDI – Sub Data Instance</summary>
    public class SDI : SclUnNaming
    {
        public int? DOIId { get; set; }
        public DOI? DOI { get; set; }

        public int? SDIParentId { get; set; }
        public SDI? SDIParent { get; set; }

        [Required, MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        public uint? Ix { get; set; }

        [MaxLength(255)]
        public string? SAddr { get; set; }

        public ICollection<SDI> ChildSDIs { get; set; } = new List<SDI>();
        public ICollection<DAI> DAIs { get; set; } = new List<DAI>();
    }

    /// <summary>tDAI – Data Attribute Instance</summary>
    public class DAI : SclUnNaming
    {
        public int? DOIId { get; set; }
        public DOI? DOI { get; set; }

        public int? SDIId { get; set; }
        public SDI? SDI { get; set; }

        [Required, MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? SAddr { get; set; }

        [MaxLength(10)]
        public string? ValKind { get; set; }

        public uint? Ix { get; set; }
        public bool? ValImport { get; set; }

        public ICollection<Val> Vals { get; set; } = new List<Val>();
    }

    /// <summary>tVal</summary>
    public class Val
    {
        [Key]
        public int Id { get; set; }

        public int? DAIId { get; set; }
        public DAI? DAI { get; set; }

        public int? DataAttributeId { get; set; }
        public DataAttribute? DataAttribute { get; set; }

        public int? BDAId { get; set; }
        public BDA? BDA { get; set; }

        public uint? SGroup { get; set; }

        [MaxLength(1000)]
        public string? Value { get; set; }
    }
}
