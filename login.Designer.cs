namespace SsbAdmin {
  partial class login {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
      this.srv_grpCred = new System.Windows.Forms.GroupBox();
      this.srv_txtPwd = new System.Windows.Forms.TextBox();
      this.srv_txtUid = new System.Windows.Forms.TextBox();
      this.label49 = new System.Windows.Forms.Label();
      this.label48 = new System.Windows.Forms.Label();
      this.srv_cboAuth = new System.Windows.Forms.ComboBox();
      this.label47 = new System.Windows.Forms.Label();
      this.srv_txtName = new System.Windows.Forms.TextBox();
      this.label46 = new System.Windows.Forms.Label();
      this.srv_btnOK = new System.Windows.Forms.Button();
      this.srv_btnCancel = new System.Windows.Forms.Button();
      this.srv_grpCred.SuspendLayout();
      this.SuspendLayout();
      // 
      // srv_grpCred
      // 
      this.srv_grpCred.Controls.Add(this.srv_txtPwd);
      this.srv_grpCred.Controls.Add(this.srv_txtUid);
      this.srv_grpCred.Controls.Add(this.label49);
      this.srv_grpCred.Controls.Add(this.label48);
      this.srv_grpCred.Location = new System.Drawing.Point(62, 97);
      this.srv_grpCred.Name = "srv_grpCred";
      this.srv_grpCred.Size = new System.Drawing.Size(251, 82);
      this.srv_grpCred.TabIndex = 14;
      this.srv_grpCred.TabStop = false;
      this.srv_grpCred.Text = "Credentials";
      // 
      // srv_txtPwd
      // 
      this.srv_txtPwd.Location = new System.Drawing.Point(78, 48);
      this.srv_txtPwd.Name = "srv_txtPwd";
      this.srv_txtPwd.PasswordChar = '*';
      this.srv_txtPwd.Size = new System.Drawing.Size(156, 20);
      this.srv_txtPwd.TabIndex = 10;
      // 
      // srv_txtUid
      // 
      this.srv_txtUid.Location = new System.Drawing.Point(78, 25);
      this.srv_txtUid.Name = "srv_txtUid";
      this.srv_txtUid.Size = new System.Drawing.Size(156, 20);
      this.srv_txtUid.TabIndex = 9;
      // 
      // label49
      // 
      this.label49.AutoSize = true;
      this.label49.Location = new System.Drawing.Point(22, 51);
      this.label49.Name = "label49";
      this.label49.Size = new System.Drawing.Size(53, 13);
      this.label49.TabIndex = 8;
      this.label49.Text = "Password";
      this.label49.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // label48
      // 
      this.label48.AutoSize = true;
      this.label48.Location = new System.Drawing.Point(15, 25);
      this.label48.Name = "label48";
      this.label48.Size = new System.Drawing.Size(60, 13);
      this.label48.TabIndex = 7;
      this.label48.Text = "User Name";
      this.label48.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // srv_cboAuth
      // 
      this.srv_cboAuth.FormattingEnabled = true;
      this.srv_cboAuth.Location = new System.Drawing.Point(112, 70);
      this.srv_cboAuth.Name = "srv_cboAuth";
      this.srv_cboAuth.Size = new System.Drawing.Size(234, 21);
      this.srv_cboAuth.TabIndex = 13;
      this.srv_cboAuth.SelectedIndexChanged += new System.EventHandler(this.srv_cboAuth_SelectedIndexChanged);
      // 
      // label47
      // 
      this.label47.AutoSize = true;
      this.label47.Location = new System.Drawing.Point(31, 73);
      this.label47.Name = "label47";
      this.label47.Size = new System.Drawing.Size(75, 13);
      this.label47.TabIndex = 12;
      this.label47.Text = "Authentication";
      this.label47.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // srv_txtName
      // 
      this.srv_txtName.Location = new System.Drawing.Point(112, 35);
      this.srv_txtName.Name = "srv_txtName";
      this.srv_txtName.Size = new System.Drawing.Size(234, 20);
      this.srv_txtName.TabIndex = 10;
      // 
      // label46
      // 
      this.label46.AutoSize = true;
      this.label46.Location = new System.Drawing.Point(37, 38);
      this.label46.Name = "label46";
      this.label46.Size = new System.Drawing.Size(69, 13);
      this.label46.TabIndex = 11;
      this.label46.Text = "Server Name";
      this.label46.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // srv_btnOK
      // 
      this.srv_btnOK.Location = new System.Drawing.Point(58, 202);
      this.srv_btnOK.Name = "srv_btnOK";
      this.srv_btnOK.Size = new System.Drawing.Size(75, 23);
      this.srv_btnOK.TabIndex = 15;
      this.srv_btnOK.Text = "OK";
      this.srv_btnOK.Click += new System.EventHandler(this.srv_btnOK_Click);
      // 
      // srv_btnCancel
      // 
      this.srv_btnCancel.Location = new System.Drawing.Point(257, 202);
      this.srv_btnCancel.Name = "srv_btnCancel";
      this.srv_btnCancel.Size = new System.Drawing.Size(75, 23);
      this.srv_btnCancel.TabIndex = 16;
      this.srv_btnCancel.Text = "Cancel";
      this.srv_btnCancel.Click += new System.EventHandler(this.srv_btnCancel_Click);
      // 
      // login
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(393, 234);
      this.Controls.Add(this.srv_btnCancel);
      this.Controls.Add(this.srv_btnOK);
      this.Controls.Add(this.srv_grpCred);
      this.Controls.Add(this.srv_cboAuth);
      this.Controls.Add(this.label47);
      this.Controls.Add(this.srv_txtName);
      this.Controls.Add(this.label46);
      this.Name = "login";
      this.Text = "login";
      this.srv_grpCred.ResumeLayout(false);
      this.srv_grpCred.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox srv_grpCred;
    public System.Windows.Forms.TextBox srv_txtPwd;
    public System.Windows.Forms.TextBox srv_txtUid;
    private System.Windows.Forms.Label label49;
    private System.Windows.Forms.Label label48;
    public System.Windows.Forms.ComboBox srv_cboAuth;
    private System.Windows.Forms.Label label47;
    private System.Windows.Forms.TextBox srv_txtName;
    private System.Windows.Forms.Label label46;
    public System.Windows.Forms.Button srv_btnOK;
    private System.Windows.Forms.Button srv_btnCancel;

  }
}