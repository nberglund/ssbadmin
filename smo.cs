#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Smo.;
using Microsoft.SqlServer.Management.Common;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo.Broker;
using Microsoft.Samples.SqlServer;
using System.Collections;
using Microsoft.Win32;
using System.Windows.Forms;

#endregion

namespace SsbAdmin {

  internal delegate void DelObjectDel(SsbEnum sType, object smoObj);
  internal delegate Server ConnectDelegate(SSBIServer serv);

  class smo {
    public smo() {
      

    }

   
    static string sqlConversation = @"select se.conversation_handle, 
                          se.conversation_group_id, 
                          s.name [service],  
                          sc.name [contract], 
                          se.state_desc, 
                          se.far_service,
                          se.is_initiator,
                          se.lifetime,
                          se.far_broker_instance,
                          s.service_id 
                          from sys.conversation_endpoints se 
                          join sys.services s on se.service_id = s.service_id 
                          join sys.service_contracts sc on se.service_contract_id = sc.service_contract_id"; 
                          //where se.state in ('SO','SI','CO')";

    public static Server CreateServer(string servername, string uid, string pwd) {
      ServerConnection conn = new ServerConnection();
      Server srv = null;
      conn.ServerInstance = servername;
      if (uid == null)
        conn.LoginSecure = true;
      else {
        conn.LoginSecure = false;
        conn.Login = uid;
        conn.Password = pwd;
      }
      try{
        conn.Connect();
        srv = new Server(conn);
      }
 
      catch (ConnectionFailureException e) {
        smo.ShowException(e);
      }

      return srv;

    }

    public static Server CreateServer2(SSBIServer serv) {
      ServerConnection conn = new ServerConnection();
      Server srv = null;
      //conn.ServerInstance
      conn.ServerInstance = serv.Name;
      if(serv.Authentication == SSBServerAuthentication.Integrated)
        conn.LoginSecure = true;
      else {
        conn.LoginSecure = false;
        conn.Login = serv.UserId;
        conn.Password = serv.Password;
      }
      try {
        conn.Connect();
        srv = new Server(conn);
        serv.SMOServer = srv;
        return srv;
      }

      catch (ConnectionFailureException e) {
        throw e;
        
      }

      

      finally {
        if (srv != null && srv.ConnectionContext.SqlConnectionObject.State == ConnectionState.Open)
          srv.ConnectionContext.Disconnect();

      }

    }

    internal static ArrayList GetServers(SsbServerLocation loc) {
            
      ArrayList retServers = new ArrayList();
      ArrayList al = new ArrayList();
      
      switch (loc) {

        case SsbServerLocation.Local:
          GetLocalServers(ref al);
          break;

        case SsbServerLocation.Network:
          GetNetWorkedServers(ref al);
          break;
      }

      IEnumerator en = al.GetEnumerator();
      while (en.MoveNext()) {
        retServers.Add(new SSBIServer(en.Current.ToString()));
      }

      return retServers;
    }

    public static void GetNetWorkedServers(ref ArrayList al) {
      string servName;
      string instName;
      string verNo;
      SqlDataSourceEnumerator se = SqlDataSourceEnumerator.Instance;
      DataTable dt = se.GetDataSources();
      foreach (DataRow row in dt.Rows) {
        //get major version
        verNo = row["version"].ToString();
        if (CheckVersion(verNo)) {
          instName = row["InstanceName"].ToString();
          servName = row["ServerName"].ToString();
          if (instName != string.Empty)
            servName = servName + "\\" + instName;

          if (!al.Contains(servName))
            al.Add(servName);

        }
      }
    }

