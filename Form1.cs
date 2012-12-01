#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Broker;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading;

#endregion

namespace SsbAdmin {

  internal delegate void ServersLoadedDelegate();
  partial class Form1 : Form {

    
    event ServersLoadedDelegate ServersLoaded;


    SsbEnum ssbType = SsbEnum.None;
    //string dbName = null;
    //string svrName = null;
    SSBIServer dbServ = null;
    Database dBase = null;
    int level;
    object smoObj = null;

    CollectionBase convCollB = null;

    TreeNode delNode;
    
    bool expandError = false;

    ICollection servColl = null;

    bool serversLoaded = false;
    

    ToolStripMenuItem mnuTopFile;
    ToolStripMenuItem mnuTopView;
    ToolStripMenuItem mnuTopEdit;
    ToolStripMenuItem mnuTopHelp;
    ToolStripMenuItem mnuSubNewDropDown;
    ToolStripMenuItem mnuFileNewDropDown;
    ToolStripMenuItem mnuSubNew;
    ToolStripMenuItem mnuSubExit;
    ToolStripSeparator mnuSubSep;
    ToolStripSeparator mnuSubSep2;
    ToolStripMenuItem mnuSubRefresh;
    ToolStripMenuItem mnuSubCopy;
    ToolStripMenuItem mnuSubDelete;
    ToolStripMenuItem mnuSubAlter;
    ToolStripMenuItem mnuSubMsgType;
    ToolStripMenuItem mnuSubContract;
    ToolStripMenuItem mnuSubQueue;
    ToolStripMenuItem mnuSubService;
    ToolStripMenuItem mnuSubRoute;
    ToolStripMenuItem mnuSubRemoteBinding;
    ToolStripMenuItem mnuSubConversation;
    ToolStripMenuItem mnuSubSendMsg;
    ToolStripMenuItem mnuSubSendMsgFromService;
    ToolStripMenuItem mnuSubDataBase;
    ToolStripMenuItem mnuSubNewConversation;
    ToolStripMenuItem mnuSubEndConversationFromService;
    ToolStripMenuItem mnuSubViewMsg;
    ToolStripMenuItem mnuSubOpenConn;
    ToolStripMenuItem mnuSubAbout;
    ToolStripMenuItem mnuSubLogin;
    ToolStripMenuItem mnuSubEndpoint;
    ToolStripMenuItem mnuSubCertificate;
    ToolStripMenuItem mnuSubUser;
    ToolStripMenuItem mnuSubEpCertUsrAlter;
    ToolStripMenuItem mnuSubEpCertUsrDrop;
    ToolStripMenuItem mnuSubCreateServiceListing;
    ToolStripMenuItem mnuSubImportServiceListing;
    ToolStripMenuItem mnuSubSecureService;

        
    ContextMenuStrip mnuCtx;

    public Form1() {
      //sp = new splash();
      //sp.Show();
      InitializeComponent();
      LoadServers();
      SetupMenus();
      SetUpEventHooks();
      tabControl1.Visible = false;
      SetUpGrids();
      
    }

      

    #region event-hooks

    void SetUpEventHooks() {

      #region TreeView tv1
      
      this.tv1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv1_BeforeExpand);
      this.tv1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv1_AfterSelect);
      this.tv1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv1_NodeMouseClick);
      tv1.AfterCollapse += new TreeViewEventHandler(tv1_AfterCollapse);
      #endregion

      #region menus
      mnuTopFile.DropDownItemClicked += new ToolStripItemClickedEventHandler(mnuTopFile_DropDownItemClicked);
      mnuTopFile.DropDownOpening += new EventHandler(mnuTopFile_DropDownOpening);

      mnuTopEdit.DropDownOpening += new EventHandler(mnuTopEdit_DropDownOpening);
      mnuTopView.DropDownOpening += new EventHandler(mnuTopView_DropDownOpening);

      mnuFileNewDropDown.DropDownOpening += new EventHandler(mnuFileNewDropDown_DropDownOpening);
      mnuFileNewDropDown.DropDownItemClicked += new ToolStripItemClickedEventHandler(mnuFileNewDropDown_DropDownItemClicked);
      mnuSubNewDropDown.DropDownItemClicked += new ToolStripItemClickedEventHandler(mnuSubNewDropDown_DropDownItemClicked);
      mnuSubNewDropDown.DropDownOpening += new EventHandler(mnuSubNewDropDown_DropDownOpening);
      
      mnuCtx.ItemClicked += new ToolStripItemClickedEventHandler(mnuCtx_ItemClicked);

      mnuSubAlter.Click +=new EventHandler(mnuSubAlter_Click);

      mnuSubCopy.Click += new EventHandler(mnuSubCopy_Click);

      mnuSubDelete.Click += new EventHandler(mnuSubDelete_Click);

      mnuSubRefresh.Click += new EventHandler(mnuSubRefresh_Click);
      mnuSubSendMsg.Click += new EventHandler(mnuSubSendMsg_Click);
      mnuSubNewConversation.Click += new EventHandler(mnuSubNewConversation_Click);
      mnuSubSendMsgFromService.Click += new EventHandler(mnuSubSendMsgFromService_Click);
      
      mnuSubEndConversationFromService.Click +=new EventHandler(mnuSubEndConversationFromService_Click);

      mnuSubOpenConn.Click += new EventHandler(mnuSubOpenConn_Click);
      mnuSubViewMsg.Click += new EventHandler(mnuSubViewMsg_Click);

      mnuSubAbout.Click += new EventHandler(mnuSubAbout_Click);
      //mnuSubLogin.Click += new EventHandler(mnuSubLogin_Click);

      //mnuSubEndpoint.Click += new EventHandler(mnuSubEndpoint_Click);
      mnuSubEpCertUsrAlter.Click += new EventHandler(mnuSubEpCertUsrAlter_Click);
      mnuSubEpCertUsrDrop.Click += new EventHandler(mnuSubEpCertUsrDrop_Click);

      mnuSubCreateServiceListing.Click += new EventHandler(mnuSubCreateServiceListing_Click);
      mnuSubImportServiceListing.Click += new EventHandler(mnuSubImportServiceListing_Click);
      mnuSubSecureService.Click += new EventHandler(mnuSubSecureService_Click);
