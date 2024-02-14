select distinct adCode, a.offerNumber, s.SOURCENAME_001, a.onSale,
concat('insert into AdDetails(AdCode, AdVersionID, PrimaryAdName, StartDate) 
values(''', a.adCode, ''', ''', offerNumber, ''', ''', rtrim(replace(s.SOURCENAME_001, '''', '')),''', ''', cast(onSale as date), ''')')
FROM dbo.ADVERTISING a inner join 
		SOURCES s on a.adCode = s.REFSOURCE inner join
		DW_PUB p on a.pubCode = p.pubCode
where 
((adCode >= '120000' and adCode < '130000' and len(adCode) > 4)
or 
(adCode >= '603000' and adCode < '613000' and len(adCode) > 4))
--order by a.pubCode


