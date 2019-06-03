﻿Imports DWSIM.Drawing.SkiaSharp.GraphicObjects
Imports DWSIM.Interfaces.Enums.GraphicObjects
Imports DWSIM.DrawingTools.Point

Namespace GraphicObjects.Shapes

    Public Class EnergyStreamGraphic

        Inherits ShapeGraphic

#Region "Constructors"

        Public Sub New()
            Me.ObjectType = DWSIM.Interfaces.Enums.GraphicObjects.ObjectType.EnergyStream
            Me.Description = "Поток энергии"
            Me.IsEnergyStream = True
        End Sub

        Public Sub New(ByVal graphicPosition As SKPoint)
            Me.New()
            CreateConnectors(1, 1)
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

            With InputConnectors

                If .Count <> 0 Then
                    .Item(0).Position = New Point(X, Y + 0.5 * Height)
                Else
                    .Add(myIC1)
                End If

            End With

            With OutputConnectors

                If .Count <> 0 Then
                    .Item(0).Position = New Point(X + Width, Y + 0.5 * Height)
                Else
                    .Add(myOC1)
                End If

            End With

        End Sub

        Public Overrides Sub Draw(ByVal g As Object)

            Dim canvas As SKCanvas = DirectCast(g, SKCanvas)

            CreateConnectors(0, 0)
            UpdateStatus()

            MyBase.Draw(g)


            Dim myPen As New SKPaint()
            With myPen
                .Color = LineColor
                .StrokeWidth = LineWidth
                .IsStroke = Not Fill
                .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
                .PathEffect = SKPathEffect.CreateCorner(1.0F)
            End With

            Dim gp As New SKPath()

            gp.MoveTo(Convert.ToInt32(X), Convert.ToInt32(Y + 0.35 * Height))
            gp.LineTo(Convert.ToInt32(X + 0.75 * Width), Convert.ToInt32(Y + 0.35 * Height))
            gp.LineTo(Convert.ToInt32(X + 0.75 * Width), Convert.ToInt32(Y + 0.25 * Height))
            gp.LineTo(Convert.ToInt32(X + Width), Convert.ToInt32(Y + 0.5 * Height))
            gp.LineTo(Convert.ToInt32(X + 0.75 * Width), Convert.ToInt32(Y + 0.75 * Height))
            gp.LineTo(Convert.ToInt32(X + 0.75 * Width), Convert.ToInt32(Y + 0.65 * Height))
            gp.LineTo(Convert.ToInt32(X), Convert.ToInt32(Y + 0.65 * Height))
            gp.LineTo(Convert.ToInt32(X), Convert.ToInt32(Y + 0.35 * Height))

            gp.Close()

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
                                    New SKColor() {SKColors.White, SKColors.Yellow},
                                    Nothing, SKShaderTileMode.Clamp)
                End With

                canvas.DrawPath(gp, gradPen)

            End If

            canvas.DrawPath(gp, myPen)

            'If Not OverrideColors Then
            '    Dim fillPen As New SKPaint()
            '    With fillPen
            '        .Color = SKColors.Yellow
            '        .StrokeWidth = LineWidth
            '        .IsStroke = False
            '        .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
            '    End With
            '    canvas.DrawPath(gp, fillPen)
            'End If

        End Sub

    End Class

End Namespace