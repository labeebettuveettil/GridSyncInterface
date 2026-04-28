using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GridSyncInterface.Models.Base;
using GridSyncInterface.Models.Enums;

namespace GridSyncInterface.Models
{
    // ?? Substation hierarchy ?????????????????????????????????????????????????

    /// <summary>tLNode – logical node reference used in substation topology</summary>
    public class LNode : SclUnNaming
    {
        [MaxLength(64)]
        public string? IedName { get; set; }

        [MaxLength(64)]
        public string? LdInst { get; set; }

        [MaxLength(11)]
        public string? Prefix { get; set; }

        [Required, MaxLength(4)]
        public string LnClass { get; set; } = string.Empty;

        [MaxLength(12)]
        public string? LnInst { get; set; }

        [MaxLength(255)]
        public string? LnType { get; set; }

        // Discriminator FK – one of the following will be set
        public int? LNodeContainerId { get; set; }
        public LNodeContainer? LNodeContainer { get; set; }
    }

    /// <summary>tLNodeContainer (abstract) – base for everything that holds LNodes.</summary>
    public abstract class LNodeContainer : SclNaming
    {
        public ICollection<LNode> LNodes { get; set; } = new List<LNode>();
    }

    /// <summary>tPowerSystemResource (abstract)</summary>
    public abstract class PowerSystemResource : LNodeContainer { }

    /// <summary>tEquipmentContainer (abstract)</summary>
    public abstract class EquipmentContainer : PowerSystemResource
    {
        public ICollection<PowerTransformer> PowerTransformers { get; set; } = new List<PowerTransformer>();
        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
    }

    /// <summary>tEquipment (abstract)</summary>
    public abstract class Equipment : PowerSystemResource
    {
        public bool Virtual { get; set; } = false;
    }

    /// <summary>tSubstation</summary>
    public class Substation : EquipmentContainer
    {
        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        // A Substation can also live inside a Process
        public int? ProcessParentId { get; set; }
        public Process? ProcessParent { get; set; }

        public ICollection<VoltageLevel> VoltageLevels { get; set; } = new List<VoltageLevel>();
        public ICollection<Function> Functions { get; set; } = new List<Function>();
    }

    /// <summary>tVoltageLevel</summary>
    public class VoltageLevel : EquipmentContainer
    {
        public int? SubstationId { get; set; }
        public Substation? Substation { get; set; }

        public decimal? NomFreq { get; set; }
        public byte? NumPhases { get; set; }

        // Voltage child element (tVoltage)
        public decimal? VoltageValue { get; set; }
        [MaxLength(10)]
        public string? VoltageUnit { get; set; }
        [MaxLength(10)]
        public string? VoltageMultiplier { get; set; }

        public ICollection<Bay> Bays { get; set; } = new List<Bay>();
        public ICollection<Function> Functions { get; set; } = new List<Function>();
    }

    /// <summary>tBay</summary>
    public class Bay : EquipmentContainer
    {
        public int? VoltageLevelId { get; set; }
        public VoltageLevel? VoltageLevel { get; set; }

        public ICollection<ConductingEquipment> ConductingEquipments { get; set; } = new List<ConductingEquipment>();
        public ICollection<ConnectivityNode> ConnectivityNodes { get; set; } = new List<ConnectivityNode>();
        public ICollection<Function> Functions { get; set; } = new List<Function>();
    }

    /// <summary>tAbstractConductingEquipment (abstract)</summary>
    public abstract class AbstractConductingEquipment : Equipment
    {
        public ICollection<Terminal> Terminals { get; set; } = new List<Terminal>();
        public ICollection<SubEquipment> SubEquipments { get; set; } = new List<SubEquipment>();
    }

    /// <summary>tConductingEquipment</summary>
    public class ConductingEquipment : AbstractConductingEquipment
    {
        // FK – belongs to Bay, Function, SubFunction, Line, or Process
        public int? BayId { get; set; }
        public Bay? Bay { get; set; }

        public int? FunctionId { get; set; }
        public Function? FunctionParent { get; set; }

        public int? SubFunctionId { get; set; }
        public SubFunction? SubFunctionParent { get; set; }

        public int? LineParentId { get; set; }
        public Line? LineParent { get; set; }

        public int? ProcessParentId { get; set; }
        public Process? ProcessParent { get; set; }

        [Required, MaxLength(10)]
        public string EquipmentType { get; set; } = string.Empty;   // tCommonConductingEquipmentEnum

        public ICollection<EqFunction> EqFunctions { get; set; } = new List<EqFunction>();
    }