#endregion

      dvgMsg.CellDoubleClick += new DataGridViewCellEventHandler(this.dvgMsg_CellDoubleClick);
      dvgConv.CellClick += new DataGridViewCellEventHandler(dvgConv_CellClick);
      dvgConv.RowEnter += new DataGridViewCellEventHandler(dvgConv_RowEnter);
      dvgConv.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(dvgConv_RowContextMenuStripNeeded);
      dvgCnvMsgs.CellDoubleClick += new DataGridViewCellEventHandler(dvgCnvMsgs_CellDoubleClick);
      q_dvgMsg.CellDoubleClick += new DataGridViewCellEventHandler(q_dvgMsg_CellDoubleClick);
      q_dvgMsg.RowContextMenuStripNeeded +=new DataGridViewRowContextMenuStripNeededEventHandler(msgGrids_RowContextMenuStripNeeded);
      dvgMsg.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(msgGrids_RowContextMenuStripNeeded);
      dvgCnvMsgs.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(msgGrids_RowContextMenuStripNeeded);

      q_dgvStatus.CellDoubleClick += new DataGridViewCellEventHandler(q_dvgMsg_CellDoubleClick);
      q_dgvStatus.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(msgGrids_RowContextMenuStripNeeded);

      
      srv_cboCnvState.SelectedIndexChanged += new EventHandler(srv_cboCnvState_SelectedIndexChanged);
      srv_cboSource.SelectedIndexChanged += new EventHandler(srv_cboCnvState_SelectedIndexChanged);
      serv_dvgEp.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(endPCertsGrids_RowContextMenuStripNeeded);
      db_dgvCerts.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(endPCertsGrids_RowContextMenuStripNeeded); 
      
      
      //db_lbUsers.MouseClick += new MouseEventHandler(db_lbUsers_MouseClick);
     
      
    
    }

    #endregion


    void SetupMenus() {
      mnuTopFile = new ToolStripMenuItem();
      mnuTopFile.Name = "mnuTopFile";
      mnuTopFile.Text = "&File";
      
      mnuTopEdit = new ToolStripMenuItem();
      mnuTopEdit.Name = "mnuTopEdit";
      mnuTopEdit.Text = "&Edit";
      
      mnuTopView = new ToolStripMenuItem();
      mnuTopView.Name = "mnuTopView";
      mnuTopView.Text = "&View";

      mnuTopHelp = new ToolStripMenuItem();
      mnuTopHelp.Name = "mnuTopHelp";
      mnuTopHelp.Text = "&Help";

      mnuFileNewDropDown = new ToolStripMenuItem();
      mnuFileNewDropDown.Name = "mnuFileNewDropDown";
      mnuFileNewDropDown.Text = "New";
      
      mnuSubNewDropDown = new ToolStripMenuItem();
      mnuSubNewDropDown.Name = "mnuSubNewDropDown";
      mnuSubNewDropDown.Text = "New";
      
      mnuSubNew = new ToolStripMenuItem();
      mnuSubNew.Name = "mnuSubNew";
      mnuSubNew.Text = "New";
      
      mnuSubExit = new ToolStripMenuItem();
      mnuSubExit.Name = "mnuSubExit";
      mnuSubExit.Text = "E&xit";
      
      mnuSubRefresh = new ToolStripMenuItem();
      mnuSubRefresh.Name = "mnuSubRefresh";
      mnuSubRefresh.Text = "Refresh";
      mnuSubRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.F5 )));
      mnuSubRefresh.Enabled = true;
      
      mnuSubCopy = new ToolStripMenuItem();
      mnuSubCopy.Name = "mnuSubCopy";
      mnuSubCopy.Text = "&Copy/Re-Deploy";
      mnuSubCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
      mnuSubCopy.Enabled = true;
      

      mnuSubDelete = new ToolStripMenuItem();
      mnuSubDelete.Name = "mnuSubDelete";
      mnuSubDelete.Text = "&Delete/Drop";
      mnuSubDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Delete)));
      mnuSubDelete.Enabled = true;
      

      mnuSubMsgType = new ToolStripMenuItem();
      mnuSubMsgType.Name = "mnuSubMsgType";
      mnuSubMsgType.Tag = 0;
      mnuSubMsgType.Text = "Message Type";
      

      mnuSubContract = new ToolStripMenuItem();
      mnuSubContract.Name = "mnuSubContract";
      mnuSubContract.Tag = 1;
      mnuSubContract.Text = "Contract";
      

      mnuSubQueue = new ToolStripMenuItem();
      mnuSubQueue.Name = "mnuSubQueue";
      mnuSubQueue.Tag = 2;
      mnuSubQueue.Text = "Queue";
      

      mnuSubService = new ToolStripMenuItem();
      mnuSubService.Name = "mnuSubService";
      mnuSubService.Tag = 3;
      mnuSubService.Text = "Service";
      
      mnuSubRoute = new ToolStripMenuItem();
      mnuSubRoute.Name = "mnuSubRoute";
      mnuSubRoute.Tag = 4;
      mnuSubRoute.Text = "Route";
      
      mnuSubRemoteBinding = new ToolStripMenuItem();
      mnuSubRemoteBinding.Name = "mnuSubRemoteBinding";
      mnuSubRemoteBinding.Tag = 5;
      mnuSubRemoteBinding.Text = "Remote Service Binding";
      
      mnuSubAlter = new ToolStripMenuItem();
      mnuSubAlter.Name = "mnuSubAlter";
      mnuSubAlter.Tag = 7;
      mnuSubAlter.Text = "Edit/Alter";

      mnuSubConversation = new ToolStripMenuItem();
      mnuSubConversation.Name = "mnuSubConversation";
      mnuSubConversation.Tag = 6;
      mnuSubConversation.Text = "Conversation";

      mnuSubNewConversation = new ToolStripMenuItem();
      mnuSubNewConversation.Name = "mnuSubNewConversation";
      mnuSubNewConversation.Tag = 8;
      mnuSubNewConversation.Text = "New Conversation";
      
      mnuSubSendMsg = new ToolStripMenuItem();
      mnuSubSendMsg.Name = "mnuSubSendMsg";
      mnuSubSendMsg.Tag = 9;
      mnuSubSendMsg.Text = "Send Message";

      
      mnuSubDataBase = new ToolStripMenuItem();
      mnuSubDataBase.Name = "mnuSubDataBase";
      mnuSubDataBase.Tag = 10;
      mnuSubDataBase.Text = "Database";

      mnuSubSendMsgFromService = new ToolStripMenuItem();
      mnuSubSendMsgFromService.Name = "mnuSubSendMsgFromService";
      mnuSubSendMsgFromService.Tag = 20;
      mnuSubSendMsgFromService.Text = "Send Message";
      
      mnuSubEndConversationFromService = new ToolStripMenuItem();
      mnuSubEndConversationFromService.Name = "mnuSubEndConversationFromService";
      mnuSubEndConversationFromService.Tag = 30;
      mnuSubEndConversationFromService.Text = "End Conversation";

      //mnuSubViewMsg
      mnuSubViewMsg = new ToolStripMenuItem();
      mnuSubViewMsg.Name = "mnuSubViewMsg";
      mnuSubViewMsg.Tag = 40;
      mnuSubViewMsg.Text = "View Message";

      
      mnuSubOpenConn = new ToolStripMenuItem();
      mnuSubOpenConn.Name = "mnuSubOpenConn";
      mnuSubOpenConn.Tag = 60;
      mnuSubOpenConn.Text = "Connect";

      mnuSubAbout = new ToolStripMenuItem();
      mnuSubAbout.Name = "mnuSubAbout";
      mnuSubAbout.Tag = 80;
      mnuSubAbout.Text = "&About SSB Admin";

      mnuSubLogin = new ToolStripMenuItem();
      mnuSubLogin.Name = "mnuSubLogin";
      mnuSubLogin.Tag = 100;
      mnuSubLogin.Text = "Login";

      mnuSubEndpoint = new ToolStripMenuItem();
      mnuSubEndpoint.Name = "mnuSubEndpoint";
      mnuSubEndpoint.Tag = 120;
      mnuSubEndpoint.Text = "Endpoint";


      mnuSubCertificate = new ToolStripMenuItem();
      mnuSubCertificate.Name = "mnuSubDataBase";
      mnuSubCertificate.Tag = 140;
      mnuSubCertificate.Text = "Certificate";


      mnuSubUser = new ToolStripMenuItem();
      mnuSubUser.Name = "mnuSubUser";
      mnuSubUser.Tag = 160;
      mnuSubUser.Text = "User";

      mnuSubEpCertUsrAlter = new ToolStripMenuItem();
      mnuSubEpCertUsrAlter.Name = "mnuSubEpCertUsrAlter";
      mnuSubEpCertUsrAlter.Tag = 180;
      mnuSubEpCertUsrAlter.Text = "Edit/Alter";

      mnuSubEpCertUsrDrop = new ToolStripMenuItem();
      mnuSubEpCertUsrDrop.Name = "mnuSubEpCertUsrDrop";
      mnuSubEpCertUsrDrop.Tag = 200;
      mnuSubEpCertUsrDrop.Text = "Delete/Drop";

      mnuSubCreateServiceListing = new ToolStripMenuItem();
      mnuSubCreateServiceListing.Name = "mnuSubCreateServiceListing";
      mnuSubCreateServiceListing.Tag = 300;
      mnuSubCreateServiceListing.Text = "Create Service Listing";

      mnuSubImportServiceListing = new ToolStripMenuItem();
      mnuSubImportServiceListing.Name = "mnuSubImportServiceListing";
      mnuSubImportServiceListing.Tag = 340;
      mnuSubImportServiceListing.Text = "Import Service Listing";

      mnuSubSecureService = new ToolStripMenuItem();
      mnuSubSecureService.Name = "mnuSubSecureService";
      mnuSubSecureService.Tag = 380;
      mnuSubSecureService.Text = "Secure Service ";

      
      
      mnuSubSep = new ToolStripSeparator();
      
      mnuSubSep2 = new ToolStripSeparator();

      mnuFileNewDropDown.DropDownItems.AddRange(new ToolStripItem[] { mnuSubLogin, mnuSubEndpoint, mnuSubDataBase, mnuSubCertificate, mnuSubUser, mnuSubMsgType, mnuSubContract, mnuSubQueue, mnuSubService, mnuSubRoute, mnuSubRemoteBinding, mnuSubConversation });
      mnuTopEdit.DropDownItems.AddRange(new ToolStripItem[] { mnuSubRefresh, mnuSubCopy, mnuSubAlter, mnuSubDelete });
      mnuTopView.DropDownItems.AddRange(new ToolStripItem[] { mnuSubRefresh, mnuSubViewMsg });
      mnuTopHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuSubAbout });
      menuStrip1.Items.AddRange(new ToolStripItem[] { mnuTopFile, mnuTopEdit, mnuTopView, mnuTopHelp });
      
      mnuCtx = new ContextMenuStrip();
      
    }

    void SetupContextMenuAndShow(TreeNodeMouseClickEventArgs e) {
      mnuCtx.Items.Clear();
      mnuSubRefresh.Text = "Refresh";
      //check that we're not deleting this item
        int lvl = e.Node.Level;
        if (lvl < 1) {
          mnuSubRefresh.Text = "Add/Browse Server(s)";
          mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubRefresh });
        }

        else if (lvl == 1) {
          dbServ = (SSBIServer)e.Node.Tag;
          if (dbServ.HasLoggedIn)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubNewDropDown, mnuSubSep, mnuSubRefresh });
          else if (!dbServ.HasLoggedIn && !dbServ.IsTryingToConnect)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubOpenConn });
          else if (!dbServ.HasLoggedIn && dbServ.IsTryingToConnect) {
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubOpenConn });
            mnuSubOpenConn.Enabled = false;
          }
        }


        else if (lvl == 2) {
          
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubNewDropDown, mnuSubSep, mnuSubCreateServiceListing,mnuSubImportServiceListing,mnuSubSep2,mnuSubAlter, mnuSubDelete });

        }
        else if (lvl == 3) {
          if(e.Node.Index == (int)SsbEnum.Service)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubNew, mnuSubSep, mnuSubCreateServiceListing, mnuSubImportServiceListing, mnuSubSecureService, mnuSubSep2,mnuSubRefresh });

          else
          mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubNew, mnuSubSep, mnuSubRefresh });
        }

        else if (lvl == 4) {
          //TODO - can only alter message type at the mom
          mnuSubDelete.Text = "Drop/Delete";
          if (ssbType == SsbEnum.MessageType)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubCopy, mnuSubSep, mnuSubAlter, mnuSubDelete });
          else if (ssbType == SsbEnum.Contract)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubCopy, mnuSubSep, mnuSubDelete });
          else if (ssbType == SsbEnum.Service)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubRefresh, mnuSubSep2, mnuSubCreateServiceListing,mnuSubSecureService ,mnuSubNewConversation, mnuSubSep, mnuSubDelete });
          else if (ssbType == SsbEnum.Queu || ssbType == SsbEnum.Route || ssbType == SsbEnum.RemoteBinding)
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubDelete });
          else if (ssbType == SsbEnum.Conversation) {
            mnuSubDelete.Text = "End Conversation";
            mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubRefresh, mnuSubSendMsg, mnuSubSep, mnuSubDelete });
          }
        }


        if (OkToExpand(e.Node)) {
          foreach (ToolStripItem ti in mnuCtx.Items)
            ti.Enabled = true;
        }
        else {
          foreach (ToolStripItem ti in mnuCtx.Items)
            ti.Enabled = false;

        }
     
     

        tv1.ContextMenuStrip = mnuCtx;
        tv1.ContextMenuStrip.Show(tv1, e.Location);

    }

    void SetUpGrids() {
      
      serv_dvgEp.AutoGenerateColumns = false;

      serv_dvgEp.Columns.Add("name", "Name");
      serv_dvgEp.Columns["name"].DataPropertyName = "Name";

      serv_dvgEp.Columns.Add("port", "Port");
      serv_dvgEp.Columns["port"].DataPropertyName = "Port";
      
      serv_dvgEp.Columns.Add("state", "State");
      serv_dvgEp.Columns["state"].DataPropertyName = "State";
      
      serv_dvgEp.Columns.Add("fwd", "Msg. Fwd. Enabled");
      serv_dvgEp.Columns["fwd"].DataPropertyName = "IsMessageForwardingEnabled";
      
      serv_dvgEp.Columns.Add("size", "Msg. Fwd. Size");
      serv_dvgEp.Columns["size"].DataPropertyName = "MessageForwardSize";
      
      serv_dvgEp.Columns.Add("auth", "Authentication");
      serv_dvgEp.Columns["auth"].DataPropertyName = "Authentication";

      serv_dvgEp.Columns.Add("cert", "Certificate");
      serv_dvgEp.Columns["cert"].DataPropertyName = "Certificate";

      serv_dvgEp.Columns.Add("encrypt", "Encryption");
      serv_dvgEp.Columns["encrypt"].DataPropertyName = "Encryption";

      serv_dvgEp.Columns.Add("algo", "Algorithm");
      serv_dvgEp.Columns["algo"].DataPropertyName = "Algorithm";
      

      dvgConv.AutoGenerateColumns = false;

      dvgConv.Columns.Add("cnvhandle", "Handle");
      dvgConv.Columns["cnvhandle"].DataPropertyName = "Handle";

      //dvgConv.Columns.Add("frmServ", "Initiating Service");
      //dvgConv.Columns["frmServ"].DataPropertyName = "FromService";
      
      dvgConv.Columns.Add("toServ", "Remote Service");
      dvgConv.Columns["toServ"].DataPropertyName = "ToService";
      
      dvgConv.Columns.Add("contract", "Contract");
      dvgConv.Columns["contract"].DataPropertyName = "Contract";
      
      dvgConv.Columns.Add("initiator", "Initiator");
      dvgConv.Columns["initiator"].DataPropertyName = "IsInitiator";
      
      dvgConv.Columns.Add("remGuid", "Remote Broker");
      dvgConv.Columns["remGuid"].DataPropertyName = "FarBrokerGuid";

      dvgConv.Columns.Add("state", "State");
      dvgConv.Columns["state"].DataPropertyName = "State";
      

    

      dvgMsg.AutoGenerateColumns = false;
      
      dvgMsg.Columns.Add("cnvhandle", "Conversation");
      dvgMsg.Columns["cnvhandle"].DataPropertyName = "ConversationHandle";

      dvgMsg.Columns.Add("sendserv", "Sending Service");
      dvgMsg.Columns["sendserv"].DataPropertyName = "RemoteServiceName";

      dvgMsg.Columns.Add("contract", "Contract");
      dvgMsg.Columns["contract"].DataPropertyName = "ContractName";

      dvgMsg.Columns.Add("type", "Message Type");
      dvgMsg.Columns["type"].DataPropertyName = "Type";

      dvgMsg.Columns.Add("sequence", "Sequence Number");
      dvgMsg.Columns["sequence"].DataPropertyName = "SequenceNumber";

      dvgMsg.Columns.Add("validation", "Validation");
      dvgMsg.Columns["validation"].DataPropertyName = "Validation";

      q_dvgMsg.AutoGenerateColumns = false;

      q_dvgMsg.Columns.Add("cnvhandle", "Conversation");
      q_dvgMsg.Columns["cnvhandle"].DataPropertyName = "ConversationHandle";

      q_dvgMsg.Columns.Add("recserv", "Receiving Service");
      q_dvgMsg.Columns["recserv"].DataPropertyName = "ServiceName";

      q_dvgMsg.Columns.Add("sendserv", "Sending Service");
      q_dvgMsg.Columns["sendserv"].DataPropertyName = "RemoteServiceName";

      q_dvgMsg.Columns.Add("contract", "Contract");
      q_dvgMsg.Columns["contract"].DataPropertyName = "ContractName";

      q_dvgMsg.Columns.Add("type", "Message Type");
      q_dvgMsg.Columns["type"].DataPropertyName = "Type";

      q_dvgMsg.Columns.Add("sequence", "Sequence Number");
      q_dvgMsg.Columns["sequence"].DataPropertyName = "SequenceNumber";

      q_dvgMsg.Columns.Add("validation", "Validation");
      q_dvgMsg.Columns["validation"].DataPropertyName = "Validation";

      q_dgvStatus.AutoGenerateColumns = false;

      q_dgvStatus.Columns.Add("cnvhandle", "Conversation");
      q_dgvStatus.Columns["cnvhandle"].DataPropertyName = "ConversationHandle";

      q_dgvStatus.Columns.Add("recserv", "Service");
      q_dgvStatus.Columns["recserv"].DataPropertyName = "ServiceName";

      q_dgvStatus.Columns.Add("sendserv", "Remote Service");
      q_dgvStatus.Columns["sendserv"].DataPropertyName = "RemoteService";

      q_dgvStatus.Columns.Add("contract", "Contract");
      q_dgvStatus.Columns["contract"].DataPropertyName = "Contract";

      q_dgvStatus.Columns.Add("type", "Message Type");
      q_dgvStatus.Columns["type"].DataPropertyName = "MessageType";

      q_dgvStatus.Columns.Add("cnverror", "Conversation Error");
      q_dgvStatus.Columns["cnverror"].DataPropertyName = "IsConversationError";

      q_dgvStatus.Columns.Add("enddlg", "Is EndDialog");
      q_dgvStatus.Columns["enddlg"].DataPropertyName = "IsEndDialog";

      q_dgvStatus.Columns.Add("txstat", "Transmission Status");
      q_dgvStatus.Columns["txstat"].DataPropertyName = "TransmissionStatus";
            
      dvgCnvMsgs.AutoGenerateColumns = false;
      
      dvgCnvMsgs.Columns.Add("cnvhandle", "Conversation");
      dvgCnvMsgs.Columns["cnvhandle"].DataPropertyName = "ConversationHandle";

      dvgCnvMsgs.Columns.Add("type", "Message Type");
      dvgCnvMsgs.Columns["type"].DataPropertyName = "Type";

      dvgCnvMsgs.Columns.Add("sequence", "Sequence Number");
      dvgCnvMsgs.Columns["sequence"].DataPropertyName = "SequenceNumber";

      dvgCnvMsgs.Columns.Add("validation", "Validation");
      dvgCnvMsgs.Columns["validation"].DataPropertyName = "Validation";

      db_dgvCerts.AutoGenerateColumns = false;

      db_dgvCerts.Columns.Add("name", "Name");
      db_dgvCerts.Columns["name"].DataPropertyName = "Name";

      db_dgvCerts.Columns.Add("encrypttype", "Encryption Type");
      db_dgvCerts.Columns["encrypttype"].DataPropertyName = "PrivateKeyEncryptionType";

      db_dgvCerts.Columns.Add("dialog", "Active for Dialog");
      db_dgvCerts.Columns["dialog"].DataPropertyName = "ActiveForServiceBrokerDialog";

      db_dgvCerts.Columns.Add("start", "Start Date");
      db_dgvCerts.Columns["start"].DataPropertyName = "StartDate";

      db_dgvCerts.Columns.Add("end", "End Date");
      db_dgvCerts.Columns["end"].DataPropertyName = "ExpirationDate";
        

    }


    void LoadServers() {
      Cursor.Current = Cursors.WaitCursor;
      toolStripStatusLabel1.Text = "Loading Servers...";
      if (tv1.Nodes.Count > 0)
        tv1.Nodes.Clear();
      serversLoaded = false;
      this.ServersLoaded += new ServersLoadedDelegate(Form1_ServersLoaded);
      GetServersDelegate gd = new GetServersDelegate(smo.GetServers);
      IAsyncResult iar = gd.BeginInvoke(SsbServerLocation.Local, new AsyncCallback(GetServers), gd);


    }

    void Form1_ServersLoaded() {
      TVSetUp.SetUpTreeView(tv1, servColl);
      serversLoaded = true;
      toolStripStatusLabel1.Text = "Ready!";
      this.ServersLoaded -= new ServersLoadedDelegate(Form1_ServersLoaded);
      Cursor.Current = Cursors.Default;
      
      
    }

    void GetServers(IAsyncResult iar) {
           
      if (this.InvokeRequired)
        this.BeginInvoke(new AsyncCallback(GetServers), new object[] { iar });
      else {
        GetServersDelegate gd = (GetServersDelegate)iar.AsyncState;
        servColl = gd.EndInvoke(iar);
        
        ArrayList ar = new ArrayList();
        
        
        if (ServersLoaded != null)
          ServersLoaded();
      }

    }

    

    
    

    //void ConnectCallback(IAsyncResult iar) {
    //  SSBIServer serv = null;
    //  TreeNode tn = null;
    //  Server s = null;
    //  if (this.InvokeRequired) {
    //    this.BeginInvoke(new AsyncCallback(ConnectCallback), new object[] { iar });
    //  }
    //  else {
    //    try {
          
    //     ConnectAsyncDelegate del = (ConnectAsyncDelegate)iar.AsyncState;
    //      //ConnectDelegate del = (ConnectDelegate)iar.AsyncState;
    //      //Server s = del.EndInvoke(ref serv, ref tn, iar);
    //      //Server s = del.EndInvoke(iar);
    //      serv.HasLoggedIn = true;
    //      serv.SMOServer = s;
    //      tn.Tag = serv;
    //      if (tn == tv1.SelectedNode) {
    //        dbServ = serv;
    //        tn.Expand();
    //      }
    //      toolStripStatusLabel1.Text = "Ready!";
    //      tn.ForeColor = Color.Black;
                   

    //    }

    //    catch (FailedOperationException fe) {
    //      string msg = string.Format(fe.InnerException.InnerException.Message);
    //      MessageBox.Show(msg);
    //      tn.ForeColor = Color.Black;
    //      toolStripStatusLabel1.Text = "Ready!";
    //    }

    //    catch (Exception e) {
    //      MessageBox.Show(e.Message);
    //      tn.ForeColor = Color.Black;
    //      toolStripStatusLabel1.Text = "Ready!";
    //    }

    //    finally {
    //      //connNode = null;
          
    //    }


    //  }


    //}



    void ClickSSBObject(TreeNode tn) {
      //object smob;
      bool toRefresh = true;

      if (ssbType == SsbEnum.Server) {
        SSBIServer serv = (SSBIServer)tn.Tag;
        toRefresh = serv.HasLoggedIn;
      }
        
      if(toRefresh)        
        RefreshSSBObject(tn);
      
    }

    void RefreshSSBObject(TreeNode tn) {
      object smob;
      string name = null;
     
        switch (ssbType) {

          case SsbEnum.Database:
            //if(tn.Tag.GetType() == typeof(SSBIDatabase))
            //  name = ((SSBIDatabase)tn.Tag).DataBase.Name;
            //else if (tn.Tag.GetType() == typeof(Database))
            //  name = ((Database)tn.Tag).Name;
            name = TVSetUp.GetDatabaseFromTag(tn).Name;
            break;
          case SsbEnum.MessageType:
            name = ((MessageType)tn.Tag).Name;
            break;
          case SsbEnum.Contract:
            name = ((ServiceContract)tn.Tag).Name;
            break;
          case SsbEnum.Queu:
            name = ((ServiceQueue)tn.Tag).Name;
            break;
          case SsbEnum.Service:
            name = ((BrokerService)tn.Tag).Name;
            break;
          case SsbEnum.Route:
            name = ((ServiceRoute)tn.Tag).Name;
            break;
          case SsbEnum.RemoteBinding:
            name = ((RemoteServiceBinding)tn.Tag).Name;
            break;
          case SsbEnum.Conversation:
            name = ((SSBIConversation)tn.Tag).Handle.ToString();
            break;


        }

        smob = smo.GetObject2(dbServ, dBase, name, ssbType);
        tn.Tag = null;
        tn.Tag = smob;
        SetUpSSBObject(smob);
        smoObj = smob;
      
    }

    //binds the properties of the chosen ssb object to the controls
    //on the form
    void SetUpSSBObject(object smob) {
      Server svr = null;
      SSBIServer ssbiserv = null;
      MessageType mt = null;
      ServiceContract sc = null;
      ServiceQueue q = null;
      BrokerService serv = null;
      ServiceRoute rt = null;
      RemoteServiceBinding bind = null;
      SSBIConversation smc = null;
      Database db = null;
      TabPage tp;


      //reset everything
      ClearControls();

      SetUpTabPage(out tp);

      switch (ssbType) {
        case SsbEnum.Server:
          srv_lbLogins.Items.Clear();
          srv_lbLogins.DisplayMember = "Name";
          ssbiserv = (SSBIServer)smob;
          srv_txtName.Text = ssbiserv.Name;
          srv_txtName.Enabled = false;
          svr = ssbiserv.SMOServer;
          foreach (Login l in svr.Logins) {
            if(!l.Name.Contains("#"))
              srv_lbLogins.Items.Add(l);
          }

          srv_grpLogins.Enabled = true;
          srv_grpEp.Enabled = true;

          EndpointCollection epc = svr.Endpoints;
          SSBIEndpointCollection sepc = new SSBIEndpointCollection(epc);


          serv_dvgEp.DataSource = sepc;
          
          
          break;

        case SsbEnum.Database :
          SSBIDatabase sDb = null;
          db_lbUsers.Items.Clear();
          db_lbUsers.DisplayMember = "Name";
          //db_dgvCerts.Rows.Clear();
          if (smob.GetType() == typeof(SSBIDatabase)) {
            sDb = (SSBIDatabase)smob;
            db = sDb.DataBase;
          }
          else if (smob.GetType() == typeof(Database))
            db = (Database)smob;
          db_txtName.Text = db.Name;
          db_chkMasterKey.Checked = db.MasterKey != null;
          db_chkTrustWorthy.Checked = sDb.IsTrustworthy;
          foreach (User u in db.Users) {
            if(!u.Name.Contains("#"))
              db_lbUsers.Items.Add(u);

          }

          
          db_dgvCerts.DataSource = new SSBICertificateCollection(db.Certificates);

          
          db_grpCert.Enabled = true;
          db_grpUsers.Enabled = true;
          
          break;
        case SsbEnum.MessageType:
          mt = (MessageType)(smob);
          int x = mt.ID;
          textBox1.Text = mt.Name;
          textBox2.Text = mt.Owner;
          textBox12.Text = mt.MessageTypeValidation.ToString();
          if (mt.MessageTypeValidation == MessageTypeValidation.XmlSchemaCollection)
            textBox3.Text = mt.ValidationXmlSchemaCollection.ToString();
          
          break;
        case SsbEnum.Contract:
          sc = (ServiceContract)(smob);
          textBox4.Text = sc.Name;
          textBox14.Text = sc.Owner;
          MessageTypeMappingCollection mtColl = sc.MessageTypeMappings;
          foreach (MessageTypeMapping mtm in sc.MessageTypeMappings) {
            dataGridView1.Rows.Add(new object[] { mtm.Name, mtm.MessageSource.ToString() });
          }

          break;
        case SsbEnum.Queu:
          q = (ServiceQueue)(smob);
          textBox5.Text = q.Name;
          if (!q.Name.Contains("transmission_queue")) {
            checkBox1.Checked = q.IsEnqueueEnabled;
            checkBox2.Checked = q.IsRetentionEnabled;
            checkBox3.Checked = q.IsActivationEnabled;
            if (q.IsActivationEnabled) {
              textBox6.Text = q.ProcedureDatabase;
              textBox9.Text = q.ProcedureSchema;
              textBox10.Text = q.ProcedureName;
              textBox7.Text = q.MaxReaders.ToString();
              textBox8.Text = q.ExecutionContextPrincipal;
            }
            q_dvgMsg.DataSource = smo.GetMessageCollection(null, dBase, Guid.Empty, q);
            q_dgvStatus.Visible = false;
            q_dvgMsg.Visible = true;
            groupBox4.Text = "Received Messages";
          
          }
          else {
            q_dgvStatus.Visible = true;
            q_dvgMsg.Visible = false;
            q_dgvStatus.DataSource = smo.GetStatuses(dBase);
            groupBox4.Text = "Transmission Status Messages";

          }

          break;
        case SsbEnum.Service:
          serv = (BrokerService)smob;
          srv_cboCnvState.Items.Clear();
          srv_cboSource.Items.Clear();
          srv_cboCnvState.Items.Add("*");
          srv_cboCnvState.Items.Add("STARTED_OUTBOUND");
          srv_cboCnvState.Items.Add("STARTED_INBOUND");
          srv_cboCnvState.Items.Add("CONVERSING");
          srv_cboCnvState.Items.Add("DISCONNECTED_INBOUND");
          srv_cboCnvState.Items.Add("DISCONNECTED_OUTBOUND");
          srv_cboCnvState.Items.Add("ERROR");

          srv_cboSource.Items.Add("*");
          srv_cboSource.Items.Add("Initiator");
          srv_cboSource.Items.Add("Target");



         //TODO - I've had problems with Service ID's

          try {
            int y = serv.ID;
            dvgConv.Rows.Clear();
            textBox11.Text = serv.Name;
            textBox15.Text = serv.Owner;
            textBox13.Text = serv.QueueName;

            foreach (ServiceContractMapping scm in serv.ServiceContractMappings)
              listBox1.Items.Add(scm.Name);


            SSBIConversationCollection convColl = smo.GetConversationCollection(dBase, serv);


            convCollB = convColl;

            srv_cboCnvState.Text = "*";
            srv_cboSource.Text = "*";
            FilterConversation();
          }
          catch (NullReferenceException nullex) {
            MessageBox.Show("Service ID is null");
          }

          catch (Exception ex) {
            smo.ShowException(ex);
          }
          
          break;
        
          
        case SsbEnum.Route:
          rt = (ServiceRoute)(smob);
          textBox16.Text = rt.Name;
          textBox17.Text = rt.Owner;
          textBox18.Text = rt.RemoteService;
          textBox19.Text = rt.BrokerInstance;
          textBox20.Text = rt.ExpirationDate.ToString();
          textBox21.Text = rt.Address;
          textBox22.Text = rt.MirrorAddress;

          break;
        case SsbEnum.RemoteBinding:
          bind = (RemoteServiceBinding)(smob);
          textBox23.Text = bind.Name;
          textBox24.Text = bind.Owner;
          textBox25.Text = bind.RemoteService;
          //textBox26.Text = bind.Contract;
          textBox27.Text = bind.CertificateUser;
          checkBox4.Checked = bind.IsAnonymous;
          
          break;

        case SsbEnum.Conversation:
          smc = (SSBIConversation)(smob);
          cnv_txtDlgGuid.Text = smc.Handle.ToString();
          cnv_txtRelGrpHndl.Text = smc.GroupId.ToString();
          cnv_txtFrmServ.Text = smc.FromService;
          cnv_txtToSrv.Text = smc.ToService;
          cnv_txtContract.Text = smc.Contract;
          cnv_txtState.Text = smc.State;
          if (smc.FarBrokerGuid == Guid.Empty)
            cnv_txtFarBroker.Text = "NULL";
          else
            cnv_txtFarBroker.Text = smc.FarBrokerGuid.ToString();
          cnv_txtLifeTime.Text = smc.Lifetime.ToString();
          cnv_chkInitiator.Checked = smc.IsInitiator;
          dvgCnvMsgs.DataSource = smc.MessageCollection;
          
          break;
      }

      if (ssbType == SsbEnum.Server)
        tabControl1.SelectedIndex = 9;
      else {
        tabControl1.SelectedIndex = (int)ssbType;
        
      }

      SetReadOnly(tp);
      tabControl1.Visible = true;
      
      


    }

    private void FilterConversation() {
      bool filterState = false;
      bool filterSource = false;
      bool initiator = false;
      string convState = "";
      dvgMsg.DataSource = null;
      dvgConv.DataSource = null;
      SSBIConversationCollection convColl = (SSBIConversationCollection)convCollB;
      SSBIConversationCollection cnvColl = new SSBIConversationCollection();
      if (srv_cboCnvState.Text != "*") {
        filterState = true;
        convState = srv_cboCnvState.Text;
      }
      if (srv_cboSource.Text != "*") {
        filterSource = true;
        initiator = srv_cboSource.Text == "Initiator";
      }
      if(filterState || filterSource) {
        foreach (SSBIConversation conv in convColl) {
          if (filterState && !filterSource) {
            if (conv.State == srv_cboCnvState.Text)
              cnvColl.Add(conv);
          }
          else if (!filterState && filterSource) {
            if (conv.IsInitiator == initiator)
              cnvColl.Add(conv);
          }
          else if (filterState && filterSource) {
            if (conv.IsInitiator == initiator && conv.State == srv_cboCnvState.Text)
              cnvColl.Add(conv);
          }
        }

      }
      else
        cnvColl = convColl;

      dvgConv.DataSource = cnvColl;


      if (cnvColl.Count > 0)
        dvgMsg.DataSource = cnvColl.Item(0).MessageCollection;
    }

    void SetReadOnly(TabPage tp) {
      foreach (Control c in tp.Controls) {
        if (c.GetType() == typeof(TextBox)) {
          TextBox txt = (TextBox)c;
          txt.Enabled = false;

        }
        else if (c.GetType() == typeof(CheckBox)) {
          CheckBox cb = (CheckBox)c;
          cb.Enabled = false;

        }

        else if (c.GetType() == typeof(ComboBox)) {
          ComboBox cbo = (ComboBox)c;
          cbo.Enabled = false;

        }

        else if (c.GetType() == typeof(GroupBox)) {
          GroupBox grp = (GroupBox)c;
          if (tp.Name != "tabPage4" && tp.Name != "tabPage7" && tp.Name != "tabPage3" && tp.Name != "tabPage9" && tp.Name != "tabPage8")
            grp.Enabled = false;

        }

      }
      

    }

    void ClearControls() {
      foreach (Control c in tabControl1.Controls) {
        CleanControls(c);
        if (c.HasChildren) {
          Control.ControlCollection coll = (Control.ControlCollection)c.Controls;
          ClearControls2(coll);
        }
      }

    }


    void ClearControls2(Control.ControlCollection coll) {
      foreach (Control c in coll) {
        CleanControls(c);
        if (c.HasChildren) {
          Control.ControlCollection coll2 = (Control.ControlCollection)c.Controls;
          ClearControls2(coll2);
        }
      }

    }

    void CleanControls(Control c) {
      if (c.GetType() == typeof(TextBox)) {
        TextBox txt = (TextBox)c;
        txt.Text = "";

      }
      else if (c.GetType() == typeof(DataGridView)) {
        DataGridView dv = (DataGridView)c;
        if (dv.DataSource != null)
          dv.DataSource = null;
        if(dv.Rows.Count > 0)
          dv.Rows.Clear();

      }

      else if (c.GetType() == typeof(CheckBox)) {
        CheckBox cb = (CheckBox)c;
        cb.Checked = false;

      }

      else if (c.GetType() == typeof(ListBox)) {
        ListBox lb = (ListBox)c;
        lb.Items.Clear();

      }

    }

    private void LoadForm2(int idx, bool toEdit ) {
      LoadForm2(idx, toEdit, smoObj);
    }

    private void LoadForm2(int idx, bool toEdit, object objToEdit) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      SsbEnum sbe = (SsbEnum)idx;
      Form2 f2 = null;
      TVSetUp.PopulateServArrayFromTreeView(tv1);
      //Form3 f2 = new Form3();
      if ((!toEdit && sbe == SsbEnum.Database) || (!toEdit && sbe == SsbEnum.Login) || (!toEdit && sbe == SsbEnum.EndPoint)) {
        if (sbe == SsbEnum.Database || sbe == SsbEnum.Login || sbe == SsbEnum.EndPoint)
          f2 = new Form2(sbe, dbServ);
      }
      else if (!toEdit && sbe != SsbEnum.Conversation && ssbType != SsbEnum.Service && sbe != SsbEnum.Message && ssbType != SsbEnum.Server)
        f2 = new Form2(sbe, dbServ, dBase);
      else if (!toEdit && sbe == SsbEnum.Service && ssbType == SsbEnum.Service)
        f2 = new Form2(sbe, dbServ, dBase);
      else if (!toEdit && sbe == SsbEnum.Conversation && ssbType == SsbEnum.Conversation)
        f2 = new Form2(sbe, dbServ, dBase);
      else if (!toEdit && sbe == SsbEnum.Conversation && ssbType == SsbEnum.None)
        f2 = new Form2(sbe, dbServ, dBase);
      else if (!toEdit && sbe == SsbEnum.Conversation && ssbType == SsbEnum.Service)
        f2 = new Form2(sbe, dbServ, dBase, false, objToEdit);
      else if (!toEdit && sbe == SsbEnum.Conversation && ssbType != SsbEnum.Service)
        f2 = new Form2(sbe, dbServ, dBase);
      else if (!toEdit && sbe == SsbEnum.Message && ssbType == SsbEnum.Conversation)
        f2 = new Form2(sbe, dbServ, dBase, false, objToEdit);
      else if (!toEdit && sbe == SsbEnum.Message && ssbType == SsbEnum.Service)
        f2 = new Form2(sbe, dbServ, dBase, false, objToEdit);
      else if (!toEdit && sbe == SsbEnum.CreateListing && level < 4)
        f2 = new Form2(sbe, dbServ, dBase);
      else if (!toEdit && sbe == SsbEnum.CreateListing && level == 4)
        f2 = new Form2(sbe, dbServ, dBase, false, objToEdit);
      else if (!toEdit && sbe == SsbEnum.ImportListing)
        f2 = new Form2(sbe, dbServ, dBase);


      else if (toEdit)
        f2 = new Form2(sbe, dbServ, dBase, true, objToEdit);

      f2.Processed += new SsbEventDel(f2_Processed);
      f2.Show();
      Cursor.Current = crs;
    }

    private void LoadForm3() {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      Form3 f3 = null;
      f3 = new Form3(ssbType, smoObj);
      f3.ShowDialog();
      Cursor.Current = crs;
    }

    void EndConversation(SSBIConversation cnv) {
      string question = string.Format("Do you want to delete {0}", cnv.Handle);
      DialogResult res = MessageBox.Show(question, "Deletion", MessageBoxButtons.YesNo);
      if (res == DialogResult.Yes) {
        smo.EndConversation(cnv);
      }

    }



    private void DeleteEpCert(SsbEnum sbe, NamedSmoObject delObj) {
      Cursor crs = null;
      try {
        SsbEnum sType = sbe;
        string question = "Do you want to delete {0}: {1}";
        string objName = delObj.Name;
        question = string.Format(question, sType.ToString(), objName);

        DialogResult res = MessageBox.Show(question, "Deletion", MessageBoxButtons.YesNo);
        crs = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        if (res == DialogResult.Yes) {
          toolStripStatusLabel1.Text = string.Format("Deleting {0}: {1}", sType.ToString(), objName);
          smo.DeleteObject(sbe, delObj);
        }
        if (ssbType == SsbEnum.Server || ssbType == SsbEnum.Database)
          ClickSSBObject(tv1.SelectedNode);
      }
      catch(Exception e) {
        smo.ShowException(e);

      }
      finally {
        Cursor.Current = crs;
        toolStripStatusLabel1.Text = "Ready";
      }

    }



    private void DeleteSSBObject(TreeNode tn) {
      SsbEnum sType = ssbType;
      string question = "Do you want to delete {0}: {1}";
      string objName = tn.Text;
      if (ssbType == SsbEnum.Service && smoObj.GetType() == typeof(SSBIConversation)) {
        sType = SsbEnum.Conversation;
        objName = ((SSBIConversation)smoObj).Handle.ToString();
        question = "Do you want to end {0}: {1}";
      }
      question = string.Format(question, sType.ToString(), objName);

      DialogResult res = MessageBox.Show(question, "Deletion", MessageBoxButtons.YesNo);
      if (res == DialogResult.Yes) {
        toolStripStatusLabel1.Text = string.Format("Deleting {0}: {1}", sType.ToString(), objName);
        tn.ForeColor = Color.Gray;
        delNode = tn;
        if (tn.IsExpanded)
          tn.Collapse();
        DelObjectDel del = new DelObjectDel(smo.DeleteObject);
        IAsyncResult iar = del.BeginInvoke(sType, smoObj, new AsyncCallback(FinishedDel), del);
      }
     
    }

    void FinishedDel(IAsyncResult iar) {
      if (this.InvokeRequired) {
        this.BeginInvoke(new AsyncCallback(FinishedDel), new object[] { iar });
      }
      else {
        try {
          DelObjectDel del = (DelObjectDel)iar.AsyncState;
          del.EndInvoke(iar);
          TVSetUp.ExpandNodes1(tv1, delNode.Parent, false, false, SsbEnum.None);
          //set ssbType
          level = tv1.SelectedNode.Level;

          if (level == 0)
            ssbType = SsbEnum.None;
          else if (level == 1)
            ssbType = SsbEnum.Server;
          else if (level == 2)
            ssbType = SsbEnum.Database;
          else if (level == 4)
            ssbType = (SsbEnum)tv1.SelectedNode.Index;



          
          if (tv1.SelectedNode.Level == 3 || tv1.SelectedNode.Level == 0)
            tabControl1.Visible = false;
          else {
            SetUpSSBObject((object)tv1.SelectedNode.Tag);
          }

          toolStripStatusLabel1.Text = "Ready!";
        }

        catch (FailedOperationException fe) {
          smo.ShowException(fe);
          delNode.ForeColor = Color.Black;
          toolStripStatusLabel1.Text = "Ready!";
        }

        catch (Exception e) {
          smo.ShowException(e);
          delNode.ForeColor = Color.Black;
          toolStripStatusLabel1.Text = "Ready!";
        }

        finally {
          delNode = null;
        }


      }
    }

    #region event methods

    void mnuCtx_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
      int idx;
      //check what we are clicking
      string name = e.ClickedItem.Name;
      if (name != "mnuSubNewDropDown") {
        switch (level) {

          
          case 3: //top ssb object
            idx = tv1.SelectedNode.Index;
            if (name == "mnuSubNew")
              LoadForm2(idx, false);
            break;

        }
      }

    }

    void mnuTopFile_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
      string name = e.ClickedItem.Name;
      if (name == "mnuSubExit") {
        this.Close();
        this.Dispose();
      }


    }

    void mnuFileNewDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
      int idx = (int)e.ClickedItem.Tag;
      LoadForm2(idx, false);  
    
    }

    
    void mnuSubNewDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
      int idx = (int)e.ClickedItem.Tag;
      LoadForm2(idx, false);

    }

    void f2_Processed(object sender, SsbEventsArgs e) {
      bool expanded = true;
      if (level == 1 && tv1.SelectedNode.IsExpanded && e.state == SsbState.New)
        TVSetUp.ExpandNodes1(tv1, tv1.SelectedNode, true, false, SsbEnum.None);

      
      if (ssbType == e.ssbType && dbServ.Name == e.server && dBase.Name == e.dbName && level > 1) {
        if (level == 2 && e.state == SsbState.Edited)
          RefreshSSBObject(tv1.SelectedNode);
        if (level == 3) 
          expanded = tv1.SelectedNode.IsExpanded;


        if (expanded || tv1.SelectedNode.Nodes.Count == 0) {
          if (e.state == SsbState.New || e.state == SsbState.Edited)
            TVSetUp.ExpandNodes1(tv1, tv1.SelectedNode, true, false, SsbEnum.None);
        }
      }

      if (ssbType == SsbEnum.Service && e.ssbType == SsbEnum.Conversation && dbServ.Name == e.server && dBase.Name == e.dbName && level == 4) {
        if (e.state == SsbState.New || e.state == SsbState.Edited)
          ClickSSBObject(tv1.SelectedNode);
      }

      if (ssbType == SsbEnum.Server) {
        if (e.ssbType == SsbEnum.Login || e.ssbType == SsbEnum.EndPoint) {
          ClickSSBObject(tv1.SelectedNode);

        }
      }

      if (ssbType == SsbEnum.Database) {
        if (e.ssbType == SsbEnum.User || e.ssbType == SsbEnum.Certificate) {
          //db_lbUsers.Items.Add((User)e.updated);
          ClickSSBObject(tv1.SelectedNode);

        }

      }

    }

    void tv1_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e) {
      level = e.Node.Level;
      TreeNode tn = e.Node; ;
      //check that we're not deleting that particular node right now
      if (OkToExpand(e.Node)) {

        tv1.BeginUpdate();
        tv1.SelectedNode = tn;
        if (TVSetUp.ExpandNodes1(tv1, tn, false, false, SsbEnum.None)) {
          if (!expandError) {
            tv1.SelectedNode = tn;
            if (tn.Level == 1 || tn.Level == 2 || tn.Level == 4)
              ClickSSBObject(tn);


          }
          tv1.EndUpdate();
          expandError = false;
        }
        else {
          e.Cancel = true;
          tv1.EndUpdate();
        }
      }
      else
        e.Cancel = true;
      


    }

    bool OkToExpand(TreeNode tn) {
      bool ok = true;
      string msg = "";
      if (tn == delNode) {
        msg = "This object can not be expanded as it is being deleted";
        ok = false;
      }
      else if (level == 1 && ((SSBIServer)tn.Tag).IsTryingToConnect) {
        msg = "This object can not be expanded as a connection attempt is underway";
        ok = false;

      }
      if (!ok)
        MessageBox.Show(msg);

      return ok;
    }

    void tv1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
      TreeNode tn = e.Node;
      if (e.Button == MouseButtons.Right) {
        level = e.Node.Level;
        SetupContextMenuAndShow(e);
        tv1.SelectedNode = e.Node;
        
      }
    }

    void tv1_AfterSelect(object sender, TreeViewEventArgs e) {
      dbServ = null;
      dBase = null;
      TreeNode tn = e.Node;
      level = tn.Level;
      ssbType = SsbEnum.None;
      try {
        if (level == 0) {
          tabControl1.Visible = false;
          dbServ = null;
          dBase = null;
        }

        else if (level == 1) {
          tabControl1.Visible = false;
          dbServ = (SSBIServer)tn.Tag;
          ssbType = SsbEnum.Server;
          ClickSSBObject(tn);
        }

        else if (level == 2) {
          dbServ = (SSBIServer)tn.Parent.Tag;
          dBase = TVSetUp.GetDatabaseFromTag(tn);
          ssbType = SsbEnum.Database;
          ClickSSBObject(tn);
        }

        else if (level == 3) {
          tabControl1.Visible = false;
          dbServ = (SSBIServer)tn.Parent.Parent.Tag;
          dBase = TVSetUp.GetDatabaseFromTag(tn.Parent);
          //dBase = (Database)tn.Parent.Tag;
          ssbType = (SsbEnum)tn.Index;
        }

        else if (level == 4) {
          dbServ = (SSBIServer)tn.Parent.Parent.Parent.Tag;
          dBase = TVSetUp.GetDatabaseFromTag(tn.Parent.Parent);
          //dBase = (Database)tn.Parent.Parent.Tag;
          ssbType = (SsbEnum)tn.Parent.Index;
          ClickSSBObject(tn);
        }
      }
      catch (Exception ex) {
        //if (e.Node.IsExpanded)
        //  e.Node.Collapse();
        expandError = true;
        smo.ShowException(ex);
        e.Node.Parent.Collapse();
        tv1.SelectedNode = e.Node.Parent;
        

      }

     
    }

    
    void mnuTopFile_DropDownOpening(object sender, EventArgs e) {
      mnuTopFile.DropDownItems.Clear();
      mnuTopFile.DropDownItems.AddRange(new ToolStripItem[] { mnuSubOpenConn, mnuFileNewDropDown, mnuSubSep, mnuSubExit });
      
      mnuFileNewDropDown.Enabled = level > 0 && dbServ.HasLoggedIn;
      mnuSubOpenConn.Enabled = level == 1 && !dbServ.HasLoggedIn && !dbServ.IsTryingToConnect;
      

    }

    void mnuTopEdit_DropDownOpening(object sender, EventArgs e) {
      mnuTopEdit.DropDownItems.Clear();
      mnuTopEdit.DropDownItems.AddRange(new ToolStripItem[] {mnuSubCopy, mnuSubAlter, mnuSubDelete });
      mnuSubAlter.Enabled = false;
      mnuSubCopy.Enabled = false;
      mnuSubDelete.Text = "Delete/Drop";
      bool lvl = level == 4 || level == 2;
      mnuSubDelete.Enabled = lvl;

      

      if (ssbType == SsbEnum.Conversation)
        mnuSubDelete.Text = "End Conversation";

      //for copy re-deploy check what SSBType it is
      if (ssbType == SsbEnum.MessageType || ssbType == SsbEnum.Contract)
        mnuSubCopy.Enabled = level == 4;

      //TODO - at the moment I can only alter message types. Change in later build
      if (ssbType == SsbEnum.MessageType || ssbType == SsbEnum.Database)
        mnuSubAlter.Enabled = level == 4;

    }

    void mnuTopView_DropDownOpening(object sender, EventArgs e) {
      mnuTopView.DropDownItems.Clear();
      mnuTopView.DropDownItems.AddRange(new ToolStripItem[] { mnuSubRefresh, mnuSubViewMsg });
      mnuSubRefresh.Text = "Refresh";
      mnuSubRefresh.Enabled = false;
      mnuSubViewMsg.Enabled = false;
      TreeNode tn = tv1.SelectedNode;
      if (level != 2 && level != 4) {
        if(level ==0) 
          mnuSubRefresh.Text = "Add/Browse Server(s)";
        
        mnuSubRefresh.Enabled = true;
      }

      if (level == 4) {
        if(ssbType == SsbEnum.Service && dvgMsg.SelectedRows.Count > 0)
          mnuSubViewMsg.Enabled = true;
        else if(ssbType == SsbEnum.Conversation && dvgCnvMsgs.SelectedRows.Count > 0)
          mnuSubViewMsg.Enabled = true;
        else if(ssbType == SsbEnum.Queu && q_dvgMsg.SelectedRows.Count > 0)
          mnuSubViewMsg.Enabled = true;

      }

    }

    void mnuFileNewDropDown_DropDownOpening(object sender, EventArgs e) {
      mnuFileNewDropDown.DropDownItems.Clear();
      mnuFileNewDropDown.DropDownItems.AddRange(new ToolStripItem[] { mnuSubLogin, mnuSubEndpoint, mnuSubDataBase, mnuSubCertificate, mnuSubUser,  mnuSubMsgType, mnuSubContract, mnuSubQueue, mnuSubService, mnuSubRoute, mnuSubRemoteBinding, mnuSubConversation });

      foreach (ToolStripItem ti in mnuFileNewDropDown.DropDownItems)
        ti.Enabled = false;

      if (level == 1) {
        mnuSubLogin.Enabled = true;
        mnuSubEndpoint.Enabled = true;
        mnuSubDataBase.Enabled = true;

      }
      else if (level > 1) {
        foreach (ToolStripItem ti in mnuFileNewDropDown.DropDownItems)
          ti.Enabled = true;

      }
        
    
    }

    void mnuSubNewDropDown_DropDownOpening(object sender, EventArgs e) {
      mnuSubNewDropDown.DropDownItems.Clear();
      if(level==1)
        mnuSubNewDropDown.DropDownItems.AddRange(new ToolStripItem[] { mnuSubLogin, mnuSubEndpoint, mnuSubDataBase});
      else if (level == 2)
        mnuSubNewDropDown.DropDownItems.AddRange(new ToolStripItem[] { mnuSubCertificate, mnuSubUser,  mnuSubMsgType, mnuSubContract, mnuSubQueue, mnuSubService, mnuSubRoute, mnuSubRemoteBinding, mnuSubConversation });

      foreach (ToolStripDropDownItem i in mnuSubNewDropDown.DropDownItems)
        i.Enabled = true;

    }

    void mnuSubRefresh_Click(object sender, EventArgs e) {
      if (level == 0) {
        addServer f4 = new addServer();
        f4.ShowDialog();
        if (f4.DialogResult == DialogResult.OK) {
          ICollection addedServers = f4.addedServers;
          if (addedServers != null)
            TVSetUp.SetUpTreeView(tv1, addedServers, true);
        }
      }
      
      if (level == 1 || level == 3)
        TVSetUp.ExpandNodes1(tv1, tv1.SelectedNode, true, false, SsbEnum.None);
        
      else if (level == 4)
        SetUpSSBObject(smoObj);

    }

    void mnuSubEndpoint_Click(object sender, EventArgs e) {
      LoadForm2((int)SsbEnum.EndPoint, false);  
    }

    void mnuSubAlter_Click(object sender, EventArgs e) {
      LoadForm2((int)ssbType, true);

    }

    void mnuSubCopy_Click(object sender, EventArgs e) {
      LoadForm3();

    }

    void mnuSubDelete_Click(object sender, EventArgs e) {
      DeleteSSBObject(tv1.SelectedNode);
    }

    void mnuSubNewConversation_Click(object sender, EventArgs e) {
      LoadForm2((int)SsbEnum.Conversation, false);
    
    }

    void mnuSubSendMsg_Click(object sender, EventArgs e) {
      LoadForm2((int)SsbEnum.Message, false);
      
    }

    void mnuSubSendMsgFromService_Click(object sender, EventArgs e) {
      object o = smoObj;
      try {
        SSBIConversationCollection cnvColl = (SSBIConversationCollection)dvgConv.DataSource;
        SSBIConversation cnv = cnvColl.Item(dvgConv.CurrentRow.Index);
        smoObj = cnv;
        LoadForm2((int)SsbEnum.Message, false);
      }
      finally {
        smoObj = o;
      }
    }

    void  mnuSubEndConversationFromService_Click(object sender, EventArgs e) {
      object o = smoObj;
      try {
        SSBIConversationCollection cnvColl = (SSBIConversationCollection)dvgConv.DataSource;
        SSBIConversation cnv = cnvColl.Item(dvgConv.CurrentRow.Index);
        smoObj = cnv;
        //DeleteSSBObject(
        EndConversation(cnv);
        ClickSSBObject(tv1.SelectedNode);
      }
      finally {
        smoObj = o;
      }
    }

    void mnuSubViewMsg_Click(object sender, EventArgs e) {
      DataGridView dgv = null;
      //string n = ctx.GetContainerControl().ActiveControl.Name;
      if (ssbType == SsbEnum.Service)
        dgv = dvgMsg;

      else if (ssbType == SsbEnum.Queu) {
        if(q_dvgMsg.Visible)
          dgv = q_dvgMsg;
        else if (q_dgvStatus.Visible)
          dgv = q_dgvStatus;
      }

      else if (ssbType == SsbEnum.Conversation)
        dgv = dvgCnvMsgs;

      if (dgv != null)
        ViewMsgObject(dgv, dgv.SelectedRows[0].Index);
    
    }

    void mnuSubOpenConn_Click(object sender, EventArgs e) {
      TVSetUp.Connect(tv1, tv1.SelectedNode);

    }

    void mnuSubAbout_Click(object sender, EventArgs e) {
      about ab = new about();
      ab.ShowDialog();
    }

    

    
    void tv1_AfterCollapse(object sender, TreeViewEventArgs e) {
      //tabControl1.Visible = false;
      if (e.Node.Level == 2 || e.Node.Level == 1 ) {
        tabControl1.Visible = true;
      }
      else
        tabControl1.Visible = false;
    }

    
    private void dvgConv_CellClick(object sender, DataGridViewCellEventArgs e) {
      //if (e.RowIndex > -1) {
      //  SSBIConversationCollection cnvColl = (SSBIConversationCollection)dvgConv.DataSource;
      //  SSBIConversation cnv = cnvColl.Item(e.RowIndex);
      //  dvgMsg.DataSource = cnv.MessageCollection;
      //}
      

    }

    void dvgConv_RowEnter(object sender, DataGridViewCellEventArgs e) {
      if (e.RowIndex > -1) {
        SSBIConversationCollection cnvColl = (SSBIConversationCollection)dvgConv.DataSource;
        SSBIConversation cnv = cnvColl.Item(e.RowIndex);
        dvgMsg.DataSource = cnv.MessageCollection;
      }
    }

    void dvgConv_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e) {
      mnuCtx.Items.Clear();

      dvgConv.CurrentCell = dvgConv.Rows[e.RowIndex].Cells[0];
      mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubSendMsgFromService, mnuSubSep,mnuSubEndConversationFromService });
      e.ContextMenuStrip = mnuCtx;
      
    
    }

    void msgGrids_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e) {
      mnuCtx.Items.Clear();
      DataGridView dgv = (DataGridView)sender;

      dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[0];
      mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubViewMsg });
      e.ContextMenuStrip = mnuCtx;
      
      
      


    }

    
    void endPCertsGrids_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e) {
      mnuCtx.Items.Clear();
      DataGridView dgv = (DataGridView)sender;
      dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[0];
      mnuCtx.Items.AddRange(new ToolStripItem[] { mnuSubEpCertUsrAlter, mnuSubSep, mnuSubEpCertUsrDrop });
      e.ContextMenuStrip = mnuCtx;
    }

          

    private void dvgMsg_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {

      ViewMsgObject(dvgMsg, e.RowIndex);
      
    }

    private void ViewMsgObject(DataGridView dgv, int idx) {
      if (idx > -1) {
        object o = null;
        try {
          o = smoObj;
          if (dgv.Name != "q_dgvStatus") {
            SSBIMessageCollection msgColl = (SSBIMessageCollection)dgv.DataSource;
            SSBIMessage msg = (SSBIMessage)msgColl.Item(idx);
            smoObj = msg;
          }
          else {
            SSBITxStatusCollection txStatColl = (SSBITxStatusCollection)dgv.DataSource;
            TxStatus txStat = (TxStatus)txStatColl.Item(idx);
            smoObj = txStat;

          }

          
          LoadForm2((int)SsbEnum.Message, true);
        }
        finally {
          smoObj = o;
        }

      }
    }

    void dvgCnvMsgs_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
      ViewMsgObject(dvgCnvMsgs, e.RowIndex);
    }

    void q_dvgMsg_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
      DataGridView dgv = (DataGridView)sender;
      ViewMsgObject(dgv, e.RowIndex);
    }

    void srv_cboCnvState_SelectedIndexChanged(object sender, EventArgs e) {
      
      //dvgConv.DataSource;
      //SSBIConversationCollection cnvColl = (SSBIConversationCollection)dvgConv.DataSource;
      //SSBIConversation cnv = cnvColl.Item(e.RowIndex);
      //dvgMsg.DataSource = cnv.MessageCollection;
      FilterConversation();
      
      
    
    }

    void srv_btnLogin_Click(object sender, EventArgs e) {
      LoadForm2((int)SsbEnum.Login, false);
    }

    void mnuSubEpCertUsrDrop_Click(object sender, EventArgs e) {
      object obj;
      NamedSmoObject objToDelete = null; ;
      SsbEnum sbe;
      GetObjectToUpdateDelete(out obj, out sbe);
      if (sbe == SsbEnum.EndPoint)
        objToDelete = ((SSBIEndpoint)obj).EndPoint;
      else if (sbe == SsbEnum.Certificate)
        objToDelete = (Certificate)obj;

      DeleteEpCert(sbe, (NamedSmoObject)objToDelete);
      
    }

    void mnuSubSecureService_Click(object sender, EventArgs e) {
    }

    void mnuSubImportServiceListing_Click(object sender, EventArgs e) {
      LoadForm2((int)SsbEnum.ImportListing, false);
    }

    void mnuSubCreateServiceListing_Click(object sender, EventArgs e) {
      LoadForm2((int)SsbEnum.CreateListing, false);
      
    }


    void mnuSubEpCertUsrAlter_Click(object sender, EventArgs e) {
      object objToUpdate;
      SsbEnum sbe;
      GetObjectToUpdateDelete(out objToUpdate, out sbe);
      LoadForm2((int)sbe, true, objToUpdate);
    }

    private void GetObjectToUpdateDelete(out object objToUpdate, out SsbEnum sbe) {
      DataGridView dgv = null;
      objToUpdate = null;
      sbe = 0;
      if (ssbType == SsbEnum.Server) {
        dgv = serv_dvgEp;
        sbe = SsbEnum.EndPoint;
        objToUpdate = ((SSBIEndpointCollection)dgv.DataSource).Item(dgv.SelectedRows[0].Index);
      }
      else if (ssbType == SsbEnum.Database) {
        dgv = db_dgvCerts;
        sbe = SsbEnum.Certificate;
        objToUpdate = ((SSBICertificateCollection)dgv.DataSource).Item(dgv.SelectedRows[0].Index);
      }
    }


    void db_lbUsers_MouseClick(object sender, MouseEventArgs e) {

      throw new Exception("The method or operation is not implemented.");
    }

    #endregion



    private void SetUpTabPage(out TabPage tp) {
      foreach (TabPage tp1 in tabControl1.TabPages)
        this.tabControl1.Controls.Remove(tp1);

      tp = null;


      switch (ssbType) {
        case SsbEnum.Server:
          this.tabControl1.Controls.Add(tabPage9);
          tp = tabPage9;
          break;
        case SsbEnum.Database:
          this.tabControl1.Controls.Add(tabPage8);
          tp = tabPage8;
          break;
        case SsbEnum.MessageType:
          this.tabControl1.Controls.Add(tabPage1);
          tp = tabPage1;
          break;
        case SsbEnum.Contract:
          this.tabControl1.Controls.Add(tabPage2);
          tp = tabPage2;
          break;
        case SsbEnum.Queu:
          this.tabControl1.Controls.Add(tabPage3);
          tp = tabPage3;
          break;
        case SsbEnum.Service:
          this.tabControl1.Controls.Add(tabPage4);
          tp = tabPage4;
          break;
        case SsbEnum.Route:
          this.tabControl1.Controls.Add(tabPage5);
          tp = tabPage5;
          break;
        case SsbEnum.RemoteBinding:
          this.tabControl1.Controls.Add(tabPage6);
          tp = tabPage6;
          break;
        case SsbEnum.Conversation:
          this.tabControl1.Controls.Add(tabPage7);
          tp = tabPage7;
          break;
      }

      tabControl1.Visible = true;
    }

    private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

    }

    

    
  }

  }
