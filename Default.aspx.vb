Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Dim constr As String = "Data Source=SHAHEER\SQLEXPRESS;Initial Catalog=constr1;Integrated Security=True"
        Dim query As String = "SELECT * FROM Table1"
        Using con As SqlConnection = New SqlConnection(constr)
            Using sda As SqlDataAdapter = New SqlDataAdapter(query, con)
                Using dt As DataTable = New DataTable()
                    sda.Fill(dt)
                    GridView1.DataSource = dt
                    GridView1.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Protected Sub Insert(ByVal sender As Object, ByVal e As EventArgs)
        Dim name As String = txtName.Text
        Dim country As String = txtCountry.Text
        Dim query As String = "INSERT INTO Table1 VALUES(@Name, @Country)"
        Dim constr As String = "Data Source=SHAHEER\SQLEXPRESS;Initial Catalog=constr1;Integrated Security=True"
        txtName.Text = ""
        txtCountry.Text = ""
        Using con As SqlConnection = New SqlConnection(constr)
            Using cmd As SqlCommand = New SqlCommand(query)
                cmd.Parameters.AddWithValue("@Name", name)
                cmd.Parameters.AddWithValue("@Country", country)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using

        Me.BindGrid()
    End Sub

    Protected Sub OnRowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        GridView1.EditIndex = e.NewEditIndex
        Me.BindGrid()
    End Sub

    Protected Sub OnRowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
        Dim customerId As Integer = Convert.ToInt32(GridView1.DataKeys(e.RowIndex).Values(0))
        Dim name As String = (TryCast(row.FindControl("txtName"), TextBox)).Text
        Dim country As String = (TryCast(row.FindControl("txtCountry"), TextBox)).Text
        Dim query As String = "UPDATE Table1 SET Name=@Name, Country=@Country WHERE CustomerId=@CustomerId"
        Dim constr As String = "Data Source=SHAHEER\SQLEXPRESS;Initial Catalog=constr1;Integrated Security=True"
        Using con As SqlConnection = New SqlConnection(constr)
            Using cmd As SqlCommand = New SqlCommand(query)
                cmd.Parameters.AddWithValue("@CustomerId", customerId)
                cmd.Parameters.AddWithValue("@Name", name)
                cmd.Parameters.AddWithValue("@Country", country)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using

        GridView1.EditIndex = -1
        Me.BindGrid()
    End Sub

    Protected Sub OnRowCancelingEdit(ByVal sender As Object, ByVal e As EventArgs)
        GridView1.EditIndex = -1
        Me.BindGrid()
    End Sub

    Protected Sub OnRowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim customerId As Integer = Convert.ToInt32(GridView1.DataKeys(e.RowIndex).Values(0))
        Dim query As String = "DELETE FROM Table1 WHERE CustomerId=@CustomerId"
        Dim constr As String = "Data Source=SHAHEER\SQLEXPRESS;Initial Catalog=constr1;Integrated Security=True"
        Using con As SqlConnection = New SqlConnection(constr)
            Using cmd As SqlCommand = New SqlCommand(query)
                cmd.Parameters.AddWithValue("@CustomerId", customerId)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using

        Me.BindGrid()
    End Sub

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow AndAlso e.Row.RowIndex <> GridView1.EditIndex Then
            TryCast(e.Row.Cells(2).Controls(2), LinkButton).Attributes("onclick") = "return confirm('Do you want to delete this row?');"
        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

End Class