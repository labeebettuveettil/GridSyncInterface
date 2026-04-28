using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GridSyncInterface.Models.Base;

namespace GridSyncInterface.Models
{
    /// <summary>
    /// Represents the root SCL element.  
    /// One row = one configuration project/file.
    /// </summary>
    public class SCL : SclBaseElement
    {
        [Required, MaxLength(10)]
        public string Version { get; set; } = "2007";

        [Required, MaxLength(1)]
        public string Revision { get; set; } = "B";

        public byte Release { get; set; } = 4;

        // ?? Child collections ??????????????????????????????????????????????
        public SclHeader? Header { get; set; }

        public ICollection<Substation> Substations { get; set; } = new List<Substation>();
        public SclCommunication? Communication { get; set; }
        public ICollection<IED> IEDs { get; set; } = new List<IED>();
        public DataTypeTemplates? DataTypeTemplates { get; set; }
        public ICollection<Line> Lines { get; set; } = new List<Line>();
        public ICollection<Process> Processes { get; set; } = new List<Process>();
    }

    /// <summary>tHeader</summary>
    public class SclHeader
    {
        [Key]
        public int Id { get; set; }

        // FK to SCL
        public int SclId { get; set; }
        public SCL? Scl { get; set; }

        [Required, MaxLength(255)]
        public string HeaderId { get; set; } = string.Empty;  // the 'id' attribute

        [MaxLength(255)]
        public string? Version { get; set; }

        [MaxLength(255)]
        public string? Revision { get; set; }

        [MaxLength(255)]
        public string? ToolID { get; set; }

        [MaxLength(50)]
        public string NameStructure { get; set; } = "IEDName";

        public string? TextContent { get; set; }

        public ICollection<Hitem> History { get; set; } = new List<Hitem>();
    }

    /// <summary>tHitem – history item</summary>
    public class Hitem
    {
        [Key]
        public int Id { get; set; }

        public int SclHeaderId { get; set; }
        public SclHeader? SclHeader { get; set; }

        [Required, MaxLength(255)]
        public string Version { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Revision { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string When { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Who { get; set; }

        [MaxLength(1000)]
        public string? What { get; set; }

        [MaxLength(1000)]
        public string? Why { get; set; }

        public string? Content { get; set; }
    }
}
