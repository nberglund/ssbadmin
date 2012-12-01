namespace SsbAdmin {
  partial class addServer {
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
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lbNetWork = new System.Windows.Forms.ListBox();
      this.lbChosen = new System.Windows.Forms.ListBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnAdd = new System.Windows.Forms.Button();
      this.btnBrowse = new System.Windows.Forms.Button();
      this.btnAddFromList = new System.Windows.Forms.Button();
      this.btnRemoveFromList = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(92, 93);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(232, 20);
      this.textBox1.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(23, 96);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(63, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Server name";
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(128, 322);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(258, 322);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      // 
      // lbNetWork
      // 
      this.lbNetWork.FormattingEnabled = true;
      this.lbNetWork.Location = new System.Drawing.Point(17, 157);
      this.lbNetWork.Name = "lbNetWork";
      this.lbNetWork.Size = new System.Drawing.Size(165, 147);
      this.lbNetWork.TabIndex = 4;
      // 
      // lbChosen
      // 
      this.lbChosen.FormattingEnabled = true;
      this.lbChosen.Location = new System.Drawing.Point(290, 157);
      this.lbChosen.Name = "lbChosen";
      this.lbChosen.Size = new System.Drawing.Size(175, 147);
      this.lbChosen.TabIndex = 5;
      lbChosen.DisplayMember = "Name";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
      this.label2.Location = new System.Drawing.Point(20, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(333, 48);
      this.label2.TabIndex = 6;
      this.label2.Text = "Enter name of server to add in the Server name text box.\r\nIf you want to add more" +
          " servers, click on the Add button.\r\nTo browse for servers click on the Browse bu" +
          "tton.";
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(343, 90);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(75, 23);
      this.btnAdd.TabIndex = 7;
      this.btnAdd.Text = "Add";
      
      // 
      // btnBrowse
      // 
      this.btnBrowse.Location = new System.Drawing.Point(107, 128);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new System.Drawing.Size(75, 23);
      this.btnBrowse.TabIndex = 8;
      this.btnBrowse.Text = "Browse...";
      
      // 
      // btnAddFromList
      // 
      this.btnAddFromList.Location = new System.Drawing.Point(200, 213);
      this.btnAddFromList.Name = "btnAddFromList";
      this.btnAddFromList.Size = new System.Drawing.Size(75, 23);
      this.btnAddFromList.TabIndex = 9;
      this.btnAddFromList.Text = ">>>";
      this.btnAddFromList.Click += new System.EventHandler(this.btnAddFromList_Click);
      // 
      // btnRemoveFromList
      // 
      this.btnRemoveFromList.Location = new System.Drawing.Point(200, 256);
      this.btnRemoveFromList.Name = "btnRemoveFromList";
      this.btnRemoveFromList.Size = new System.Drawing.Size(75, 23);
      this.btnRemoveFromList.TabIndex = 10;
      this.btnRemoveFromList.Text = "<<<";
      
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(16, 138);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(85, 13);
      this.label3.TabIndex = 11;
      this.label3.Text = "Available Servers";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(289, 138);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(84, 13);
      this.label4.TabIndex = 12;
      this.label4.Text = "Selected Servers";
      // 
      // addServer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(477, 356);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.btnRemoveFromList);
      this.Controls.Add(this.btnAddFromList);
      this.Controls.Add(this.btnBrowse);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.lbChosen);
      this.Controls.Add(this.lbNetWork);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.Name = "addServer";
      this.Text = "Add/Browse Server(s)";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ListBox lbNetWork;
    private System.Windows.Forms.ListBox lbChosen;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.Button btnAddFromList;
    private System.Windows.Forms.Button btnRemoveFromList;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
  }
}