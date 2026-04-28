using System.Xml.Linq;
using GridSyncInterface.Data;
using GridSyncInterface.Models;

namespace GridSyncInterface.Parser
{
    /// <summary>
    /// Parses IEC 61850 SCL files (*.icd, *.iid, *.cid, *.scd) into the
    /// application object model and persists the result via EF Core.
    /// </summary>
    public class SclParser
    {
        // ?? SCL namespace (Ed.1 files may omit it; we handle both) ??????????
        private static readonly XNamespace Ns = "http://www.iec.ch/TC57/2007/SCL";

        private readonly SclDbContext _db;
        private readonly List<string> _warnings = [];

        public SclParser(SclDbContext db) => _db = db;

        // ????????????????????????????????????????????????????????????????????
        // Public entry points
        // ????????????????????????????????????????????????????????????????????

        /// <summary>Parse a file path and store the result in the database.</summary>
        public async Task<SclParserResult> ParseFileAsync(string filePath,
            CancellationToken ct = default)
        {
            if (!File.Exists(filePath))
                return SclParserResult.Fail($"File not found: {filePath}");

            try
            {
                var doc = XDocument.Load(filePath, LoadOptions.None);
                return await ParseDocumentAsync(doc, ct);
            }
            catch (Exception ex)
            {
                return SclParserResult.Fail($"Failed to load/parse XML: {ex.Message}");
            }
        }

        /// <summary>Parse from a stream and store the result in the database.</summary>
        public async Task<SclParserResult> ParseStreamAsync(Stream stream,
            CancellationToken ct = default)
        {
            try
            {
                var doc = await XDocument.LoadAsync(stream, LoadOptions.None, ct);
                return await ParseDocumentAsync(doc, ct);
            }
            catch (Exception ex)
            {
                return SclParserResult.Fail($"Failed to parse XML stream: {ex.Message}");
            }
        }

        // ????????????????????????????????????????????????????????????????????
        // Core document parsing
        // ????????????????????????????????????????????????????????????????????

        private async Task<SclParserResult> ParseDocumentAsync(XDocument doc,
            CancellationToken ct)
        {
            var root = doc.Root;
            if (root == null)
                return SclParserResult.Fail("Empty XML document.");

            // Support both namespaced and non-namespaced SCL files
            var ns = root.Name.Namespace;

            var scl = new SCL
            {
                Version = Attr(root, "version", "2007"),
                Revision = Attr(root, "revision", "B"),
                Release = AttrByte(root, "release", 4)
            };

            scl.Header = ParseHeader(root, ns);
            scl.Communication = ParseCommunication(root, ns);
            scl.DataTypeTemplates = ParseDataTypeTemplates(root, ns);

            foreach (var substEl in root.Elements(ns + "Substation"))
                scl.Substations.Add(ParseSubstation(substEl, ns));

            foreach (var iedEl in root.Elements(ns + "IED"))
                scl.IEDs.Add(ParseIed(iedEl, ns));

            foreach (var lineEl in root.Elements(ns + "Line"))
                scl.Lines.Add(ParseLine(lineEl, ns));

            foreach (var procEl in root.Elements(ns + "Process"))
                scl.Processes.Add(ParseProcess(procEl, ns));

            try
            {
                _db.SCLs.Add(scl);
                await _db.SaveChangesAsync(ct);
                return SclParserResult.Ok(scl.Id, _warnings);
            }
            catch (Exception ex)
            {
                return SclParserResult.Fail($"Database save failed: {ex.Message}");
            }
        }

        // ????????????????????????????????????????????????????????????????????
        // Header
        // ????????????????????????????????????????????????????????????????????

        private SclHeader? ParseHeader(XElement root, XNamespace ns)
        {
            var el = root.Element(ns + "Header");
            if (el == null) return null;

            var hdr = new SclHeader
            {
                HeaderId = Attr(el, "id"),
                Version = AttrOrNull(el, "version"),
                Revision = AttrOrNull(el, "revision"),
                ToolID = AttrOrNull(el, "toolID"),
                NameStructure = Attr(el, "nameStructure", "IEDName"),
                TextContent = el.Element(ns + "Text")?.Value
            };

            foreach (var hi in el.Elements(ns + "History").Elements(ns + "Hitem"))
            {
                hdr.History.Add(new Hitem
                {
                    Version = Attr(hi, "version"),
                    Revision = Attr(hi, "revision"),
                    When = Attr(hi, "when"),
                    Who = AttrOrNull(hi, "who"),
                    What = AttrOrNull(hi, "what"),
                    Why = AttrOrNull(hi, "why"),
                    Content = hi.Value.NullIfEmpty()
                });
            }

            return hdr;
        }

        // ????????????????????????????????????????????????????????????????????
        // Communication
        // ????????????????????????????????????????????????????????????????????

        private SclCommunication? ParseCommunication(XElement root, XNamespace ns)
        {
            var el = root.Element(ns + "Communication");
            if (el == null) return null;

            var comm = new SclCommunication { Desc = AttrOrNull(el, "desc") };

            foreach (var snEl in el.Elements(ns + "SubNetwork"))
            {
                var sn = new SubNetwork
                {
                    Name = Attr(snEl, "name"),
                    Desc = AttrOrNull(snEl, "desc"),
                    NetworkType = AttrOrNull(snEl, "type")
                };

                var brEl = snEl.Element(ns + "BitRate");
                if (brEl != null)
                {
                    sn.BitRateValue = ParseDecimal(brEl.Value);
                    sn.BitRateMultiplier = AttrOrNull(brEl, "multiplier");
                }

                foreach (var capEl in snEl.Elements(ns + "ConnectedAP"))
                    sn.ConnectedAPs.Add(ParseConnectedAP(capEl, ns));

                comm.SubNetworks.Add(sn);
            }

            return comm;
        }

        private ConnectedAP ParseConnectedAP(XElement el, XNamespace ns)
        {
            var cap = new ConnectedAP
            {
                IedName = Attr(el, "iedName"),
                ApName = Attr(el, "apName"),
                Desc = AttrOrNull(el, "desc"),
                RedProt = AttrOrNull(el, "redProt")
            };

            var addrEl = el.Element(ns + "Address");
            if (addrEl != null)
                cap.Address = ParseNetworkAddress(addrEl, ns);

            foreach (var gseEl in el.Elements(ns + "GSE"))
                cap.GSEs.Add(ParseGSE(gseEl, ns));

            foreach (var smvEl in el.Elements(ns + "SMV"))
                cap.SMVs.Add(ParseSMV(smvEl));

            foreach (var pcEl in el.Elements(ns + "PhysConn"))
                cap.PhysConns.Add(ParsePhysConn(pcEl, ns));

            return cap;
        }

        private NetworkAddress ParseNetworkAddress(XElement el, XNamespace ns)
        {
            var addr = new NetworkAddress();
            foreach (var p in el.Elements(ns + "P"))
                addr.PAddresses.Add(new PAddress { PType = Attr(p, "type"), Value = p.Value.NullIfEmpty() });
            return addr;
        }

