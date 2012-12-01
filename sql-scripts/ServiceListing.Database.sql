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
-- Name: sp_save_service_listing
--
-- Purpose: saves a service listing to a file
--
-- Notes: Size of service listing is limited to 8000 bytes
--		Use sp_export_service_listing to create the service listing
--		The file is overwritten if exists
--
-- Parameters:
--		@service_listing: the service listing 
--		@filename: the fully qualified name of the file 
--
-----------------------------------------------------
if object_id ('sp_save_service_listing') is not null
begin
	drop procedure sp_save_service_listing;
end
go

create procedure sp_save_service_listing
	@service_listing as xml,
	@filename as nvarchar(256)
AS
BEGIN
DECLARE @fso int;
DECLARE @fsd_temp int;
DECLARE @stempfolder NVARCHAR (256);
DECLARE @adostream int;
DECLARE @hr int;
DECLARE @src varchar(255), @desc varchar(255)

DECLARE @file varbinary(8000);
SELECT @file = cast(@service_listing as varbinary(8000));

--
--
EXEC @hr = sp_OACreate 'ADODB.Stream', @adostream OUTPUT;
WHILE (@hr = 0)
BEGIN
		EXEC @hr = sp_OAMethod @adostream, 'Open';
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OASetProperty @adostream, 'Type', 1;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Write', NULL, @file;
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OAMethod @adostream, 'SaveToFile', NULL, @filename, 2;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Close';
		IF (@hr <> 0) BREAK;

		EXEC sp_OADestroy @adostream;
		break;

	BREAK;
END

IF @hr <> 0
BEGIN
   EXEC sp_OAGetErrorInfo @adostream, @src OUTPUT, @desc OUTPUT;
   RAISERROR (N'COM Error %x %s %s', 16, 1, @hr, @src, @desc);
   RETURN
END

END
GO

-----------------------------------------------------
--
-- Name: sp_load_service_listing
--
-- Purpose: loads a service listing to a file
--
-- Notes: Size of service listing is limited to 8000 bytes
--
-- Parameters:
--		@filename: the fully qualified name of the file 
--		@service_listing: output, the service listing 
--
-----------------------------------------------------
if object_id ('sp_load_service_listing') is not null
begin
	drop procedure sp_load_service_listing;
end
go

create procedure sp_load_service_listing
	(@filename as nvarchar(256),
	@service_listing as xml output)
AS
BEGIN
DECLARE @adostream int;
DECLARE @hr int;
DECLARE @file varbinary(8000);
DECLARE @src varchar(255), @desc varchar(255)

--
--
EXEC @hr = sp_OACreate 'ADODB.Stream', @adostream OUTPUT;
WHILE (@hr = 0)
BEGIN

		EXEC @hr = sp_OAMethod @adostream, 'Open';
		IF (@hr <> 0) BREAK;
		
		EXEC @hr = sp_OASetProperty @adostream, 'Type', 1;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'LoadFromFile', NULL, @filename;
		IF (@hr <> 0) BREAK;

		EXEC @hr = sp_OAMethod @adostream, 'Read', @file OUTPUT;
		IF (@hr <> 0) BREAK;
		
		SELECT @service_listing = @file;

		EXEC @hr = sp_OAMethod @adostream, 'Close';
		IF (@hr <> 0) BREAK;

		EXEC sp_OADestroy @adostream;
		break;

END

IF @hr <> 0
BEGIN
   EXEC sp_OAGetErrorInfo @adostream, @src OUTPUT, @desc OUTPUT;
   RAISERROR (N'COM Error %x %s %s', 16, 1, @hr, @src, @desc);
   RETURN
END

END
GO

-----------------------------------------------------
--
-- Name: sp_export_service_listing
--
-- Purpose: creates a service listing
--
-- Notes: Will export all the necessary info, including
--		the required certificates, the routing info
--		and the supported contracts/message types for 
--		the exported service. A Service Broker endpoint
--		must be configured in order to export the
--		needed routing info.
--
-- Parameters:
--		@service_name: the exported service name
--		@servicel_isting: output, the service listing 
--
-----------------------------------------------------
if object_id('sp_export_service_listing') is not null
	drop procedure sp_export_service_listing;
