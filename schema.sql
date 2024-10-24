CREATE TABLE Campaigns (
  PostCodeSector VARCHAR(10) NULL,
  Uprn BIGINT NULL,
  Campaign VARCHAR(100) NOT NULL,
  Date DATETIME NOT NULL);

DELIMITER $$
CREATE PROCEDURE Campaigns_Insert
	(PostCodeSector VARCHAR(10), Uprn BIGINT, Campaign VARCHAR(100), Date DATETIME)
BEGIN
	INSERT Campaigns
		(PostCodeSector, Uprn, Campaign, Date)
	VALUES
		(PostCodeSector, Uprn, Campaign, Date);
END$$

DELIMITER ;
