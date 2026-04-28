using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GridSyncInterface.GridSyncInterface.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SCLs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Revision = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Release = table.Column<byte>(type: "tinyint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCLs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Communications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SclId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communications_SCLs_SclId",
                        column: x => x.SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataTypeTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SclId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTypeTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTypeTemplates_SCLs_SclId",
                        column: x => x.SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Headers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SclId = table.Column<int>(type: "int", nullable: false),
                    HeaderId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Revision = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ToolID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NameStructure = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headers_SCLs_SclId",
                        column: x => x.SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IEDs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SclId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IedType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConfigVersion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OriginalSclVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OriginalSclRevision = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OriginalSclRelease = table.Column<byte>(type: "tinyint", nullable: false),
                    EngRight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IEDs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IEDs_SCLs_SclId",
                        column: x => x.SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LNodeContainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerType = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    ConnectivityNode_BayId = table.Column<int>(type: "int", nullable: true),
                    LineId = table.Column<int>(type: "int", nullable: true),
                    PathName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConductingEquipmentId = table.Column<int>(type: "int", nullable: true),
                    SubEquipmentId = table.Column<int>(type: "int", nullable: true),
                    PowerTransformerId = table.Column<int>(type: "int", nullable: true),
                    TransformerWindingId = table.Column<int>(type: "int", nullable: true),
                    TapChangerId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipmentId = table.Column<int>(type: "int", nullable: true),
                    EqFunctionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EqFunctionId = table.Column<int>(type: "int", nullable: true),
                    EqSubFunctionParentId = table.Column<int>(type: "int", nullable: true),
                    EqSubFunctionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Virtual = table.Column<bool>(type: "bit", nullable: true),
                    ConductingEquipment_BayId = table.Column<int>(type: "int", nullable: true),
                    FunctionId = table.Column<int>(type: "int", nullable: true),
                    SubFunctionId = table.Column<int>(type: "int", nullable: true),
                    ConductingEquipment_LineParentId = table.Column<int>(type: "int", nullable: true),
                    ConductingEquipment_ProcessParentId = table.Column<int>(type: "int", nullable: true),
                    EquipmentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TransformerWinding_PowerTransformerId = table.Column<int>(type: "int", nullable: true),
                    WindingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentContainerId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipment_FunctionId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipment_SubFunctionId = table.Column<int>(type: "int", nullable: true),
                    EqFunctionParentId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipment_EqSubFunctionParentId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipment_LineParentId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipment_ProcessParentId = table.Column<int>(type: "int", nullable: true),
                    GeneralEquipment_EquipmentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PowerTransformer_EquipmentContainerId = table.Column<int>(type: "int", nullable: true),
                    PowerTransformer_EquipmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bay_VoltageLevelId = table.Column<int>(type: "int", nullable: true),
                    Substation_SclId = table.Column<int>(type: "int", nullable: true),
                    Substation_ProcessParentId = table.Column<int>(type: "int", nullable: true),
                    VoltageLevel_SubstationId = table.Column<int>(type: "int", nullable: true),
                    VoltageLevel_NomFreq = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    VoltageLevel_NumPhases = table.Column<byte>(type: "tinyint", nullable: true),
                    VoltageLevel_VoltageValue = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    VoltageUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VoltageLevel_VoltageMultiplier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SubstationId = table.Column<int>(type: "int", nullable: true),
                    VoltageLevelId = table.Column<int>(type: "int", nullable: true),
                    BayId = table.Column<int>(type: "int", nullable: true),
                    LineParentId = table.Column<int>(type: "int", nullable: true),
                    ProcessParentId = table.Column<int>(type: "int", nullable: true),
                    FunctionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SclId = table.Column<int>(type: "int", nullable: true),
                    ProcessId = table.Column<int>(type: "int", nullable: true),
                    LineType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NomFreq = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    NumPhases = table.Column<byte>(type: "tinyint", nullable: true),
                    VoltageValue = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    VoltageMultiplier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Process_SclId = table.Column<int>(type: "int", nullable: true),
                    ProcessParentId1 = table.Column<int>(type: "int", nullable: true),
                    ProcessType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AbstractConductingEquipmentId = table.Column<int>(type: "int", nullable: true),
                    Phase = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SubEquipment_Virtual = table.Column<bool>(type: "bit", nullable: true),
                    SubEquipment_PowerTransformerId = table.Column<int>(type: "int", nullable: true),
                    SubEquipment_TapChangerId = table.Column<int>(type: "int", nullable: true),
                    SubFunction_FunctionId = table.Column<int>(type: "int", nullable: true),
                    SubFunctionParentId = table.Column<int>(type: "int", nullable: true),
                    SubFunctionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TapChanger_TransformerWindingId = table.Column<int>(type: "int", nullable: true),
                    TapChangerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TapChanger_Virtual = table.Column<bool>(type: "bit", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LNodeContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_AbstractConductingEquipmentId",
                        column: x => x.AbstractConductingEquipmentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_BayId",
                        column: x => x.BayId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_Bay_VoltageLevelId",
                        column: x => x.Bay_VoltageLevelId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ConductingEquipmentId",
                        column: x => x.ConductingEquipmentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ConductingEquipment_BayId",
                        column: x => x.ConductingEquipment_BayId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ConductingEquipment_LineParentId",
                        column: x => x.ConductingEquipment_LineParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ConductingEquipment_ProcessParentId",
                        column: x => x.ConductingEquipment_ProcessParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ConnectivityNode_BayId",
                        column: x => x.ConnectivityNode_BayId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_EqFunctionId",
                        column: x => x.EqFunctionId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_EqFunctionParentId",
                        column: x => x.EqFunctionParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_EqSubFunctionParentId",
                        column: x => x.EqSubFunctionParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_EquipmentContainerId",
                        column: x => x.EquipmentContainerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_GeneralEquipmentId",
                        column: x => x.GeneralEquipmentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_GeneralEquipment_EqSubFunctionParentId",
                        column: x => x.GeneralEquipment_EqSubFunctionParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_GeneralEquipment_FunctionId",
                        column: x => x.GeneralEquipment_FunctionId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_GeneralEquipment_LineParentId",
                        column: x => x.GeneralEquipment_LineParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_GeneralEquipment_ProcessParentId",
                        column: x => x.GeneralEquipment_ProcessParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_GeneralEquipment_SubFunctionId",
                        column: x => x.GeneralEquipment_SubFunctionId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_LineId",
                        column: x => x.LineId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_LineParentId",
                        column: x => x.LineParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_PowerTransformerId",
                        column: x => x.PowerTransformerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_PowerTransformer_EquipmentContainerId",
                        column: x => x.PowerTransformer_EquipmentContainerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ProcessParentId",
                        column: x => x.ProcessParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_ProcessParentId1",
                        column: x => x.ProcessParentId1,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubEquipmentId",
                        column: x => x.SubEquipmentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubEquipment_PowerTransformerId",
                        column: x => x.SubEquipment_PowerTransformerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubEquipment_TapChangerId",
                        column: x => x.SubEquipment_TapChangerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubFunctionId",
                        column: x => x.SubFunctionId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubFunctionParentId",
                        column: x => x.SubFunctionParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubFunction_FunctionId",
                        column: x => x.SubFunction_FunctionId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_SubstationId",
                        column: x => x.SubstationId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_Substation_ProcessParentId",
                        column: x => x.Substation_ProcessParentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_TapChangerId",
                        column: x => x.TapChangerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_TapChanger_TransformerWindingId",
                        column: x => x.TapChanger_TransformerWindingId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_TransformerWindingId",
                        column: x => x.TransformerWindingId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_TransformerWinding_PowerTransformerId",
                        column: x => x.TransformerWinding_PowerTransformerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_VoltageLevelId",
                        column: x => x.VoltageLevelId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_LNodeContainers_VoltageLevel_SubstationId",
                        column: x => x.VoltageLevel_SubstationId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_SCLs_Process_SclId",
                        column: x => x.Process_SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_SCLs_SclId",
                        column: x => x.SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LNodeContainers_SCLs_Substation_SclId",
                        column: x => x.Substation_SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    SclId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_SCLs_SclId",
                        column: x => x.SclId,
                        principalTable: "SCLs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunicationId = table.Column<int>(type: "int", nullable: false),
                    NetworkType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BitRateValue = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    BitRateMultiplier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubNetworks_Communications_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "Communications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DATypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataTypeTemplatesId = table.Column<int>(type: "int", nullable: false),
                    IedType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SclId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DATypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DATypes_DataTypeTemplates_DataTypeTemplatesId",
                        column: x => x.DataTypeTemplatesId,
                        principalTable: "DataTypeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataTypeTemplatesId = table.Column<int>(type: "int", nullable: false),
                    IedType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Cdc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SclId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOTypes_DataTypeTemplates_DataTypeTemplatesId",
                        column: x => x.DataTypeTemplatesId,
                        principalTable: "DataTypeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnumTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataTypeTemplatesId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SclId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumTypes_DataTypeTemplates_DataTypeTemplatesId",
                        column: x => x.DataTypeTemplatesId,
                        principalTable: "DataTypeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LNodeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataTypeTemplatesId = table.Column<int>(type: "int", nullable: false),
                    IedType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SclId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LNodeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LNodeTypes_DataTypeTemplates_DataTypeTemplatesId",
                        column: x => x.DataTypeTemplatesId,
                        principalTable: "DataTypeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hitems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SclHeaderId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Revision = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    When = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Who = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    What = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Why = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hitems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hitems_Headers_SclHeaderId",
                        column: x => x.SclHeaderId,
                        principalTable: "Headers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IedId = table.Column<int>(type: "int", nullable: false),
                    NameLength = table.Column<int>(type: "int", nullable: false),
                    DynAssociation = table.Column<bool>(type: "bit", nullable: true),
                    DynAssociationMax = table.Column<long>(type: "bigint", nullable: true),
                    GetDirectory = table.Column<bool>(type: "bit", nullable: true),
                    GetDataObjectDefinition = table.Column<bool>(type: "bit", nullable: true),
                    DataObjectDirectory = table.Column<bool>(type: "bit", nullable: true),
                    GetDataSetValue = table.Column<bool>(type: "bit", nullable: true),
                    SetDataSetValue = table.Column<bool>(type: "bit", nullable: true),
                    DataSetDirectory = table.Column<bool>(type: "bit", nullable: true),
                    ReadWrite = table.Column<bool>(type: "bit", nullable: true),
                    TimerActivatedControl = table.Column<bool>(type: "bit", nullable: true),
                    GetCBValues = table.Column<bool>(type: "bit", nullable: true),
                    GSEDir = table.Column<bool>(type: "bit", nullable: true),
                    ConfLdName = table.Column<bool>(type: "bit", nullable: true),
                    ConfDataSetMax = table.Column<long>(type: "bigint", nullable: true),
                    ConfDataSetMaxAttributes = table.Column<long>(type: "bigint", nullable: true),
                    ConfDataSetModify = table.Column<bool>(type: "bit", nullable: false),
                    DynDataSetMax = table.Column<long>(type: "bigint", nullable: true),
                    DynDataSetMaxAttributes = table.Column<long>(type: "bigint", nullable: true),
                    ConfReportControlMax = table.Column<long>(type: "bigint", nullable: true),
                    ConfReportControlBufMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfReportControlBufConf = table.Column<bool>(type: "bit", nullable: false),
                    ConfReportControlMaxBuf = table.Column<long>(type: "bigint", nullable: true),
                    ConfLogControlMax = table.Column<long>(type: "bigint", nullable: true),
                    ReportSettingsCbName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsDatSet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsRptID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsOptFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsBufTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsTrgOps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsIntgPd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportSettingsResvTms = table.Column<bool>(type: "bit", nullable: false),
                    ReportSettingsOwner = table.Column<bool>(type: "bit", nullable: false),
                    LogSettingsCbName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogSettingsDatSet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogSettingsLogEna = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogSettingsTrgOps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogSettingsIntgPd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSESettingsCbName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSESettingsDatSet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSESettingsAppID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSESettingsDataLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSESettingsKdaParticipant = table.Column<bool>(type: "bit", nullable: false),
                    GSESettingsMcSecuritySignature = table.Column<bool>(type: "bit", nullable: false),
                    GSESettingsMcSecurityEncryption = table.Column<bool>(type: "bit", nullable: false),
                    SMVSettingsCbName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVSettingsDatSet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVSettingsSvID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVSettingsOptFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVSettingsSmpRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVSettingsSamplesPerSec = table.Column<bool>(type: "bit", nullable: false),
                    SMVSettingsPdcTimeStamp = table.Column<bool>(type: "bit", nullable: false),
                    SMVSettingsSynchSrcId = table.Column<bool>(type: "bit", nullable: false),
                    SMVSettingsNofASDU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVSettingsKdaParticipant = table.Column<bool>(type: "bit", nullable: false),
                    SMVSettingsMcSecuritySignature = table.Column<bool>(type: "bit", nullable: false),
                    SMVSettingsMcSecurityEncryption = table.Column<bool>(type: "bit", nullable: false),
                    GOOSEMax = table.Column<long>(type: "bigint", nullable: true),
                    GOOSEFixedOffs = table.Column<bool>(type: "bit", nullable: false),
                    GOOSEGoose = table.Column<bool>(type: "bit", nullable: false),
                    GOOSE_rGOOSE = table.Column<bool>(type: "bit", nullable: false),
                    GSSEMax = table.Column<long>(type: "bigint", nullable: true),
                    SMVscMax = table.Column<long>(type: "bigint", nullable: true),
                    SMVscDelivery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMVscDeliveryConf = table.Column<bool>(type: "bit", nullable: false),
                    SMVscSv = table.Column<bool>(type: "bit", nullable: false),
                    SMVsc_rSV = table.Column<bool>(type: "bit", nullable: false),
                    FileHandling = table.Column<bool>(type: "bit", nullable: true),
                    FileHandlingMms = table.Column<bool>(type: "bit", nullable: false),
                    FileHandlingFtp = table.Column<bool>(type: "bit", nullable: false),
                    FileHandlingFtps = table.Column<bool>(type: "bit", nullable: false),
                    ConfLNsFixPrefix = table.Column<bool>(type: "bit", nullable: false),
                    ConfLNsFixLnInst = table.Column<bool>(type: "bit", nullable: false),
                    ClientServicesGoose = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesGsse = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesBufReport = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesUnbufReport = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesReadLog = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesSv = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesSupportsLdName = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesMaxAttributes = table.Column<long>(type: "bigint", nullable: true),
                    ClientServicesMaxReports = table.Column<long>(type: "bigint", nullable: true),
                    ClientServicesMaxGOOSE = table.Column<long>(type: "bigint", nullable: true),
                    ClientServicesMaxSMV = table.Column<long>(type: "bigint", nullable: true),
                    ClientServices_rGOOSE = table.Column<bool>(type: "bit", nullable: true),
                    ClientServices_rSV = table.Column<bool>(type: "bit", nullable: true),
                    ClientServicesNoIctBinding = table.Column<bool>(type: "bit", nullable: true),
                    SupSubscriptionMaxGo = table.Column<long>(type: "bigint", nullable: true),
                    SupSubscriptionMaxSv = table.Column<long>(type: "bigint", nullable: true),
                    RedProtHsr = table.Column<bool>(type: "bit", nullable: true),
                    RedProtPrp = table.Column<bool>(type: "bit", nullable: true),
                    RedProtRstp = table.Column<bool>(type: "bit", nullable: true),
                    TimeSyncProtSntp = table.Column<bool>(type: "bit", nullable: true),
                    TimeSyncProt_iec61850_9_3 = table.Column<bool>(type: "bit", nullable: true),
                    TimeSyncProt_c37_238 = table.Column<bool>(type: "bit", nullable: true),
                    TimeSyncProtOther = table.Column<bool>(type: "bit", nullable: true),
                    CommProtIpv6 = table.Column<bool>(type: "bit", nullable: true),
                    ValueHandlingSetToRO = table.Column<bool>(type: "bit", nullable: true),
                    ConfSigRefMax = table.Column<long>(type: "bigint", nullable: true),
                    SettingGroupsSGEdit = table.Column<bool>(type: "bit", nullable: true),
                    SettingGroupsSGEditResvTms = table.Column<bool>(type: "bit", nullable: true),
                    SettingGroupsConfSG = table.Column<bool>(type: "bit", nullable: true),
                    SettingGroupsConfSGResvTms = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IedServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IedServices_IEDs_IedId",
                        column: x => x.IedId,
                        principalTable: "IEDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KDCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IedId = table.Column<int>(type: "int", nullable: false),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KDCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KDCs_IEDs_IedId",
                        column: x => x.IedId,
                        principalTable: "IEDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    LnType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LNodeContainerId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LNodes_LNodeContainers_LNodeContainerId",
                        column: x => x.LNodeContainerId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terminals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbstractConductingEquipmentId = table.Column<int>(type: "int", nullable: true),
                    TransformerWindingNeutralId = table.Column<int>(type: "int", nullable: true),
                    TerminalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ConnectivityNode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubstationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VoltageLevelName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CNodeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LineName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terminals_LNodeContainers_AbstractConductingEquipmentId",
                        column: x => x.AbstractConductingEquipmentId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Terminals_LNodeContainers_TransformerWindingNeutralId",
                        column: x => x.TransformerWindingNeutralId,
                        principalTable: "LNodeContainers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElementLocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    AcquiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementLocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElementLocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementLocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMemberships_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectedAPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubNetworkId = table.Column<int>(type: "int", nullable: false),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RedProt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedAPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedAPs_SubNetworks_SubNetworkId",
                        column: x => x.SubNetworkId,
                        principalTable: "SubNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BDAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DATypeId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SAddr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ValKind = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AttributeType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Count = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValImport = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BDAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BDAs_DATypes_DATypeId",
                        column: x => x.DATypeId,
                        principalTable: "DATypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOTypeId = table.Column<int>(type: "int", nullable: false),
                    Dchg = table.Column<bool>(type: "bit", nullable: false),
                    Qchg = table.Column<bool>(type: "bit", nullable: false),
                    Dupd = table.Column<bool>(type: "bit", nullable: false),
                    Fc = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SAddr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ValKind = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AttributeType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Count = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValImport = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAttributes_DOTypes_DOTypeId",
                        column: x => x.DOTypeId,
                        principalTable: "DOTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SDOs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SdoType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Count = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SDOs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SDOs_DOTypes_DOTypeId",
                        column: x => x.DOTypeId,
                        principalTable: "DOTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnumVals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnumTypeId = table.Column<int>(type: "int", nullable: false),
                    Ord = table.Column<int>(type: "int", nullable: false),
                    EnumDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumVals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumVals_EnumTypes_EnumTypeId",
                        column: x => x.EnumTypeId,
                        principalTable: "EnumTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LNodeTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    DoType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccessControl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Transient = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataObjects_LNodeTypes_LNodeTypeId",
                        column: x => x.LNodeTypeId,
                        principalTable: "LNodeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IedId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Router = table.Column<bool>(type: "bit", nullable: false),
                    Clock = table.Column<bool>(type: "bit", nullable: false),
                    Kdc = table.Column<bool>(type: "bit", nullable: false),
                    AccessPointServicesId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPoints_IEDs_IedId",
                        column: x => x.IedId,
                        principalTable: "IEDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessPoints_IedServices_AccessPointServicesId",
                        column: x => x.AccessPointServicesId,
                        principalTable: "IedServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommunicationControlBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectedAPId = table.Column<int>(type: "int", nullable: false),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CbName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CbType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    MinTime = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    MaxTime = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationControlBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationControlBlocks_ConnectedAPs_ConnectedAPId",
                        column: x => x.ConnectedAPId,
                        principalTable: "ConnectedAPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysConns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectedAPId = table.Column<int>(type: "int", nullable: false),
                    PhysConnType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysConns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysConns_ConnectedAPs_ConnectedAPId",
                        column: x => x.ConnectedAPId,
                        principalTable: "ConnectedAPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProtNsEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAttributeId = table.Column<int>(type: "int", nullable: true),
                    DATypeId = table.Column<int>(type: "int", nullable: true),
                    ProtNsType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NamespaceValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtNsEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtNsEntries_DATypes_DATypeId",
                        column: x => x.DATypeId,
                        principalTable: "DATypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProtNsEntries_DataAttributes_DataAttributeId",
                        column: x => x.DataAttributeId,
                        principalTable: "DataAttributes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessPointGooseId = table.Column<int>(type: "int", nullable: true),
                    AccessPointSmvId = table.Column<int>(type: "int", nullable: true),
                    XferNumber = table.Column<long>(type: "bigint", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectCommonName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubjectIdHierarchy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IssuerCommonName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IssuerIdHierarchy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_AccessPoints_AccessPointGooseId",
                        column: x => x.AccessPointGooseId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Certificates_AccessPoints_AccessPointSmvId",
                        column: x => x.AccessPointSmvId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServerAts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessPointId = table.Column<int>(type: "int", nullable: false),
                    ApName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerAts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerAts_AccessPoints_AccessPointId",
                        column: x => x.AccessPointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessPointId = table.Column<int>(type: "int", nullable: false),
                    Timeout = table.Column<long>(type: "bigint", nullable: false),
                    AuthNone = table.Column<bool>(type: "bit", nullable: false),
                    AuthPassword = table.Column<bool>(type: "bit", nullable: false),
                    AuthWeak = table.Column<bool>(type: "bit", nullable: false),
                    AuthStrong = table.Column<bool>(type: "bit", nullable: false),
                    AuthCertificate = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servers_AccessPoints_AccessPointId",
                        column: x => x.AccessPointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetworkAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectedAPId = table.Column<int>(type: "int", nullable: true),
                    ControlBlockId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetworkAddresses_CommunicationControlBlocks_ControlBlockId",
                        column: x => x.ControlBlockId,
                        principalTable: "CommunicationControlBlocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NetworkAddresses_ConnectedAPs_ConnectedAPId",
                        column: x => x.ConnectedAPId,
                        principalTable: "ConnectedAPs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PPhysConns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhysConnId = table.Column<int>(type: "int", nullable: false),
                    PType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPhysConns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PPhysConns_PhysConns_PhysConnId",
                        column: x => x.PhysConnId,
                        principalTable: "PhysConns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Associations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    AssociationDesc = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AssociationID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Associations_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    Inst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LdName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AccessControl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LDevices_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NetworkAddressId = table.Column<int>(type: "int", nullable: false),
                    PType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PAddresses_NetworkAddresses_NetworkAddressId",
                        column: x => x.NetworkAddressId,
                        principalTable: "NetworkAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogicalNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LnType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LogicalNodeKind = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    LDeviceId = table.Column<int>(type: "int", nullable: true),
                    LnClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogicalNode_LDeviceId = table.Column<int>(type: "int", nullable: true),
                    AccessPointId = table.Column<int>(type: "int", nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    LogicalNode_LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LogicalNode_Inst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogicalNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogicalNodes_AccessPoints_AccessPointId",
                        column: x => x.AccessPointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogicalNodes_LDevices_LDeviceId",
                        column: x => x.LDeviceId,
                        principalTable: "LDevices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogicalNodes_LDevices_LogicalNode_LDeviceId",
                        column: x => x.LogicalNode_LDeviceId,
                        principalTable: "LDevices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Controls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DatSet = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ControlType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ConfRev = table.Column<long>(type: "bigint", nullable: true),
                    LN0Id = table.Column<int>(type: "int", nullable: true),
                    GseType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AppID = table.Column<string>(type: "nvarchar(129)", maxLength: 129, nullable: true),
                    FixedOffs = table.Column<bool>(type: "bit", nullable: true),
                    SecurityEnable = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Protocol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampledValueControl_LN0Id = table.Column<int>(type: "int", nullable: true),
                    SmvID = table.Column<string>(type: "nvarchar(129)", maxLength: 129, nullable: true),
                    Multicast = table.Column<bool>(type: "bit", nullable: true),
                    SmpRate = table.Column<long>(type: "bigint", nullable: true),
                    NofASDU = table.Column<long>(type: "bigint", nullable: true),
                    SmpMod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SampledValueControl_SecurityEnable = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    SmvOptRefreshTime = table.Column<bool>(type: "bit", nullable: true),
                    SmvOptSampleSynchronized = table.Column<bool>(type: "bit", nullable: true),
                    SmvOptSampleRate = table.Column<bool>(type: "bit", nullable: true),
                    SmvOptDataSet = table.Column<bool>(type: "bit", nullable: true),
                    SmvOptSecurity = table.Column<bool>(type: "bit", nullable: true),
                    SmvOptTimestamp = table.Column<bool>(type: "bit", nullable: true),
                    SmvOptSynchSourceId = table.Column<bool>(type: "bit", nullable: true),
                    SampledValueControl_Protocol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntgPd = table.Column<long>(type: "bigint", nullable: true),
                    TrgOpsDchg = table.Column<bool>(type: "bit", nullable: true),
                    TrgOpsQchg = table.Column<bool>(type: "bit", nullable: true),
                    TrgOpsDupd = table.Column<bool>(type: "bit", nullable: true),
                    TrgOpsPeriod = table.Column<bool>(type: "bit", nullable: true),
                    TrgOpsGi = table.Column<bool>(type: "bit", nullable: true),
                    LogControl_LN0Id = table.Column<int>(type: "int", nullable: true),
                    LogicalNodeId = table.Column<int>(type: "int", nullable: true),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    LogName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LogEna = table.Column<bool>(type: "bit", nullable: true),
                    ReasonCode = table.Column<bool>(type: "bit", nullable: true),
                    BufTime = table.Column<long>(type: "bigint", nullable: true),
                    ReportControl_LN0Id = table.Column<int>(type: "int", nullable: true),
                    ReportControl_LogicalNodeId = table.Column<int>(type: "int", nullable: true),
                    RptID = table.Column<string>(type: "nvarchar(129)", maxLength: 129, nullable: true),
                    ReportControl_ConfRev = table.Column<long>(type: "bigint", nullable: true),
                    Buffered = table.Column<bool>(type: "bit", nullable: true),
                    ReportControl_BufTime = table.Column<long>(type: "bigint", nullable: true),
                    Indexed = table.Column<bool>(type: "bit", nullable: true),
                    OptSeqNum = table.Column<bool>(type: "bit", nullable: true),
                    OptTimeStamp = table.Column<bool>(type: "bit", nullable: true),
                    OptDataSet = table.Column<bool>(type: "bit", nullable: true),
                    OptReasonCode = table.Column<bool>(type: "bit", nullable: true),
                    OptDataRef = table.Column<bool>(type: "bit", nullable: true),
                    OptEntryID = table.Column<bool>(type: "bit", nullable: true),
                    OptConfigRef = table.Column<bool>(type: "bit", nullable: true),
                    OptBufOvfl = table.Column<bool>(type: "bit", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Controls_LogicalNodes_LN0Id",
                        column: x => x.LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Controls_LogicalNodes_LogControl_LN0Id",
                        column: x => x.LogControl_LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Controls_LogicalNodes_LogicalNodeId",
                        column: x => x.LogicalNodeId,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Controls_LogicalNodes_ReportControl_LN0Id",
                        column: x => x.ReportControl_LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Controls_LogicalNodes_ReportControl_LogicalNodeId",
                        column: x => x.ReportControl_LogicalNodeId,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Controls_LogicalNodes_SampledValueControl_LN0Id",
                        column: x => x.SampledValueControl_LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LN0Id = table.Column<int>(type: "int", nullable: true),
                    LogicalNodeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSets_LogicalNodes_LN0Id",
                        column: x => x.LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DataSets_LogicalNodes_LogicalNodeId",
                        column: x => x.LogicalNodeId,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DOIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LN0Id = table.Column<int>(type: "int", nullable: true),
                    LogicalNodeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Ix = table.Column<long>(type: "bigint", nullable: true),
                    AccessControl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOIs_LogicalNodes_LN0Id",
                        column: x => x.LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DOIs_LogicalNodes_LogicalNodeId",
                        column: x => x.LogicalNodeId,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Inputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LN0Id = table.Column<int>(type: "int", nullable: true),
                    LogicalNodeId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inputs_LogicalNodes_LN0Id",
                        column: x => x.LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inputs_LogicalNodes_LogicalNodeId",
                        column: x => x.LogicalNodeId,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SclLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LN0Id = table.Column<int>(type: "int", nullable: true),
                    LogicalNodeId = table.Column<int>(type: "int", nullable: true),
                    LogName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SclLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SclLogs_LogicalNodes_LN0Id",
                        column: x => x.LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SclLogs_LogicalNodes_LogicalNodeId",
                        column: x => x.LogicalNodeId,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SettingControls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LN0Id = table.Column<int>(type: "int", nullable: false),
                    NumOfSGs = table.Column<long>(type: "bigint", nullable: false),
                    ActSG = table.Column<long>(type: "bigint", nullable: false),
                    ResvTms = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingControls_LogicalNodes_LN0Id",
                        column: x => x.LN0Id,
                        principalTable: "LogicalNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ControlIEDNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GSEControlId = table.Column<int>(type: "int", nullable: true),
                    SVControlId = table.Column<int>(type: "int", nullable: true),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApRef = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlIEDNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlIEDNames_Controls_GSEControlId",
                        column: x => x.GSEControlId,
                        principalTable: "Controls",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ControlIEDNames_Controls_SVControlId",
                        column: x => x.SVControlId,
                        principalTable: "Controls",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RptEnableds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportControlId = table.Column<int>(type: "int", nullable: false),
                    Max = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptEnableds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RptEnableds_Controls_ReportControlId",
                        column: x => x.ReportControlId,
                        principalTable: "Controls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FCDAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataSetId = table.Column<int>(type: "int", nullable: false),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DaName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Fc = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Ix = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCDAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCDAs_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SDIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOIId = table.Column<int>(type: "int", nullable: true),
                    SDIParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Ix = table.Column<long>(type: "bigint", nullable: true),
                    SAddr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SDIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SDIs_DOIs_DOIId",
                        column: x => x.DOIId,
                        principalTable: "DOIs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SDIs_SDIs_SDIParentId",
                        column: x => x.SDIParentId,
                        principalTable: "SDIs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExtRefs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InputsId = table.Column<int>(type: "int", nullable: false),
                    ExtRefDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DaName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IntAddr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SrcLDInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SrcPrefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    SrcLNClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    SrcLNInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    SrcCBName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PServT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PLN = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PDO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PDA = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtRefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtRefs_Inputs_InputsId",
                        column: x => x.InputsId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientLNs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RptEnabledId = table.Column<int>(type: "int", nullable: false),
                    ClientDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IedName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LdInst = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    LnClass = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    LnInst = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ApRef = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLNs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLNs_RptEnableds_RptEnabledId",
                        column: x => x.RptEnabledId,
                        principalTable: "RptEnableds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOIId = table.Column<int>(type: "int", nullable: true),
                    SDIId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SAddr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ValKind = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Ix = table.Column<long>(type: "bigint", nullable: true),
                    ValImport = table.Column<bool>(type: "bit", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAIs_DOIs_DOIId",
                        column: x => x.DOIId,
                        principalTable: "DOIs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DAIs_SDIs_SDIId",
                        column: x => x.SDIId,
                        principalTable: "SDIs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DAIId = table.Column<int>(type: "int", nullable: true),
                    DataAttributeId = table.Column<int>(type: "int", nullable: true),
                    BDAId = table.Column<int>(type: "int", nullable: true),
                    SGroup = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vals_BDAs_BDAId",
                        column: x => x.BDAId,
                        principalTable: "BDAs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vals_DAIs_DAIId",
                        column: x => x.DAIId,
                        principalTable: "DAIs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vals_DataAttributes_DataAttributeId",
                        column: x => x.DataAttributeId,
                        principalTable: "DataAttributes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_AccessPointServicesId",
                table: "AccessPoints",
                column: "AccessPointServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_IedId",
                table: "AccessPoints",
                column: "IedId");

            migrationBuilder.CreateIndex(
                name: "IX_Associations_ServerId",
                table: "Associations",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ProjectId_Timestamp",
                table: "AuditLogs",
                columns: new[] { "ProjectId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BDAs_DATypeId",
                table: "BDAs",
                column: "DATypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_AccessPointGooseId",
                table: "Certificates",
                column: "AccessPointGooseId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_AccessPointSmvId",
                table: "Certificates",
                column: "AccessPointSmvId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLNs_RptEnabledId",
                table: "ClientLNs",
                column: "RptEnabledId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationControlBlocks_ConnectedAPId",
                table: "CommunicationControlBlocks",
                column: "ConnectedAPId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_SclId",
                table: "Communications",
                column: "SclId",
                unique: true,
                filter: "[SclId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedAPs_SubNetworkId",
                table: "ConnectedAPs",
                column: "SubNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlIEDNames_GSEControlId",
                table: "ControlIEDNames",
                column: "GSEControlId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlIEDNames_SVControlId",
                table: "ControlIEDNames",
                column: "SVControlId");

            migrationBuilder.CreateIndex(
                name: "IX_Controls_LN0Id",
                table: "Controls",
                column: "LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Controls_LogControl_LN0Id",
                table: "Controls",
                column: "LogControl_LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Controls_LogicalNodeId",
                table: "Controls",
                column: "LogicalNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Controls_ReportControl_LN0Id",
                table: "Controls",
                column: "ReportControl_LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Controls_ReportControl_LogicalNodeId",
                table: "Controls",
                column: "ReportControl_LogicalNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Controls_SampledValueControl_LN0Id",
                table: "Controls",
                column: "SampledValueControl_LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_DAIs_DOIId",
                table: "DAIs",
                column: "DOIId");

            migrationBuilder.CreateIndex(
                name: "IX_DAIs_SDIId",
                table: "DAIs",
                column: "SDIId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAttributes_DOTypeId",
                table: "DataAttributes",
                column: "DOTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataObjects_LNodeTypeId",
                table: "DataObjects",
                column: "LNodeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_LN0Id",
                table: "DataSets",
                column: "LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_LogicalNodeId",
                table: "DataSets",
                column: "LogicalNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTypeTemplates_SclId",
                table: "DataTypeTemplates",
                column: "SclId",
                unique: true,
                filter: "[SclId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DATypes_DataTypeTemplatesId",
                table: "DATypes",
                column: "DataTypeTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_DATypes_SclId",
                table: "DATypes",
                column: "SclId");

            migrationBuilder.CreateIndex(
                name: "IX_DOIs_LN0Id",
                table: "DOIs",
                column: "LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_DOIs_LogicalNodeId",
                table: "DOIs",
                column: "LogicalNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_DOTypes_DataTypeTemplatesId",
                table: "DOTypes",
                column: "DataTypeTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_DOTypes_SclId",
                table: "DOTypes",
                column: "SclId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementLocks_ExpiresAt",
                table: "ElementLocks",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_ElementLocks_ProjectId_EntityType_EntityId",
                table: "ElementLocks",
                columns: new[] { "ProjectId", "EntityType", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElementLocks_UserId",
                table: "ElementLocks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumTypes_DataTypeTemplatesId",
                table: "EnumTypes",
                column: "DataTypeTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumTypes_SclId",
                table: "EnumTypes",
                column: "SclId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumVals_EnumTypeId",
                table: "EnumVals",
                column: "EnumTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtRefs_InputsId",
                table: "ExtRefs",
                column: "InputsId");

            migrationBuilder.CreateIndex(
                name: "IX_FCDAs_DataSetId",
                table: "FCDAs",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Headers_SclId",
                table: "Headers",
                column: "SclId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hitems_SclHeaderId",
                table: "Hitems",
                column: "SclHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_IEDs_Name",
                table: "IEDs",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_IEDs_SclId_Name",
                table: "IEDs",
                columns: new[] { "SclId", "Name" },
                unique: true,
                filter: "[SclId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IedServices_IedId",
                table: "IedServices",
                column: "IedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inputs_LN0Id",
                table: "Inputs",
                column: "LN0Id",
                unique: true,
                filter: "[LN0Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Inputs_LogicalNodeId",
                table: "Inputs",
                column: "LogicalNodeId",
                unique: true,
                filter: "[LogicalNodeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KDCs_IedId",
                table: "KDCs",
                column: "IedId");

            migrationBuilder.CreateIndex(
                name: "IX_LDevices_ServerId",
                table: "LDevices",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_AbstractConductingEquipmentId",
                table: "LNodeContainers",
                column: "AbstractConductingEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_Bay_VoltageLevelId",
                table: "LNodeContainers",
                column: "Bay_VoltageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_BayId",
                table: "LNodeContainers",
                column: "BayId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ConductingEquipment_BayId",
                table: "LNodeContainers",
                column: "ConductingEquipment_BayId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ConductingEquipment_LineParentId",
                table: "LNodeContainers",
                column: "ConductingEquipment_LineParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ConductingEquipment_ProcessParentId",
                table: "LNodeContainers",
                column: "ConductingEquipment_ProcessParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ConductingEquipmentId",
                table: "LNodeContainers",
                column: "ConductingEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ConnectivityNode_BayId",
                table: "LNodeContainers",
                column: "ConnectivityNode_BayId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_EqFunctionId",
                table: "LNodeContainers",
                column: "EqFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_EqFunctionParentId",
                table: "LNodeContainers",
                column: "EqFunctionParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_EqSubFunctionParentId",
                table: "LNodeContainers",
                column: "EqSubFunctionParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_EquipmentContainerId",
                table: "LNodeContainers",
                column: "EquipmentContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_FunctionId",
                table: "LNodeContainers",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_GeneralEquipment_EqSubFunctionParentId",
                table: "LNodeContainers",
                column: "GeneralEquipment_EqSubFunctionParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_GeneralEquipment_FunctionId",
                table: "LNodeContainers",
                column: "GeneralEquipment_FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_GeneralEquipment_LineParentId",
                table: "LNodeContainers",
                column: "GeneralEquipment_LineParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_GeneralEquipment_ProcessParentId",
                table: "LNodeContainers",
                column: "GeneralEquipment_ProcessParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_GeneralEquipment_SubFunctionId",
                table: "LNodeContainers",
                column: "GeneralEquipment_SubFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_GeneralEquipmentId",
                table: "LNodeContainers",
                column: "GeneralEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_LineId",
                table: "LNodeContainers",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_LineParentId",
                table: "LNodeContainers",
                column: "LineParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_Name",
                table: "LNodeContainers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_PowerTransformer_EquipmentContainerId",
                table: "LNodeContainers",
                column: "PowerTransformer_EquipmentContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_PowerTransformerId",
                table: "LNodeContainers",
                column: "PowerTransformerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_Process_SclId",
                table: "LNodeContainers",
                column: "Process_SclId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ProcessId",
                table: "LNodeContainers",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ProcessParentId",
                table: "LNodeContainers",
                column: "ProcessParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_ProcessParentId1",
                table: "LNodeContainers",
                column: "ProcessParentId1");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SclId",
                table: "LNodeContainers",
                column: "SclId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubEquipment_PowerTransformerId",
                table: "LNodeContainers",
                column: "SubEquipment_PowerTransformerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubEquipment_TapChangerId",
                table: "LNodeContainers",
                column: "SubEquipment_TapChangerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubEquipmentId",
                table: "LNodeContainers",
                column: "SubEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubFunction_FunctionId",
                table: "LNodeContainers",
                column: "SubFunction_FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubFunctionId",
                table: "LNodeContainers",
                column: "SubFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubFunctionParentId",
                table: "LNodeContainers",
                column: "SubFunctionParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_Substation_ProcessParentId",
                table: "LNodeContainers",
                column: "Substation_ProcessParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_Substation_SclId",
                table: "LNodeContainers",
                column: "Substation_SclId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_SubstationId",
                table: "LNodeContainers",
                column: "SubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_TapChanger_TransformerWindingId",
                table: "LNodeContainers",
                column: "TapChanger_TransformerWindingId",
                unique: true,
                filter: "[TransformerWindingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_TapChangerId",
                table: "LNodeContainers",
                column: "TapChangerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_TransformerWinding_PowerTransformerId",
                table: "LNodeContainers",
                column: "TransformerWinding_PowerTransformerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_TransformerWindingId",
                table: "LNodeContainers",
                column: "TransformerWindingId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_VoltageLevel_SubstationId",
                table: "LNodeContainers",
                column: "VoltageLevel_SubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeContainers_VoltageLevelId",
                table: "LNodeContainers",
                column: "VoltageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodes_LNodeContainerId",
                table: "LNodes",
                column: "LNodeContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeTypes_DataTypeTemplatesId",
                table: "LNodeTypes",
                column: "DataTypeTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_LNodeTypes_SclId",
                table: "LNodeTypes",
                column: "SclId");

            migrationBuilder.CreateIndex(
                name: "IX_LogicalNodes_AccessPointId",
                table: "LogicalNodes",
                column: "AccessPointId");

            migrationBuilder.CreateIndex(
                name: "IX_LogicalNodes_LDeviceId",
                table: "LogicalNodes",
                column: "LDeviceId",
                unique: true,
                filter: "[LDeviceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LogicalNodes_LogicalNode_LDeviceId",
                table: "LogicalNodes",
                column: "LogicalNode_LDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkAddresses_ConnectedAPId",
                table: "NetworkAddresses",
                column: "ConnectedAPId",
                unique: true,
                filter: "[ConnectedAPId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkAddresses_ControlBlockId",
                table: "NetworkAddresses",
                column: "ControlBlockId",
                unique: true,
                filter: "[ControlBlockId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PAddresses_NetworkAddressId",
                table: "PAddresses",
                column: "NetworkAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysConns_ConnectedAPId",
                table: "PhysConns",
                column: "ConnectedAPId");

            migrationBuilder.CreateIndex(
                name: "IX_PPhysConns_PhysConnId",
                table: "PPhysConns",
                column: "PhysConnId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberships_ProjectId_UserId",
                table: "ProjectMemberships",
                columns: new[] { "ProjectId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberships_UserId",
                table: "ProjectMemberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedByUserId",
                table: "Projects",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SclId",
                table: "Projects",
                column: "SclId",
                unique: true,
                filter: "[SclId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProtNsEntries_DataAttributeId",
                table: "ProtNsEntries",
                column: "DataAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtNsEntries_DATypeId",
                table: "ProtNsEntries",
                column: "DATypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RptEnableds_ReportControlId",
                table: "RptEnableds",
                column: "ReportControlId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SclLogs_LN0Id",
                table: "SclLogs",
                column: "LN0Id");

            migrationBuilder.CreateIndex(
                name: "IX_SclLogs_LogicalNodeId",
                table: "SclLogs",
                column: "LogicalNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_SDIs_DOIId",
                table: "SDIs",
                column: "DOIId");

            migrationBuilder.CreateIndex(
                name: "IX_SDIs_SDIParentId",
                table: "SDIs",
                column: "SDIParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SDOs_DOTypeId",
                table: "SDOs",
                column: "DOTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerAts_AccessPointId",
                table: "ServerAts",
                column: "AccessPointId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_AccessPointId",
                table: "Servers",
                column: "AccessPointId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingControls_LN0Id",
                table: "SettingControls",
                column: "LN0Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubNetworks_CommunicationId",
                table: "SubNetworks",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_AbstractConductingEquipmentId",
                table: "Terminals",
                column: "AbstractConductingEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_TransformerWindingNeutralId",
                table: "Terminals",
                column: "TransformerWindingNeutralId",
                unique: true,
                filter: "[TransformerWindingNeutralId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vals_BDAId",
                table: "Vals",
                column: "BDAId");

            migrationBuilder.CreateIndex(
                name: "IX_Vals_DAIId",
                table: "Vals",
                column: "DAIId");

            migrationBuilder.CreateIndex(
                name: "IX_Vals_DataAttributeId",
                table: "Vals",
                column: "DataAttributeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Associations");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "ClientLNs");

            migrationBuilder.DropTable(
                name: "ControlIEDNames");

            migrationBuilder.DropTable(
                name: "DataObjects");

            migrationBuilder.DropTable(
                name: "ElementLocks");

            migrationBuilder.DropTable(
                name: "EnumVals");

            migrationBuilder.DropTable(
                name: "ExtRefs");

            migrationBuilder.DropTable(
                name: "FCDAs");

            migrationBuilder.DropTable(
                name: "Hitems");

            migrationBuilder.DropTable(
                name: "KDCs");

            migrationBuilder.DropTable(
                name: "LNodes");

            migrationBuilder.DropTable(
                name: "PAddresses");

            migrationBuilder.DropTable(
                name: "PPhysConns");

            migrationBuilder.DropTable(
                name: "ProjectMemberships");

            migrationBuilder.DropTable(
                name: "ProtNsEntries");

            migrationBuilder.DropTable(
                name: "SclLogs");

            migrationBuilder.DropTable(
                name: "SDOs");

            migrationBuilder.DropTable(
                name: "ServerAts");

            migrationBuilder.DropTable(
                name: "SettingControls");

            migrationBuilder.DropTable(
                name: "Terminals");

            migrationBuilder.DropTable(
                name: "Vals");

            migrationBuilder.DropTable(
                name: "RptEnableds");

            migrationBuilder.DropTable(
                name: "LNodeTypes");

            migrationBuilder.DropTable(
                name: "EnumTypes");

            migrationBuilder.DropTable(
                name: "Inputs");

            migrationBuilder.DropTable(
                name: "DataSets");

            migrationBuilder.DropTable(
                name: "Headers");

            migrationBuilder.DropTable(
                name: "NetworkAddresses");

            migrationBuilder.DropTable(
                name: "PhysConns");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "LNodeContainers");

            migrationBuilder.DropTable(
                name: "BDAs");

            migrationBuilder.DropTable(
                name: "DAIs");

            migrationBuilder.DropTable(
                name: "DataAttributes");

            migrationBuilder.DropTable(
                name: "Controls");

            migrationBuilder.DropTable(
                name: "CommunicationControlBlocks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DATypes");

            migrationBuilder.DropTable(
                name: "SDIs");

            migrationBuilder.DropTable(
                name: "DOTypes");

            migrationBuilder.DropTable(
                name: "ConnectedAPs");

            migrationBuilder.DropTable(
                name: "DOIs");

            migrationBuilder.DropTable(
                name: "DataTypeTemplates");

            migrationBuilder.DropTable(
                name: "SubNetworks");

            migrationBuilder.DropTable(
                name: "LogicalNodes");

            migrationBuilder.DropTable(
                name: "Communications");

            migrationBuilder.DropTable(
                name: "LDevices");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "AccessPoints");

            migrationBuilder.DropTable(
                name: "IedServices");

            migrationBuilder.DropTable(
                name: "IEDs");

            migrationBuilder.DropTable(
                name: "SCLs");
        }
    }
}
