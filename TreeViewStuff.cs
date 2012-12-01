#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Broker;
using System.Collections;
using System.Threading;

#endregion

namespace SsbAdmin {

  internal delegate ArrayList GetServersDelegate(SsbServerLocation loc);
  delegate void SetStatusTextDelegate(Control hostControl, ToolStripStatusLabel statusLabel, string text);


  delegate void ConnectAsyncDelegate(SSBIServer serv, TreeView tv, TreeNode tn);
  delegate void ConnectCompletedDelegate(SSBIServer serv, Server srv, TreeView tv, TreeNode tn, bool success, string msg);


  internal class TVSetUp {

    static ICollection servers = null;
    static ArrayList servNameArray = null;

    //public static event TreeViewLoadedDelegate TreeViewLoaded;

    public static void SetUpTreeView(TreeView tv1) {
      SetUpTreeView(tv1, null);

    }

    public static void PopulateServArrayFromTreeView(TreeView tv) {
      ArrayList al = new ArrayList();
      servNameArray = new ArrayList();
      foreach (TreeNode tn in tv.Nodes[0].Nodes) {
        al.Add((SSBIServer)tn.Tag);
        servNameArray.Add(((SSBIServer)tn.Tag).Name);
      }

      servers = al;

    }


    public static void SetUpTreeView(TreeView tv1, ICollection _servers) {

      SetUpTreeView(tv1, _servers, false);

    }

    public static void SetUpTreeView(TreeView tv1, ICollection _servers, bool addServers) {
      TreeNode tn = null;
      TreeNode selectedNode = null;
      ICollection servColl = null;
      if (!addServers) {
        if (_servers != null)
          servers = _servers;

        servColl = servers;
        tv1.Nodes.Clear();
        tn = new TreeNode("Servers");
        tv1.Nodes.Add(tn);

        selectedNode = tn;
        
      }
      else {
        PopulateServArrayFromTreeView(tv1);
        tn = tv1.Nodes[0];
        servColl = _servers;
        selectedNode = tv1.SelectedNode;
       
      }

      IEnumerator senum = servColl.GetEnumerator();
      
     while (senum.MoveNext()) {
       SSBIServer serv = (SSBIServer)senum.Current;
       if (addServers && servNameArray.Contains(serv.Name))
         continue;
       TreeNode tnserv = new TreeNode();
       tnserv.Text = serv.Name;
       tnserv.Tag = serv;
       tn.Nodes.Add(tnserv);
       tnserv.Nodes.Add("");
     }

     tv1.SelectedNode = selectedNode;

    }


    static void RetrieveAndLoadObjects(SSBIServer s, Database db, TreeNode tnode, SsbEnum ssbType) {
      ICollection coll = smo.GetSSBObjects2(s, db, ssbType);
      IEnumerator en = coll.GetEnumerator();
      while (en.MoveNext()) {
        TreeNode tn = new TreeNode();
        tn.Text = en.Current.ToString();
        tn.Tag = en.Current;
        tnode.Nodes.Add(tn);
      }
      if (ssbType == SsbEnum.Queu) {
        TreeNode tn = new TreeNode();
        ServiceQueue txQ = new ServiceQueue();
        //txQ.Parent.Parent = GetDatabaseFromTag(tn.Parent);
        txQ.Name = "sys.transmission_queue";
        tn.Text = txQ.Name;
        tn.Tag = txQ;
        tnode.Nodes.Add(tn);
      }


    }

    internal static void SetStatusText(Control hostControl, ToolStripStatusLabel statusLabel, string text) {
      if(hostControl.InvokeRequired)
        hostControl.BeginInvoke(new SetStatusTextDelegate(SetStatusText), new object[] { hostControl, statusLabel, text });
      else
        statusLabel.Text = text;

    }

