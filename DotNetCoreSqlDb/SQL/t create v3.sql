create table AdVersionDetails(
AdVersionID				varchar(30)		primary key,	--OFFERNO -> OfferID
PrimaryVersionName		varchar(100),
SecondaryVersionName	varchar(100),
Product					varchar(50)
)


create table AdDetails(
AdCode				varchar(30)			primary key,	--KeyCode -> AdCode
AdVersionID			varchar(30)			foreign key references AdVersionDetails(AdVersionID),
PrimaryAdName		varchar(100),	--Old Publication
SecondaryAdName		varchar(100),
Category			varchar(30),
StartDate			date,
DateCreated			datetime
)

create table AdPhone(
AdCode				varchar(30)			foreign key references AdDetails(AdCode),
TFN					varchar(12)			,
StartDate			date
)


create table AdSpend(
AdCode				varchar(30)			foreign key references AdDetails(AdCode),
SpendDate			date,
Amount				money,
Circulation			int
)

--drop table AdSpend
--drop table AdSource
--drop table AdOfferPhone
--drop table AdDetails

--select * from AdOfferPhone