DROP DATABASE IF EXISTS `chain_of_supermarkets`;
CREATE DATABASE `chain_of_supermarkets` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `chain_of_supermarkets`;

CREATE TABLE IF NOT EXISTS `measures` (
	`id` int NOT NULL AUTO_INCREMENT,
	`name` nvarchar(50) NOT NULL,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `vendors` (
	`id` int NOT NULL AUTO_INCREMENT,
	`name` nvarchar(50) NOT NULL,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `products` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` nvarchar(50) NOT NULL,
  `price` decimal(18,2) NOT NULL,
  `vendor_id` int NOT NULL,
  `measure_id` int NOT NULL,
  PRIMARY KEY (`id`),
  FOREIGN KEY (`vendor_id`) REFERENCES `vendors` (`id`),
  FOREIGN KEY (`measure_id`) REFERENCES `measures` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `locations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` nvarchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `sales` (
  `id` int NOT NULL AUTO_INCREMENT,
  `quantity` decimal(18,2) NOT NULL,
  `date_of_sale` datetime NOT NULL,
  `product_id` int NOT NULL,
  `location_id` int NOT NULL,
  PRIMARY KEY (`id`),
  FOREIGN KEY (`product_id`) REFERENCES `products` (`id`),
  FOREIGN KEY (`location_id`) REFERENCES `locations` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `expenses` (
  `id` int NOT NULL AUTO_INCREMENT,
  `expense_value` decimal(18,2) NOT NULL,
  `expense_month` datetime NOT NULL,
  `vendor_id` int NOT NULL,
  PRIMARY KEY (`id`),
  FOREIGN KEY (`vendor_id`) REFERENCES `vendors` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;