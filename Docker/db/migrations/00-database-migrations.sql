CREATE DATABASE `proton_db`;

CREATE TABLE `proton_db`.`Notes`(
    `Id` INT NOT NULL AUTO_INCREMENT,
    `AnswerToWhatGame` LONGTEXT,
    `AppSelectionMethod` LONGTEXT,
    `AudioFaults` LONGTEXT,
    `Duration` LONGTEXT,
    `Extra` LONGTEXT,
    `GraphicalFaults` LONGTEXT,
    `InputFaults` LONGTEXT,
    `Installs` LONGTEXT,
    `IsImpactedByAntiCheat` LONGTEXT,
    `IsMultiplayerImportant` LONGTEXT,
    `LocalMultiplayerAttempted` LONGTEXT,
    `OnlineMultiplayerAttempted` LONGTEXT,
    `Opens` LONGTEXT,
    `PerformanceFaults` LONGTEXT,
    `ProtonVersion` LONGTEXT,
    `SaveGameFaults` LONGTEXT,
    `SignificantBugs` LONGTEXT,
    `StabilityFaults` LONGTEXT,
    `StartsPlay` LONGTEXT,
    `Type` LONGTEXT,
    `Verdict` LONGTEXT,
    `WindowingFaults` LONGTEXT,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `proton_db`.`Reports`(
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Title` LONGTEXT NOT NULL,
    `AppId` INT NOT NULL,
    `Timestamp` INT NOT NULL,
    `Cpu` LONGTEXT,
    `Gpu` LONGTEXT,
    `GpuDriver` LONGTEXT,
    `Kernel` LONGTEXT,
    `Os` LONGTEXT,
    `Ram` LONGTEXT,
    `AppSelectionMethod` LONGTEXT,
    `AudioFaults` LONGTEXT,
    `Duration` LONGTEXT,
    `Extra` LONGTEXT,
    `GraphicalFaults` LONGTEXT,
    `InputFaults` LONGTEXT,
    `Installs` LONGTEXT,
    `IsImpactedByAntiCheat` LONGTEXT,
    `IsMultiplayerImportant` LONGTEXT,
    `LocalMultiplayerAttempted` LONGTEXT,
    `NotesId` INT,
    `OnlineMultiplayerAttempted` LONGTEXT,
    `Opens` LONGTEXT,
    `PerformanceFaults` LONGTEXT,
    `ProtonVersion` LONGTEXT,
    `SaveGameFaults` LONGTEXT,
    `SignificantBugs` LONGTEXT,
    `StabilityFaults` LONGTEXT,
    `StartsPlay` LONGTEXT,
    `Type` LONGTEXT,
    `Verdict` LONGTEXT,
    `WindowingFaults` LONGTEXT,
    PRIMARY KEY (`Id`),
    FOREIGN KEY (NotesId) REFERENCES `proton_db`.`Notes`(Id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;