create table Customers
(
	Id int not null IDENTITY (1,1) PRIMARY KEY,
	[Name] varchar(100) not null,
	Balance decimal(12,4)
);

create table Orders
(
	Id int not null IDENTITY (1,1) PRIMARY KEY,
	CustomerId int not null FOREIGN KEY REFERENCES Customers(Id),
	OrderDate DateTime2 not null,
	OrderTotal decimal(12,4) not null
);

create table Products
(
	Id int not null IDENTITY (1,1) PRIMARY KEY,
	[Description] varchar(30) not null,
	ImageLocation varchar(100) not null,
	LargeUncessesaryDataField varBinary(MAX),
	Price decimal(12,4) not null
);

create table OrderItems
(
	Id int not null IDENTITY (1,1) PRIMARY KEY,
	OrderId int not null FOREIGN KEY REFERENCES Orders(Id),
	ProductId int not null FOREIGN KEY REFERENCES Products(Id),
	Quantity int not null,
	ItemPrice decimal(12,4) not null,
	LinePrice decimal(12,4) not null
);
