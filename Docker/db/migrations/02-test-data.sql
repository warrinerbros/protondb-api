INSERT INTO `proton_db`.`Notes`
(
    `AppSelectionMethod`
)
VALUES
    (
        'Note about Launch'
    );

INSERT INTO `proton_db`.`Reports`
(
    `Title`,
    `AppId`,
    `Timestamp`,
    `AppSelectionMethod`,
    `NotesId`
)
VALUES
(
    'Mario 64',
    '123456',
    12345,
    'Launch',
    1
);