Imports System.Data.OleDb

Public Class AdminForm

    Private Sub LoadUsers()
        Dim con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")
        Dim da As New OleDbDataAdapter("SELECT * FROM admin", con)
        Dim dt As New DataTable
        da.Fill(dt)
        dgvusers.DataSource = dt
    End Sub

    Private Sub LoadUserData()
        Try
            Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;")
            Dim query As String = "SELECT * FROM admin"
            Dim adapter As New OleDbDataAdapter(query, conn)
            Dim dt As New DataTable()

            adapter.Fill(dt)
            dgvusers.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AdminForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadUsers()
    End Sub

    Private Sub ClearFields()
        txtusername.Clear()
        txtemail.Clear()
        txtpassword.Clear()
        txtcontact.Clear()
        txtnationality.Clear()
        txtbalance.Clear()
        cbmonth.SelectedIndex = -1
        cbday.SelectedIndex = -1
        cbyear.SelectedIndex = -1
        txtstatus.Clear()
        txtgender.Clear()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnblock.Click
        If txtusername.Text.Trim() = "" Then
            MessageBox.Show("Please select a user to block.")
            Exit Sub
        End If

        Try
            Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")
                con.Open()
                Dim cmd As New OleDbCommand("UPDATE admin SET Status=? WHERE [User]=?", con)
                cmd.Parameters.AddWithValue("?", "Blocked")
                cmd.Parameters.AddWithValue("?", txtusername.Text.Trim())

                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MessageBox.Show("User blocked successfully.")
                    LoadUsers()
                    ClearFields()
                Else
                    MessageBox.Show("Block failed. Check if the user exists.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Block error: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        If txtusername.Text.Trim() = "" Then
            MessageBox.Show("Please select a user to update.")
            Exit Sub
        End If

        Dim dob As String = cbmonth.Text & "-" & cbday.Text & "-" & cbyear.Text

        If txtemail.Text.Trim() = "" OrElse txtpassword.Text.Trim() = "" OrElse txtcontact.Text.Trim() = "" OrElse
       txtnationality.Text.Trim() = "" OrElse txtbalance.Text.Trim() = "" OrElse txtstatus.Text.Trim() = "" OrElse
       txtgender.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all the fields.")
            Exit Sub
        End If

        Try
            Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")
                con.Open()

                Dim query As String = "UPDATE admin SET Email=?, Pass=?, Contact=?, DOB=?, Gender=?, Nationality=?, Balance=?, Status=? WHERE [User]=?"

                Using cmd As New OleDbCommand(query, con)
                    cmd.Parameters.Clear()

                    cmd.Parameters.AddWithValue("?", txtemail.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtpassword.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtcontact.Text.Trim())
                    cmd.Parameters.AddWithValue("?", dob)
                    cmd.Parameters.AddWithValue("?", txtgender.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtnationality.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtbalance.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtstatus.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtusername.Text.Trim())

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("User info updated successfully.")
                        LoadUsers()
                        ClearFields()
                    Else
                        MessageBox.Show("No user was updated. Please check if the username is correct.")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Update failed: " & ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnunblock.Click
        If txtusername.Text.Trim() = "" Then
            MessageBox.Show("Please select a user to unblock.")
            Exit Sub
        End If

        Try
            Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")
                con.Open()

                Dim checkStatusQuery As String = "SELECT [Status] FROM admin WHERE [User]=?"
                Using checkCmd As New OleDbCommand(checkStatusQuery, con)
                    checkCmd.Parameters.AddWithValue("?", txtusername.Text.Trim())
                    Dim status As Object = checkCmd.ExecuteScalar()

                    If status Is Nothing Then
                        MessageBox.Show("User does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    If status.ToString() = "Active" Then
                        MessageBox.Show("This user is already active.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    If status.ToString() = "Blocked" Then
                        Dim unblockQuery As String = "UPDATE admin SET Status=? WHERE [User]=?"
                        Using unblockCmd As New OleDbCommand(unblockQuery, con)
                            unblockCmd.Parameters.AddWithValue("?", "Active")
                            unblockCmd.Parameters.AddWithValue("?", txtusername.Text.Trim())

                            Dim rowsAffected As Integer = unblockCmd.ExecuteNonQuery()

                            If rowsAffected > 0 Then
                                MessageBox.Show("User unblocked successfully.")
                                LoadUsers()
                                ClearFields()
                            Else
                                MessageBox.Show("Unblock failed. Please check if the username is correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    Else
                        MessageBox.Show("This user is not blocked.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Unblock error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvusers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvusers.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvusers.Rows(e.RowIndex)
            txtusername.Text = row.Cells("User").Value.ToString()
            txtemail.Text = row.Cells("Email").Value.ToString()
            txtpassword.Text = row.Cells("Pass").Value.ToString()
            txtcontact.Text = row.Cells("Contact").Value.ToString()
            txtnationality.Text = row.Cells("Nationality").Value.ToString()
            txtbalance.Text = row.Cells("Balance").Value.ToString()

            Dim dobParts = row.Cells("DOB").Value.ToString().Split("-"c)
            If dobParts.Length = 3 Then
                cbmonth.Text = dobParts(0)
                cbday.Text = dobParts(1)
                cbyear.Text = dobParts(2)
            End If

            txtstatus.Text = row.Cells("Status").Value.ToString()
            txtgender.Text = row.Cells("Gender").Value.ToString()
        End If
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        If txtusername.Text.Trim() = "" Then
            MessageBox.Show("Please select a user to delete.")
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to delete this user?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")
            Dim cmd As New OleDbCommand("DELETE FROM admin WHERE [User]=?", con)
            cmd.Parameters.AddWithValue("?", txtusername.Text)
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()
            MessageBox.Show("User deleted successfully.")

            txtusername.Clear()
            txtemail.Clear()
            txtpassword.Clear()
            txtcontact.Clear()
            txtnationality.Clear()
            txtbalance.Clear()
            cbmonth.SelectedIndex = -1
            cbday.SelectedIndex = -1
            cbyear.SelectedIndex = -1
            txtstatus.Clear()
            txtgender.Clear()

            LoadUsers()
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub btnrefresh_Click(sender As Object, e As EventArgs) Handles btnrefresh.Click
        LoadUserData()
    End Sub
End Class
