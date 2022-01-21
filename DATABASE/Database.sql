USE [sample1]
GO

CREATE TABLE Customer(Customer_Id INT IDENTITY(1,1) PRIMARY KEY,
                      First_Name VARCHAR(10),
                      Last_Name VARCHAR(10),
                      Email_Address VARCHAR(20),
                      Mobile_NUmber BIGINT,
                      PASSWORDD VARCHAR(50)
)

CREATE TABLE Service_Provider(Service_Provider_Id INT IDENTITY(1,1) PRIMARY KEY,
                              First_Name VARCHAR(10),
                              Last_Name VARCHAR(10),
                              Email_Address VARCHAR(20),
                              Mobile_NUmber BIGINT,
                              PASSWORDD VARCHAR(50),
                              DOB date,
                              Nationality VARCHAR(15),
                              Avatar VARCHAR(10),
                              Street_Name VARCHAR(15),
                              House_Number INT,
                              Postal_Code INT,
                              City VARCHAR(15),
                              Rating INT
)

CREATE TABLE Block_Cust_SP(WhoIsBlocking INT,
                           WhoIsBlocked INT
)

CREATE TABLE Customer_Address(Customer_Id INT FOREIGN KEY REFERENCES Customer(Customer_Id),
                              Cust_Address VARCHAR                
)

CREATE TABLE Book_Service(Service_Provider_Id INT IDENTITY(1,1) PRIMARY KEY,
                          Customer_Id INT FOREIGN KEY REFERENCES Customer(Customer_Id), 
                          Postal_Code INT,
                          Schedule_Date DATE,
                          Schedule_Time DATE,
                          Schedule_Duration INT,
                          Has_Extra_Service INT,
                          Comments VARCHAR(200),
                          Has_Pets BIT,
                          Service_Address VARCHAR(50)
)

CREATE TABLE Service_Status(Service_Id INT FOREIGN KEY REFERENCES Book_Service(Service_Provider_Id),
                            Service_Status INT,
                            Service_Provider_Id INT FOREIGN KEY REFERENCES Service_Provider(Service_Provider_Id)
)