namespace TGEditor
{
    partial class Editor
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sceneOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ambientLightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.particleEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TranslationGizmoButton = new System.Windows.Forms.Button();
            this.RotationGizmoButton = new System.Windows.Forms.Button();
            this.ScaleGizmoButton = new System.Windows.Forms.Button();
            this.SnapSpacingButton = new System.Windows.Forms.Button();
            this.PointLightButton = new System.Windows.Forms.Button();
            this.EntityCollideCallbackButton = new System.Windows.Forms.Button();
            this.CreatePlaneButton = new System.Windows.Forms.Button();
            this.UniformScaleGizmoButton = new System.Windows.Forms.Button();
            this.TranslationSnapTextBox = new System.Windows.Forms.TextBox();
            this.GridSpacingLabel = new System.Windows.Forms.Label();
            this.RotationSnapTextBox = new System.Windows.Forms.TextBox();
            this.RotationLabel = new System.Windows.Forms.Label();
            this.ScaleSnapTextBox = new System.Windows.Forms.TextBox();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.ResetSnapButton = new System.Windows.Forms.Button();
            this.XnaDisplayPanel = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SpotLightButton = new System.Windows.Forms.Button();
            this.DefaultLightButton = new System.Windows.Forms.Button();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ImportedModelsListBox = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MultiTex4Button = new System.Windows.Forms.Button();
            this.MultiTex4TextBox = new System.Windows.Forms.TextBox();
            this.MultiTex3Button = new System.Windows.Forms.Button();
            this.MultiTex3TextBox = new System.Windows.Forms.TextBox();
            this.MultiTex2Button = new System.Windows.Forms.Button();
            this.MultiTex2TextBox = new System.Windows.Forms.TextBox();
            this.MultiTex1Button = new System.Windows.Forms.Button();
            this.MultiTex1TextBox = new System.Windows.Forms.TextBox();
            this.GlowTextureButton = new System.Windows.Forms.Button();
            this.GlowTextureTextBox = new System.Windows.Forms.TextBox();
            this.NormalTextureButton = new System.Windows.Forms.Button();
            this.NormalTextureTextBox = new System.Windows.Forms.TextBox();
            this.SpecularTextureButton = new System.Windows.Forms.Button();
            this.SpecularTextureTextBox = new System.Windows.Forms.TextBox();
            this.DiffuesTextureButton = new System.Windows.Forms.Button();
            this.DiffuseTextureTextBox = new System.Windows.Forms.TextBox();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SoundButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.importToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1298, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.importSceneToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // importSceneToolStripMenuItem
            // 
            this.importSceneToolStripMenuItem.Name = "importSceneToolStripMenuItem";
            this.importSceneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.importSceneToolStripMenuItem.Text = "Import Scene";
            this.importSceneToolStripMenuItem.Click += new System.EventHandler(this.importSceneToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boundingBoxToolStripMenuItem,
            this.sceneOptionsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // boundingBoxToolStripMenuItem
            // 
            this.boundingBoxToolStripMenuItem.Name = "boundingBoxToolStripMenuItem";
            this.boundingBoxToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.boundingBoxToolStripMenuItem.Text = "BoundingBox";
            this.boundingBoxToolStripMenuItem.Click += new System.EventHandler(this.boundingBoxToolStripMenuItem_Click);
            // 
            // sceneOptionsToolStripMenuItem
            // 
            this.sceneOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ambientLightToolStripMenuItem});
            this.sceneOptionsToolStripMenuItem.Name = "sceneOptionsToolStripMenuItem";
            this.sceneOptionsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.sceneOptionsToolStripMenuItem.Text = "SceneOptions";
            // 
            // ambientLightToolStripMenuItem
            // 
            this.ambientLightToolStripMenuItem.Name = "ambientLightToolStripMenuItem";
            this.ambientLightToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.ambientLightToolStripMenuItem.Text = "AmbientLight";
            this.ambientLightToolStripMenuItem.Click += new System.EventHandler(this.ambientLightToolStripMenuItem_Click_1);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.modelToolStripMenuItem.Text = "Model";
            this.modelToolStripMenuItem.Click += new System.EventHandler(this.modelToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem,
            this.particleEditorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // modelEditorToolStripMenuItem
            // 
            this.modelEditorToolStripMenuItem.Name = "modelEditorToolStripMenuItem";
            this.modelEditorToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.modelEditorToolStripMenuItem.Text = "Model Editor";
            // 
            // particleEditorToolStripMenuItem
            // 
            this.particleEditorToolStripMenuItem.Name = "particleEditorToolStripMenuItem";
            this.particleEditorToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.particleEditorToolStripMenuItem.Text = "Particle Editor";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // TranslationGizmoButton
            // 
            this.TranslationGizmoButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.TranslationGizmoButton.Location = new System.Drawing.Point(731, 648);
            this.TranslationGizmoButton.Name = "TranslationGizmoButton";
            this.TranslationGizmoButton.Size = new System.Drawing.Size(45, 45);
            this.TranslationGizmoButton.TabIndex = 2;
            this.TranslationGizmoButton.Text = "Translation";
            this.TranslationGizmoButton.UseVisualStyleBackColor = false;
            this.TranslationGizmoButton.Click += new System.EventHandler(this.TranslationGizmoButton_Click);
            // 
            // RotationGizmoButton
            // 
            this.RotationGizmoButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.RotationGizmoButton.Location = new System.Drawing.Point(782, 648);
            this.RotationGizmoButton.Name = "RotationGizmoButton";
            this.RotationGizmoButton.Size = new System.Drawing.Size(45, 45);
            this.RotationGizmoButton.TabIndex = 3;
            this.RotationGizmoButton.Text = "Rotation";
            this.RotationGizmoButton.UseVisualStyleBackColor = false;
            this.RotationGizmoButton.Click += new System.EventHandler(this.RotationGizmoButton_Click);
            // 
            // ScaleGizmoButton
            // 
            this.ScaleGizmoButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ScaleGizmoButton.Location = new System.Drawing.Point(833, 648);
            this.ScaleGizmoButton.Name = "ScaleGizmoButton";
            this.ScaleGizmoButton.Size = new System.Drawing.Size(45, 45);
            this.ScaleGizmoButton.TabIndex = 4;
            this.ScaleGizmoButton.Text = "Scale";
            this.ScaleGizmoButton.UseVisualStyleBackColor = false;
            this.ScaleGizmoButton.Click += new System.EventHandler(this.ScaleGizmoButton_Click);
            // 
            // SnapSpacingButton
            // 
            this.SnapSpacingButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SnapSpacingButton.Location = new System.Drawing.Point(986, 648);
            this.SnapSpacingButton.Name = "SnapSpacingButton";
            this.SnapSpacingButton.Size = new System.Drawing.Size(45, 45);
            this.SnapSpacingButton.TabIndex = 6;
            this.SnapSpacingButton.Text = "Snap";
            this.SnapSpacingButton.UseVisualStyleBackColor = false;
            this.SnapSpacingButton.Click += new System.EventHandler(this.SnapSpacingButton_Click);
            // 
            // PointLightButton
            // 
            this.PointLightButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PointLightButton.Location = new System.Drawing.Point(952, 29);
            this.PointLightButton.Name = "PointLightButton";
            this.PointLightButton.Size = new System.Drawing.Size(45, 45);
            this.PointLightButton.TabIndex = 11;
            this.PointLightButton.Text = "PointLight";
            this.PointLightButton.UseVisualStyleBackColor = false;
            this.PointLightButton.Click += new System.EventHandler(this.PointLightButton_Click);
            // 
            // EntityCollideCallbackButton
            // 
            this.EntityCollideCallbackButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.EntityCollideCallbackButton.Location = new System.Drawing.Point(952, 131);
            this.EntityCollideCallbackButton.Name = "EntityCollideCallbackButton";
            this.EntityCollideCallbackButton.Size = new System.Drawing.Size(45, 45);
            this.EntityCollideCallbackButton.TabIndex = 12;
            this.EntityCollideCallbackButton.Text = "Collide";
            this.EntityCollideCallbackButton.UseVisualStyleBackColor = false;
            this.EntityCollideCallbackButton.Click += new System.EventHandler(this.EntityCollideCallbackButton_Click);
            // 
            // CreatePlaneButton
            // 
            this.CreatePlaneButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CreatePlaneButton.Location = new System.Drawing.Point(952, 182);
            this.CreatePlaneButton.Name = "CreatePlaneButton";
            this.CreatePlaneButton.Size = new System.Drawing.Size(45, 45);
            this.CreatePlaneButton.TabIndex = 13;
            this.CreatePlaneButton.Text = "Plane";
            this.CreatePlaneButton.UseVisualStyleBackColor = false;
            this.CreatePlaneButton.Click += new System.EventHandler(this.CreatePlaneButton_Click);
            // 
            // UniformScaleGizmoButton
            // 
            this.UniformScaleGizmoButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.UniformScaleGizmoButton.Location = new System.Drawing.Point(884, 648);
            this.UniformScaleGizmoButton.Name = "UniformScaleGizmoButton";
            this.UniformScaleGizmoButton.Size = new System.Drawing.Size(45, 45);
            this.UniformScaleGizmoButton.TabIndex = 14;
            this.UniformScaleGizmoButton.Text = "UniformScale";
            this.UniformScaleGizmoButton.UseVisualStyleBackColor = false;
            this.UniformScaleGizmoButton.Click += new System.EventHandler(this.UniformScaleButton_Click);
            // 
            // TranslationSnapTextBox
            // 
            this.TranslationSnapTextBox.Location = new System.Drawing.Point(1108, 649);
            this.TranslationSnapTextBox.Name = "TranslationSnapTextBox";
            this.TranslationSnapTextBox.Size = new System.Drawing.Size(45, 20);
            this.TranslationSnapTextBox.TabIndex = 15;
            this.TranslationSnapTextBox.Text = "10";
            this.TranslationSnapTextBox.TextChanged += new System.EventHandler(this.TranslationSnapTextBox_TextChanged);
            // 
            // GridSpacingLabel
            // 
            this.GridSpacingLabel.AutoSize = true;
            this.GridSpacingLabel.Location = new System.Drawing.Point(1037, 651);
            this.GridSpacingLabel.Name = "GridSpacingLabel";
            this.GridSpacingLabel.Size = new System.Drawing.Size(65, 13);
            this.GridSpacingLabel.TabIndex = 16;
            this.GridSpacingLabel.Text = "Translation :";
            // 
            // RotationSnapTextBox
            // 
            this.RotationSnapTextBox.Location = new System.Drawing.Point(1218, 649);
            this.RotationSnapTextBox.Name = "RotationSnapTextBox";
            this.RotationSnapTextBox.Size = new System.Drawing.Size(44, 20);
            this.RotationSnapTextBox.TabIndex = 17;
            this.RotationSnapTextBox.Text = "30";
            this.RotationSnapTextBox.TextChanged += new System.EventHandler(this.RotationSnapTextBox_TextChanged);
            // 
            // RotationLabel
            // 
            this.RotationLabel.AutoSize = true;
            this.RotationLabel.Location = new System.Drawing.Point(1159, 652);
            this.RotationLabel.Name = "RotationLabel";
            this.RotationLabel.Size = new System.Drawing.Size(53, 13);
            this.RotationLabel.TabIndex = 18;
            this.RotationLabel.Text = "Rotation :";
            // 
            // ScaleSnapTextBox
            // 
            this.ScaleSnapTextBox.Location = new System.Drawing.Point(1108, 674);
            this.ScaleSnapTextBox.Name = "ScaleSnapTextBox";
            this.ScaleSnapTextBox.Size = new System.Drawing.Size(45, 20);
            this.ScaleSnapTextBox.TabIndex = 19;
            this.ScaleSnapTextBox.Text = "0.5";
            this.ScaleSnapTextBox.TextChanged += new System.EventHandler(this.ScaleSnapTextBox_TextChanged);
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.AutoSize = true;
            this.ScaleLabel.Location = new System.Drawing.Point(1062, 674);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(40, 13);
            this.ScaleLabel.TabIndex = 20;
            this.ScaleLabel.Text = "Scale :";
            // 
            // ResetSnapButton
            // 
            this.ResetSnapButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ResetSnapButton.Location = new System.Drawing.Point(1162, 672);
            this.ResetSnapButton.Name = "ResetSnapButton";
            this.ResetSnapButton.Size = new System.Drawing.Size(100, 23);
            this.ResetSnapButton.TabIndex = 21;
            this.ResetSnapButton.Text = "Reset Snap";
            this.ResetSnapButton.UseVisualStyleBackColor = false;
            this.ResetSnapButton.Click += new System.EventHandler(this.ResetSnapButton_Click_1);
            // 
            // XnaDisplayPanel
            // 
            this.XnaDisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.XnaDisplayPanel.Location = new System.Drawing.Point(0, 27);
            this.XnaDisplayPanel.Name = "XnaDisplayPanel";
            this.XnaDisplayPanel.Size = new System.Drawing.Size(724, 712);
            this.XnaDisplayPanel.TabIndex = 24;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(731, 27);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(216, 615);
            this.propertyGrid1.TabIndex = 0;
            // 
            // SpotLightButton
            // 
            this.SpotLightButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SpotLightButton.Location = new System.Drawing.Point(952, 80);
            this.SpotLightButton.Name = "SpotLightButton";
            this.SpotLightButton.Size = new System.Drawing.Size(45, 45);
            this.SpotLightButton.TabIndex = 25;
            this.SpotLightButton.Text = "SpotLight";
            this.SpotLightButton.UseVisualStyleBackColor = false;
            this.SpotLightButton.Click += new System.EventHandler(this.SpotLightButton_Click);
            // 
            // DefaultLightButton
            // 
            this.DefaultLightButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.DefaultLightButton.Location = new System.Drawing.Point(730, 699);
            this.DefaultLightButton.Name = "DefaultLightButton";
            this.DefaultLightButton.Size = new System.Drawing.Size(45, 45);
            this.DefaultLightButton.TabIndex = 26;
            this.DefaultLightButton.Text = "DefaultLight";
            this.DefaultLightButton.UseVisualStyleBackColor = false;
            this.DefaultLightButton.Click += new System.EventHandler(this.DefaultLightButton_Click);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.tabPage1);
            this.MainTabControl.Controls.Add(this.tabPage2);
            this.MainTabControl.Location = new System.Drawing.Point(1004, 27);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(258, 615);
            this.MainTabControl.TabIndex = 27;
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage1.Controls.Add(this.ImportedModelsListBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(250, 589);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Models";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ImportedModelsListBox
            // 
            this.ImportedModelsListBox.FormattingEnabled = true;
            this.ImportedModelsListBox.Location = new System.Drawing.Point(-2, -2);
            this.ImportedModelsListBox.Name = "ImportedModelsListBox";
            this.ImportedModelsListBox.Size = new System.Drawing.Size(250, 589);
            this.ImportedModelsListBox.TabIndex = 22;
            this.ImportedModelsListBox.SelectedIndexChanged += new System.EventHandler(this.ImportedModelsListBox_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.MultiTex4Button);
            this.tabPage2.Controls.Add(this.MultiTex4TextBox);
            this.tabPage2.Controls.Add(this.MultiTex3Button);
            this.tabPage2.Controls.Add(this.MultiTex3TextBox);
            this.tabPage2.Controls.Add(this.MultiTex2Button);
            this.tabPage2.Controls.Add(this.MultiTex2TextBox);
            this.tabPage2.Controls.Add(this.MultiTex1Button);
            this.tabPage2.Controls.Add(this.MultiTex1TextBox);
            this.tabPage2.Controls.Add(this.GlowTextureButton);
            this.tabPage2.Controls.Add(this.GlowTextureTextBox);
            this.tabPage2.Controls.Add(this.NormalTextureButton);
            this.tabPage2.Controls.Add(this.NormalTextureTextBox);
            this.tabPage2.Controls.Add(this.SpecularTextureButton);
            this.tabPage2.Controls.Add(this.SpecularTextureTextBox);
            this.tabPage2.Controls.Add(this.DiffuesTextureButton);
            this.tabPage2.Controls.Add(this.DiffuseTextureTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(250, 589);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TriangleMesh",
            "Box",
            "Sphere",
            "Capsule"});
            this.comboBox1.Location = new System.Drawing.Point(100, 231);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(45, 189);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Texture4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(45, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Texture3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Texture2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Texture1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Glow";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Normal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Specular";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Diffuse";
            // 
            // MultiTex4Button
            // 
            this.MultiTex4Button.Location = new System.Drawing.Point(207, 186);
            this.MultiTex4Button.Name = "MultiTex4Button";
            this.MultiTex4Button.Size = new System.Drawing.Size(32, 20);
            this.MultiTex4Button.TabIndex = 15;
            this.MultiTex4Button.Text = "...";
            this.MultiTex4Button.UseVisualStyleBackColor = true;
            this.MultiTex4Button.Click += new System.EventHandler(this.MultiTex4Button_Click);
            // 
            // MultiTex4TextBox
            // 
            this.MultiTex4TextBox.Location = new System.Drawing.Point(100, 186);
            this.MultiTex4TextBox.Name = "MultiTex4TextBox";
            this.MultiTex4TextBox.Size = new System.Drawing.Size(100, 20);
            this.MultiTex4TextBox.TabIndex = 14;
            // 
            // MultiTex3Button
            // 
            this.MultiTex3Button.Location = new System.Drawing.Point(207, 160);
            this.MultiTex3Button.Name = "MultiTex3Button";
            this.MultiTex3Button.Size = new System.Drawing.Size(32, 20);
            this.MultiTex3Button.TabIndex = 13;
            this.MultiTex3Button.Text = "...";
            this.MultiTex3Button.UseVisualStyleBackColor = true;
            this.MultiTex3Button.Click += new System.EventHandler(this.MultiTex3Button_Click);
            // 
            // MultiTex3TextBox
            // 
            this.MultiTex3TextBox.Location = new System.Drawing.Point(100, 160);
            this.MultiTex3TextBox.Name = "MultiTex3TextBox";
            this.MultiTex3TextBox.Size = new System.Drawing.Size(100, 20);
            this.MultiTex3TextBox.TabIndex = 12;
            // 
            // MultiTex2Button
            // 
            this.MultiTex2Button.Location = new System.Drawing.Point(207, 134);
            this.MultiTex2Button.Name = "MultiTex2Button";
            this.MultiTex2Button.Size = new System.Drawing.Size(32, 20);
            this.MultiTex2Button.TabIndex = 11;
            this.MultiTex2Button.Text = "...";
            this.MultiTex2Button.UseVisualStyleBackColor = true;
            this.MultiTex2Button.Click += new System.EventHandler(this.MultiTex2Button_Click);
            // 
            // MultiTex2TextBox
            // 
            this.MultiTex2TextBox.Location = new System.Drawing.Point(100, 134);
            this.MultiTex2TextBox.Name = "MultiTex2TextBox";
            this.MultiTex2TextBox.Size = new System.Drawing.Size(100, 20);
            this.MultiTex2TextBox.TabIndex = 10;
            // 
            // MultiTex1Button
            // 
            this.MultiTex1Button.Location = new System.Drawing.Point(207, 108);
            this.MultiTex1Button.Name = "MultiTex1Button";
            this.MultiTex1Button.Size = new System.Drawing.Size(32, 20);
            this.MultiTex1Button.TabIndex = 9;
            this.MultiTex1Button.Text = "...";
            this.MultiTex1Button.UseVisualStyleBackColor = true;
            this.MultiTex1Button.Click += new System.EventHandler(this.MultiTex1Button_Click);
            // 
            // MultiTex1TextBox
            // 
            this.MultiTex1TextBox.Location = new System.Drawing.Point(100, 108);
            this.MultiTex1TextBox.Name = "MultiTex1TextBox";
            this.MultiTex1TextBox.Size = new System.Drawing.Size(100, 20);
            this.MultiTex1TextBox.TabIndex = 8;
            // 
            // GlowTextureButton
            // 
            this.GlowTextureButton.Location = new System.Drawing.Point(207, 82);
            this.GlowTextureButton.Name = "GlowTextureButton";
            this.GlowTextureButton.Size = new System.Drawing.Size(32, 20);
            this.GlowTextureButton.TabIndex = 7;
            this.GlowTextureButton.Text = "...";
            this.GlowTextureButton.UseVisualStyleBackColor = true;
            this.GlowTextureButton.Click += new System.EventHandler(this.GlowTextureButton_Click);
            // 
            // GlowTextureTextBox
            // 
            this.GlowTextureTextBox.Location = new System.Drawing.Point(101, 82);
            this.GlowTextureTextBox.Name = "GlowTextureTextBox";
            this.GlowTextureTextBox.Size = new System.Drawing.Size(100, 20);
            this.GlowTextureTextBox.TabIndex = 6;
            // 
            // NormalTextureButton
            // 
            this.NormalTextureButton.Location = new System.Drawing.Point(207, 56);
            this.NormalTextureButton.Name = "NormalTextureButton";
            this.NormalTextureButton.Size = new System.Drawing.Size(32, 20);
            this.NormalTextureButton.TabIndex = 5;
            this.NormalTextureButton.Text = "...";
            this.NormalTextureButton.UseVisualStyleBackColor = true;
            this.NormalTextureButton.Click += new System.EventHandler(this.NormalTextureButton_Click);
            // 
            // NormalTextureTextBox
            // 
            this.NormalTextureTextBox.Location = new System.Drawing.Point(100, 56);
            this.NormalTextureTextBox.Name = "NormalTextureTextBox";
            this.NormalTextureTextBox.Size = new System.Drawing.Size(100, 20);
            this.NormalTextureTextBox.TabIndex = 4;
            // 
            // SpecularTextureButton
            // 
            this.SpecularTextureButton.Location = new System.Drawing.Point(207, 30);
            this.SpecularTextureButton.Name = "SpecularTextureButton";
            this.SpecularTextureButton.Size = new System.Drawing.Size(32, 20);
            this.SpecularTextureButton.TabIndex = 3;
            this.SpecularTextureButton.Text = "...";
            this.SpecularTextureButton.UseVisualStyleBackColor = true;
            this.SpecularTextureButton.Click += new System.EventHandler(this.SpecularTextureButton_Click);
            // 
            // SpecularTextureTextBox
            // 
            this.SpecularTextureTextBox.Location = new System.Drawing.Point(100, 30);
            this.SpecularTextureTextBox.Name = "SpecularTextureTextBox";
            this.SpecularTextureTextBox.Size = new System.Drawing.Size(100, 20);
            this.SpecularTextureTextBox.TabIndex = 2;
            // 
            // DiffuesTextureButton
            // 
            this.DiffuesTextureButton.Location = new System.Drawing.Point(207, 4);
            this.DiffuesTextureButton.Name = "DiffuesTextureButton";
            this.DiffuesTextureButton.Size = new System.Drawing.Size(32, 20);
            this.DiffuesTextureButton.TabIndex = 1;
            this.DiffuesTextureButton.Text = "...";
            this.DiffuesTextureButton.UseVisualStyleBackColor = true;
            this.DiffuesTextureButton.Click += new System.EventHandler(this.DiffuesTextureButton_Click);
            // 
            // DiffuseTextureTextBox
            // 
            this.DiffuseTextureTextBox.Location = new System.Drawing.Point(100, 4);
            this.DiffuseTextureTextBox.Name = "DiffuseTextureTextBox";
            this.DiffuseTextureTextBox.Size = new System.Drawing.Size(100, 20);
            this.DiffuseTextureTextBox.TabIndex = 0;
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // SoundButton
            // 
            this.SoundButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SoundButton.Location = new System.Drawing.Point(952, 233);
            this.SoundButton.Name = "SoundButton";
            this.SoundButton.Size = new System.Drawing.Size(45, 45);
            this.SoundButton.TabIndex = 28;
            this.SoundButton.Text = "Sound";
            this.SoundButton.UseVisualStyleBackColor = false;
            this.SoundButton.Click += new System.EventHandler(this.SoundButton_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1298, 759);
            this.Controls.Add(this.SoundButton);
            this.Controls.Add(this.DefaultLightButton);
            this.Controls.Add(this.SpotLightButton);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.CreatePlaneButton);
            this.Controls.Add(this.XnaDisplayPanel);
            this.Controls.Add(this.EntityCollideCallbackButton);
            this.Controls.Add(this.ResetSnapButton);
            this.Controls.Add(this.ScaleLabel);
            this.Controls.Add(this.ScaleSnapTextBox);
            this.Controls.Add(this.RotationLabel);
            this.Controls.Add(this.RotationSnapTextBox);
            this.Controls.Add(this.GridSpacingLabel);
            this.Controls.Add(this.TranslationSnapTextBox);
            this.Controls.Add(this.UniformScaleGizmoButton);
            this.Controls.Add(this.PointLightButton);
            this.Controls.Add(this.SnapSpacingButton);
            this.Controls.Add(this.ScaleGizmoButton);
            this.Controls.Add(this.RotationGizmoButton);
            this.Controls.Add(this.TranslationGizmoButton);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.MainTabControl);
            this.Name = "Editor";
            this.Text = "TGEditor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boundingBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button TranslationGizmoButton;
        private System.Windows.Forms.Button ScaleGizmoButton;
        private System.Windows.Forms.Button SnapSpacingButton;
        private System.Windows.Forms.Button PointLightButton;
        private System.Windows.Forms.Button EntityCollideCallbackButton;
        private System.Windows.Forms.Button CreatePlaneButton;
        private System.Windows.Forms.Button UniformScaleGizmoButton;
        private System.Windows.Forms.TextBox TranslationSnapTextBox;
        private System.Windows.Forms.Label GridSpacingLabel;
        private System.Windows.Forms.TextBox RotationSnapTextBox;
        private System.Windows.Forms.Label RotationLabel;
        private System.Windows.Forms.TextBox ScaleSnapTextBox;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.Button ResetSnapButton;
        internal System.Windows.Forms.Button RotationGizmoButton;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem particleEditorToolStripMenuItem;
        private System.Windows.Forms.Panel XnaDisplayPanel;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Button SpotLightButton;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSceneToolStripMenuItem;
        private System.Windows.Forms.Button DefaultLightButton;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.ListBox ImportedModelsListBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button DiffuesTextureButton;
        private System.Windows.Forms.TextBox DiffuseTextureTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MultiTex4Button;
        private System.Windows.Forms.TextBox MultiTex4TextBox;
        private System.Windows.Forms.Button MultiTex3Button;
        private System.Windows.Forms.TextBox MultiTex3TextBox;
        private System.Windows.Forms.Button MultiTex2Button;
        private System.Windows.Forms.TextBox MultiTex2TextBox;
        private System.Windows.Forms.Button MultiTex1Button;
        private System.Windows.Forms.TextBox MultiTex1TextBox;
        private System.Windows.Forms.Button GlowTextureButton;
        private System.Windows.Forms.TextBox GlowTextureTextBox;
        private System.Windows.Forms.Button NormalTextureButton;
        private System.Windows.Forms.TextBox NormalTextureTextBox;
        private System.Windows.Forms.Button SpecularTextureButton;
        private System.Windows.Forms.TextBox SpecularTextureTextBox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem sceneOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ambientLightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Button SoundButton;
    }
}