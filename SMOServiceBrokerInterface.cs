using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Samples.SqlServer;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Broker;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;

namespace SsbAdmin {

  public class SSBIServiceListing {
    Database _db;
    SqlXml _listing;

    

    public SqlXml Listing {
      get { return _listing; }
      
    }
	

    public SSBIServiceListing(Database db) {
      _db = db;
    }

    public SqlXml Create(BrokerService serv) {
      return Create(serv, null);

    }

    public SqlXml LoadFromFile(string fileName) {

      XmlReader r = XmlReader.Create(fileName);
      _listing = new SqlXml(r);
      return null;
    }

    public SqlXml Create(BrokerService serv, string c) {
      try {
        SqlConnection conn = smo.CreateNewConnection(_db);
        conn.ChangeDatabase(_db.Name);
        SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = "sp_export_service_listing";
        cmd.CommandType = CommandType.StoredProcedure;
        //service name
        cmd.Parameters.Add("@service_name", SqlDbType.VarChar, 256);
        cmd.Parameters[0].Value = serv.Name;
        //cert - if any
        cmd.Parameters.Add("@cert_name", SqlDbType.VarChar, 256);
        cmd.Parameters[1].Value = c;
        
          
        cmd.Parameters.Add("@service_listing", SqlDbType.Xml);
        cmd.Parameters[2].Direction = ParameterDirection.InputOutput;
        cmd.Parameters[2].Value = new SqlXml();

        cmd.ExecuteNonQuery();
        //string list = cmd.Parameters[2].Value.ToString();

        _listing = (SqlXml)cmd.Parameters[2].Value;
        

      }
      catch (Exception ex) {
        throw;
      }

      return _listing;
      
    }

  }

  public class SSBIDatabase {
    Database _db;
    bool isTrustWorthy = false;

    public SSBIDatabase(Database db) {
      _db = db;
      isTrustWorthy = GetTw();
      
    }

    public SSBIDatabase(Database db, bool tw) {
      _db = db;
      IsTrustworthy = tw;
      isTrustWorthy = tw;

    }

    public Database DataBase {
      get { return _db; }
    }

    public bool IsTrustworthy {
      get { return isTrustWorthy; }
      set { SetTw(value); }
    }

    bool GetTw() {
      bool isTw;
      SqlConnection conn = smo.CreateNewConnection(_db);
      conn.ChangeDatabase("master");
      SqlCommand cmd = conn.CreateCommand();
      cmd.CommandText = string.Format("select is_trustworthy_on from sys.databases where name = '{0}'", _db.Name);
      isTw = (bool)cmd.ExecuteScalar();
      conn.Close();
      return isTw;

    }

    void SetTw(bool isTw) {
      string sqlCmd = "";
      SqlConnection conn = smo.CreateNewConnection(_db);
      conn.ChangeDatabase("master");
      SqlCommand cmd = conn.CreateCommand();
      if (isTw)
        sqlCmd = string.Format("ALTER DATABASE [{0}] SET TRUSTWORTHY ON", _db.Name);
      else
        sqlCmd = string.Format("ALTER DATABASE [{0}] SET TRUSTWORTHY OFF", _db.Name);

      cmd.CommandText = sqlCmd;
      cmd.ExecuteNonQuery();
      conn.Close();
      

    }



  }

  public class SSBIServer {
    SSBServerAuthentication _auth;
    string _uid;
    string _pwd;
    Server _server;
    string _name;
    bool _login = false;
    bool _local = false;
    bool _connectionAttempt  = false;
    bool _isDeleting = false;

    public SSBServerAuthentication Authentication {
      get { return _auth; }
      set { _auth = value; }
    }

    public string Name {
      get { return _name; }
      set { _name = value; }

    }

    public string UserId {
      get { return _uid; }
      set { _uid = value; }
    }

    public string Password {
      get { return _pwd; }
      set { _pwd = value; }
    }

    public Server SMOServer {
      get { return _server; }
      set { _server = value; }
    }
    public bool HasLoggedIn {
      get { return _login; }
      set { _login = value; }
    }
    public bool IsLocal {
      get { return _local; }
      set { _local = value; }
    }

    public bool IsTryingToConnect {
      get { return _connectionAttempt; }
      set { _connectionAttempt = value; }
    }

    public bool IsTryingToDelete {
      get { return _isDeleting; }
      set { _isDeleting = value; }
    }



    public SSBIServer() {

    }

    public SSBIServer(string n) :this(n, false) {
      
    }
    public SSBIServer(string n, bool isLoc) {
      _name = n;
      _local = isLoc;
    }

    public override string ToString() {
      return Name;
    }



  }
  

  public class SSBIConversationCollection : CollectionBase {

