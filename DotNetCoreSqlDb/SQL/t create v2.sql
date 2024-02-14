create table OfferDetails(
AdVersionID			varchar(30)		primary key,	--OFFERNO -> AdVersionID
PrimaryName		varchar(100),
SecondaryName	varchar(100),
Product			varchar(50)
)

create table SourceDetails(
SourceID		int				primary key,	--pubcode
PrimaryName		varchar(100),
SecondaryName	varchar(100),
Category		varchar(30)
)

create table AdDetails(
AdCode		varchar(30)			primary key,	--KeyCode -> AdCode
AdVersionID		varchar(30)			foreign key references OfferDetails(AdVersionID),
SourceID	int					foreign key references SourceDetails(SourceID),
StartDate		date
)
alter table adDetails add DateCreated datetime
go

create table AdOfferPhone(
AdCode		varchar(30)			foreign key references AdDetails(AdCode),
AdVersionID		varchar(30)			foreign key references OfferDetails(AdVersionID),	--offerno
TFN			varchar(12)
)


create table AdSpend(
AdCode		varchar(30)			foreign key references AdDetails(AdCode),
SpendDate	date,
Amount		money,
Circulation int
)


--drop table AdSpend
--drop table AdSource
--drop table AdOfferPhone
--drop table AdDetails