        private GSE ParseGSE(XElement el, XNamespace ns)
        {
            var gse = new GSE
            {
                LdInst = Attr(el, "ldInst"),
                CbName = Attr(el, "cbName"),
                Desc = AttrOrNull(el, "desc")
            };

            var addrEl = el.Element(ns + "Address");
            if (addrEl != null) gse.Address = ParseNetworkAddress(addrEl, ns);

            var minEl = el.Element(ns + "MinTime");
            if (minEl != null) gse.MinTime = ParseDecimal(minEl.Value);

            var maxEl = el.Element(ns + "MaxTime");
            if (maxEl != null) gse.MaxTime = ParseDecimal(maxEl.Value);

            return gse;
        }

        private SMV ParseSMV(XElement el)
        {
            return new SMV
            {
                LdInst = Attr(el, "ldInst"),
                CbName = Attr(el, "cbName"),
                Desc = AttrOrNull(el, "desc")
            };
        }

        private PhysConn ParsePhysConn(XElement el, XNamespace ns)
        {
            var pc = new PhysConn
            {
                PhysConnType = Attr(el, "type", "Connection"),
                Desc = AttrOrNull(el, "desc")
            };
            foreach (var p in el.Elements(ns + "P"))
                pc.PValues.Add(new PPhysConn { PType = Attr(p, "type"), Value = p.Value.NullIfEmpty() });
            return pc;
        }

        // ????????????????????????????????????????????????????????????????????
        // IED
        // ????????????????????????????????????????????????????????????????????

        private IED ParseIed(XElement el, XNamespace ns)
        {
            var ied = new IED
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                IedType = AttrOrNull(el, "type"),
                Manufacturer = AttrOrNull(el, "manufacturer"),
                ConfigVersion = AttrOrNull(el, "configVersion"),
                OriginalSclVersion = AttrOrNull(el, "originalSclVersion"),
                OriginalSclRevision = AttrOrNull(el, "originalSclRevision"),
                OriginalSclRelease = AttrByte(el, "originalSclRelease", 1),
                EngRight = Attr(el, "engRight", "full"),
                Owner = AttrOrNull(el, "owner")
            };

            var svcEl = el.Element(ns + "Services");
            if (svcEl != null) ied.Services = ParseIedServices(svcEl, ns);

            foreach (var apEl in el.Elements(ns + "AccessPoint"))
                ied.AccessPoints.Add(ParseAccessPoint(apEl, ns));

            foreach (var kdcEl in el.Elements(ns + "KDC"))
                ied.KDCs.Add(new KDC { IedName = Attr(kdcEl, "iedName"), ApName = Attr(kdcEl, "apName") });

            return ied;
        }

        private AccessPoint ParseAccessPoint(XElement el, XNamespace ns)
        {
            var ap = new AccessPoint
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                Router = AttrBool(el, "router"),
                Clock = AttrBool(el, "clock"),
                Kdc = AttrBool(el, "kdc")
            };

            var srvEl = el.Element(ns + "Server");
            if (srvEl != null) ap.Server = ParseServer(srvEl, ns);

            var srvAtEl = el.Element(ns + "ServerAt");
            if (srvAtEl != null)
                ap.ServerAt = new ServerAt { ApName = Attr(srvAtEl, "apName"), Desc = AttrOrNull(srvAtEl, "desc") };

            foreach (var lnEl in el.Elements(ns + "LN"))
                ap.DirectLNs.Add(ParseLogicalNode(lnEl, ns));

