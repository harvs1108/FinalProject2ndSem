Imports System.Data.OleDb
Imports System.Data
Imports System.Windows.Forms
Public Class Form3
    Dim connect As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\steph\Documents\Systembank.accdb")

    Private WithEvents animationTimer As New Timer()
    Private targetWidth As Integer
    Private currentPanel As Panel

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        displayuser()
        loadBalance()
        loadUserProfile()
        animationTimer.Interval = 10
        dpuser.Parent = PictureBox2
    End Sub

    Private Sub AnimatePanelWidth(panel As Panel, target As Integer)
        currentPanel = panel
        targetWidth = target
        animationTimer.Start()
    End Sub

    Private Sub animationTimer_Tick(sender As Object, e As EventArgs) Handles animationTimer.Tick
        If currentPanel.Width < targetWidth Then
            currentPanel.Width += 10
        ElseIf currentPanel.Width > targetWidth Then
            currentPanel.Width -= 10
        Else
            animationTimer.Stop()
        End If
    End Sub

    Sub displayuser()
        Dim user As String = Form1.user.Text
        user = user.Substring(0, 1).ToUpper & user.Substring(1)
        dpuser.Text = user
    End Sub

    Sub loadBalance()
        Try
            connect.Open()

            Dim sql As String = "SELECT balance FROM admin WHERE [user] = ?"
            Dim cmd As New OleDbCommand(sql, connect)
            cmd.Parameters.AddWithValue("?", Form1.user.Text)

            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                Dim bal As Decimal = Convert.ToDecimal(reader("balance"))
                balance.Text = " " & bal.ToString("N2")
            Else
                balance.Text = "0.00"
                MessageBox.Show("User not found in admin table.", "Notice")
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Failed to load balance: " & ex.Message, "Error")
        Finally
            connect.Close()
        End Try
    End Sub



    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtamount.TextChanged

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btndeposit_Click(sender As Object, e As EventArgs) Handles btndeposit.Click
        If IsNumeric(txtamount.Text) AndAlso Val(txtamount.Text) > 0 Then
            Dim amount As Decimal = Val(txtamount.Text)
            UpdateBalance(amount, "deposit")
        Else
            MessageBox.Show("Please enter a valid amount to deposit.", "Warning")
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        AnimatePanelWidth(Panel3, 535)
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        AnimatePanelWidth(Panel3, 13)
    End Sub

    Private Sub Label3_Click_1(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        txtamount.Text = txtamount.Text & 100
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        txtamount.Text = txtamount.Text & 5000
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        txtamount.Text = txtamount.Text & 500
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        txtamount.Text = txtamount.Text & 1000
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        txtamount.Text = txtamount.Text & 10000
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        txtamount.Text = txtamount.Text & 20000
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        txtamount.Text = txtamount.Text & 30000
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        txtamount.Text = txtamount.Text & 50000
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        clear()
    End Sub
    Private Sub clear()
        txtamount.Text = ""
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnwithdraw.Click
        If IsNumeric(txtamount.Text) AndAlso Val(txtamount.Text) > 0 Then
            Dim amount As Decimal = Val(txtamount.Text)
            Dim currentBalance As Decimal = GetCurrentBalance()

            If amount > currentBalance Then
                MessageBox.Show("Insufficient balance!", "Error")
            Else
                UpdateBalance(-amount, "withdraw")
            End If
        Else
            MessageBox.Show("Please enter a valid amount to withdraw.", "Warning")
        End If
    End Sub

    Private Sub UpdateBalance(amount As Decimal, action As String)
        Try
            connect.Open()
            Dim sql As String = "UPDATE admin SET balance = balance + ? WHERE [user] = ?"
            Dim cmd As New OleDbCommand(sql, connect)
            cmd.Parameters.AddWithValue("?", amount)
            cmd.Parameters.AddWithValue("?", Form1.user.Text)
            cmd.ExecuteNonQuery()
            connect.Close()

            loadBalance()
            MessageBox.Show("Successfully " & action & "ed ₱" & amount.ToString("N2"), "Success")
            txtamount.Clear()
        Catch ex As Exception
            MessageBox.Show("Error during " & action & ": " & ex.Message, "Error")
        Finally
            If connect.State = ConnectionState.Open Then connect.Close()
        End Try
    End Sub

    Private Function GetCurrentBalance() As Decimal
        Dim bal As Decimal = 0
        Try
            connect.Open()
            Dim sql As String = "SELECT balance FROM admin WHERE [user] = ?"
            Dim cmd As New OleDbCommand(sql, connect)
            cmd.Parameters.AddWithValue("?", Form1.user.Text)
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                bal = Convert.ToDecimal(reader("balance"))
            End If
            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading current balance: " & ex.Message)
        Finally
            connect.Close()
        End Try
        Return bal
    End Function

    Private Sub PictureBox5_Click_1(sender As Object, e As EventArgs) Handles PictureBox5.Click

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles pb1.Click

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles txtloadamount.TextChanged

    End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click

    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click

    End Sub

    Private Sub Label15_Click(sender As Object, e As EventArgs) Handles Label15.Click

    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles btngametopup.Click
        Dim topUpAmount As Decimal
        If Decimal.TryParse(txtgameamount.Text, topUpAmount) Then
            Dim currentBalance As Decimal = Decimal.Parse(balance.Text)
            If topUpAmount > 0 AndAlso currentBalance >= topUpAmount Then
                Dim newBalance As Decimal = currentBalance - topUpAmount

                Try
                    connect.Open()
                    Dim sql As String = "UPDATE admin SET balance = ? WHERE [user] = ?"
                    Dim cmd As New OleDbCommand(sql, connect)
                    cmd.Parameters.AddWithValue("?", newBalance)
                    cmd.Parameters.AddWithValue("?", Form1.user.Text)
                    cmd.ExecuteNonQuery()

                    balance.Text = newBalance.ToString("N2")
                    MessageBox.Show("Game top-up successfully added to ID: " & txtgameid.Text)
                Catch ex As Exception
                    MessageBox.Show("Failed to process top-up: " & ex.Message)
                Finally
                    connect.Close()
                End Try
            Else
                MessageBox.Show("Insufficient balance or invalid amount.")
            End If
        Else
            MessageBox.Show("Please enter a valid top-up amount.")
        End If
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles txtmobilenumber.TextChanged

    End Sub

    Private Sub btnload_Click(sender As Object, e As EventArgs) Handles btnload.Click
        Dim loadAmount As Decimal
        If Decimal.TryParse(txtloadamount.Text, loadAmount) Then
            Dim currentBalance As Decimal = Decimal.Parse(balance.Text)
            If loadAmount > 0 AndAlso currentBalance >= loadAmount Then
                Dim newBalance As Decimal = currentBalance - loadAmount

                Try
                    connect.Open()
                    Dim sql As String = "UPDATE admin SET balance = ? WHERE [user] = ?"
                    Dim cmd As New OleDbCommand(sql, connect)
                    cmd.Parameters.AddWithValue("?", newBalance)
                    cmd.Parameters.AddWithValue("?", Form1.user.Text)
                    cmd.ExecuteNonQuery()

                    balance.Text = newBalance.ToString("N2")
                    MessageBox.Show("Load successfully sent to " & txtmobilenumber.Text)
                Catch ex As Exception
                    MessageBox.Show("Failed to process load: " & ex.Message)
                Finally
                    connect.Close()
                End Try
            Else
                MessageBox.Show("Insufficient balance or invalid amount.")
            End If
        Else
            MessageBox.Show("Please enter a valid load amount.")
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        AnimatePanelWidth(Panel5, 11)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        AnimatePanelWidth(Panel5, 531)
    End Sub

    Private Sub btnprintreceipt_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub btnloan_Click(sender As Object, e As EventArgs) Handles btnloan.Click
        Dim loanAmount As Decimal

        If Decimal.TryParse(txtloanamount.Text, loanAmount) Then
            If loanAmount > 0 Then
                Try
                    If connect.State = ConnectionState.Open Then connect.Close()
                    connect.Open()

                    Dim sql As String = "UPDATE admin SET balance = balance + ? WHERE [user] = ?"
                    Dim cmd As New OleDbCommand(sql, connect)
                    cmd.Parameters.AddWithValue("?", loanAmount)
                    cmd.Parameters.AddWithValue("?", Form1.user.Text.Trim())
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show("Loan granted successfully. Amount: ₱" & loanAmount.ToString("N2"), "Loan Success")
                    Else
                        MessageBox.Show("Loan failed. User not found.", "Error")
                    End If

                Catch ex As Exception
                    MessageBox.Show("Loan error: " & ex.Message, "Database Error")
                Finally
                    If connect.State = ConnectionState.Open Then connect.Close()
                End Try
                loadBalance()
            Else
                MessageBox.Show("Loan amount must be greater than 0.", "Invalid Amount")
            End If
        Else
            MessageBox.Show("Please enter a valid loan amount.", "Invalid Input")
        End If
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        AnimatePanelWidth(Panel4, 11)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        AnimatePanelWidth(Panel4, 535)
    End Sub

    Private Sub Panel6_Paint(sender As Object, e As PaintEventArgs) Handles Panel6.Paint

    End Sub
    Sub loadUserProfile()
        Try
            connect.Open()

            Dim sql As String = "SELECT [user], email, contact, gender, nationality, dob, balance FROM admin WHERE [user] = ?"
            Dim cmd As New OleDbCommand(sql, connect)
            cmd.Parameters.AddWithValue("?", Form1.user.Text.Trim())

            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                lbluser.Text = " " & reader("user").ToString()
                lblemail.Text = " " & reader("email").ToString()
                lblcontact.Text = " " & reader("contact").ToString()
                lblgender.Text = " " & reader("gender").ToString()
                lblnationality.Text = " " & reader("nationality").ToString()
                lblprofilebalance.Text = " ₱" & Convert.ToDecimal(reader("balance")).ToString("N2")
                lbldob.Text = " " & reader("dob").ToString()
            Else
                MessageBox.Show("User profile not found.", "Notice")
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Failed to load profile: " & ex.Message, "Error")
        Finally
            If connect.State = ConnectionState.Open Then connect.Close()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label4.Hide()
        dpuser.Hide()
        AnimatePanelWidth(Panel6, 530)

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label4.Show()
        dpuser.Show()
        AnimatePanelWidth(Panel6, 10)
    End Sub



    Private Sub timerdatetime_Tick(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub dpuser_Click(sender As Object, e As EventArgs) Handles dpuser.Click

    End Sub

    Private Sub Panel7_Paint(sender As Object, e As PaintEventArgs) Handles Panel7.Paint

    End Sub

    Private Sub Button20_Click_1(sender As Object, e As EventArgs) Handles Button20.Click
        AnimatePanelWidth(Panel7, 13)
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        AnimatePanelWidth(Panel7, 535)
    End Sub

    Private Sub balance_Click(sender As Object, e As EventArgs) Handles balance.Click

    End Sub
End Class