    public static bool ExpandNodes1(TreeView tv1, TreeNode tn, bool refresh, bool filterSsb, SsbEnum sType) {
      SsbEnum ssbType = SsbEnum.None;
      ssbType = sType;
      SSBIServer serv = null;
      bool res = true;

      int lvl = tn.Level;
      try {
        if (lvl < 1)
          ssbType = SsbEnum.None;

        else if (lvl == 1) {
          ssbType = SsbEnum.Server;
          serv = (SSBIServer)tn.Tag;
          if (!serv.HasLoggedIn) {
            res = false;
            Connect(tv1, tn);
          }
          //SetUpSSBObject(tn.Tag);

        }
        else if (lvl == 2 && !filterSsb)
          ssbType = SsbEnum.Database;
        else if (lvl == 2 && filterSsb)
          ssbType = sType;
        
        else if (lvl > 2 && !filterSsb)
          ssbType = (SsbEnum)tn.Index;
        else if (lvl > 2 && filterSsb)
          ssbType = sType;

        if (lvl != 1)
          TVSetUp.ExpandNodes(tv1, tn, ref ssbType);
        else if (lvl == 1 && serv.HasLoggedIn) {
          TVSetUp.ExpandNodes(tv1, tn, ref ssbType);

        }

        //if (lvl == 3 && tabControl1.Visible)
        //  tabControl1.Visible = false;

        return res;
      }
      catch (Exception ex) {
        smo.ShowException(ex);
        return false;
      }

    }
    

    public static bool GetLoginCredentials(SSBIServer srv) {
      bool res = true;
      if (!srv.HasLoggedIn) {
        res = false;
        login frmLogin = new login(srv);
        if (frmLogin.ShowDialog() == DialogResult.OK) {
          srv.Authentication = (SSBServerAuthentication)frmLogin.srv_cboAuth.SelectedItem;
          if (srv.Authentication == SSBServerAuthentication.Mixed) {
            srv.UserId = frmLogin.srv_txtUid.Text;
            srv.Password = frmLogin.srv_txtPwd.Text;
          }
          res = true;

        }

        frmLogin.Dispose();
      }
      return res;


    }


    public static void Connect(TreeView tv, TreeNode connNode) {
      SSBIServer srv = (SSBIServer)connNode.Tag;
      if (!srv.HasLoggedIn) {
        if (GetLoginCredentials(srv)) {
          //toolStripStatusLabel1.Text = string.Format("Connecting to: {0}", srv.Name);
          connNode.ForeColor = Color.Gray;
          srv.IsTryingToConnect = true;
          connNode.Tag = srv;
          connNode.Text = srv.Name + " - Connecting";
          ConnectAsyncDelegate cd = new ConnectAsyncDelegate(ConnectAsync);
          IAsyncResult iar = cd.BeginInvoke(srv, tv, connNode, null, cd);

        }
      }
    }

    static void ConnectAsync(SSBIServer srv, TreeView tv, TreeNode connNode) {
      Server serv = null;
      try {
        ConnectDelegate cd = new ConnectDelegate(smo.CreateServer2);
        IAsyncResult iar = cd.BeginInvoke(srv, null, null);
        while (!iar.IsCompleted) {
          //TVSetUp.SetStatusText(tv, toolStripStatusLabel1, string.Format("Connecting to: {0}", srv.Name));
          Thread.Sleep(50);
        }

        serv = cd.EndInvoke(iar);
        ConnectCompleted(srv, serv, tv, connNode, true, null);
      }

      catch (Exception e) {
        ConnectCompleted(srv, serv, tv, connNode, false, e.Message);
      }


    }

    static void ConnectCompleted(SSBIServer serv, Server srv, TreeView tv, TreeNode tn, bool success, string msg) {

      if (tv.InvokeRequired) {
        tv.Invoke(new ConnectCompletedDelegate(ConnectCompleted), new object[] { serv, srv, tv, tn, success, msg });
      }
      else {

        try {
          if (success) {
            serv.HasLoggedIn = true;
            serv.SMOServer = srv;
            serv.IsTryingToConnect = false;
            tn.Tag = serv;
            if (tn == tv.SelectedNode) {
              tn.Expand();
            }
          }
          else {
            MessageBox.Show(msg);
          }

        }

        catch (FailedOperationException fe) {
          smo.ShowException(fe);


        }

        catch (Exception e) {
          smo.ShowException(e);

        }


        finally {
          //connNode = null;
          tn.ForeColor = Color.Black;
          //toolStripStatusLabel1.Text = "Ready!";
          serv.IsTryingToConnect = false;
          tn.Tag = serv;
          tn.Text = serv.Name;
        }


      }


    }

