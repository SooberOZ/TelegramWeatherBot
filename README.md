# TelegramWeatherBot
for the application to work properly, please create a database named "WeatherBot" and create 2 tables:

CREATE TABLE Users 
(
Id INT IDENTITY(1,1) PRIMARY KEY,
TelegramId BIGINT UNIQUE NOT NULL,
Name NVARCHAR(100) NOT NULL
);

CREATE TABLE WeatherHistory 
(
Id INT IDENTITY(1,1) PRIMARY KEY,
UserId INT NOT NULL,
City NVARCHAR(100) NOT NULL,
Temperature FLOAT NOT NULL,
Description NVARCHAR(255) NOT NULL,
Date DATETIME DEFAULT GETDATE(),
FOREIGN KEY (UserId) REFERENCES Users(Id)
);

C# .Net version 8.0
