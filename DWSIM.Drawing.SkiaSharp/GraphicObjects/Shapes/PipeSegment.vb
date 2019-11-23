Imports System.IO
Imports DWSIM.Drawing.SkiaSharp.GraphicObjects
Imports DWSIM.Interfaces.Enums.GraphicObjects
Imports DWSIM.DrawingTools.Point
Imports SkiaSharp.Extended.Svg

Namespace GraphicObjects.Shapes

    Public Class PipeSegmentGraphic

        Inherits ShapeGraphic

#Region "Constructors"

        Public Sub New()
            Me.ObjectType = DWSIM.Interfaces.Enums.GraphicObjects.ObjectType.Pipe
            Me.Description = "Сегмент трубы"
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
            Me.EnergyConnector.Direction = ConDir.Down
            Me.EnergyConnector.ConnectorName = "Energy Stream"

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

        Shared Dim svgLeft As SKSvg
        Shared Dim svgCenter As SKSvg
        Shared Dim svgRight As SKSvg

        Shared Sub New()
            'alexander load SVG template
            svgLeft = New SKSvg()
            svgCenter = New SKSvg()
            svgRight = New SKSvg()
            Using svgMs = New MemoryStream(My.Resources.item_PipeSegment_left)
                svgLeft.Load(svgMs)
            End using

            Using svgMs = New MemoryStream(My.Resources.item_PipeSegment_center)
                svgCenter.Load(svgMs)
            End using            
            
            Using svgMs = New MemoryStream(My.Resources.item_PipeSegment_right)
                svgRight.Load(svgMs)
            End using
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

            Dim rect1 As New SKRect(X , Y, X + Width, Y + Height)
            Dim rect0 As New SKRect(X, Y, X + 0.1 * Width, Y + Height) 'left oval
            Dim rect2 As New SKRect(X + 0.9 * Width, Y, X + Width, Y + Height) 'right oval

            canvas.DrawOval(rect0, myPen2)
            canvas.DrawOval(rect2, myPen2)

            If GradientMode Then

                Dim gradPen As New SKPaint()
                With gradPen
                    .Color = LineColor
                    .StrokeWidth = LineWidth
                    .IsStroke = False
                    .IsAntialias = GlobalSettings.Settings.DrawingAntiAlias
                    .Shader = SKShader.CreateLinearGradient(New SKPoint(X, Y), New SKPoint(X, Y + Height),
                                    New SKColor() {SKColors.White, LineColor},
                                    New Single() {0.0, 1.0}, SKShaderTileMode.Clamp)
                End With

                canvas.DrawRect(rect1, gradPen)
                canvas.DrawOval(rect0, gradPen)
                canvas.DrawOval(rect2, gradPen)
                canvas.DrawOval(rect0, myPen2)

            End If

            'set bounds from SVG
            'Dim boundsSvgLeft = svgLeft.CanvasSize
            'Dim boundsSvgRight = svgRight.CanvasSize
            'Dim boundsSvgCenter = svgCenter.CanvasSize

            'Dim yRatio = Height / boundsSvgLeft.Height
            'Dim widthInSvgCoords = Width/yRatio

            'Dim centerWidthInSvgCoord = widthInSvgCoords - boundsSvgLeft.Width - boundsSvgRight.Width
            'Dim allWidthInSvgCoord = centerWidthInSvgCoord + boundsSvgLeft.Width + boundsSvgRight.Width
            'Dim cnterZoom  = centerWidthInSvgCoord/boundsSvgCenter.Width

            'Dim xRatio = Width*0.95 / allWidthInSvgCoord

            'Dim leftXRatio = xRatio
            'Dim rightXRatio = xRatio
            'Dim centerXRatio = cnterZoom * xRatio

            'Dim leftX = X + Width*0.05
            'Dim rightX = X - Width*0.05 + Width-xRatio*boundsSvgRight.Width
            'Dim centerX = X+Width*0.05 + leftXRatio * boundsSvgLeft.Width

            ''save canvas state
            'canvas.Save()
            ''draw SVG Left
            'canvas.Translate(leftX, Y)
            'canvas.Scale(leftXRatio, yRatio)
            'canvas.DrawPicture(svgLeft.Picture)
            ''restore canvas state
            'canvas.Restore()

            ''save canvas state
            'canvas.Save()
            ''draw SVG Right
            'canvas.Translate(rightX, Y)
            'canvas.Scale(rightXRatio, yRatio)
            'canvas.DrawPicture(svgRight.Picture)
            ''restore canvas state
            'canvas.Restore()

            ''save canvas state
            'canvas.Save()
            ''draw SVG center
            'canvas.Translate(centerX, Y)
            'canvas.Scale(centerXRatio, yRatio)
            'canvas.DrawPicture(svgCenter.Picture)
            ''restore canvas state
            'canvas.Restore()

            'canvas.DrawRect(rect1, myPen2)
            myPen2.Dispose()
        End Sub

    End Class

End Namespace
