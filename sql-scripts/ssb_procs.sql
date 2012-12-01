-- =============================================
-- Script file for admin stored procedures for
-- SQL Server Service Broker
-- =============================================

-- Drop stored procedures if it already exists
IF EXISTS (
  SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES 
   WHERE SPECIFIC_SCHEMA = N'dbo'
     AND SPECIFIC_NAME = N'ssb_CheckDbProps' 
)
   DROP PROCEDURE dbo.ssb_CheckDbProps;
GO

CREATE PROCEDURE dbo.ssb_CheckDbProps
                      @dbName nvarchar(128) = null,
                      @brkrEnabled int out,
                      @mstrKey int out,
                      @trstWorthy int out,
                      @endPoint int out,
                      @db nvarchar(128) out,
                      @dbId int out
	
AS
	
if @db is null
  set @dbId = db_id();
else
  set @dbId = db_id(@db);

 
-- check that we are broker enabled, 
-- if we have a master key 
-- and if we are trustworthy
select @brkrEnabled = is_broker_enabled,
       @mstrKey = is_master_key_encrypted_by_server,
       @trstWorthy = is_trustworthy_on
from sys.databases
where database_id = @dbId

-- check if we have an endpoint
select @endpoint = count(*) 
from sys.endpoints
where type = 3; -- type 3 = SERVICE_BROKER

GO

