namespace GridSyncInterface.Models.Enums
{
    // tFCEnum - Functional Constraint
    public enum FunctionalConstraint
    {
        ST, MX, CO, SP, SG, SE, SV, CF, DC, EX, SR, BL, OR
    }

    // tValKindEnum
    public enum ValKind
    {
        Spec, Conf, RO, Set
    }

    // tGSEControlTypeEnum
    public enum GSEControlType
    {
        GSSE, GOOSE
    }

    // tSMVDeliveryEnum
    public enum SMVDelivery
    {
        Unicast, Multicast, Both
    }

    // tPhaseEnum
    public enum Phase
    {
        A, B, C, N, All, None, AB, BC, CA
    }

    // tAuthenticationEnum
    public enum AuthenticationKind
    {
        None, Password, Weak, Strong, Certificate
    }

    // tAssociationKindEnum
    public enum AssociationKind
    {
        PreEstablished, Predefined
    }

    // tRedProtEnum
    public enum RedProt
    {
        None, Hsr, Prp, Rstp
    }

    // tServiceSettingsNoDynEnum
    public enum ServiceSettingsNoDyn
    {
        Conf, Fix
    }

    // tServiceSettingsEnum
    public enum ServiceSettings
    {
        Dyn, Conf, Fix
    }

    // tSmpMod
    public enum SmpMod
    {
        SmpPerPeriod, SmpPerSec, SecPerSmp
    }

    // tRightEnum
    public enum Right
    {
        Full, Fix, Dataflow
    }

    // tServiceType
    public enum ServiceType
    {
        Poll, Report, GOOSE, SMV
    }

    // tPredefinedTypeOfSecurityEnum
    public enum TypeOfSecurity
    {
        None, Signature, SignatureAndEncryption
    }

    // tCDCEnum - Common Data Class
    public enum CDC
    {
        SPS, DPS, INS, ENS, ACT, ACD, SEC, BCR, HST, VSS,
        MV, CMV, SAV, WYE, DEL, SEQ, HMV, HWYE, HDEL,
        SPC, DPC, INC, ENC, BSC, ISC, APC, BAC,
        SPG, ING, ENG, ORG, TSG, CUG, VSG, ASG, CURVE, CSG,
        DPL, LPL, CSD, CST, BTS, UTS, LTS, GTS, MTS, NTS, STS, CTS, OTS,
        VSD, ORS, TCS, SCA
    }

    // tBasicTypeEnum
    public enum BasicType
    {
        BOOLEAN, INT8, INT16, INT24, INT32, INT64, INT128,
        INT8U, INT16U, INT24U, INT32U,
        FLOAT32, FLOAT64,
        Enum, Dbpos, Tcmd, Quality, Timestamp,
        VisString32, VisString64, VisString65, VisString129, VisString255,
        Octet64, Unicode255, Struct, EntryTime, Check, ObjRef, Currency,
        PhyComAddr, TrgOps, OptFlds, SvOptFlds, LogOptFlds, EntryID,
        Octet6, Octet16
    }

    // tPhysConnTypeEnum
    public enum PhysConnType
    {
        Connection, RedConn
    }
}
