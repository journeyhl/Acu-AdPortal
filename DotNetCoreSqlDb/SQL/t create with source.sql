create table OfferDetails(
AdVersionID			varchar(10)		primary key,	--OFFERNO -> AdVersionID
PrimaryName		varchar(100),
SecondaryName	varchar(100),
Product			varchar(50),
StartDate		date
)

create table SourceDetails(
AdVersionID		int				primary key,
PrimaryName		varchar(100),
SecondaryName	varchar(100),
Category		varchar(30)
)

create table AdDetails(
AdCode		varchar(30)			primary key,	--KeyCode -> AdCode
StartDate	date, 
TFN			varchar(12),
AdVersionID	int					foreign key references SourceDetails(AdVersionID),
AdVersionID		varchar(10)			foreign key references OfferDetails(AdVersionID)
)


create table AdSpend(
AdCode		varchar(30)			foreign key references AdDetails(AdCode),
SpendDate	date,
Amount		money,
Circulation int
)


