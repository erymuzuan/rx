USE [Sph]
GO

TRUNCATE TABLE  [Sph].[Space]
GO
SET IDENTITY_INSERT [Sph].[Space] ON
INSERT INTO [Sph].[Space] ([SpaceId],[TemplateName], [TemplateId], [BuildingId], [FloorName], [RegistrationNo], [LotName], [BuildingName], [Status], [Category], [IsOnline], [IsAvailable], [RentalType], [State], [City], [Data], [CreatedDate], [CreatedBy], [ChangedDate], [ChangedBy]) VALUES (16,'Cafeteria', 1, 34, N'Tingkat 2', N'BPH/BGS/Kafe/2013/001', N'WPJB/02/03,', N'Wisma Persekutuan Johor Baharu', NULL, N'Kafeteria', 1, 1, NULL, N'Johor', N'Johor Baharu', N'<Space xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.bespoke.com.my/" WebId="a0f09179-6e5a-442e-a55d-f6b16ea2bb9c" CreatedDate="0001-01-01T00:00:00" ChangedDate="0001-01-01T00:00:00" SpaceId="16" BuildingId="34" LotName="WPJB/02/03," FloorName="Tingkat 2" Size="500" Category="Kafeteria" IsOnline="true" RegistrationNo="BPH/BGS/Kafe/2013/001" IsAvailable="true" ContactPerson="Ruzzaima" State="Johor" City="Johor Baharu" BuildingName="Wisma Persekutuan Johor Baharu" BuildingLot="A503-101-101" RentalRate="1500" TemplateId="4"><ApplicationTemplateOptions><int>3</int></ApplicationTemplateOptions><LotCollection /><CustomFieldValueCollection><CustomFieldValue WebId="1f2676df-7c36-4618-80e5-2473d7ff01a6" Name="CafeteriaType" Type="String" Value="Tempatan" /><CustomFieldValue WebId="f4a25599-9350-41af-a6ef-4e5f9cdefbe3" Value="Bulanan" /></CustomFieldValueCollection><Address /></Space>', N'2013-08-22 15:27:00', N'-', N'2013-08-22 15:28:00', N'-')
SET IDENTITY_INSERT [Sph].[Space] OFF