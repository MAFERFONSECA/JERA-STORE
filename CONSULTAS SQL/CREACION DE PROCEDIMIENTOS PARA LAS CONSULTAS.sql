

select * from USUARIO

create proc sp_RegistrarUsuario(
@USU_NOMBRES varchar(100),
@USU_APELLIDOS varchar(100),
@USU_CORREO varchar(100),
@USU_CLAVE varchar(100),
@USU_ACTIVO bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE USU_CORREO = @USU_CORREO)
	begin
		insert into USUARIO(USU_NOMBRES,USU_APELLIDOS,USU_CORREO,USU_CLAVE,USU_ACTIVO) values
		(@USU_NOMBRES,@USU_APELLIDOS,@USU_CORREO,@USU_CLAVE,@USU_ACTIVO)

		SET @Resultado = scope_identity()
	end
	else 
	set @Mensaje = 'El correo del usuario ya existe'
end



go

create proc sp_EditarUsuario(
@USU_ID int,
@USU_NOMBRES varchar(100),
@USU_APELLIDOS varchar(100),
@USU_CORREO varchar(100),
@USU_ACTIVO bit,
@Mensaje varchar(500) output,
@Resultado bit output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE USU_CORREO = @USU_CORREO and USU_ID != @USU_ID)
	begin

		update top (1) USUARIO set 
		USU_NOMBRES = @USU_NOMBRES,
		USU_APELLIDOS = @USU_APELLIDOS,
		USU_CORREO= @USU_CORREO,
		USU_ACTIVO = @USU_ACTIVO
		where USU_ID = @USU_ID

		SET @Resultado = 1
	end
	else
	set @Mensaje = 'El correo del usuario ya existe'
end


select * from CATEGORIA

create proc sp_RegistrarCategoria(
@CAT_DESCRIPCION varchar(100),
@CAT_ACTIVO bit,
@Mensaje varchar(500) output, 
@Resultado int output
)
as
begin
SET @Resultado = 0

IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE CAT_DESCRIPCION = @CAT_DESCRIPCION)
begin
insert into CATEGORIA (CAT_DESCRIPCION,CAT_ACTIVO) values
(@CAT_DESCRIPCION,@CAT_ACTIVO)

SET @Resultado = scope_identity()
end
else
set @Mensaje ='La categoría ya existe'
end


create proc sp_EditarCategoria(
@CAT_ID int,
@CAT_DESCRIPCION varchar (100),
@CAT_ACTIVO bit,
@Mensaje varchar (500) output,
@Resultado bit output
)
as
begin
SET @Resultado = 0
IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE CAT_DESCRIPCION = @CAT_DESCRIPCION and CAT_ID != @CAT_ID)
begin

update top (1) CATEGORIA set
CAT_DESCRIPCION = @CAT_DESCRIPCION,
CAT_ACTIVO = @CAT_ACTIVO
where CAT_ID = @CAT_ID

SET @Resultado = 1
end
else
set @Mensaje = 'La categoría ya existe'
end


create proc sp_EliminarCategoria(
@CAT_ID int,
@Mensaje varchar(500) output,
@Resultado bit output
)
as
begin
SET @Resultado = 0
IF NOT EXISTS (select * from PRODUCTO p
inner join CATEGORIA c on c.CAT_ID = p.CAT_ID
where p.CAT_ID = @CAT_ID)
begin
delete top (1) from CATEGORIA where CAT_ID = @CAT_ID
SET @Resultado = 1
end
else
set @Mensaje = 'La categoría se encuentra relacionada a un producto'
end 


select * from Marca

create proc sp_RegistrarMarca(
@MAR_DESCRIPCION varchar(100),
@MAR_ACTIVO bit,
@Mensaje varchar(500) output, 
@Resultado int output
)
as
begin
SET @Resultado = 0

IF NOT EXISTS (SELECT * FROM MARCA WHERE MAR_DESCRIPCION = @MAR_DESCRIPCION)
begin
insert into MARCA (MAR_DESCRIPCION,MAR_ACTIVO) values
(@MAR_DESCRIPCION,@MAR_ACTIVO)

SET @Resultado = scope_identity()
end
else
set @Mensaje ='La marca ya existe'
end



create proc sp_EditarMarca(
@MAR_ID int,
@MAR_DESCRIPCION varchar (100),
@MAR_ACTIVO bit,
@Mensaje varchar (500) output,
@Resultado bit output
)
as
begin
SET @Resultado = 0
IF NOT EXISTS (SELECT * FROM MARCA WHERE MAR_DESCRIPCION = @MAR_DESCRIPCION and MAR_ID != @MAR_ID)
begin

update top (1) MARCA set
MAR_DESCRIPCION = @MAR_DESCRIPCION,
MAR_ACTIVO = @MAR_ACTIVO
where MAR_ID = @MAR_ID

SET @Resultado = 1
end
else
set @Mensaje = 'La marca ya existe'
end