    public static void GetLocalServers(ref ArrayList al) {
      string machine = System.Environment.MachineName;
      string sqlServer = "";
      string regRoot = @"SOFTWARE\Microsoft\Microsoft SQL Server";
      String[] instances = null;
      RegistryKey rk = null;
      rk = Registry.LocalMachine.OpenSubKey(regRoot);
      instances = (String[])rk.GetValue("InstalledInstances");
      if (instances.Length > 0) {
        //first check Instance Names
        RegistryKey rkSql = rk.OpenSubKey("Instance Names\\SQL");
        foreach (string inst in (string[])rkSql.GetValueNames()) {
          string mssqlval = (string)rkSql.GetValue(inst);
          if (mssqlval != string.Empty) {
            if (CheckRegForVersion(rk, mssqlval)) {
              if (inst == "MSSQLSERVER")
                sqlServer = machine;
              else
                sqlServer = machine + "\\" + inst;

              if(!al.Contains(sqlServer))
                al.Add(sqlServer);
            }
          }
        }
        rkSql.Close();
        //having gone through the entries in instance names
        //loop through the instances (to be on the safe side)
        foreach (String element in instances) {
          //"MSSQLSERVER" should have been picked up above
          if (element != "MSSQLSERVER") {
            sqlServer = machine + "\\" + element;
            if (!al.Contains(sqlServer)) {
              if (CheckRegForVersion(rk, element))
                al.Add(sqlServer);
            }
          }
        }
      }
    }

    static bool CheckRegForVersion(RegistryKey rk, string element) {
      RegistryKey rkMSSql = rk.OpenSubKey(element + "\\MSSQLServer\\CurrentVersion");
      string verNo = (string)rkMSSql.GetValue("CurrentVersion");
      return CheckVersion(verNo);
    }

    static bool CheckVersion(string verNo) {
      bool ret = false;
      int firstDot = verNo.IndexOf('.', 0);
      if (firstDot != -1) {
        verNo = verNo.Substring(0, firstDot);
        if (int.Parse(verNo) > 8)
          ret = true;
      }
      return ret;
    }
   
    internal static ICollection GetSSBObjects2(SSBIServer serv, Database db, SsbEnum index) {
      ICollection ssbColl = null;
      ServiceBroker sb = null;
      Server s = null;
      try {
        s = serv.SMOServer;
        if(s == null)
          s = CreateServer2(serv);


        if (db != null) {
          db = s.Databases[db.Name];
          sb = db.ServiceBroker;
        }



        switch (index) {
          case SsbEnum.Server:
            ssbColl = s.Databases;
            break;
          case SsbEnum.Database:
            ssbColl = s.Databases;
            break;

          case SsbEnum.MessageType:
            ssbColl = sb.MessageTypes;
            break;
          case SsbEnum.Contract:
            ssbColl = sb.ServiceContracts;
            break;
          case SsbEnum.Queu:
            ssbColl = sb.Queues;
            
            break;
          case SsbEnum.Service:
            ssbColl = sb.Services;
            break;
          case SsbEnum.Route:
            ssbColl = sb.Routes;
            break;
          case SsbEnum.RemoteBinding:
            ssbColl = sb.RemoteServiceBindings;
            break;

          case SsbEnum.Conversation:
            ssbColl = GetConversationCollection(db, null);

            break;
        }
        return ssbColl;

      }
      catch (Exception e) {
        throw e;
      }
      finally {

        if (s != null)
          s.ConnectionContext.Disconnect();
      }

      
    }


