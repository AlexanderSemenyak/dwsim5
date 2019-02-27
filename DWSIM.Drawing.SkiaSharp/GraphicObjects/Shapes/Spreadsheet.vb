﻿Imports DWSIM.Drawing.SkiaSharp.GraphicObjects
Imports DWSIM.Interfaces.Enums.GraphicObjects
Imports DWSIM.DrawingTools.Point

Namespace GraphicObjects.Shapes

    Public Class SpreadsheetGraphic

        Inherits ShapeGraphic

#Region "Constructors"

        Public Sub New()
            Me.ObjectType = DWSIM.Interfaces.Enums.GraphicObjects.ObjectType.ExcelUO
            Me.Description = "Блок Excel"
        End Sub

        Public Sub New(ByVal graphicPosition As SKPoint)
            Me.New()
            Me.SetPosition(graphicPosition)
        End Sub

        Public Sub New(ByVal posX As Integer, ByVal posY As Integer)
            Me.New(New SKPoint(posX, posY))
        End Sub

        Public Sub New(ByVal graphicPosition As SKPoint, ByVal graphicSize As SKSize)
            Me.New(graphicPosition)
            Me.SetSize(graphicSize)
        End Sub

        Public Sub New(ByVal posX As Integer, ByVal posY As Integer, ByVal graphicSize As SKSize)
            Me.New(New SKPoint(posX, posY), graphicSize)
        End Sub

        Public Sub New(ByVal posX As Integer, ByVal posY As Integer, ByVal width As Integer, ByVal height As Integer)
            Me.New(New SKPoint(posX, posY), New SKSize(width, height))
        End Sub

#End Region

        Public Overrides Sub CreateConnectors(InCount As Integer, OutCount As Integer)

            Dim myIC1 As New ConnectionPoint
            myIC1.Position = New Point(X, Y + 0 * Height)
            myIC1.Type = ConType.ConIn

            Dim myIC2 As New ConnectionPoint
            myIC2.Position = New Point(X, Y + 0.33 * Height)
            myIC2.Type = ConType.ConIn

            Dim myIC3 As New ConnectionPoint
            myIC3.Position = New Point(X, Y + 0.66 * Height)
            myIC3.Type = ConType.ConIn

            Dim myIC4 As New ConnectionPoint
            myIC4.Position = New Point(X, Y + 1 * Height)
            myIC4.Type = ConType.ConIn


            Dim myEC1 As New ConnectionPoint
            myEC1.Position = New Point(X + 0.5 * Width, Y + Height)
            myEC1.Type = ConType.ConEn


            Dim myOC1 As New ConnectionPoint
            myOC1.Position = New Point(X + Width, Y + 0 * Height)
            myOC1.Type = ConType.ConOut

            Dim myOC2 As New ConnectionPoint
            myOC2.Position = New Point(X + Width, Y + 0.33 * Height)
            myOC2.Type = ConType.ConOut

            Dim myOC3 As New ConnectionPoint
            myOC3.Position = New Point(X + Width, Y + 0.66 * Height)
            myOC3.Type = ConType.ConOut

            Dim myOC4 As New ConnectionPoint
            myOC4.Position = New Point(X + Width, Y + 1 * Height)
            myOC4.Type = ConType.ConOut

            Me.EnergyConnector.Position = New Point(X + 0.5 * Width, Y + Height)
            Me.EnergyConnector.Type = ConType.ConEn
            Me.EnergyConnector.Direction = ConDir.Up

            With InputConnectors

                If .Count <> 0 Then
                    .Item(0).Position = New Point(X, Y + 0 * Height)
                    .Item(1).Position = New Point(X, Y + 0.33 * Height)
                    .Item(2).Position = New Point(X, Y + 0.66 * Height)
                    .Item(3).Position = New Point(X, Y + 1 * Height)
                    .Item(4).Position = New Point(X + 0.5 * Width, Y + Height)
                Else
                    .Add(myIC1)
                    .Add(myIC2)
                    .Add(myIC3)
                    .Add(myIC4)
                    .Add(myEC1)
                End If

            End With

            With OutputConnectors

                If .Count <> 0 Then
                    .Item(0).Position = New Point(X + Width, Y + 0 * Height)
                    .Item(1).Position = New Point(X + Width, Y + 0.33 * Height)
                    .Item(2).Position = New Point(X + Width, Y + 0.66 * Height)
                    .Item(3).Position = New Point(X + Width, Y + 1 * Height)
                Else
                    .Add(myOC1)
                    .Add(myOC2)
                    .Add(myOC3)
                    .Add(myOC4)
                End If

            End With

            Me.EnergyConnector.Active = False

        End Sub

        Public Overrides Sub Draw(ByVal g As Object)

            Dim canvas As SKCanvas = DirectCast(g, SKCanvas)

            CreateConnectors(0, 0)
            UpdateStatus()

            MyBase.Draw(g)

            Dim myPen2 As New SKPaint()
            With myPen2
                .Color = LineColor
                .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
                .IsStroke = True
                .StrokeWidth = LineWidth
            End With

            Dim rect1 As New SKRect(X + 0.1 * Width, Y, X + 0.9 * Width, Y + Height)


            If GradientMode Then

                Dim r0 As New SKRect(X, Y, X + Width, Y + Height)

                Dim radius2 = 0.8F * Math.Min(Width, Height)
                Dim center = New SKPoint(r0.MidX, r0.MidY)
                Dim offCenter = center - New SKPoint(radius2 / 2, radius2 / 2)

                Dim gradPen As New SKPaint()
                With gradPen
                    .Color = LineColor
                    .StrokeWidth = LineWidth
                    .IsStroke = False
                    .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
                    .Shader = SKShader.CreateTwoPointConicalGradient(
                                    offCenter, 1, center, radius2,
                                    New SKColor() {SKColors.White, LineColor},
                                    Nothing, SKShaderTileMode.Clamp)
                End With

                canvas.DrawRoundRect(rect1, 2, 2, gradPen)

            End If

            canvas.DrawRoundRect(rect1, 2, 2, myPen2)

            Dim tpaint As New SKPaint()

            With tpaint
                .TextSize = 10.0#
                .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
                .Color = LineColor
                .IsStroke = False
                .Typeface = DefaultTypeFace
            End With

            Dim trect As New SKRect(0, 0, 2, 2)
            tpaint.GetTextPath("XLS", 0, 0).GetBounds(trect)

            Dim ax, ay As Integer
            ax = Me.X + (Me.Width - (trect.Right - trect.Left)) / 2
            ay = Me.Y + (Me.Height - (trect.Top - trect.Bottom)) / 2

            If FlippedH Or FlippedV Or Rotation <> 0 Then

                Dim currmat = canvas.TotalMatrix

                canvas.Save()

                If FlippedV And Not FlippedH Then
                    canvas.Scale(1, -1, (X + Width / 2), (Y + Height / 2))
                ElseIf FlippedH And Not FlippedV Then
                    canvas.Scale(-1, 1, (X + Width / 2), (Y + Height / 2))
                ElseIf FlippedH And FlippedV Then
                    canvas.Scale(-1, -1, (X + Width / 2), (Y + Height / 2))
                End If

                If Rotation <> 0.0 Then canvas.RotateDegrees(Rotation, X + Width / 2, Y + Height / 2)

                canvas.DrawText("XLS", ax, ay, tpaint)

                canvas.SetMatrix(currmat)

            Else

                canvas.DrawText("XLS", ax, ay, tpaint)

            End If

        End Sub

    End Class

End Namespace