create proc sp_EliminarMarca(
@MAR_ID int,
@Mensaje varchar(500) output,
@Resultado bit output
)
as
begin
SET @Resultado = 0
IF NOT EXISTS (select * from PRODUCTO p
inner join MARCA m on m.MAR_ID = p.MAR_ID
where p.MAR_ID = @MAR_ID)
begin
delete top (1) from MARCA where MAR_ID = @MAR_ID
SET @Resultado = 1
end
else
set @Mensaje = 'La marca se encuentra relacionada a un producto'
end 



select * from PRODUCTO

create proc sp_RegistrarProducto(
@PRO_NOMBRE varchar(100),
@PRO_DESCRIPCION varchar(100),
@MAR_ID varchar(100),
@CAT_ID varchar (100),
@PRO_PRECIO decimal(10,2),
@PRO_STOCK int,
@PRO_ACTIVO bit, 
@Mensaje varchar (500) output,
@Resultado int output 
)
as
begin
SET @Resultado = 0
IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE PRO_NOMBRE = @PRO_NOMBRE)
begin
insert into PRODUCTO(PRO_NOMBRE,PRO_DESCRIPCION,MAR_ID,CAT_ID,PRO_PRECIO,PRO_STOCK,PRO_ACTIVO) values
(@PRO_NOMBRE,@PRO_DESCRIPCION,@MAR_ID,@CAT_ID,@PRO_PRECIO,@PRO_STOCK,@PRO_ACTIVO)

SET @Resultado = scope_identity()
end
else
set @Mensaje ='El producto ya existe'
end



create proc sp_EditarProducto(
@PRO_ID int, 
@PRO_NOMBRE varchar(100),
@PRO_DESCRIPCION varchar(100),
@MAR_ID varchar(100),
@CAT_ID varchar (100),
@PRO_PRECIO decimal(10,2),
@PRO_STOCK int,
@PRO_ACTIVO bit, 
@Mensaje varchar (500) output,
@Resultado int output 
)
as 
begin
SET @Resultado = 0
IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE PRO_NOMBRE = @PRO_NOMBRE and PRO_ID != @PRO_ID)
begin
update PRODUCTO set
PRO_NOMBRE = @PRO_NOMBRE, 
PRO_DESCRIPCION = @PRO_DESCRIPCION,
MAR_ID = @MAR_ID,
CAT_ID = @CAT_ID,
PRO_PRECIO = @PRO_PRECIO,
PRO_STOCK = @PRO_STOCK,
PRO_ACTIVO = @PRO_ACTIVO
where PRO_ID = @PRO_ID

SET @Resultado = 1
end
else
set @Mensaje = 'El producto ya existe'
end 



create proc sp_EliminarProducto(
@PRO_ID int,
@Mensaje varchar(500) output,
@Resultado bit output
)
as
begin
SET @Resultado = 0
IF NOT EXISTS (select * from DETALLE_VENTA dv
inner join PRODUCTO p on p.PRO_ID = dv.PRO_ID
where p.PRO_ID = @PRO_ID)
begin
delete top (1) from PRODUCTO where PRO_ID = @PRO_ID
SET @Resultado = 1
end
else
set @Mensaje = 'El producto se encuentra relacionado a una venta'
end 


create proc sp_ReporteDashboard
as
begin
select 
(select count (*) from cliente) [TotalCliente],
(select isnull(sum(DETV_CANTIDAD),0) from DETALLE_VENTA) [TotalVenta],
(select count (*) from PRODUCTO) [TotalProducto]
end 


select p.PRO_ID, p.PRO_NOMBRE, p.PRO_DESCRIPCION, 
m.MAR_ID, m.MAR_DESCRIPCION[DesMarca],
c.CAT_ID, c.CAT_DESCRIPCION[DesCategoria],
p.PRO_PRECIO, p.PRO_STOCK, p.PRO_RUTAIMAGEN, p.PRO_NOMBREIMAGEN, p.PRO_ACTIVO
from PRODUCTO p
inner join MARCA m on m.MAR_ID = p.MAR_ID
inner join CATEGORIA c on c.CAT_ID = p.CAT_ID

--consulta historial de ventas

create proc sp_ReporteVentas(
@fechainicio varchar(10),
@fechafin varchar(10),
@idtransaccion varchar(50)
)
as
begin 

set dateformat dmy;

select CONVERT(char(10),v.VEN_FECHA, 103)[FechaVenta], CONCAT(c.CLI_NOMBRES,' ', c.CLI_APELLIDOS)[Cliente], 
p.PRO_NOMBRE[Producto], P.PRO_PRECIO[Precio], dv.DETV_CANTIDAD[Cantidad], dv.DETV_TOTAL[Total], v.VEN_IDTRANSACCION[IdTransaccion]
from DETALLE_VENTA dv 
inner join PRODUCTO p on p.PRO_ID = dv.PRO_ID
inner join VENTA v on v.VEN_ID = dv.VEN_ID 
inner join CLIENTE c on c.CLI_ID = v.CLI_ID 
where CONVERT(date,v.VEN_FECHA) between @fechainicio and @fechafin
and v.VEN_IDTRANSACCION = iif(@idtransaccion = '', v.VEN_IDTRANSACCION,@idtransaccion)

end 