go

create procedure sp_export_service_listing
	(@service_name as sysname,
	@service_listing as xml output)
as
begin
	-- Export message types
	declare @messages xml;

	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @messages = (select distinct mt.name as [@name],
		mt.validation_desc as [@validation]
		from sys.service_message_types mt
			join sys.service_contract_message_usages mtu on mt.message_type_id = mtu.message_type_id
			join sys.service_contract_usages scu on scu.service_contract_id = mtu.service_contract_id
			join sys.services s on s.service_id = scu.service_id
		where s.name = @service_name
		for xml path ('message'));

	-- Export contracts
	declare @contracts xml;
	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @contracts = (select sc.name as [@name],
		(
			select mt.name as [@name],
				case 
					when mtu.is_sent_by_initiator = 1 and mtu.is_sent_by_target = 1 then 'ANY'
					when mtu.is_sent_by_initiator = 1 and mtu.is_sent_by_target = 0 then 'INITIATOR'
					when mtu.is_sent_by_initiator = 0 and mtu.is_sent_by_target = 1 then 'TARGET'
				end as [@sent-by]
				from sys.service_message_types mt 
					join sys.service_contract_message_usages mtu on mtu.message_type_id = mt.message_type_id
					where sc.service_contract_id = mtu.service_contract_id
			for xml path ('message'), type
		) as [*]
		from sys.service_contracts sc
			join sys.service_contract_usages scu on scu.service_contract_id = sc.service_contract_id
			join sys.services s on s.service_id = scu.service_id
		where s.name = @service_name
		for xml path ('contract'));

	-- Export service info and security info
	declare @service xml;
	declare @certblob varbinary(max);
	declare @certxml xml;

	if exists (select * from sys.certificates c
					join sys.services s on c.principal_id = s.principal_id
					where s.name = @service_name
					and c.pvt_key_encryption_type = 'MK'
					and c.is_active_for_begin_dialog = 1)
	begin
		declare @certname sysname;
		-- pick the latest expiry date cert
		select top(1) @certname = c.name
			from sys.certificates c
				join sys.services s on c.principal_id = s.principal_id
				where s.name = @service_name
				and c.pvt_key_encryption_type = 'MK'
				and c.is_active_for_begin_dialog = 1
			order by expiry_date desc;

		exec sp_get_certificate_blob @certname, @certblob output;

		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		select @certxml = (select c.issuer_name as [@issuer-name],
					c.cert_serial_number as [@serial-number],
					@certblob as [blob]
				from sys.certificates c
					where c.name = @certname
				for xml path ('certificate'), type, BINARY BASE64);
		
	end;

	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @service = (
		select s.name as [@name],
			db.service_broker_guid as [@broker-instance],
			(
				select case when exists (
					select * from sys.database_permissions dbp
						where dbp.class_desc = 'SERVICE'
							and dbp.permission_name = 'SEND'
							and dbp.state_desc = 'GRANT'
							and dbp.major_id = s.service_id and dbp.minor_id = 0
							and dbp.grantee_principal_id = 0)
					then 'Yes'
					else 'No'
				end
			) as [@public-access],
			(
				select sc.name as [@name]
					from sys.service_contracts sc
					join sys.service_contract_usages scu on scu.service_contract_id = sc.service_contract_id
					where scu.service_id = s.service_id
				for xml path ('contract'), type
			) as [*],
			@certxml as [*]
			from sys.services s
				join sys.databases db on db.name = db_name()
			where s.name = @service_name
			for xml path ('service'));

	--  Export routing and adjacent authentication info
	declare @binding xml;
	declare @adjcertname sysname;
	declare @adjcertclause xml;
	
	select @adjcertname = c.name 
		from master.sys.certificates c
			join sys.service_broker_endpoints sb on c.certificate_id = sb.certificate_id;

	if (@adjcertname is not null)
	begin
		declare @blob varbinary(max);
		
		exec master..sp_get_certificate_blob @adjcertname, @blob output;

		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		select @adjcertclause = (
			select c.issuer_name as [@issuer-name],
				c.cert_serial_number as [@serial-number],
				@blob as [blob]
			from master.sys.certificates c
				where name = @adjcertname
			for xml path ('certificate'), type);
		
	end;

	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @binding = (select serverproperty('machinename') as [@machinename],
		tcp.port as [@tcp-port],
		sb.connection_auth_desc as [@authentication],
		sb.encryption_algorithm_desc as [@encryption],
		case 
		when exists (
			select * from sys.server_permissions p
				where class_desc = 'ENDPOINT'
				and permission_name = 'CONNECT'
				and grantee_principal_id = 2
				and major_id = sb.endpoint_id
				and minor_id = 0)
			then 'Yes'
		else 'No'
		end as [@public-access],
		@adjcertclause as [*]
		from sys.tcp_endpoints tcp
		join  sys.service_broker_endpoints sb on sb.endpoint_id = tcp.endpoint_id
		for xml path ('endpoint'));

	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @service_listing = (select
			suser_name() as [@author],
			getdate() as [@timestamp],
			'1.0' as [@version],
			@messages as [*],
			@contracts as [*],
			@service as [*],
			@binding as [*]
			for xml path ('definition'), type);