    public void Add(SSBIConversation cnv) {
      List.Add(cnv);
    }

    public void Remove(int index) {
      if (index > Count - 1 || index < 0) {
        System.Windows.Forms.MessageBox.Show("Index not valid!");
      }
      else {
        List.RemoveAt(index);
      }

    }

    public SSBIConversation Item(int Index) {

      return (SSBIConversation)List[Index];
      
    }

  }

  public class SSBICertificateCollection : CollectionBase {

    public void Add(Certificate cnv) {
      List.Add(cnv);
    }

    public SSBICertificateCollection(CertificateCollection c) {
      foreach (Certificate cert in c)
        if(!cert.Name.Contains("#"))
          this.Add(cert);

    }

    public void Remove(int index) {
      if (index > Count - 1 || index < 0) {
        System.Windows.Forms.MessageBox.Show("Index not valid!");
      }
      else {
        List.RemoveAt(index);
      }

    }

    public Certificate Item(int Index) {

      return (Certificate)List[Index];

    }

  }


  public class SSBIEndpoint {

    #region constructor
    public SSBIEndpoint(Endpoint ep) {
      _name = ep.Name;
      _state = ep.EndpointState;
      _isMsgFwd = ep.Payload.ServiceBroker.IsMessageForwardingEnabled;
      _fwdSize = ep.Payload.ServiceBroker.MessageForwardingSize;
      _auth = ep.Payload.ServiceBroker.EndpointAuthenticationOrder;
      _cert = ep.Payload.ServiceBroker.Certificate;
      _encrypt = ep.Payload.ServiceBroker.EndpointEncryption;
      _alg = ep.Payload.ServiceBroker.EndpointEncryptionAlgorithm;
      _port = ep.Protocol.Tcp.ListenerPort;
      _ep = ep;

    }
    #endregion

    #region fields
    string _name;
    EndpointState _state;
    bool _isMsgFwd;
    int _fwdSize;
    EndpointAuthenticationOrder _auth;
    string _cert;
    EndpointEncryption _encrypt;
    EndpointEncryptionAlgorithm _alg;
    int _port;
    Endpoint _ep;

    #endregion

    #region properties
    public string Name {
      get { return _name; }
    }

    public int Port {
      get { return _port; }
    }

    public EndpointState State {
      get { return _state; }
    }

    public bool IsMessageForwardingEnabled {
      get { return _isMsgFwd; }
    }

    public int MessageForwardSize {
      get { return _fwdSize; }
    }

    public EndpointAuthenticationOrder Authentication {
      get { return _auth; }
    }
    public string Certificate {
      get { return _cert; }
    }

    public EndpointEncryption Encryption {
      get { return _encrypt; }
    }
    public EndpointEncryptionAlgorithm Algorithm {
      get { return _alg; }
    }

    public Endpoint EndPoint {
      get { return _ep; }
    }

    #endregion

  }

  public class SSBIEndpointCollection : CollectionBase {

    public void Add(SSBIEndpoint cnv) {
      List.Add(cnv);
    }

    public SSBIEndpointCollection(EndpointCollection epc) {
      foreach (Endpoint ep in epc) {
        if (ep.EndpointType == EndpointType.ServiceBroker)
          this.Add(new SSBIEndpoint(ep));
      }
    }

    public void Remove(int index) {
      if (index > Count - 1 || index < 0) {
        System.Windows.Forms.MessageBox.Show("Index not valid!");
      }
      else {
        List.RemoveAt(index);
      }

    }

    public SSBIEndpoint Item(int Index) {

      return (SSBIEndpoint)List[Index];

    }

  }

  public class SSBIConversation {

    #region fields
    Guid _handle;
    Guid _grpId;
    string _fromService;
    string _toService;
    string _state;
    string _contract;
    bool _initiator;
    DateTime _lifeTime;
    Guid _farBrokerGuid;
    BrokerService _serv;
    
    
    #endregion

    #region properties

    public Guid Handle {
      get { return _handle; }
      set { _handle = value; }
    }

    public Guid GroupId {
      get { return _grpId; }
      set { _grpId = value; }
    }

    public string FromService {
      get { return _fromService; }
      set { _fromService = value; }
    }

    public string ToService {
      get { return _toService; }
      set { _toService = value; }
    }

    public string State {
      get { return _state; }
      set { _state = value; }
    }

    public string Contract {
      get { return _contract; }
      set { _contract = value; }
    }

    public bool IsInitiator {
      get { return _initiator; }
      set { _initiator = value; }
    }

    public DateTime Lifetime {
      get { return _lifeTime; }
      set { _lifeTime = value; }
    }

    public Guid FarBrokerGuid {
      get { return _farBrokerGuid; }
      set { _farBrokerGuid = value; }
    }

