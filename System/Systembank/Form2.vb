Imports System.Data.OleDb
Imports System.Data
Imports System.Windows.Forms.VisualStyles
Imports System.Text.RegularExpressions
Public Class Form2
    Dim connect As New OleDbConnection
    Dim command As New OleDbCommand
    Dim sql As String = Nothing
    Private Sub form2_load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;"
        supass.PasswordChar = "●"c
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.ExitThread()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form1.Show()
        Me.Close()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles yy.SelectedIndexChanged

    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dd.SelectedIndexChanged

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles mm.SelectedIndexChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles suuser.TextChanged

    End Sub

    Private Sub email_TextChanged(sender As Object, e As EventArgs) Handles suemail.TextChanged

    End Sub

    Private Sub signupbtn_Click(sender As Object, e As EventArgs) Handles signupbtn.Click
        If connect.State = ConnectionState.Closed Then
            connect.Open()
        End If

        If Not chkagree.Checked Then
            MessageBox.Show("You must agree to the Terms and Conditions before signing up.", "Agreement Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If suemail.Text = "" Or suuser.Text = "" Or supass.Text = "" Or sucontact.Text = "" Or sugender.Text = "" Or sunationality.Text = "" Or mm.Text = "" Or dd.Text = "" Or yy.Text = "" Then
            MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Not Regex.IsMatch(sucontact.Text, "^[0-9]{11}$") Then
            MessageBox.Show("Contact number must be 11 digits and numeric only!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If lblpassStrength.Text = "Weak" Then
            MessageBox.Show("Please choose a stronger password!", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim checkdata As String = "SELECT COUNT(*) FROM admin WHERE user = '" & suuser.Text & "'"
        command = New OleDbCommand(checkdata, connect)
        Dim check = Convert.ToInt32(command.ExecuteScalar())

        If check > 0 Then
            MessageBox.Show("Username: " & suuser.Text & " already exists!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Dim dob As String = mm.Text & " " & dd.Text & ", " & yy.Text

            sql = "INSERT INTO admin ([email], [user], [pass], [contact], [gender], [nationality], [dob]) VALUES (?, ?, ?, ?, ?, ?, ?)"
            command = New OleDbCommand(sql, connect)

            command.Parameters.AddWithValue("@1", suemail.Text)
            command.Parameters.AddWithValue("@2", suuser.Text)
            command.Parameters.AddWithValue("@3", supass.Text)
            command.Parameters.AddWithValue("@4", sucontact.Text)
            command.Parameters.AddWithValue("@5", sugender.Text)
            command.Parameters.AddWithValue("@6", sunationality.Text)
            command.Parameters.AddWithValue("@7", dob)

            command.ExecuteNonQuery()

            MessageBox.Show("Successfully created a new account!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information)

            suemail.Clear()
            suuser.Clear()
            supass.Clear()
            sucontact.Clear()
            sugender.SelectedIndex = -1
            sunationality.SelectedIndex = -1
            yy.SelectedIndex = -1
            mm.SelectedIndex = -1
            dd.SelectedIndex = -1
            lblpassStrength.Text = ""
            chkagree.Checked = False

            Form1.Show()
            Me.Close()
        End If
    End Sub

    Private Sub supass_TextChanged(sender As Object, e As EventArgs) Handles supass.TextChanged
        Dim password As String = supass.Text
        Dim strength As String = ""

        If password.Length < 6 Then
            strength = "Weak"
            lblpassStrength.ForeColor = Color.Red
        ElseIf Regex.IsMatch(password, "[A-Z]") AndAlso Regex.IsMatch(password, "[0-9]") Then
            strength = "Medium"
            lblpassStrength.ForeColor = Color.Orange
        End If

        If password.Length >= 8 AndAlso Regex.IsMatch(password, "[A-Z]") AndAlso Regex.IsMatch(password, "[0-9]") AndAlso Regex.IsMatch(password, "[^a-zA-Z0-9]") Then
            strength = "Strong"
            lblpassStrength.ForeColor = Color.Green
        End If

        lblpassStrength.Text = strength
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkagree.CheckedChanged

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sugender.SelectedIndexChanged

    End Sub

    Private Sub sucontact_TextChanged(sender As Object, e As EventArgs) Handles sucontact.TextChanged

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            supass.PasswordChar = ControlChars.NullChar
        Else
            supass.PasswordChar = "●"c
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class