    internal static void DeploySsbObj(object obj, string svrName, string dbName, SsbEnum ssbType, bool isEdit) {
      Server svr = CreateServer(svrName, null, null);
      Database db = svr.Databases[dbName];
      ServiceBroker sb = db.ServiceBroker;
      MessageType mt = null;
      ServiceContract sc = null;
      ServiceQueue q = null;
      BrokerService serv = null;
      ServiceRoute rt = null;
      RemoteServiceBinding bind = null;

      try {
        switch (ssbType) {
          case SsbEnum.MessageType:
            MessageType mtNew = new MessageType();
            mtNew.Parent = sb;
            mt = (MessageType)obj;
            mtNew.Name = mt.Name;
            mtNew.MessageTypeValidation = mt.MessageTypeValidation;
            if (mt.MessageTypeValidation == MessageTypeValidation.XmlSchemaCollection)
              mtNew.ValidationXmlSchemaCollection = mt.ValidationXmlSchemaCollection;

            if (isEdit)
              mtNew.Alter();
            else
              mtNew.Create();

            break;

          case SsbEnum.Contract:
            ServiceContract scNew = new ServiceContract();
            sc = (ServiceContract)obj;
            scNew.Parent = sb;
            scNew.Name = sc.Name;
            foreach (MessageTypeMapping mtm in sc.MessageTypeMappings) {
              if (!sb.MessageTypes.Contains(mtm.Name)) {
                ServiceBroker sbParent = sc.Parent;
                MessageType mtp = sbParent.MessageTypes[mtm.Name];
                DeploySsbObj(mtp, svrName, dbName, SsbEnum.MessageType, false);
              }

              MessageTypeMapping mtmNew = new MessageTypeMapping();
              mtmNew.Name = mtm.Name;
              mtmNew.Parent = scNew;
              mtmNew.MessageSource = mtm.MessageSource;
              scNew.MessageTypeMappings.Add(mtmNew);

            }
            
            if (isEdit)
              scNew.Alter();
            else
              scNew.Create();

            break;

          case SsbEnum.Queu:
            q = (ServiceQueue)obj;
            q.Parent = sb;

            if (isEdit)
              q.Alter();
            else
              q.Create();

            break;

          case SsbEnum.Service:
            serv = (BrokerService)obj;
            serv.Parent = sb;

            if (isEdit)
              serv.Alter();
            else
              serv.Create();

            break;

          case SsbEnum.Route:
            rt = (ServiceRoute)obj;
            rt.Parent = sb;

            if (isEdit)
              rt.Alter();
            else
              rt.Create();

            break;

          case SsbEnum.RemoteBinding:
            bind = (RemoteServiceBinding)obj;
            bind.Parent = sb;

            if (isEdit)
              bind.Alter();
            else
              bind.Create();

            break;

        }
      }
      catch (FailedOperationException e) {
        string err = string.Format("{0}", e.InnerException);
        //throw;
      }
      catch (Exception ex) {
        string errx = string.Format("{0}", ex.InnerException);

      }

      finally {
        svr.ConnectionContext.Disconnect();
      }

    }


       

    internal static BrokerService CreateSMOService(Database db, string name) {
      ServiceBroker sb = db.ServiceBroker;
      BrokerService bserv = null;
      bserv = sb.Services[name];
      
      //svr.ConnectionContext.Disconnect();
      return bserv;
    }

    internal static void CreateUserWithNoLogin(User usr) {
      string certUser = "";
      SqlConnection conn = null;
      try {
        conn = smo.CreateNewConnection(usr.Parent);
        conn.ChangeDatabase(usr.Parent.Name);
        SqlCommand cmd = conn.CreateCommand();
        string cmdText = string.Format("CREATE USER [{0}] WITHOUT LOGIN", usr.Name);
        //kludge - have to check for certificate somehow
        try {
          if (usr.Certificate != string.Empty)
            certUser = string.Format(" FOR CERTIFICATE {0}", usr.Certificate);
        }
        catch { }

        cmd.CommandText = cmdText + certUser;
        cmd.ExecuteNonQuery();
      }

      finally {
        if (conn != null) {
          if (conn.State == ConnectionState.Open)
            conn.Dispose();

        }

      }
      
        



    }

    internal static void DeleteObject(SsbEnum sType, object smoObj) {

         switch (sType) {

            case SsbEnum.Database:
             Database db = null;
              if(smoObj.GetType() == typeof(SSBIDatabase))
                db = ((SSBIDatabase)smoObj).DataBase;
              else if (smoObj.GetType() == typeof(Database))
                db = (Database)smoObj;
              
              db.Drop();
              break;

            case SsbEnum.MessageType:
              MessageType mt = (MessageType)smoObj;
              mt.Drop();
              break;
            case SsbEnum.Contract:
              ServiceContract sc = (ServiceContract)smoObj;
              sc.Drop();
              break;
            case SsbEnum.Queu:
              ServiceQueue sq = (ServiceQueue)smoObj;
              sq.Drop();
              break;
            case SsbEnum.Service:
              BrokerService bs = (BrokerService)smoObj;
              bs.Drop();
              break;
            case SsbEnum.Route:
              ServiceRoute sr = (ServiceRoute)smoObj;
              sr.Drop();
              break;
            case SsbEnum.RemoteBinding:
              RemoteServiceBinding rsb = (RemoteServiceBinding)smoObj;
              rsb.Drop();
              break;
            case SsbEnum.Conversation:
              SSBIConversation cnv = (SSBIConversation)smoObj;
              smo.EndConversation(cnv);
              break;

            case SsbEnum.EndPoint:
              Endpoint ep = (Endpoint)smoObj;
              ep.Drop();
              break;

            case SsbEnum.Certificate:
              Certificate cert = (Certificate)smoObj;
              cert.Drop();
              break;
          }


    }



