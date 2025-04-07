Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class Form4
    Dim con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblerror.Visible = False
        lblstrenght.Text = ""

        txtnewpassword.PasswordChar = "●"
        txtconfirmpassword.PasswordChar = "●"
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
    End Sub

    Private Sub txtEmailContact_TextChanged(sender As Object, e As EventArgs) Handles txtEmailContact.TextChanged
        lblerror.Visible = False
    End Sub

    Private Sub txtnewpassword_TextChanged(sender As Object, e As EventArgs) Handles txtnewpassword.TextChanged
        Dim password As String = txtnewpassword.Text

        If password.Length < 6 Then
            lblstrenght.Text = "Weak"
            lblstrenght.ForeColor = Color.Red
        ElseIf password.Length >= 6 AndAlso Regex.IsMatch(password, "[A-Z]") AndAlso Regex.IsMatch(password, "[0-9]") Then
            lblstrenght.Text = "Medium"
            lblstrenght.ForeColor = Color.Orange
        End If

        If password.Length >= 8 AndAlso
           Regex.IsMatch(password, "[A-Z]") AndAlso
           Regex.IsMatch(password, "[a-z]") AndAlso
           Regex.IsMatch(password, "[0-9]") AndAlso
           Regex.IsMatch(password, "[^a-zA-Z0-9]") Then
            lblstrenght.Text = "Strong"
            lblstrenght.ForeColor = Color.Green
        End If

        lblerror.Visible = False
    End Sub

    Private Sub lblstrenght_Click(sender As Object, e As EventArgs) Handles lblstrenght.Click
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnreset.Click
        If txtEmailContact.Text = "" Or txtnewpassword.Text = "" Or txtconfirmpassword.Text = "" Then
            MessageBox.Show("Please complete all fields.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtnewpassword.Text <> txtconfirmpassword.Text Then
            lblerror.Text = "Passwords do not match!"
            lblerror.ForeColor = Color.Red
            lblerror.Visible = True
            Exit Sub
        End If

        Try
            Using connection As New OleDbConnection(con.ConnectionString)
                connection.Open()

                Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM admin WHERE email=? OR contact=?", connection)
                checkCmd.Parameters.AddWithValue("?", txtEmailContact.Text.Trim())
                checkCmd.Parameters.AddWithValue("?", txtEmailContact.Text.Trim())

                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If count > 0 Then
                    Dim updateCmd As New OleDbCommand("UPDATE admin SET [Pass]=? WHERE email=? OR contact=?", connection)
                    updateCmd.Parameters.AddWithValue("?", txtconfirmpassword.Text.Trim())
                    updateCmd.Parameters.AddWithValue("?", txtEmailContact.Text.Trim())
                    updateCmd.Parameters.AddWithValue("?", txtEmailContact.Text.Trim())

                    updateCmd.ExecuteNonQuery()
                    MessageBox.Show("Password successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Me.Hide()
                    Form1.Show()
                    Me.Close()
                Else
                    MessageBox.Show("Account not found. Please enter a valid email or contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Application.ExitThread()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub chkShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowPassword.CheckedChanged
        If chkShowPassword.Checked Then
            txtnewpassword.PasswordChar = ControlChars.NullChar
            txtconfirmpassword.PasswordChar = ControlChars.NullChar
        Else
            txtnewpassword.PasswordChar = "●"
            txtconfirmpassword.PasswordChar = "●"
        End If
    End Sub
End Class