    internal static Database GetDatabaseFromTag(TreeNode tn) {
      Database db = null;
      if (tn.Tag.GetType() == typeof(SSBIDatabase))
        db = ((SSBIDatabase)tn.Tag).DataBase;
      else if (tn.Tag.GetType() == typeof(Database))
        db = (Database)tn.Tag;

      return db;


    }

    internal static void ExpandNodes(TreeView tv1, TreeNode tn, ref SsbEnum ssbType) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      SSBIServer serv = null;
      Database db = null;
      //dbName = null;
      //svrName = null;
      int lvl = tn.Level;
      try {
        if (lvl > 0)
          tn.Nodes.Clear();

        //get databases
        if (lvl == 1) {
          serv = (SSBIServer)tn.Tag;
          DatabaseCollection dbColl = (DatabaseCollection)smo.GetSSBObjects2(serv, null, SsbEnum.Server);
          foreach (Database db2 in dbColl) {
            TreeNode tndb = new TreeNode();
            tndb.Text = db2.Name;
            tndb.Tag = db2;
            tndb.Nodes.Add(new TreeNode(""));
            tn.Nodes.Add(tndb);
          }

        }
        //set up main broker objects
        else if (lvl == 2) {
          //statusStrip1.Text = "Loading SSB Objects...";
          if (ssbType == SsbEnum.Database) {
            tn.Nodes.Add(new TreeNode("Message Types"));
            tn.Nodes.Add(new TreeNode("Message Contracts"));
            tn.Nodes.Add(new TreeNode("Queues"));
            tn.Nodes.Add(new TreeNode("Services"));
            tn.Nodes.Add(new TreeNode("Routes"));
            tn.Nodes.Add(new TreeNode("Remote Service Bindings"));
            tn.Nodes.Add(new TreeNode("Conversations"));
            
          }
          else {
            if (ssbType == SsbEnum.MessageType)
              tn.Nodes.Add(new TreeNode("Message Types"));
            else if (ssbType == SsbEnum.Contract)
              tn.Nodes.Add(new TreeNode("Message Contracts"));
            else if (ssbType == SsbEnum.Queu)
              tn.Nodes.Add(new TreeNode("Queues"));
            else if (ssbType == SsbEnum.Service)
              tn.Nodes.Add(new TreeNode("Services"));
            else if (ssbType == SsbEnum.Route)
              tn.Nodes.Add(new TreeNode("Routes"));
            else if (ssbType == SsbEnum.RemoteBinding)
              tn.Nodes.Add(new TreeNode("Remote Service Bindings"));
            else if (ssbType == SsbEnum.Conversation)
              tn.Nodes.Add(new TreeNode("Conversations"));
            

          }

          foreach (TreeNode tnChild in tn.Nodes) {
            tnChild.Nodes.Add(new TreeNode(""));
          }
        }

          //retrieve objects for chosen main SSB Object
        else if (lvl == 3) {
          //statusStrip1.Text = "Loading Objects...";
          db = GetDatabaseFromTag(tn.Parent);
          //db = (Database)tn.Parent.Tag;
          serv = (SSBIServer)tn.Parent.Parent.Tag;
          RetrieveAndLoadObjects(serv, db, tn, ssbType);

        }

        else if (lvl == 4) {
          //statusStrip1.Text = "Loading Objects...";
          TreeNode tnp = tn.Parent;
          db = GetDatabaseFromTag(tn.Parent);
          //db = (Database)tnp.Parent.Tag;
          serv = (SSBIServer)tnp.Parent.Parent.Tag;
          RetrieveAndLoadObjects(serv, db, tnp, ssbType);

          foreach (TreeNode tne in tnp.Nodes) {
            if (tne.Text == tn.Text) {
              tv1.SelectedNode = tne;
              break;
            }
          }
        }

        //if (lvl != 4)
        //tv1.SelectedNode = tn;
      }
      catch (Exception ex) {
        throw ex;
      }


    }
  }
}
