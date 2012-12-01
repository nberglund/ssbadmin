#region Using directives

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Broker;



#endregion

namespace SsbAdmin {
  partial class Form3 : Form {

    SsbEnum ssbTypeF3 = SsbEnum.None;
    SSBIServer dbServ;
    Database dBase;
    int level;
    bool isDirty;
    SsbState _state;
    bool browse;
    Control ctrl;
    string objName;
    

    ToolStripMenuItem mnuTopFile;
    ToolStripMenuItem mnuTopEdit;
    ToolStripMenuItem mnuTopView;
    ToolStripMenuItem mnuSubExit;
    ToolStripMenuItem mnuSubRefresh;
    ToolStripSeparator mnuSubSep;
    ToolStripSeparator mnuSubSep2;
    ToolStripMenuItem mnuSubPaste;
    ToolStripMenuItem mnuSubOpenConn;
    ContextMenuStrip mnuCtx;
    object ssbObj = null;


    public Form3(SsbEnum sType, bool _browse, Control ctrlToUpdate) {
      objName = "";
      ctrl = ctrlToUpdate;
      ssbTypeF3 = sType;
      InitializeComponent();
      SetupMenus();
      SetUpEventHooks();
      TVSetUp.SetUpTreeView(tv1);
      this.Text = string.Format("Find {0}", ssbTypeF3.ToString());
      ssbObj = null;
      isDirty = true;
      btnPaste.Enabled = false;
      browse = _browse;
      btnPaste.Text = "OK";

    }

    public Form3(SsbEnum sType, object _ssbObj) {
      ssbTypeF3 = sType;
      InitializeComponent();
      SetupMenus();
      SetUpEventHooks();
      TVSetUp.SetUpTreeView(tv1);
      this.Text = string.Format("Deploy {0}", ssbTypeF3.ToString());
      ssbObj = _ssbObj;
      isDirty = true;
      btnPaste.Enabled = false;
      browse = false;
      btnPaste.Text = "Paste/Deploy";

    }

    #region event-hooks

    void SetUpEventHooks() {

      #region TreeView tv1

      this.tv1.BeforeExpand += new TreeViewCancelEventHandler(this.tv1_BeforeExpand);
      this.tv1.AfterSelect += new TreeViewEventHandler(this.tv1_AfterSelect);
      this.tv1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tv1_NodeMouseClick);
      #endregion

      #region menus
      mnuTopFile.DropDownOpening += new EventHandler(mnuTopFile_DropDownOpening);
      mnuTopEdit.DropDownOpening += new EventHandler(mnuTopEdit_DropDownOpening);
      mnuTopView.DropDownOpening += new EventHandler(mnuTopView_DropDownOpening);

      mnuSubPaste.Click += new EventHandler(mnuSubPaste_Click);

      mnuSubExit.Click += new EventHandler(mnuSubExit_Click);

      mnuSubOpenConn.Click += new EventHandler(mnuSubOpenConn_Click);
      
      #endregion

      btnPaste.Click += new EventHandler(btnPaste_Click);
      btnCancel.Click += new EventHandler(btnCancel_Click);

    }

    
    #endregion