    internal static object GetObject(Database db, string objName, SsbEnum index) {
      object smob = null;
      Server serv = null;
      Database db2 = null;
      try {
        serv = CreateServer(db.Parent.Name, null, null);
        //serv = CreateServer2(db.Parent.Name, null, null);
        db2 = serv.Databases[db.Name];
        ServiceBroker sb = serv.Databases[db2.Name].ServiceBroker;

        switch (index) {

          case SsbEnum.Database:

            smob = serv.Databases[objName];
            //serv.ConnectionContext.Disconnect();
            break;

          case SsbEnum.MessageType:
            smob = sb.MessageTypes[objName];
            break;

          case SsbEnum.Contract:
            smob = sb.ServiceContracts[objName];
            break;

          case SsbEnum.Service:
            smob = sb.Services[objName];
            break;

          case SsbEnum.Queu:
            smob = sb.Queues[objName];
            break;

          case SsbEnum.Route:
            smob = sb.Routes[objName];
            break;

          case SsbEnum.RemoteBinding:
            smob = sb.RemoteServiceBindings[objName];
            break;

          case SsbEnum.Conversation:
            smob = smo.GetConversationCollection(db, null, new Guid(objName)).Item(0);
            break;

        }

        if (smob == null) {
          string errMsg = string.Format("Can not retrieve {0}: {1}.\nIt may have been dropped/deleted.", index.ToString(), objName);
          throw new ApplicationException(errMsg);
        }

        return smob;
      }

      catch(NullReferenceException) {
        if (serv != null && db2 == null) {
          throw new ApplicationException(string.Format("Can not connect to database: {0}.\nIt may have been dropped/deleted.", db.Name));

        }

        return null;

      }
      finally {
        serv.ConnectionContext.Disconnect();
        
      }
    }

    internal static object GetObject2(SSBIServer s, Database db, string objName, SsbEnum index) {
      object smob = null;
      Server serv = null;
      Database db2 = null;
      ServiceBroker sb = null;
      try {
        
        serv = CreateServer2(s);
        s.SMOServer = serv;
        if (db != null) {
          db2 = serv.Databases[db.Name];
          sb = serv.Databases[db2.Name].ServiceBroker;
        }

        switch (index) {

          case SsbEnum.Server:
            smob = s;
            break;

          case SsbEnum.Database:
            db2 = serv.Databases[objName];
            smob = new SSBIDatabase(db2);
            //serv.ConnectionContext.Disconnect();
            break;

          case SsbEnum.MessageType:
            smob = sb.MessageTypes[objName];
            break;

          case SsbEnum.Contract:
            smob = sb.ServiceContracts[objName];
            break;

          case SsbEnum.Service:
            smob = sb.Services[objName];
            break;

          case SsbEnum.Queu:
            if(objName == "sys.transmission_queue") {
              ServiceQueue txQ = new ServiceQueue();
              txQ.Name = "sys.transmission_queue";
              txQ.Parent = sb;
              txQ.Schema = "sys";
              smob = txQ;
            }
            else
              smob = sb.Queues[objName];
            break;

          case SsbEnum.Route:
            smob = sb.Routes[objName];
            break;

          case SsbEnum.RemoteBinding:
            smob = sb.RemoteServiceBindings[objName];
            break;

          case SsbEnum.Conversation:
            smob = smo.GetConversationCollection(db, null, new Guid(objName)).Item(0);
            break;

        }

        if (smob == null) {
          string errMsg = string.Format("Can not retrieve {0}: {1}.\nIt may have been dropped/deleted.", index.ToString(), objName);
          throw new ApplicationException(errMsg);
        }

        return smob;
      }