    /// <summary>tSubEquipment</summary>
    public class SubEquipment : PowerSystemResource
    {
        public int? AbstractConductingEquipmentId { get; set; }
        public AbstractConductingEquipment? Parent { get; set; }

        [MaxLength(5)]
        public string Phase { get; set; } = "none";

        public bool Virtual { get; set; } = false;

        public ICollection<EqFunction> EqFunctions { get; set; } = new List<EqFunction>();
    }

    /// <summary>tPowerTransformer</summary>
    public class PowerTransformer : Equipment
    {
        public int? EquipmentContainerId { get; set; }
        public EquipmentContainer? EquipmentContainerParent { get; set; }

        public string EquipmentType { get; set; } = "PTR";   // fixed

        public ICollection<TransformerWinding> TransformerWindings { get; set; } = new List<TransformerWinding>();
        public ICollection<SubEquipment> SubEquipments { get; set; } = new List<SubEquipment>();
        public ICollection<EqFunction> EqFunctions { get; set; } = new List<EqFunction>();
    }

    /// <summary>tTransformerWinding</summary>
    public class TransformerWinding : AbstractConductingEquipment
    {
        public int? PowerTransformerId { get; set; }
        public PowerTransformer? PowerTransformer { get; set; }

        public string WindingType { get; set; } = "PTW";   // fixed

        public TapChanger? TapChanger { get; set; }
        public Terminal? NeutralPoint { get; set; }
        public ICollection<EqFunction> EqFunctions { get; set; } = new List<EqFunction>();
    }

    /// <summary>tTapChanger</summary>
    public class TapChanger : PowerSystemResource
    {
        public int? TransformerWindingId { get; set; }
        public TransformerWinding? TransformerWinding { get; set; }

        public string TapChangerType { get; set; } = "LTC";   // fixed
        public bool Virtual { get; set; } = false;

        public ICollection<SubEquipment> SubEquipments { get; set; } = new List<SubEquipment>();
        public ICollection<EqFunction> EqFunctions { get; set; } = new List<EqFunction>();
    }

    /// <summary>tGeneralEquipment</summary>
    public class GeneralEquipment : Equipment
    {
        public int? EquipmentContainerId { get; set; }
        public EquipmentContainer? EquipmentContainerParent { get; set; }

        public int? FunctionId { get; set; }
        public Function? FunctionParent { get; set; }

        public int? SubFunctionId { get; set; }
        public SubFunction? SubFunctionParent { get; set; }

        // FK for EqFunction.GeneralEquipments collection (EqFunction is parent)
        public int? EqFunctionParentId { get; set; }
        public EqFunction? EqFunctionParent { get; set; }

        // FK for EqSubFunction.GeneralEquipments collection
        public int? EqSubFunctionParentId { get; set; }
        public EqSubFunction? EqSubFunctionParent { get; set; }

        // FK for Line.GeneralEquipments collection
        public int? LineParentId { get; set; }
        public Line? LineParent { get; set; }

        // FK for Process.GeneralEquipments collection
        public int? ProcessParentId { get; set; }
        public Process? ProcessParent { get; set; }

        [Required, MaxLength(10)]
        public string EquipmentType { get; set; } = string.Empty;   // tGeneralEquipmentEnum

        // EqFunctions where THIS GeneralEquipment is the parent
        public ICollection<EqFunction> EqFunctions { get; set; } = new List<EqFunction>();
    }

    /// <summary>tConnectivityNode</summary>
    public class ConnectivityNode : LNodeContainer
    {
        public int? BayId { get; set; }
        public Bay? Bay { get; set; }

        public int? LineId { get; set; }
        public Line? Line { get; set; }

        [Required, MaxLength(255)]
        public string PathName { get; set; } = string.Empty;
    }

    /// <summary>tTerminal</summary>
    public class Terminal : SclUnNaming
    {
        public int? AbstractConductingEquipmentId { get; set; }
        public AbstractConductingEquipment? Parent { get; set; }

        public int? TransformerWindingNeutralId { get; set; }
        public TransformerWinding? NeutralWinding { get; set; }

