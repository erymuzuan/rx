﻿SET IDENTITY_INSERT [Sph].[BuildingTemplate] ON
INSERT INTO [Sph].[BuildingTemplate] ([BuildingTemplateId], [Name], [Data], [IsActive], [CreatedDate], [CreatedBy], [ChangedDate], [ChangedBy]) VALUES (1, N'Bangunan Kerajaan', N'<BuildingTemplate xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.bespoke.com.my/" WebId="311fc503-9bed-4e1e-82dd-ca5fc2c742ff" CreatedDate="0001-01-01T00:00:00" ChangedDate="0001-01-01T00:00:00" BuildingTemplateId="1" Name="Bangunan Kerajaan" Description="template untuk bangunan kerajaan" IsActive="true"><CustomFieldCollection /><FormDesign Name="Bangunan Kerajaan" Description="Maklumat bangunan kerajaan"><FormElementCollection><FormElement xsi:type="TextBox" WebId="64157bec-519f-45c7-b7b6-44c8b64d3782" Name="Single line text" Label="Nama" Tooltip="" Path="Name" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="e92d5d9b-eac1-445d-9f6e-02985a7fabab" HelpText="" DefaultValue=""><MinLength xsi:nil="true" /><MaxLength xsi:nil="true" /></FormElement><FormElement xsi:type="AddressElement" WebId="da22a915-01d2-44c3-bfd7-be58b50bd399" Name="Address" Label="Alamat" Tooltip="" Path="Address" IsRequired="false" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="e87aa2e1-d575-4074-b3f9-f4b4f1ccf106" HelpText="" /><FormElement xsi:type="BuildingMapElement" Name="Show map button" Label="Peta Lokasi" Tooltip="" Path="" IsRequired="false" Size="input-large" CssClass="btn-success" Visible="true" Enable="" ElementId="ab86c8a7-d250-448a-a540-ef4462661389" HelpText="" Icon="icon-globe" /><FormElement xsi:type="BuildingFloorsElement" Name="Floors Table" Label="Lantai" Tooltip="" Path="FloorCollection" IsRequired="false" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="622a3986-1b31-4a28-b56c-3c0760320041" HelpText="" /></FormElementCollection></FormDesign></BuildingTemplate>', 1, N'2013-07-29 12:21:00', N'-', N'2013-08-22 15:07:00', N'-')
INSERT INTO [Sph].[BuildingTemplate] ([BuildingTemplateId], [Name], [Data], [IsActive], [CreatedDate], [CreatedBy], [ChangedDate], [ChangedBy]) VALUES (2, N'Bangunan Swasta', N'<BuildingTemplate xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.bespoke.com.my/" WebId="36035111-37bf-4223-be23-ef5e4e019600" CreatedDate="0001-01-01T00:00:00" ChangedDate="0001-01-01T00:00:00" BuildingTemplateId="2" Name="Bangunan Swasta" Description="template untuk bangunan swasta" IsActive="true"><CustomFieldCollection><CustomField Order="0" Name="" IsRequired="true" Type="String"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField><CustomField Order="0" Name="BuildDateTime" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField><CustomField Order="0" Name="DoneDate" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField></CustomFieldCollection><FormDesign Name="Bangunan Swasta" Description="Maklumat Bangunan Swasta"><FormElementCollection><FormElement xsi:type="TextBox" WebId="a0efd12a-238b-4f7f-87e5-9df3c5e3b6e5" Name="Single line text" Label="Nama" Tooltip="" Path="" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="dc544ba8-a349-418f-9fb9-f32bdb107857" HelpText="" DefaultValue=""><CustomField Order="0" Name="" IsRequired="true" Type="String"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField><MinLength xsi:nil="true" /><MaxLength xsi:nil="true" /></FormElement><FormElement xsi:type="DatePicker" WebId="82df8bba-fcf6-4e01-a2a5-9337b3b15178" Name="Tarikh" Label="Tarikh Dibina" Tooltip="" Path="BuildDateTime" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="cd5cb79a-bcd8-4a47-a816-b840b6c52b51" HelpText=""><CustomField Order="0" Name="BuildDateTime" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField></FormElement><FormElement xsi:type="DatePicker" WebId="82df8bba-fcf6-4e01-a2a5-9337b3b15178" Name="Tarikh" Label="Tarikh Siap" Tooltip="" Path="DoneDate" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="efc01b28-b385-4123-8470-7462002e689b" HelpText=""><CustomField Order="0" Name="DoneDate" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField></FormElement><FormElement xsi:type="AddressElement" WebId="55cc5a31-d420-46d6-9f8d-0651e65a476e" Name="Address" Label="Alamat" Tooltip="" Path="Address" IsRequired="false" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="5eaf4253-6aac-4ccd-964d-00518c38c3a2" HelpText="" /><FormElement xsi:type="BuildingMapElement" Name="Show map button" Label="Tunjuk Peta" Tooltip="" Path="" IsRequired="false" Size="input-large" CssClass="btn-success " Visible="true" Enable="" ElementId="bb4a20bf-3dba-4a2a-873e-10a1d5954ce4" HelpText="" Icon="icon-globe" /><FormElement xsi:type="BuildingFloorsElement" Name="Floors Table" Label="Lantai" Tooltip="" Path="FloorCollection" IsRequired="false" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="e488729d-423a-4c18-a38e-55d43cb9892d" HelpText="" /></FormElementCollection></FormDesign></BuildingTemplate>', 1, N'2013-07-29 12:23:00', N'-', N'2013-08-22 15:10:00', N'-')
INSERT INTO [Sph].[BuildingTemplate] ([BuildingTemplateId], [Name], [Data], [IsActive], [CreatedDate], [CreatedBy], [ChangedDate], [ChangedBy]) VALUES (7, N'Bangunan Pejabat', N'<BuildingTemplate xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.bespoke.com.my/" WebId="812a32a8-7983-43d1-846c-101ed5adc6b5" CreatedDate="0001-01-01T00:00:00" ChangedDate="0001-01-01T00:00:00" BuildingTemplateId="7" Name="Bangunan Pejabat" Description="" IsActive="true"><CustomFieldCollection><CustomField Order="0" Name="" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField><CustomField Order="0" Name="DoneDateTime" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField><CustomField Order="0" Name="Contractor" IsRequired="true" Type="String"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField></CustomFieldCollection><FormDesign Name="Bangunan Pejabat" Description="Maklumat yang diperlukan bagi ruang pejabat" ConfirmationText="" ImageStoreId=""><FormElementCollection><FormElement xsi:type="TextBox" WebId="c1549be3-4c4f-43a6-82f3-9353840a4539" Name="Single line text" Label="Nama" Tooltip="" Path="Name" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="70b10a5e-9c1d-4ca7-b2ad-46c63bcf5a81" HelpText="" DefaultValue=""><MinLength xsi:nil="true" /><MaxLength xsi:nil="true" /></FormElement><FormElement xsi:type="DatePicker" WebId="8ea176aa-f40d-4c91-8377-67ed9d43e8fd" Name="Tarikh" Label="Tarikh Dibina" Tooltip="" Path="" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="4451f990-4162-4c74-906e-416cf6c17213" HelpText=""><CustomField Order="0" Name="" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField></FormElement><FormElement xsi:type="DatePicker" WebId="8ea176aa-f40d-4c91-8377-67ed9d43e8fd" Name="Tarikh" Label="Tarikh Disiapkan" Tooltip="" Path="DoneDateTime" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="f3d7715a-f2b8-4ce4-ad66-0b20a982a84d" HelpText=""><CustomField Order="0" Name="DoneDateTime" IsRequired="true" Type="DateTime"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField></FormElement><FormElement xsi:type="TextBox" WebId="7daef20a-0675-4889-a3ab-68aa02b4c732" Name="Single line text" Label="Pemaju" Tooltip="" Path="Contractor" IsRequired="true" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="2b049256-cc9d-4eaa-994b-95af93b48e1c" HelpText="" DefaultValue=""><CustomField Order="0" Name="Contractor" IsRequired="true" Type="String"><MaxLength xsi:nil="true" /><MinLength xsi:nil="true" /></CustomField><MinLength xsi:nil="true" /><MaxLength xsi:nil="true" /></FormElement><FormElement xsi:type="AddressElement" WebId="bdb9b2ff-bb45-40b7-8aef-cfb4dad9827b" Name="Address" Label="Alamat" Tooltip="" Path="Address" IsRequired="false" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="51e9ef7b-7e77-45fe-a897-4083e49c2bac" HelpText="" /><FormElement xsi:type="BuildingFloorsElement" Name="Floors Table" Label="Label 1" Tooltip="" Path="FloorCollection" IsRequired="false" Size="input-large" CssClass="" Visible="true" Enable="" ElementId="33b24dd1-0fac-4d72-b707-091c0ce08b44" HelpText="" /></FormElementCollection></FormDesign></BuildingTemplate>', 1, N'2013-08-19 12:21:00', N'-', N'2013-08-22 15:14:00', N'-')
SET IDENTITY_INSERT [Sph].[BuildingTemplate] OFF
