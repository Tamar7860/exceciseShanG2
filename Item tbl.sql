USE firstdb;
CREATE TABLE item (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,  
    Name VARCHAR(100),
    AnotherId INT NOT NULL 
);