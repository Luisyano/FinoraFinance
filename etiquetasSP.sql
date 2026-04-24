Create Or Alter Procedure sp_FiltrarEtiquetasPaginadas
(
    @search Nvarchar(100) = Null,
    @page Int = 1,
    @pageSize Int = 10,
    @userId Nvarchar(450)
)
As
Begin
    Set Nocount On;
    Declare @inicio Int = (@page - 1) * @pageSize;

    Select 
        Id,
        Nombre,
        Descripcion,
        UserId,
        Count(*) Over() As TotalRegistros
    From 
        Etiquetas
    Where 
        UserId = @userId
        And (@search Is Null Or Nombre Like '%' + @search + '%')
    Order By 
        Nombre
    Offset @inicio Rows
    Fetch Next @pageSize Rows Only;
End
Go