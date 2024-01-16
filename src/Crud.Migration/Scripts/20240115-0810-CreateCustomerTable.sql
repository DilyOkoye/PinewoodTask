--You need to check if the table exists
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TableName' and xtype='U')
BEGIN
    CREATE TABLE tblCustomers 
  (
      Id uniqueidentifier NOT NULL,
    Name varchar(200)  NOT NULL,
    Email varchar(200)   NULL,
    Phone varchar(200)   NULL,
    Subscribed bit  NULL,
    CONSTRAINT tblCustomers_pkey PRIMARY KEY (Id)
  )
END