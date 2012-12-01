#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Broker;
using System.Collections.Specialized;
using System.Collections;
using System.IO;
using Microsoft.Samples.SqlServer;
using System.Xml;
using System.Data.SqlTypes;


#endregion



namespace SsbAdmin {
  partial class Form2 : Form {

    ToolStripMenuItem mnuTopFile;
    ToolStripMenuItem mnuTopEdit;
    ToolStripMenuItem mnuSubNewDropDown;
    ToolStripMenuItem mnuSubNew;
    ToolStripMenuItem mnuSubExit;
    ToolStripSeparator mnuSubSep;
    ToolStripMenuItem mnuSubRefresh;
    ToolStripMenuItem mnuSubSave;
    ToolStripMenuItem mnuSubMsgType;
    ToolStripMenuItem mnuSubContract;
    ToolStripMenuItem mnuSubQueue;

    SsbEnum ssbType;
    SSBIServer dbServ;
    Database dBase;
    object updatedobj = null;
    object objToUpdate = null;
    SsbState _state;
    bool isDirty = false;
    bool isEdit = false;
    bool listingDirty = false;
    string formText = "New";
    SSBIServiceListing sl = null;

    #region constructors

    public Form2(SsbEnum idx) : this(idx, null) { }

    public Form2(SsbEnum idx, SSBIServer srv):this(idx, srv, null) {}

    public Form2(SsbEnum idx, SSBIServer srv, Database db)
      : this(idx, srv, db, false, null) {
      }

    public Form2(SsbEnum idx, SSBIServer srv, Database db, bool toEdit, object smob) {

      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      InitializeComponent();
      isEdit = toEdit;
      if (isEdit)
        formText = "Edit";
      SetupMenus();
      SetUpEventHooks();
      ssbType = idx;
      SetUpTabs(idx);
      if (smob != null)
        objToUpdate = smob;

      _state = SsbState.Cancelled;

      if (srv != null) {
        dbServ = srv;
      }

      if (ssbType == SsbEnum.EndPoint) {
        db = ((SSBIDatabase)smo.GetObject2(dbServ, null, "master", SsbEnum.Database)).DataBase;
      }

      if (db!= null) {
        dBase = db;
      }


      SetUpTab();

      //we are editing
      if (isEdit && smob != null) {
        objToUpdate = smob;
        SetUpObject(smob);
      }
      if (ssbType == SsbEnum.CreateListing && smob != null)
        isDirty = true;
      else
        isDirty = false;
      this.DialogResult = DialogResult.Cancel;
      Cursor.Current = crs;

      

    }

    public Form2(SsbEnum idx, string _srvName, string _dbName, bool toEdit, object smob, Stream msgStream) {
      
    }
    #endregion

    #region event hooks

    void SetUpEventHooks() {

      this.cboValSchema.SelectedIndexChanged += new System.EventHandler(this.txtName_TextChanged);
      this.cboVal.SelectedIndexChanged += new System.EventHandler(this.cboVal_SelectedIndexChanged);
      this.cboUser.SelectedIndexChanged += new System.EventHandler(this.txtName_TextChanged);
      this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
      this.btnNewMsgType.Click += new System.EventHandler(this.button3_Click);
      this.button1.Click += new System.EventHandler(this.button1_Click);
      this.cboQDb.SelectedIndexChanged += new System.EventHandler(this.cboQDb_SelectedIndexChanged);
      this.cboProc.SelectedIndexChanged += new System.EventHandler(this.txtName_TextChanged);
      this.txtExecuteAs.TextChanged += new System.EventHandler(this.txtName_TextChanged);
      this.txtReaders.TextChanged += new System.EventHandler(this.txtName_TextChanged);
      this.chkActivation.CheckStateChanged += new System.EventHandler(this.txtName_TextChanged);
      this.chkActivation.CheckedChanged += new System.EventHandler(this.chkActivation_CheckedChanged);
      this.chkRetention.CheckStateChanged += new System.EventHandler(this.txtName_TextChanged);
      this.chkStatus.CheckStateChanged += new System.EventHandler(this.txtName_TextChanged);
      this.btnNewQ.Click += new System.EventHandler(this.btnNewQ_Click);
      this.btnNewCtr.Click += new System.EventHandler(this.btnNewCtr_Click);
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      this.btnAddCtr.Click += new System.EventHandler(this.btnAddCtr_Click);
      this.cboQueue.SelectedIndexChanged += new System.EventHandler(this.cboQueue_SelectedIndexChanged);
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.button2.Click += new System.EventHandler(this.button2_Click);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_Closing);

      lgn_rdWin.CheckedChanged += new EventHandler(lgn_rdWin_CheckedChanged);
      lgn_txtLoginName.TextChanged += new EventHandler(TextChanged2);
      lgn_txtConfirm.TextChanged += new EventHandler(TextChanged2);
      lgn_txtPwd.TextChanged += new EventHandler(TextChanged2);
      
      this.rdUser.CheckedChanged += new EventHandler(rdUser_CheckedChanged);
      this.rdTCP.CheckedChanged += new EventHandler(rdTCP_CheckedChanged);

      mnuSubNewDropDown.DropDownItemClicked += new ToolStripItemClickedEventHandler(mnuSubNewDropDown_DropDownItemClicked);
      mnuSubNewDropDown.DropDownOpening += new EventHandler(mnuSubNewDropDown_DropDownOpening);
      mnuSubNewDropDown.DropDownItems.AddRange(new ToolStripItem[] { mnuSubMsgType, mnuSubContract, mnuSubQueue });
      mnuSubSave.Click += new EventHandler(mnuSubSave_Click);
      mnuSubExit.Click += new EventHandler(mnuSubExit_Click);

      db_chkMasterKey.CheckedChanged += new EventHandler(db_chkMasterKey_CheckedChanged);

      cnv_cboFrmServ.SelectedIndexChanged += new EventHandler(cnv_cboFrmServ_SelectedIndexChanged);
      cnv_btnFind.Click += new EventHandler(cnv_btnFind_Click);

      msg_cboConv.SelectedIndexChanged += new EventHandler(msg_cboConv_SelectedIndexChanged);
      msg_cboMsgType.SelectedIndexChanged += new EventHandler(msg_cboMsgType_SelectedIndexChanged);

      rem_btnFind.Click += new EventHandler(rem_btnFind_Click);
      rt_btnFind.Click += new EventHandler(rt_btnFind_Click);

      cert_cboSource.SelectedIndexChanged += new EventHandler(cert_cboSource_SelectedIndexChanged);

      //cert_cboSource.S += new EventHandler(cert_cboSource_Click);
      cert_chkMasterKey.CheckedChanged += new EventHandler(cert_chkMasterKey_CheckedChanged);
      cert_btnBrowseCert.Click +=new EventHandler(cert_Browse_Click);
      cert_btnBrowsePvk.Click += new EventHandler(cert_Browse_Click);
      
      usr_btnNewCert.Click += new EventHandler(usr_btnNewCert_Click);
      usr_btnNewLogin.Click += new EventHandler(usr_btnNewLogin_Click);
      usr_chkLogin.CheckedChanged += new EventHandler(usr_chkLogin_CheckedChanged);

      btnNewUser.Click += new EventHandler(btnNewUser_Click);

      ep_cboAuth.SelectedIndexChanged +=new EventHandler(ep_cboAuth_SelectedIndexChanged);
      ep_btnNewCert.Click += new EventHandler(ep_btnNewCert_Click);
      ep_cboEncrypt.SelectedIndexChanged +=new EventHandler(ep_cboEncrypt_SelectedIndexChanged);
      ep_chkFwd.CheckedChanged += new EventHandler(ep_chkFwd_CheckedChanged);

      svcL_btnBrowse.Click += new EventHandler(svcL_btnBrowse_Click);
      svcL_btnView.Click += new EventHandler(svcL_btnView_Click);
    
    }

    

    

    

    
    

    
    
    

    

   
    #endregion

    #region methods

