#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace SsbAdmin {
  partial class Form1 {
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
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.label17 = new System.Windows.Forms.Label();
      this.textBox12 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.label18 = new System.Windows.Forms.Label();
      this.textBox14 = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.q_dgvStatus = new System.Windows.Forms.DataGridView();
      this.q_dvgMsg = new System.Windows.Forms.DataGridView();
      this.textBox10 = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.textBox9 = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.Label();
      this.textBox8 = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.textBox6 = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.checkBox3 = new System.Windows.Forms.CheckBox();
      this.label8 = new System.Windows.Forms.Label();
      this.checkBox2 = new System.Windows.Forms.CheckBox();
      this.label7 = new System.Windows.Forms.Label();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.label6 = new System.Windows.Forms.Label();
      this.textBox5 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.tabPage4 = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label48 = new System.Windows.Forms.Label();
      this.label47 = new System.Windows.Forms.Label();
      this.srv_cboCnvState = new System.Windows.Forms.ComboBox();
      this.srv_cboSource = new System.Windows.Forms.ComboBox();
      this.label34 = new System.Windows.Forms.Label();
      this.label33 = new System.Windows.Forms.Label();
      this.dvgMsg = new System.Windows.Forms.DataGridView();
      this.dvgConv = new System.Windows.Forms.DataGridView();
      this.label19 = new System.Windows.Forms.Label();
      this.textBox15 = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.Label();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.textBox13 = new System.Windows.Forms.TextBox();
      this.label16 = new System.Windows.Forms.Label();
      this.textBox11 = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.Label();
      this.tabPage5 = new System.Windows.Forms.TabPage();
      this.label25 = new System.Windows.Forms.Label();
      this.label24 = new System.Windows.Forms.Label();
      this.textBox22 = new System.Windows.Forms.TextBox();
      this.textBox21 = new System.Windows.Forms.TextBox();
      this.textBox18 = new System.Windows.Forms.TextBox();
      this.label22 = new System.Windows.Forms.Label();
      this.textBox19 = new System.Windows.Forms.TextBox();
      this.label23 = new System.Windows.Forms.Label();
      this.textBox20 = new System.Windows.Forms.TextBox();
      this.label26 = new System.Windows.Forms.Label();
      this.label20 = new System.Windows.Forms.Label();
      this.textBox16 = new System.Windows.Forms.TextBox();
      this.textBox17 = new System.Windows.Forms.TextBox();
      this.label21 = new System.Windows.Forms.Label();
      this.tabPage6 = new System.Windows.Forms.TabPage();
      this.checkBox4 = new System.Windows.Forms.CheckBox();
      this.label32 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.textBox27 = new System.Windows.Forms.TextBox();
      this.textBox26 = new System.Windows.Forms.TextBox();
      this.label30 = new System.Windows.Forms.Label();
      this.textBox25 = new System.Windows.Forms.TextBox();
      this.label29 = new System.Windows.Forms.Label();
      this.label28 = new System.Windows.Forms.Label();
      this.textBox24 = new System.Windows.Forms.TextBox();
      this.textBox23 = new System.Windows.Forms.TextBox();
      this.label27 = new System.Windows.Forms.Label();
      this.tabPage7 = new System.Windows.Forms.TabPage();
      this.label38 = new System.Windows.Forms.Label();
      this.cnv_chkInitiator = new System.Windows.Forms.CheckBox();
      this.label37 = new System.Windows.Forms.Label();
      this.cnv_txtLifeTime = new System.Windows.Forms.TextBox();
      this.cnv_txtFarBroker = new System.Windows.Forms.TextBox();
      this.label36 = new System.Windows.Forms.Label();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.dvgCnvMsgs = new System.Windows.Forms.DataGridView();
      this.cnv_txtContract = new System.Windows.Forms.TextBox();
      this.cnv_txtFrmServ = new System.Windows.Forms.TextBox();
      this.label45 = new System.Windows.Forms.Label();
      this.cnv_txtState = new System.Windows.Forms.TextBox();
      this.label44 = new System.Windows.Forms.Label();
      this.cnv_txtRelGrpHndl = new System.Windows.Forms.TextBox();
      this.label42 = new System.Windows.Forms.Label();
      this.label40 = new System.Windows.Forms.Label();
      this.label39 = new System.Windows.Forms.Label();
      this.cnv_txtToSrv = new System.Windows.Forms.TextBox();
      this.cnv_txtDlgGuid = new System.Windows.Forms.TextBox();
      this.label35 = new System.Windows.Forms.Label();
      this.tabPage8 = new System.Windows.Forms.TabPage();
      this.label77 = new System.Windows.Forms.Label();
      this.db_chkTrustWorthy = new System.Windows.Forms.CheckBox();
      this.db_grpCert = new System.Windows.Forms.GroupBox();
      this.db_dgvCerts = new System.Windows.Forms.DataGridView();
      this.db_grpUsers = new System.Windows.Forms.GroupBox();
      this.db_lbUsers = new System.Windows.Forms.ListBox();
      this.label43 = new System.Windows.Forms.Label();
      this.db_chkMasterKey = new System.Windows.Forms.CheckBox();
      this.db_txtName = new System.Windows.Forms.TextBox();
      this.label41 = new System.Windows.Forms.Label();
      this.tabPage9 = new System.Windows.Forms.TabPage();
      this.srv_grpEp = new System.Windows.Forms.GroupBox();
      this.serv_dvgEp = new System.Windows.Forms.DataGridView();
      this.srv_grpLogins = new System.Windows.Forms.GroupBox();
      this.srv_lbLogins = new System.Windows.Forms.ListBox();
      this.srv_txtName = new System.Windows.Forms.TextBox();
      this.label46 = new System.Windows.Forms.Label();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.tabPage3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.q_dgvStatus)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.q_dvgMsg)).BeginInit();
      this.tabPage4.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dvgMsg)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dvgConv)).BeginInit();
      this.tabPage5.SuspendLayout();
      this.tabPage6.SuspendLayout();
      this.tabPage7.SuspendLayout();
      this.groupBox3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dvgCnvMsgs)).BeginInit();
      this.tabPage8.SuspendLayout();
      this.db_grpCert.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.db_dgvCerts)).BeginInit();
      this.db_grpUsers.SuspendLayout();
      this.tabPage9.SuspendLayout();
      this.srv_grpEp.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.serv_dvgEp)).BeginInit();
      this.srv_grpLogins.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 24);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.tv1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
      this.splitContainer1.Size = new System.Drawing.Size(932, 477);
      this.splitContainer1.SplitterDistance = 313;
      this.splitContainer1.SplitterWidth = 6;
      this.splitContainer1.TabIndex = 1;
      this.splitContainer1.Text = "splitContainer1";
      // 
      // tv1
      // 
      this.tv1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tv1.HideSelection = false;
      this.tv1.Location = new System.Drawing.Point(0, 0);
      this.tv1.Name = "tv1";
      this.tv1.Size = new System.Drawing.Size(309, 473);
      this.tv1.TabIndex = 3;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Controls.Add(this.tabPage4);
      this.tabControl1.Controls.Add(this.tabPage5);
      this.tabControl1.Controls.Add(this.tabPage6);
      this.tabControl1.Controls.Add(this.tabPage7);
      this.tabControl1.Controls.Add(this.tabPage8);
      this.tabControl1.Controls.Add(this.tabPage9);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(609, 473);
      this.tabControl1.TabIndex = 1;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.label17);
      this.tabPage1.Controls.Add(this.textBox12);
      this.tabPage1.Controls.Add(this.label3);
      this.tabPage1.Controls.Add(this.textBox3);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.textBox2);
      this.tabPage1.Controls.Add(this.textBox1);
      this.tabPage1.Controls.Add(this.label1);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(601, 447);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Message Types";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(52, 75);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(53, 13);
      this.label17.TabIndex = 6;
      this.label17.Text = "Validation";
      this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox12
      // 
      this.textBox12.Location = new System.Drawing.Point(115, 69);
      this.textBox12.Name = "textBox12";
      this.textBox12.Size = new System.Drawing.Size(129, 20);
      this.textBox12.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 101);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(95, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Validation Schema";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(115, 95);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(378, 20);
      this.textBox3.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(36, 49);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(68, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Authorization\r\n";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(115, 43);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(129, 20);
      this.textBox2.TabIndex = 2;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(115, 17);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(378, 20);
      this.textBox1.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(71, 23);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Name";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.label18);
      this.tabPage2.Controls.Add(this.textBox14);
      this.tabPage2.Controls.Add(this.groupBox1);
      this.tabPage2.Controls.Add(this.textBox4);
      this.tabPage2.Controls.Add(this.label4);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(601, 447);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Message Contracts";
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(36, 47);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(68, 13);
      this.label18.TabIndex = 6;
      this.label18.Text = "Authorization\r\n";
      this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox14
      // 
      this.textBox14.Location = new System.Drawing.Point(115, 43);
      this.textBox14.Name = "textBox14";
      this.textBox14.Size = new System.Drawing.Size(129, 20);
      this.textBox14.TabIndex = 2;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.dataGridView1);
      this.groupBox1.Location = new System.Drawing.Point(7, 83);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(566, 136);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Message Types";
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
      this.dataGridView1.Location = new System.Drawing.Point(6, 19);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.Size = new System.Drawing.Size(554, 104);
      this.dataGridView1.TabIndex = 3;
      // 
      // Column1
      // 
      this.Column1.HeaderText = "Name";
      this.Column1.Name = "Column1";
      // 
      // Column2
      // 
      this.Column2.HeaderText = "Sent By";
      this.Column2.Name = "Column2";
      // 
      // textBox4
      // 
      this.textBox4.Location = new System.Drawing.Point(115, 17);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new System.Drawing.Size(378, 20);
      this.textBox4.TabIndex = 1;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(71, 21);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(35, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Name";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.groupBox4);
      this.tabPage3.Controls.Add(this.textBox10);
      this.tabPage3.Controls.Add(this.label13);
      this.tabPage3.Controls.Add(this.textBox9);
      this.tabPage3.Controls.Add(this.label12);
      this.tabPage3.Controls.Add(this.textBox8);
      this.tabPage3.Controls.Add(this.label11);
      this.tabPage3.Controls.Add(this.textBox7);
      this.tabPage3.Controls.Add(this.label10);
      this.tabPage3.Controls.Add(this.textBox6);
      this.tabPage3.Controls.Add(this.label9);
      this.tabPage3.Controls.Add(this.checkBox3);
      this.tabPage3.Controls.Add(this.label8);
      this.tabPage3.Controls.Add(this.checkBox2);
      this.tabPage3.Controls.Add(this.label7);
      this.tabPage3.Controls.Add(this.checkBox1);
      this.tabPage3.Controls.Add(this.label6);
      this.tabPage3.Controls.Add(this.textBox5);
      this.tabPage3.Controls.Add(this.label5);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage3.Size = new System.Drawing.Size(601, 447);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Queues";
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.q_dgvStatus);
      this.groupBox4.Controls.Add(this.q_dvgMsg);
      this.groupBox4.Location = new System.Drawing.Point(14, 202);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(581, 224);
      this.groupBox4.TabIndex = 20;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Received Messages";
      // 
      // q_dgvStatus
      // 
      this.q_dgvStatus.AllowUserToAddRows = false;
      this.q_dgvStatus.AllowUserToDeleteRows = false;
      this.q_dgvStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.q_dgvStatus.Location = new System.Drawing.Point(11, 19);
      this.q_dgvStatus.Name = "q_dgvStatus";
      this.q_dgvStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.q_dgvStatus.Size = new System.Drawing.Size(564, 199);
      this.q_dgvStatus.TabIndex = 1;
      this.q_dgvStatus.Text = "dataGridView2";
      // 
      // q_dvgMsg
      // 
      this.q_dvgMsg.AllowUserToAddRows = false;
      this.q_dvgMsg.AllowUserToDeleteRows = false;
      this.q_dvgMsg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.q_dvgMsg.Location = new System.Drawing.Point(11, 19);
      this.q_dvgMsg.Name = "q_dvgMsg";
      this.q_dvgMsg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.q_dvgMsg.Size = new System.Drawing.Size(564, 199);
      this.q_dvgMsg.TabIndex = 0;
      this.q_dvgMsg.Text = "dataGridView2";
      // 
      // textBox10
      // 
      this.textBox10.Location = new System.Drawing.Point(120, 122);
      this.textBox10.Name = "textBox10";
      this.textBox10.Size = new System.Drawing.Size(378, 20);
      this.textBox10.TabIndex = 7;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(24, 126);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(87, 13);
      this.label13.TabIndex = 19;
      this.label13.Text = "Procedure Name";
      this.label13.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox9
      // 
      this.textBox9.Location = new System.Drawing.Point(120, 96);
      this.textBox9.Name = "textBox9";
      this.textBox9.Size = new System.Drawing.Size(378, 20);
      this.textBox9.TabIndex = 6;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(13, 101);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(98, 13);
      this.label12.TabIndex = 17;
      this.label12.Text = "Procedure Schema";
      this.label12.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox8
      // 
      this.textBox8.Location = new System.Drawing.Point(290, 150);
      this.textBox8.Name = "textBox8";
      this.textBox8.Size = new System.Drawing.Size(208, 20);
      this.textBox8.TabIndex = 9;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(223, 154);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(61, 13);
      this.label11.TabIndex = 15;
      this.label11.Text = "Execute As";
      this.label11.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox7
      // 
      this.textBox7.Location = new System.Drawing.Point(120, 150);
      this.textBox7.Name = "textBox7";
      this.textBox7.Size = new System.Drawing.Size(51, 20);
      this.textBox7.TabIndex = 8;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 154);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(105, 13);
      this.label10.TabIndex = 13;
      this.label10.Text = "Max Queue Readers";
      this.label10.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox6
      // 
      this.textBox6.Location = new System.Drawing.Point(120, 69);
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new System.Drawing.Size(378, 20);
      this.textBox6.TabIndex = 5;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(40, 73);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(73, 13);
      this.label9.TabIndex = 11;
      this.label9.Text = "Procedure Db";
      this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // checkBox3
      // 
      this.checkBox3.AutoSize = true;
      this.checkBox3.Location = new System.Drawing.Point(334, 49);
      this.checkBox3.Name = "checkBox3";
      this.checkBox3.Size = new System.Drawing.Size(15, 14);
      this.checkBox3.TabIndex = 4;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(248, 47);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(79, 13);
      this.label8.TabIndex = 9;
      this.label8.Text = "Activation - ON";
      this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // checkBox2
      // 
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new System.Drawing.Point(227, 49);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new System.Drawing.Size(15, 14);
      this.checkBox2.TabIndex = 3;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(141, 47);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(78, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Retention - ON";
      this.label7.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(120, 49);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(15, 14);
      this.checkBox1.TabIndex = 2;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(51, 49);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(62, 13);
      this.label6.TabIndex = 5;
      this.label6.Text = "Status - ON";
      this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox5
      // 
      this.textBox5.Location = new System.Drawing.Point(120, 17);
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new System.Drawing.Size(378, 20);
      this.textBox5.TabIndex = 1;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(80, 23);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(35, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Name";
      this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // tabPage4
      // 
      this.tabPage4.Controls.Add(this.groupBox2);
      this.tabPage4.Controls.Add(this.label19);
      this.tabPage4.Controls.Add(this.textBox15);
      this.tabPage4.Controls.Add(this.label15);
      this.tabPage4.Controls.Add(this.listBox1);
      this.tabPage4.Controls.Add(this.textBox13);
      this.tabPage4.Controls.Add(this.label16);
      this.tabPage4.Controls.Add(this.textBox11);
      this.tabPage4.Controls.Add(this.label14);
      this.tabPage4.Location = new System.Drawing.Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage4.Size = new System.Drawing.Size(601, 447);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Services";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label48);
      this.groupBox2.Controls.Add(this.label47);
      this.groupBox2.Controls.Add(this.srv_cboCnvState);
      this.groupBox2.Controls.Add(this.srv_cboSource);
      this.groupBox2.Controls.Add(this.label34);
      this.groupBox2.Controls.Add(this.label33);
      this.groupBox2.Controls.Add(this.dvgMsg);
      this.groupBox2.Controls.Add(this.dvgConv);
      this.groupBox2.Location = new System.Drawing.Point(30, 144);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(533, 329);
      this.groupBox2.TabIndex = 25;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Conversations and Received Messages";
      // 
      // label48
      // 
      this.label48.AutoSize = true;
      this.label48.Location = new System.Drawing.Point(290, 35);
      this.label48.Name = "label48";
      this.label48.Size = new System.Drawing.Size(32, 13);
      this.label48.TabIndex = 26;
      this.label48.Text = "State";
      this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label47
      // 
      this.label47.AutoSize = true;
      this.label47.Location = new System.Drawing.Point(51, 35);
      this.label47.Name = "label47";
      this.label47.Size = new System.Drawing.Size(41, 13);
      this.label47.TabIndex = 25;
      this.label47.Text = "Source";
      this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // srv_cboCnvState
      // 
      this.srv_cboCnvState.FormattingEnabled = true;
      this.srv_cboCnvState.Location = new System.Drawing.Point(324, 32);
      this.srv_cboCnvState.Name = "srv_cboCnvState";
      this.srv_cboCnvState.Size = new System.Drawing.Size(199, 21);
      this.srv_cboCnvState.TabIndex = 5;
      // 
      // srv_cboSource
      // 
      this.srv_cboSource.FormattingEnabled = true;
      this.srv_cboSource.Location = new System.Drawing.Point(94, 32);
      this.srv_cboSource.Name = "srv_cboSource";
      this.srv_cboSource.Size = new System.Drawing.Size(121, 21);
      this.srv_cboSource.TabIndex = 4;
      // 
      // label34
      // 
      this.label34.AutoSize = true;
      this.label34.Location = new System.Drawing.Point(6, 172);
      this.label34.Name = "label34";
      this.label34.Size = new System.Drawing.Size(55, 13);
      this.label34.TabIndex = 3;
      this.label34.Text = "Messages";
      // 
      // label33
      // 
      this.label33.AutoSize = true;
      this.label33.Location = new System.Drawing.Point(5, 16);
      this.label33.Name = "label33";
      this.label33.Size = new System.Drawing.Size(74, 13);
      this.label33.TabIndex = 2;
      this.label33.Text = "Conversations";
      // 
      // dvgMsg
      // 
      this.dvgMsg.AllowUserToAddRows = false;
      this.dvgMsg.AllowUserToDeleteRows = false;
      this.dvgMsg.AllowUserToOrderColumns = true;
      this.dvgMsg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.dvgMsg.Location = new System.Drawing.Point(6, 188);
      this.dvgMsg.MultiSelect = false;
      this.dvgMsg.Name = "dvgMsg";
      this.dvgMsg.ReadOnly = true;
      this.dvgMsg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dvgMsg.Size = new System.Drawing.Size(517, 111);
      this.dvgMsg.TabIndex = 1;
      // 
      // dvgConv
      // 
      this.dvgConv.AllowUserToAddRows = false;
      this.dvgConv.AllowUserToDeleteRows = false;
      this.dvgConv.AllowUserToOrderColumns = true;
      this.dvgConv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.dvgConv.CausesValidation = false;
      this.dvgConv.Location = new System.Drawing.Point(6, 59);
      this.dvgConv.MultiSelect = false;
      this.dvgConv.Name = "dvgConv";
      this.dvgConv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dvgConv.Size = new System.Drawing.Size(517, 110);
      this.dvgConv.TabIndex = 0;
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(48, 46);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(68, 13);
      this.label19.TabIndex = 24;
      this.label19.Text = "Authorization";
      this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox15
      // 
      this.textBox15.Location = new System.Drawing.Point(125, 42);
      this.textBox15.Name = "textBox15";
      this.textBox15.Size = new System.Drawing.Size(129, 20);
      this.textBox15.TabIndex = 2;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(16, 94);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(98, 13);
      this.label15.TabIndex = 22;
      this.label15.Text = "Message Contracts";
      this.label15.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // listBox1
      // 
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new System.Drawing.Point(124, 94);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(378, 43);
      this.listBox1.TabIndex = 4;
      // 
      // textBox13
      // 
      this.textBox13.Location = new System.Drawing.Point(125, 68);
      this.textBox13.Name = "textBox13";
      this.textBox13.Size = new System.Drawing.Size(378, 20);
      this.textBox13.TabIndex = 3;
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(47, 72);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(70, 13);
      this.label16.TabIndex = 20;
      this.label16.Text = "Queue Name";
      this.label16.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox11
      // 
      this.textBox11.Location = new System.Drawing.Point(125, 16);
      this.textBox11.Name = "textBox11";
      this.textBox11.Size = new System.Drawing.Size(378, 20);
      this.textBox11.TabIndex = 1;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(84, 20);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(35, 13);
      this.label14.TabIndex = 6;
      this.label14.Text = "Name";
      this.label14.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // tabPage5
      // 
      this.tabPage5.Controls.Add(this.label25);
      this.tabPage5.Controls.Add(this.label24);
      this.tabPage5.Controls.Add(this.textBox22);
      this.tabPage5.Controls.Add(this.textBox21);
      this.tabPage5.Controls.Add(this.textBox18);
      this.tabPage5.Controls.Add(this.label22);
      this.tabPage5.Controls.Add(this.textBox19);
      this.tabPage5.Controls.Add(this.label23);
      this.tabPage5.Controls.Add(this.textBox20);
      this.tabPage5.Controls.Add(this.label26);
      this.tabPage5.Controls.Add(this.label20);
      this.tabPage5.Controls.Add(this.textBox16);
      this.tabPage5.Controls.Add(this.textBox17);
      this.tabPage5.Controls.Add(this.label21);
      this.tabPage5.Location = new System.Drawing.Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage5.Size = new System.Drawing.Size(601, 447);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "Routes";
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(38, 172);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(74, 13);
      this.label25.TabIndex = 43;
      this.label25.Text = "Mirror Address";
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(67, 146);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(45, 13);
      this.label24.TabIndex = 42;
      this.label24.Text = "Address";
      // 
      // textBox22
      // 
      this.textBox22.Location = new System.Drawing.Point(126, 169);
      this.textBox22.Name = "textBox22";
      this.textBox22.Size = new System.Drawing.Size(378, 20);
      this.textBox22.TabIndex = 41;
      // 
      // textBox21
      // 
      this.textBox21.Location = new System.Drawing.Point(126, 143);
      this.textBox21.Name = "textBox21";
      this.textBox21.Size = new System.Drawing.Size(378, 20);
      this.textBox21.TabIndex = 40;
      // 
      // textBox18
      // 
      this.textBox18.Location = new System.Drawing.Point(126, 63);
      this.textBox18.Name = "textBox18";
      this.textBox18.Size = new System.Drawing.Size(378, 20);
      this.textBox18.TabIndex = 3;
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(69, 124);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(43, 13);
      this.label22.TabIndex = 38;
      this.label22.Text = "Lifetime";
      this.label22.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox19
      // 
      this.textBox19.Location = new System.Drawing.Point(126, 91);
      this.textBox19.Name = "textBox19";
      this.textBox19.Size = new System.Drawing.Size(378, 20);
      this.textBox19.TabIndex = 4;
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(30, 98);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(82, 13);
      this.label23.TabIndex = 37;
      this.label23.Text = "Broker Instance";
      this.label23.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox20
      // 
      this.textBox20.Location = new System.Drawing.Point(126, 117);
      this.textBox20.Name = "textBox20";
      this.textBox20.Size = new System.Drawing.Size(147, 20);
      this.textBox20.TabIndex = 5;
      // 
      // label26
      // 
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(29, 70);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(83, 13);
      this.label26.TabIndex = 34;
      this.label26.Text = "Remote Service";
      this.label26.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(44, 40);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(68, 13);
      this.label20.TabIndex = 28;
      this.label20.Text = "Authorization\r\n";
      this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox16
      // 
      this.textBox16.Location = new System.Drawing.Point(126, 11);
      this.textBox16.Multiline = true;
      this.textBox16.Name = "textBox16";
      this.textBox16.Size = new System.Drawing.Size(378, 20);
      this.textBox16.TabIndex = 1;
      // 
      // textBox17
      // 
      this.textBox17.Location = new System.Drawing.Point(126, 37);
      this.textBox17.Name = "textBox17";
      this.textBox17.Size = new System.Drawing.Size(127, 20);
      this.textBox17.TabIndex = 2;
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(77, 18);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(35, 13);
      this.label21.TabIndex = 27;
      this.label21.Text = "Name";
      this.label21.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // tabPage6
      // 
      this.tabPage6.Controls.Add(this.checkBox4);
      this.tabPage6.Controls.Add(this.label32);
      this.tabPage6.Controls.Add(this.label31);
      this.tabPage6.Controls.Add(this.textBox27);
      this.tabPage6.Controls.Add(this.textBox26);
      this.tabPage6.Controls.Add(this.label30);
      this.tabPage6.Controls.Add(this.textBox25);
      this.tabPage6.Controls.Add(this.label29);
      this.tabPage6.Controls.Add(this.label28);
      this.tabPage6.Controls.Add(this.textBox24);
      this.tabPage6.Controls.Add(this.textBox23);
      this.tabPage6.Controls.Add(this.label27);
      this.tabPage6.Location = new System.Drawing.Point(4, 22);
      this.tabPage6.Name = "tabPage6";
      this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage6.Size = new System.Drawing.Size(601, 447);
      this.tabPage6.TabIndex = 5;
      this.tabPage6.Text = "Remote Service Bindings";
      // 
      // checkBox4
      // 
      this.checkBox4.AutoSize = true;
      this.checkBox4.Location = new System.Drawing.Point(111, 145);
      this.checkBox4.Name = "checkBox4";
      this.checkBox4.Size = new System.Drawing.Size(15, 14);
      this.checkBox4.TabIndex = 6;
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Location = new System.Drawing.Point(14, 145);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(87, 13);
      this.label32.TabIndex = 43;
      this.label32.Text = "Anonymous - ON";
      this.label32.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(77, 125);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(29, 13);
      this.label31.TabIndex = 41;
      this.label31.Text = "User";
      this.label31.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox27
      // 
      this.textBox27.Location = new System.Drawing.Point(111, 119);
      this.textBox27.Name = "textBox27";
      this.textBox27.Size = new System.Drawing.Size(127, 20);
      this.textBox27.TabIndex = 5;
      // 
      // textBox26
      // 
      this.textBox26.Location = new System.Drawing.Point(111, 93);
      this.textBox26.Name = "textBox26";
      this.textBox26.Size = new System.Drawing.Size(378, 20);
      this.textBox26.TabIndex = 4;
      // 
      // label30
      // 
      this.label30.AutoSize = true;
      this.label30.Location = new System.Drawing.Point(58, 99);
      this.label30.Name = "label30";
      this.label30.Size = new System.Drawing.Size(47, 13);
      this.label30.TabIndex = 39;
      this.label30.Text = "Contract";
      this.label30.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textBox25
      // 
      this.textBox25.Location = new System.Drawing.Point(111, 67);
      this.textBox25.Name = "textBox25";
      this.textBox25.Size = new System.Drawing.Size(378, 20);
      this.textBox25.TabIndex = 3;
      // 
      // label29
      // 
      this.label29.AutoSize = true;
      this.label29.Location = new System.Drawing.Point(20, 70);
      this.label29.Name = "label29";
      this.label29.Size = new System.Drawing.Size(83, 13);
      this.label29.TabIndex = 36;
      this.label29.Text = "Remote Service";
      this.label29.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // label28
      // 
      this.label28.AutoSize = true;
      this.label28.Location = new System.Drawing.Point(35, 47);
      this.label28.Name = "label28";
      this.label28.Size = new System.Drawing.Size(68, 13);
      this.label28.TabIndex = 31;
      this.label28.Text = "Authorization\r\n";
      this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // textBox24
      // 
      this.textBox24.Location = new System.Drawing.Point(111, 41);
      this.textBox24.Name = "textBox24";
      this.textBox24.Size = new System.Drawing.Size(127, 20);
      this.textBox24.TabIndex = 2;
      // 
      // textBox23
      // 
      this.textBox23.Location = new System.Drawing.Point(111, 15);
      this.textBox23.Multiline = true;
      this.textBox23.Name = "textBox23";
      this.textBox23.Size = new System.Drawing.Size(378, 20);
      this.textBox23.TabIndex = 1;
      // 
      // label27
      // 
      this.label27.AutoSize = true;
      this.label27.Location = new System.Drawing.Point(71, 21);
      this.label27.Name = "label27";
      this.label27.Size = new System.Drawing.Size(35, 13);
      this.label27.TabIndex = 29;
      this.label27.Text = "Name";
      this.label27.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // tabPage7
      // 
      this.tabPage7.Controls.Add(this.label38);
      this.tabPage7.Controls.Add(this.cnv_chkInitiator);
      this.tabPage7.Controls.Add(this.label37);
      this.tabPage7.Controls.Add(this.cnv_txtLifeTime);
      this.tabPage7.Controls.Add(this.cnv_txtFarBroker);
      this.tabPage7.Controls.Add(this.label36);
      this.tabPage7.Controls.Add(this.groupBox3);
      this.tabPage7.Controls.Add(this.cnv_txtContract);
      this.tabPage7.Controls.Add(this.cnv_txtFrmServ);
      this.tabPage7.Controls.Add(this.label45);
      this.tabPage7.Controls.Add(this.cnv_txtState);
      this.tabPage7.Controls.Add(this.label44);
      this.tabPage7.Controls.Add(this.cnv_txtRelGrpHndl);
      this.tabPage7.Controls.Add(this.label42);
      this.tabPage7.Controls.Add(this.label40);
      this.tabPage7.Controls.Add(this.label39);
      this.tabPage7.Controls.Add(this.cnv_txtToSrv);
      this.tabPage7.Controls.Add(this.cnv_txtDlgGuid);
      this.tabPage7.Controls.Add(this.label35);
      this.tabPage7.Location = new System.Drawing.Point(4, 22);
      this.tabPage7.Name = "tabPage7";
      this.tabPage7.Size = new System.Drawing.Size(601, 447);
      this.tabPage7.TabIndex = 6;
      this.tabPage7.Text = "Conversations";
      // 
      // label38
      // 
      this.label38.AutoSize = true;
      this.label38.Location = new System.Drawing.Point(274, 200);
      this.label38.Name = "label38";
      this.label38.Size = new System.Drawing.Size(41, 13);
      this.label38.TabIndex = 47;
      this.label38.Text = "Initiator";
      this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cnv_chkInitiator
      // 
      this.cnv_chkInitiator.AutoSize = true;
      this.cnv_chkInitiator.Location = new System.Drawing.Point(321, 198);
      this.cnv_chkInitiator.Name = "cnv_chkInitiator";
      this.cnv_chkInitiator.Size = new System.Drawing.Size(15, 14);
      this.cnv_chkInitiator.TabIndex = 46;
      // 
      // label37
      // 
      this.label37.AutoSize = true;
      this.label37.Location = new System.Drawing.Point(60, 201);
      this.label37.Name = "label37";
      this.label37.Size = new System.Drawing.Size(50, 13);
      this.label37.TabIndex = 45;
      this.label37.Text = "Life Time";
      this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cnv_txtLifeTime
      // 
      this.cnv_txtLifeTime.Location = new System.Drawing.Point(117, 195);
      this.cnv_txtLifeTime.Name = "cnv_txtLifeTime";
      this.cnv_txtLifeTime.Size = new System.Drawing.Size(140, 20);
      this.cnv_txtLifeTime.TabIndex = 44;
      // 
      // cnv_txtFarBroker
      // 
      this.cnv_txtFarBroker.Location = new System.Drawing.Point(117, 117);
      this.cnv_txtFarBroker.Name = "cnv_txtFarBroker";
      this.cnv_txtFarBroker.Size = new System.Drawing.Size(358, 20);
      this.cnv_txtFarBroker.TabIndex = 43;
      // 
      // label36
      // 
      this.label36.AutoSize = true;
      this.label36.Location = new System.Drawing.Point(6, 123);
      this.label36.Name = "label36";
      this.label36.Size = new System.Drawing.Size(103, 13);
      this.label36.TabIndex = 42;
      this.label36.Text = "Remote Broker Guid";
      this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.dvgCnvMsgs);
      this.groupBox3.Location = new System.Drawing.Point(36, 266);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(528, 194);
      this.groupBox3.TabIndex = 41;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Received Messages";
      // 
      // dvgCnvMsgs
      // 
      this.dvgCnvMsgs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.dvgCnvMsgs.Location = new System.Drawing.Point(13, 28);
      this.dvgCnvMsgs.MultiSelect = false;
      this.dvgCnvMsgs.Name = "dvgCnvMsgs";
      this.dvgCnvMsgs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dvgCnvMsgs.Size = new System.Drawing.Size(509, 150);
      this.dvgCnvMsgs.TabIndex = 0;
      // 
      // cnv_txtContract
      // 
      this.cnv_txtContract.Location = new System.Drawing.Point(117, 143);
      this.cnv_txtContract.Name = "cnv_txtContract";
      this.cnv_txtContract.Size = new System.Drawing.Size(358, 20);
      this.cnv_txtContract.TabIndex = 40;
      // 
      // cnv_txtFrmServ
      // 
      this.cnv_txtFrmServ.Location = new System.Drawing.Point(117, 66);
      this.cnv_txtFrmServ.Name = "cnv_txtFrmServ";
      this.cnv_txtFrmServ.Size = new System.Drawing.Size(358, 20);
      this.cnv_txtFrmServ.TabIndex = 39;
      // 
      // label45
      // 
      this.label45.AutoSize = true;
      this.label45.Location = new System.Drawing.Point(80, 171);
      this.label45.Name = "label45";
      this.label45.Size = new System.Drawing.Size(32, 13);
      this.label45.TabIndex = 36;
      this.label45.Text = "State";
      this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cnv_txtState
      // 
      this.cnv_txtState.Location = new System.Drawing.Point(117, 169);
      this.cnv_txtState.Name = "cnv_txtState";
      this.cnv_txtState.Size = new System.Drawing.Size(140, 20);
      this.cnv_txtState.TabIndex = 35;
      // 
      // label44
      // 
      this.label44.AutoSize = true;
      this.label44.Location = new System.Drawing.Point(36, 46);
      this.label44.Name = "label44";
      this.label44.Size = new System.Drawing.Size(73, 13);
      this.label44.TabIndex = 34;
      this.label44.Text = "Group Handle";
      this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cnv_txtRelGrpHndl
      // 
      this.cnv_txtRelGrpHndl.Location = new System.Drawing.Point(117, 40);
      this.cnv_txtRelGrpHndl.Name = "cnv_txtRelGrpHndl";
      this.cnv_txtRelGrpHndl.Size = new System.Drawing.Size(358, 20);
      this.cnv_txtRelGrpHndl.TabIndex = 32;
      // 
      // label42
      // 
      this.label42.AutoSize = true;
      this.label42.Location = new System.Drawing.Point(66, 150);
      this.label42.Name = "label42";
      this.label42.Size = new System.Drawing.Size(47, 13);
      this.label42.TabIndex = 30;
      this.label42.Text = "Contract";
      this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label40
      // 
      this.label40.AutoSize = true;
      this.label40.Location = new System.Drawing.Point(25, 98);
      this.label40.Name = "label40";
      this.label40.Size = new System.Drawing.Size(83, 13);
      this.label40.TabIndex = 28;
      this.label40.Text = "Remote Service";
      this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label39
      // 
      this.label39.AutoSize = true;
      this.label39.Location = new System.Drawing.Point(36, 69);
      this.label39.Name = "label39";
      this.label39.Size = new System.Drawing.Size(72, 13);
      this.label39.TabIndex = 27;
      this.label39.Text = "Local Service";
      this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cnv_txtToSrv
      // 
      this.cnv_txtToSrv.Location = new System.Drawing.Point(117, 91);
      this.cnv_txtToSrv.Name = "cnv_txtToSrv";
      this.cnv_txtToSrv.Size = new System.Drawing.Size(359, 20);
      this.cnv_txtToSrv.TabIndex = 24;
      // 
      // cnv_txtDlgGuid
      // 
      this.cnv_txtDlgGuid.Location = new System.Drawing.Point(117, 14);
      this.cnv_txtDlgGuid.Name = "cnv_txtDlgGuid";
      this.cnv_txtDlgGuid.Size = new System.Drawing.Size(358, 20);
      this.cnv_txtDlgGuid.TabIndex = 22;
      // 
      // label35
      // 
      this.label35.AutoSize = true;
      this.label35.Location = new System.Drawing.Point(36, 20);
      this.label35.Name = "label35";
      this.label35.Size = new System.Drawing.Size(74, 13);
      this.label35.TabIndex = 21;
      this.label35.Text = "Dialog Handle";
      this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tabPage8
      // 
      this.tabPage8.Controls.Add(this.label77);
      this.tabPage8.Controls.Add(this.db_chkTrustWorthy);
      this.tabPage8.Controls.Add(this.db_grpCert);
      this.tabPage8.Controls.Add(this.db_grpUsers);
      this.tabPage8.Controls.Add(this.label43);
      this.tabPage8.Controls.Add(this.db_chkMasterKey);
      this.tabPage8.Controls.Add(this.db_txtName);
      this.tabPage8.Controls.Add(this.label41);
      this.tabPage8.Location = new System.Drawing.Point(4, 22);
      this.tabPage8.Name = "tabPage8";
      this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage8.Size = new System.Drawing.Size(601, 447);
      this.tabPage8.TabIndex = 7;
      this.tabPage8.Text = "Database";
      // 
      // label77
      // 
      this.label77.AutoSize = true;
      this.label77.Location = new System.Drawing.Point(140, 43);
      this.label77.Name = "label77";
      this.label77.Size = new System.Drawing.Size(62, 13);
      this.label77.TabIndex = 110;
      this.label77.Text = "Trustworthy";
      this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // db_chkTrustWorthy
      // 
      this.db_chkTrustWorthy.AutoSize = true;
      this.db_chkTrustWorthy.Location = new System.Drawing.Point(204, 43);
      this.db_chkTrustWorthy.Name = "db_chkTrustWorthy";
      this.db_chkTrustWorthy.Size = new System.Drawing.Size(15, 14);
      this.db_chkTrustWorthy.TabIndex = 109;
      // 
      // db_grpCert
      // 
      this.db_grpCert.Controls.Add(this.db_dgvCerts);
      this.db_grpCert.Location = new System.Drawing.Point(7, 271);
      this.db_grpCert.Name = "db_grpCert";
      this.db_grpCert.Size = new System.Drawing.Size(588, 149);
      this.db_grpCert.TabIndex = 7;
      this.db_grpCert.TabStop = false;
      this.db_grpCert.Text = "Certificates";
      // 
      // db_dgvCerts
      // 
      this.db_dgvCerts.AllowUserToAddRows = false;
      this.db_dgvCerts.AllowUserToDeleteRows = false;
      this.db_dgvCerts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.db_dgvCerts.Location = new System.Drawing.Point(6, 19);
      this.db_dgvCerts.MultiSelect = false;
      this.db_dgvCerts.Name = "db_dgvCerts";
      this.db_dgvCerts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.db_dgvCerts.Size = new System.Drawing.Size(576, 124);
      this.db_dgvCerts.TabIndex = 0;
      this.db_dgvCerts.Text = "dataGridView2";
      // 
      // db_grpUsers
      // 
      this.db_grpUsers.Controls.Add(this.db_lbUsers);
      this.db_grpUsers.Location = new System.Drawing.Point(13, 82);
      this.db_grpUsers.Name = "db_grpUsers";
      this.db_grpUsers.Size = new System.Drawing.Size(185, 173);
      this.db_grpUsers.TabIndex = 6;
      this.db_grpUsers.TabStop = false;
      this.db_grpUsers.Text = "Users";
      // 
      // db_lbUsers
      // 
      this.db_lbUsers.FormattingEnabled = true;
      this.db_lbUsers.Location = new System.Drawing.Point(6, 19);
      this.db_lbUsers.Name = "db_lbUsers";
      this.db_lbUsers.Size = new System.Drawing.Size(173, 147);
      this.db_lbUsers.TabIndex = 0;
      // 
      // label43
      // 
      this.label43.AutoSize = true;
      this.label43.Location = new System.Drawing.Point(45, 42);
      this.label43.Name = "label43";
      this.label43.Size = new System.Drawing.Size(60, 13);
      this.label43.TabIndex = 5;
      this.label43.Text = "Master Key";
      this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // db_chkMasterKey
      // 
      this.db_chkMasterKey.AutoSize = true;
      this.db_chkMasterKey.Location = new System.Drawing.Point(112, 43);
      this.db_chkMasterKey.Name = "db_chkMasterKey";
      this.db_chkMasterKey.Size = new System.Drawing.Size(15, 14);
      this.db_chkMasterKey.TabIndex = 4;
      // 
      // db_txtName
      // 
      this.db_txtName.Location = new System.Drawing.Point(112, 17);
      this.db_txtName.Name = "db_txtName";
      this.db_txtName.Size = new System.Drawing.Size(378, 20);
      this.db_txtName.TabIndex = 3;
      // 
      // label41
      // 
      this.label41.AutoSize = true;
      this.label41.Location = new System.Drawing.Point(71, 23);
      this.label41.Name = "label41";
      this.label41.Size = new System.Drawing.Size(35, 13);
      this.label41.TabIndex = 2;
      this.label41.Text = "Name";
      this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tabPage9
      // 
      this.tabPage9.Controls.Add(this.srv_grpEp);
      this.tabPage9.Controls.Add(this.srv_grpLogins);
      this.tabPage9.Controls.Add(this.srv_txtName);
      this.tabPage9.Controls.Add(this.label46);
      this.tabPage9.Location = new System.Drawing.Point(4, 22);
      this.tabPage9.Name = "tabPage9";
      this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage9.Size = new System.Drawing.Size(601, 447);
      this.tabPage9.TabIndex = 8;
      this.tabPage9.Text = "Server";
      // 
      // srv_grpEp
      // 
      this.srv_grpEp.Controls.Add(this.serv_dvgEp);
      this.srv_grpEp.Location = new System.Drawing.Point(17, 234);
      this.srv_grpEp.Name = "srv_grpEp";
      this.srv_grpEp.Size = new System.Drawing.Size(546, 144);
      this.srv_grpEp.TabIndex = 10;
      this.srv_grpEp.TabStop = false;
      this.srv_grpEp.Text = "Service Broker Endpoints";
      // 
      // serv_dvgEp
      // 
      this.serv_dvgEp.AllowUserToAddRows = false;
      this.serv_dvgEp.AllowUserToDeleteRows = false;
      this.serv_dvgEp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.serv_dvgEp.Location = new System.Drawing.Point(6, 28);
      this.serv_dvgEp.MultiSelect = false;
      this.serv_dvgEp.Name = "serv_dvgEp";
      this.serv_dvgEp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.serv_dvgEp.Size = new System.Drawing.Size(534, 91);
      this.serv_dvgEp.TabIndex = 0;
      this.serv_dvgEp.Text = "dataGridView2";
      // 
      // srv_grpLogins
      // 
      this.srv_grpLogins.Controls.Add(this.srv_lbLogins);
      this.srv_grpLogins.Location = new System.Drawing.Point(17, 67);
      this.srv_grpLogins.Name = "srv_grpLogins";
      this.srv_grpLogins.Size = new System.Drawing.Size(243, 122);
      this.srv_grpLogins.TabIndex = 9;
      this.srv_grpLogins.TabStop = false;
      this.srv_grpLogins.Text = "Logins";
      // 
      // srv_lbLogins
      // 
      this.srv_lbLogins.FormattingEnabled = true;
      this.srv_lbLogins.Location = new System.Drawing.Point(6, 19);
      this.srv_lbLogins.Name = "srv_lbLogins";
      this.srv_lbLogins.Size = new System.Drawing.Size(227, 95);
      this.srv_lbLogins.TabIndex = 0;
      // 
      // srv_txtName
      // 
      this.srv_txtName.Location = new System.Drawing.Point(115, 21);
      this.srv_txtName.Name = "srv_txtName";
      this.srv_txtName.Size = new System.Drawing.Size(234, 20);
      this.srv_txtName.TabIndex = 5;
      // 
      // label46
      // 
      this.label46.AutoSize = true;
      this.label46.Location = new System.Drawing.Point(31, 24);
      this.label46.Name = "label46";
      this.label46.Size = new System.Drawing.Size(69, 13);
      this.label46.TabIndex = 6;
      this.label46.Text = "Server Name";
      this.label46.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(932, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 501);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(932, 22);
      this.statusStrip1.TabIndex = 3;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
      this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
      // 
      // Form1
      // 
      this.ClientSize = new System.Drawing.Size(932, 523);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "Form1";
      this.Text = "SSB Admin";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.q_dgvStatus)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.q_dvgMsg)).EndInit();
      this.tabPage4.ResumeLayout(false);
      this.tabPage4.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dvgMsg)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dvgConv)).EndInit();
      this.tabPage5.ResumeLayout(false);
      this.tabPage5.PerformLayout();
      this.tabPage6.ResumeLayout(false);
      this.tabPage6.PerformLayout();
      this.tabPage7.ResumeLayout(false);
      this.tabPage7.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dvgCnvMsgs)).EndInit();
      this.tabPage8.ResumeLayout(false);
      this.tabPage8.PerformLayout();
      this.db_grpCert.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.db_dgvCerts)).EndInit();
      this.db_grpUsers.ResumeLayout(false);
      this.tabPage9.ResumeLayout(false);
      this.tabPage9.PerformLayout();
      this.srv_grpEp.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.serv_dvgEp)).EndInit();
      this.srv_grpLogins.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion




   
    //}
    private SplitContainer splitContainer1;
    private TreeView tv1;
    private TabControl tabControl1;
    private TabPage tabPage2;
    private Label label18;
    private TextBox textBox14;
    private GroupBox groupBox1;
    private DataGridView dataGridView1;
    private TextBox textBox4;
    private Label label4;
    private TabPage tabPage3;
    private TextBox textBox10;
    private Label label13;
    private TextBox textBox9;
    private Label label12;
    private TextBox textBox8;
    private Label label11;
    private TextBox textBox7;
    private Label label10;
    private TextBox textBox6;
    private Label label9;
    private CheckBox checkBox3;
    private Label label8;
    private CheckBox checkBox2;
    private Label label7;
    private CheckBox checkBox1;
    private Label label6;
    private TextBox textBox5;
    private Label label5;
    private TabPage tabPage4;
    private Label label19;
    private TextBox textBox15;
    private Label label15;
    private ListBox listBox1;
    private TextBox textBox13;
    private Label label16;
    private TextBox textBox11;
    private Label label14;
    private TabPage tabPage5;
    private TextBox textBox18;
    private Label label22;
    private TextBox textBox19;
    private Label label23;
    private TextBox textBox20;
    private Label label26;
    private Label label20;
    private TextBox textBox16;
    private TextBox textBox17;
    private Label label21;
    private TabPage tabPage6;
    private CheckBox checkBox4;
    private Label label32;
    private Label label31;
    private TextBox textBox27;
    private TextBox textBox26;
    private Label label30;
    private TextBox textBox25;
    private Label label29;
    private Label label28;
    private TextBox textBox24;
    private TextBox textBox23;
    private Label label27;
    private TabPage tabPage1;
    private Label label17;
    private TextBox textBox12;
    private Label label3;
    private TextBox textBox3;
    private Label label2;
    private TextBox textBox2;
    private TextBox textBox1;
    private Label label1;
    private TextBox textBox21;
    private TextBox textBox22;
    private Label label24;
    private Label label25;
    private GroupBox groupBox2;
    private DataGridView dvgConv;
    private DataGridView dvgMsg;
    private TabPage tabPage7;
    private Label label34;
    private Label label33;
    private Label label44;
    private TextBox cnv_txtRelGrpHndl;
    private Label label42;
    private Label label40;
    private Label label39;
    private TextBox cnv_txtToSrv;
    private TextBox cnv_txtDlgGuid;
    private Label label35;
    private TextBox cnv_txtContract;
    private TextBox cnv_txtFrmServ;
    private Label label45;
    private TextBox cnv_txtState;
    private GroupBox groupBox3;
    private DataGridView dvgCnvMsgs;
    private Label label38;
    private CheckBox cnv_chkInitiator;
    private Label label37;
    private TextBox cnv_txtLifeTime;
    private TextBox cnv_txtFarBroker;
    private Label label36;
    private TabPage tabPage8;
    private Label label43;
    private CheckBox db_chkMasterKey;
    private TextBox db_txtName;
    private Label label41;
    private MenuStrip menuStrip1;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel toolStripStatusLabel1;
    private GroupBox groupBox4;
    private DataGridView q_dvgMsg;
    private TabPage tabPage9;
    private TextBox srv_txtName;
    private Label label46;
    private GroupBox srv_grpLogins;
    private ComboBox srv_cboSource;
    private ComboBox srv_cboCnvState;
    private GroupBox srv_grpEp;
    private ListBox srv_lbLogins;
    private GroupBox db_grpUsers;
    private ListBox db_lbUsers;
    private GroupBox db_grpCert;
    private DataGridView db_dgvCerts;
    private DataGridView serv_dvgEp;
    private Label label77;
    private CheckBox db_chkTrustWorthy;
    private Label label48;
    private Label label47;
    private DataGridView q_dgvStatus;
    private DataGridViewTextBoxColumn Column1;
    private DataGridViewTextBoxColumn Column2;


  }
}

