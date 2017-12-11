CREATE INDEX idx_Customer_Json_AccountNo
ON [DevV1].[Customer]([AccountNo])  
GO
CREATE INDEX idx_Customer_Json_FirstName
ON [DevV1].[Customer]([FirstName])  
GO
CREATE INDEX idx_Customer_Json_LastName
ON [DevV1].[Customer]([LastName])  
GO
CREATE INDEX idx_Customer_Json_Age
ON [DevV1].[Customer]([Age])  
GO
CREATE INDEX idx_Customer_Json_Gender
ON [DevV1].[Customer]([Gender])  
GO
CREATE INDEX idx_Customer_Json_IsPriority
ON [DevV1].[Customer]([IsPriority])  
GO
CREATE INDEX idx_Customer_Json_RegisteredDate
ON [DevV1].[Customer]([RegisteredDate])  
GO
CREATE INDEX idx_Customer_Json_Rating
ON [DevV1].[Customer]([Rating])  
GO
CREATE INDEX idx_Customer_Json_PrimaryContact
ON [DevV1].[Customer]([PrimaryContact])  
GO
CREATE INDEX idx_Customer_Json_Department
ON [DevV1].[Customer]([Department])  
GO
CREATE INDEX idx_Customer_Json_AddressState
ON [DevV1].[Customer]([Address.State])  
GO
CREATE INDEX idx_Customer_Json_AddressLocality
ON [DevV1].[Customer]([Address.Locality])  
GO
CREATE INDEX idx_Customer_Json_ContactName
ON [DevV1].[Customer]([Contact.Name])  