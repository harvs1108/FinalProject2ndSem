Imports System.Data.OleDb
Imports System.ComponentModel

Public Class Form1
    Dim connect As New OleDbConnection
    Dim command As New OleDbCommand
    Dim sql As String = Nothing

    Dim loginAttempts As Integer = 0
    Const maxAttempts As Integer = 3
    Dim cooldownSeconds As Integer = 30
    Dim lastFailedLogin As DateTime = DateTime.MinValue

    Private WithEvents loginWorker As New BackgroundWorker()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;"

        If My.Settings.RememberMe Then
            user.Text = My.Settings.Username
            pass.Text = My.Settings.Password
            chkrememberme.Checked = True
        End If
        pass.PasswordChar = "●"c

        loginWorker.WorkerReportsProgress = False
        loginWorker.WorkerSupportsCancellation = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.ExitThread()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub loginbtn_Click(sender As Object, e As EventArgs) Handles loginbtn.Click

        If loginAttempts >= maxAttempts AndAlso (DateTime.Now - lastFailedLogin).TotalSeconds < cooldownSeconds Then
            Dim remainingTime As Integer = cooldownSeconds - CType((DateTime.Now - lastFailedLogin).TotalSeconds, Integer)
            MessageBox.Show("Too many failed login attempts. Please try again in " & remainingTime & " seconds.", "Cooldown", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If


        If String.IsNullOrWhiteSpace(user.Text) OrElse String.IsNullOrWhiteSpace(pass.Text) Then
            MessageBox.Show("Please enter both Username and Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If


        loginWorker.RunWorkerAsync()

        If loginWorker.IsBusy Then
            MessageBox.Show("Login process is still running. Please wait...", "Login in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If loginAttempts >= maxAttempts AndAlso (DateTime.Now - lastFailedLogin).TotalSeconds < cooldownSeconds Then
            Dim remainingTime As Integer = cooldownSeconds - CType((DateTime.Now - lastFailedLogin).TotalSeconds, Integer)
            MessageBox.Show("Too many failed login attempts. Please try again in " & remainingTime & " seconds.", "Cooldown", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(user.Text) OrElse String.IsNullOrWhiteSpace(pass.Text) Then
            MessageBox.Show("Please enter both Username and Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        loginbtn.Enabled = False ' 🔒 Disable button temporarily


        loginWorker.RunWorkerAsync()
    End Sub



    Private Sub loginWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles loginWorker.DoWork
        Dim sql As String = "SELECT [User] FROM admin WHERE [user] = @user AND [pass] = @pass"
        Try
            Using command As New OleDbCommand(sql, connect)
                command.Parameters.AddWithValue("@user", user.Text.Trim())
                command.Parameters.AddWithValue("@pass", pass.Text)

                If connect.State = ConnectionState.Closed Then connect.Open()
                Dim resultUser As Object = command.ExecuteScalar()
                e.Result = If(resultUser IsNot Nothing, resultUser.ToString(), "")
            End Using
        Catch ex As Exception
            e.Result = "Error: " & ex.Message
        Finally
            If connect.State = ConnectionState.Open Then connect.Close()
        End Try
    End Sub

    Private Sub loginWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles loginWorker.RunWorkerCompleted
        If e.Result.ToString().StartsWith("Error:") Then
            MessageBox.Show(e.Result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim username As String = e.Result.ToString()

        If username <> "" Then
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

            MessageBox.Show("Login successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)


            If username.ToLower() = "admin" Then
                AdminForm.Show()
            Else
                Form3.Show()
            End If

            Me.Hide()
        Else
            loginAttempts += 1
            lastFailedLogin = DateTime.Now

            If loginAttempts >= maxAttempts Then
                MessageBox.Show("Too many failed login attempts. Please try again in " & cooldownSeconds & " seconds.", "Cooldown", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                MessageBox.Show("Invalid Username or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub


    Private Sub chkshowpassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkshowpassword.CheckedChanged
        pass.PasswordChar = If(chkshowpassword.Checked, Chr(0), "●"c)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub lnkforgotpassword_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkforgotpassword.LinkClicked
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub user_TextChanged(sender As Object, e As EventArgs) Handles user.TextChanged

    End Sub
End Class