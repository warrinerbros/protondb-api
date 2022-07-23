-- DB User with minimum rights necessary
USE proton_db;
    
CREATE USER 'proton_db_user'@'%' IDENTIFIED WITH mysql_native_password BY 'Abc1234';

GRANT ALL PRIVILEGES ON * TO 'proton_db_user'@'%';