using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GridSyncInterface.Models.Base;
using GridSyncInterface.Models.Enums;

namespace GridSyncInterface.Models
{
    // ?? DataTypeTemplates ????????????????????????????????????????????????????

    /// <summary>tDataTypeTemplates root container</summary>
    public class DataTypeTemplates
    {
        [Key]
        public int Id { get; set; }

        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        public ICollection<LNodeType> LNodeTypes { get; set; } = new List<LNodeType>();
        public ICollection<DOType> DOTypes { get; set; } = new List<DOType>();
        public ICollection<DAType> DATypes { get; set; } = new List<DAType>();
        public ICollection<EnumType> EnumTypes { get; set; } = new List<EnumType>();
    }

    /// <summary>tLNodeType</summary>
    public class LNodeType : SclIDNaming
    {
        public int DataTypeTemplatesId { get; set; }
        public DataTypeTemplates? DataTypeTemplates { get; set; }

        [MaxLength(255)]
        public string IedType { get; set; } = string.Empty;

        [Required, MaxLength(4)]
        public string LnClass { get; set; } = string.Empty;

        public ICollection<DataObject> DOs { get; set; } = new List<DataObject>();
    }

    /// <summary>tDO – Data Object reference in LNodeType</summary>
    public class DataObject : SclUnNaming
    {
        public int LNodeTypeId { get; set; }
        public LNodeType? LNodeType { get; set; }

        [Required, MaxLength(12)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string DoType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? AccessControl { get; set; }

        public bool Transient { get; set; } = false;
    }

    /// <summary>tDOType</summary>
    public class DOType : SclIDNaming
    {
        public int DataTypeTemplatesId { get; set; }
        public DataTypeTemplates? DataTypeTemplates { get; set; }

        [MaxLength(255)]
        public string IedType { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        public string Cdc { get; set; } = string.Empty;   // CDC enum as string

        public ICollection<SDO> SDOs { get; set; } = new List<SDO>();
        public ICollection<DataAttribute> DAs { get; set; } = new List<DataAttribute>();
    }

    /// <summary>tSDO – Sub Data Object in DOType</summary>
    public class SDO : SclUnNaming
    {
        public int DOTypeId { get; set; }
        public DOType? DOType { get; set; }

        [Required, MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string SdoType { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Count { get; set; } = "0";
    }

    /// <summary>tAbstractDataAttribute (abstract) – base for DA and BDA</summary>
    public abstract class AbstractDataAttribute : SclUnNaming
    {
        [Required, MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? SAddr { get; set; }

        [Required, MaxLength(20)]
        public string BType { get; set; } = string.Empty;   // BasicType as string

        [MaxLength(10)]
        public string ValKind { get; set; } = "Set";

        [MaxLength(255)]
        public string? AttributeType { get; set; }   // reference to DAType or EnumType id

        [MaxLength(50)]
        public string Count { get; set; } = "0";

        public bool ValImport { get; set; } = false;

        public ICollection<Val> Vals { get; set; } = new List<Val>();
    }

    /// <summary>tDA – Data Attribute in DOType</summary>
    public class DataAttribute : AbstractDataAttribute
    {
        public int DOTypeId { get; set; }
        public DOType? DOType { get; set; }

        // Trigger options
        public bool Dchg { get; set; }
        public bool Qchg { get; set; }
        public bool Dupd { get; set; }

        [Required, MaxLength(5)]
        public string Fc { get; set; } = string.Empty;   // FunctionalConstraint enum

        public ICollection<ProtNs> ProtNsEntries { get; set; } = new List<ProtNs>();
    }

    /// <summary>tDAType</summary>
    public class DAType : SclIDNaming
    {
        public int DataTypeTemplatesId { get; set; }
        public DataTypeTemplates? DataTypeTemplates { get; set; }

        [MaxLength(255)]
        public string IedType { get; set; } = string.Empty;

        public ICollection<BDA> BDAs { get; set; } = new List<BDA>();
        public ICollection<ProtNs> ProtNsEntries { get; set; } = new List<ProtNs>();
    }

    /// <summary>tBDA – Basic Data Attribute in DAType</summary>
    public class BDA : AbstractDataAttribute
    {
        public int DATypeId { get; set; }
        public DAType? DAType { get; set; }
    }

    /// <summary>tProtNs – Protocol namespace</summary>
    public class ProtNs
    {
        [Key]
        public int Id { get; set; }

        public int? DataAttributeId { get; set; }
        public DataAttribute? DataAttribute { get; set; }

        public int? DATypeId { get; set; }
        public DAType? DAType { get; set; }

        [MaxLength(10)]
        public string ProtNsType { get; set; } = "8-MMS";

        [MaxLength(255)]
        public string? NamespaceValue { get; set; }
    }

    /// <summary>tEnumType</summary>
    public class EnumType : SclIDNaming
    {
        public int DataTypeTemplatesId { get; set; }
        public DataTypeTemplates? DataTypeTemplates { get; set; }

        public ICollection<EnumVal> EnumVals { get; set; } = new List<EnumVal>();
    }

    /// <summary>tEnumVal</summary>
    public class EnumVal
    {
        [Key]
        public int Id { get; set; }

        public int EnumTypeId { get; set; }
        public EnumType? EnumType { get; set; }

        public int Ord { get; set; }

        [MaxLength(255)]
        public string? EnumDesc { get; set; }

        [MaxLength(127)]
        public string? Value { get; set; }
    }
}
