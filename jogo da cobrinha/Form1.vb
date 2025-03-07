Public Class Form1
    Public cabeca As New PictureBox
    Public fruta As New PictureBox
    Public corpo As New List(Of PictureBox)

    Public tamanhobase As Integer = 40

    Public random As New Random
    Public direcao As String = ""
    Public executou As Boolean = False



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With cabeca
            .Width = tamanhobase
            .Height = .Width
            .BackColor = Color.Green
            .Top = Me.Height / 2
            .Left = Me.Width / 2
            .Name = "cabeca"
        End With
        With fruta
            .Width = tamanhobase
            .Height = .Width
            .BackColor = Color.Red
            .Name = "fruta"
            .Location = New Point(random.Next(0, Me.Width - tamanhobase), random.Next(0, Me.Height - tamanhobase))
        End With
        direcao = "direita"
        Me.Controls.Add(cabeca)
        Me.Controls.Add(fruta)
        corpo.Add(cabeca)
        Timer1.Enabled = True
        Timer2.Enabled = True
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim numerodepartes = corpo.Count
        Do While numerodepartes > 1
            numerodepartes -= 1
            corpo.Item(numerodepartes).Location = corpo.Item(numerodepartes - 1).Location
            If corpo.Item(numerodepartes).Visible = False Then
                corpo.Item(numerodepartes).Visible = True
            End If
        Loop

        With cabeca
            Select Case direcao
                Case = "direita"
                    .Left += .Width
                Case = "esquerda"
                    .Left -= .Width
                Case = "cima"
                    .Top -= .Width
                Case = "baixo"
                    .Top += .Width
            End Select
        End With
        executou = True
        colisaocorpo()
    End Sub

    Public Sub colisaocorpo()
        For Each parte In corpo
            If parte.Name <> "cabeca" And parte.Bounds.IntersectsWith(Controls.Find("cabeca", True)(0).Bounds) Then
                For i = corpo.IndexOf(parte) To (corpo.Count - 1) - corpo.IndexOf(parte)
                    Me.Controls.Remove(corpo(i))
                Next
                corpo.RemoveRange(corpo.IndexOf(parte), (corpo.Count - 1) - corpo.IndexOf(parte))
                Exit For
            End If
        Next
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If cabeca.Bounds.IntersectsWith(fruta.Bounds) Then
            fruta.Location = New Point(random.Next(0, Me.Width - tamanhobase), random.Next(0, Me.Height - tamanhobase))
            Dim pedaco As New PictureBox
            With pedaco
                .Width = tamanhobase
                .Height = .Width
                .BackColor = Color.Cyan
                .Top = Me.Height / 2
                .Left = Me.Width / 2
                .Visible = False
                .Name = "pedaco"
            End With
            Me.Controls.Add(pedaco)
            corpo.Add(pedaco)
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If executou = True Then
            executou = True
            Select Case e.KeyCode
                Case Keys.A And direcao <> "direita"
                    direcao = "esquerda"
                Case Keys.D And direcao <> "esquerda"
                    direcao = "direita"
                Case Keys.W And direcao <> "baixo"
                    direcao = "cima"
                Case Keys.S And direcao <> "cima"
                    direcao = "baixo"
                Case Else
                    executou = False
            End Select

        End If
    End Sub
End Class
