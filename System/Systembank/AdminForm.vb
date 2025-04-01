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
            Dim dt As New DataTable
            Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;")
                con.Open()
                Dim sql As String = "SELECT * FROM admin"
                Using cmd As New OleDbCommand(sql, con)
                    Using da As New OleDbDataAdapter(cmd)
                        da.Fill(dt)
                        dgvusers.DataSource = dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
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
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles txtnationality.TextChanged

    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnblock.Click
        If dgvusers.SelectedRows.Count > 0 Then
            Dim selectedUser As String = dgvusers.SelectedRows(0).Cells("User").Value.ToString()

            Dim confirm = MessageBox.Show("Are you sure you want to block this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = DialogResult.Yes Then
                Try
                    Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;")
                        con.Open()
                        Dim sql As String = "UPDATE admin SET Status='Blocked' WHERE [User]=@user"
                        Using cmd As New OleDbCommand(sql, con)
                            cmd.Parameters.AddWithValue("@User", selectedUser)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("User has been blocked.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadUsers()
                Catch ex As Exception
                    MessageBox.Show("Error blocking user: " & ex.Message)
                End Try
            End If
        Else
            MessageBox.Show("Please select a user first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        If txtusername.Text.Trim() = "" Then
            MessageBox.Show("Please select a user to update.")
            Exit Sub
        End If

        Dim dob As String = cbmonth.Text & "-" & cbday.Text & "-" & cbyear.Text

        Try
            Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")
                con.Open()

                Dim query As String = "UPDATE admin SET Email=?, Pass=?, Contact=?, DOB=?, Gender=?, Nationality=?, Balance=?, Status=? WHERE [User]=?"
                Using cmd As New OleDbCommand(query, con)
                    cmd.Parameters.AddWithValue("?", txtemail.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtpassword.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtcontact.Text.Trim())
                    cmd.Parameters.AddWithValue("?", dob)
                    cmd.Parameters.AddWithValue("?", txtgender.Text.Trim()) ' This was missing in the correct place
                    cmd.Parameters.AddWithValue("?", txtnationality.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtbalance.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtstatus.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtusername.Text.Trim()) ' Should be last


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

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles txtemail.TextChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnunblock.Click
        If dgvusers.SelectedRows.Count > 0 Then
            Dim selectedUser As String = dgvusers.SelectedRows(0).Cells("User").Value.ToString()

            Dim confirm = MessageBox.Show("Are you sure you want to unblock this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = DialogResult.Yes Then
                Try
                    Using con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb;")
                        con.Open()
                        Dim sql As String = "UPDATE admin SET Status='Active' WHERE [User]=@user"
                        Using cmd As New OleDbCommand(sql, con)
                            cmd.Parameters.AddWithValue("@user", selectedUser)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("User has been unblocked.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadUsers()
                Catch ex As Exception
                    MessageBox.Show("Error unblocking user: " & ex.Message)
                End Try
            End If
        Else
            MessageBox.Show("Please select a user first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
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
            txtgender.Text = row.Cells("Gender").Value.ToString()
            txtstatus.Text = row.Cells("Status").Value.ToString()
            Dim dobParts = row.Cells("DOB").Value.ToString().Split("-"c)
            If dobParts.Length = 3 Then
                cbmonth.Text = dobParts(0)
                cbday.Text = dobParts(1)
                cbyear.Text = dobParts(2)
            End If
        End If

    End Sub

    Private Sub txtpassword_TextChanged(sender As Object, e As EventArgs) Handles txtpassword.TextChanged

    End Sub

    Private Sub txtdob_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbday.SelectedIndexChanged

    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbyear.SelectedIndexChanged

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

    Private Sub cmbstatus_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtusername_TextChanged(sender As Object, e As EventArgs) Handles txtusername.TextChanged

    End Sub

    Private Sub txtstatus_TextChanged(sender As Object, e As EventArgs) Handles txtstatus.TextChanged

    End Sub

    Private Sub cbmonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbmonth.SelectedIndexChanged

    End Sub
End Class