using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SsbAdmin {
  public partial class login : Form {

    public login(SSBIServer serv) {
      InitializeComponent();
      this.Text = "Login";
      srv_cboAuth.Items.Clear();
      srv_cboAuth.Items.Add(SSBServerAuthentication.Integrated);
      srv_cboAuth.Items.Add(SSBServerAuthentication.Mixed);
      srv_cboAuth.SelectedIndex = 0;
      srv_txtName.Text = serv.Name;
      srv_txtName.Enabled = false;
      

    }

    private void srv_cboAuth_SelectedIndexChanged(object sender, EventArgs e) {
      srv_grpCred.Visible = (srv_cboAuth.Text == SSBServerAuthentication.Mixed.ToString());
    }

    private void srv_btnOK_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
      this.Close();

    }

    private void srv_btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    
  }
}