end
go


-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface_create_message_types
--
-- Purpose: creates the message types from a service listing
--
-- Notes: Used by sp_import_service_listing_interface
--
-- Parameters:
--		@servicel_isting: the service listing 
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface_create_message_types') is not null
	drop procedure sp_import_service_listing_interface_create_message_types;
go

create procedure sp_import_service_listing_interface_create_message_types
	(@service_listing as xml)
as
begin
	declare @name sysname;
	declare @message sysname;
	declare @validation nvarchar(128);
	declare @sql nvarchar(max);

	begin transaction;

	-------------------------------------------
	--
	-- create message types
	--
	------------------------------------------

	declare crsMessages cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@name', 'sysname'), n.value('@validation', 'nvarchar(128)')
		from @service_listing.nodes('/definition/message[@name!="DEFAULT"]') t(n);

	open crsMessages
	fetch next from crsMessages into @name, @validation;
	while @@fetch_status = 0
	begin
		select @sql = N'create message type ' + quotename(@name) + N' validation = ' + 
			case @validation
				when 'BINARY' then N'NONE'
				when 'XML' then 'WELL_FORMED_XML'
				else @validation 
			end
		print @sql;
		exec sp_executesql @sql;
	
		fetch next from crsMessages into @name, @validation;
	end

	close crsMessages;
	deallocate crsMessages;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface_drop_message_types
--
-- Purpose: drops the message types from a service listing
--
-- Notes: Used by sp_import_service_listing_interface
--
-- Parameters:
--		@servicel_isting: the service listing 
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface_drop_message_types') is not null
	drop procedure sp_import_service_listing_interface_drop_message_types;
go

create procedure sp_import_service_listing_interface_drop_message_types
	(@service_listing as xml)
as
begin
	declare @name sysname;
	declare @message sysname;
	declare @sql nvarchar(max);

	begin transaction;

	-------------------------------------------
	--
	-- drop message types
	--
	------------------------------------------

	declare crsMessages cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@name', 'sysname')
		from @service_listing.nodes('/definition/message[@name!="DEFAULT"]') t(n);

	open crsMessages
	fetch next from crsMessages into @name;
	while @@fetch_status = 0
	begin
		if exists (select * from sys.service_message_types where name = @name)
		begin
			select @sql = N'drop message type ' + quotename(@name);
			exec sp_executesql @sql;
		end
	
		fetch next from crsMessages into @name;
	end

	close crsMessages;
	deallocate crsMessages;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface_create_contract
--
-- Purpose: creates the contracts from a service listing
--
-- Notes: Used by sp_import_service_listing_interface
--
-- Parameters:
--		@servicel_isting: the service listing 
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface_create_contract') is not null
	drop procedure sp_import_service_listing_interface_create_contract;
go

create procedure sp_import_service_listing_interface_create_contract
	(@service_listing as xml)