      catch (NullReferenceException) {
        if (serv != null && db2 == null) {
          throw new ApplicationException(string.Format("Can not connect to database: {0}.\nIt may have been dropped/deleted.", db.Name));

        }

        return null;

      }

      catch (ConnectionFailureException e) {
        throw e;
        return null;
      }
      finally {
        serv.ConnectionContext.Disconnect();

      }
    }





    internal static Service GetSSBIService(Database db, string servName) {
      BrokerService bServ = db.ServiceBroker.Services[servName];
      
      
      SqlConnection conn = CreateNewConnection(db);

      if (conn.Database != bServ.Parent.Parent.Name)
        conn.ChangeDatabase(bServ.Parent.Parent.Name);

      return new Service(bServ.Name, false, conn);

    }

    internal static SSBIConversationCollection GetConversationCollection(Database db, BrokerService bServ) {


      return GetConversationCollection(db, bServ, Guid.Empty);


    }

    internal static SSBIConversationCollection GetConversationCollection(Database db, BrokerService bServ, Guid handle) {
      SqlConnection conn = null;
      int servid;
      conn = CreateNewConnection(db);

      SqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = sqlConversation;

      if (bServ != null) {
        servid = bServ.ID;
        cmd.CommandText += " and se.service_id = @id";
        cmd.Parameters.Add("@id", SqlDbType.Int, 4);
        cmd.Parameters["@id"].Value = servid;
      }

      if (handle != Guid.Empty) {
        cmd.CommandText += " and se.conversation_handle = @h";
        cmd.Parameters.Add("@h", SqlDbType.UniqueIdentifier);
        cmd.Parameters["@h"].Value = handle;
      }

      if (conn.Database != db.Name)
        conn.ChangeDatabase(db.Name);
      
      SqlDataReader dr = cmd.ExecuteReader();
      SSBIConversationCollection scColl = new SSBIConversationCollection();
      while (dr.Read()) {
        string srvName = dr["service"].ToString();
        if(bServ == null)
          bServ = CreateSMOService(db, srvName);
        SSBIConversation sc = new SSBIConversation(bServ, dr);
        scColl.Add(sc);
      }
      
      conn.Close();

      return scColl;
    }

    

    internal static void EndConversation(SSBIConversation conv) {


      SqlConnection conn = CreateNewConnection(conv.DBase);
      SqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = "End conversation @h";

      cmd.Parameters.Add("@h", SqlDbType.UniqueIdentifier);
      cmd.Parameters[0].Value = conv.Handle;

      if (conn.Database != conv.DBase.Name)
        conn.ChangeDatabase(conv.DBase.Name);

      cmd.ExecuteNonQuery();
      
      conn.Close();

    }

    internal static SSBITxStatusCollection GetStatuses(Database db) {
      string sqlConn = string.Format("server={0};database={1};Integrated Security='SSPI';", db.Parent.Name, db.Name);

      string sqlExec = @"select conversation_handle cnvHandle,
                                to_service_name remServ,
                                from_service_name [service],
                                service_contract_name [contract],
                                message_type_name [messageType],
                                is_conversation_error [cnvError],
                                is_end_of_dialog [endDialog],
                                transmission_status [txStat]
                                from sys.transmission_queue";

      SqlConnection conn = CreateNewConnection(db);
      SqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = sqlExec;

      if (conn.Database != db.Name)
        conn.ChangeDatabase(db.Name);

      return new SSBITxStatusCollection(cmd.ExecuteReader());

    }

