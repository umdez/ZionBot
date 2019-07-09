namespace OtClientBot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.msmMain = new MetroFramework.Components.MetroStyleManager(this.components);
            this.tabController = new MetroFramework.Controls.MetroTabControl();
            this.tabMain = new MetroFramework.Controls.MetroTabPage();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.tabLogger = new MetroFramework.Controls.MetroTabPage();
            this.btnLogsClear = new MetroFramework.Controls.MetroButton();
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            this.tabScripting = new MetroFramework.Controls.MetroTabPage();
            this.btnNewScript = new MetroFramework.Controls.MetroButton();
            this.wpfScriptsHost = new System.Windows.Forms.Integration.ElementHost();
            this.tabWalker = new MetroFramework.Controls.MetroTabPage();
            this.rbtnNW = new MetroFramework.Controls.MetroRadioButton();
            this.btnDeleteWpt = new MetroFramework.Controls.MetroButton();
            this.btnLoadWpt = new MetroFramework.Controls.MetroButton();
            this.btnSaveWpt = new MetroFramework.Controls.MetroButton();
            this.txtWaypointName = new MetroFramework.Controls.MetroTextBox();
            this.lstSavedWaypoints = new System.Windows.Forms.ListBox();
            this.btnWaypointClear = new MetroFramework.Controls.MetroButton();
            this.toggleWalker = new MetroFramework.Controls.MetroToggle();
            this.rbtnSE = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnS = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnSW = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnE = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnC = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnW = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnNE = new MetroFramework.Controls.MetroRadioButton();
            this.rbtnN = new MetroFramework.Controls.MetroRadioButton();
            this.btnUse = new MetroFramework.Controls.MetroButton();
            this.btnShovel = new MetroFramework.Controls.MetroButton();
            this.btnRope = new MetroFramework.Controls.MetroButton();
            this.btnStand = new MetroFramework.Controls.MetroButton();
            this.btnWalk = new MetroFramework.Controls.MetroButton();
            this.btnWaypointDelete = new MetroFramework.Controls.MetroButton();
            this.btnWaypointEdit = new MetroFramework.Controls.MetroButton();
            this.btnWaypointDown = new MetroFramework.Controls.MetroButton();
            this.btnWaypointUp = new MetroFramework.Controls.MetroButton();
            this.lstWaypoints = new System.Windows.Forms.ListBox();
            this.tabAutoLoot = new MetroFramework.Controls.MetroTabPage();
            this.toggleAutoLoot = new MetroFramework.Controls.MetroToggle();
            this.tabTargeting = new MetroFramework.Controls.MetroTabPage();
            this.btnTargetingAddNewPolicy = new MetroFramework.Controls.MetroButton();
            this.btnTargetingClearPolicyList = new MetroFramework.Controls.MetroButton();
            this.btnTargetingLoadPolicies = new MetroFramework.Controls.MetroButton();
            this.btnTargetingSavePolicies = new MetroFramework.Controls.MetroButton();
            this.wpfTargetingPolicyPanel = new System.Windows.Forms.Integration.ElementHost();
            this.chkAttackAllCreaturesThatAttack = new MetroFramework.Controls.MetroToggle();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.chkSwitchToHigherPriority = new MetroFramework.Controls.MetroToggle();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.toggleTargeting = new MetroFramework.Controls.MetroToggle();
            this.tabPython = new MetroFramework.Controls.MetroTabPage();
            this.wpfPythonConsole = new System.Windows.Forms.Integration.ElementHost();
            this.metroToggle1 = new MetroFramework.Controls.MetroToggle();
            this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.msmMain)).BeginInit();
            this.tabController.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabLogger.SuspendLayout();
            this.tabScripting.SuspendLayout();
            this.tabWalker.SuspendLayout();
            this.tabAutoLoot.SuspendLayout();
            this.tabTargeting.SuspendLayout();
            this.tabPython.SuspendLayout();
            this.SuspendLayout();
            // 
            // msmMain
            // 
            this.msmMain.Owner = this;
            this.msmMain.Style = MetroFramework.MetroColorStyle.White;
            this.msmMain.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // tabController
            // 
            this.tabController.Controls.Add(this.tabMain);
            this.tabController.Controls.Add(this.tabScripting);
            this.tabController.Controls.Add(this.tabLogger);
            this.tabController.Controls.Add(this.tabWalker);
            this.tabController.Controls.Add(this.tabAutoLoot);
            this.tabController.Controls.Add(this.tabTargeting);
            this.tabController.Controls.Add(this.tabPython);
            this.tabController.Location = new System.Drawing.Point(7, 63);
            this.tabController.Name = "tabController";
            this.tabController.SelectedIndex = 2;
            this.tabController.Size = new System.Drawing.Size(633, 331);
            this.tabController.TabIndex = 0;
            this.tabController.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabController.TabIndexChanged += new System.EventHandler(this.tabController_TabIndexChanged);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.metroLabel2);
            this.tabMain.HorizontalScrollbarBarColor = true;
            this.tabMain.Location = new System.Drawing.Point(4, 35);
            this.tabMain.Name = "tabMain";
            this.tabMain.Size = new System.Drawing.Size(625, 292);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Miscellaneous";
            this.tabMain.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabMain.VerticalScrollbarBarColor = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(12, 46);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(0, 0);
            this.metroLabel2.TabIndex = 3;
            // 
            // tabLogger
            // 
            this.tabLogger.Controls.Add(this.btnLogsClear);
            this.tabLogger.Controls.Add(this.rtxtLog);
            this.tabLogger.HorizontalScrollbarBarColor = true;
            this.tabLogger.Location = new System.Drawing.Point(4, 35);
            this.tabLogger.Name = "tabLogger";
            this.tabLogger.Size = new System.Drawing.Size(625, 292);
            this.tabLogger.TabIndex = 3;
            this.tabLogger.Text = "Logs";
            this.tabLogger.VerticalScrollbarBarColor = true;
            // 
            // btnLogsClear
            // 
            this.btnLogsClear.Location = new System.Drawing.Point(528, 266);
            this.btnLogsClear.Name = "btnLogsClear";
            this.btnLogsClear.Size = new System.Drawing.Size(75, 23);
            this.btnLogsClear.TabIndex = 3;
            this.btnLogsClear.Text = "Clear";
            this.btnLogsClear.Click += new System.EventHandler(this.btnLogsClear_Click);
            // 
            // rtxtLog
            // 
            this.rtxtLog.Location = new System.Drawing.Point(3, 3);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.ReadOnly = true;
            this.rtxtLog.Size = new System.Drawing.Size(619, 286);
            this.rtxtLog.TabIndex = 2;
            this.rtxtLog.Text = "";
            // 
            // tabScripting
            // 
            this.tabScripting.Controls.Add(this.btnNewScript);
            this.tabScripting.Controls.Add(this.wpfScriptsHost);
            this.tabScripting.HorizontalScrollbarBarColor = true;
            this.tabScripting.Location = new System.Drawing.Point(4, 35);
            this.tabScripting.Name = "tabScripting";
            this.tabScripting.Size = new System.Drawing.Size(625, 292);
            this.tabScripting.TabIndex = 6;
            this.tabScripting.Text = "Scripting";
            this.tabScripting.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabScripting.VerticalScrollbarBarColor = true;
            // 
            // btnNewScript
            // 
            this.btnNewScript.Location = new System.Drawing.Point(3, 268);
            this.btnNewScript.Name = "btnNewScript";
            this.btnNewScript.Size = new System.Drawing.Size(75, 21);
            this.btnNewScript.TabIndex = 4;
            this.btnNewScript.Text = "New Script";
            this.btnNewScript.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnNewScript.Click += new System.EventHandler(this.btnNewScript_Click);
            // 
            // wpfScriptsHost
            // 
            this.wpfScriptsHost.Location = new System.Drawing.Point(0, 10);
            this.wpfScriptsHost.Name = "wpfScriptsHost";
            this.wpfScriptsHost.Size = new System.Drawing.Size(619, 255);
            this.wpfScriptsHost.TabIndex = 3;
            this.wpfScriptsHost.Child = null;
            // 
            // tabWalker
            // 
            this.tabWalker.Controls.Add(this.rbtnNW);
            this.tabWalker.Controls.Add(this.btnDeleteWpt);
            this.tabWalker.Controls.Add(this.btnLoadWpt);
            this.tabWalker.Controls.Add(this.btnSaveWpt);
            this.tabWalker.Controls.Add(this.txtWaypointName);
            this.tabWalker.Controls.Add(this.lstSavedWaypoints);
            this.tabWalker.Controls.Add(this.btnWaypointClear);
            this.tabWalker.Controls.Add(this.toggleWalker);
            this.tabWalker.Controls.Add(this.rbtnSE);
            this.tabWalker.Controls.Add(this.rbtnS);
            this.tabWalker.Controls.Add(this.rbtnSW);
            this.tabWalker.Controls.Add(this.rbtnE);
            this.tabWalker.Controls.Add(this.rbtnC);
            this.tabWalker.Controls.Add(this.rbtnW);
            this.tabWalker.Controls.Add(this.rbtnNE);
            this.tabWalker.Controls.Add(this.rbtnN);
            this.tabWalker.Controls.Add(this.btnUse);
            this.tabWalker.Controls.Add(this.btnShovel);
            this.tabWalker.Controls.Add(this.btnRope);
            this.tabWalker.Controls.Add(this.btnStand);
            this.tabWalker.Controls.Add(this.btnWalk);
            this.tabWalker.Controls.Add(this.btnWaypointDelete);
            this.tabWalker.Controls.Add(this.btnWaypointEdit);
            this.tabWalker.Controls.Add(this.btnWaypointDown);
            this.tabWalker.Controls.Add(this.btnWaypointUp);
            this.tabWalker.Controls.Add(this.lstWaypoints);
            this.tabWalker.HorizontalScrollbarBarColor = true;
            this.tabWalker.Location = new System.Drawing.Point(4, 35);
            this.tabWalker.Name = "tabWalker";
            this.tabWalker.Size = new System.Drawing.Size(625, 292);
            this.tabWalker.Style = MetroFramework.MetroColorStyle.White;
            this.tabWalker.TabIndex = 2;
            this.tabWalker.Text = "Walker";
            this.tabWalker.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabWalker.VerticalScrollbarBarColor = true;
            // 
            // rbtnNW
            // 
            this.rbtnNW.AutoSize = true;
            this.rbtnNW.Location = new System.Drawing.Point(494, 64);
            this.rbtnNW.Name = "rbtnNW";
            this.rbtnNW.Size = new System.Drawing.Size(26, 15);
            this.rbtnNW.TabIndex = 29;
            this.rbtnNW.TabStop = true;
            this.rbtnNW.Text = " ";
            this.rbtnNW.UseVisualStyleBackColor = true;
            // 
            // btnDeleteWpt
            // 
            this.btnDeleteWpt.Location = new System.Drawing.Point(448, 201);
            this.btnDeleteWpt.Name = "btnDeleteWpt";
            this.btnDeleteWpt.Size = new System.Drawing.Size(40, 23);
            this.btnDeleteWpt.TabIndex = 27;
            this.btnDeleteWpt.Text = "delete";
            this.btnDeleteWpt.Click += new System.EventHandler(this.btnDeleteWpt_Click);
            // 
            // btnLoadWpt
            // 
            this.btnLoadWpt.Location = new System.Drawing.Point(448, 230);
            this.btnLoadWpt.Name = "btnLoadWpt";
            this.btnLoadWpt.Size = new System.Drawing.Size(40, 23);
            this.btnLoadWpt.TabIndex = 26;
            this.btnLoadWpt.Text = "load";
            this.btnLoadWpt.Click += new System.EventHandler(this.btnLoadWpt_Click);
            // 
            // btnSaveWpt
            // 
            this.btnSaveWpt.Location = new System.Drawing.Point(448, 259);
            this.btnSaveWpt.Name = "btnSaveWpt";
            this.btnSaveWpt.Size = new System.Drawing.Size(40, 23);
            this.btnSaveWpt.TabIndex = 25;
            this.btnSaveWpt.Text = "save";
            this.btnSaveWpt.Click += new System.EventHandler(this.btnSaveWpt_Click);
            // 
            // txtWaypointName
            // 
            this.txtWaypointName.Location = new System.Drawing.Point(494, 259);
            this.txtWaypointName.Name = "txtWaypointName";
            this.txtWaypointName.Size = new System.Drawing.Size(120, 23);
            this.txtWaypointName.TabIndex = 24;
            // 
            // lstSavedWaypoints
            // 
            this.lstSavedWaypoints.FormattingEnabled = true;
            this.lstSavedWaypoints.Location = new System.Drawing.Point(494, 170);
            this.lstSavedWaypoints.Name = "lstSavedWaypoints";
            this.lstSavedWaypoints.Size = new System.Drawing.Size(120, 82);
            this.lstSavedWaypoints.TabIndex = 23;
            this.lstSavedWaypoints.SelectedIndexChanged += new System.EventHandler(this.lstSavedWaypoints_SelectedIndexChanged);
            // 
            // btnWaypointClear
            // 
            this.btnWaypointClear.Location = new System.Drawing.Point(306, 170);
            this.btnWaypointClear.Name = "btnWaypointClear";
            this.btnWaypointClear.Size = new System.Drawing.Size(40, 23);
            this.btnWaypointClear.TabIndex = 22;
            this.btnWaypointClear.Text = "clear";
            this.btnWaypointClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // toggleWalker
            // 
            this.toggleWalker.AutoSize = true;
            this.toggleWalker.Location = new System.Drawing.Point(512, 27);
            this.toggleWalker.Name = "toggleWalker";
            this.toggleWalker.Size = new System.Drawing.Size(80, 17);
            this.toggleWalker.TabIndex = 21;
            this.toggleWalker.Text = "Off";
            this.toggleWalker.UseVisualStyleBackColor = true;
            this.toggleWalker.CheckedChanged += new System.EventHandler(this.toggleWalker_CheckedChanged);
            // 
            // rbtnSE
            // 
            this.rbtnSE.AutoSize = true;
            this.rbtnSE.Location = new System.Drawing.Point(558, 106);
            this.rbtnSE.Name = "rbtnSE";
            this.rbtnSE.Size = new System.Drawing.Size(26, 15);
            this.rbtnSE.TabIndex = 20;
            this.rbtnSE.TabStop = true;
            this.rbtnSE.Text = " ";
            this.rbtnSE.UseVisualStyleBackColor = true;
            // 
            // rbtnS
            // 
            this.rbtnS.AutoSize = true;
            this.rbtnS.Location = new System.Drawing.Point(526, 106);
            this.rbtnS.Name = "rbtnS";
            this.rbtnS.Size = new System.Drawing.Size(26, 15);
            this.rbtnS.TabIndex = 19;
            this.rbtnS.TabStop = true;
            this.rbtnS.Text = " ";
            this.rbtnS.UseVisualStyleBackColor = true;
            // 
            // rbtnSW
            // 
            this.rbtnSW.AutoSize = true;
            this.rbtnSW.Location = new System.Drawing.Point(494, 106);
            this.rbtnSW.Name = "rbtnSW";
            this.rbtnSW.Size = new System.Drawing.Size(26, 15);
            this.rbtnSW.TabIndex = 18;
            this.rbtnSW.TabStop = true;
            this.rbtnSW.Text = " ";
            this.rbtnSW.UseVisualStyleBackColor = true;
            // 
            // rbtnE
            // 
            this.rbtnE.AutoSize = true;
            this.rbtnE.Location = new System.Drawing.Point(558, 85);
            this.rbtnE.Name = "rbtnE";
            this.rbtnE.Size = new System.Drawing.Size(26, 15);
            this.rbtnE.TabIndex = 17;
            this.rbtnE.TabStop = true;
            this.rbtnE.Text = " ";
            this.rbtnE.UseVisualStyleBackColor = true;
            // 
            // rbtnC
            // 
            this.rbtnC.AutoSize = true;
            this.rbtnC.Location = new System.Drawing.Point(526, 85);
            this.rbtnC.Name = "rbtnC";
            this.rbtnC.Size = new System.Drawing.Size(26, 15);
            this.rbtnC.TabIndex = 16;
            this.rbtnC.TabStop = true;
            this.rbtnC.Text = " ";
            this.rbtnC.UseVisualStyleBackColor = true;
            // 
            // rbtnW
            // 
            this.rbtnW.AutoSize = true;
            this.rbtnW.Location = new System.Drawing.Point(494, 85);
            this.rbtnW.Name = "rbtnW";
            this.rbtnW.Size = new System.Drawing.Size(26, 15);
            this.rbtnW.TabIndex = 15;
            this.rbtnW.TabStop = true;
            this.rbtnW.Text = " ";
            this.rbtnW.UseVisualStyleBackColor = true;
            // 
            // rbtnNE
            // 
            this.rbtnNE.AutoSize = true;
            this.rbtnNE.Location = new System.Drawing.Point(558, 64);
            this.rbtnNE.Name = "rbtnNE";
            this.rbtnNE.Size = new System.Drawing.Size(26, 15);
            this.rbtnNE.TabIndex = 13;
            this.rbtnNE.TabStop = true;
            this.rbtnNE.Text = " ";
            this.rbtnNE.UseVisualStyleBackColor = true;
            // 
            // rbtnN
            // 
            this.rbtnN.AutoSize = true;
            this.rbtnN.Location = new System.Drawing.Point(526, 64);
            this.rbtnN.Name = "rbtnN";
            this.rbtnN.Size = new System.Drawing.Size(26, 15);
            this.rbtnN.TabIndex = 12;
            this.rbtnN.TabStop = true;
            this.rbtnN.Text = " ";
            this.rbtnN.UseVisualStyleBackColor = true;
            // 
            // btnUse
            // 
            this.btnUse.Location = new System.Drawing.Point(393, 143);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(72, 23);
            this.btnUse.TabIndex = 11;
            this.btnUse.Text = "Use";
            this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
            // 
            // btnShovel
            // 
            this.btnShovel.Location = new System.Drawing.Point(393, 114);
            this.btnShovel.Name = "btnShovel";
            this.btnShovel.Size = new System.Drawing.Size(72, 23);
            this.btnShovel.TabIndex = 10;
            this.btnShovel.Text = "Shovel";
            this.btnShovel.Click += new System.EventHandler(this.btnShovel_Click);
            // 
            // btnRope
            // 
            this.btnRope.Location = new System.Drawing.Point(393, 85);
            this.btnRope.Name = "btnRope";
            this.btnRope.Size = new System.Drawing.Size(72, 23);
            this.btnRope.TabIndex = 9;
            this.btnRope.Text = "Rope";
            this.btnRope.Click += new System.EventHandler(this.btnRope_Click);
            // 
            // btnStand
            // 
            this.btnStand.Location = new System.Drawing.Point(393, 56);
            this.btnStand.Name = "btnStand";
            this.btnStand.Size = new System.Drawing.Size(72, 23);
            this.btnStand.TabIndex = 8;
            this.btnStand.Text = "Stand";
            this.btnStand.Click += new System.EventHandler(this.btnStand_Click);
            // 
            // btnWalk
            // 
            this.btnWalk.Location = new System.Drawing.Point(393, 27);
            this.btnWalk.Name = "btnWalk";
            this.btnWalk.Size = new System.Drawing.Size(72, 23);
            this.btnWalk.TabIndex = 7;
            this.btnWalk.Text = "Walk";
            this.btnWalk.Click += new System.EventHandler(this.btnWalk_Click);
            // 
            // btnWaypointDelete
            // 
            this.btnWaypointDelete.Location = new System.Drawing.Point(306, 129);
            this.btnWaypointDelete.Name = "btnWaypointDelete";
            this.btnWaypointDelete.Size = new System.Drawing.Size(40, 23);
            this.btnWaypointDelete.TabIndex = 6;
            this.btnWaypointDelete.Text = "delete";
            this.btnWaypointDelete.Click += new System.EventHandler(this.btnWaypointDelete_Click);
            // 
            // btnWaypointEdit
            // 
            this.btnWaypointEdit.Location = new System.Drawing.Point(306, 100);
            this.btnWaypointEdit.Name = "btnWaypointEdit";
            this.btnWaypointEdit.Size = new System.Drawing.Size(40, 23);
            this.btnWaypointEdit.TabIndex = 5;
            this.btnWaypointEdit.Text = "edit";
            this.btnWaypointEdit.Click += new System.EventHandler(this.btnWaypointEdit_Click);
            // 
            // btnWaypointDown
            // 
            this.btnWaypointDown.Location = new System.Drawing.Point(306, 56);
            this.btnWaypointDown.Name = "btnWaypointDown";
            this.btnWaypointDown.Size = new System.Drawing.Size(40, 23);
            this.btnWaypointDown.TabIndex = 4;
            this.btnWaypointDown.Text = "down";
            this.btnWaypointDown.Click += new System.EventHandler(this.btnWaypointDown_Click);
            // 
            // btnWaypointUp
            // 
            this.btnWaypointUp.Location = new System.Drawing.Point(306, 27);
            this.btnWaypointUp.Name = "btnWaypointUp";
            this.btnWaypointUp.Size = new System.Drawing.Size(40, 23);
            this.btnWaypointUp.TabIndex = 3;
            this.btnWaypointUp.Text = "up";
            this.btnWaypointUp.Click += new System.EventHandler(this.btnWaypointUp_Click);
            // 
            // lstWaypoints
            // 
            this.lstWaypoints.FormattingEnabled = true;
            this.lstWaypoints.Location = new System.Drawing.Point(12, 27);
            this.lstWaypoints.Name = "lstWaypoints";
            this.lstWaypoints.Size = new System.Drawing.Size(288, 212);
            this.lstWaypoints.TabIndex = 2;
            // 
            // tabAutoLoot
            // 
            this.tabAutoLoot.Controls.Add(this.toggleAutoLoot);
            this.tabAutoLoot.HorizontalScrollbarBarColor = true;
            this.tabAutoLoot.Location = new System.Drawing.Point(4, 35);
            this.tabAutoLoot.Name = "tabAutoLoot";
            this.tabAutoLoot.Size = new System.Drawing.Size(625, 292);
            this.tabAutoLoot.TabIndex = 5;
            this.tabAutoLoot.Text = "AutoLoot";
            this.tabAutoLoot.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabAutoLoot.VerticalScrollbarBarColor = true;
            // 
            // toggleAutoLoot
            // 
            this.toggleAutoLoot.AutoSize = true;
            this.toggleAutoLoot.Location = new System.Drawing.Point(520, 15);
            this.toggleAutoLoot.Name = "toggleAutoLoot";
            this.toggleAutoLoot.Size = new System.Drawing.Size(80, 17);
            this.toggleAutoLoot.TabIndex = 2;
            this.toggleAutoLoot.Text = "Off";
            this.toggleAutoLoot.UseVisualStyleBackColor = true;
            this.toggleAutoLoot.CheckedChanged += new System.EventHandler(this.toggleAutoLoot_CheckedChanged);
            // 
            // tabTargeting
            // 
            this.tabTargeting.Controls.Add(this.btnTargetingAddNewPolicy);
            this.tabTargeting.Controls.Add(this.btnTargetingClearPolicyList);
            this.tabTargeting.Controls.Add(this.btnTargetingLoadPolicies);
            this.tabTargeting.Controls.Add(this.btnTargetingSavePolicies);
            this.tabTargeting.Controls.Add(this.wpfTargetingPolicyPanel);
            this.tabTargeting.Controls.Add(this.chkAttackAllCreaturesThatAttack);
            this.tabTargeting.Controls.Add(this.metroLabel5);
            this.tabTargeting.Controls.Add(this.chkSwitchToHigherPriority);
            this.tabTargeting.Controls.Add(this.metroLabel4);
            this.tabTargeting.Controls.Add(this.toggleTargeting);
            this.tabTargeting.HorizontalScrollbarBarColor = true;
            this.tabTargeting.Location = new System.Drawing.Point(4, 35);
            this.tabTargeting.Name = "tabTargeting";
            this.tabTargeting.Size = new System.Drawing.Size(625, 292);
            this.tabTargeting.Style = MetroFramework.MetroColorStyle.White;
            this.tabTargeting.TabIndex = 4;
            this.tabTargeting.Text = "Targeting";
            this.tabTargeting.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabTargeting.VerticalScrollbarBarColor = true;
            // 
            // btnTargetingAddNewPolicy
            // 
            this.btnTargetingAddNewPolicy.Location = new System.Drawing.Point(12, 198);
            this.btnTargetingAddNewPolicy.Name = "btnTargetingAddNewPolicy";
            this.btnTargetingAddNewPolicy.Size = new System.Drawing.Size(95, 18);
            this.btnTargetingAddNewPolicy.TabIndex = 24;
            this.btnTargetingAddNewPolicy.Text = "Add New";
            this.btnTargetingAddNewPolicy.Click += new System.EventHandler(this.btnTargetingAddNewPolicy_Click);
            // 
            // btnTargetingClearPolicyList
            // 
            this.btnTargetingClearPolicyList.Location = new System.Drawing.Point(113, 198);
            this.btnTargetingClearPolicyList.Name = "btnTargetingClearPolicyList";
            this.btnTargetingClearPolicyList.Size = new System.Drawing.Size(95, 18);
            this.btnTargetingClearPolicyList.TabIndex = 23;
            this.btnTargetingClearPolicyList.Text = "Clear";
            this.btnTargetingClearPolicyList.Click += new System.EventHandler(this.btnTargetingClearPolicyList_Click);
            // 
            // btnTargetingLoadPolicies
            // 
            this.btnTargetingLoadPolicies.Location = new System.Drawing.Point(317, 198);
            this.btnTargetingLoadPolicies.Name = "btnTargetingLoadPolicies";
            this.btnTargetingLoadPolicies.Size = new System.Drawing.Size(95, 18);
            this.btnTargetingLoadPolicies.TabIndex = 22;
            this.btnTargetingLoadPolicies.Text = "Load";
            this.btnTargetingLoadPolicies.Click += new System.EventHandler(this.btnTargetingLoadPolicies_Click);
            // 
            // btnTargetingSavePolicies
            // 
            this.btnTargetingSavePolicies.Location = new System.Drawing.Point(214, 198);
            this.btnTargetingSavePolicies.Name = "btnTargetingSavePolicies";
            this.btnTargetingSavePolicies.Size = new System.Drawing.Size(95, 18);
            this.btnTargetingSavePolicies.TabIndex = 21;
            this.btnTargetingSavePolicies.Text = "Save";
            this.btnTargetingSavePolicies.Click += new System.EventHandler(this.btnTargetingSavePolicies_Click);
            // 
            // wpfTargetingPolicyPanel
            // 
            this.wpfTargetingPolicyPanel.Location = new System.Drawing.Point(12, 12);
            this.wpfTargetingPolicyPanel.Name = "wpfTargetingPolicyPanel";
            this.wpfTargetingPolicyPanel.Size = new System.Drawing.Size(400, 180);
            this.wpfTargetingPolicyPanel.TabIndex = 18;
            this.wpfTargetingPolicyPanel.Child = null;
            // 
            // chkAttackAllCreaturesThatAttack
            // 
            this.chkAttackAllCreaturesThatAttack.AutoSize = true;
            this.chkAttackAllCreaturesThatAttack.Checked = true;
            this.chkAttackAllCreaturesThatAttack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttackAllCreaturesThatAttack.Location = new System.Drawing.Point(12, 270);
            this.chkAttackAllCreaturesThatAttack.Name = "chkAttackAllCreaturesThatAttack";
            this.chkAttackAllCreaturesThatAttack.Size = new System.Drawing.Size(80, 17);
            this.chkAttackAllCreaturesThatAttack.TabIndex = 17;
            this.chkAttackAllCreaturesThatAttack.Text = "On";
            this.chkAttackAllCreaturesThatAttack.UseVisualStyleBackColor = true;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(98, 270);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(231, 19);
            this.metroLabel5.TabIndex = 16;
            this.metroLabel5.Text = "Attack all Creatures that attacks player";
            this.metroLabel5.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // chkSwitchToHigherPriority
            // 
            this.chkSwitchToHigherPriority.AutoSize = true;
            this.chkSwitchToHigherPriority.Checked = true;
            this.chkSwitchToHigherPriority.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSwitchToHigherPriority.Location = new System.Drawing.Point(12, 247);
            this.chkSwitchToHigherPriority.Name = "chkSwitchToHigherPriority";
            this.chkSwitchToHigherPriority.Size = new System.Drawing.Size(80, 17);
            this.chkSwitchToHigherPriority.TabIndex = 15;
            this.chkSwitchToHigherPriority.Text = "On";
            this.chkSwitchToHigherPriority.UseVisualStyleBackColor = true;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(98, 247);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(262, 19);
            this.metroLabel4.TabIndex = 14;
            this.metroLabel4.Text = "Switch to Higher Priority target if it appears";
            this.metroLabel4.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // toggleTargeting
            // 
            this.toggleTargeting.AutoSize = true;
            this.toggleTargeting.Location = new System.Drawing.Point(534, 12);
            this.toggleTargeting.Name = "toggleTargeting";
            this.toggleTargeting.Size = new System.Drawing.Size(80, 17);
            this.toggleTargeting.TabIndex = 13;
            this.toggleTargeting.Text = "Off";
            this.toggleTargeting.UseVisualStyleBackColor = true;
            this.toggleTargeting.CheckedChanged += new System.EventHandler(this.toggleTargeting_CheckedChanged);
            // 
            // tabPython
            // 
            this.tabPython.Controls.Add(this.wpfPythonConsole);
            this.tabPython.HorizontalScrollbarBarColor = true;
            this.tabPython.Location = new System.Drawing.Point(4, 35);
            this.tabPython.Name = "tabPython";
            this.tabPython.Size = new System.Drawing.Size(625, 292);
            this.tabPython.TabIndex = 1;
            this.tabPython.Text = "Python";
            this.tabPython.VerticalScrollbarBarColor = true;
            // 
            // wpfPythonConsole
            // 
            this.wpfPythonConsole.Location = new System.Drawing.Point(3, 3);
            this.wpfPythonConsole.Name = "wpfPythonConsole";
            this.wpfPythonConsole.Size = new System.Drawing.Size(619, 286);
            this.wpfPythonConsole.TabIndex = 2;
            this.wpfPythonConsole.Text = "Python Console";
            this.wpfPythonConsole.Child = null;
            // 
            // metroToggle1
            // 
            this.metroToggle1.AutoSize = true;
            this.metroToggle1.Location = new System.Drawing.Point(560, 40);
            this.metroToggle1.Name = "metroToggle1";
            this.metroToggle1.Size = new System.Drawing.Size(80, 17);
            this.metroToggle1.TabIndex = 1;
            this.metroToggle1.Text = "Off";
            this.metroToggle1.UseVisualStyleBackColor = true;
            this.metroToggle1.CheckedChanged += new System.EventHandler(this.toggleTopMost_CheckedChanged);
            // 
            // metroLabel11
            // 
            this.metroLabel11.AutoSize = true;
            this.metroLabel11.Location = new System.Drawing.Point(489, 38);
            this.metroLabel11.Name = "metroLabel11";
            this.metroLabel11.Size = new System.Drawing.Size(65, 19);
            this.metroLabel11.TabIndex = 2;
            this.metroLabel11.Text = "Top Most";
            this.metroLabel11.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 400);
            this.Controls.Add(this.metroLabel11);
            this.Controls.Add(this.tabController);
            this.Controls.Add(this.metroToggle1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.Text = "Zion Bot";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.msmMain)).EndInit();
            this.tabController.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            this.tabLogger.ResumeLayout(false);
            this.tabScripting.ResumeLayout(false);
            this.tabWalker.ResumeLayout(false);
            this.tabWalker.PerformLayout();
            this.tabAutoLoot.ResumeLayout(false);
            this.tabAutoLoot.PerformLayout();
            this.tabTargeting.ResumeLayout(false);
            this.tabTargeting.PerformLayout();
            this.tabPython.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager msmMain;
        private MetroFramework.Controls.MetroTabControl tabController;
        private MetroFramework.Controls.MetroTabPage tabMain;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTabPage tabPython;
        private System.Windows.Forms.Integration.ElementHost wpfPythonConsole;
        private Shell shell1;
        private MetroFramework.Controls.MetroTabPage tabWalker;
        private MetroFramework.Controls.MetroRadioButton rbtnSE;
        private MetroFramework.Controls.MetroRadioButton rbtnS;
        private MetroFramework.Controls.MetroRadioButton rbtnSW;
        private MetroFramework.Controls.MetroRadioButton rbtnE;
        private MetroFramework.Controls.MetroRadioButton rbtnC;
        private MetroFramework.Controls.MetroRadioButton rbtnW;
        private MetroFramework.Controls.MetroRadioButton rbtnNE;
        private MetroFramework.Controls.MetroRadioButton rbtnN;
        private MetroFramework.Controls.MetroButton btnUse;
        private MetroFramework.Controls.MetroButton btnShovel;
        private MetroFramework.Controls.MetroButton btnRope;
        private MetroFramework.Controls.MetroButton btnStand;
        private MetroFramework.Controls.MetroButton btnWalk;
        private MetroFramework.Controls.MetroButton btnWaypointDelete;
        private MetroFramework.Controls.MetroButton btnWaypointEdit;
        private MetroFramework.Controls.MetroButton btnWaypointDown;
        private MetroFramework.Controls.MetroButton btnWaypointUp;
        private System.Windows.Forms.ListBox lstWaypoints;
        private MetroFramework.Controls.MetroToggle toggleWalker;
        private MetroFramework.Controls.MetroButton btnWaypointClear;
        private MetroFramework.Controls.MetroTabPage tabLogger;
        private System.Windows.Forms.RichTextBox rtxtLog;
        private MetroFramework.Controls.MetroToggle metroToggle1;
        private MetroFramework.Controls.MetroButton btnDeleteWpt;
        private MetroFramework.Controls.MetroButton btnLoadWpt;
        private MetroFramework.Controls.MetroButton btnSaveWpt;
        private MetroFramework.Controls.MetroTextBox txtWaypointName;
        private System.Windows.Forms.ListBox lstSavedWaypoints;
        private MetroFramework.Controls.MetroButton btnLogsClear;
        private MetroFramework.Controls.MetroRadioButton rbtnNW;
        private MetroFramework.Controls.MetroTabPage tabTargeting;
        private MetroFramework.Controls.MetroToggle toggleTargeting;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroToggle chkAttackAllCreaturesThatAttack;
        private MetroFramework.Controls.MetroTabPage tabAutoLoot;
        private MetroFramework.Controls.MetroToggle toggleAutoLoot;
        private MetroFramework.Controls.MetroLabel metroLabel11;
        private MetroFramework.Controls.MetroButton btnTargetingSavePolicies;
        private System.Windows.Forms.Integration.ElementHost wpfTargetingPolicyPanel;
        private MetroFramework.Controls.MetroToggle chkSwitchToHigherPriority;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroButton btnTargetingAddNewPolicy;
        private MetroFramework.Controls.MetroButton btnTargetingClearPolicyList;
        private MetroFramework.Controls.MetroButton btnTargetingLoadPolicies;
        private MetroFramework.Controls.MetroTabPage tabScripting;
        private System.Windows.Forms.Integration.ElementHost wpfScriptsHost;
        private MetroFramework.Controls.MetroButton btnNewScript;
    }
}