    public Database DBase {
      get { return _serv.Parent.Parent; }
      
    }

    public SSBIMessageCollection MessageCollection {
      get { return smo.GetMessageCollection(_serv, _serv.Parent.Parent, _handle, null); ; }
      //set { _msgColl = value; }
    }
   
    
    
    #endregion

    #region constructor

   /* @"select se.conversation_handle, se.conversation_group_id, s.name [service],  
               sc.name [contract], se.state_desc, se.far_service, se.is_initiator,
               se.lifetime,se.far_broker_instance, from sys.conversation_endpoints se 
                          join sys.services s on se.service_id = s.service_id 
                          join sys.service_contracts sc on se.service_contract_id = sc.service_contract_id 
                          where se.state in ('SO','SI','CO')";*/
    public SSBIConversation(BrokerService bServ, IDataRecord rec){
      _handle = rec.GetGuid(0);
      _grpId = rec.GetGuid(1);
      _fromService = rec.GetString(2);
      _toService = rec.GetString(5);
      _state = rec.GetString(4);
      _contract = rec.GetString(3);
      _initiator = rec.GetBoolean(6);
      _lifeTime = rec.GetDateTime(7);
      string o = rec[8].ToString();
      _farBrokerGuid = Guid.Empty;
      if(o != string.Empty)
        _farBrokerGuid = new Guid(rec.GetString(8));
      _serv = bServ;

      //_msgColl = smo.GetMessageCollection(bServ, bServ.Parent.Parent, _handle, null);
      
      
    }


    #endregion

    #region methods
    public override string ToString() {
      return this.Handle.ToString();
    }
    #endregion


  }

  public class SSBIMessage : Message {
    Guid _cnvHandle;
    public Guid ConversationHandle {
      get { return _cnvHandle; }
      set {_cnvHandle = value;}
      
    }

    string _service;
    public string ServiceName {
      get { return _service; }
      set { _service = value; }
    }

    
    

  }

  public class SSBIMessageCollection : CollectionBase {
    public void Add(SSBIMessage msg) {
      List.Add(msg);
    }

    public void Remove(int index) {
      if (index > Count - 1 || index < 0) {
        System.Windows.Forms.MessageBox.Show("Index not valid!");
      }
      else {
        List.RemoveAt(index);
      }

    }

    public Message Item(int Index) {

      return (Message)List[Index];

    }

  }

  public class SSBITxStatusCollection : CollectionBase {

    public SSBITxStatusCollection(SqlDataReader dr) {
      while (dr.Read())
        Add(new TxStatus(dr));
    }


    public void Add(TxStatus txStat) {
      List.Add(txStat);
    }

    public void Remove(int index) {
      if (index > Count - 1 || index < 0) {
        System.Windows.Forms.MessageBox.Show("Index not valid!");
      }
      else {
        List.RemoveAt(index);
      }

    }

    public TxStatus Item(int Index) {

      return (TxStatus)List[Index];

    }

  }

  /*
   select conversation_handle cnvHandle,
       to_service_name remServ,
       from_service_name [service],
       service_contract_name [contract],
       message_type_name [messageType],
       is_conversation_error [cnvError],
       is_end_of_dialog [endDialog],
       transmission_status [txStat]
from sys.transmission_queue*/

  public class TxStatus {
    Guid _cnvHandle;
    public Guid ConversationHandle {
      get { return _cnvHandle; }
      set { _cnvHandle = value; }

    }

    string _service;
    public string ServiceName {
      get { return _service; }
      set { _service = value; }
    }

    string _remService;
    public string RemoteService {
      get { return _remService; }
      set { _remService = value; }

    }

    string _contract;
    public string Contract {
      get { return _contract; }
      set { _contract = value; }

    }

    string _msgType;
    public string MessageType {
      get { return _msgType; }
      set { _msgType = value; }

    }

    bool _isCnvError;
    public bool IsConversationError {
      get { return _isCnvError; }
      set { _isCnvError = value; }
    }

    bool _isEndDialog;
    public bool IsEndDialog {
      get { return _isEndDialog; }
      set { _isEndDialog = value; }
    }

    string _txStatus;
    public string TransmissionStatus {
      get { return _txStatus; }
      set { _txStatus = value; }
    }

    public TxStatus(IDataRecord rec) {
      _cnvHandle = rec.GetGuid(0);
      _remService = rec.GetString(1);
      _service = rec.GetString(2);
      _contract = rec.GetString(3);
      _msgType = rec.GetString(4);
      _isCnvError = rec.GetBoolean(5);
      _isEndDialog = rec.GetBoolean(6);
      _txStatus = rec.GetString(7);
    }

  }

  
}
