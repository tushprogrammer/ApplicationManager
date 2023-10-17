SELECT TOP (1000) [Id]
      ,[Name]
      ,[Description]
  FROM [DataBaseApplication].[dbo].[Contacts]

  TRUNCATE TABLE Contacts

  INSERT INTO Contacts (Name, Description)
VALUES
    (N'Image_address', N'Default/address.png'),
    (N'Адрес', N'Ул. Пушкина. Дом Колотушкина'),
    (N'Телефон', N'88005553535'),
    (N'Факс', N'88005553535'),
    (N'Фио директора', N'Иванов Иван Иванович'),
    (N'Email', N'c#@mail.com');