namespace SsbAdmin {
  partial class Form3 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.tv1 = new System.Windows.Forms.TreeView();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnPaste = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer1.Location = new System.Drawing.Point(0, 24);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.tv1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
      this.splitContainer1.Panel2.Controls.Add(this.btnPaste);
      this.splitContainer1.Size = new System.Drawing.Size(283, 335);
      this.splitContainer1.SplitterDistance = 281;
      this.splitContainer1.TabIndex = 4;
      this.splitContainer1.Text = "splitContainer1";
      // 
      // tv1
      // 
      this.tv1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tv1.Location = new System.Drawing.Point(0, 0);
      this.tv1.Name = "tv1";
      this.tv1.Size = new System.Drawing.Size(283, 281);
      this.tv1.TabIndex = 0;
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(172, 15);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Exit";
      // 
      // btnPaste
      // 
      this.btnPaste.Location = new System.Drawing.Point(26, 15);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size(86, 23);
      this.btnPaste.TabIndex = 0;
      this.btnPaste.Text = "Paste/Deploy";
      // 
      // menuStrip1
      // 
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(283, 24);
      this.menuStrip1.TabIndex = 5;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // Form3
      // 
      this.ClientSize = new System.Drawing.Size(283, 359);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "Form3";
      this.Text = "Form3";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TreeView tv1;
    private System.Windows.Forms.Button btnPaste;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.MenuStrip menuStrip1;

  }
}