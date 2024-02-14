create procedure generateExternalAdCode
AS
SELECT MAX(CAST (AdCode as int)) + 1 AdCode
FROM AdDetails 
WHERE LEFT(CAST (AdCode as int), 1)  = 1 AND AdCode NOT LIKE '[A-Za-z]%'
go

create procedure generateInternalAdCode
AS
SELECT MAX(CAST (AdCode as int)) + 1 AdCode
FROM AdDetails 
WHERE LEFT(CAST (AdCode as int), 1)  = 6 AND AdCode NOT LIKE '[A-Za-z]%'
go

create procedure acSearch @AdCode varchar(30)
AS
SELECT * FROM AdDetails WHERE AdCode = @AdCode
go

create procedure insertAdvertisement
					@AdCode			varchar(30), 
					@AdVersionID	varchar(30), 
					@PrimaryAdName	varchar(30),
					@SecondaryAdName varchar(30),
					@Category		varchar(30), 
					@StartDate		date, 
					@SpendDate		date, 
					@SpendAmount	money,
					@Circulation	int
AS
INSERT INTO 
AdDetails (			AdCode			,
					AdVersionID		,
					PrimaryAdName	,
					SecondaryAdName	,
					Category		,
					StartDate		,
					DateCreated		
)VALUES (	
					@AdCode, 
					@AdVersionID, 
					@PrimaryAdName,
					@SecondaryAdName	,
					@Category, 
					@StartDate, 
					GETDATE()
)
INSERT INTO 
AdPhone(			AdCode		,
					StartDate			
)VALUES(			
					@AdCode		,
					@StartDate	
)
INSERT INTO 
AdSpend VALUES (	@AdCode,
					@SpendDate,
					@SpendAmount,
					@Circulation
)
go

create procedure AdVersionProduct @AdVersionID varchar(30)
as
SELECT Product
FROM AdVersionDetails 
where AdVersionID = @AdVersionID
go

create procedure PostPhone @AdCode varchar(30), @OnSale date, @Phone varchar(20)
as
update AdPhone set TFN = @Phone 
where AdCode = @AdCode and StartDate = @OnSale
go

alter table AdPhone drop  constraint FK__AdPhone__AdCode__4BCC3ABA