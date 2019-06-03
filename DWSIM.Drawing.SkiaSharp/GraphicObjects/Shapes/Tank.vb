﻿Imports DWSIM.Drawing.SkiaSharp.GraphicObjects
Imports DWSIM.Interfaces.Enums.GraphicObjects
Imports DWSIM.DrawingTools.Point

Namespace GraphicObjects.Shapes

    Public Class TankGraphic

        Inherits ShapeGraphic

#Region "Constructors"

        Public Sub New()
            Me.ObjectType = DWSIM.Interfaces.Enums.GraphicObjects.ObjectType.Tank
            Me.Description = "Емкость хранения жидкой фракции"
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

        Public Overrides Sub PositionConnectors()

            CreateConnectors(0, 0)

        End Sub

        Public Overrides Sub CreateConnectors(InCount As Integer, OutCount As Integer)

            Dim myIC1 As New ConnectionPoint
            myIC1.Position = New Point(X, Y + 0.5 * Height)
            myIC1.Type = ConType.ConIn

            Dim myOC1 As New ConnectionPoint
            myOC1.Position = New Point(X + Width, Y + 0.5 * Height)
            myOC1.Type = ConType.ConOut

            Me.EnergyConnector.Position = New Point(X + 0.5 * Width, Y + Height)
            Me.EnergyConnector.Type = ConType.ConEn
            Me.EnergyConnector.Active = False

            With InputConnectors

                If .Count <> 0 Then
                    .Item(0).Position = New Point(X, Y + 0.5 * Height)
                Else
                    .Add(myIC1)
                End If

                .Item(0).ConnectorName = "Inlet"

            End With

            With OutputConnectors

                If .Count <> 0 Then
                    .Item(0).Position = New Point(X + Width, Y + 0.5 * Height)
                Else
                    .Add(myOC1)
                End If

                .Item(0).ConnectorName = "Outlet"

            End With

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

            Dim myPen3 As New SKPaint()
            With myPen3
                .Color = If(GlobalSettings.Settings.DarkMode, SKColors.Black, SKColors.White)
                .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
                .IsStroke = False
            End With

            'center
            Dim rect1 As New SKRect(X + 0.1 * Width, Y + 0.1 * Height, X + 0.1 * Width + 0.8 * Width, Y + 0.1 * Height + 0.8 * Height)
            'top
            Dim rect2 As New SKRect(X + 0.1 * Width, Y, X + 0.1 * Width + 0.8 * Width, Y + 0.2 * Height)
            'bottom
            Dim rect3 As New SKRect(X + 0.1 * Width, Y + 0.8 * Height, X + 0.1 * Width + 0.8 * Width, Y + 0.8 * Height + 0.2 * Height)

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

                canvas.DrawOval(rect3, gradPen)
                canvas.DrawRect(rect1, gradPen)
                canvas.DrawOval(rect2, gradPen)

                canvas.DrawOval(New SKRect(X + 0.2 * Width, Y + 0.6 * Height, X + 0.5 * Width, Y + 0.7 * Height), gradPen)
                canvas.DrawOval(New SKRect(X + 0.5 * Width, Y + 0.6 * Height, X + 0.8 * Width, Y + 0.7 * Height), gradPen)

            End If

            canvas.DrawRect(rect1, myPen2)
            If Not GradientMode Then canvas.DrawRect(rect1, myPen3)

            If Not GradientMode Then canvas.DrawOval(rect2, myPen3)

            If Not GradientMode Then canvas.DrawOval(rect2, myPen3)
            canvas.DrawOval(rect2, myPen2)

            If Not GradientMode Then canvas.DrawOval(rect3, myPen3)
            canvas.DrawOval(rect3, myPen2)

            Dim rect4 As New SKRect(X + 0.1 * Width, Y + 0.5 * Height, X + 0.1 * Width + 0.8 * Width, Y + 0.1 * Height + 0.8 * Height)

            If Not GradientMode Then canvas.DrawRect(rect4, myPen3)

        End Sub

    End Class

End Namespace