    internal static SSBIMessageCollection GetMessageCollection(BrokerService bServ, Database db, Guid handle, ServiceQueue q) {
      string sqlConn = string.Format("server={0};database={1};Integrated Security='SSPI';", db.Parent.Name, db.Name);
      string schema;
      string qName;

      string sqlExec = @"select q.conversation_group_id, 
                                       q.conversation_handle, 
				                               q.message_sequence_number, 
                                       q.service_name,
                                       se.far_service, 
                                       q.service_contract_name, 
				                               q.message_type_name, 
                                       q.validation, message_body
                                       from {0}.{1} q
                                       join sys.conversation_endpoints se
                                       on q.conversation_handle = se.conversation_handle";
                                       //where se.state in ('SO','SI','CO')";
                                       
      SqlConnection conn = CreateNewConnection(db);
      SqlCommand cmd = conn.CreateCommand();

      if (bServ == null && q != null) {
        schema = q.Schema;
        qName = q.Name;
      }
      else {
        schema = bServ.QueueSchema;
        qName = bServ.QueueName;
        sqlExec += " and q.service_id = @id";
        int id = bServ.ID;
        cmd.Parameters.Add("@id", SqlDbType.Int, 4);
        cmd.Parameters["@id"].Value = id;
      }

      if (handle != Guid.Empty) {
        sqlExec += " and q.conversation_handle = @h";
        cmd.Parameters.Add("@h", SqlDbType.UniqueIdentifier);
        cmd.Parameters["@h"].Value = handle;
      }
      
     
      sqlExec = string.Format(sqlExec, schema, qName);
      
     cmd.CommandText = sqlExec;

      if (conn.Database != db.Name)
        conn.ChangeDatabase(db.Name);

      SqlDataReader reader = cmd.ExecuteReader();
      SSBIMessageCollection scColl = new SSBIMessageCollection();
      while (reader.Read()) {
        Guid cnvHandle;
        SSBIMessage msg = new SSBIMessage();
        msg.ConversationGroupId = reader.GetGuid(0);
        cnvHandle = reader.GetGuid(1);
        msg.ConversationHandle = cnvHandle;
        //msg.Conversation = new Conversation(srv, cnvHandle);
        msg.SequenceNumber = reader.GetInt64(2);
        msg.RemoteServiceName = reader.GetString(4);
        msg.ServiceName = reader.GetString(3);
        msg.ContractName = reader.GetString(5);
        msg.Type = reader.GetString(6);
        msg.Validation = reader.GetString(7);
        if (!reader.IsDBNull(8)) {
          SqlBytes sb = reader.GetSqlBytes(8);
          msg.Body = sb.Stream;
          
        }
        else
          msg.Body = null;

        scColl.Add(msg);
      }

      conn.Close();

      return scColl;
    }

    internal static SqlConnection CreateNewConnection(Database db) {
      //create a new connection
      SqlConnection conn = new SqlConnection();
      SqlConnection oldConn = null;
      oldConn = db.Parent.ConnectionContext.SqlConnectionObject;
      conn.ConnectionString = oldConn.ConnectionString;
      conn.Open();

      if (conn.Database != db.Name)
        conn.ChangeDatabase(db.Name);

      
      return conn;


    }

    public static void ShowException(Exception e) {
      StringBuilder errMsg = new StringBuilder();
      errMsg.Append(e.Message + "\n");
      if (e.InnerException != null) {
        errMsg.Append(e.InnerException.Message + "\n");
        if (e.InnerException.InnerException != null)
          errMsg.Append(e.InnerException.InnerException.Message + "\n");

      }

      
      Cursor.Current = Cursors.Default;

      MessageBox.Show(errMsg.ToString(), "Error");
 
     

    }


  }

  public enum SsbEnum {
    None = -1,
    MessageType = 0,
    Contract = 1,
    Queu = 2,
    Service = 3,
    Route = 4,
    RemoteBinding = 5,
    Conversation = 6,
    Message = 7,
    Database = 10,
    Server = 12,
    Login = 100,
    EndPoint = 120,
    Certificate = 140,
    User = 160,
    CreateListing = 300,
    ImportListing = 340,
    SecureService = 380

  }

  public enum SsbState {
    New,
    Edited,
    Cancelled

  }

  /// <summary>
  /// Enumeration for location of servers:
  /// Local - instances on the local machine
  /// Registered - instances registered on the machine
  /// Network - servers available on the network
  /// </summary>
  public enum SsbServerLocation {
    Local,
    Registered,
    Network
  }

  /// <summary>
  /// Enumeration indicating how to authenticate against this server
  /// </summary>
  public enum SSBServerAuthentication {
    Integrated = 0,
    Mixed = 1
  }
}