        [MaxLength(255)]
        public string TerminalName { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string ConnectivityNode { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ProcessName { get; set; }

        [MaxLength(255)]
        public string? SubstationName { get; set; }

        [MaxLength(255)]
        public string? VoltageLevelName { get; set; }

        [MaxLength(255)]
        public string? BayName { get; set; }

        [Required, MaxLength(255)]
        public string CNodeName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? LineName { get; set; }
    }

    /// <summary>tFunction</summary>
    public class Function : PowerSystemResource
    {
        public int? SubstationId { get; set; }
        public Substation? Substation { get; set; }

        public int? VoltageLevelId { get; set; }
        public VoltageLevel? VoltageLevel { get; set; }

        public int? BayId { get; set; }
        public Bay? Bay { get; set; }

        public int? LineParentId { get; set; }
        public Line? LineParent { get; set; }

        public int? ProcessParentId { get; set; }
        public Process? ProcessParent { get; set; }

        [MaxLength(255)]
        public string? FunctionType { get; set; }

        public ICollection<SubFunction> SubFunctions { get; set; } = new List<SubFunction>();
        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
        public ICollection<ConductingEquipment> ConductingEquipments { get; set; } = new List<ConductingEquipment>();
    }

    /// <summary>tSubFunction</summary>
    public class SubFunction : PowerSystemResource
    {
        public int? FunctionId { get; set; }
        public Function? FunctionParent { get; set; }

        public int? SubFunctionParentId { get; set; }
        public SubFunction? SubFunctionParent { get; set; }

        [MaxLength(255)]
        public string? SubFunctionType { get; set; }

        public ICollection<SubFunction> SubFunctions { get; set; } = new List<SubFunction>();
        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
        public ICollection<ConductingEquipment> ConductingEquipments { get; set; } = new List<ConductingEquipment>();
    }

    /// <summary>tAbstractEqFuncSubFunc / tEqFunction / tEqSubFunction</summary>
    public class EqFunction : PowerSystemResource
    {
        // parent can be ConductingEquipment, SubEquipment, PowerTransformer, TransformerWinding, TapChanger, GeneralEquipment
        public int? ConductingEquipmentId { get; set; }
        public ConductingEquipment? ConductingEquipmentParent { get; set; }

        public int? SubEquipmentId { get; set; }
        public SubEquipment? SubEquipmentParent { get; set; }

        public int? PowerTransformerId { get; set; }
        public PowerTransformer? PowerTransformerParent { get; set; }

        public int? TransformerWindingId { get; set; }
        public TransformerWinding? TransformerWindingParent { get; set; }

        public int? TapChangerId { get; set; }
        public TapChanger? TapChangerParent { get; set; }

        public int? GeneralEquipmentId { get; set; }
        public GeneralEquipment? GeneralEquipmentParent { get; set; }

        [MaxLength(255)]
        public string? EqFunctionType { get; set; }

        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
        public ICollection<EqSubFunction> EqSubFunctions { get; set; } = new List<EqSubFunction>();
    }

    /// <summary>tEqSubFunction</summary>
    public class EqSubFunction : PowerSystemResource
    {
        public int? EqFunctionId { get; set; }
        public EqFunction? EqFunctionParent { get; set; }

        public int? EqSubFunctionParentId { get; set; }
        public EqSubFunction? EqSubFunctionParent { get; set; }

        [MaxLength(255)]
        public string? EqSubFunctionType { get; set; }

        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
        public ICollection<EqSubFunction> EqSubFunctions { get; set; } = new List<EqSubFunction>();
    }

    /// <summary>tLine</summary>
    public class Line : PowerSystemResource
    {
        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        public int? ProcessId { get; set; }
        public Process? ProcessParent { get; set; }

        [MaxLength(255)]
        public string? LineType { get; set; }

        public decimal? NomFreq { get; set; }
        public byte? NumPhases { get; set; }

        public decimal? VoltageValue { get; set; }
        [MaxLength(10)]
        public string? VoltageMultiplier { get; set; }

        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
        public ICollection<Function> Functions { get; set; } = new List<Function>();
        public ICollection<ConductingEquipment> ConductingEquipments { get; set; } = new List<ConductingEquipment>();
        public ICollection<ConnectivityNode> ConnectivityNodes { get; set; } = new List<ConnectivityNode>();
    }

    /// <summary>tProcess</summary>
    public class Process : PowerSystemResource
    {
        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        public int? ProcessParentId { get; set; }
        public Process? ProcessParent { get; set; }

        [MaxLength(255)]
        public string? ProcessType { get; set; }

        public ICollection<GeneralEquipment> GeneralEquipments { get; set; } = new List<GeneralEquipment>();
        public ICollection<Function> Functions { get; set; } = new List<Function>();
        public ICollection<ConductingEquipment> ConductingEquipments { get; set; } = new List<ConductingEquipment>();
        public ICollection<Substation> Substations { get; set; } = new List<Substation>();
        public ICollection<Line> Lines { get; set; } = new List<Line>();
        public ICollection<Process> SubProcesses { get; set; } = new List<Process>();
    }
}
