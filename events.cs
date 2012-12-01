#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace SsbAdmin {

  public delegate void SsbEventDel(object sender, SsbEventsArgs e);

  public class SsbEventsArgs : EventArgs {
    public string server;
    public string dbName;
    public SsbEnum ssbType;
    public SsbState state;
    public object updated;
    public SsbEventsArgs(string _server, string _dbName, SsbEnum _ssbType, SsbState _state, object _updated) {
      server = _server;
      dbName = _dbName;
      ssbType = _ssbType;
      state = _state;
      updated = _updated;
    }
  }
}
