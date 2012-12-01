----------------------------------------------------
-- 
-- Copyright (c) Microsoft Corporation.
--
----------------------------------------------------

/*
------------------------------------------------------
--
--			PLEASE READ CAREFULLY
--
-- The Service Broker Service Listing procedures
--	need to enable the Ole Automation Procedures feature
--	of SQL Server 2005. Please read carefully the SQL Server 2005 
--	Books Online 'Ole Automation Procedures Option' before enabling this 
--	option. Use the Surface Area Configuration tool or run the 
--	T-SQL script bellow to enable this feature.
--

exec sp_configure 'Show Advanced Options', 1
reconfigure

exec sp_configure 'Ole Automation Procedures', 1
reconfigure
*/

use master;
go

-----------------------------------------------------
--
-- Name: sp_get_certificate_blob
--
-- Purpose: Retrieves a certificate body as 
--   a varbinary(8000) variable
--
-- Notes: Saves the certificate as a temp file
--		and loads the file into the variable
--		using an ADODB.Stream object
--
-- Parameters:
--		@certname: the name of the certificate 
--		@certbody: output, the body of the certificate
--
-----------------------------------------------------
if object_id ('sp_get_certificate_blob') is not null
begin
	drop procedure sp_get_certificate_blob;
end
go

create procedure sp_get_certificate_blob
	@certname SYSNAME,
	@certbody VARBINARY(8000) OUTPUT
AS
BEGIN
DECLARE @fso int;
DECLARE @fsd_temp int;
DECLARE @stempfolder NVARCHAR (256);
DECLARE @adostream int;
DECLARE @hr int;
DECLARE @src varchar(255), @desc varchar(255)

DECLARE @tempFileName NVARCHAR(256);
DECLARE @tempCertFile NVARCHAR(256);
DECLARE @sql NVARCHAR(MAX);

