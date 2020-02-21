using PerfCap;
using System.Windows.Forms;

namespace jPerf
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jPXLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.helpFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton5 = new System.Windows.Forms.ToolStripDropDownButton();
            this.markersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMarkerListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.showMarkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smoothModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.startRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFlagButton = new System.Windows.Forms.ToolStripButton();
            this.statusIcon = new System.Windows.Forms.ToolStripStatusLabel();
            this.noneToolStripMenuItem = new PerfCap.ToolStripRadioButton();
            this.moderateToolStripMenuItem = new PerfCap.ToolStripRadioButton();
            this.highToolStripMenuItem = new PerfCap.ToolStripRadioButton();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton4,
            this.toolStripDropDownButton5,
            this.toolStripDropDownButton3,
            this.toolStripDropDownButton2,
            this.addFlagButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(609, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // plotView1
            // 
            this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotView1.Location = new System.Drawing.Point(0, 28);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(609, 310);
            this.plotView1.TabIndex = 4;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusIcon,
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 328);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(609, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(48, 17);
            this.statusText.Text = "Status...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::PerfCap.Properties.Resources.background_transparent;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(609, 303);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jPXLogFileToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // jPXLogFileToolStripMenuItem
            // 
            this.jPXLogFileToolStripMenuItem.Name = "jPXLogFileToolStripMenuItem";
            this.jPXLogFileToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.jPXLogFileToolStripMenuItem.Text = "JPerf Marker File (.JPM)";
            this.jPXLogFileToolStripMenuItem.Click += new System.EventHandler(this.jPMLogFileToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(107, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.toolStripDropDownButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton4.Image")));
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton4.Text = "Help";
            // 
            // helpFileToolStripMenuItem
            // 
            this.helpFileToolStripMenuItem.Name = "helpFileToolStripMenuItem";
            this.helpFileToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.helpFileToolStripMenuItem.Text = "Help File";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(117, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton5
            // 
            this.toolStripDropDownButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markersToolStripMenuItem});
            this.toolStripDropDownButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton5.Image")));
            this.toolStripDropDownButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton5.Name = "toolStripDropDownButton5";
            this.toolStripDropDownButton5.Size = new System.Drawing.Size(40, 22);
            this.toolStripDropDownButton5.Text = "Edit";
            // 
            // markersToolStripMenuItem
            // 
            this.markersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMarkerListToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.markersToolStripMenuItem.Name = "markersToolStripMenuItem";
            this.markersToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.markersToolStripMenuItem.Text = "Markers";
            // 
            // viewMarkerListToolStripMenuItem
            // 
            this.viewMarkerListToolStripMenuItem.Name = "viewMarkerListToolStripMenuItem";
            this.viewMarkerListToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.viewMarkerListToolStripMenuItem.Text = "Edit Markers";
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMarkersToolStripMenuItem,
            this.smoothModeToolStripMenuItem});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton3.Text = "View";
            // 
            // showMarkersToolStripMenuItem
            // 
            this.showMarkersToolStripMenuItem.Checked = true;
            this.showMarkersToolStripMenuItem.CheckOnClick = true;
            this.showMarkersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMarkersToolStripMenuItem.Name = "showMarkersToolStripMenuItem";
            this.showMarkersToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.showMarkersToolStripMenuItem.Text = "Show Markers";
            // 
            // smoothModeToolStripMenuItem
            // 
            this.smoothModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.moderateToolStripMenuItem,
            this.highToolStripMenuItem});
            this.smoothModeToolStripMenuItem.Name = "smoothModeToolStripMenuItem";
            this.smoothModeToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.smoothModeToolStripMenuItem.Text = "Smooth Mode";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startRecordingToolStripMenuItem,
            this.stopRecordingToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(57, 22);
            this.toolStripDropDownButton2.Text = "Record";
            // 
            // startRecordingToolStripMenuItem
            // 
            this.startRecordingToolStripMenuItem.Name = "startRecordingToolStripMenuItem";
            this.startRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.startRecordingToolStripMenuItem.Text = "Start Recording";
            this.startRecordingToolStripMenuItem.Click += new System.EventHandler(this.startRecordingToolStripMenuItem_Click);
            // 
            // stopRecordingToolStripMenuItem
            // 
            this.stopRecordingToolStripMenuItem.Enabled = false;
            this.stopRecordingToolStripMenuItem.Name = "stopRecordingToolStripMenuItem";
            this.stopRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.stopRecordingToolStripMenuItem.Text = "Stop Recording";
            this.stopRecordingToolStripMenuItem.Click += new System.EventHandler(this.stopRecordingToolStripMenuItem_Click);
            // 
            // addFlagButton
            // 
            this.addFlagButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addFlagButton.Enabled = false;
            this.addFlagButton.Image = global::PerfCap.Properties.Resources.FlagIcon_red;
            this.addFlagButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFlagButton.Name = "addFlagButton";
            this.addFlagButton.Size = new System.Drawing.Size(23, 22);
            this.addFlagButton.Text = "Create a Marker";
            this.addFlagButton.Click += new System.EventHandler(this.addFlagButton_Click);
            // 
            // statusIcon
            // 
            this.statusIcon.ForeColor = System.Drawing.Color.Green;
            this.statusIcon.Name = "statusIcon";
            this.statusIcon.Size = new System.Drawing.Size(19, 17);
            this.statusIcon.Text = "✅";
            this.statusIcon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.noneToolStripMenuItem.Text = "None";
            // 
            // moderateToolStripMenuItem
            // 
            this.moderateToolStripMenuItem.Name = "moderateToolStripMenuItem";
            this.moderateToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.moderateToolStripMenuItem.Text = "Moderate";
            // 
            // highToolStripMenuItem
            // 
            this.highToolStripMenuItem.Name = "highToolStripMenuItem";
            this.highToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.highToolStripMenuItem.Text = "High";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 350);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.plotView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "JPerf";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem startRecordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopRecordingToolStripMenuItem;
        private OxyPlot.WindowsForms.PlotView plotView1;
        private System.Windows.Forms.ToolStripButton addFlagButton;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jPXLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem smoothModeToolStripMenuItem;
        private ToolStripMenuItem showMarkersToolStripMenuItem;
        private ToolStripRadioButton noneToolStripMenuItem;
        private ToolStripRadioButton moderateToolStripMenuItem;
        private ToolStripRadioButton highToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton4;
        private ToolStripMenuItem helpFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton5;
        private ToolStripMenuItem markersToolStripMenuItem;
        private ToolStripMenuItem viewMarkerListToolStripMenuItem;
        private ToolStripMenuItem clearAllToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusText;
        private PictureBox pictureBox1;
        private ToolStripStatusLabel statusIcon;
    }
}

