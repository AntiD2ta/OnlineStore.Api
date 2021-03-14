DROP TABLE IF EXISTS AspNetRoleClaims;
CREATE TABLE `AspNetRoleClaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4,
  `ClaimValue` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS AspNetRoles;
CREATE TABLE `AspNetRoles` (
  `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS AspNetUserClaims;
CREATE TABLE `AspNetUserClaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4,
  `ClaimValue` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS AspNetUserLogins;
CREATE TABLE `AspNetUserLogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4,
  `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS AspNetUserRoles;
CREATE TABLE `AspNetUserRoles` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS AspNetUsers;
CREATE TABLE `AspNetUsers` (
  `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `Nombre` longtext CHARACTER SET utf8mb4,
  `Apellido` longtext CHARACTER SET utf8mb4,
  `Role` longtext CHARACTER SET utf8mb4,
  `UserName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4,
  `SecurityStamp` longtext CHARACTER SET utf8mb4,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4,
  `PhoneNumber` longtext CHARACTER SET utf8mb4,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS AspNetUserTokens;
CREATE TABLE `AspNetUserTokens` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS Ordenes;
CREATE TABLE `Ordenes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha` datetime(6) NOT NULL,
  `Estado` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `UsuarioId` varchar(255) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ProductoId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Ordenes_ProductoId` (`ProductoId`),
  KEY `IX_Ordenes_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_Ordenes_AspNetUsers_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `AspNetUsers` (`Id`),
  CONSTRAINT `FK_Ordenes_Productos_ProductoId` FOREIGN KEY (`ProductoId`) REFERENCES `Productos` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS Productos;
CREATE TABLE `Productos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
  `Descripcion` varchar(100) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Cantidad` int(11) NOT NULL,
  `Slug` longtext CHARACTER SET utf8mb4,
  `Precio` decimal(65,30) NOT NULL,
  `UsuarioId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Productos_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_Productos_AspNetUsers_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `AspNetUsers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;









INSERT INTO AspNetUsers(Id,Nombre,Apellido,Role,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount) VALUES('84f4e104-1f7b-4c71-a6c5-903f439b5708',X'456c6261',X'4c617a6f',X'56656e646f72','vendor','VENDOR','vendor@gmail.com','VENDOR@GMAIL.COM',0,X'4151414141414541414363514141414145505835687148676644672b79346b4e394e734f5449426974466f71684b7278344f4b6930373965747469663745365434564847484f37786d5a6b7451716b376c773d3d',X'4b5a3646474f594d4f4e4b4b4e564a47504654434c504a425733435645513545',X'36353561363034342d653030332d343538632d386230332d373131353230323436333339',NULL,0,0,NULL,1,0),('af505e75-dae6-4fe1-a8c6-395edb588130',X'456c7361',X'506f6c696e646f',X'436c69656e74','client','CLIENT','client@gmail.com','CLIENT@GMAIL.COM',0,X'415141414141454141436351414141414550504b656b4e6a6438637153756f4a37534165687a6c327543332b323666745a556f6b7362512b70396455757866715944752f4f68377a316e496c74794a4672413d3d',X'33355743533658414932504e4b3357434f4e4745424850425a4b4b354d4a5435',X'62636662346166612d303136332d346364302d623038652d323363363233373136336339',NULL,0,0,NULL,1,0),('b3c4c3cf-f7ab-474c-bb01-b8fda0ce5b2f',X'416c616e',X'427269746f',X'41646d696e6973747261746f72','admin','ADMIN','admin@gmail.com','ADMIN@GMAIL.COM',0,X'41514141414145414143635141414141454a61483565444b30584979774a4d5135524f386c465865365a4e6c677452596c306b77555376697a4c516a6e2b594b6d5074652b464748754856575247466b7a773d3d',X'4f4b45564332354245473557375343424d45424d464532525a56465346554a50',X'66356164633330632d643864632d343735642d396536392d386636373463346436376432',NULL,0,0,NULL,1,0);


INSERT INTO Productos(Id,Nombre,Descripcion,Cantidad,Slug,Precio,UsuarioId) VALUES(1,X'4d617274696c6c6f','Martillo para martillar',8,X'6d617274696c6c6f2d7072656d69756d',35.000000000000000000000000000000,'b3c4c3cf-f7ab-474c-bb01-b8fda0ce5b2f');







