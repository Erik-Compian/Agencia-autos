-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-04-2026 a las 01:05:21
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `agencia_autos`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `administrador`
--

CREATE TABLE `administrador` (
  `Id_Admin` int(11) NOT NULL,
  `Usuario` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `administrador`
--

INSERT INTO `administrador` (`Id_Admin`, `Usuario`, `Password`, `Nombre`, `Email`) VALUES
(1, 'admin', 'admin01', 'Administrador Principal', 'admin@agencia.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cliente`
--

CREATE TABLE `cliente` (
  `Id_Cliente` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Email` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cliente`
--

INSERT INTO `cliente` (`Id_Cliente`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
(1, 'Aldair', 'Lopez', '8445379136', 'aldairlopez@uadec.edu.mx'),
(2, 'Erik', 'Compian', '8448080876', 'erikc123@gmail.com'),
(3, 'Diego ', 'Gomez', '8445698978', 'diegogmz25@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `marca`
--

CREATE TABLE `marca` (
  `Id_Marca` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `marca`
--

INSERT INTO `marca` (`Id_Marca`, `Nombre`) VALUES
(5, 'Chevrolet'),
(3, 'Honda'),
(2, 'Nissan'),
(1, 'Toyota'),
(4, 'Volkswagen');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `modelo`
--

CREATE TABLE `modelo` (
  `Id_Modelo` int(11) NOT NULL,
  `Id_Marca` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Anio` year(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `modelo`
--

INSERT INTO `modelo` (`Id_Modelo`, `Id_Marca`, `Nombre`, `Anio`) VALUES
(1, 3, 'Civic', '2010'),
(4, 5, 'Aveo', '2024'),
(5, 5, 'Silverado', '2023'),
(6, 3, 'CR-V', '2024'),
(7, 2, 'Sentra', '2024'),
(8, 2, 'Frontier', '2023'),
(9, 1, 'Corolla', '2024'),
(10, 1, 'Hilux', '2022'),
(11, 4, 'Jetta', '2024'),
(12, 4, 'Tiguan', '2024');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proximo_servicio`
--

CREATE TABLE `proximo_servicio` (
  `Id_Prox_Serv` int(11) NOT NULL,
  `Folio` int(11) NOT NULL,
  `Fecha_Prog` date NOT NULL,
  `Km_Proximo` int(11) DEFAULT NULL,
  `Notas` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proximo_servicio`
--

INSERT INTO `proximo_servicio` (`Id_Prox_Serv`, `Folio`, `Fecha_Prog`, `Km_Proximo`, `Notas`) VALUES
(1, 1, '2026-10-25', 97000, 'Le toca cambio de bujías en 6 meses'),
(2, 3, '2026-10-25', 35000, 'Sistema: Recordatorio automático (6 meses o 10,000 km).');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `refaccion`
--

CREATE TABLE `refaccion` (
  `Id_Refaccion` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Codigo` varchar(50) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Stock` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `refaccion`
--

INSERT INTO `refaccion` (`Id_Refaccion`, `Nombre`, `Codigo`, `Precio`, `Stock`) VALUES
(1, 'Aceite Sintético 5W-30', 'ACE-001', 250.00, 48),
(2, 'Filtro de Aceite', 'FIL-002', 120.00, 30),
(3, 'Bujía Iridium', 'BUJ-003', 180.00, 100),
(4, 'Balatas Delanteras', 'BAL-004', 850.00, 20);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `servicio`
--

CREATE TABLE `servicio` (
  `Folio` int(11) NOT NULL,
  `Id_Vehiculo` int(11) NOT NULL,
  `Id_Admin` int(11) NOT NULL,
  `Id_Tipo_Serv` int(11) NOT NULL,
  `Quien_Entrego` varchar(100) NOT NULL,
  `Fecha_Ingreso` date NOT NULL,
  `Fecha_Salida` date DEFAULT NULL,
  `Estatus` enum('En espera','En proceso','Finalizado') NOT NULL DEFAULT 'En espera',
  `Descripcion` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `servicio`
--

INSERT INTO `servicio` (`Folio`, `Id_Vehiculo`, `Id_Admin`, `Id_Tipo_Serv`, `Quien_Entrego`, `Fecha_Ingreso`, `Fecha_Salida`, `Estatus`, `Descripcion`) VALUES
(1, 1, 1, 1, 'William Wallace', '2026-04-25', '2026-04-25', 'Finalizado', 'Revisión general de rutina.'),
(2, 1, 1, 2, 'Aldair Lopez', '2026-04-25', NULL, 'En proceso', 'Requiere diagnóstico por falla.'),
(3, 2, 1, 1, 'Diego Gomez', '2026-04-25', '2026-04-25', 'Finalizado', 'Revisión general de rutina.');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `servicio_refaccion`
--

CREATE TABLE `servicio_refaccion` (
  `Folio` int(11) NOT NULL,
  `Id_Refaccion` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL DEFAULT 1,
  `Precio_Aplicado` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `servicio_refaccion`
--

INSERT INTO `servicio_refaccion` (`Folio`, `Id_Refaccion`, `Cantidad`, `Precio_Aplicado`) VALUES
(1, 1, 1, 250.00),
(2, 1, 2, 250.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_servicio`
--

CREATE TABLE `tipo_servicio` (
  `Id_Tipo_Serv` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL,
  `Precio` decimal(10,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo_servicio`
--

INSERT INTO `tipo_servicio` (`Id_Tipo_Serv`, `Nombre`, `Descripcion`, `Precio`) VALUES
(1, 'Preventivo', 'Mantenimiento programado para prevenir fallas', 850.00),
(2, 'Correctivo', 'Reparación de fallas existentes en el vehículo', 1250.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vehiculo`
--

CREATE TABLE `vehiculo` (
  `Id_Vehiculo` int(11) NOT NULL,
  `Id_Cliente` int(11) NOT NULL,
  `Id_Modelo` int(11) NOT NULL,
  `Num_Serie` varchar(50) NOT NULL,
  `Color` varchar(30) DEFAULT NULL,
  `Placa` varchar(20) NOT NULL,
  `Km_Actual` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `vehiculo`
--

INSERT INTO `vehiculo` (`Id_Vehiculo`, `Id_Cliente`, `Id_Modelo`, `Num_Serie`, `Color`, `Placa`, `Km_Actual`) VALUES
(1, 1, 1, '800142fg', 'Plata', 'FKD9156', 92000),
(2, 3, 4, '3G1BE51RS123456', 'Negro', 'MEX2026', 25000);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `administrador`
--
ALTER TABLE `administrador`
  ADD PRIMARY KEY (`Id_Admin`),
  ADD UNIQUE KEY `Usuario` (`Usuario`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indices de la tabla `cliente`
--
ALTER TABLE `cliente`
  ADD PRIMARY KEY (`Id_Cliente`);

--
-- Indices de la tabla `marca`
--
ALTER TABLE `marca`
  ADD PRIMARY KEY (`Id_Marca`),
  ADD UNIQUE KEY `Nombre` (`Nombre`);

--
-- Indices de la tabla `modelo`
--
ALTER TABLE `modelo`
  ADD PRIMARY KEY (`Id_Modelo`),
  ADD KEY `FK_Mod_Marca` (`Id_Marca`);

--
-- Indices de la tabla `proximo_servicio`
--
ALTER TABLE `proximo_servicio`
  ADD PRIMARY KEY (`Id_Prox_Serv`),
  ADD UNIQUE KEY `Folio` (`Folio`);

--
-- Indices de la tabla `refaccion`
--
ALTER TABLE `refaccion`
  ADD PRIMARY KEY (`Id_Refaccion`),
  ADD UNIQUE KEY `Codigo` (`Codigo`);

--
-- Indices de la tabla `servicio`
--
ALTER TABLE `servicio`
  ADD PRIMARY KEY (`Folio`),
  ADD KEY `FK_Ser_Vehiculo` (`Id_Vehiculo`),
  ADD KEY `FK_Ser_Admin` (`Id_Admin`),
  ADD KEY `FK_Ser_TipoServ` (`Id_Tipo_Serv`);

--
-- Indices de la tabla `servicio_refaccion`
--
ALTER TABLE `servicio_refaccion`
  ADD PRIMARY KEY (`Folio`,`Id_Refaccion`),
  ADD KEY `FK_SR_Refaccion` (`Id_Refaccion`);

--
-- Indices de la tabla `tipo_servicio`
--
ALTER TABLE `tipo_servicio`
  ADD PRIMARY KEY (`Id_Tipo_Serv`),
  ADD UNIQUE KEY `Nombre` (`Nombre`);

--
-- Indices de la tabla `vehiculo`
--
ALTER TABLE `vehiculo`
  ADD PRIMARY KEY (`Id_Vehiculo`),
  ADD UNIQUE KEY `Num_Serie` (`Num_Serie`),
  ADD UNIQUE KEY `Placa` (`Placa`),
  ADD KEY `FK_Veh_Cliente` (`Id_Cliente`),
  ADD KEY `FK_Veh_Modelo` (`Id_Modelo`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `administrador`
--
ALTER TABLE `administrador`
  MODIFY `Id_Admin` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `cliente`
--
ALTER TABLE `cliente`
  MODIFY `Id_Cliente` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `marca`
--
ALTER TABLE `marca`
  MODIFY `Id_Marca` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `modelo`
--
ALTER TABLE `modelo`
  MODIFY `Id_Modelo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `proximo_servicio`
--
ALTER TABLE `proximo_servicio`
  MODIFY `Id_Prox_Serv` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `refaccion`
--
ALTER TABLE `refaccion`
  MODIFY `Id_Refaccion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `servicio`
--
ALTER TABLE `servicio`
  MODIFY `Folio` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `tipo_servicio`
--
ALTER TABLE `tipo_servicio`
  MODIFY `Id_Tipo_Serv` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `vehiculo`
--
ALTER TABLE `vehiculo`
  MODIFY `Id_Vehiculo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `modelo`
--
ALTER TABLE `modelo`
  ADD CONSTRAINT `FK_Mod_Marca` FOREIGN KEY (`Id_Marca`) REFERENCES `marca` (`Id_Marca`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `proximo_servicio`
--
ALTER TABLE `proximo_servicio`
  ADD CONSTRAINT `FK_Prox_Servicio` FOREIGN KEY (`Folio`) REFERENCES `servicio` (`Folio`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `servicio`
--
ALTER TABLE `servicio`
  ADD CONSTRAINT `FK_Ser_Admin` FOREIGN KEY (`Id_Admin`) REFERENCES `administrador` (`Id_Admin`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Ser_TipoServ` FOREIGN KEY (`Id_Tipo_Serv`) REFERENCES `tipo_servicio` (`Id_Tipo_Serv`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Ser_Vehiculo` FOREIGN KEY (`Id_Vehiculo`) REFERENCES `vehiculo` (`Id_Vehiculo`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `servicio_refaccion`
--
ALTER TABLE `servicio_refaccion`
  ADD CONSTRAINT `FK_SR_Refaccion` FOREIGN KEY (`Id_Refaccion`) REFERENCES `refaccion` (`Id_Refaccion`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_SR_Servicio` FOREIGN KEY (`Folio`) REFERENCES `servicio` (`Folio`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `vehiculo
`
--
ALTER TABLE `vehiculo`
  ADD CONSTRAINT `FK_Veh_Cliente` FOREIGN KEY (`Id_Cliente`) REFERENCES `cliente` (`Id_Cliente`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Veh_Modelo` FOREIGN KEY (`Id_Modelo`) REFERENCES `modelo` (`Id_Modelo`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;