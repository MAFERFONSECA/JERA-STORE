
create proc sp_RegistrarCliente(
@CLI_NOMBRES varchar(100),
@CLI_APELLIDOS varchar(100),
@CLI_CORREO varchar(100),
@CLI_CLAVE varchar(100),
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE CLI_CORREO = @CLI_CORREO)
	begin
		insert into CLIENTE(CLI_NOMBRES,CLI_APELLIDOS,CLI_CORREO,CLI_CLAVE,CLI_REESTABLECER) values
		(@CLI_NOMBRES,@CLI_APELLIDOS,@CLI_CORREO,@CLI_CLAVE,0)

		SET @Resultado = scope_identity()
	end
	else 
	set @Mensaje = 'El correo del usuario ya existe'
end


create proc sp_ExisteCarrito(
@CLI_ID int,
@PRO_ID int,
@Resultado bit output
)
as
begin
	if exists(select * from carrito where CLI_ID = @CLI_ID and PRO_ID = @PRO_ID)
		set @Resultado = 1
	else
		set @Resultado = 0
end





create proc sp_OperacionCarrito(
@CLI_ID int,
@PRO_ID int,
@Sumar bit,
@Mensaje varchar (500) output,
@Resultado bit output
)
as
begin
	set @Resultado = 1
	set @Mensaje = ''

	declare @existecarrito bit = iif(exists(select * from carrito where CLI_ID = @CLI_ID and PRO_ID = @PRO_ID),1,0)
	declare @stockproducto int = (select PRO_STOCK from PRODUCTO where PRO_ID = @PRO_ID)

	BEGIN TRY 

	BEGIN TRANSACTION OPERACION

	if(@Sumar = 1)
	begin 

		if(@stockproducto > 0)
		begin

			if(@existecarrito = 1)
				update CARRITO set CARR_CANTIDAD = CARR_CANTIDAD + 1 where CLI_ID = @CLI_ID and PRO_ID = @PRO_ID
			else
				insert into CARRITO(CLI_ID,PRO_ID,CARR_CANTIDAD) values (@CLI_ID,@PRO_ID,1)

		update PRODUCTO set PRO_STOCK = PRO_STOCK - 1 where PRO_ID = @PRO_ID 
	end
	else
	begin
		set @Resultado = 0
		set @Mensaje = 'El producto no cuenta con stock disponible'
	end
	
end
else
begin
	update CARRITO set CARR_CANTIDAD = CARR_CANTIDAD - 1 where CLI_ID = @CLI_ID and PRO_ID = @PRO_ID 
	update PRODUCTO set PRO_STOCK = PRO_STOCK + 1 where PRO_ID = @PRO_ID

end

COMMIT TRANSACTION OPERACION 

END TRY
BEGIN CATCH 
	set @Resultado = 0
	set @Mensaje = ERROR_MESSAGE()
	ROLLBACK TRANSACTION OPERACION 
END CATCH 

end 





			

create function fn_obtenerCarritoCliente(
@CLI_ID int
)
returns table
as
return(
	select  p.PRO_ID, m.MAR_DESCRIPCION[DesMarca],p.PRO_NOMBRE,p.PRO_PRECIO,c.CARR_CANTIDAD,p.PRO_RUTAIMAGEN,p.PRO_NOMBREIMAGEN

	from CARRITO c
	inner join PRODUCTO p on p.PRO_ID = c.PRO_ID
	inner join MARCA m on m.MAR_ID = p.MAR_ID
	where c.CLI_ID = @CLI_ID
)


select * from fn_obtenerCarritoCliente(3)


create proc sp_EliminarCarrito(
@CLI_ID int,
@PRO_ID int, 
@Resultado bit output 
)
as
begin
	
	set @Resultado = 1
	declare @cantidadproducto int = (select CARR_CANTIDAD from CARRITO where CLI_ID = @CLI_ID and PRO_ID = @PRO_ID)

	BEGIN TRY

		BEGIN TRANSACTION OPERACION

		update PRODUCTO set PRO_STOCK = PRO_STOCK + @cantidadproducto where PRO_ID = @PRO_ID
		delete top (1) from CARRITO where CLI_ID = @CLI_ID and PRO_ID = @PRO_ID

		COMMIT TRANSACTION OPERACION 

	END TRY
	BEGIN CATCH
		set @Resultado = 0
		ROLLBACK TRANSACTION OPERACION
	END CATCH
end
















declare @CAT_ID int = 0

select distinct m.MAR_ID,m.MAR_DESCRIPCION from PRODUCTO p
inner join CATEGORIA c on c.CAT_ID= p.CAT_ID
inner join MARCA m on m.MAR_ID= p.MAR_ID and m.MAR_ACTIVO=1
where c.CAT_ID= iif(@CAT_ID = 0, c.CAT_ID, @CAT_ID)


select count (*) from carrito where CLI_ID = 1







CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[PRO_ID] int NULL,
	[DETV_CANTIDAD] int NULL, 
	[DETV_TOTAL] decimal (10,2) NULL 
)


create procedure usp_RegistrarVenta(
@CLI_ID int,
@VEN_TOTALPRODUCTO int,
@VEN_MONTOTOTAL decimal (10,2),
@VEN_CONTACTO varchar(100),
@COL_ID varchar(6),
@VEN_TELEFONO varchar (10),
@VEN_DIRECCION varchar(100),
@VEN_IDTRANSACCION varchar(50),
@DETALLEVENTA[EDetalle_Venta] READONLY,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin

	begin try

		declare @VEN_ID int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro 

		insert into VENTA(CLI_ID, VEN_TOTALPRODUCTO, VEN_MONTOTOTAL, VEN_CONTACTO,COL_ID, VEN_TELEFONO, VEN_DIRECCION, VEN_IDTRANSACCION)
		values(@CLI_ID,@VEN_TOTALPRODUCTO,@VEN_MONTOTOTAL,@VEN_CONTACTO,@COL_ID,@VEN_TELEFONO,@VEN_DIRECCION,@VEN_IDTRANSACCION)

		set @VEN_ID = SCOPE_IDENTITY()

		insert into DETALLE_VENTA (VEN_ID, PRO_ID, DETV_CANTIDAD, DETV_TOTAL)
		select @VEN_ID, PRO_ID, DETV_CANTIDAD, DETV_TOTAL from @DETALLEVENTA


		DELETE FROM CARRITO WHERE CLI_ID = @CLI_ID

		commit transaction registro

		end try
		begin catch 
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro 
		end catch 

end



CREATE FUNCTION fn_ListarCompra(
@idcliente int
)
RETURNS TABLE
AS
RETURN
(
select p.PRO_RUTAIMAGEN,p.PRO_NOMBREIMAGEN,p.PRO_NOMBRE,p.PRO_PRECIO,dv.DETV_CANTIDAD,dv.DETV_TOTAL,v.VEN_IDTRANSACCION from DETALLE_VENTA dv
inner join PRODUCTO p on p.PRO_ID= dv.PRO_ID
inner join VENTA v on v.VEN_ID = dv.VEN_ID
where v.CLI_ID = @idcliente

)
 GO



