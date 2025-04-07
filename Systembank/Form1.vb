Imports System.Data.OleDb
Imports System.ComponentModel

Public Class Form1
    Dim connect As New OleDbConnection
    Dim sql As String = Nothing

    Dim loginAttempts As Integer = 0
    Const maxAttempts As Integer = 3
    Dim cooldownSeconds As Integer = 30
    Dim lastFailedLogin As DateTime = DateTime.MinValue

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;"

        If My.Settings.RememberMe Then
            user.Text = My.Settings.Username
            pass.Text = My.Settings.Password
            chkrememberme.Checked = True
        End If
        pass.PasswordChar = "●"c
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.ExitThread()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub loginbtn_Click(sender As Object, e As EventArgs) Handles loginbtn.Click
        Dim username As String = user.Text.Trim()
        Dim password As String = pass.Text.Trim()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If loginAttempts >= maxAttempts AndAlso (DateTime.Now - lastFailedLogin).TotalSeconds < cooldownSeconds Then
            Dim remainingTime As Integer = cooldownSeconds - CType((DateTime.Now - lastFailedLogin).TotalSeconds, Integer)
            MessageBox.Show("Too many failed login attempts. Please try again in " & remainingTime & " seconds.", "Cooldown", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using con As New OleDbConnection(connect.ConnectionString)
                con.Open()
                Dim query As String = "SELECT [User], [Pass], [Status] FROM admin WHERE [User]=? AND Pass=?"
                Using cmd As New OleDbCommand(query, con)
                    cmd.Parameters.AddWithValue("?", username)
                    cmd.Parameters.AddWithValue("?", password)

                    Dim reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        reader.Read()
                        If reader("Status").ToString() = "Blocked" Then
                            MessageBox.Show("Your account is blocked. Please contact the admin.", "Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Else
                            MessageBox.Show("Login successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            loginAttempts = 0

                            If chkrememberme.Checked Then
                                My.Settings.Username = user.Text
                                My.Settings.Password = pass.Text
                                My.Settings.RememberMe = True
                                My.Settings.Save()
                            Else
                                My.Settings.Username = ""
                                My.Settings.Password = ""
                                My.Settings.RememberMe = False
                                My.Settings.Save()
                            End If

                            If username.ToLower() = "admin" Then
                                AdminForm.Show()
                            Else
                                Form3.Show()
                            End If

                            Me.Hide()
                        End If
                    Else
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error occurred during login: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        loginAttempts += 1
        lastFailedLogin = DateTime.Now

        If loginAttempts >= maxAttempts Then
            MessageBox.Show("Too many failed login attempts. Please try again in " & cooldownSeconds & " seconds.", "Cooldown", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub chkshowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkshowpassword.CheckedChanged
        pass.PasswordChar = If(chkshowpassword.Checked, Chr(0), "*"c)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
    End Sub

    Private Sub lnkforgotpassword_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkforgotpassword.LinkClicked
        Form4.Show()
        Me.Hide()
    End Sub
End Class
