create database BDCARRITO

GO

USE BDCARRITO

GO

CREATE TABLE CATEGORIA (
CAT_ID int primary key identity,
CAT_DESCRIPCION varchar(100),
CAT_ACTIVO bit default 1,
CAT_FECHAREGISTRO datetime default getdate())

GO

CREATE TABLE MARCA (
MAR_ID int primary key identity,
MAR_DESCRIPCION varchar(100),
MAR_ACTIVO bit default 1,
MAR_FECHAREGISTRO datetime default getdate())

GO

CREATE TABLE PRODUCTO (
PRO_ID int primary key identity,
PRO_NOMBRE varchar(500),
PRO_DESCRIPCION varchar(500),
MAR_ID int references MARCA(MAR_ID),
CAT_ID int references CATEGORIA(CAT_ID),
PRO_PRECIO decimal(10,2) default 0,
PRO_STOCK int,
PRO_RUTAIMAGEN varchar(100),
PRO_NOMBREIMAGEN varchar(100),
PRO_ACTIVO bit default 1,
PRO_FECHAREGISTRO datetime default getdate()
)

GO

CREATE TABLE CLIENTE (
CLI_ID int primary key identity,
CLI_NOMBRES varchar(100),
CLI_APELLIDOS varchar(100),
CLI_CORREO varchar(100),
CLI_CLAVE varchar(150),
CLI_REESTABLECER bit default 0,
CLI_FECHAREGISTRO datetime default getdate()
)

GO

CREATE TABLE CARRITO(
CARR_ID int primary key identity,
CLI_ID int references CLIENTE (CLI_ID),
PRO_ID int references PRODUCTO (PRO_ID),
CARR_CANTIDAD int
)

GO

create table VENTA(
VEN_ID int primary key identity,
CLI_ID int references CLIENTE(CLI_ID),
VEN_TOTALPRODUCTO int,
VEN_MONTOTOTAL decimal(10,2),
VEN_CONTACTO varchar(50),
COL_ID varchar(10),
VEN_TELEFONO varchar(50),
VEN_DIRECCION varchar(500),
VEN_IDTRANSACCION varchar(50),
VEN_FECHA datetime default getdate()
)

GO

create table DETALLE_VENTA(
DETV_ID int primary key identity,
VEN_ID int references VENTA (VEN_ID),
PRO_ID int references PRODUCTO(PRO_ID),
DETV_CANTIDAD int,
DETV_TOTAL decimal(10,2)
)

GO

CREATE TABLE USUARIO(
USU_ID int primary key identity,
USU_NOMBRES varchar(100),
USU_APELLIDOS varchar(100),
USU_CORREO varchar(100),
USU_CLAVE varchar(150),
USU_REESTABLECER bit default 1,
USU_ACTIVO bit default 1,
USU_FECHAREGISTRO datetime default getdate()
)
GO

CREATE TABLE MUNICIPIO( 
MUN_ID varchar(2) NOT NULL,
MUN_DESCRIPCION varchar(45) NOT NULL
)

GO

CREATE TABLE CIUDAD (
CIU_ID varchar(4) NOT NULL, 
CIU_DESCRIPCION varchar(45) NOT NULL,
MUN_ID varchar(2) NOT NULL
)

GO

CREATE TABLE COLONIA (
COL_ID varchar(6) NOT NULL,
COL_DESCRIPCION varchar(45) NOT NULL,
CIU_ID varchar(4) NOT NULL,
MUN_ID varchar(2) NOT NULL
)
SELECT * FROM USUARIO