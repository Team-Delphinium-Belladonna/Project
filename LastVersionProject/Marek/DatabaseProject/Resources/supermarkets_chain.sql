CREATE DATABASE supermarkets_chain;

USE supermarkets_chain;

CREATE TABLE IF NOT EXISTS vendors(
	id INT NOT NULL AUTO_INCREMENT,
    vendor_name NVARCHAR(100) not null,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS measures(
	id INT NOT NULL AUTO_INCREMENT,
    measure_name NVARCHAR(50),
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS products(
	id INT NOT NULL AUTO_INCREMENT,
    vendor_id INT NOT NULL,
    product_name NVARCHAR(50) NOT NULL,
    measure_id INT NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_vendors
		FOREIGN KEY (vendor_id) 
		REFERENCES vendors (id),
    CONSTRAINT fk_measures
		FOREIGN KEY (measure_id) 
		REFERENCES measures (id)
);

CREATE TABLE IF NOT EXISTS expenses (
  id INT NOT NULL AUTO_INCREMENT,
  expense DECIMAL(8,2) NOT NULL ,
  PRIMARY KEY (id)
  );
  
CREATE TABLE IF NOT EXISTS vendors_expenses (
  vendor_id INT NOT NULL,
  expense_id INT NOT NULL,
  CONSTRAINT fk_vendors_expenses_vendors
    FOREIGN KEY (vendor_id)
    REFERENCES vendors (id),
  CONSTRAINT fk_vendors_expenses_expenses
    FOREIGN KEY (expense_id)
    REFERENCES expenses (id)
);

CREATE TABLE IF NOT EXISTS sales (
  id INT NOT NULL AUTO_INCREMENT,
  product_id INT NOT NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_products
    FOREIGN KEY (product_id)
    REFERENCES products (id)
);

INSERT INTO vendors(vendor_name) VALUE('Nestle Sofia Corp.');
INSERT INTO vendors(vendor_name) VALUE('Zagorka Corp.');
INSERT INTO vendors(vendor_name) VALUE('Targovishte Bottling Company Ltd.');
INSERT INTO vendors(vendor_name) VALUE('Coca-Cola');
INSERT INTO vendors(vendor_name) VALUE('Simid A&D');
INSERT INTO vendors(vendor_name) VALUE('Bella AD');


INSERT INTO measures(measure_name) VALUE('liters');
INSERT INTO measures(measure_name) VALUE('pieces');
INSERT INTO measures(measure_name) VALUE('kg');

INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(2 ,'Beer "Zagorka"', 1, 0.86);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(3 ,'Vodka "Targovishte"', 1, 7.56);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(2 ,'Beer "Beck\'s"', 1, 1.03);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(1 ,'Chocolate "Milka"', 3, 0.86);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(4 ,'Soda "Coca-Cola"', 1, 1.20);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(5 ,'Bread "Rustic Bread"', 3, 1.00);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(1 ,'Cereal "Nesquick"', 3, 4.60);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(5 ,'Bread "White Bread"', 3, 0.80);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(4 ,'Soda "Fanta"', 1, 1.10);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(5 ,'Bread "Brown Bread"', 3, 0.90);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(1 ,'Ice Cream "Magnum"', 2, 2.99);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(4 ,'Soda "Sprite"', 1, 1.15);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(2 ,'Cider "Strongbow"', 1, 1.60);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(1 ,'Chocolate "LZ"', 3, 1.40);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(6 ,'Ham "Leki"', 3, 3.49);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(4 ,'Ice Tea "Nestea"', 1, 1.59);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(2 ,'Beer "Amstel"', 1, 0.90);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(6 ,'Mince "Naroden"', 3, 1.20);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(1 ,'Biscuits "Zhiten Dar"', 2, 2.25);
INSERT INTO products(vendor_id, product_name, measure_id, price)
VALUES(4 ,'Juice "Cappy"', 1, 1.19);

INSERT INTO sales(product_id)
Values(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20);


INSERT INTO expenses(expense) 
Values 
(30),
(40),
(30),
(55),
(16),
(10);


INSERT INTO vendors_expenses(vendor_id, expense_id) 
Values 
(1, 1),
(2, 5),
(3, 4),
(4, 4),
(5, 3),
(6, 2);