as
begin
	declare @name sysname;
	declare @message sysname;
	declare @sent_by nvarchar(128);
	declare @last_contract sysname;
	declare @comma nvarchar(1);
	declare @sql nvarchar(max);

	begin transaction;

	-------------------------------------------
	--
	-- create contracts
	--
	------------------------------------------
	declare crsContracts cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('../@name', 'sysname') as [name],
			n.value('@name', 'sysname') as [message],
			n.value('@sent-by', 'nvarchar(128)') as sent_by
		from @service_listing.nodes('/definition/contract[@name!="DEFAULT"]/message') t(n);

	select @comma = N'', @sql = N'', @last_contract = N'';
	
	open crsContracts
	fetch next from crsContracts into @name, @message, @sent_by;
	while @@fetch_status = 0
	begin
		if @name <> @last_contract
		begin
		begin
			if @sql <> N''
			begin
				select @sql = @sql + N')';
				--print @sql;
				exec sp_executesql @sql;
			end
			select @sql = N'create contract ' + quotename (@name) + N'(', @comma = N'';
		end
		end

		select @sql = @sql + @comma + quotename(@message) + N' SENT BY ' + @sent_by;
		select @comma = N',', @last_contract = @name;
		fetch next from crsContracts into @name, @message, @sent_by;
	end

	if @sql <> N''
	begin
		select @sql = @sql + N')';
		--print @sql;
		exec sp_executesql @sql;
	end

	close crsContracts;
	deallocate crsContracts;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface_drop_contract
--
-- Purpose: Drops the contracts from a service listing
--
-- Notes: Used by sp_import_service_listing_interface
--
-- Parameters:
--		@servicel_isting: the service listing 
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface_drop_contract') is not null
	drop procedure sp_import_service_listing_interface_drop_contract;
go

create procedure sp_import_service_listing_interface_drop_contract
	(@service_listing as xml)
as
begin
	declare @name sysname;
	declare @sql nvarchar(max);

	begin transaction;

	-------------------------------------------
	--
	-- drop contracts
	--
	------------------------------------------
	declare crsContracts cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@name', 'sysname') 
			from @service_listing.nodes('/definition/contract[@name!="DEFAULT"]') t(n);

	open crsContracts
	fetch next from crsContracts into @name;
	while @@fetch_status = 0
	begin
		if exists (select * from sys.service_contracts where name = @name)
		begin
			select @sql = 'drop contract ' + quotename(@name);
			exec sp_executesql @sql;
		end
		fetch next from crsContracts into @name;
	end

	close crsContracts;
	deallocate crsContracts;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface
--
-- Purpose: Imports the message types and contracts from
--		a service listing. By default also drops them 
--		if already existing, before creating new ones.
--
-- Notes: Used by sp_import_target_service_listing_at_initiator
--
-- Parameters:
--		@service_listing: the service listing
--		@dropexisting: flag to overwrite the dropping of already 
--			existing  contracts and message types
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface') is not null
	drop procedure sp_import_service_listing_interface;
go

create procedure sp_import_service_listing_interface
	(@service_listing as xml,
	@dropexisting as bit = 1)
as
begin
	begin transaction
	if (@dropexisting = 1)
	begin
		exec sp_import_service_listing_interface_drop_contract @service_listing;
		exec sp_import_service_listing_interface_drop_message_types @service_listing;
	end
	exec sp_import_service_listing_interface_create_message_types @service_listing;
	exec sp_import_service_listing_interface_create_contract @service_listing;
	commit
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_create_route
--
-- Purpose: creates a route to the service contained in the
--		service listing
--
-- Notes: The service name and the broker instance will be used
--		to name the newly created route. If a route to the same 
--		service and broker instance already exists, by default
--		it will be dropped.
--
-- Parameters:
--		@service_listing: the service listing
--		@dropexisting: flag to control the dropping of
--				already exsiting route
--
-----------------------------------------------------
if object_id('sp_import_service_listing_create_route') is not null
	drop procedure sp_import_service_listing_create_route;
go

create procedure sp_import_service_listing_create_route
	(@service_listing as xml,
	@dropexisting as bit = 1)
