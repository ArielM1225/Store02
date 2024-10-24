CREATE DATABASE STORE02

USE STORE02

CREATE TABLE Products (
ProductID INT IDENTITY(1, 1) PRIMARY KEY,
NameProduct NVARCHAR(100) NOT NULL,
DescriptionProduct NVARCHAR(255),
Price DECIMAL(10, 2) NOT NULL,
Stock INT NOT NULL DEFAULT 0 CHECK (Stock >=0),
CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
UpdatedAt DATETIME NULL
);


CREATE TABLE Customers (
CustomerID INT IDENTITY(1, 1) PRIMARY KEY,
FirstName NVARCHAR(100) NOT NULL,
LastName NVARCHAR(100) NOT NULL,
Email NVARCHAR(100) NOT NULL UNIQUE,
PhoneNumber NVARCHAR(15),
AddressCustomer NVARCHAR(255),
City NVARCHAR(100),
PostalCode NVARCHAR(10),
Country NVARCHAR(100),
CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
UpdatedAt DATETIME NULL
);


CREATE TABLE Suppliers (
SupplierID INT IDENTITY(1, 1) PRIMARY KEY,
SupplierName VARCHAR(100) NOT NULL,
ContactInfo NVARCHAR(255),
CreatedAt DATETIME NULL DEFAULT GETDATE(),
UpdatedAt DATETIME NULL
);


CREATE TABLE OrderPurchase (
OrderPID INT IDENTITY(1, 1) PRIMARY KEY,
SupplierID INT NOT NULL,
OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
TotalAmount DECIMAL(10, 2) NOT NULL DEFAULT 0,
StatusOrder NVARCHAR(20) NOT NULL CHECK (StatusOrder IN ('Pending', 'Completed', 'Canceled')),
FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
);


CREATE TABLE OrderSale (
OrderSID INT IDENTITY(1, 1) PRIMARY KEY,
CustomerID INT NOT NULL,
OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
TotalAmount DECIMAL(10, 2) NOT NULL DEFAULT 0,
StatusOrder NVARCHAR(20) NOT NULL CHECK (StatusOrder IN ('Pending', 'Completed', 'Canceled')),
FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
);


CREATE TABLE OrderPurchaseDetail (
    OrderDetailID INT IDENTITY(1, 1) PRIMARY KEY,
    OrderPID INT NOT NULL, -- Clave foránea a Order_Purchase
    ProductID INT NOT NULL, -- Clave foránea a Product
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderPID) REFERENCES OrderPurchase(OrderPID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


CREATE TABLE OrderSaleDetail (
    OrderDetailID INT IDENTITY(1, 1) PRIMARY KEY,
    OrderSID INT NOT NULL, -- Clave foránea a Order_Sale
    ProductID INT NOT NULL, -- Clave foránea a Product
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderSID) REFERENCES OrderSale(OrderSID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


CREATE TABLE OrderHistory (
    OrderHistoryID INT IDENTITY(1, 1) PRIMARY KEY,
    OrderID INT NOT NULL, -- Puede ser OrderID de Order_Sale o Order_Purchase
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    StatusOrder NVARCHAR(20) NOT NULL CHECK (StatusOrder IN ('Pending', 'Completed', 'Canceled')),
    OrderType NVARCHAR(10) NOT NULL CHECK (OrderType IN ('Sale', 'Purchase')),
    ProductID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Price DECIMAL(10, 2) NOT NULL CHECK (Price >= 0),
    HistoryDate DATETIME NOT NULL DEFAULT GETDATE()
);

USE STORE02

SELECT * FROM Customers;
SELECT * FROM Suppliers;
SELECT * FROM Products;
SELECT * FROM OrderHistory;
SELECT * FROM OrderPurchase;
SELECT * FROM OrderPurchaseDetail;

DROP TABLE OrderHistory;
DROP TABLE OrderPurchaseDetail
DROP TABLE OrderSaleDetail
DROP TABLE OrderPurchase
DROP TABLE OrderSale
DROP TABLE Products
DROP TABLE Customers
DROP TABLE Suppliers

DROP DATABASE STORE02
USE master

DELETE FROM OrderHistory
WHERE OrderID = 22

DELETE FROM OrderPurchaseDetail
WHERE OrderPID = 22

DELETE FROM OrderPurchase
WHERE OrderPID = 22