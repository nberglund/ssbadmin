using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace SsbAdmin {
  public partial class addServer : Form {


    public ICollection addedServers = null;
    
    public addServer() {
      InitializeComponent();
      SetEventHandlers();


    }

    private void BrowseForServers() {
      Cursor crs = Cursor.Current;
      try {
        GetServersDelegate gd = new GetServersDelegate(smo.GetServers);
        Cursor.Current = Cursors.WaitCursor;
        IAsyncResult iar = gd.BeginInvoke(SsbServerLocation.Network, null, null);
        while (!iar.IsCompleted) {
          Thread.Sleep(100);
        }
        ArrayList al = gd.EndInvoke(iar); ;
        lbNetWork.Items.Clear();
        IEnumerator en = al.GetEnumerator();
        while (en.MoveNext())
          lbNetWork.Items.Add(((SSBIServer)en.Current).Name);

      }

      catch (Exception e) {
        smo.ShowException(e);
      }
      finally {
        btnBrowse.Enabled = true;
        Cursor.Current = crs;
      }
    }

    #region event hooks

    void SetEventHandlers() {
      #region buttons
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
      this.btnRemoveFromList.Click += new System.EventHandler(this.btnRemoveFromList_Click);
      this.btnCancel.Click += new EventHandler(btnCancel_Click);
      #endregion

    }


    #endregion


    #region event methods

    private void btnAdd_Click(object sender, EventArgs e) {
      if (lbChosen.FindStringExact(textBox1.Text) == -1 && textBox1.Text != string.Empty) {
        lbChosen.Items.Add(new SSBIServer(textBox1.Text));
      }
      textBox1.Text = "";
    }

    private void btnAddFromList_Click(object sender, EventArgs e) {
      
      if (lbNetWork.SelectedIndex != -1) {
        string servername = lbNetWork.SelectedItem.ToString();
        if (lbChosen.FindStringExact(servername) == -1) {
          lbChosen.Items.Add(new SSBIServer(servername));
          lbNetWork.Items.Remove(servername);
        }
      }
   

    }

    private void btnRemoveFromList_Click(object sender, EventArgs e) {
      if (lbChosen.SelectedIndex != -1) {
        string servername = ((SSBIServer)lbChosen.SelectedItem).Name;
        if (lbNetWork.FindStringExact(servername) == -1) {
          lbNetWork.Items.Add(servername);

        }
        lbChosen.Items.Remove(lbChosen.SelectedItem);
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e) {
      btnBrowse.Enabled = false;
      BrowseForServers();
      
    }

    
    

    private void btnOK_Click(object sender, EventArgs e) {
      if (lbChosen.FindStringExact(textBox1.Text) == -1 && textBox1.Text != string.Empty) {
        lbChosen.Items.Add(new SSBIServer(textBox1.Text));
      }

      addedServers = lbChosen.Items;
      this.DialogResult = DialogResult.OK;
      this.Close();

    }

    void btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    

    #endregion 

  }
}