as
begin
	declare @service nvarchar(256);
	declare @broker_instance nvarchar(256);
	declare @existingroute sysname;
	declare @address nvarchar(max);
	declare @sql nvarchar(max);

	begin transaction;

	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @address = N'tcp://' + 
		n.value('@machinename','nvarchar(256)') + N':' + 
		n.value('@tcp-port','varchar(5)')
		from @service_listing.nodes('/definition/endpoint') t(n);

	declare crsServices cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@name', 'nvarchar(256)'), 
				n.value('@broker-instance', 'nvarchar(256)')
			from @service_listing.nodes('/definition/service') t(n);

	open crsServices
	fetch next from crsServices into @service, @broker_instance;
	while @@fetch_status = 0
	begin
		declare @routename sysname;
		select @routename = @service + '_' + isnull (@broker_instance, '');
		
		if exists (select * from sys.routes where name = @routename and @dropexisting = 1)
		begin
			select @sql = 'drop route ' + quotename (@routename);
			exec sp_executesql @sql;
		end

		select @sql = 'create route ' + quotename(@routename) + 
			N' with service_name = ' + quotename(@service, '''') + 
			case 
				when @broker_instance is null then N''
				else N', broker_instance = ' + quotename (@broker_instance,'''')
			end 
			+ N', address = ' + 
			case
				when exists (select * from sys.databases where service_broker_guid = @broker_instance)
					then N'''LOCAL'''
				else quotename(@address, '''')
			end;

		print @sql;
		exec sp_executesql @sql;
		fetch next from crsServices into @service, @broker_instance;
	end

	close crsServices;
	deallocate crsServices;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_create_user_and_cert
--
-- Purpose: creates a user and a certificate from a certificate
--		info in a service listing
--
-- Notes: internally used by other procedures
--
-- Parameters:
--
-----------------------------------------------------
if object_id('sp_import_service_listing_create_user_and_cert') is not null
	drop procedure sp_import_service_listing_create_user_and_cert;
go

create procedure sp_import_service_listing_create_user_and_cert
	(@service_listing as xml,
	@issuername nvarchar(max),
	@serialnumber nvarchar(max),
	@proxy_user_name sysname = NULL,
	@dropexisting as bit = 1,
	@username sysname output)
as
begin
	declare @sql nvarchar(max);
	select @username = ISNULL(@proxy_user_name, @issuername + ': ' + @serialnumber);

	if exists (select * from sys.certificates where issuer_name = @issuername and cert_serial_number = @serialnumber and @dropexisting = 1)
	begin
		declare @certname sysname;
		select @certname = name from sys.certificates where issuer_name = @issuername and cert_serial_number = @serialnumber;
		select @sql = N'drop certificate ' + quotename(@certname);
		exec sp_executesql @sql;
	end

	declare @blob varbinary(max);
	with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @blob = (select @service_listing.value ('(/definition/service/certificate[@issuer-name=sql:variable("@issuername")][@serial-number=sql:variable("@serialnumber")]/blob)[1]', 'varbinary(max)'));
	exec sp_create_certificate_from_blob @username, @blob;

	if exists (Select * from sys.database_principals where name = @username and @dropexisting = 1)
	begin
		select @sql = N'drop user ' + quotename(@username);
		exec sp_executesql @sql;
	end

	select @sql = N'create user ' + quotename(@username) + N' without login';
	
	print @sql;
	exec sp_executesql @sql;

	select @sql = N'alter authorization on certificate::' + quotename(@username) + ' to ' + quotename(@username);
	print @sql;
	exec sp_executesql @sql;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_create_remote_binding
--
-- Purpose: creates the remote service binding needed to
--		begin secure dialogs with a service
--
-- Notes: The newly created RSB will be named based on the name of the target 
--			service (the service name from the service listing).
--
-- Parameters:
--		@service_listing: the service listing
--		@proxy_user_name: the name of the proxy user that represents the 
--				the target service owner. By default the name will be
--				generated from the certificate issuer name and serial number. 
--		@requestanonymous: flag to control the ANONYMOUS clause of the 
--				created RSB. The service listing has to specify the 
--				attribute public-access="Yes".
--		@dropexisting: flag to control the dropping of already existing remote 
--				service binding for the same remote service.
--
-----------------------------------------------------
if object_id('sp_import_service_listing_create_remote_binding') is not null
	drop procedure sp_import_service_listing_create_remote_binding;
go

create procedure sp_import_service_listing_create_remote_binding
	(@service_listing as xml,
	@proxy_user_name sysname = NULL,
	@requestanonymous as bit = 0,
	@dropexisting as bit = 1)
as
begin
	declare @service nvarchar(256);
	declare @publicaccess varchar(3);
	declare @issuername nvarchar(max);
	declare @serialnumber nvarchar(max);
	declare @sql nvarchar(max);

	begin transaction;

	declare crsCertificates cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@issuer-name', 'nvarchar(max)') as issue_name,
			n.value('@serial-number', 'nvarchar(max)') as serial_number,
			n.value('../@name', 'nvarchar(256)') as service_name,
			n.value('../@public-access', 'varchar(3)') as public_access
		from @service_listing.nodes('/definition/service/certificate') t(n);

	open crsCertificates
	fetch next from crsCertificates into @issuername, @serialnumber, @service, @publicaccess;
	while @@fetch_status = 0
	begin
		declare @username sysname;

		if  exists (select * from sys.remote_service_bindings where remote_service_name = @service and @dropexisting = 1)
		begin
			declare @rsbname sysname;
			select @rsbname = name from sys.remote_service_bindings where remote_service_name = @service;
			select @sql = N'drop remote service binding ' + quotename (@rsbname);
			exec sp_executesql @sql;
		end

		exec sp_import_service_listing_create_user_and_cert @service_listing, @issuername, @serialnumber, @proxy_user_name, @dropexisting, @username output;
	
		select @sql = N'create remote service binding ' + quotename(@service) +
			 N' to service ' + quotename(@service, '''') + 
			 N' with user = ' + quotename(@username) +
			case 
				when @requestanonymous = 1 and @publicaccess = 'Yes' then N', anonymous = on'
				else N', anonymous = off'
			end;
		print @sql;
		exec sp_executesql @sql;

		fetch next from crsCertificates into @issuername, @serialnumber, @service, @publicaccess;
	end

	close crsCertificates;
	deallocate crsCertificates;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_grant_send_to_service
--
-- Purpose: creates a proxy user for an initiator service owner and
--		grants SEND permission to a given target service to him. 
--
-- Notes: used by sp_import_initiator_service_listing_at_target
--
-- Parameters:
--
-----------------------------------------------------
if object_id('sp_import_service_listing_grant_send_to_service') is not null
	drop procedure sp_import_service_listing_grant_send_to_service;
go

create procedure sp_import_service_listing_grant_send_to_service
	(@service_listing as xml,
	@targeted_service_name sysname,
	@proxy_user_name as sysname = NULL,
	@dropexisting as bit = 1)
as
begin
	declare @issuername nvarchar(max);
	declare @serialnumber nvarchar(max);
	declare @sql nvarchar(max);

	begin transaction;

	declare crsCertificates cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@issuer-name', 'nvarchar(max)') as issuer_name,
			n.value('@serial-number', 'nvarchar(max)') as serial_number
		FROM @service_listing.nodes('/definition/service/certificate') t(n);

	open crsCertificates
	fetch next from crsCertificates into @issuername, @serialnumber;
	while @@fetch_status = 0
	begin
		declare @username sysname;


		exec sp_import_service_listing_create_user_and_cert @service_listing, @issuername, @serialnumber, @proxy_user_name, @dropexisting, @username output;
	
		select @sql = N'grant send on service::' + quotename (@targeted_service_name) + N' to ' + quotename(@username);
		print @sql;
		exec sp_executesql @sql;

		fetch next from crsCertificates into @issuername, @serialnumber;
	end

	close crsCertificates;
	deallocate crsCertificates;

	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_target_service_listing_at_initiator
--
-- Purpose: imports, at the initiator site, a service listing
--			created for a target service
--
-- Notes: Should be used by the initiator service administrator. 
--		It will create the routing info needed to send messages 
--		to the target service. It will import the security info
--		needed for targeting the service (the RSB). It will import
--		the contracts and message types used by the target service.
--
-- Parameters:
--		@service_listing: the target service listing
--		@proxy_user_name: local name to be used for the target 
--				service owner proxy user
--		@requestanonymous: flag to request an anonymous configuration.
--				It cannot be honored if the target service does
--				not accept public access.
--
-----------------------------------------------------
if object_id('sp_import_target_service_listing_at_initiator') is not null
	drop procedure sp_import_target_service_listing_at_initiator;
go

create procedure sp_import_target_service_listing_at_initiator
	(@service_listing as xml,
	@proxy_user_name sysname = null,
	@requestanonymous bit = 0)
as
begin
	begin transaction
	exec sp_import_service_listing_interface @service_listing;
	exec sp_import_service_listing_create_remote_binding @service_listing, @proxy_user_name, @requestanonymous;
	exec sp_import_service_listing_create_route @service_listing;
	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_import_initiator_service_listing_at_target
--
-- Purpose: imports, at the target site, a service listing
--			created by an initiator 
--
-- Notes: Should be used by the target service administrator. 
--			It will import the routing information needed to
--			send back replies to the initiator and the security 
--			information needed to grant SEND access to the initiator
--			service owner. If the initiator service listing does
--			not contain a certificate, no SEND persmission is granted.
--			If anonymous security is desired, the target service 
--			administrator should grant SEND permissions on the
--			targeted service to [Public].
--
-- Parameters:
--		@service_listing: the initator service listing
--		@targeted_service_name: the service targeted by the initiator.
--			SEND permission will be granted on this service
--			to the proxy user representing the initiator service owner.
--		@proxy_user_name: name to be used for the proxy user representing
--			the initiator service owner.
--
-----------------------------------------------------
if object_id('sp_import_initiator_service_listing_at_target') is not null
	drop procedure sp_import_initiator_service_listing_at_target;
go

create procedure sp_import_initiator_service_listing_at_target
	(@service_listing as xml,
	@targeted_service_name as sysname,
	@proxy_user_name as sysname = NULL)
as
begin
	begin transaction
	exec sp_import_service_listing_grant_send_to_service @service_listing, @targeted_service_name, @proxy_user_name;
	exec sp_import_service_listing_create_route @service_listing;
	commit;
end
go

-----------------------------------------------------
--
-- Name: sp_secure_service
--
-- Purpose: prepares a service for secure dialogs
--
-- Notes: creates a certificate for the service owner.
--		Should be run by both the initiator and target
--		service administrators. It must be run BEFORE
--		exporting the service listing.
--
-- Parameters:
--		@service_name: the name of the service to be secured
--
-----------------------------------------------------
if object_id('sp_secure_service') is not null
	drop procedure sp_secure_service;
go

create procedure sp_secure_service
	(@service_name as sysname)
as
begin
	declare @sql nvarchar(max);
	declare @owner sysname;
	declare @owner_id int;

	select @owner = p.name,
		@owner_id = s.principal_id 
		from sys.services s
			join sys.database_principals p on s.principal_id = p.principal_id
			where s.name = @service_name;
	if @owner is not null
	begin
		declare @certname sysname;
		select top(1) @certname = c.name from sys.certificates c
			where pvt_key_encryption_type = 'MK'
				and is_active_for_begin_dialog = 1
				and principal_id = @owner_id
			and start_date < getdate ()
			and getdate() < expiry_date
			order by expiry_date desc;
		if @certname is not null
		begin
			print 'Existing certificate ' + quotename(@certname) + ' will be used to secure the service.';
		end
		else
		begin
			select @certname = @service_name;
			select @sql = N'create certificate ' + quotename(@certname) + 
				N' authorization ' + quotename (@owner) +
				N' with subject = ' + quotename(@service_name,'''') +
				N' active for begin_dialog = on';
			print @sql;
			exec sp_executesql @sql;
		end
	end
end
go