--
--
EXEC @hr = sp_OACreate 'Scripting.FileSystemObject', @fso OUTPUT;
WHILE (@hr = 0)
BEGIN
	EXEC @hr = sp_OAMethod @fso, 'GetSpecialFolder', @fsd_temp OUTPUT, 2; -- TemporaryFolder
	IF (@hr <> 0) BREAK;
	EXEC @hr = sp_OAGetProperty @fsd_temp, 'Path', @stempfolder OUTPUT;
	IF (@hr <> 0) BREAK;
	EXEC @hr = sp_OAMethod @fso, 'GetTempName', @tempFileName OUTPUT;
	IF (@hr <> 0) BREAK;

	SELECT @tempCertFile = @stempfolder + N'\'+ @tempFileName;
	SELECT @sql = N'backup certificate ' + QUOTENAME(@certname) + N' to file = ' + QUOTENAME (@tempCertFile, '''');
	EXEC sp_executesql @sql;

	WHILE (@hr = 0)
	BEGIN
		EXEC @hr = sp_OACreate 'ADODB.Stream', @adostream OUTPUT;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Open';
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OASetProperty @adostream, 'Type', 1;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'LoadFromFile', NULL, @tempCertFile;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Read', @certbody OUTPUT;
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OAMethod @adostream, 'Close';
		IF (@hr <> 0) BREAK;

		EXEC sp_OADestroy @adostream;
		break;
	END

	EXEC sp_OAMethod @fso, 'DeleteFile', NULL, @tempCertFile;

	BREAK;
END

IF @hr <> 0
BEGIN
   EXEC sp_OAGetErrorInfo @fso, @src OUTPUT, @desc OUTPUT;
   RAISERROR (N'COM Error %x %s %s', 16, 1, @hr, @src, @desc);
   RETURN
END

END
GO

-----------------------------------------------------
--
-- Name: sp_create_certificate_from_blob
--
-- Purpose: creates a certificate from a body 
--   as a varbinary(8000) variable
--
-- Notes: Saves the body into a temp file
--		and constructs dynamic T-SQL to load the 
--		certificate from this file
--
-- Parameters:
--		@certname: the name of the certificate to be created
--		@certbody: the body of the certificate
--
-----------------------------------------------------
if object_id ('sp_create_certificate_from_blob') is not null
begin
	drop procedure sp_create_certificate_from_blob;
end
go

create procedure sp_create_certificate_from_blob
	@certname SYSNAME,
	@certbody VARBINARY(8000)
AS
BEGIN
DECLARE @fso int;
DECLARE @fsd_temp int;
DECLARE @stempfolder NVARCHAR (256);
DECLARE @adostream int;
DECLARE @hr int;
DECLARE @src varchar(255), @desc varchar(255)

DECLARE @tempFileName NVARCHAR(256);
DECLARE @tempCertFile NVARCHAR(256);
DECLARE @sql NVARCHAR(MAX);

if (@certbody is NULL)
begin
   RAISERROR (N'@certbody is NULL', 16, 2);
end


--
--
EXEC @hr = sp_OACreate 'Scripting.FileSystemObject', @fso OUTPUT;
WHILE (@hr = 0)
BEGIN
	EXEC @hr = sp_OAMethod @fso, 'GetSpecialFolder', @fsd_temp OUTPUT, 2; -- TemporaryFolder
	IF (@hr <> 0) BREAK;
	EXEC @hr = sp_OAGetProperty @fsd_temp, 'Path', @stempfolder OUTPUT;
	IF (@hr <> 0) BREAK;
	EXEC @hr = sp_OAMethod @fso, 'GetTempName', @tempFileName OUTPUT;
	IF (@hr <> 0) BREAK;

	SELECT @tempCertFile = @stempfolder + N'\'+ @tempFileName;

	WHILE (@hr = 0)
	BEGIN
		EXEC @hr = sp_OACreate 'ADODB.Stream', @adostream OUTPUT;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Open';
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OASetProperty @adostream, 'Type', 1;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Write', NULL, @certbody;
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OAMethod @adostream, 'SaveToFile', NULL, @tempCertFile;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Close';
		IF (@hr <> 0) BREAK;

		EXEC sp_OADestroy @adostream;
		break;
	END

	IF (@hr = 0)
	BEGIN
		SELECT @sql = N'create certificate ' + QUOTENAME(@certname) + N' from file = ' + QUOTENAME (@tempCertFile, '''');
		print @sql;
		EXEC sp_executesql @sql;
	
		EXEC sp_OAMethod @fso, 'DeleteFile', NULL, @tempCertFile;
	END

	BREAK;
END

IF @hr <> 0
BEGIN
   EXEC sp_OAGetErrorInfo @fso, @src OUTPUT, @desc OUTPUT;
   RAISERROR (N'COM Error %x %s %s', 16, 1, @hr, @src, @desc);
   RETURN
END

END
GO

-----------------------------------------------------
--
-- Name: sp_import_service_listing_grant_connect_on_endpoint
--
-- Purpose: imports a proxy user for a service host machine
--			and grants CONNECT permission on the broker endpoint
--
-- Notes: Both initiator service administrator and target service 
--		administrator need to do this step. The initator service listing needs
--		to be imported by the target and vice-versa.This step needs to be done 
--		only once for each pair of machines. A proxy login will be created
--		to represent the peer service host machine. This login will be
--		granted CONNECT permission on the broker endpoint.
--		Only CERTIFICATE authentication option is supported by this procedure. 
--
-- Parameters:
--		@service_listing: the service listing
--		@loginpassword: a password is required for the proxy login
--		@proxyinstanceuser: the local name for the proxy user
--		@dropexisting: flag to control the dropping of already
--			existing proxy login, user and certificate
--
-----------------------------------------------------
if object_id('sp_import_service_listing_grant_connect_on_endpoint') is not null
	drop procedure sp_import_service_listing_grant_connect_on_endpoint;
go

create procedure sp_import_service_listing_grant_connect_on_endpoint
	(@service_listing as xml,
	@loginpassword nvarchar(max),
	@proxyinstanceuser sysname = NULL,
	@dropexisting as bit = 1)
as
begin
	declare @handle int;
	declare @issuername nvarchar(max);
	declare @serialnumber nvarchar(max);
	declare @endpoint sysname;
	declare @blob varbinary(max);
	declare @sql nvarchar(max);

	if (db_name () <> 'master')
	begin
		raiserror (N'This stored proc can only be run in master database.', 16, 1);
		return -1;
	end

	select @endpoint = name from sys.service_broker_endpoints;
	if (@endpoint is null)
	begin
		raiserror (N'There is no broker endpoint configured in the instance.', 16, 1);
		return -1;
	end

	begin transaction;

	declare crsCertificates cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		select n.value('@issuer-name', 'nvarchar(max)'), n.value('@serial-number', 'nvarchar(max)'),
			n.value('blob[1]', 'varbinary(max)')
		from @service_listing.nodes('/definition/endpoint/certificate') t(n);

	open crsCertificates
	fetch next from crsCertificates into @issuername, @serialnumber, @blob;
	while @@fetch_status = 0
	begin
		declare @username sysname;
		select @username = ISNULL(@proxyinstanceuser, @issuername +': ' + @serialnumber);

		if exists (select * from master.sys.certificates where issuer_name = @issuername and cert_serial_number = @serialnumber and @dropexisting = 1)
		begin
			declare @certname sysname;
			select @certname = name from sys.certificates where issuer_name = @issuername and cert_serial_number = @serialnumber;
			select @sql = 'drop certificate ' + quotename(@certname);
			exec sp_executesql @sql;
		end
		
		if  exists (Select * from sys.server_principals where name = @username and @dropexisting = 1)
		begin
			select @sql = N'drop login ' + quotename(@username);
			exec sp_executesql @sql;
		end

		if  exists (Select * from sys.database_principals where name = @username and @dropexisting = 1)
		begin
			select @sql = N'drop user ' + quotename(@username);
			exec sp_executesql @sql;
		end
		
		select @sql = N'create login ' + quotename(@username) + ' with password = ' + quotename(@loginpassword, '''');
		print @sql;
		exec sp_executesql @sql;

		select @sql = N'create user ' + quotename(@username);
		print @sql;
		exec sp_executesql @sql;

		exec sp_create_certificate_from_blob @username, @blob;

		select @sql = N'alter authorization on certificate::' + quotename(@username) + N' to ' + quotename(@username);
		print @sql;
		exec sp_executesql @sql;
	
		select @sql = N'grant connect on endpoint::' + quotename (@endpoint) + N' to ' + quotename(@username);
		print @sql;
		exec sp_executesql @sql;

		fetch next from crsCertificates into @issuername, @serialnumber, @blob;
	end

	close crsCertificates;
	deallocate crsCertificates;

	commit;
end
go