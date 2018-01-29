namespace LFVMapEdit
{
	partial class FrmMapEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMapEdit));
            LFVMapControler.GridBrick gridBrick1 = new LFVMapControler.GridBrick();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowGridInFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbcMapEdit = new System.Windows.Forms.TabControl();
            this.tbpMapProperties = new System.Windows.Forms.TabPage();
            this.nudRows = new System.Windows.Forms.NumericUpDown();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            this.dudWidth = new System.Windows.Forms.DomainUpDown();
            this.dudHeigth = new System.Windows.Forms.DomainUpDown();
            this.lblSize = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblIndices = new System.Windows.Forms.Label();
            this.tbpBricks = new System.Windows.Forms.TabPage();
            this.btnAddBrick = new System.Windows.Forms.Button();
            this.ltvBricks = new System.Windows.Forms.ListView();
            this.tbpBrickProperties = new System.Windows.Forms.TabPage();
            this.brickGrid1 = new LFVMapEdit.BrickGrid();
            this.btnEditGrid = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pcmMapPaint = new LFVMapEdit.PictureMap();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.pencilToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.fillBrushToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.lineToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.circleToolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.levelToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.ofdMapEdit = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tbcMapEdit.SuspendLayout();
            this.tbpMapProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            this.tbpBricks.SuspendLayout();
            this.tbpBrickProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brickGrid1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcmMapPaint)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(718, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showGridToolStripMenuItem,
            this.ShowGridInFrontToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.Checked = true;
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.showGridToolStripMenuItem.Text = "&Show Grid";
            this.showGridToolStripMenuItem.Click += new System.EventHandler(this.showGridToolStripMenuItem_Click);
            // 
            // ShowGridInFrontToolStripMenuItem
            // 
            this.ShowGridInFrontToolStripMenuItem.Checked = true;
            this.ShowGridInFrontToolStripMenuItem.CheckOnClick = true;
            this.ShowGridInFrontToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGridInFrontToolStripMenuItem.Name = "ShowGridInFrontToolStripMenuItem";
            this.ShowGridInFrontToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ShowGridInFrontToolStripMenuItem.Text = "Show Grid In &Front";
            this.ShowGridInFrontToolStripMenuItem.Click += new System.EventHandler(this.ShowGridInFrontToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(718, 459);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.tbcMapEdit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 459);
            this.panel2.TabIndex = 1;
            // 
            // tbcMapEdit
            // 
            this.tbcMapEdit.Controls.Add(this.tbpMapProperties);
            this.tbcMapEdit.Controls.Add(this.tbpBricks);
            this.tbcMapEdit.Controls.Add(this.tbpBrickProperties);
            this.tbcMapEdit.Location = new System.Drawing.Point(0, 0);
            this.tbcMapEdit.Name = "tbcMapEdit";
            this.tbcMapEdit.SelectedIndex = 0;
            this.tbcMapEdit.Size = new System.Drawing.Size(239, 459);
            this.tbcMapEdit.TabIndex = 0;
            // 
            // tbpMapProperties
            // 
            this.tbpMapProperties.Controls.Add(this.button1);
            this.tbpMapProperties.Controls.Add(this.nudRows);
            this.tbpMapProperties.Controls.Add(this.nudColumns);
            this.tbpMapProperties.Controls.Add(this.btnApplyChanges);
            this.tbpMapProperties.Controls.Add(this.dudWidth);
            this.tbpMapProperties.Controls.Add(this.dudHeigth);
            this.tbpMapProperties.Controls.Add(this.lblSize);
            this.tbpMapProperties.Controls.Add(this.label5);
            this.tbpMapProperties.Controls.Add(this.label4);
            this.tbpMapProperties.Controls.Add(this.label3);
            this.tbpMapProperties.Controls.Add(this.label2);
            this.tbpMapProperties.Controls.Add(this.label1);
            this.tbpMapProperties.Controls.Add(this.lblIndices);
            this.tbpMapProperties.Location = new System.Drawing.Point(4, 22);
            this.tbpMapProperties.Name = "tbpMapProperties";
            this.tbpMapProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMapProperties.Size = new System.Drawing.Size(231, 433);
            this.tbpMapProperties.TabIndex = 0;
            this.tbpMapProperties.Text = "Map";
            this.tbpMapProperties.UseVisualStyleBackColor = true;
            // 
            // nudRows
            // 
            this.nudRows.Location = new System.Drawing.Point(63, 82);
            this.nudRows.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRows.Name = "nudRows";
            this.nudRows.Size = new System.Drawing.Size(66, 20);
            this.nudRows.TabIndex = 3;
            this.nudRows.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nudColumns
            // 
            this.nudColumns.Location = new System.Drawing.Point(63, 56);
            this.nudColumns.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudColumns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Size = new System.Drawing.Size(66, 20);
            this.nudColumns.TabIndex = 3;
            this.nudColumns.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Location = new System.Drawing.Point(75, 121);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(106, 23);
            this.btnApplyChanges.TabIndex = 2;
            this.btnApplyChanges.Text = "&Apply Changes";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
            // 
            // dudWidth
            // 
            this.dudWidth.Items.Add("8");
            this.dudWidth.Items.Add("16");
            this.dudWidth.Items.Add("32");
            this.dudWidth.Items.Add("48");
            this.dudWidth.Items.Add("64");
            this.dudWidth.Items.Add("128");
            this.dudWidth.Location = new System.Drawing.Point(135, 30);
            this.dudWidth.Name = "dudWidth";
            this.dudWidth.Size = new System.Drawing.Size(46, 20);
            this.dudWidth.TabIndex = 1;
            this.dudWidth.Text = "16";
            // 
            // dudHeigth
            // 
            this.dudHeigth.Items.Add("8");
            this.dudHeigth.Items.Add("16");
            this.dudHeigth.Items.Add("32");
            this.dudHeigth.Items.Add("48");
            this.dudHeigth.Items.Add("64");
            this.dudHeigth.Items.Add("128");
            this.dudHeigth.Location = new System.Drawing.Point(63, 30);
            this.dudHeigth.Name = "dudHeigth";
            this.dudHeigth.Size = new System.Drawing.Size(46, 20);
            this.dudHeigth.TabIndex = 1;
            this.dudHeigth.Text = "16";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(60, 105);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(44, 13);
            this.lblSize.TabIndex = 0;
            this.lblSize.Text = "30 X 30";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Rows:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Columns:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tile Size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // lblIndices
            // 
            this.lblIndices.AutoSize = true;
            this.lblIndices.Location = new System.Drawing.Point(9, 6);
            this.lblIndices.Name = "lblIndices";
            this.lblIndices.Size = new System.Drawing.Size(35, 13);
            this.lblIndices.TabIndex = 0;
            this.lblIndices.Text = "label1";
            // 
            // tbpBricks
            // 
            this.tbpBricks.Controls.Add(this.btnAddBrick);
            this.tbpBricks.Controls.Add(this.ltvBricks);
            this.tbpBricks.Location = new System.Drawing.Point(4, 22);
            this.tbpBricks.Name = "tbpBricks";
            this.tbpBricks.Padding = new System.Windows.Forms.Padding(3);
            this.tbpBricks.Size = new System.Drawing.Size(231, 433);
            this.tbpBricks.TabIndex = 1;
            this.tbpBricks.Text = "Bricks";
            this.tbpBricks.UseVisualStyleBackColor = true;
            // 
            // btnAddBrick
            // 
            this.btnAddBrick.Location = new System.Drawing.Point(150, 287);
            this.btnAddBrick.Name = "btnAddBrick";
            this.btnAddBrick.Size = new System.Drawing.Size(75, 26);
            this.btnAddBrick.TabIndex = 1;
            this.btnAddBrick.Text = "&Add";
            this.btnAddBrick.UseVisualStyleBackColor = true;
            this.btnAddBrick.Click += new System.EventHandler(this.btnAddBrick_Click);
            // 
            // ltvBricks
            // 
            this.ltvBricks.Location = new System.Drawing.Point(6, 6);
            this.ltvBricks.MultiSelect = false;
            this.ltvBricks.Name = "ltvBricks";
            this.ltvBricks.OwnerDraw = true;
            this.ltvBricks.Size = new System.Drawing.Size(219, 275);
            this.ltvBricks.TabIndex = 0;
            this.ltvBricks.UseCompatibleStateImageBehavior = false;
            this.ltvBricks.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ltvBricks_DrawItem);
            this.ltvBricks.SelectedIndexChanged += new System.EventHandler(this.ltvBricks_SelectedIndexChanged);
            // 
            // tbpBrickProperties
            // 
            this.tbpBrickProperties.AutoScroll = true;
            this.tbpBrickProperties.Controls.Add(this.brickGrid1);
            this.tbpBrickProperties.Controls.Add(this.btnEditGrid);
            this.tbpBrickProperties.Location = new System.Drawing.Point(4, 22);
            this.tbpBrickProperties.Name = "tbpBrickProperties";
            this.tbpBrickProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tbpBrickProperties.Size = new System.Drawing.Size(231, 433);
            this.tbpBrickProperties.TabIndex = 2;
            this.tbpBrickProperties.Text = "Brick Properties";
            this.tbpBrickProperties.UseVisualStyleBackColor = true;
            // 
            // brickGrid1
            // 
            this.brickGrid1.Image = ((System.Drawing.Image)(resources.GetObject("brickGrid1.Image")));
            this.brickGrid1.Location = new System.Drawing.Point(3, 6);
            this.brickGrid1.Name = "brickGrid1";
            this.brickGrid1.NewImage = null;
            this.brickGrid1.QtdColumns = 20;
            this.brickGrid1.QtdRows = 20;
            this.brickGrid1.Size = new System.Drawing.Size(320, 320);
            this.brickGrid1.TabIndex = 2;
            this.brickGrid1.TabStop = false;
            this.brickGrid1.TileHeigth = 16;
            this.brickGrid1.TileWidth = 16;
            // 
            // btnEditGrid
            // 
            this.btnEditGrid.Location = new System.Drawing.Point(6, 388);
            this.btnEditGrid.Name = "btnEditGrid";
            this.btnEditGrid.Size = new System.Drawing.Size(83, 23);
            this.btnEditGrid.TabIndex = 1;
            this.btnEditGrid.Text = "Edit";
            this.btnEditGrid.UseVisualStyleBackColor = true;
            this.btnEditGrid.Click += new System.EventHandler(this.btnEditGrid_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pcmMapPaint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 434);
            this.panel1.TabIndex = 2;
            // 
            // pcmMapPaint
            // 
            this.pcmMapPaint.CurrentLayer = 0;
            gridBrick1.Images = null;
            gridBrick1.QtdColumns = 0;
            gridBrick1.QtdRows = 0;
            this.pcmMapPaint.GridBrick = gridBrick1;
            this.pcmMapPaint.Location = new System.Drawing.Point(3, 3);
            this.pcmMapPaint.Name = "pcmMapPaint";
            this.pcmMapPaint.QtdColumns = 20;
            this.pcmMapPaint.QtdRows = 20;
            this.pcmMapPaint.ShowGrid = true;
            this.pcmMapPaint.ShowGridInFront = true;
            this.pcmMapPaint.Size = new System.Drawing.Size(320, 320);
            this.pcmMapPaint.TabIndex = 0;
            this.pcmMapPaint.TabStop = false;
            this.pcmMapPaint.TileHeigth = 16;
            this.pcmMapPaint.TileWidth = 16;
            this.pcmMapPaint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pcmMapPaint_MouseMove);
            this.pcmMapPaint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcmMapPaint_MouseDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pencilToolStripButton,
            this.fillBrushToolStripButton,
            this.lineToolStripButton,
            this.circleToolStripButton1,
            this.copyToolStripButton,
            this.levelToolStripComboBox});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(475, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // pencilToolStripButton
            // 
            this.pencilToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pencilToolStripButton.Image = global::LFVMapEdit.Properties.Resources.Pencil;
            this.pencilToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pencilToolStripButton.Name = "pencilToolStripButton";
            this.pencilToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pencilToolStripButton.Text = "pencilToolStripButton";
            this.pencilToolStripButton.Click += new System.EventHandler(this.pencilToolStripButton_Click);
            // 
            // fillBrushToolStripButton
            // 
            this.fillBrushToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fillBrushToolStripButton.Image = global::LFVMapEdit.Properties.Resources.FillBrush;
            this.fillBrushToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fillBrushToolStripButton.Name = "fillBrushToolStripButton";
            this.fillBrushToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.fillBrushToolStripButton.Text = "pencilToolStripButton";
            this.fillBrushToolStripButton.Click += new System.EventHandler(this.fillBrushToolStripButton_Click);
            // 
            // lineToolStripButton
            // 
            this.lineToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lineToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("lineToolStripButton.Image")));
            this.lineToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lineToolStripButton.Name = "lineToolStripButton";
            this.lineToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.lineToolStripButton.Text = "lineToolStripButton";
            this.lineToolStripButton.Click += new System.EventHandler(this.lineToolStripButton_Click);
            // 
            // circleToolStripButton1
            // 
            this.circleToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.circleToolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("circleToolStripButton1.Image")));
            this.circleToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.circleToolStripButton1.Name = "circleToolStripButton1";
            this.circleToolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.circleToolStripButton1.Text = "circleToolStripButton1";
            this.circleToolStripButton1.ToolTipText = "circleToolStripButton1";
            this.circleToolStripButton1.Click += new System.EventHandler(this.circleToolStripButton1_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "Copy";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
            // 
            // levelToolStripComboBox
            // 
            this.levelToolStripComboBox.DropDownHeight = 50;
            this.levelToolStripComboBox.IntegralHeight = false;
            this.levelToolStripComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.levelToolStripComboBox.Name = "levelToolStripComboBox";
            this.levelToolStripComboBox.Size = new System.Drawing.Size(75, 25);
            this.levelToolStripComboBox.Text = "1";
            this.levelToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.levelToolStripComboBox_SelectedIndexChanged);
            // 
            // ofdMapEdit
            // 
            this.ofdMapEdit.Multiselect = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMapEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 483);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMapEdit";
            this.Text = "Map Edit";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tbcMapEdit.ResumeLayout(false);
            this.tbpMapProperties.ResumeLayout(false);
            this.tbpMapProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            this.tbpBricks.ResumeLayout(false);
            this.tbpBrickProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.brickGrid1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcmMapPaint)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabControl tbcMapEdit;
		private System.Windows.Forms.TabPage tbpMapProperties;
		private System.Windows.Forms.TabPage tbpBricks;
		private System.Windows.Forms.TabPage tbpBrickProperties;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private PictureMap pcmMapPaint;
		private System.Windows.Forms.Label lblIndices;
        private System.Windows.Forms.Button btnApplyChanges;
        private System.Windows.Forms.DomainUpDown dudHeigth;
        private System.Windows.Forms.DomainUpDown dudWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudRows;
        private System.Windows.Forms.NumericUpDown nudColumns;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowGridInFrontToolStripMenuItem;
        private System.Windows.Forms.ListView ltvBricks;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton pencilToolStripButton;
        private System.Windows.Forms.ToolStripButton fillBrushToolStripButton;
        private System.Windows.Forms.OpenFileDialog ofdMapEdit;
        private System.Windows.Forms.Button btnAddBrick;
		private System.Windows.Forms.ToolStripButton lineToolStripButton;
		private System.Windows.Forms.ToolStripButton circleToolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripComboBox levelToolStripComboBox;
        private System.Windows.Forms.Button btnEditGrid;
        private BrickGrid brickGrid1;
        private System.Windows.Forms.Button button1;
	}
}