    void SetupMenus() {
      mnuTopFile = new ToolStripMenuItem();
      mnuSubNewDropDown = new ToolStripMenuItem();
      mnuTopEdit = new ToolStripMenuItem();
      mnuSubNew = new ToolStripMenuItem();
      mnuSubExit = new ToolStripMenuItem();
      mnuSubRefresh = new ToolStripMenuItem();
      mnuSubSave = new ToolStripMenuItem();
      mnuSubMsgType = new ToolStripMenuItem();
      mnuSubContract = new ToolStripMenuItem();
      mnuSubSep = new ToolStripSeparator();
      mnuSubQueue = new ToolStripMenuItem();

      menuStrip1.Items.AddRange(new ToolStripItem[] { mnuTopFile });
      mnuTopFile.DropDownItems.AddRange(new ToolStripItem[] { mnuSubNewDropDown, mnuSubSep, mnuSubSave, mnuSubSep, mnuSubExit });

      mnuTopFile.Name = "mnuTopFile";
      mnuTopFile.Text = "&File";


      mnuTopEdit.Name = "mnuTopEdit";
      mnuTopEdit.Text = "&Edit";
      

      mnuSubNewDropDown.Name = "mnuSubNewDropDown";
      mnuSubNewDropDown.Text = "New";
      
      mnuSubNew.Name = "mnuSubNew";
      mnuSubNew.Text = "New";
      
      mnuSubExit.Name = "mnuSubExit";
      mnuSubExit.Text = "E&xit";
      
      mnuSubRefresh.Name = "mnuSubRefresh";
      mnuSubRefresh.Text = "Refresh";
      mnuSubSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.F5)));
      
      
      mnuSubSave.Name = "mnuSubSave";
      mnuSubSave.Text = "&Save";
      mnuSubSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      mnuSubSave.ShowShortcutKeys = true;


      mnuSubMsgType.Name = "mnuSubMsgType";
      mnuSubMsgType.Tag = 0;
      mnuSubMsgType.Text = "Message Type";
      
      mnuSubContract.Name = "mnuSubContract";
      mnuSubContract.Tag = 1;
      mnuSubContract.Text = "Contract";
      
      mnuSubQueue.Name = "mnuSubQueue";
      mnuSubQueue.Tag = 2;
      mnuSubQueue.Text = "Queue";
          

    }

    void SetUpTab() {
      IEnumerator en = null; ;
      UserCollection uc = null;
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      cboUser.Items.Clear();
      if (ssbType != SsbEnum.Database && ssbType != SsbEnum.Login && ssbType != SsbEnum.EndPoint && ssbType != SsbEnum.User) {
        //uc = smo.GetUsers(srvName, dbName);
        uc = dBase.Users;
        cboUser.Items.Clear();
        cboUser.DisplayMember = "Name";
        //cboUser.DataSource = uc;
        foreach (User usr in uc)
          cboUser.Items.Add(usr);
      }

      XmlSchemaCollectionCollection xmlColl = null;
      switch (ssbType) {

        case SsbEnum.Login  :
          this.Text = string.Format("{0} Login", formText);
          lgn_rdWin.Checked = true;
          lgn_grpSql.Enabled = false;
          lgn_chkEnforcePolicy.Checked = true;

          break;


        case SsbEnum.Database :
          this.Text = string.Format("{0} Database", formText);
          db_chkMasterKey.Checked = true;
          tabPage9.Controls.Add(txtName);
          break;

        case SsbEnum.MessageType :
          this.Text = string.Format("{0} Message Type", formText);
          cboValSchema.Items.Clear();
          cboVal.Items.Clear();
          if (dBase != null) {
            
            //xmlColl = smo.GetXmlSchemaCollections(srvName, dbName);

            xmlColl = dBase.XmlSchemaCollections;
            
            foreach (XmlSchemaCollection xc in xmlColl)
              cboValSchema.Items.Add(xc.Name);

            cboValSchema.Enabled = false;

            Array val = Enum.GetValues(typeof(MessageTypeValidation));
            cboVal.DataSource = val;

          }
          break;
          

        case SsbEnum.Contract:
          this.Text = string.Format("{0} Contract", formText);
          lbMsgTypes.Items.Clear();
          tabPage2.Controls.Add(txtName);
          tabPage2.Controls.Add(cboUser);
          tabPage2.Controls.Add(btnNewUser);
          en = smo.GetSSBObjects2(dbServ, dBase, SsbEnum.MessageType).GetEnumerator();
          lbMsgTypes.DisplayMember = "Name";
          while (en.MoveNext()) {
            MessageType m = (MessageType)en.Current;
            lbMsgTypes.Items.Add(m);
          }
         
          DataGridViewComboBoxColumn cbo = new DataGridViewComboBoxColumn();
          cbo.DropDownWidth = 160;
          cbo.Width = 120;
          cbo.Name = "SentBy";
          cbo.HeaderText = "Sent By";
          cbo.MaxDropDownItems = 3;
          if (dvMsgTypes.ColumnCount > 1)
            dvMsgTypes.Columns.Remove("SentBy");
          dvMsgTypes.Rows.Clear();
          Array ms = Enum.GetValues(typeof(MessageSource));
          for (int i = 0; i < ms.Length; i++)
            cbo.Items.Add(ms.GetValue(i).ToString());
          dvMsgTypes.Columns.Add(cbo);
          dvMsgTypes.Columns[0].HeaderText = "Name";
          break;

        case SsbEnum.Queu:
          LoadDataBases(dbServ.SMOServer);
          this.Text = string.Format("{0} Queue", formText);
          tabPage3.Controls.Add(txtName);
          if (dBase != null) {
            cboQDb.SelectedItem = dBase;
          }
          chkStatus.Checked = true;
          cboQDb.Enabled = false;
          cboProc.Enabled = false;
          txtReaders.Text = "0";
          txtReaders.Enabled = false;
          txtExecuteAs.Text = "dbo";
          txtExecuteAs.Enabled = false;
          grpExecuteAs.Enabled = false;
          rdSelf.Checked = true;
          txtExecuteAs.Enabled = false;
          break;

        case SsbEnum.Service:
          this.Text = string.Format("{0} Service", formText);
          cboQueue.Items.Clear();
          lbCtr.Items.Clear();
          lbChosenCtr.Items.Clear();
          cboQueue.DisplayMember = "Name";
          lbCtr.DisplayMember = "Name";
          tabPage4.Controls.Add(txtName);
          tabPage4.Controls.Add(cboUser);
          tabPage4.Controls.Add(btnNewUser);
          en = smo.GetSSBObjects2(dbServ, dBase, SsbEnum.Contract).GetEnumerator();
          //lbCtr.Items.Add("<new>");
          while (en.MoveNext()) {
            ServiceContract sc = (ServiceContract)en.Current;
            lbCtr.Items.Add(sc);
          }

          en = smo.GetSSBObjects2(dbServ, dBase, SsbEnum.Queu).GetEnumerator();
          while (en.MoveNext()) {
            ServiceQueue sq = (ServiceQueue)en.Current;
            cboQueue.Items.Add(sq);
          }

          break;

        case SsbEnum.Route:
          this.Text = string.Format("{0} Route", formText);
          tabPage5.Controls.Add(txtName);
          tabPage5.Controls.Add(cboUser);
          rdLocal.Checked = true;
          txtAddress.Enabled = false;
          txtMirror.Enabled = false;
          break;

        case SsbEnum.RemoteBinding:
          this.Text = string.Format("{0} Remote Service Binding", formText);
          tabPage6.Controls.Add(txtName);
          tabPage6.Controls.Add(cboUser);
          tabPage6.Controls.Add(btnNewUser);
          
          foreach (User usr2 in uc)
            cboRemUser.Items.Add(usr2.Name);

          break;

        case SsbEnum.Conversation:
          this.Text = string.Format("{0} Conversation", formText);
          cnv_cboContract.Items.Clear();
          cnv_cboFrmServ.Items.Clear();
          cnv_cboFrmServ.DisplayMember = "Name";
          if (!isEdit && objToUpdate == null) {
            en = smo.GetSSBObjects2(dbServ, dBase, SsbEnum.Service).GetEnumerator();
            while (en.MoveNext()) {
              BrokerService bServ = (BrokerService)en.Current;
              cnv_cboFrmServ.Items.Add(bServ);
            }
          }
          else if (!isEdit && objToUpdate != null) {
            //BrokerService bServ = ((SSBIService)objToUpdate).SMOService;
            BrokerService bServ = (BrokerService)objToUpdate;
              cnv_cboFrmServ.Items.Add(bServ);
              cnv_cboFrmServ.SelectedItem = bServ;
          }

          cnv_chkEncryption.Checked = true;
          
        break;


        case SsbEnum.Message:
          if (!isEdit)
            formText = "Create and Send";
          else
            formText = "View";
          this.Text = string.Format("{0} Message", formText);

          msg_cboConv.Items.Clear();
          msg_cboMsgType.Items.Clear();
          if (!isEdit) {
            SSBIConversation conv = (SSBIConversation)objToUpdate;
            msg_cboConv.DisplayMember = "Handle";
            msg_cboConv.Items.Add(conv);
            msg_cboConv.SelectedItem = conv;
            msg_txtFrom.Text = conv.FromService;
            msg_txtTo.Text = conv.ToService;
          }
          else {
            if (objToUpdate.GetType() == typeof(SSBIMessage)) {
              SSBIMessage msg = (SSBIMessage)objToUpdate;
              msg_cboConv.Text = msg.ConversationHandle.ToString();
              msg_txtFrom.Text = msg.RemoteServiceName;
              msg_txtTo.Text = msg.ServiceName;
              msg_cboMsgType.Text = msg.Type;
              msg_txtVal.Text = msg.Validation;
              if (msg.Body != null) {
                StreamReader sr = new StreamReader(msg.Body);
                msg_rchMsg.Text = sr.ReadToEnd();
                msg.Body.Position = 0;
              }
            }
            else if (objToUpdate.GetType() == typeof(TxStatus)) {
              TxStatus txStat = (TxStatus)objToUpdate;
              msg_cboConv.Text = txStat.ConversationHandle.ToString();
              msg_txtFrom.Text = txStat.RemoteService;//msg.RemoteServiceName;
              msg_txtTo.Text = txStat.ServiceName;
              msg_cboMsgType.Text = txStat.MessageType;
              msg_rchMsg.Text = txStat.TransmissionStatus;
            }

            button2.Visible = false;
            

          }

        break;

        case SsbEnum.Certificate :
          this.Text = string.Format("{0} Certificate", formText);
          cert_cboSource.Items.Add("Certificate with Private Key File");
          cert_cboSource.Items.Add("Self Signed Certificate");
          cert_cboSource.Items.Add("Certificate from Public Key");
          cert_cboSource.SelectedIndex = 0;
          cert_chkMasterKey.Checked = true;
          if (!isEdit) {
            cert_dtExpiry.Value = cert_dtValidFrom.Value.AddYears(1);
            cert_dtValidFrom.MinDate = DateTime.Now;
            cert_dtExpiry.MinDate = cert_dtValidFrom.Value;
          }
          cert_txtEncrypt.Enabled = false;
           

          tabPage12.Controls.Add(txtName);
          tabPage12.Controls.Add(cboUser);
          tabPage12.Controls.Add(btnNewUser);
          break;

        case SsbEnum.User:
          this.Text = string.Format("{0} User", formText);
          usr_cboLogin.Items.Clear();
          usr_cboCerts.Items.Clear();

          usr_chkLogin.Checked = true;

          tabPage13.Controls.Add(txtName);
          foreach (Login l in dBase.Parent.Logins) {
            if(!l.Name.Contains("#"))
              usr_cboLogin.Items.Add(l.Name);

          }

          foreach (Certificate c in dBase.Certificates) {
            if(!c.Name.Contains("#"))
              usr_cboCerts.Items.Add(c.Name);
          }
          
          break;

        case SsbEnum.EndPoint :
          this.Text = string.Format("{0} EndPoint", formText);
          cboUser.Items.Clear();
          ep_cboCert.Items.Clear();
          ep_cboCert.DisplayMember = "Name";
          tabPage11.Controls.Add(txtName);
          tabPage11.Controls.Add(cboUser);
          
          foreach (Login l in dbServ.SMOServer.Logins){
            if(!l.Name.Contains("#"))
              ep_cboAuth.Items.Add(l.Name);

          }

          foreach (Certificate c in dBase.Certificates) {
            
            if(!c.Name.Contains("#"))
              ep_cboCert.Items.Add(c.Name);
          }
        
          ep_cboState.DataSource = Enum.GetValues(typeof(EndpointState));
          ep_cboState.SelectedIndex = (int)EndpointState.Stopped;
          ep_txtPort.Text = "4022";
          ep_txtIp.Text = "ALL";
          ep_cboAuth.DataSource = Enum.GetValues(typeof(EndpointAuthenticationOrder));
          //TODO - seems that the enum is not correct
          ep_cboAuth.SelectedIndex = 2;// (int)EndpointAuthenticationOrder.Negotiate;
          ep_cboEncrypt.DataSource = Enum.GetValues(typeof(EndpointEncryption));
          ep_cboEncrypt.SelectedIndex = (int)EndpointEncryption.Required;
          ep_cboAlgorithm.DataSource = Enum.GetValues(typeof(EndpointEncryptionAlgorithm));
          ep_cboAlgorithm.SelectedIndex = (int)EndpointEncryptionAlgorithm.RC4;
          ep_pnlCert.Enabled = false;
          //ep_pnlAlgo.Enabled = false;
          ep_pnlMsgFwd.Enabled = false;
          break;

        case SsbEnum.CreateListing:
          this.Text = string.Format("Create/Export Service Listing");
          tabPage14.Controls.Add(cnv_cboFrmServ);
          cnv_cboFrmServ.Items.Clear();
          cnv_cboFrmServ.DisplayMember = "Name";
          svcL_PreviewListing.Enabled = false;
          svcL_grpListing.Enabled = false;
          en = smo.GetSSBObjects2(dbServ, dBase, SsbEnum.Service).GetEnumerator();
          if (objToUpdate == null) {
            while (en.MoveNext()) {
              BrokerService bServ = (BrokerService)en.Current;
              cnv_cboFrmServ.Items.Add(bServ);
            }
            
          }
          else if (objToUpdate != null) {
            BrokerService bServ = (BrokerService)objToUpdate;
            cnv_cboFrmServ.Items.Add(bServ);
            cnv_cboFrmServ.SelectedIndex = 0;
            svcL_PreviewListing.Enabled = true;
            isDirty = true;

          }

          svcL_cboCerts.Items.Clear();
          foreach (Certificate c in dBase.Certificates) {
            if (!c.Name.Contains("#") && c.PrivateKeyEncryptionType == PrivateKeyEncryptionType.MasterKey)
              svcL_cboCerts.Items.Add(c.Name);
          }

          break;

        case SsbEnum.ImportListing :
          this.Text = string.Format("Import Service Listing");
          tabPage15.Controls.Add(txtName);
          svcL_btnView.Enabled = false;
          break;



      }


      tabControl1.TabIndex = 80;
      tabControl1.TabStop = false;
      txtName.TabIndex = 0;
      
      txtName.Focus();
      //isDirty = false;
      Cursor.Current = crs;

    }

    int UpDateSsb() {

      updatedobj = null;
      //if (!isDirty)
        //return 0;
      if (!ValidateData()) {
        Cursor crs = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;

        try {
          Database db = null;
          
          ServiceBroker sb = null;
          if (ssbType != SsbEnum.Database && ssbType != SsbEnum.Login && ssbType != SsbEnum.EndPoint) {
             sb = dBase.ServiceBroker;
          }
          switch (ssbType) {
            case SsbEnum.Database:
              MasterKey mk = null;
              SSBIDatabase sbd = null;
              if (isEdit) {
                sbd = (SSBIDatabase)objToUpdate;
                db = sbd.DataBase;
              }
              else {
                db = new Database();
                db.Name = txtName.Text;
                db.Parent = dbServ.SMOServer;
              }

              if (isEdit) {
               if(db.MasterKey != null && db_chkMasterKey.Checked == false) {
                 mk = db.MasterKey;
                 mk.Drop();
                 
               }
               else if (db.MasterKey == null && db_chkMasterKey.Checked) {
                 mk = new MasterKey();
                 mk.Parent = db;
                 mk.Create(db_txtMkPwd.Text);
               }

               db.Alter();
               if (sbd.IsTrustworthy != db_chkTrustWorthy.Checked)
                 sbd.IsTrustworthy = db_chkTrustWorthy.Checked;
               

              }
              else {
                db.Create();
                sbd = new SSBIDatabase(db);
                
                
                if (db_chkMasterKey.Checked) {
                  mk = new MasterKey();
                  mk.Parent = db;
                  mk.Create(db_txtMkPwd.Text);

                }

                if (db_chkTrustWorthy.Checked) {
                  sbd.IsTrustworthy = true;
                }

              }
              if (dBase == null)
                dBase = db;

              //Server serv = db.Parent;


              updatedobj = db;
              break;
            case SsbEnum.MessageType:
              MessageType mt = null;
              if (isEdit)
                mt = (MessageType)objToUpdate;
              else {
                mt = new MessageType();
                mt.Parent = sb;
                mt.Name = txtName.Text;
              }
              if (cboUser.Text != string.Empty)
                mt.Owner = cboUser.Text;
              mt.MessageTypeValidation = (MessageTypeValidation)Enum.Parse(typeof(MessageTypeValidation), cboVal.Text);
              if (cboValSchema.Enabled)
                mt.ValidationXmlSchemaCollection = cboValSchema.Text;

              if (isEdit)
                mt.Alter();
              else
                mt.Create();
              updatedobj = mt;
              break;

            case SsbEnum.Contract:
              ServiceContract sc = new ServiceContract();
              sc.Parent = sb;
              sc.Name = txtName.Text;
              if (cboUser.Text != string.Empty)
                sc.Owner = cboUser.Text;
              //get the message types
              foreach (DataGridViewRow row in dvMsgTypes.Rows) {
                sc.MessageTypeMappings.Add(new MessageTypeMapping(sc, row.Cells[0].Value.ToString(), (MessageSource)Enum.Parse(typeof(MessageSource), row.Cells[1].Value.ToString())));
              }

              if (isEdit)
                sc.Alter();
              else
                sc.Create();

              updatedobj = sc;
              break;

            case SsbEnum.Queu:
              ServiceQueue q = null;
              if (isEdit)
                q = (ServiceQueue)objToUpdate;
              else {
                q = new ServiceQueue();
                q.Parent = sb;
                q.Name = txtName.Text;
              }
              q.IsEnqueueEnabled = chkStatus.Checked;
              if (chkRetention.Checked)
                q.IsRetentionEnabled = true;

              //if (chkActivation.Checked) {
              //if(isEdit)
              //  q.IsActivationEnabled = chkActivation.Checked;
              //
              if (chkActivation.Checked) {
                q.IsActivationEnabled = chkActivation.Checked;
                if (dBase.Name != cboQDb.Text)
                  q.ProcedureDatabase = cboQDb.Text;
                StoredProcedure sp = (StoredProcedure)cboProc.SelectedItem;
                q.ProcedureSchema = sp.Schema;
                q.ProcedureName = cboProc.Text;
                q.MaxReaders = short.Parse(txtReaders.Text);
                if (rdOwner.Checked)
                  q.ActivationExecutionContext = ActivationExecutionContext.Owner;
                else if (rdSelf.Checked)
                  q.ActivationExecutionContext = ActivationExecutionContext.Self;
                else if (rdUser.Checked) {
                  q.ActivationExecutionContext = ActivationExecutionContext.ExecuteAsUser;
                  q.ExecutionContextPrincipal = txtExecuteAs.Text;
                }


              }

              if (isEdit)
                q.Alter();
              else
                q.Create();

              updatedobj = q;

              break;

            case SsbEnum.Service:
              BrokerService bserv = null;
              if (isEdit)
                bserv = (BrokerService)objToUpdate;
              else {
                bserv = new BrokerService();
                bserv.Parent = sb;
                bserv.Name = txtName.Text;
              }
              if (cboUser.Text != string.Empty)
                bserv.Owner = cboUser.Text;

              ServiceQueue servq = (ServiceQueue)cboQueue.SelectedItem;
              bserv.QueueName = servq.Name;
              bserv.QueueSchema = servq.Schema;

              if (lbChosenCtr.Items.Count > 0) {
                foreach (object o in lbChosenCtr.Items) {
                  ServiceContract servctr = o as ServiceContract;
                  ServiceContractMapping scm = new ServiceContractMapping(bserv, servctr.Name);
                  bserv.ServiceContractMappings.Add(scm);
                }
              }
              

              if (isEdit)
                bserv.Alter();
              else
                bserv.Create();

              updatedobj = bserv;

              break;

            case SsbEnum.Route:
              ServiceRoute srt = null;
              if (isEdit)
                srt = (ServiceRoute)objToUpdate;
              else {
                srt = new ServiceRoute();
                srt.Name = txtName.Text;
                srt.Parent = sb;
              }

              if (cboUser.Text != string.Empty)
                srt.Owner = cboUser.Text;

              if (textBroker.Text != string.Empty)
                srt.BrokerInstance = textBroker.Text;

              if (textRemote.Text != string.Empty)
                srt.RemoteService = textRemote.Text;

              if (textLifeTime.Text != string.Empty)
                srt.ExpirationDate = DateTime.Parse(textLifeTime.Text);

              if (rdLocal.Checked)
                srt.Address = "LOCAL";

              if (rdTransport.Checked)
                srt.Address = "TRANSPORT";

              if (rdTCP.Checked)
                srt.Address = "TCP://" + txtAddress.Text;

              if (txtMirror.Text != string.Empty)
                srt.MirrorAddress = "TCP://" + txtMirror.Text;

              //StringCollection sColl = srt.Script();
              //foreach (string s in sColl)
              //  MessageBox.Show(s);

              if (isEdit)
                srt.Alter();
              else
                srt.Create();

              updatedobj = srt;

              break;

            case SsbEnum.RemoteBinding:
              RemoteServiceBinding remBind = null;
              if (isEdit)
                remBind = (RemoteServiceBinding)objToUpdate;
              else {
                remBind = new RemoteServiceBinding();
                remBind.Name = txtName.Text;
                remBind.Parent = sb;
              }

              if (cboUser.Text != string.Empty)
                remBind.Owner = cboUser.Text;

              remBind.RemoteService = textRemServ.Text;

              remBind.CertificateUser = cboRemUser.Text;
              remBind.IsAnonymous = chkAnon.Checked;

              StringCollection sColl = remBind.Script();
              foreach (string s in sColl)
                MessageBox.Show(s);

              if (isEdit)
                remBind.Alter();
              else
                remBind.Create();

              updatedobj = remBind;

              break;

            case SsbEnum.Conversation:
              TimeSpan ts = TimeSpan.Zero;
              Guid grpHandle = Guid.Empty;
              string convContract = "DEFAULT";
              BrokerService bServ = (BrokerService)cnv_cboFrmServ.SelectedItem;
             
              string toService = cnv_txtToSrv.Text;
              
              if (cnv_txtLifetime.Text != string.Empty && cnv_txtLifetime.Text != "0")
                ts = TimeSpan.FromSeconds(double.Parse(cnv_txtLifetime.Text));

              if (cnv_cboContract.Text != string.Empty)
                convContract = cnv_cboContract.Text;

              if (cnv_txtRelGrpHndl.Text != string.Empty)
                grpHandle = new Guid(cnv_txtRelGrpHndl.Text);

              //get a service object
              Service smoserv = smo.GetSSBIService(bServ.Parent.Parent, bServ.Name);
              if (smoserv.Connection.State == ConnectionState.Closed)
                smoserv.Connection.Open();

              smoserv.Connection.ChangeDatabase(bServ.Parent.Parent.Name);
              updatedobj = smoserv.BeginDialog(toService, convContract, ts, cnv_chkEncryption.Checked, grpHandle);
              break;

            case SsbEnum.Message:
              SSBIConversation msgConv = (SSBIConversation)msg_cboConv.SelectedItem;
              string servName = msg_txtFrom.Text;
              //we need a service object
              Service msgSsbiServ = smo.GetSSBIService(dBase, msgConv.FromService);
              if (msgSsbiServ.Connection.State== ConnectionState.Closed)
                 msgSsbiServ.Connection.Open();

               msgSsbiServ.Connection.ChangeDatabase(dBase.Name);
              Conversation msgCnv = new Conversation(msgSsbiServ, msgConv.Handle);
              string msgType = msg_cboMsgType.SelectedText;
              string msgString = msg_rchMsg.Text;
              msgType = msg_cboMsgType.Text;
              MemoryStream msgBody = new MemoryStream(Encoding.ASCII.GetBytes(msgString));

              Microsoft.Samples.SqlServer.Message msg = new Microsoft.Samples.SqlServer.Message(msgType, msgBody);
              msgCnv.Send(msg);

  

              break;

            case SsbEnum.Login :
              string pwd = "";
              Login lg = new Login();
              lg.Parent = dbServ.SMOServer;
              lg.Name = lgn_txtLoginName.Text;
              if (lgn_rdSql.Checked) {
                pwd = lgn_txtPwd.Text;
                lg.PasswordPolicyEnforced = lgn_chkEnforcePolicy.Checked;
                lg.LoginType = LoginType.SqlLogin;
                lg.Create(pwd);
              }
              else {
                lg.Create();
              }

              updatedobj = lg;
              break;

            case SsbEnum.Certificate:
              string certOwner  = "dbo";
              int certSource = cert_cboSource.SelectedIndex;
              Certificate cert = new Certificate();
              if(!isEdit) {
                cert.Name = txtName.Text;
                if(cboUser.Text != "")
                  certOwner = cboUser.Text;
                cert.Parent = dBase;
                cert.Owner = certOwner;
                

              }
                
              cert.ActiveForServiceBrokerDialog = cert_chkBeginDlg.Checked;
              

             
              if (certSource == 0) {
                if (!isEdit) {
                  if (cert_chkMasterKey.Checked)
                    cert.Create(cert_txtCertPath.Text, CertificateSourceType.File, cert_txtPrivPath.Text, cert_txtDecrypt.Text);
                  else
                    cert.Create(cert_txtCertPath.Text, CertificateSourceType.File, cert_txtPrivPath.Text, cert_txtDecrypt.Text, cert_txtEncrypt.Text);
                  
                }
              }
              else if (certSource == 1) {
                if (!isEdit) {
                  cert.StartDate = cert_dtValidFrom.Value;
                  cert.ExpirationDate = cert_dtExpiry.Value;
                  cert.Subject = cert_txtCertPath.Text;

                  if (cert_chkMasterKey.Checked) {
                    cert.Create();
                  }
                  else {
                    cert.Create(cert_txtEncrypt.Text);
                  }
                }
                

              }
              else if (certSource == 2) {
                if (!isEdit) {
                  cert.Create(cert_txtCertPath.Text, CertificateSourceType.File);
                }

              }

              if (isEdit)
                cert.Alter();

              updatedobj = cert;
              break;

            case SsbEnum.User :
              User usr;
              if (!isEdit) {
                usr = new User();
                usr.Name = txtName.Text;
                usr.Parent = dBase;
                if(usr_chkLogin.Checked)
                  usr.Login = usr_cboLogin.Text;
                

              }
              else
                usr = (User)objToUpdate;

              if (usr_cboCerts.SelectedIndex != -1)
                usr.Certificate = usr_cboCerts.Text;

              
              if (!isEdit)
                if (usr_chkLogin.Checked)
                  usr.Create();
                else
                  smo.CreateUserWithNoLogin(usr);

              else
                usr.Alter();
              updatedobj = usr;
              break;

            case SsbEnum.EndPoint :
              Endpoint ep = null;
              if (!isEdit) {
                ep = new Endpoint();
                ep.Name = txtName.Text;
                ep.Parent = dbServ.SMOServer;
                ep.ProtocolType = ProtocolType.Tcp;
                ep.EndpointType = EndpointType.ServiceBroker;
              }
              else
                ep = ((SSBIEndpoint)objToUpdate).EndPoint;

              ep.Protocol.Tcp.ListenerPort = int.Parse(ep_txtPort.Text);
              if (ep_txtIp.Text == "ALL")
                ep.Protocol.Tcp.ListenerIPAddress = System.Net.IPAddress.Any;
              else {
                ep.Protocol.Tcp.ListenerIPAddress = System.Net.IPAddress.Parse(ep_txtIp.Text);
              }
              ep.Payload.ServiceBroker.EndpointAuthenticationOrder = (EndpointAuthenticationOrder)ep_cboAuth.SelectedItem;
              if (ep_cboCert.SelectedIndex != -1)
                ep.Payload.ServiceBroker.Certificate = ep_cboCert.Text;

              ep.Payload.ServiceBroker.EndpointEncryption = (EndpointEncryption)ep_cboEncrypt.SelectedItem;
              if (ep_cboAlgorithm.SelectedIndex != -1)
                ep.Payload.ServiceBroker.EndpointEncryptionAlgorithm = (EndpointEncryptionAlgorithm)ep_cboAlgorithm.SelectedItem;

              ep.Payload.ServiceBroker.IsMessageForwardingEnabled = ep_chkFwd.Checked;

              if (ep_txtSize.Text != string.Empty)
                ep.Payload.ServiceBroker.MessageForwardingSize = int.Parse(ep_txtSize.Text);

              if(!isEdit)  
                ep.Create();

              

              switch ((EndpointState)ep_cboState.SelectedIndex) {
                case EndpointState.Disabled :
                  ep.Disable();
                  break;

                case EndpointState.Started :
                  ep.Start();
                  break;
                case EndpointState.Stopped :
                  if (isEdit)
                    ep.Stop();
                  break;
              }

              if (isEdit)
                ep.Alter();
              
              break;

            case SsbEnum.CreateListing :
              CreateListing(true);
              
              break;
     


          }

          

          if (isEdit)
            _state = SsbState.Edited;
          else
            _state = SsbState.New;

          Cursor.Current = crs;

          this.DialogResult = DialogResult.OK;

          ExitAndClose();


        }
        catch (FailedOperationException e) {
          smo.ShowException(e);
        }
        catch (Exception ex) {
          smo.ShowException(ex);
        }

        finally {
          if(dbServ !=null)
            dbServ.SMOServer.ConnectionContext.Disconnect();

        }

      }

      return 0;


    }

    private SqlXml CreateListing(bool saveToFile) {
      SqlXml listing;
      if (listingDirty) {
        Certificate c = null;
        sl = new SSBIServiceListing(dBase);
        BrokerService srvc = (BrokerService)cnv_cboFrmServ.Items[cnv_cboFrmServ.SelectedIndex];
        if (svcL_cboCerts.Text != "")
          listing = sl.Create(srvc, svcL_cboCerts.Text);
        else
          listing = sl.Create(srvc);
      }
      else
        listing = sl.Listing;

      listingDirty = false;
      isDirty = true;

      if (saveToFile) {
        string fileFilter = "XML file (*.xml)|*.xml|All files (*.*)|*.*";
        string fileName;
        if (DoDialog(false, fileFilter, "c:\\", out fileName) ){
          FileStream fs = new FileStream(fileName,FileMode.OpenOrCreate, FileAccess.Write);
          TextWriter w = new StreamWriter(fs);
          XmlReader r = listing.CreateReader();
          r.Read();
          w.Write(r.ReadOuterXml());
          w.Flush();
          w.Close();
          fs.Close();

        }



      }

      return listing;



    }

    void SetUpObject(object smob) {

      switch (ssbType) {
        case SsbEnum.Database:
          SSBIDatabase sdb = (SSBIDatabase)smob;
          Database db = sdb.DataBase;
          txtName.Text = db.Name;
          db_chkMasterKey.Checked = db.MasterKey != null;
          db_chkTrustWorthy.Checked = sdb.IsTrustworthy;
          if (db.MasterKey != null) {
            db_txtMkPwd.Enabled = false;
          }

          break;
        case SsbEnum.MessageType:
          MessageType mt = (MessageType)smob;
          txtName.Text = mt.Name;
          if (mt.Owner != string.Empty)
            cboUser.SelectedItem = mt.Owner;
          cboVal.SelectedItem = mt.MessageTypeValidation;
          if (mt.ValidationXmlSchemaCollection.Length > 0)
            cboValSchema.SelectedItem = mt.ValidationXmlSchemaCollection;

          break;

        case SsbEnum.Queu:
          ServiceQueue q = (ServiceQueue)smob;
          txtName.Text = q.Name;
          chkStatus.Checked = q.IsEnqueueEnabled;
          chkRetention.Checked = q.IsRetentionEnabled;
          chkActivation.Checked = q.IsActivationEnabled;
          if (q.ProcedureDatabase == "")
            cboQDb.SelectedItem = dBase;
          else
            cboQDb.SelectedItem = q.ProcedureDatabase;
          
          cboProc.Text = q.ProcedureName;
          txtReaders.Text = q.MaxReaders.ToString();
          switch (q.ActivationExecutionContext) {
            case ActivationExecutionContext.Self :
              rdSelf.Checked = true;
              break;

            case ActivationExecutionContext.Owner:
              rdOwner.Checked = true;
              break;

            case ActivationExecutionContext.ExecuteAsUser:
              rdUser.Checked = true;
              txtExecuteAs.Text = q.ExecutionContextPrincipal;
              break;
          }
          
          break;

        case SsbEnum.Service:
          BrokerService bserv = (BrokerService)smob;
          bserv.Name = txtName.Text;
          if (cboUser.Text != string.Empty)
            bserv.Owner = cboUser.Text;


          ServiceQueue servq = (ServiceQueue)cboQueue.SelectedItem;
          bserv.QueueName = servq.Name;
          bserv.QueueSchema = servq.Schema;

          if (lbChosenCtr.Items.Count > 0) {
            foreach (object o in lbChosenCtr.Items) {
              ServiceContract servctr = o as ServiceContract;
              ServiceContractMapping scm = new ServiceContractMapping(bserv, servctr.Name);
              bserv.ServiceContractMappings.Add(scm);
            }
          }
          break;

        case SsbEnum.Certificate :
          Certificate cert = (Certificate)smob;
          txtName.Text = cert.Name;
          cboUser.Text = cert.Owner;
          cboUser.Enabled = false;
          cert_chkBeginDlg.Checked = cert.ActiveForServiceBrokerDialog;
          cert_txtCertPath.Text = cert.Subject;
          cert_txtCertPath.Enabled = false;
          

          cert_chkMasterKey.Checked = cert.PrivateKeyEncryptionType == PrivateKeyEncryptionType.MasterKey;
          
          if (cert.PrivateKeyEncryptionType != PrivateKeyEncryptionType.NoKey) {
            if (cert.Subject == cert.Issuer)
              cert_cboSource.SelectedIndex = 1;
            else
              cert_cboSource.SelectedIndex = 0;

          }
          else
            cert_cboSource.SelectedIndex = 2;

          cert_cboSource.Enabled = false;
          btnNewUser.Visible = false;

          cert_dtValidFrom.Value = cert.StartDate;
          cert_dtExpiry.Value = cert.ExpirationDate;

          cert_dtValidFrom.Enabled = false;
          cert_dtExpiry.Enabled = false;

          cert_grpPvk.Enabled = false;
          cert_chkMasterKey.Enabled = false;

          label64.Visible = true;
          label58.Visible = false;
          break;

        case SsbEnum.EndPoint :
          SSBIEndpoint ep = (SSBIEndpoint)smob;
          txtName.Text = ep.Name;
          cboUser.SelectedItem = ep.EndPoint.Owner;
          ep_cboState.SelectedItem = ep.State;
          ep_txtPort.Text = ep.Port.ToString();
          if (ep.EndPoint.Protocol.Tcp.ListenerIPAddress == System.Net.IPAddress.Any)
            ep_txtIp.Text = "ALL";
          else
            ep_txtIp.Text = ep.EndPoint.Protocol.Tcp.ListenerIPAddress.ToString();

          ep_cboAuth.SelectedItem = ep.Authentication;
          if (ep.Certificate != null)
            ep_cboCert.Text = ep.Certificate;

          ep_cboEncrypt.SelectedItem = ep.Encryption;

          ep_cboAlgorithm.SelectedItem = ep.Algorithm;

          ep_chkFwd.Checked = ep.IsMessageForwardingEnabled;
          ep_txtSize.Text = ep.MessageForwardSize.ToString();

          cboUser.Enabled = false;

          break;
      }

      txtName.Enabled = false;

    }

    private void ExitAndClose() {
      if (isDirty && _state == SsbState.Cancelled) {
        DialogResult res = MessageBox.Show("Do you want to save your work before exiting?", "Save Work", MessageBoxButtons.YesNoCancel);
        if (res == DialogResult.Yes)
          UpDateSsb();
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

    bool ValidateData() {
      bool fail = false;
      StringBuilder errMsg = new StringBuilder();
      errMsg.Append("Following required data is missing:\n");
      if (ssbType != SsbEnum.Conversation && ssbType != SsbEnum.Message && ssbType != SsbEnum.Login && ssbType != SsbEnum.CreateListing) {
        if (txtName.Text == string.Empty) {
          errMsg.Append("  *Name - A name needs to be supplied\n");
          fail = true;
        }
      }

      switch (ssbType) {
        case SsbEnum.Database :
          if (!isEdit) {
            if (db_chkMasterKey.Checked && db_txtMkPwd.Text == string.Empty) {
              errMsg.Append("  *Master Key Password - You should supply a password for the Master Key for security reasons.\n");
              fail = true;
            }
          }
          else {
            if (dBase.MasterKey == null && db_chkMasterKey.Checked && db_txtMkPwd.Text == string.Empty) {
              errMsg.Append("  *Master Key Password - You should supply a password for the Master Key for security reasons.\n");
              fail = true;
            }


          }

          if (!db_chkMasterKey.Checked && db_txtMkPwd.Text != string.Empty) {
            errMsg.Append("  *Master Key - It seems you have supplied a password for the master key, but not checked the check box for the Master Key.\n");
            fail = true;
          }

          break;

        case SsbEnum.MessageType:
          if (cboValSchema.Enabled && cboValSchema.Text == string.Empty) {
            errMsg.Append("  *Validation Schema - Choose what XML Schema collection to use for validation\n");
            fail = true;
          }
          break;
        case SsbEnum.Contract:
          if (dvMsgTypes.Rows.Count == 0) {
            errMsg.Append("  *Message Types - The contract needs to have Message Types\n");
            fail = true;
          }
          foreach (DataGridViewRow row in dvMsgTypes.Rows) {
            if (row.Cells[1].Value == null) {
              string name = row.Cells[0].Value.ToString();
              string err = string.Format("  *Sent By - Sent By needs to be chosen for Message Type: {0}\n", name);
              errMsg.Append(err);
              fail = true;

            }
          }

          break;
        case SsbEnum.Queu:
          if (chkActivation.Checked) {
            short maxReaders;
            if (cboProc.Text == string.Empty) {
              errMsg.Append("  *Procedure - Choose a stored procedure to act as an Activation procedure\n");
              fail = true;
            }
            if (txtReaders.Text == string.Empty) {
              errMsg.Append("  *Max Readers - You need to supply the maximum number of Queue Readers\n");
              fail = true;
            }
            if (!short.TryParse(txtReaders.Text, out maxReaders)) {
              errMsg.Append("  *Max Readers - Not a number (or not between 0 and 32767)\n");
              fail = true;
            }

            if (rdUser.Checked && txtExecuteAs.Text == string.Empty) {
              errMsg.Append("  *Execute As - You need to supply a user name for Execute As\n");
              fail = true;
            }
          }
          break;

        case SsbEnum.Service:
          if (cboQueue.Text == string.Empty) {
            errMsg.Append("  *Queue - A Service requires a Queue\n");
            fail = true;
          }

          break;

        case SsbEnum.Route:
          DateTime lifeTime;

          if (textLifeTime.Text != string.Empty) {
            if (!DateTime.TryParse(textLifeTime.Text, out lifeTime)) {
              errMsg.Append("  *LifeTime - LifeTime is not a date\n");
              fail = true;
            }
            else if (lifeTime < DateTime.Now) {
              errMsg.Append("  *LifeTime - LifeTime has to be greater than now\n");
              fail = true;
            }

          }

          if (rdTransport.Checked && textRemote.Text == string.Empty) {
            errMsg.Append("  *Remote Service - A Route with 'TRANSPORT' as address requires a Remote Service Name\n");
            fail = true;
          }

          if (rdTCP.Checked && txtAddress.Text == string.Empty) {
            errMsg.Append("  *Address - You need to define an address for the route\n");
            fail = true;
          }

          break;

        case SsbEnum.RemoteBinding:
          if (textRemServ.Text == string.Empty) {
            errMsg.Append("  *Remote Service - You need to define a remote service for the binding\n");
            fail = true;
          }

          if (cboRemUser.Text == string.Empty) {
            errMsg.Append("  *User - A user needs to be defined\n");
            fail = true;

          }

          break;



        case SsbEnum.Conversation:
          if (cnv_cboFrmServ.Text == string.Empty) {
            errMsg.Append("  *From Service - A service to send from needs to be chosen\n");
            fail = true;

          }

          if (cnv_txtToSrv.Text == string.Empty) {
            errMsg.Append("  *To Service - A service to send to needs to be chosen\n");
            fail = true;

          }

          break;

        case SsbEnum.Message:
              
          
          if (msg_cboMsgType.Text == string.Empty) {
            errMsg.Append("  *Message Type - A Message Type needs to be chosen\n");
            fail = true;

          }
          
          break;
        case SsbEnum.Login :
          if (lgn_txtLoginName.Text == string.Empty) {
            errMsg.Append("  *Login Name - A Login Name needs to be entered\n");
            fail = true;
          }
          if (lgn_rdSql.Checked) {
            if (lgn_txtPwd.Text != lgn_txtConfirm.Text) {
              errMsg.Append("  *Password - The entered Password does not match Confirm Password\n");
              fail = true;
            }
          }
          break;

        case SsbEnum.Certificate:
          string missingCert = "  *Certificate File Path - The path to the certificate file is missing\n";
          string missingSubject = "  *Subject - A self signed certificate needs a subject\n";
          //check what type of certificate
          int certSource = cert_cboSource.SelectedIndex;
          if (certSource == 0 || certSource == 2) { //pvk cert, public key cert
            if (cert_txtCertPath.Text == string.Empty) {
              errMsg.Append(missingCert);
              fail = true;
            }

            if (certSource == 0 && cert_txtPrivPath.Text == string.Empty) {
              errMsg.Append("  *Private Key File Path - The path for the private key file is missing\n");
              fail = true;
            }

            if (certSource == 0 && cert_txtDecrypt.Text == string.Empty) {
              errMsg.Append("  *Decryption Password - The password for decryption of the private key needs to be supplied\n");
              fail = true;
            }

            
          }
          else if (certSource == 1) {//publik key cert
            if (cert_txtCertPath.Text == string.Empty) {
              errMsg.Append(missingSubject);
              fail = true;
            }

            if(cert_dtExpiry.Value < cert_dtValidFrom.Value) {
              errMsg.Append("  *Valid To - The expiry date of the certificate can not be less than the Valid From date\n");
              fail = true;
            }


          }

          if (certSource < 2) {
            if (!cert_chkMasterKey.Checked && cert_txtEncrypt.Text == string.Empty) {
              errMsg.Append("  *Encryption Password - A password to encrypt the key with needs to be supplied\n");
              fail = true;
            }


            if (!isEdit && cert_chkMasterKey.Checked) {
              if (dBase.MasterKey == null) {
                string noMasterKey = "  *Encryption by Master Key - The database does not have a Master Key. You need to change the encryption type or edit the database and create a Master Mey"; 
                DialogResult dlgRes = MessageBox.Show("The database does not have a Master Key. This is needed in order to encrypt the certificate.\nDo you want to create a Master Key?", "Master Key needed", MessageBoxButtons.YesNo);
                if (dlgRes == DialogResult.Yes) {
                  if ((dlgRes = LoadForm2(SsbEnum.Database, true, dBase)) == DialogResult.Cancel) {
                    errMsg.Append(noMasterKey);
                    fail = true;
                  }
                  else {
                    errMsg.Append(noMasterKey);
                    fail = true;
                  }
                }
              }
            }
          }
         break;

        case SsbEnum.User :
          if (!isEdit) {
            if (usr_cboLogin.Text == string.Empty && usr_chkLogin.Checked == true) {
              errMsg.Append("  *Login Name - A Login needs to be selected\n");
              fail = true;
            }
          }

          break;

        case SsbEnum.EndPoint :
          int ip;
          if (!isEdit) {
            if (ep_txtPort.Text == string.Empty) {
              errMsg.Append("  *Listener Port - A port number needs to be entered. Default is 4022.\n");
              ep_txtPort.Text = "4022";
              fail = true;
            }
            else {
              if(!int.TryParse(ep_txtPort.Text, out ip)) {
                errMsg.Append("  *Listener Port - Port number needs to be numeric. Default is 4022.\n");
                ep_txtPort.Text = "4022";
                fail = true;
              }
            }
            if (ep_txtIp.Text == string.Empty) {
              errMsg.Append("  *Listener IP - Listener IP address is required. Default is 'ALL'.\n");
              ep_txtIp.Text = "ALL";
              fail = true;
            }

            else {
              if (ep_txtIp.Text != "ALL" && !int.TryParse(ep_txtIp.Text, out ip)) {
                errMsg.Append("  *Listener IP - Listener IP needs to be numeric (or 'ALL'). Default is 'ALL'.\n");
                ep_txtIp.Text = "ALL";
                fail = true;
              }
            }
            
            if (ep_cboAuth.Text.Contains("Certificate") && ep_cboCert.SelectedIndex == -1) {
              errMsg.Append("  *Certificate - A certificate needs to be selected.\n");
              fail = true;
            }
            if (!ep_cboEncrypt.Text.Contains("Disabled") && ep_cboAlgorithm.SelectedIndex == -1) {
              errMsg.Append("  *Algorithm - An encryption algorithm needs to be selected.\n");
              fail = true;
            }

            if (ep_chkFwd.Checked && ep_txtSize.Text == string.Empty) {
              errMsg.Append("  *Message Size - Storage size for messages to be forwarded is required.\n");
              fail = true;
            }

            else if (ep_chkFwd.Checked && !int.TryParse(ep_txtSize.Text, out ip)) {
              errMsg.Append("  *Message Size - Storage size needs to be numeric.\n");
              fail = true;
            }
 
          }

          break;

        case SsbEnum.CreateListing :
          if (cnv_cboFrmServ.Text == "") {
            errMsg.Append("  *Service - A Service need to be chosen.\n");
            fail = true;
          }
          break;
      }

      if(fail)
        MessageBox.Show(errMsg.ToString(),"Missing or invalid data encountered");
      return fail;


    }


    void SetUpTabs(SsbEnum idx) {
      foreach(TabPage tp in tabControl1.TabPages)
        this.tabControl1.Controls.Remove(tp);

      
      switch (idx) {
        case SsbEnum.Login:
          this.tabControl1.Controls.Add(tabPage10);
          break;
        case SsbEnum.EndPoint:
          this.tabControl1.Controls.Add(tabPage11);
          break;
        case SsbEnum.Database:
          this.tabControl1.Controls.Add(tabPage9);
          break;
        case SsbEnum.MessageType:
          this.tabControl1.Controls.Add(tabPage1);
          break;
        case SsbEnum.Contract:
          this.tabControl1.Controls.Add(tabPage2);
          break;
        case SsbEnum.Queu:
          this.tabControl1.Controls.Add(tabPage3);
          break;
        case SsbEnum.Service:
          this.tabControl1.Controls.Add(tabPage4);
          break;
        case SsbEnum.Route:
          this.tabControl1.Controls.Add(tabPage5);
          break;
        case SsbEnum.RemoteBinding:
          this.tabControl1.Controls.Add(tabPage6);
          break;
        case SsbEnum.Conversation:
          this.tabControl1.Controls.Add(tabPage7);
          break;
        case SsbEnum.Message:
          this.tabControl1.Controls.Add(tabPage8);
          break;
        case SsbEnum.Certificate:
          this.tabControl1.Controls.Add(tabPage12);
          break;
        case SsbEnum.User:
          this.tabControl1.Controls.Add(tabPage13);
          break;
        case SsbEnum.CreateListing:
          this.tabControl1.Controls.Add(tabPage14);
          break;
        case SsbEnum.ImportListing:
          this.tabControl1.Controls.Add(tabPage15);
          break;
      }

      tabControl1.Visible = true;
      

    }

   
    void LoadDataBases(Server srv) {
      if (ssbType == SsbEnum.Queu) {
        cboQDb.Items.Clear();
        cboQDb.DisplayMember = "Name";
        foreach (Database db in srv.Databases) {
          cboQDb.Items.Add(db);
        }

      }


    }

    private DialogResult LoadForm2(SsbEnum _ssbEnum, bool doEdit, object objToEdit) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      Form2 f2 = new Form2(_ssbEnum, dbServ, dBase, doEdit, objToEdit);
      f2.Processed += new SsbEventDel(f2_Processed);
      DialogResult res = f2.ShowDialog();
      Cursor.Current = crs;
      return res;
    }

    private DialogResult LoadForm2(SsbEnum _ssbEnum) {
      return LoadForm2(_ssbEnum, false, null);
    }



    private void LoadForm3(SsbEnum sType, Control ctrlToUpdate) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      Form3 f3 = new Form3(sType, true, ctrlToUpdate);
      f3.ShowDialog();
      Cursor.Current = crs;
    }
    #endregion

    #region event-methods
    private void cboVal_SelectedIndexChanged(object sender, EventArgs e) {
      cboValSchema.Enabled = false;
      isDirty = true;
      if (cboVal.SelectedIndex == (int)MessageTypeValidation.XmlSchemaCollection)
        cboValSchema.Enabled = true;

    }

    private void button2_Click(object sender, EventArgs e) {
      UpDateSsb();
    }

    
    private void cboQueue_SelectedIndexChanged(object sender, EventArgs e) {
      isDirty = true;

    }

    
    private void button1_Click(object sender, EventArgs e) {
      string name;
      name = lbMsgTypes.Text;
      if (name == "<new>") {

      }
      else {
        dvMsgTypes.Rows.Add(new object[] { name});
        isDirty = true;
      }
    }

   

    //when procedure databases changes on the queue tab reload procs
    private void cboQDb_SelectedIndexChanged(object sender, EventArgs e) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      cboProc.Items.Clear();
      cboProc.DisplayMember = "Name";
      cboProc.ValueMember = "Schema";
      foreach (StoredProcedure sp in dBase.StoredProcedures) {
        //if (!sp.IsSystemObject) {
          cboProc.Items.Add(sp);
        //}
      }
      isDirty = true;
      Cursor.Current = crs;

    }

    //enable/disable activation related controls
    private void chkActivation_CheckedChanged(object sender, EventArgs e) {
      bool isChecked = chkActivation.Checked;
      cboQDb.Enabled = isChecked;
      cboProc.Enabled = isChecked;
      txtReaders.Enabled = isChecked;
      grpExecuteAs.Enabled = isChecked;
    }

    private void btnAddCtr_Click(object sender, EventArgs e) {
      lbChosenCtr.DisplayMember = "Name";
      ServiceContract sc = (ServiceContract)lbCtr.SelectedItem;
      lbChosenCtr.Items.Add(sc);
      isDirty = true;
    }

    private void btnRemove_Click(object sender, EventArgs e) {
      if (lbChosenCtr.SelectedIndex != -1)
        lbChosenCtr.Items.RemoveAt(lbChosenCtr.SelectedIndex);
    }


    
    private void button3_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.MessageType);
    }


    void f2_Processed(object sender, SsbEventsArgs e) {
      
      if (e.state == SsbState.New && e.ssbType != SsbEnum.User) {
        switch (ssbType) {
          case SsbEnum.Contract :
            MessageType mt = (MessageType)e.updated;
            lbMsgTypes.Items.Add(mt.Name);
            dvMsgTypes.Rows.Add(new object[] { mt.Name });
            break;

          case SsbEnum.Service :
            if (e.ssbType == SsbEnum.Queu) {
              ServiceQueue sq = (ServiceQueue)e.updated;
              cboQueue.Items.Add(sq);
              cboQueue.SelectedItem = sq;

            }
            else if (e.ssbType == SsbEnum.Contract) {
              ServiceContract sc = (ServiceContract)e.updated;
              lbCtr.Items.Add(sc);
              lbChosenCtr.Items.Add(sc);
            }

            break;

          case SsbEnum.User :
            if (e.ssbType == SsbEnum.Login) {
              usr_cboLogin.Items.Add(((Login)e.updated).Name);
              usr_cboLogin.Text = ((Login)e.updated).Name;

            }

            if (e.ssbType == SsbEnum.Certificate) {
              usr_cboCerts.Items.Add(((Certificate)e.updated).Name);
              usr_cboCerts.Text = ((Certificate)e.updated).Name;

            }

            break;
          case SsbEnum.EndPoint :
            if (e.ssbType == SsbEnum.Certificate) {
              ep_cboCert.Items.Add(((Certificate)e.updated).Name);
              ep_cboCert.Text = ((Certificate)e.updated).Name;

            }

            break;


        }

      }

        else if (e.state == SsbState.New && e.ssbType == SsbEnum.User) {
          cboUser.Items.Add((User)e.updated);
          cboUser.SelectedItem = (User)e.updated;
        }

      
    
    }

    private void Form2_Closing(object sender, FormClosingEventArgs e) {
      string dbName = null;
      if (Processed != null) {
        if (dBase != null)
          dbName = dBase.Name;
        SsbEventsArgs evts = new SsbEventsArgs(dbServ.Name, dbName, ssbType, _state, updatedobj);
        Processed(this, evts);
      }

    }

    private void btnNewQ_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.Queu);
    }

    private void btnNewCtr_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.Contract);

    }

    
    void mnuSubNewDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
      int idx = (int)e.ClickedItem.Tag;
      LoadForm2((SsbEnum)idx);

    }


    void mnuTopFile_Click(object sender, EventArgs e) {
      mnuSubNewDropDown.Enabled = false;
      mnuSubSave.Enabled = false;
      if (isDirty)
        mnuSubSave.Enabled = true;

      if (ssbType == SsbEnum.Contract || ssbType == SsbEnum.Service) {
        mnuSubNewDropDown.Enabled = true;

      }
      //ToolStripItem itm = e.ClickedItem;
    }

    void mnuSubNewDropDown_DropDownOpening(object sender, EventArgs e) {
      mnuSubContract.Enabled = false;
      mnuSubMsgType.Enabled = false;
      mnuSubQueue.Enabled = false;
      if (ssbType == SsbEnum.Contract)
        mnuSubMsgType.Enabled = true;
      else if (ssbType == SsbEnum.Service) {
        mnuSubContract.Enabled = true;
        mnuSubQueue.Enabled = true;

      }

    }

    void mnuSubExit_Click(object sender, EventArgs e) {
      ExitAndClose();

    }

    void mnuSubSave_Click(object sender, EventArgs e) {
      UpDateSsb();

    }


    private void txtName_TextChanged(object sender, EventArgs e) {
      isDirty = true;
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      ExitAndClose();
    }

    void rdUser_CheckedChanged(object sender, EventArgs e) {
      txtExecuteAs.Enabled = rdUser.Checked;
    }

    void db_chkMasterKey_CheckedChanged(object sender, EventArgs e) {
      if (isEdit) {
        isDirty = ((dBase.MasterKey == null && db_chkMasterKey.Checked) || (dBase.MasterKey != null && !db_chkMasterKey.Checked));
      }
    }

    void cnv_cboFrmServ_SelectedIndexChanged(object sender, EventArgs e) {
      isDirty = true;
      listingDirty = true;
      if (ssbType != SsbEnum.CreateListing) {
        cnv_cboContract.Text = "";
        cnv_cboContract.Items.Clear();
        BrokerService bServ = (BrokerService)cnv_cboFrmServ.SelectedItem;
        ServiceContractMappingCollection scc = bServ.ServiceContractMappings;
        cnv_cboContract.DisplayMember = "Name";
        cnv_cboContract.ValueMember = "Name";
        foreach (ServiceContractMapping scm in scc) {
          cnv_cboContract.Items.Add(scm);
        }
      }
      else if (ssbType == SsbEnum.CreateListing && cnv_cboFrmServ.Text != "") {
        svcL_PreviewListing.Enabled = true;
      }
      else if (ssbType == SsbEnum.CreateListing && cnv_cboFrmServ.Text == "") {
        svcL_PreviewListing.Enabled = false;
      }
       
      
    
    }

    void cnv_btnFind_Click(object sender, EventArgs e) {
      LoadForm3(SsbEnum.Service, cnv_txtToSrv); 
         
    }

    void rt_btnFind_Click(object sender, EventArgs e) {
      LoadForm3(SsbEnum.Service, textRemote);
    }

    void rem_btnFind_Click(object sender, EventArgs e) {
      LoadForm3(SsbEnum.Service, textRemServ);
    }

    void rdTCP_CheckedChanged(object sender, EventArgs e) {
      if (!rdTCP.Checked) {
        txtAddress.Text = "";
        txtMirror.Text = "";
      }
      txtAddress.Enabled = rdTCP.Checked;
      txtMirror.Enabled = rdTCP.Checked;


    }

    void msg_cboMsgType_SelectedIndexChanged(object sender, EventArgs e) {
      isDirty = true;
      MessageTypeMapping mtm = (MessageTypeMapping)msg_cboMsgType.SelectedItem;
      MessageType mt = (MessageType)smo.GetObject(dBase, mtm.Name, SsbEnum.MessageType);
      msg_txtVal.Text = "";
      msg_txtVal.Text = mt.MessageTypeValidation.ToString();
    
    }

    void msg_cboConv_SelectedIndexChanged(object sender, EventArgs e) {
      SSBIConversation conv = (SSBIConversation)msg_cboConv.SelectedItem;
      ServiceContract msg_sc = (ServiceContract)smo.GetObject(dBase, conv.Contract, SsbEnum.Contract);
      MessageTypeMappingCollection msg_mtmColl = msg_sc.MessageTypeMappings;
      msg_cboMsgType.Items.Clear();
      msg_cboMsgType.DisplayMember = "Name";
      foreach (MessageTypeMapping msg_mtm in msg_mtmColl) {
        msg_cboMsgType.Items.Add(msg_mtm);
      }
    }

    void lgn_rdWin_CheckedChanged(object sender, EventArgs e) {
      lgn_grpSql.Enabled = lgn_rdSql.Checked;
      lgn_chkEnforcePolicy .Checked = true;
      lgn_txtPwd.Text = "";
      lgn_txtConfirm.Text = "";
      
    }

    void cert_chkMasterKey_CheckedChanged(object sender, EventArgs e) {
      cert_txtEncrypt.Enabled = !cert_chkMasterKey.Checked;
    }

    void cert_cboSource_SelectedIndexChanged(object sender, EventArgs e) {
      cert_grpPvk.Enabled = cert_cboSource.SelectedIndex == 0;
      label64.Visible = cert_cboSource.SelectedIndex == 1;
      label58.Visible = cert_cboSource.SelectedIndex != 1;
      cert_btnBrowseCert.Visible = cert_cboSource.SelectedIndex != 1;
      //cert_btnBrowsePvk.Visible = cert_cboSource.SelectedIndex == 0;
      cert_dtValidFrom.Enabled = cert_cboSource.SelectedIndex == 1;
      cert_dtExpiry.Enabled = cert_cboSource.SelectedIndex == 1;
      cert_chkMasterKey.Enabled = cert_cboSource.SelectedIndex != 2;
      cert_txtEncrypt.Enabled = !cert_chkMasterKey.Checked && cert_chkMasterKey.Enabled;
      
    }

    void cert_Browse_Click(object sender, EventArgs e) {
      string fileFilter = "Certificate files (*.cer)|*.cer|All files (*.*)|*.*";
      string fileName;
      bool isPvk = false;
      if (((Button)sender).Name == "cert_btnBrowsePvk") {
        fileFilter = "Private Key files (*.pvk)|*.pvk|All files (*.*)|*.*";
        isPvk = true;
      }
      if (DoDialog(true, fileFilter, "c:\\", out fileName)) {

        if (!isPvk)
          cert_txtCertPath.Text = fileName;
        else
          cert_txtPrivPath.Text = fileName;
      }

    }

    
    private bool DoDialog(bool openFile, string fileFilter, string initDir, out string fileName) {
      bool ret = false;
      FileDialog fDlg = null;
      if (openFile)
        fDlg = new OpenFileDialog();
      else
        fDlg = new SaveFileDialog();
      fileName = "";

      fDlg.InitialDirectory = initDir;
      fDlg.Filter = fileFilter;
      fDlg.FilterIndex = 1;
      fDlg.RestoreDirectory = true;
      if (fDlg.ShowDialog() == DialogResult.OK) {
        ret = true;
        fileName = fDlg.FileName;
      }
      return ret;
    }

    void svcL_btnView_Click(object sender, EventArgs e) {
      
    
    }

    void svcL_btnBrowse_Click(object sender, EventArgs e) {
      string fileFilter = "XML file (*.xml)|*.xml|All files (*.*)|*.*";
        string fileName;
        if (DoDialog(true, fileFilter, "c:\\", out fileName)) {
          txtName.Text = fileName;
          svcL_btnView.Enabled = true;
        }
    }

    void usr_btnNewLogin_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.Login);  
    }

    void usr_btnNewCert_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.Certificate);  
    
    }

    void usr_chkLogin_CheckedChanged(object sender, EventArgs e) {
      usr_pnlLogin.Enabled = usr_chkLogin.Checked;  
    }

    void btnNewUser_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.User);  
    }

    void ep_chkFwd_CheckedChanged(object sender, EventArgs e) {
      ep_pnlMsgFwd.Enabled = ep_chkFwd.Checked;
    }

    
    void ep_btnNewCert_Click(object sender, EventArgs e) {
      LoadForm2(SsbEnum.Certificate);
    }

    void ep_cboEncrypt_SelectedIndexChanged(object sender, EventArgs e) {
      ep_pnlAlgo.Enabled = !ep_cboEncrypt.Text.Contains("Disabled");
    }

    void ep_cboAuth_SelectedIndexChanged(object sender, EventArgs e) {
      ep_pnlCert.Enabled = ep_cboAuth.Text.Contains("Certificate");
    }

    private void svcL_PreviewListing_Click(object sender, EventArgs e) {
      SqlXml listing;
      if (!ValidateData()) {
        try {
          listing = CreateListing(false);
          svcL_grpListing.Enabled = true;
          XmlReader r = listing.CreateReader();
          r.Read();
          svcL_rchListing.Text = r.ReadOuterXml();
          r.Close();

        }
        catch {
          throw;
        }
      }
    }

    
    void TextChanged2(object sender, EventArgs e) {
      isDirty = true;
    }


    #endregion

    public event SsbEventDel Processed;

    

   

    

    
  }
}