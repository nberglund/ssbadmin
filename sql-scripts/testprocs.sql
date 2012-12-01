use master;

drop database ssb2;

create database ssb2;

use ssb2;

create master key
encryption by password = 'hello11'

create certificate testCert
with subject = 'test cert'
	

drop xml schema collection invoice
create xml schema collection invoice
as
'<xs:schema 
  targetNamespace="urn:Invoice" 
  xmlns="urn:Invoice" 
  xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="Invoice">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="OrderId" type="xs:int" />
        <xs:element name="Value" type="xs:decimal" />
        <xs:element name="Stuff" type="xs:decimal" />
      </xs:sequence>
      <xs:attribute name="Id" type="xs:int" />
    </xs:complexType>
  </xs:element>
</xs:schema>'

create xml schema collection invoice2
as
'<xs:schema 
  targetNamespace="urn:Invoice2" 
  xmlns="urn:Invoice2" 
  xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="Invoice2">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="OrderId" type="xs:int" />
        <xs:element name="Value" type="xs:decimal" />
        <xs:element name="Stuff" type="xs:decimal" />
      </xs:sequence>
      <xs:attribute name="Id" type="xs:int" />
    </xs:complexType>
  </xs:element>
</xs:schema>'


create message type inv1
validation = valid_xml with schema collection Invoice

create message type inv2
validation = valid_xml with schema collection Invoice2

create message type inv3
validation = well_formed_xml

create message type m1
validation = NONE

create contract invc
(
  inv1
  sent by initiator,
  inv2
  sent by target,
  inv3
  sent by any,
  m1
  sent by any
)

create queue q1
with status =on
drop service invserv
create service invserv
on queue q1
(invc)

	
declare @service_listing xml
exec sp_export_service_listing 'invserv', 'testCert', @service_listing out
select @service_listing

declare @service_listing xml;
set @service_listing = '<definition xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" author="DEV-1\Administrator" timestamp="2006-01-02T22:12:19.950" version="1.0">
  <schemacoll xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="invoice">
    <xmlcoll>
      <xsd:schema xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:Invoice" xmlns="" targetNamespace="urn:Invoice">
        <xsd:element name="Invoice">
          <xsd:complexType>
            <xsd:complexContent>
              <xsd:restriction base="xsd:anyType">
                <xsd:sequence>
                  <xsd:element name="OrderId" type="xsd:int" />
                  <xsd:element name="Value" type="xsd:decimal" />
                  <xsd:element name="Stuff" type="xsd:decimal" />
                </xsd:sequence>
                <xsd:attribute name="Id" type="xsd:int" />
              </xsd:restriction>
            </xsd:complexContent>
          </xsd:complexType>
        </xsd:element>
      </xsd:schema>
    </xmlcoll>
  </schemacoll>
  <schemacoll xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="invoice2">
    <xmlcoll>
      <xsd:schema xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:Invoice2" xmlns="" targetNamespace="urn:Invoice2">
        <xsd:element name="Invoice2">
          <xsd:complexType>
            <xsd:complexContent>
              <xsd:restriction base="xsd:anyType">
                <xsd:sequence>
                  <xsd:element name="OrderId" type="xsd:int" />
                  <xsd:element name="Value" type="xsd:decimal" />
                  <xsd:element name="Stuff" type="xsd:decimal" />
                </xsd:sequence>
                <xsd:attribute name="Id" type="xsd:int" />
              </xsd:restriction>
            </xsd:complexContent>
          </xsd:complexType>
        </xsd:element>
      </xsd:schema>
    </xmlcoll>
  </schemacoll>
  <message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="inv1" validation="XML">
    <collection>invoice</collection>
  </message>
  <message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="inv2" validation="XML">
    <collection>invoice2</collection>
  </message>
  <message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="inv3" validation="XML">
    <collection xsi:nil="true" />
  </message>
  <message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="m1" validation="BINARY">
    <collection xsi:nil="true" />
  </message>
  <contract xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="invc">
    <message xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="inv3" sent-by="ANY" />
    <message xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="m1" sent-by="ANY" />
    <message xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="inv1" sent-by="INITIATOR" />
    <message xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="inv2" sent-by="TARGET" />
  </contract>
  <service xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="invserv" broker-instance="5C449207-0EC0-4FB2-BA90-C79F862C13F4" public-access="No">
    <contract xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" name="invc" />
    <certificate xmlns="http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing" issuer-name="test cert" serial-number="96 07 ae 78 4e 1d 3b 8d 4e 1d ff 9a 95 22 42 78">
      <blob>MIIBqzCCARSgAwIBAgIQlgeueE4dO41OHf+alSJCeDANBgkqhkiG9w0BAQUFADAUMRIwEAYDVQQDEwl0ZXN0IGNlcnQwHhcNMDYwMTAyMjIwODI0WhcNMDcwMTAyMjIwODI0WjAUMRIwEAYDVQQDEwl0ZXN0IGNlcnQwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBALDqnnTytPRZJunI3Dqjdrvg3/I2sn2hNyfjqlaDX0nnIJjPCc3kJNEQWHkYUiNhN904Dc0Kk/EzowFPJyE6XmzybpZ04hn55fwfwZGkK0ijlKwdAbzK44gm8+GByH0oB1bOwNQ5u4hc6KSOkN21NE1ou+0Mv4YQkbMdFNUwakjVAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAquD4WNDaHulMFmgo7dV0eyNDfzG68D2fsOfgYsTRb7zAKdduWR7xOEcPWS1om/v1LaFUlS/6AX4N7LucuO7gEMooPfXo0pScLM4mbtxAu2Q2CzY+LjVjx4TAOi4RLw1mSk2J73i4Oevc2aK+/fQ1hHSyjYQS5oj9g5pHxIUAa+Y=</blob>
    </certificate>
  </service>
</definition>';

exec sp_import_service_listing_interface @service_listing, 1

select * from sys.xml_schema_collections

declare @name sysname;
declare @sql varchar(max)

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
      print @sql
			--exec sp_executesql @sql;
		end
	
		fetch next from crsCollection into @name;
	end

	close crsCollection;
	deallocate crsCollection;



  declare @name sysname;
	declare @collection xml;
  declare @coll nvarchar(max);
	declare @sql nvarchar(max);

	
		declare crsCollection cursor read_only for 
	  with xmlnamespaces ('http://schemas.microsoft.com/SQL/ServiceBroker/ServiceListing' AS sl, 'http://www.w3.org/2001/XMLSchema' as xsd) 
		SELECT n.value('@name', 'sysname'), n.query('./sl:xmlcoll/xsd:schema')
		from @service_listing.nodes('/sl:definition/sl:schemacoll') t(n);

	open crsCollection
	fetch next from crsCollection into @name, @collection;
	while @@fetch_status = 0
	begin
set @coll = cast(@collection as nvarchar(max));
select @sql = 'create xml schema collection ' + quotename(@name) + ' as ''' + @coll +''''
			

		print @sql;
		--exec sp_executesql @sql;
	
		fetch next from crsCollection into @name, @collection;
	end

	close crsCollection;
	deallocate crsCollection;


use ssb2

select * from sys.services

select * from sys.service_message_types