            return ap;
        }

        private Server ParseServer(XElement el, XNamespace ns)
        {
            var srv = new Server
            {
                Timeout = AttrUInt(el, "timeout", 30),
                Desc = AttrOrNull(el, "desc")
            };

            var authEl = el.Element(ns + "Authentication");
            if (authEl != null)
            {
                srv.AuthNone = AttrBool(authEl, "none", defaultVal: true);
                srv.AuthPassword = AttrBool(authEl, "password");
                srv.AuthWeak = AttrBool(authEl, "weak");
                srv.AuthStrong = AttrBool(authEl, "strong");
                srv.AuthCertificate = AttrBool(authEl, "certificate");
            }

            foreach (var ldEl in el.Elements(ns + "LDevice"))
                srv.LDevices.Add(ParseLDevice(ldEl, ns));

            foreach (var assocEl in el.Elements(ns + "Association"))
                srv.Associations.Add(ParseAssociation(assocEl));

            return srv;
        }

        private LDevice ParseLDevice(XElement el, XNamespace ns)
        {
            var ld = new LDevice
            {
                Inst = Attr(el, "inst"),
                LdName = AttrOrNull(el, "ldName"),
                Desc = AttrOrNull(el, "desc"),
                AccessControl = AttrOrNull(el, "accessControl")
            };

            var ln0El = el.Element(ns + "LN0");
            if (ln0El != null) ld.LN0 = ParseLN0(ln0El, ns);

            foreach (var lnEl in el.Elements(ns + "LN"))
                ld.LNs.Add(ParseLogicalNode(lnEl, ns));

            return ld;
        }

        private LN0 ParseLN0(XElement el, XNamespace ns)
        {
            var ln0 = new LN0
            {
                LnType = Attr(el, "lnType"),
                LnClass = Attr(el, "lnClass", "LLN0"),
                Inst = Attr(el, "inst", ""),
                Desc = AttrOrNull(el, "desc")
            };

            foreach (var dsEl in el.Elements(ns + "DataSet"))
                ln0.DataSets.Add(ParseDataSet(dsEl, ns));

            foreach (var rcEl in el.Elements(ns + "ReportControl"))
                ln0.ReportControls.Add(ParseReportControl(rcEl, ns));

            foreach (var lcEl in el.Elements(ns + "LogControl"))
                ln0.LogControls.Add(ParseLogControl(lcEl, ns));

            foreach (var gEl in el.Elements(ns + "GSEControl"))
                ln0.GSEControls.Add(ParseGseControl(gEl, ns));

            foreach (var svEl in el.Elements(ns + "SampledValueControl"))
                ln0.SampledValueControls.Add(ParseSampledValueControl(svEl, ns));

            var scEl = el.Element(ns + "SettingControl");
            if (scEl != null)
                ln0.SettingControl = new SettingControl
                {
                    NumOfSGs = AttrUInt(scEl, "numOfSGs", 1),
                    ActSG = AttrUInt(scEl, "actSG", 1),
                    Desc = AttrOrNull(scEl, "desc")
                };

            foreach (var doiEl in el.Elements(ns + "DOI"))
                ln0.DOIs.Add(ParseDoi(doiEl, ns));

            var inpEl = el.Element(ns + "Inputs");
            if (inpEl != null) ln0.Inputs = ParseInputs(inpEl, ns);

            foreach (var logEl in el.Elements(ns + "Log"))
                ln0.Logs.Add(new SclLog { LogName = AttrOrNull(logEl, "name"), Desc = AttrOrNull(logEl, "desc") });

            return ln0;
        }

        private LogicalNode ParseLogicalNode(XElement el, XNamespace ns)
        {
            var ln = new LogicalNode
            {
                LnType = Attr(el, "lnType"),
                LnClass = Attr(el, "lnClass"),
                Inst = Attr(el, "inst"),
                Prefix = Attr(el, "prefix", ""),
                Desc = AttrOrNull(el, "desc")
            };

            foreach (var dsEl in el.Elements(ns + "DataSet"))
                ln.DataSets.Add(ParseDataSet(dsEl, ns));

            foreach (var rcEl in el.Elements(ns + "ReportControl"))
                ln.ReportControls.Add(ParseReportControl(rcEl, ns));

            foreach (var lcEl in el.Elements(ns + "LogControl"))
                ln.LogControls.Add(ParseLogControl(lcEl, ns));

            foreach (var doiEl in el.Elements(ns + "DOI"))
                ln.DOIs.Add(ParseDoi(doiEl, ns));

            var inpEl = el.Element(ns + "Inputs");
            if (inpEl != null) ln.Inputs = ParseInputs(inpEl, ns);

            foreach (var logEl in el.Elements(ns + "Log"))
                ln.Logs.Add(new SclLog { LogName = AttrOrNull(logEl, "name"), Desc = AttrOrNull(logEl, "desc") });

            return ln;
        }

        private DataSet ParseDataSet(XElement el, XNamespace ns)
        {
            var ds = new DataSet { Name = Attr(el, "name"), Desc = AttrOrNull(el, "desc") };
            foreach (var fcdaEl in el.Elements(ns + "FCDA"))
            {
                ds.FCDAs.Add(new FCDA
                {
                    LdInst = AttrOrNull(fcdaEl, "ldInst"),
                    Prefix = Attr(fcdaEl, "prefix", ""),
                    LnClass = AttrOrNull(fcdaEl, "lnClass"),
                    LnInst = AttrOrNull(fcdaEl, "lnInst"),
                    DoName = AttrOrNull(fcdaEl, "doName"),
                    DaName = AttrOrNull(fcdaEl, "daName"),
                    Fc = Attr(fcdaEl, "fc"),
                    Ix = AttrUIntOrNull(fcdaEl, "ix")
                });
            }
            return ds;
        }

        private ReportControl ParseReportControl(XElement el, XNamespace ns)
        {
            var rc = new ReportControl
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                DatSet = AttrOrNull(el, "datSet"),
                RptID = AttrOrNull(el, "rptID"),
                ConfRev = AttrUInt(el, "confRev", 0),
                Buffered = AttrBool(el, "buffered"),
                BufTime = AttrUInt(el, "bufTime", 0),
                Indexed = AttrBool(el, "indexed", true),
                IntgPd = AttrUInt(el, "intgPd", 0)
            };

            var trgEl = el.Element(ns + "TrgOps");
            if (trgEl != null)
            {
                rc.TrgOpsDchg = AttrBool(trgEl, "dchg");
                rc.TrgOpsQchg = AttrBool(trgEl, "qchg");
                rc.TrgOpsDupd = AttrBool(trgEl, "dupd");
                rc.TrgOpsPeriod = AttrBool(trgEl, "period");
                rc.TrgOpsGi = AttrBool(trgEl, "gi", true);
            }

            var optEl = el.Element(ns + "OptFields");
            if (optEl != null)
            {
                rc.OptSeqNum = AttrBool(optEl, "seqNum");
                rc.OptTimeStamp = AttrBool(optEl, "timeStamp");
                rc.OptDataSet = AttrBool(optEl, "dataSet");
                rc.OptReasonCode = AttrBool(optEl, "reasonCode");
                rc.OptDataRef = AttrBool(optEl, "dataRef");
                rc.OptEntryID = AttrBool(optEl, "entryID");
                rc.OptConfigRef = AttrBool(optEl, "configRef");
                rc.OptBufOvfl = AttrBool(optEl, "bufOvfl", true);
            }

            var rptEnabledEl = el.Element(ns + "RptEnabled");
            if (rptEnabledEl != null)
            {
                var rptEnabled = new RptEnabled
                {
                    Max = AttrUInt(rptEnabledEl, "max", 1),
                    Desc = AttrOrNull(rptEnabledEl, "desc")
                };
                foreach (var clEl in rptEnabledEl.Elements(ns + "ClientLN"))
                {
                    rptEnabled.ClientLNs.Add(new ClientLN
                    {
                        IedName = Attr(clEl, "iedName"),
                        LdInst = Attr(clEl, "ldInst"),
                        Prefix = Attr(clEl, "prefix", ""),
                        LnClass = Attr(clEl, "lnClass"),
                        LnInst = Attr(clEl, "lnInst"),
                        ApRef = AttrOrNull(clEl, "apRef"),
                        ClientDesc = AttrOrNull(clEl, "desc")
                    });
                }
                rc.RptEnabled = rptEnabled;
            }

            return rc;
        }

        private LogControl ParseLogControl(XElement el, XNamespace ns)
        {
            var lc = new LogControl
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                DatSet = AttrOrNull(el, "datSet"),
                LdInst = AttrOrNull(el, "ldInst"),
                Prefix = Attr(el, "prefix", ""),
                LnClass = Attr(el, "lnClass", "LLN0"),
                LnInst = AttrOrNull(el, "lnInst"),
                LogName = Attr(el, "logName"),
                LogEna = AttrBool(el, "logEna", true),
                ReasonCode = AttrBool(el, "reasonCode", true),
                BufTime = AttrUInt(el, "bufTime", 0),
                IntgPd = AttrUInt(el, "intgPd", 0)
            };

            var trgEl = el.Element(ns + "TrgOps");
            if (trgEl != null)
            {
                lc.TrgOpsDchg = AttrBool(trgEl, "dchg");
                lc.TrgOpsQchg = AttrBool(trgEl, "qchg");
                lc.TrgOpsDupd = AttrBool(trgEl, "dupd");
                lc.TrgOpsPeriod = AttrBool(trgEl, "period");
                lc.TrgOpsGi = AttrBool(trgEl, "gi", true);
            }
            return lc;
        }

        private GSEControl ParseGseControl(XElement el, XNamespace ns)
        {
            var gc = new GSEControl
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                DatSet = AttrOrNull(el, "datSet"),
                AppID = Attr(el, "appID"),
                GseType = Attr(el, "type", "GOOSE"),
                FixedOffs = AttrBool(el, "fixedOffs"),
                SecurityEnable = Attr(el, "securityEnable", "None"),
                ConfRev = AttrUIntOrNull(el, "confRev")
            };

            foreach (var iedNameEl in el.Elements(ns + "IEDName"))
                gc.IEDNames.Add(ParseControlIedName(iedNameEl));

            return gc;
        }

        private SampledValueControl ParseSampledValueControl(XElement el, XNamespace ns)
        {
            var sv = new SampledValueControl
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                DatSet = AttrOrNull(el, "datSet"),
                SmvID = Attr(el, "smvID"),
                Multicast = AttrBool(el, "multicast", true),
                SmpRate = AttrUInt(el, "smpRate", 0),
                NofASDU = AttrUInt(el, "nofASDU", 0),
                SmpMod = Attr(el, "smpMod", "SmpPerPeriod"),
                SecurityEnable = Attr(el, "securityEnable", "None"),
                ConfRev = AttrUIntOrNull(el, "confRev")
            };

            var smvOptEl = el.Element(ns + "SmvOpts");
            if (smvOptEl != null)
            {
                sv.SmvOptRefreshTime = AttrBool(smvOptEl, "refreshTime");
                sv.SmvOptSampleSynchronized = AttrBool(smvOptEl, "sampleSynchronized", true);
                sv.SmvOptSampleRate = AttrBool(smvOptEl, "sampleRate");
                sv.SmvOptDataSet = AttrBool(smvOptEl, "dataSet");
                sv.SmvOptSecurity = AttrBool(smvOptEl, "security");
                sv.SmvOptTimestamp = AttrBool(smvOptEl, "timestamp");
                sv.SmvOptSynchSourceId = AttrBool(smvOptEl, "synchSourceId");
            }

            foreach (var iedNameEl in el.Elements(ns + "IEDName"))
                sv.IEDNames.Add(ParseControlIedName(iedNameEl));

            return sv;
        }

        private static ControlIEDName ParseControlIedName(XElement el) => new()
        {
            IedName = el.Value.Trim(),
            ApRef = AttrOrNullS(el, "apRef"),
            LdInst = AttrOrNullS(el, "ldInst"),
            Prefix = AttrOrNullS(el, "prefix"),
            LnClass = AttrOrNullS(el, "lnClass"),
            LnInst = AttrOrNullS(el, "lnInst")
        };

        private static Association ParseAssociation(XElement el) => new()
        {
            IedName = Attr(el, "iedName"),
            LdInst = Attr(el, "ldInst"),
            Prefix = (string?)el.Attribute("prefix") ?? "",
            LnClass = Attr(el, "lnClass"),
            LnInst = Attr(el, "lnInst"),
            Kind = Attr(el, "kind"),
            AssociationID = AttrOrNullS(el, "associationID"),
            AssociationDesc = AttrOrNullS(el, "desc")
        };

        // ????????????????????????????????????????????????????????????????????
        // DOI / SDI / DAI
        // ????????????????????????????????????????????????????????????????????

        private DOI ParseDoi(XElement el, XNamespace ns)
        {
            var doi = new DOI
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                Ix = AttrUIntOrNull(el, "ix"),
                AccessControl = AttrOrNull(el, "accessControl")
            };
            foreach (var sdiEl in el.Elements(ns + "SDI"))
                doi.SDIs.Add(ParseSdi(sdiEl, ns));
            foreach (var daiEl in el.Elements(ns + "DAI"))
                doi.DAIs.Add(ParseDai(daiEl, ns));
            return doi;
        }

        private SDI ParseSdi(XElement el, XNamespace ns)
        {
            var sdi = new SDI
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                Ix = AttrUIntOrNull(el, "ix"),
                SAddr = AttrOrNull(el, "sAddr")
            };
            foreach (var childSdi in el.Elements(ns + "SDI"))
                sdi.ChildSDIs.Add(ParseSdi(childSdi, ns));
            foreach (var daiEl in el.Elements(ns + "DAI"))
                sdi.DAIs.Add(ParseDai(daiEl, ns));
            return sdi;
        }

        private DAI ParseDai(XElement el, XNamespace ns)
        {
            var dai = new DAI
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                SAddr = AttrOrNull(el, "sAddr"),
                ValKind = AttrOrNull(el, "valKind"),
                Ix = AttrUIntOrNull(el, "ix"),
                ValImport = AttrBoolOrNull(el, "valImport")
            };
            foreach (var valEl in el.Elements(ns + "Val"))
                dai.Vals.Add(new Val { SGroup = AttrUIntOrNull(valEl, "sGroup"), Value = valEl.Value.NullIfEmpty() });
            return dai;
        }

        private Inputs ParseInputs(XElement el, XNamespace ns)
        {
            var inputs = new Inputs { Desc = AttrOrNull(el, "desc") };
            foreach (var extEl in el.Elements(ns + "ExtRef"))
            {
                inputs.ExtRefs.Add(new ExtRef
                {
                    ExtRefDesc = AttrOrNull(extEl, "desc"),
                    IedName = AttrOrNull(extEl, "iedName"),
                    LdInst = AttrOrNull(extEl, "ldInst"),
                    Prefix = AttrOrNull(extEl, "prefix"),
                    LnClass = AttrOrNull(extEl, "lnClass"),
                    LnInst = AttrOrNull(extEl, "lnInst"),
                    DoName = AttrOrNull(extEl, "doName"),
                    DaName = AttrOrNull(extEl, "daName"),
                    IntAddr = AttrOrNull(extEl, "intAddr"),
                    ServiceType = AttrOrNull(extEl, "serviceType"),
                    SrcLDInst = AttrOrNull(extEl, "srcLDInst"),
                    SrcPrefix = AttrOrNull(extEl, "srcPrefix"),
                    SrcLNClass = AttrOrNull(extEl, "srcLNClass"),
                    SrcLNInst = AttrOrNull(extEl, "srcLNInst"),
                    SrcCBName = AttrOrNull(extEl, "srcCBName"),
                    PServT = AttrOrNull(extEl, "pServT"),
                    PLN = AttrOrNull(extEl, "pLN"),
                    PDO = AttrOrNull(extEl, "pDO"),
                    PDA = AttrOrNull(extEl, "pDA")
                });
            }
            return inputs;
        }

        // ????????????????????????????????????????????????????????????????????
        // IED Services (flat attribute mapping)
        // ????????????????????????????????????????????????????????????????????

        private IedServices ParseIedServices(XElement el, XNamespace ns)
        {
            var svc = new IedServices
            {
                NameLength = AttrInt(el, "nameLength", 32),
                DynAssociation = AttrBoolOrNull(el, "dynAssociation"),
                GetDirectory = AttrBoolOrNull(el, "getDirectory"),
                GetDataObjectDefinition = AttrBoolOrNull(el, "getDataObjectDefinition"),
                DataObjectDirectory = AttrBoolOrNull(el, "dataObjectDirectory"),
                GetDataSetValue = AttrBoolOrNull(el, "getDataSetValue"),
                SetDataSetValue = AttrBoolOrNull(el, "setDataSetValue"),
                DataSetDirectory = AttrBoolOrNull(el, "dataSetDirectory"),
                ReadWrite = AttrBoolOrNull(el, "readWrite"),
                TimerActivatedControl = AttrBoolOrNull(el, "timerActivatedControl"),
                GetCBValues = AttrBoolOrNull(el, "getCBValues"),
                GSEDir = AttrBoolOrNull(el, "GSEDir"),
                ConfLdName = AttrBoolOrNull(el, "confLdName"),
                FileHandling = AttrBoolOrNull(el, "fileHandling"),
                GOOSEMax = AttrUIntOrNull(el, "gooseMax"),
                GSSEMax = AttrUIntOrNull(el, "gsseMax")
            };

            // ConfDataSet
            var cdEl = el.Element(ns + "ConfDataSet");
            if (cdEl != null)
            {
                svc.ConfDataSetMax = AttrUIntOrNull(cdEl, "max");
                svc.ConfDataSetMaxAttributes = AttrUIntOrNull(cdEl, "maxAttributes");
                svc.ConfDataSetModify = AttrBool(cdEl, "modify", true);
            }

            // DynDataSet
            var ddEl = el.Element(ns + "DynDataSet");
            if (ddEl != null)
            {
                svc.DynDataSetMax = AttrUIntOrNull(ddEl, "max");
                svc.DynDataSetMaxAttributes = AttrUIntOrNull(ddEl, "maxAttributes");
            }

            // ConfReportControl
            var crcEl = el.Element(ns + "ConfReportControl");
            if (crcEl != null)
            {
                svc.ConfReportControlMax = AttrUIntOrNull(crcEl, "max");
                svc.ConfReportControlBufMode = Attr(crcEl, "bufMode", "both");
                svc.ConfReportControlBufConf = AttrBool(crcEl, "bufConf");
                svc.ConfReportControlMaxBuf = AttrUIntOrNull(crcEl, "maxBuf");
            }

            // ConfLogControl
            var clcEl = el.Element(ns + "ConfLogControl");
            if (clcEl != null)
                svc.ConfLogControlMax = AttrUIntOrNull(clcEl, "max");

            // ReportSettings
            var rsEl = el.Element(ns + "ReportSettings");
            if (rsEl != null)
            {
                svc.ReportSettingsCbName = Attr(rsEl, "cbName", "Fix");
                svc.ReportSettingsDatSet = Attr(rsEl, "datSet", "Fix");
                svc.ReportSettingsRptID = Attr(rsEl, "rptID", "Fix");
                svc.ReportSettingsOptFields = Attr(rsEl, "optFields", "Fix");
                svc.ReportSettingsBufTime = Attr(rsEl, "bufTime", "Fix");
                svc.ReportSettingsTrgOps = Attr(rsEl, "trgOps", "Fix");
                svc.ReportSettingsIntgPd = Attr(rsEl, "intgPd", "Fix");
                svc.ReportSettingsResvTms = AttrBool(rsEl, "resvTms");
                svc.ReportSettingsOwner = AttrBool(rsEl, "owner");
            }

            // LogSettings
            var lsEl = el.Element(ns + "LogSettings");
            if (lsEl != null)
            {
                svc.LogSettingsCbName = Attr(lsEl, "cbName", "Fix");
                svc.LogSettingsDatSet = Attr(lsEl, "datSet", "Fix");
                svc.LogSettingsLogEna = Attr(lsEl, "logEna", "Fix");
                svc.LogSettingsTrgOps = Attr(lsEl, "trgOps", "Fix");
                svc.LogSettingsIntgPd = Attr(lsEl, "intgPd", "Fix");
            }

            // GSESettings
            var gsEl = el.Element(ns + "GSESettings");
            if (gsEl != null)
            {
                svc.GSESettingsCbName = Attr(gsEl, "cbName", "Fix");
                svc.GSESettingsDatSet = Attr(gsEl, "datSet", "Fix");
                svc.GSESettingsAppID = Attr(gsEl, "appID", "Fix");
                svc.GSESettingsDataLabel = Attr(gsEl, "dataLabel", "Fix");
                svc.GSESettingsKdaParticipant = AttrBool(gsEl, "kdaParticipant");
                svc.GSESettingsMcSecuritySignature = AttrBool(gsEl, "mcSecuritySignature");
                svc.GSESettingsMcSecurityEncryption = AttrBool(gsEl, "mcSecurityEncryption");
            }

            // SMVSettings
            var smEl = el.Element(ns + "SMVSettings");
            if (smEl != null)
            {
                svc.SMVSettingsCbName = Attr(smEl, "cbName", "Fix");
                svc.SMVSettingsDatSet = Attr(smEl, "datSet", "Fix");
                svc.SMVSettingsSvID = Attr(smEl, "svID", "Fix");
                svc.SMVSettingsOptFields = Attr(smEl, "optFields", "Fix");
                svc.SMVSettingsSmpRate = Attr(smEl, "smpRate", "Fix");
                svc.SMVSettingsSamplesPerSec = AttrBool(smEl, "samplesPerSec");
                svc.SMVSettingsPdcTimeStamp = AttrBool(smEl, "pdcTimeStamp");
                svc.SMVSettingsSynchSrcId = AttrBool(smEl, "synchSrcId");
                svc.SMVSettingsNofASDU = Attr(smEl, "nofASDU", "Fix");
                svc.SMVSettingsKdaParticipant = AttrBool(smEl, "kdaParticipant");
                svc.SMVSettingsMcSecuritySignature = AttrBool(smEl, "mcSecuritySignature");
                svc.SMVSettingsMcSecurityEncryption = AttrBool(smEl, "mcSecurityEncryption");
            }

            // GOOSE
            var goEl = el.Element(ns + "GOOSE");
            if (goEl != null)
            {
                svc.GOOSEMax = AttrUIntOrNull(goEl, "max");
                svc.GOOSEFixedOffs = AttrBool(goEl, "fixedOffs");
                svc.GOOSEGoose = AttrBool(goEl, "goose", true);
                svc.GOOSE_rGOOSE = AttrBool(goEl, "rGOOSE");
            }

            // GSSE
            var gsseEl = el.Element(ns + "GSSE");
            if (gsseEl != null) svc.GSSEMax = AttrUIntOrNull(gsseEl, "max");

            // SMVsc
            var svscEl = el.Element(ns + "SMVsc");
            if (svscEl != null)
            {
                svc.SMVscMax = AttrUIntOrNull(svscEl, "max");
                svc.SMVscDelivery = Attr(svscEl, "delivery", "multicast");
                svc.SMVscDeliveryConf = AttrBool(svscEl, "deliveryConf");
                svc.SMVscSv = AttrBool(svscEl, "sv", true);
                svc.SMVsc_rSV = AttrBool(svscEl, "rSV");
            }

            // FileHandling element
            var fhEl = el.Element(ns + "FileHandling");
            if (fhEl != null)
            {
                svc.FileHandling = true;
                svc.FileHandlingMms = AttrBool(fhEl, "mms", true);
                svc.FileHandlingFtp = AttrBool(fhEl, "ftp");
                svc.FileHandlingFtps = AttrBool(fhEl, "ftps");
            }

            // ConfLNs
            var confLnsEl = el.Element(ns + "ConfLNs");
            if (confLnsEl != null)
            {
                svc.ConfLNsFixPrefix = AttrBool(confLnsEl, "fixPrefix");
                svc.ConfLNsFixLnInst = AttrBool(confLnsEl, "fixLnInst");
            }

            // ClientServices
            var csEl = el.Element(ns + "ClientServices");
            if (csEl != null)
            {
                svc.ClientServicesGoose = AttrBoolOrNull(csEl, "goose");
                svc.ClientServicesGsse = AttrBoolOrNull(csEl, "gsse");
                svc.ClientServicesBufReport = AttrBoolOrNull(csEl, "bufReport");
                svc.ClientServicesUnbufReport = AttrBoolOrNull(csEl, "unbufReport");
                svc.ClientServicesReadLog = AttrBoolOrNull(csEl, "readLog");
                svc.ClientServicesSv = AttrBoolOrNull(csEl, "sv");
                svc.ClientServicesSupportsLdName = AttrBoolOrNull(csEl, "supportsLdName");
                svc.ClientServicesMaxAttributes = AttrUIntOrNull(csEl, "maxAttributes");
                svc.ClientServicesMaxReports = AttrUIntOrNull(csEl, "maxReports");
                svc.ClientServicesMaxGOOSE = AttrUIntOrNull(csEl, "maxGOOSE");
                svc.ClientServicesMaxSMV = AttrUIntOrNull(csEl, "maxSMV");
                svc.ClientServices_rGOOSE = AttrBoolOrNull(csEl, "rGOOSE");
                svc.ClientServices_rSV = AttrBoolOrNull(csEl, "rSV");
                svc.ClientServicesNoIctBinding = AttrBoolOrNull(csEl, "noIctBinding");
            }

            // SupSubscription
            var ssEl = el.Element(ns + "SupSubscription");
            if (ssEl != null)
            {
                svc.SupSubscriptionMaxGo = AttrUIntOrNull(ssEl, "maxGo");
                svc.SupSubscriptionMaxSv = AttrUIntOrNull(ssEl, "maxSv");
            }

            // RedProt
            var rpEl = el.Element(ns + "RedProt");
            if (rpEl != null)
            {
                svc.RedProtHsr = AttrBoolOrNull(rpEl, "hsr");
                svc.RedProtPrp = AttrBoolOrNull(rpEl, "prp");
                svc.RedProtRstp = AttrBoolOrNull(rpEl, "rstp");
            }

            // TimeSyncProt
            var tspEl = el.Element(ns + "TimeSyncProt");
            if (tspEl != null)
            {
                svc.TimeSyncProtSntp = AttrBoolOrNull(tspEl, "sntp");
                svc.TimeSyncProt_iec61850_9_3 = AttrBoolOrNull(tspEl, "iec61850-9-3");
                svc.TimeSyncProt_c37_238 = AttrBoolOrNull(tspEl, "c37-238");
                svc.TimeSyncProtOther = AttrBoolOrNull(tspEl, "other");
            }

            // CommProt
            var cpEl = el.Element(ns + "CommProt");
            if (cpEl != null)
                svc.CommProtIpv6 = AttrBoolOrNull(cpEl, "ipv6");

            // SettingGroups
            var sgEl = el.Element(ns + "SettingGroups");
            if (sgEl != null)
            {
                var sgEditEl = sgEl.Element(ns + "SGEdit");
                svc.SettingGroupsSGEdit = sgEditEl != null;
                if (sgEditEl != null)
                    svc.SettingGroupsSGEditResvTms = AttrBoolOrNull(sgEditEl, "resvTms");

                var confSgEl = sgEl.Element(ns + "ConfSG");
                svc.SettingGroupsConfSG = confSgEl != null;
                if (confSgEl != null)
                    svc.SettingGroupsConfSGResvTms = AttrBoolOrNull(confSgEl, "resvTms");
            }

            return svc;
        }

        // ????????????????????????????????????????????????????????????????????
        // Substation hierarchy
        // ????????????????????????????????????????????????????????????????????

        private Substation ParseSubstation(XElement el, XNamespace ns)
        {
            var sub = new Substation
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc")
            };
            ApplyLNodeContainerChildren(sub, el, ns);
            foreach (var vlEl in el.Elements(ns + "VoltageLevel"))
                sub.VoltageLevels.Add(ParseVoltageLevel(vlEl, ns));
            foreach (var fEl in el.Elements(ns + "Function"))
                sub.Functions.Add(ParseFunction(fEl, ns));
            return sub;
        }

        private VoltageLevel ParseVoltageLevel(XElement el, XNamespace ns)
        {
            var vl = new VoltageLevel
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                NomFreq = ParseDecimalOrNull(AttrOrNull(el, "nomFreq")),
                NumPhases = AttrByteOrNull(el, "numPhases")
            };
            var voltEl = el.Element(ns + "Voltage");
            if (voltEl != null)
            {
                vl.VoltageValue = ParseDecimal(voltEl.Value);
                vl.VoltageUnit = AttrOrNull(voltEl, "unit");
                vl.VoltageMultiplier = AttrOrNull(voltEl, "multiplier");
            }
            ApplyLNodeContainerChildren(vl, el, ns);
            foreach (var bayEl in el.Elements(ns + "Bay"))
                vl.Bays.Add(ParseBay(bayEl, ns));
            foreach (var fEl in el.Elements(ns + "Function"))
                vl.Functions.Add(ParseFunction(fEl, ns));
            return vl;
        }

        private Bay ParseBay(XElement el, XNamespace ns)
        {
            var bay = new Bay { Name = Attr(el, "name"), Desc = AttrOrNull(el, "desc") };
            ApplyLNodeContainerChildren(bay, el, ns);
            foreach (var ceEl in el.Elements(ns + "ConductingEquipment"))
                bay.ConductingEquipments.Add(ParseConductingEquipment(ceEl, ns));
            foreach (var cnEl in el.Elements(ns + "ConnectivityNode"))
                bay.ConnectivityNodes.Add(new ConnectivityNode { Name = Attr(cnEl, "name"), PathName = Attr(cnEl, "pathName"), Desc = AttrOrNull(cnEl, "desc") });
            foreach (var fEl in el.Elements(ns + "Function"))
                bay.Functions.Add(ParseFunction(fEl, ns));
            return bay;
        }

        private ConductingEquipment ParseConductingEquipment(XElement el, XNamespace ns)
        {
            var ce = new ConductingEquipment
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                EquipmentType = Attr(el, "type"),
                Virtual = AttrBool(el, "virtual")
            };
            ApplyLNodeContainerChildren(ce, el, ns);
            foreach (var tEl in el.Elements(ns + "Terminal"))
                ce.Terminals.Add(ParseTerminal(tEl));
            foreach (var seEl in el.Elements(ns + "SubEquipment"))
                ce.SubEquipments.Add(ParseSubEquipment(seEl, ns));
            foreach (var efEl in el.Elements(ns + "EqFunction"))
                ce.EqFunctions.Add(ParseEqFunction(efEl, ns));
            return ce;
        }

        private static Terminal ParseTerminal(XElement el) => new()
        {
            TerminalName = (string?)el.Attribute("name") ?? "",
            ConnectivityNode = Attr(el, "connectivityNode"),
            CNodeName = Attr(el, "cNodeName"),
            ProcessName = AttrOrNullS(el, "processName"),
            SubstationName = AttrOrNullS(el, "substationName"),
            VoltageLevelName = AttrOrNullS(el, "voltageLevelName"),
            BayName = AttrOrNullS(el, "bayName"),
            LineName = AttrOrNullS(el, "lineName"),
            Desc = AttrOrNullS(el, "desc")
        };

        private SubEquipment ParseSubEquipment(XElement el, XNamespace ns)
        {
            var se = new SubEquipment
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                Phase = Attr(el, "phase", "none"),
                Virtual = AttrBool(el, "virtual")
            };
            ApplyLNodeContainerChildren(se, el, ns);
            foreach (var efEl in el.Elements(ns + "EqFunction"))
                se.EqFunctions.Add(ParseEqFunction(efEl, ns));
            return se;
        }

        private EqFunction ParseEqFunction(XElement el, XNamespace ns)
        {
            var ef = new EqFunction
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                EqFunctionType = AttrOrNull(el, "type")
            };
            ApplyLNodeContainerChildren(ef, el, ns);
            foreach (var geEl in el.Elements(ns + "GeneralEquipment"))
                ef.GeneralEquipments.Add(ParseGeneralEquipment(geEl, ns));
            foreach (var esfEl in el.Elements(ns + "EqSubFunction"))
                ef.EqSubFunctions.Add(ParseEqSubFunction(esfEl, ns));
            return ef;
        }

        private EqSubFunction ParseEqSubFunction(XElement el, XNamespace ns)
        {
            var esf = new EqSubFunction
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                EqSubFunctionType = AttrOrNull(el, "type")
            };
            ApplyLNodeContainerChildren(esf, el, ns);
            foreach (var geEl in el.Elements(ns + "GeneralEquipment"))
                esf.GeneralEquipments.Add(ParseGeneralEquipment(geEl, ns));
            foreach (var childEl in el.Elements(ns + "EqSubFunction"))
                esf.EqSubFunctions.Add(ParseEqSubFunction(childEl, ns));
            return esf;
        }

        private GeneralEquipment ParseGeneralEquipment(XElement el, XNamespace ns)
        {
            var ge = new GeneralEquipment
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                EquipmentType = Attr(el, "type"),
                Virtual = AttrBool(el, "virtual")
            };
            ApplyLNodeContainerChildren(ge, el, ns);
            foreach (var efEl in el.Elements(ns + "EqFunction"))
                ge.EqFunctions.Add(ParseEqFunction(efEl, ns));
            return ge;
        }

        private Function ParseFunction(XElement el, XNamespace ns)
        {
            var fn = new Function
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                FunctionType = AttrOrNull(el, "type")
            };
            ApplyLNodeContainerChildren(fn, el, ns);
            foreach (var sfEl in el.Elements(ns + "SubFunction"))
                fn.SubFunctions.Add(ParseSubFunction(sfEl, ns));
            foreach (var geEl in el.Elements(ns + "GeneralEquipment"))
                fn.GeneralEquipments.Add(ParseGeneralEquipment(geEl, ns));
            foreach (var ceEl in el.Elements(ns + "ConductingEquipment"))
                fn.ConductingEquipments.Add(ParseConductingEquipment(ceEl, ns));
            return fn;
        }

        private SubFunction ParseSubFunction(XElement el, XNamespace ns)
        {
            var sf = new SubFunction
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                SubFunctionType = AttrOrNull(el, "type")
            };
            ApplyLNodeContainerChildren(sf, el, ns);
            foreach (var childEl in el.Elements(ns + "SubFunction"))
                sf.SubFunctions.Add(ParseSubFunction(childEl, ns));
            foreach (var geEl in el.Elements(ns + "GeneralEquipment"))
                sf.GeneralEquipments.Add(ParseGeneralEquipment(geEl, ns));
            foreach (var ceEl in el.Elements(ns + "ConductingEquipment"))
                sf.ConductingEquipments.Add(ParseConductingEquipment(ceEl, ns));
            return sf;
        }

        private Line ParseLine(XElement el, XNamespace ns)
        {
            var line = new Line
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                LineType = AttrOrNull(el, "type"),
                NomFreq = ParseDecimalOrNull(AttrOrNull(el, "nomFreq")),
                NumPhases = AttrByteOrNull(el, "numPhases")
            };
            var voltEl = el.Element(ns + "Voltage");
            if (voltEl != null)
            {
                line.VoltageValue = ParseDecimal(voltEl.Value);
                line.VoltageMultiplier = AttrOrNull(voltEl, "multiplier");
            }
            ApplyLNodeContainerChildren(line, el, ns);
            foreach (var geEl in el.Elements(ns + "GeneralEquipment"))
                line.GeneralEquipments.Add(ParseGeneralEquipment(geEl, ns));
            foreach (var fEl in el.Elements(ns + "Function"))
                line.Functions.Add(ParseFunction(fEl, ns));
            foreach (var ceEl in el.Elements(ns + "ConductingEquipment"))
                line.ConductingEquipments.Add(ParseConductingEquipment(ceEl, ns));
            foreach (var cnEl in el.Elements(ns + "ConnectivityNode"))
                line.ConnectivityNodes.Add(new ConnectivityNode { Name = Attr(cnEl, "name"), PathName = Attr(cnEl, "pathName"), Desc = AttrOrNull(cnEl, "desc") });
            return line;
        }

        private Process ParseProcess(XElement el, XNamespace ns)
        {
            var proc = new Process
            {
                Name = Attr(el, "name"),
                Desc = AttrOrNull(el, "desc"),
                ProcessType = AttrOrNull(el, "type")
            };
            ApplyLNodeContainerChildren(proc, el, ns);
            foreach (var geEl in el.Elements(ns + "GeneralEquipment"))
                proc.GeneralEquipments.Add(ParseGeneralEquipment(geEl, ns));
            foreach (var fEl in el.Elements(ns + "Function"))
                proc.Functions.Add(ParseFunction(fEl, ns));
            foreach (var ceEl in el.Elements(ns + "ConductingEquipment"))
                proc.ConductingEquipments.Add(ParseConductingEquipment(ceEl, ns));
            foreach (var subEl in el.Elements(ns + "Substation"))
                proc.Substations.Add(ParseSubstation(subEl, ns));
            foreach (var lineEl in el.Elements(ns + "Line"))
                proc.Lines.Add(ParseLine(lineEl, ns));
            foreach (var childEl in el.Elements(ns + "Process"))
                proc.SubProcesses.Add(ParseProcess(childEl, ns));
            return proc;
        }

        private void ApplyLNodeContainerChildren(LNodeContainer container, XElement el, XNamespace ns)
        {
            foreach (var lnEl in el.Elements(ns + "LNode"))
            {
                container.LNodes.Add(new LNode
                {
                    IedName = AttrOrNull(lnEl, "iedName"),
                    LdInst = AttrOrNull(lnEl, "ldInst"),
                    Prefix = AttrOrNull(lnEl, "prefix"),
                    LnClass = Attr(lnEl, "lnClass"),
                    LnInst = AttrOrNull(lnEl, "lnInst"),
                    LnType = AttrOrNull(lnEl, "lnType"),
                    Desc = AttrOrNull(lnEl, "desc")
                });
            }
        }

        // ????????????????????????????????????????????????????????????????????
        // DataTypeTemplates
        // ????????????????????????????????????????????????????????????????????

        private DataTypeTemplates? ParseDataTypeTemplates(XElement root, XNamespace ns)
        {
            var el = root.Element(ns + "DataTypeTemplates");
            if (el == null) return null;

            var dtt = new DataTypeTemplates();

            foreach (var lntEl in el.Elements(ns + "LNodeType"))
            {
                var lnt = new LNodeType
                {
                    SclId = Attr(lntEl, "id"),
                    Desc = AttrOrNull(lntEl, "desc"),
                    IedType = Attr(lntEl, "iedType", ""),
                    LnClass = Attr(lntEl, "lnClass")
                };
                foreach (var doEl in lntEl.Elements(ns + "DO"))
                {
                    lnt.DOs.Add(new DataObject
                    {
                        Name = Attr(doEl, "name"),
                        DoType = Attr(doEl, "type"),
                        AccessControl = AttrOrNull(doEl, "accessControl"),
                        Transient = AttrBool(doEl, "transient"),
                        Desc = AttrOrNull(doEl, "desc")
                    });
                }
                dtt.LNodeTypes.Add(lnt);
            }

            foreach (var dotEl in el.Elements(ns + "DOType"))
            {
                var dot = new DOType
                {
                    SclId = Attr(dotEl, "id"),
                    Desc = AttrOrNull(dotEl, "desc"),
                    IedType = Attr(dotEl, "iedType", ""),
                    Cdc = Attr(dotEl, "cdc")
                };
                foreach (var sdoEl in dotEl.Elements(ns + "SDO"))
                {
                    dot.SDOs.Add(new SDO
                    {
                        Name = Attr(sdoEl, "name"),
                        SdoType = Attr(sdoEl, "type"),
                        Count = Attr(sdoEl, "count", "0"),
                        Desc = AttrOrNull(sdoEl, "desc")
                    });
                }
                foreach (var daEl in dotEl.Elements(ns + "DA"))
                {
                    var da = new DataAttribute
                    {
                        Name = Attr(daEl, "name"),
                        Desc = AttrOrNull(daEl, "desc"),
                        SAddr = AttrOrNull(daEl, "sAddr"),
                        BType = Attr(daEl, "bType"),
                        ValKind = Attr(daEl, "valKind", "Set"),
                        AttributeType = AttrOrNull(daEl, "type"),
                        Count = Attr(daEl, "count", "0"),
                        ValImport = AttrBool(daEl, "valImport"),
                        Dchg = AttrBool(daEl, "dchg"),
                        Qchg = AttrBool(daEl, "qchg"),
                        Dupd = AttrBool(daEl, "dupd"),
                        Fc = Attr(daEl, "fc")
                    };
                    foreach (var valEl in daEl.Elements(ns + "Val"))
                        da.Vals.Add(new Val { SGroup = AttrUIntOrNull(valEl, "sGroup"), Value = valEl.Value.NullIfEmpty() });
                    foreach (var protNsEl in daEl.Elements(ns + "ProtNs"))
                        da.ProtNsEntries.Add(new ProtNs { ProtNsType = Attr(protNsEl, "type", "8-MMS"), NamespaceValue = protNsEl.Value.NullIfEmpty() });
                    dot.DAs.Add(da);
                }
                dtt.DOTypes.Add(dot);
            }

            foreach (var datEl in el.Elements(ns + "DAType"))
            {
                var dat = new DAType
                {
                    SclId = Attr(datEl, "id"),
                    Desc = AttrOrNull(datEl, "desc"),
                    IedType = Attr(datEl, "iedType", "")
                };
                foreach (var bdaEl in datEl.Elements(ns + "BDA"))
                {
                    var bda = new BDA
                    {
                        Name = Attr(bdaEl, "name"),
                        Desc = AttrOrNull(bdaEl, "desc"),
                        SAddr = AttrOrNull(bdaEl, "sAddr"),
                        BType = Attr(bdaEl, "bType"),
                        ValKind = Attr(bdaEl, "valKind", "Set"),
                        AttributeType = AttrOrNull(bdaEl, "type"),
                        Count = Attr(bdaEl, "count", "0"),
                        ValImport = AttrBool(bdaEl, "valImport")
                    };
                    foreach (var valEl in bdaEl.Elements(ns + "Val"))
                        bda.Vals.Add(new Val { SGroup = AttrUIntOrNull(valEl, "sGroup"), Value = valEl.Value.NullIfEmpty() });
                    dat.BDAs.Add(bda);
                }
                foreach (var protNsEl in datEl.Elements(ns + "ProtNs"))
                    dat.ProtNsEntries.Add(new ProtNs { ProtNsType = Attr(protNsEl, "type", "8-MMS"), NamespaceValue = protNsEl.Value.NullIfEmpty() });
                dtt.DATypes.Add(dat);
            }

            foreach (var enumEl in el.Elements(ns + "EnumType"))
            {
                var et = new EnumType
                {
                    SclId = Attr(enumEl, "id"),
                    Desc = AttrOrNull(enumEl, "desc")
                };
                foreach (var evEl in enumEl.Elements(ns + "EnumVal"))
                {
                    et.EnumVals.Add(new EnumVal
                    {
                        Ord = AttrInt(evEl, "ord", 0),
                        EnumDesc = AttrOrNull(evEl, "desc"),
                        Value = evEl.Value.NullIfEmpty()
                    });
                }
                dtt.EnumTypes.Add(et);
            }

            return dtt;
        }

        // ????????????????????????????????????????????????????????????????????
        // XML attribute helpers (instance overloads use _warnings)
        // ????????????????????????????????????????????????????????????????????

        private string Attr(XElement el, string name, string defaultVal = "")
        {
            var v = (string?)el.Attribute(name);
            return v ?? defaultVal;
        }

        private string? AttrOrNull(XElement el, string name) => (string?)el.Attribute(name);

        private static string Attr(XElement el, string name, string defaultVal, bool _)
            => (string?)el.Attribute(name) ?? defaultVal;

        // Static variants used in static methods
        private static string Attr(XElement el, string name) => (string?)el.Attribute(name) ?? "";
        private static string? AttrOrNullS(XElement el, string name) => (string?)el.Attribute(name);

        private static bool AttrBool(XElement el, string name, bool defaultVal = false)
        {
            var v = (string?)el.Attribute(name);
            if (v == null) return defaultVal;
            return v.Equals("true", StringComparison.OrdinalIgnoreCase) || v == "1";
        }

        private static bool? AttrBoolOrNull(XElement el, string name)
        {
            var v = (string?)el.Attribute(name);
            if (v == null) return null;
            return v.Equals("true", StringComparison.OrdinalIgnoreCase) || v == "1";
        }

        private static byte AttrByte(XElement el, string name, byte defaultVal = 0)
            => byte.TryParse((string?)el.Attribute(name), out var r) ? r : defaultVal;

        private static byte? AttrByteOrNull(XElement el, string name)
            => byte.TryParse((string?)el.Attribute(name), out var r) ? r : null;

        private static uint AttrUInt(XElement el, string name, uint defaultVal = 0)
            => uint.TryParse((string?)el.Attribute(name), out var r) ? r : defaultVal;

        private static uint? AttrUIntOrNull(XElement el, string name)
            => uint.TryParse((string?)el.Attribute(name), out var r) ? r : null;

        private static int AttrInt(XElement el, string name, int defaultVal = 0)
            => int.TryParse((string?)el.Attribute(name), out var r) ? r : defaultVal;

        private static decimal ParseDecimal(string? value)
            => decimal.TryParse(value, System.Globalization.NumberStyles.Any,
               System.Globalization.CultureInfo.InvariantCulture, out var r) ? r : 0m;

        private static decimal? ParseDecimalOrNull(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return decimal.TryParse(value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var r) ? r : null;
        }
    }

    internal static class StringExtensions
    {
        /// <summary>Returns <c>null</c> if the string is null or whitespace; otherwise the original string.</summary>
        public static string? NullIfEmpty(this string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s;
    }
}