    void SetupMenus() {
      mnuTopFile = new ToolStripMenuItem();
      mnuTopFile.Name = "mnuTopFile";
      mnuTopFile.Text = "&File";

      mnuTopEdit = new ToolStripMenuItem();
      mnuTopEdit.Name = "mnuTopEdit";
      mnuTopEdit.Text = "&Edit";
      
      
      mnuSubExit = new ToolStripMenuItem();
      mnuSubExit.Name = "mnuSubExit";
      mnuSubExit.Text = "E&xit";

      mnuTopView = new ToolStripMenuItem();
      mnuTopView.Name = "mnuTopView";
      mnuTopView.Text = "&View";
      
      
      mnuSubPaste = new ToolStripMenuItem();
      mnuSubPaste.Name = "mnuSubPaste";
      mnuSubPaste.Text = "Paste/Deploy";
      mnuSubPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
      mnuSubPaste.Enabled = true;


      mnuSubRefresh = new ToolStripMenuItem();
      mnuSubRefresh.Name = "mnuSubRefresh";
      mnuSubRefresh.Text = "Add/Browse for Server(s)";
      mnuSubRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.F5)));
      mnuSubRefresh.Enabled = true;

      mnuSubOpenConn = new ToolStripMenuItem();
      mnuSubOpenConn.Name = "mnuSubOpenConn";
      mnuSubOpenConn.Tag = 40;
      mnuSubOpenConn.Text = "Connect";
      
      
      mnuSubSep = new ToolStripSeparator();
      
      mnuSubSep2 = new ToolStripSeparator();

      mnuTopEdit.DropDownItems.AddRange(new ToolStripItem[] { mnuSubPaste });
      mnuTopFile.DropDownItems.AddRange(new ToolStripItem[] { mnuSubOpenConn, mnuSubSep, mnuSubExit });
      mnuTopView.DropDownItems.AddRange(new ToolStripItem[] { mnuSubRefresh });

      menuStrip1.Items.AddRange(new ToolStripItem[] { mnuTopFile, mnuTopEdit, mnuTopView });

      mnuCtx = new ContextMenuStrip();

    }

    void SetupContextMenuAndShow(TreeNodeMouseClickEventArgs e) {
      mnuCtx.Items.Clear();
      int lvl = e.Node.Level;

      if(lvl == 0)
        mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubRefresh });

      else if (lvl == 1) {
          dbServ = (SSBIServer)e.Node.Tag;
          if (!dbServ.HasLoggedIn && !dbServ.IsTryingToConnect)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubOpenConn });
          else if (!dbServ.HasLoggedIn && dbServ.IsTryingToConnect) {
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubOpenConn });
            mnuSubOpenConn.Enabled = false;
          }
        }
      else if (lvl > 1)
        mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubPaste });


      //foreach (ToolStripItem ti in mnuCtx.Items)
      //  ti.Enabled = true;

      tv1.ContextMenuStrip = mnuCtx;
      tv1.ContextMenuStrip.Show(tv1, e.Location);
    }

    void Deploy() {
      if (!ValidateData()) {
        Cursor crs = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        smo.DeploySsbObj(ssbObj, dbServ.Name, dBase.Name, ssbTypeF3, false);
        isDirty = false;
        Cursor.Current = crs;
        MessageBox.Show("Object deployed successfully!", "Deployment");
        ExitAndClose();
      }


    }

    void SetControl() {
      if (objName.Contains("["))
        objName = objName.Remove(objName.IndexOf('['), 1);

      if (objName.Contains("]"))
        objName = objName.Remove(objName.IndexOf(']'), 1);

      ctrl.Text = objName;
      ExitAndClose();
    }

    bool ValidateData() {
      bool fail = false;
      string errMsg = null;

      if (level == 0) {
        errMsg = "You need to choose server and database to deploy to";
        fail = true;

      }
      else if (level == 1) {
        errMsg = "You need to choose database to deploy to";
        fail = true;

      }

      if(fail)
        MessageBox.Show(errMsg);

      return fail;



    }

    private void ExitAndClose() {
      if (isDirty && _state == SsbState.Cancelled && browse == false) {
        DialogResult res = MessageBox.Show("Do you want to deploy the object before exiting?", "Save Work", MessageBoxButtons.YesNoCancel);
        if (res == DialogResult.Yes)
          Deploy();
        else if (res == DialogResult.No) {
          this.Close();
          this.Dispose();
        }

      }
      else {
        this.Close();
        this.Dispose();

      }

    }

    #region event-methods

    void tv1_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e) {
      
      TreeNode tn = e.Node;
      tv1.BeginUpdate();
      tv1.SelectedNode = tn;
      if (TVSetUp.ExpandNodes1(tv1, tn, false, true, ssbTypeF3)) {
        tv1.SelectedNode = tn;
        tv1.EndUpdate();
      }

      else {
        e.Cancel = true;
        tv1.EndUpdate();
      }


    }

    void tv1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
      TreeNode tn = e.Node;
      if (e.Button == MouseButtons.Right) {
        SetupContextMenuAndShow(e);
        tv1.SelectedNode = e.Node;

      }
    }

    void tv1_AfterSelect(object sender, TreeViewEventArgs e) {
      Cursor csr = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      TreeNode tn = e.Node;
      level = tn.Level;
      btnPaste.Enabled = false;

      //servers
      if (level == 0) {
        dbServ = null; ;
        dBase = null;
      }

      //servername
      else if (level == 1) {
        dbServ = (SSBIServer)tn.Tag;
      }

      //dbname
      else if (level == 2) {
        dbServ = (SSBIServer)tn.Parent.Tag;
        dBase = (Database)tn.Tag;
        btnPaste.Enabled = ctrl == null;
      }

      //ssbobject-type
      else if (level == 3) {
        dbServ = (SSBIServer)tn.Parent.Parent.Tag;
        dBase = (Database)tn.Parent.Tag;
        btnPaste.Enabled = ctrl == null;

      }

      //ssbobject-type
      else if (level == 4) {
        dbServ = (SSBIServer)tn.Parent.Parent.Parent.Tag;
        dBase = (Database)tn.Parent.Parent.Tag;
        btnPaste.Enabled = true;
        objName = tn.Text;
        
      }

      Cursor.Current = csr;

    }

    void mnuTopFile_DropDownOpening(object sender, EventArgs e) {
      mnuTopFile.DropDownItems.Clear();
      mnuTopFile.DropDownItems.AddRange(new ToolStripItem[] { mnuSubSep, mnuSubExit });
      mnuTopFile.DropDownItems.AddRange(new ToolStripItem[] { mnuSubOpenConn, mnuSubExit });

      mnuSubOpenConn.Enabled = level == 1 && !dbServ.HasLoggedIn && !dbServ.IsTryingToConnect;
    
    
    }

    void mnuTopView_DropDownOpening(object sender, EventArgs e) {
      mnuTopView.DropDownItems.Clear();
      mnuTopView.DropDownItems.AddRange(new ToolStripItem[] { mnuSubRefresh });

    }

    void mnuTopEdit_DropDownOpening(object sender, EventArgs e) {
      mnuTopEdit.DropDownItems.Clear();
      mnuTopEdit.DropDownItems.AddRange(new ToolStripItem[] { mnuSubPaste});
      mnuSubPaste.Enabled = false;

      if (level > 1)
        mnuSubPaste.Enabled = true;

    }

    void mnuSubPaste_Click(object sender, EventArgs e) {
      Deploy(); ;
    }

    void btnPaste_Click(object sender, EventArgs e) {
      if (browse) {
        SetControl();

      }
      else
        Deploy();
    }

    void btnCancel_Click(object sender, EventArgs e) {
      _state = SsbState.Cancelled;
      ExitAndClose();
    }

    void mnuSubExit_Click(object sender, EventArgs e) {
      _state = SsbState.Cancelled;
      ExitAndClose();
    }

    void mnuSubOpenConn_Click(object sender, EventArgs e) {
      TVSetUp.Connect(tv1, tv1.SelectedNode);  
    
    }

    

    #endregion










    
  }
}