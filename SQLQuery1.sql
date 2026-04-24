
Create Or Alter Procedure sp_FiltrarTransaccionesPaginadas
(
    @search Nvarchar(100) = Null,
    @orden Nvarchar(20) = 'fecha_desc',
    @page Int = 1,
    @pageSize Int = 10,
    @userId Nvarchar(450)
)
As
Begin
    Set Nocount On;
    Declare @inicio Int = (@page - 1) * @pageSize;

    Select 
        t.Id,
        t.Monto,
        t.Fecha,
        t.Tipo,
        t.Nota,
        t.CuentaId,
        t.EtiquetaId,
        t.UserId,
        c.Nombre As CuentaNombre,
        c.Moneda,
        e.Nombre As EtiquetaNombre,
        Count(*) Over() As TotalRegistros
    From 
        Transacciones t
        Left Join Cuentas c On t.CuentaId = c.Id
        Left Join Etiquetas e On t.EtiquetaId = e.Id
    Where 
        t.UserId = @userId
        And (@search Is Null Or 
             t.Nota Like '%' + @search + '%' Or 
             e.Nombre Like '%' + @search + '%')
    Order By 
        Case When @orden = 'fecha_desc' Then t.Fecha End Desc,
        Case When @orden = 'fecha_asc' Then t.Fecha End Asc,
        Case When @orden = 'monto_desc' Then t.Monto End Desc,
        Case When @orden = 'monto_asc' Then t.Monto End Asc,
        t.Fecha Desc
    Offset @inicio Rows
    Fetch Next @pageSize Rows Only;
End
Go