SELECT TOP (1000) [Id]
      ,[Name]
      ,[Description]
  FROM [DataBaseApplication].[dbo].[Contacts]

  TRUNCATE TABLE Contacts

  INSERT INTO Contacts (Name, Description)
VALUES
    (N'Image_address', N'Default/address.png'),
    (N'�����', N'��. �������. ��� �����������'),
    (N'�������', N'88005553535'),
    (N'����', N'88005553535'),
    (N'��� ���������', N'������ ���� ��������'),
    (N'Email', N'c#@mail.com');