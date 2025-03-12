Public Class Form1
    Public cabeca As New PictureBox
    Public fruta As New PictureBox
    Public corpo As New List(Of PictureBox)

    Public tamanhobase As Integer = 40
    Public tamanhodomapa As Integer() = {20, 10}

    Public random As New Random
    Public direcao As String = ""
    Public executou As Boolean = False

    Dim form2 As New Form

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Width = tamanhodomapa(0) * tamanhobase
        Me.Height = tamanhodomapa(1) * tamanhobase


        form2.Show()
        form2.Height = 0
        form2.Width = Me.Width

        Timer3.Enabled = True

        With cabeca
            .Width = tamanhobase
            .Height = .Width
            .BackColor = Color.Green
            .Location = lugaraleatorionomapa()
            .Name = "cabeca"
        End With
        With fruta
            .Width = tamanhobase
            .Height = .Width
            .BackColor = Color.Red
            .Name = "fruta"
            .Location = lugaraleatorionomapa()
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
            .Location = ColisaoComParede(cabeca)
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
                Dim PartesSeremRemovidas = corpo.GetRange(corpo.IndexOf(parte), (corpo.Count - 1) - corpo.IndexOf(parte))
                For Each i As Control In PartesSeremRemovidas
                    Me.Controls.Remove(i)
                Next
                corpo.RemoveRange(corpo.IndexOf(parte), (corpo.Count - 1) - corpo.IndexOf(parte))
                Exit For
            End If
        Next
    End Sub

    Public Function lugaraleatorionomapa() As Point
        Dim x As Integer = 0
        Dim y As Integer = 0
        y = random.Next(0, Int32.Parse(Me.Height / tamanhobase)) * tamanhobase
        x = random.Next(0, Int32.Parse(Me.Width / tamanhobase)) * tamanhobase
        Dim posdic As New Dictionary(Of String, Integer)
        For Each parte In corpo
            posdic.Add(parte.Left & ":" & parte.Top, 0)
        Next

        Do While x > Me.Width - tamanhobase Or y > Me.Height - tamanhobase Or posdic.ContainsKey(x & ":" & y) = True
            If posdic.ContainsKey(x & ":" & y) = True Then
                x = random.Next(0, Int32.Parse(Me.Width / tamanhobase)) * tamanhobase
                y = random.Next(0, Int32.Parse(Me.Height / tamanhobase)) * tamanhobase
            ElseIf x > Me.Width - tamanhobase Then
                x = random.Next(0, Int32.Parse(Me.Width / tamanhobase)) * tamanhobase
            Else
                y = random.Next(0, Int32.Parse(Me.Height / tamanhobase)) * tamanhobase
            End If
        Loop

        Return New Point(x, y)
    End Function

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        If cabeca.Bounds.IntersectsWith(fruta.Bounds) Then
            fruta.Location = lugaraleatorionomapa()
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

    Public Function ColisaoComParede(controle As Control) As Point
        Dim x As Integer = controle.Left
        Dim y As Integer = controle.Top

        If controle.Left < 0 Then
            x = Me.Width
            direcao = "esquerda"
        ElseIf controle.Left >= Me.Width Then
            x = 0 - tamanhobase
            direcao = "direita"
        End If

        If controle.Top < 0 Then
            y = Me.Height
            direcao = "cima"
        ElseIf controle.Top >= Me.Height Then
            y = 0 - tamanhobase
            direcao = "baixo"
        End If

        Return New Point(x, y)
    End Function

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

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        If form2.IsDisposed Then
            Me.Close()
        End If
        Me.Left = form2.Left
        Me.Top = form2.Top + 40
        form2.Text = "[                    ]"
    End Sub
End Class
