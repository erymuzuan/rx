USE [Sph]
GO

TRUNCATE TABLE [Sph]. [ContractTemplate]
GO

SET IDENTITY_INSERT [Sph].[ContractTemplate] ON
INSERT INTO [Sph].[ContractTemplate] ([ContractTemplateId], [Type], [Data], [CreatedDate], [CreatedBy], [ChangedDate], [ChangedBy]) VALUES (26, N'Kontrak Baru', N'<ContractTemplate xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.bespoke.com.my/" WebId="388b0f8b-3a60-4c00-a69d-d0ddbccb7fce" CreatedDate="0001-01-01T00:00:00" ChangedDate="0001-01-01T00:00:00" ContractTemplateId="26" Type="Kontrak Baru" Description="jana kontrak untuk penyewa baru" InterestRate="0"><DocumentTemplateCollection><DocumentTemplate Name="Surat Tawaran" StoreId="dbc5f0ad-5e5f-4212-905e-6a5d6e1f763b" /></DocumentTemplateCollection><TopicCollection /></ContractTemplate>', N'2013-06-19 15:58:00', N'-', N'2013-08-22 16:10:00', N'-')
SET IDENTITY_INSERT [Sph].[ContractTemplate] OFF
