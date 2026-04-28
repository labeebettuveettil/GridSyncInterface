using Microsoft.EntityFrameworkCore;
using GridSyncInterface.Models;
using GridSyncInterface.Models.Auth;
using GridSyncInterface.Models.Base;

namespace GridSyncInterface.Data
{
    public class SclDbContext : DbContext
    {
        public SclDbContext(DbContextOptions<SclDbContext> options) : base(options) { }

        // ?? Auth / Project ??????????????????????????????????????????????????
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMembership> ProjectMemberships { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ElementLock> ElementLocks { get; set; }

        // ?? Root ????????????????????????????????????????????????????????????
        public DbSet<SCL> SCLs { get; set; }
        public DbSet<SclHeader> Headers { get; set; }
        public DbSet<Hitem> Hitems { get; set; }

        // ?? Substation / power system ???????????????????????????????????????
        public DbSet<LNode> LNodes { get; set; }
        public DbSet<Substation> Substations { get; set; }
        public DbSet<VoltageLevel> VoltageLevels { get; set; }
        public DbSet<Bay> Bays { get; set; }
        public DbSet<ConductingEquipment> ConductingEquipments { get; set; }
        public DbSet<SubEquipment> SubEquipments { get; set; }
        public DbSet<PowerTransformer> PowerTransformers { get; set; }
        public DbSet<TransformerWinding> TransformerWindings { get; set; }
        public DbSet<TapChanger> TapChangers { get; set; }
        public DbSet<GeneralEquipment> GeneralEquipments { get; set; }
        public DbSet<ConnectivityNode> ConnectivityNodes { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<SubFunction> SubFunctions { get; set; }
        public DbSet<EqFunction> EqFunctions { get; set; }
        public DbSet<EqSubFunction> EqSubFunctions { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Process> Processes { get; set; }

        // ?? IED ??????????????????????????????????????????????????????????????
        public DbSet<IED> IEDs { get; set; }
        public DbSet<IedServices> IedServices { get; set; }
        public DbSet<AccessPoint> AccessPoints { get; set; }
        public DbSet<ServerAt> ServerAts { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<LDevice> LDevices { get; set; }
        public DbSet<Association> Associations { get; set; }
        public DbSet<LN0> LN0s { get; set; }
        public DbSet<LogicalNode> LogicalNodes { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<FCDA> FCDAs { get; set; }
        public DbSet<ReportControl> ReportControls { get; set; }
        public DbSet<RptEnabled> RptEnableds { get; set; }
        public DbSet<ClientLN> ClientLNs { get; set; }
        public DbSet<LogControl> LogControls { get; set; }
        public DbSet<GSEControl> GSEControls { get; set; }
        public DbSet<SampledValueControl> SampledValueControls { get; set; }
        public DbSet<ControlIEDName> ControlIEDNames { get; set; }
        public DbSet<SettingControl> SettingControls { get; set; }
        public DbSet<Inputs> Inputs { get; set; }
        public DbSet<ExtRef> ExtRefs { get; set; }
        public DbSet<SclLog> SclLogs { get; set; }
        public DbSet<DOI> DOIs { get; set; }
        public DbSet<SDI> SDIs { get; set; }
        public DbSet<DAI> DAIs { get; set; }
        public DbSet<Val> Vals { get; set; }
        public DbSet<KDC> KDCs { get; set; }

        // ?? Communication ???????????????????????????????????????????????????
        public DbSet<SclCommunication> Communications { get; set; }
        public DbSet<SubNetwork> SubNetworks { get; set; }
        public DbSet<ConnectedAP> ConnectedAPs { get; set; }
        public DbSet<NetworkAddress> NetworkAddresses { get; set; }
        public DbSet<PAddress> PAddresses { get; set; }
        public DbSet<GSE> GSEs { get; set; }
        public DbSet<SMV> SMVs { get; set; }
        public DbSet<PhysConn> PhysConns { get; set; }
        public DbSet<PPhysConn> PPhysConns { get; set; }

        // ?? DataTypeTemplates ???????????????????????????????????????????????
        public DbSet<DataTypeTemplates> DataTypeTemplates { get; set; }
        public DbSet<LNodeType> LNodeTypes { get; set; }
        public DbSet<DataObject> DataObjects { get; set; }
        public DbSet<DOType> DOTypes { get; set; }
        public DbSet<SDO> SDOs { get; set; }
        public DbSet<DataAttribute> DataAttributes { get; set; }
        public DbSet<DAType> DATypes { get; set; }
        public DbSet<BDA> BDAs { get; set; }
        public DbSet<ProtNs> ProtNsEntries { get; set; }
        public DbSet<EnumType> EnumTypes { get; set; }
        public DbSet<EnumVal> EnumVals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ?? Decimal precision convention (prevents silent SQL Server truncation) ??
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(6);
            }

            // ?? Auth / Project ??????????????????????????????????????????????
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<ProjectMembership>()
                .HasIndex(m => new { m.ProjectId, m.UserId }).IsUnique();

            // Project ? CreatedByUser (restrict delete to avoid cascade cycles)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project one-to-one SCL
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Scl)
                .WithOne()
                .HasForeignKey<Project>(p => p.SclId)
                .OnDelete(DeleteBehavior.SetNull);

            // AuditLog FK delete behaviour
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Project)
                .WithMany(p => p.AuditLogs)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // ElementLock FK delete behaviour
            modelBuilder.Entity<ElementLock>()
                .HasIndex(el => new { el.ProjectId, el.EntityType, el.EntityId }).IsUnique();

            modelBuilder.Entity<ElementLock>()
                .HasOne(el => el.User)
                .WithMany()
                .HasForeignKey(el => el.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ElementLock>()
                .HasOne(el => el.Project)
                .WithMany(p => p.ElementLocks)
                .HasForeignKey(el => el.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // ?? IED unique index should be per-project ??????????????????????
            modelBuilder.Entity<IED>()
                .HasIndex(i => new { i.SclId, i.Name }).IsUnique();
            // Remove old single-column unique index added by previous migration
            modelBuilder.Entity<IED>()
                .HasIndex(i => i.Name).IsUnique(false);

            // ?? TPH inheritance hierarchy ???????????????????????????????????
            modelBuilder.Entity<LNodeContainer>().ToTable("LNodeContainers")
                .HasDiscriminator<string>("ContainerType")
                .HasValue<LNodeContainer>("Base")
                .HasValue<PowerSystemResource>("PowerSystemResource")
                .HasValue<EquipmentContainer>("EquipmentContainer")
                .HasValue<Equipment>("Equipment")
                .HasValue<Substation>("Substation")
                .HasValue<VoltageLevel>("VoltageLevel")
                .HasValue<Bay>("Bay")
                .HasValue<AbstractConductingEquipment>("AbstractConductingEquipment")
                .HasValue<ConductingEquipment>("ConductingEquipment")
                .HasValue<TransformerWinding>("TransformerWinding")
                .HasValue<PowerTransformer>("PowerTransformer")
                .HasValue<SubEquipment>("SubEquipment")
                .HasValue<TapChanger>("TapChanger")
                .HasValue<GeneralEquipment>("GeneralEquipment")
                .HasValue<ConnectivityNode>("ConnectivityNode")
                .HasValue<Function>("Function")
                .HasValue<SubFunction>("SubFunction")
                .HasValue<EqFunction>("EqFunction")
                .HasValue<EqSubFunction>("EqSubFunction")
                .HasValue<Line>("Line")
                .HasValue<Process>("Process");

            modelBuilder.Entity<AnyLogicalNode>().ToTable("LogicalNodes")
                .HasDiscriminator<string>("LogicalNodeKind")
                .HasValue<LN0>("LN0")
                .HasValue<LogicalNode>("LN");

            modelBuilder.Entity<Control>().ToTable("Controls")
                .HasDiscriminator<string>("ControlType")
                .HasValue<Control>("Base")
                .HasValue<ControlWithTriggerOpt>("WithTrigger")
                .HasValue<ReportControl>("ReportControl")
                .HasValue<LogControl>("LogControl")
                .HasValue<ControlWithIEDName>("WithIEDName")
                .HasValue<GSEControl>("GSEControl")
                .HasValue<SampledValueControl>("SampledValueControl");

            modelBuilder.Entity<ControlBlock>().ToTable("CommunicationControlBlocks")
                .HasDiscriminator<string>("CbType")
                .HasValue<GSE>("GSE")
                .HasValue<SMV>("SMV");

            // ?? Relationships ???????????????????????????????????????????????
            modelBuilder.Entity<LNode>()
                .HasOne(l => l.LNodeContainer)
                .WithMany(c => c.LNodes)
                .HasForeignKey(l => l.LNodeContainerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SCL>()
                .HasOne(s => s.Header)
                .WithOne(h => h.Scl)
                .HasForeignKey<SclHeader>(h => h.SclId);

            modelBuilder.Entity<SCL>()
                .HasOne(s => s.Communication)
                .WithOne(c => c.Scl)
                .HasForeignKey<SclCommunication>(c => c.SclId);

            modelBuilder.Entity<SCL>()
                .HasOne(s => s.DataTypeTemplates)
                .WithOne(d => d.Scl)
                .HasForeignKey<DataTypeTemplates>(d => d.SclId);

            modelBuilder.Entity<IED>()
                .HasOne(i => i.Scl)
                .WithMany(s => s.IEDs)
                .HasForeignKey(i => i.SclId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IedServices>()
                .HasOne(s => s.Ied)
                .WithOne(i => i.Services)
                .HasForeignKey<IedServices>(s => s.IedId);

            modelBuilder.Entity<AccessPoint>()
                .HasOne(a => a.Ied)
                .WithMany(i => i.AccessPoints)
                .HasForeignKey(a => a.IedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Server>()
                .HasOne(s => s.AccessPoint)
                .WithOne(a => a.Server)
                .HasForeignKey<Server>(s => s.AccessPointId);

            modelBuilder.Entity<LDevice>()
                .HasOne(d => d.Server)
                .WithMany(s => s.LDevices)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LN0>()
                .HasOne(ln => ln.LDevice)
                .WithOne(d => d.LN0)
                .HasForeignKey<LN0>(ln => ln.LDeviceId)
                .OnDelete(DeleteBehavior.NoAction);

            // ?? Break all cascade paths that reference LogicalNodes (LN0 / LN) ??
            // This prevents SQL Server multiple-cascade-path errors from the
            // IED ? Server ? LDevice chain reaching LN0 then its children.

            modelBuilder.Entity<DataSet>()
                .HasOne(ds => ds.LN0).WithMany(ln => ln.DataSets)
                .HasForeignKey(ds => ds.LN0Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DataSet>()
                .HasOne(ds => ds.LogicalNode).WithMany(ln => ln.DataSets)
                .HasForeignKey(ds => ds.LogicalNodeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReportControl>()
                .HasOne(rc => rc.LN0).WithMany(ln => ln.ReportControls)
                .HasForeignKey(rc => rc.LN0Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ReportControl>()
                .HasOne(rc => rc.LogicalNode).WithMany(ln => ln.ReportControls)
                .HasForeignKey(rc => rc.LogicalNodeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LogControl>()
                .HasOne(lc => lc.LN0).WithMany(ln => ln.LogControls)
                .HasForeignKey(lc => lc.LN0Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<LogControl>()
                .HasOne(lc => lc.LogicalNode).WithMany(ln => ln.LogControls)
                .HasForeignKey(lc => lc.LogicalNodeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DOI>()
                .HasOne(d => d.LN0).WithMany(ln => ln.DOIs)
                .HasForeignKey(d => d.LN0Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DOI>()
                .HasOne(d => d.LogicalNode).WithMany(ln => ln.DOIs)
                .HasForeignKey(d => d.LogicalNodeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GSEControl>()
                .HasOne(g => g.LN0).WithMany(ln => ln.GSEControls)
                .HasForeignKey(g => g.LN0Id).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SampledValueControl>()
                .HasOne(sv => sv.LN0).WithMany(ln => ln.SampledValueControls)
                .HasForeignKey(sv => sv.LN0Id).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SettingControl>()
                .HasOne(sc => sc.LN0).WithOne(ln => ln.SettingControl)
                .HasForeignKey<SettingControl>(sc => sc.LN0Id).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Inputs>()
                .HasOne(i => i.LN0).WithOne(ln => ln.Inputs)
                .HasForeignKey<Inputs>(i => i.LN0Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Inputs>()
                .HasOne(i => i.LogicalNode).WithOne(ln => ln.Inputs)
                .HasForeignKey<Inputs>(i => i.LogicalNodeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SclLog>()
                .HasOne(sl => sl.LN0).WithMany(ln => ln.Logs)
                .HasForeignKey(sl => sl.LN0Id).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SclLog>()
                .HasOne(sl => sl.LogicalNode).WithMany(ln => ln.Logs)
                .HasForeignKey(sl => sl.LogicalNodeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LogicalNode>()
                .HasOne(ln => ln.LDevice)
                .WithMany(d => d.LNs)
                .HasForeignKey(ln => ln.LDeviceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubFunction>()
                .HasOne(sf => sf.SubFunctionParent)
                .WithMany(sf => sf.SubFunctions)
                .HasForeignKey(sf => sf.SubFunctionParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Process>()
                .HasOne(p => p.ProcessParent)
                .WithMany(p => p.SubProcesses)
                .HasForeignKey(p => p.ProcessParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SDI>()
                .HasOne(s => s.SDIParent)
                .WithMany(s => s.ChildSDIs)
                .HasForeignKey(s => s.SDIParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EqSubFunction>()
                .HasOne(e => e.EqSubFunctionParent)
                .WithMany(e => e.EqSubFunctions)
                .HasForeignKey(e => e.EqSubFunctionParentId)
                .OnDelete(DeleteBehavior.NoAction);

            // ?? EqFunction <-> GeneralEquipment (two distinct relationships) ??????
            // Relationship 1: GeneralEquipment is PARENT, EqFunction is CHILD
            modelBuilder.Entity<EqFunction>()
                .HasOne(ef => ef.GeneralEquipmentParent)
                .WithMany(ge => ge.EqFunctions)
                .HasForeignKey(ef => ef.GeneralEquipmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship 2: EqFunction is PARENT, GeneralEquipment is CHILD
            modelBuilder.Entity<EqFunction>()
                .HasMany(ef => ef.GeneralEquipments)
                .WithOne(ge => ge.EqFunctionParent)
                .HasForeignKey(ge => ge.EqFunctionParentId)
                .OnDelete(DeleteBehavior.NoAction);

            // ?? EqSubFunction -> GeneralEquipment (EqSubFunction is parent) ??????
            modelBuilder.Entity<EqSubFunction>()
                .HasMany(es => es.GeneralEquipments)
                .WithOne(ge => ge.EqSubFunctionParent)
                .HasForeignKey(ge => ge.EqSubFunctionParentId)
                .OnDelete(DeleteBehavior.NoAction);

            // ?? Line -> GeneralEquipment, ConductingEquipment, Function ????????
            modelBuilder.Entity<Line>()
                .HasMany(l => l.GeneralEquipments)
                .WithOne(ge => ge.LineParent)
                .HasForeignKey(ge => ge.LineParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Line>()
                .HasMany(l => l.ConductingEquipments)
                .WithOne(ce => ce.LineParent)
                .HasForeignKey(ce => ce.LineParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Line>()
                .HasMany(l => l.Functions)
                .WithOne(f => f.LineParent)
                .HasForeignKey(f => f.LineParentId)
                .OnDelete(DeleteBehavior.NoAction);

            // ?? Process -> Substation, GeneralEquipment, ConductingEquipment, Function ?
            modelBuilder.Entity<Process>()
                .HasMany(p => p.Substations)
                .WithOne(s => s.ProcessParent)
                .HasForeignKey(s => s.ProcessParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Process>()
                .HasMany(p => p.GeneralEquipments)
                .WithOne(ge => ge.ProcessParent)
                .HasForeignKey(ge => ge.ProcessParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Process>()
                .HasMany(p => p.ConductingEquipments)
                .WithOne(ce => ce.ProcessParent)
                .HasForeignKey(ce => ce.ProcessParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Process>()
                .HasMany(p => p.Functions)
                .WithOne(f => f.ProcessParent)
                .HasForeignKey(f => f.ProcessParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Val>()
                .HasOne(v => v.DAI).WithMany(d => d.Vals)
                .HasForeignKey(v => v.DAIId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Val>()
                .HasOne(v => v.DataAttribute).WithMany(d => d.Vals)
                .HasForeignKey(v => v.DataAttributeId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Val>()
                .HasOne(v => v.BDA).WithMany(b => b.Vals)
                .HasForeignKey(v => v.BDAId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.AccessPointGoose).WithMany(a => a.GOOSESecurities)
                .HasForeignKey(c => c.AccessPointGooseId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.AccessPointSmv).WithMany(a => a.SMVSecurities)
                .HasForeignKey(c => c.AccessPointSmvId).OnDelete(DeleteBehavior.NoAction);

            // Terminal neutral point is a one-to-one with TransformerWinding; FK lives on Terminal
            modelBuilder.Entity<TransformerWinding>()
                .HasOne(tw => tw.NeutralPoint)
                .WithOne(t => t.NeutralWinding)
                .HasForeignKey<Terminal>(t => t.TransformerWindingNeutralId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NetworkAddress>()
                .HasOne(n => n.ConnectedAP).WithOne(c => c.Address)
                .HasForeignKey<NetworkAddress>(n => n.ConnectedAPId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NetworkAddress>()
                .HasOne(n => n.ControlBlock).WithOne(c => c.Address)
                .HasForeignKey<NetworkAddress>(n => n.ControlBlockId).OnDelete(DeleteBehavior.NoAction);

            // ?? Indexes ?????????????????????????????????????????????????????
            modelBuilder.Entity<Substation>().HasIndex(s => s.Name);
            modelBuilder.Entity<LNodeType>().HasIndex(l => l.SclId);
            modelBuilder.Entity<DOType>().HasIndex(d => d.SclId);
            modelBuilder.Entity<DAType>().HasIndex(d => d.SclId);
            modelBuilder.Entity<EnumType>().HasIndex(e => e.SclId);
            modelBuilder.Entity<AuditLog>().HasIndex(a => new { a.ProjectId, a.Timestamp });
            modelBuilder.Entity<ElementLock>().HasIndex(el => el.ExpiresAt);
        }
    }
}
