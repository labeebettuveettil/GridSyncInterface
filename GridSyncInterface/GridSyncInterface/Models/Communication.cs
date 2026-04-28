using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GridSyncInterface.Models.Base;

namespace GridSyncInterface.Models
{
    // ?? Communication ????????????????????????????????????????????????????????

    /// <summary>tCommunication</summary>
    public class SclCommunication : SclUnNaming
    {
        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        public ICollection<SubNetwork> SubNetworks { get; set; } = new List<SubNetwork>();
    }

    /// <summary>tSubNetwork</summary>
    public class SubNetwork : SclNaming
    {
        public int CommunicationId { get; set; }
        public SclCommunication? Communication { get; set; }

        [MaxLength(100)]
        public string? NetworkType { get; set; }

        // BitRate child element
        public decimal? BitRateValue { get; set; }
        [MaxLength(10)]
        public string? BitRateMultiplier { get; set; }

        public ICollection<ConnectedAP> ConnectedAPs { get; set; } = new List<ConnectedAP>();
    }

    /// <summary>tConnectedAP</summary>
    public class ConnectedAP : SclUnNaming
    {
        public int SubNetworkId { get; set; }
        public SubNetwork? SubNetwork { get; set; }

        [Required, MaxLength(64)]
        public string IedName { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        public string ApName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? RedProt { get; set; }

        public NetworkAddress? Address { get; set; }
        public ICollection<GSE> GSEs { get; set; } = new List<GSE>();
        public ICollection<SMV> SMVs { get; set; } = new List<SMV>();
        public ICollection<PhysConn> PhysConns { get; set; } = new List<PhysConn>();
    }

    /// <summary>tAddress – network address container</summary>
    public class NetworkAddress
    {
        [Key]
        public int Id { get; set; }

        public int? ConnectedAPId { get; set; }
        public ConnectedAP? ConnectedAP { get; set; }

        public int? ControlBlockId { get; set; }
        public ControlBlock? ControlBlock { get; set; }

        public ICollection<PAddress> PAddresses { get; set; } = new List<PAddress>();
    }

    /// <summary>tP – address parameter</summary>
    public class PAddress
    {
        [Key]
        public int Id { get; set; }

        public int NetworkAddressId { get; set; }
        public NetworkAddress? NetworkAddress { get; set; }

        [Required, MaxLength(50)]
        public string PType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Value { get; set; }
    }

    /// <summary>tControlBlock (abstract) – base for GSE and SMV in communication section</summary>
    public abstract class ControlBlock : SclUnNaming
    {
        public int ConnectedAPId { get; set; }
        public ConnectedAP? ConnectedAP { get; set; }

        [Required, MaxLength(64)]
        public string LdInst { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        public string CbName { get; set; } = string.Empty;

        public NetworkAddress? Address { get; set; }
    }

    /// <summary>tGSE – GSE control block in communication section</summary>
    public class GSE : ControlBlock
    {
        // MinTime / MaxTime (tDurationInMilliSec)
        public decimal? MinTime { get; set; }
        public decimal? MaxTime { get; set; }
    }

    /// <summary>tSMV – SMV control block in communication section</summary>
    public class SMV : ControlBlock { }

    /// <summary>tPhysConn – Physical connection</summary>
    public class PhysConn : SclUnNaming
    {
        public int ConnectedAPId { get; set; }
        public ConnectedAP? ConnectedAP { get; set; }

        [Required, MaxLength(50)]
        public string PhysConnType { get; set; } = string.Empty;

        public ICollection<PPhysConn> PValues { get; set; } = new List<PPhysConn>();
    }

    /// <summary>tP_PhysConn</summary>
    public class PPhysConn
    {
        [Key]
        public int Id { get; set; }

        public int PhysConnId { get; set; }
        public PhysConn? PhysConn { get; set; }

        [Required, MaxLength(50)]
        public string PType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Value { get; set; }
    }
}
