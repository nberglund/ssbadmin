
-----------------------------------------------------
--
-- Name: sp_export_service_listing
--
-- Purpose: creates a service listing
--
-- Notes: Will export all the necessary info, including
--		the required certificates, the routing info
--		and the supported contracts/message types/
--    xml schema collections for 
--		the exported service. A Service Broker endpoint
--		must be configured in order to export the
--		needed routing info.
--
-- Parameters:
--		@service_name: the exported service name
--    @cert_name:  name of cert to be used [optional]
--		@servicel_isting: output, the service listing 
--
-- Change History:
--    Date      By        Desc
--    Dec 2005  Niels B   Added code to retrieve
--                        xml schema collection info
--                        and include this info for
--                        message types. Also added a param
--                        for cert name
-----------------------------------------------------
if object_id('sp_export_service_listing') is not null
	drop procedure sp_export_service_listing;
go

create procedure sp_export_service_listing
	(@service_name as sysname,
  @cert_name as sysname = null,
	@service_listing as xml output)
as
begin
  -- Export xml schema collection

  --retrieve out the unique names and schemas
  ;with schemastuff (collname,schname)
  as
  (
    select distinct c.name, sch.name
    from sys.xml_schema_collections c
    join sys.service_message_types mt on mt.xml_collection_id = c.xml_collection_id
    join sys.service_contract_message_usages mtu on mt.message_type_id = mtu.message_type_id
    join sys.service_contract_usages scu on scu.service_contract_id = mtu.service_contract_id
    join sys.services s on s.service_id = scu.service_id
    join sys.schemas sch on c.schema_id = sch.schema_id
    where s.name = @service_name
)

  --select the name and the xml schema into a temp table
  select collname as [name], xml_schema_namespace(schname, collname) as xmlcoll into #t from schemastuff

  --load it into an xml variable
  declare @schemacoll xml
  ;with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing')
    select @schemacoll = (select * from #t for xml raw ('schemacoll'), type)
  drop table #t


	-- Export message types
	declare @messages xml;

  with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
	select @messages = (select distinct mt.name as [@name],
		mt.validation_desc as [@validation],
    c.name as [collection] --include collection info
		from sys.service_message_types mt
			join sys.service_contract_message_usages mtu on mt.message_type_id = mtu.message_type_id
			join sys.service_contract_usages scu on scu.service_contract_id = mtu.service_contract_id
			join sys.services s on s.service_id = scu.service_id
      left join sys.xml_schema_collections c on mt.xml_collection_id = c.xml_collection_id
		where s.name = @service_name
		for xml path ('message'), ELEMENTS XSINIL);

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

  if(@cert_name is not null) -- user sent in a certname
  begin
    --check that the given cert is valid for begin dialog
    if not exists(select * from sys.certificates c
                  where c.name = @cert_name
                  and c.pvt_key_encryption_type = 'MK'
					        and c.is_active_for_begin_dialog = 1)
    begin
      raiserror('The given cert is either not encrypted by the master key
                  or not active for begin dialog', 16, 1);
      goto finish;
    end
  end
  else -- no cert was given
  begin

	if exists (select * from sys.certificates c
					join sys.services s on c.principal_id = s.principal_id
					where s.name = @service_name
					and c.pvt_key_encryption_type = 'MK'
					and c.is_active_for_begin_dialog = 1)
	begin
		
		-- pick the latest expiry date cert
		select top(1) @cert_name = c.name
			from sys.certificates c
				join sys.services s on c.principal_id = s.principal_id
				where s.name = @service_name
				and c.pvt_key_encryption_type = 'MK'
				and c.is_active_for_begin_dialog = 1
			order by expiry_date desc;

  end

  end;

    if(@cert_name is not null)
    begin

      exec sp_get_certificate_blob @cert_name, @certblob output;

		  with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		  select @certxml = (select c.issuer_name as [@issuer-name],
					c.cert_serial_number as [@serial-number],
					@certblob as [blob]
				from sys.certificates c
					where c.name = @cert_name
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
      @schemacoll as [*],
			@messages as [*],
			@contracts as [*],
			@service as [*],
			@binding as [*]
			for xml path ('definition'), type);


finish:

end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface_create_schema_collections
--
-- Purpose: creates xml schema collection from 
--    a service listing
--
-- Notes: Used by sp_import_service_listing_interface
--
-- Parameters:
--		@servicel_isting: the service listing 
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface_create_schema_collections') is not null
	drop procedure sp_import_service_listing_interface_create_schema_collections;
go

create procedure sp_import_service_listing_interface_create_schema_collections
	(@service_listing as xml)
as
begin
declare @name sysname;
	declare @collection xml;
  declare @coll nvarchar(max);
	declare @sql nvarchar(max);
  
  begin transaction;
	-------------------------------------------
	--
	-- create collection
	--
	------------------------------------------
		declare crsCollection cursor read_only for 
	  with xmlnamespaces ('http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing' AS sl, 'http://www.w3.org/2001/XMLSchema' as xsd) 
		SELECT n.value('@name', 'sysname'), n.query('./sl:xmlcoll/xsd:schema')
		from @service_listing.nodes('/sl:definition/sl:schemacoll') t(n);

	  open crsCollection
	  fetch next from crsCollection into @name, @collection;
	  while @@fetch_status = 0
	  begin
      -- had probs with quote name, so I had to do an explicit cast etc.
      set @coll = cast(@collection as nvarchar(max)); 
      select @sql = 'create xml schema collection ' + quotename(@name) + ' as ''' + @coll +''''
		  --print @sql;
		  exec sp_executesql @sql;
	
		  fetch next from crsCollection into @name, @collection;
	  end

	close crsCollection;
	deallocate crsCollection;
commit
end
go

-----------------------------------------------------
--
-- Name: sp_import_service_listing_interface_drop_schema_collections
--
-- Purpose: drops the xml schema collections from a #
--    service listing
--
-- Notes: Used by sp_import_service_listing_interface
--
-- Parameters:
--		@servicel_isting: the service listing 
--
-----------------------------------------------------
if object_id('sp_import_service_listing_interface_drop_schema_collections') is not null
	drop procedure sp_import_service_listing_interface_drop_schema_collections;
go

create procedure sp_import_service_listing_interface_drop_schema_collections
	(@service_listing as xml)
as
begin
	declare @name sysname;
	declare @sql nvarchar(max);

	begin transaction;

	-------------------------------------------
	--
	-- drop schema collection
	--
	------------------------------------------

	declare crsCollection cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@name', 'sysname')
		from @service_listing.nodes('/definition/schemacoll') t(n);

	open crsCollection
	fetch next from crsCollection into @name;
	while @@fetch_status = 0
	begin
		if exists (select * from sys.xml_schema_collections where name = @name)
		begin
			select @sql = N'drop xml schema collection ' + quotename(@name);
      --print @sql
			exec sp_executesql @sql;
		end
	
		fetch next from crsCollection into @name;
	end

	close crsCollection;
	deallocate crsCollection;

	commit;
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
-- Change History:
--    Date      By        Desc
--    Jan 2006  Niels B   Added code to create
--                        message types with
--                        xml schema collection validation
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
  declare @collection sysname; -- variable for collection name
	declare @sql nvarchar(max);

	begin transaction;

	-------------------------------------------
	--
	-- create message types
	--
	------------------------------------------

	declare crsMessages cursor read_only for 
		with xmlnamespaces (DEFAULT 'http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing') 
		SELECT n.value('@name', 'sysname'), 
           n.value('@validation', 'nvarchar(128)'), 
           n.value('./collection[1]', 'sysname')
		from @service_listing.nodes('/definition/message[@name!="DEFAULT"]') t(n);

	open crsMessages
	fetch next from crsMessages into @name, @validation, @collection;
	while @@fetch_status = 0
	begin
		select @sql = N'create message type ' + quotename(@name) + N' validation = ' + 
			case 
				when @validation = 'BINARY' then N'NONE'
				when @validation = 'XML' AND @collection <> '' 
          THEN 'VALID_XML WITH SCHEMA COLLECTION ' + quotename(@collection)
        when @validation = 'XML' AND @collection = '' THEN 'WELL_FORMED_XML'
        else 'EMPTY'
      end

		--print @sql;
		exec sp_executesql @sql;
	
		fetch next from crsMessages into @name, @validation, @collection;
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
-- Change History:
--    Date      By        Desc
--    Jan 2006  Niels B   Commented out some print
--                        statements
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
-- Name: sp_import_service_listing_interface
--
-- Purpose: Imports the xml schema collections, message 
--    types and contracts from a service listing. 
--    By default also drops them if already existing, 
--    before creating new ones.
--
-- Notes: Used by sp_import_target_service_listing_at_initiator
--
-- Parameters:
--		@service_listing: the service listing
--		@dropexisting: flag to overwrite the dropping of already 
--			existing  contracts, message types and xml schema
--      collections
--
-- Change History:
--    Date      By        Desc
--    Jan 2006  Niels B   Added code to drop/create
--                        xml schema collection
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
    exec sp_import_service_listing_interface_drop_schema_collections @service_listing;
	end
  exec sp_import_service_listing_interface_create_schema_collections @service_listing;
	exec sp_import_service_listing_interface_create_message_types @service_listing;
	exec sp_import_service_listing_interface_create_contract @service_listing;
	commit
end
go




