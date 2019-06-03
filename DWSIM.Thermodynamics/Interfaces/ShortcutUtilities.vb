﻿Imports System.Reflection
Imports System.Runtime.Serialization
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series
Imports DWSIM.Thermodynamics.PropertyPackages
Imports DWSIM.Thermodynamics.Streams
Imports System.Globalization
Imports DWSIM.SharedClasses.SystemsOfUnits
Imports DWSIM.ExtensionMethods
Imports System.Linq

Namespace ShortcutUtilities

    Public Enum CalculationType

        CriticalPoint = 1
        PhaseEnvelopePT = 2
        PhaseEnvelopePH = 3
        PhaseEnvelopePS = 4
        PhaseEnvelopeTH = 5
        PhaseEnvelopeTS = 6
        PhaseEnvelopeVT = 7
        PhaseEnvelopeVP = 8
        BinaryEnvelopeTxy = 9
        BinaryEnvelopePxy = 10

    End Enum

    Public Class CalculationResults

        Public Property Data As Dictionary(Of String, List(Of Double))
        Public Property DataUnits As Dictionary(Of String, String)
        Public Property CompoundData As Dictionary(Of String, Object)
        Public Property TextOutput As String = ""
        Public Property PlotModels As List(Of Global.OxyPlot.Model)
        Public Property Units As Units
        Public Property ExceptionResult As Exception
        Public Property NumberFormat As String = "0.####"
        Public Property Language As String = "en"

        Sub New()
            Data = New Dictionary(Of String, List(Of Double))
            DataUnits = New Dictionary(Of String, String)
            CompoundData = New Dictionary(Of String, Object)
            PlotModels = New List(Of Global.OxyPlot.Model)
            Units = New SI
        End Sub

    End Class

    Public Class Calculation

        Public Property CalcType As CalculationType = CalculationType.CriticalPoint
        Public Property Language As String = "en"
        Public Property PhaseEnvelopeOptions As PropertyPackages.PhaseEnvelopeOptions
        Public Property BinaryEnvelopeOptions As Object()

        Private _MaterialStream As MaterialStream

        Sub New(ByVal stream As MaterialStream)

            _MaterialStream = stream

        End Sub

        Function Calculate() As CalculationResults

            Dim Compounds As String() = _MaterialStream.Phases(0).Compounds.Values.Select(Function(x) x.Name).ToArray()
            Dim pp As PropertyPackage = _MaterialStream.PropertyPackage
            pp.CurrentMaterialStream = _MaterialStream
            Dim PropertyPackageName As String = pp.ComponentName
            Dim MixName As String = ""

            If _MaterialStream.GraphicObject IsNot Nothing Then
                MixName = _MaterialStream.GraphicObject.Tag
            End If

            Dim MixTemperature As Double = _MaterialStream.Phases(0).Properties.temperature.GetValueOrDefault
            Dim MixPressure As Double = _MaterialStream.Phases(0).Properties.pressure.GetValueOrDefault
            Dim MixEnthalpy As Double = _MaterialStream.Phases(0).Properties.enthalpy.GetValueOrDefault
            Dim MixEntropy As Double = _MaterialStream.Phases(0).Properties.entropy.GetValueOrDefault
            Dim FlashAlgorithm = pp.FlashBase.AlgoType
            Dim NumberFormat As String = _MaterialStream.FlowSheet.FlowsheetOptions.NumberFormat
            Dim Units As Units = _MaterialStream.FlowSheet.FlowsheetOptions.SelectedUnitSystem

            Dim results As New CalculationResults() With {.Units = Units, .NumberFormat = NumberFormat, .Language = Language}

            Try

                Select Case CalcType

                    Case CalculationType.CriticalPoint

                        If pp.ComponentName.Equals("Peng-Robinson (PR)") Then

                            Dim res As ArrayList = DirectCast(pp, PengRobinsonPropertyPackage).ReturnCriticalPoints()

                            For Each dl As Double() In res
                                results.Data.Add("CriticalPoint", New List(Of Double) From {dl(0).ConvertFromSI(Units.temperature), dl(1).ConvertFromSI(Units.pressure), dl(2).ConvertFromSI(Units.molar_volume)})
                            Next

                            If results.Language.Equals("pt-BR") Then

                                results.TextOutput += "Resultados do cálculo do Ponto Crítico Verdadeiro para a mistura " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Pacote de Propriedades: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                If res(0)(3) = 0.0# Then results.TextOutput += "Cálculo do Ponto Crítico Verdadeiro falhou. Mostrando informações como uma média ponderada das propriedades individuais." & System.Environment.NewLine
                                results.TextOutput += "Temperatura crítica: " & results.Data("CriticalPoint")(0).ToString(results.NumberFormat) & " " & Units.temperature & System.Environment.NewLine
                                results.TextOutput += "Pressão crítica: " & results.Data("CriticalPoint")(1).ToString(results.NumberFormat) & " " & Units.pressure & System.Environment.NewLine
                                results.TextOutput += "Volume crítico: " & results.Data("CriticalPoint")(2).ToString(results.NumberFormat) & " " & Units.molar_volume & System.Environment.NewLine

                            Else

                                results.TextOutput += "True Critical Point calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                If res(0)(3) = 0.0# Then results.TextOutput += "True Critical Point calculation failed. Showing mole-fraction averaged compound values." & System.Environment.NewLine
                                results.TextOutput += "Critical Temperature: " & results.Data("CriticalPoint")(0).ToString(results.NumberFormat) & " " & Units.temperature & System.Environment.NewLine
                                results.TextOutput += "Critical Pressure: " & results.Data("CriticalPoint")(1).ToString(results.NumberFormat) & " " & Units.pressure & System.Environment.NewLine
                                results.TextOutput += "Critical Volume: " & results.Data("CriticalPoint")(2).ToString(results.NumberFormat) & " " & Units.molar_volume & System.Environment.NewLine

                            End If

                        ElseIf pp.ComponentName.Equals("Soave-Redlich-Kwong (SRK)") Then

                            Dim res As New List(Of Double()) '= DirectCast(pp, SimulationObjects.PropertyPackages.SoaveRedlichKwongPropertyPackage).ReturnCriticalPoints()

                            For Each dl As Double() In res
                                results.Data.Add("CriticalPoint", New List(Of Double) From {dl(0).ConvertFromSI(Units.temperature), dl(1).ConvertFromSI(Units.pressure), dl(2).ConvertFromSI(Units.molar_volume)})
                            Next

                            If results.Language.Equals("pt-BR") Then

                                results.TextOutput += "Resultados do cálculo do Ponto Crítico Verdadeiro para a mistura " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Pacote de Propriedades: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                If res(0)(3) = 0.0# Then results.TextOutput += "Cálculo do Ponto Crítico Verdadeiro falhou. Mostrando informações como uma média ponderada das propriedades individuais." & System.Environment.NewLine
                                results.TextOutput += "Temperatura crítica: " & results.Data("CriticalPoint")(0).ToString(results.NumberFormat) & " " & Units.temperature & System.Environment.NewLine
                                results.TextOutput += "Pressão crítica: " & results.Data("CriticalPoint")(1).ToString(results.NumberFormat) & " " & Units.pressure & System.Environment.NewLine
                                results.TextOutput += "Volume crítico: " & results.Data("CriticalPoint")(2).ToString(results.NumberFormat) & " " & Units.molar_volume & System.Environment.NewLine

                            Else

                                results.TextOutput += "True Critical Point calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                If res(0)(3) = 0.0# Then results.TextOutput += "True Critical Point calculation failed. Showing mole-fraction averaged compound values." & System.Environment.NewLine
                                results.TextOutput += "Critical Temperature: " & results.Data("CriticalPoint")(0).ToString(results.NumberFormat) & " " & Units.temperature & System.Environment.NewLine
                                results.TextOutput += "Critical Pressure: " & results.Data("CriticalPoint")(1).ToString(results.NumberFormat) & " " & Units.pressure & System.Environment.NewLine
                                results.TextOutput += "Critical Volume: " & results.Data("CriticalPoint")(2).ToString(results.NumberFormat) & " " & Units.molar_volume & System.Environment.NewLine

                            End If

                        Else

                            If results.Language.Equals("pt-BR") Then
                                Throw New Exception("Modelo inválido.")
                            Else
                                Throw New Exception("The Critical Point utility works with PR or SRK Property Package only.")
                            End If

                        End If

                    Case CalculationType.BinaryEnvelopeTxy

                        Dim res As Object() = Nothing

                        BinaryEnvelopeOptions(0) = "T-x-y"
                        BinaryEnvelopeOptions(1) = MixPressure
                        BinaryEnvelopeOptions(2) = MixTemperature
                        res = pp.DW_ReturnBinaryEnvelope(BinaryEnvelopeOptions)

                        results.Data.Add("px", DirectCast(res(0), ArrayList).ToDoubleList())
                        results.DataUnits.Add("px", "")
                        results.Data.Add("py1", DirectCast(res(1), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("py1", Units.temperature)
                        results.Data.Add("py2", DirectCast(res(2), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("py2", Units.temperature)
                        results.Data.Add("px1l1", DirectCast(res(3), ArrayList).ToDoubleList())
                        results.DataUnits.Add("px1l1", "")
                        results.Data.Add("px1l2", DirectCast(res(4), ArrayList).ToDoubleList())
                        results.DataUnits.Add("px1l2", "")
                        results.Data.Add("py3", DirectCast(res(5), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("py3", Units.temperature)
                        results.Data.Add("pxs1", DirectCast(res(6), ArrayList).ToDoubleList())
                        results.DataUnits.Add("pxs1", "")
                        results.Data.Add("pys1", DirectCast(res(7), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("pys1", Units.temperature)
                        results.Data.Add("pxs2", DirectCast(res(8), ArrayList).ToDoubleList())
                        results.DataUnits.Add("pxs2", "")
                        results.Data.Add("pys2", DirectCast(res(9), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("pys2", Units.temperature)
                        results.Data.Add("pxc", DirectCast(res(10), ArrayList).ToDoubleList())
                        results.DataUnits.Add("pxc", "")
                        results.Data.Add("pyc", DirectCast(res(11), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("pyc", Units.temperature)

                        Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Binary Envelope (Txy) @ " &
                                            MixPressure.ConvertFromSI(Units.pressure).ToString(results.NumberFormat) & " " &
                                            Units.pressure, .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                        With model1
                            .TitleFontSize = 14
                            .SubtitleFontSize = 10
                            .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Mole Fraction " & Compounds(0), .FontSize = 12})
                            .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Temperature (" & Units.temperature & ")", .FontSize = 12})
                            .AddLineSeries(results.Data("px").ToArray, results.Data("py1").ToArray)
                            .AddLineSeries(results.Data("px").ToArray, results.Data("py2").ToArray)
                            .Series(0).Title = "Bubble Points"
                            .Series(1).Title = "Dew Points"
                            If results.Data("px1l1").Count > 0 Then
                                .AddLineSeries(results.Data("px1l1").ToArray, results.Data("py3").ToArray)
                                .AddLineSeries(results.Data("px1l2").ToArray, results.Data("py3").ToArray)
                                .Series(2).Title = "Liquid-Liquid (1)"
                                .Series(3).Title = "Liquid-Liquid (2)"
                            End If
                            If results.Data("pxs1").Count > 0 Then
                                .AddLineSeries(results.Data("pxs1").ToArray, results.Data("pys1").ToArray)
                                .Series(.Series.Count - 1).Title = "Solid-Liquid (1)"
                                .AddLineSeries(results.Data("pxs2").ToArray, results.Data("pys2").ToArray)
                                .Series(.Series.Count - 1).Title = "Solid-Liquid (2)"
                            End If
                            If results.Data("pxc").Count > 0 Then
                                .AddLineSeries(results.Data("pxc").ToArray, results.Data("pyc").ToArray)
                                .Series(.Series.Count - 1).Title = "Critical Line"
                            End If
                            .LegendFontSize = 10
                            .LegendPosition = LegendPosition.TopCenter
                            .LegendPlacement = LegendPlacement.Outside
                            .LegendOrientation = LegendOrientation.Horizontal
                            .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                        End With

                        results.PlotModels.Add(model1)

                        results.TextOutput += "Binary Envelope calculation results @ " & MixPressure.ConvertFromSI(Units.pressure).ToString(results.NumberFormat) & " " &
                                            Units.pressure & " for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                        results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                        results.TextOutput += System.Environment.NewLine
                        results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("Bubble Point (" & Units.temperature & ")").PadRight(20) & "Dew Point (" & Units.temperature & ")" & System.Environment.NewLine
                        For i As Integer = 0 To results.Data("px").Count - 1
                            results.TextOutput += results.Data("px")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("py1")(i).ToString(results.NumberFormat).PadRight(20) &
                                results.Data("py2")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                        Next
                        If results.Data("px1l1").Count > 0 Then
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("LL Line 1 (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("px1l1").Count - 1
                                results.TextOutput += results.Data("px1l1")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("py3")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("LL Line 2 (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("px1l2").Count - 1
                                results.TextOutput += results.Data("px1l2")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("py3")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                        End If
                        If results.Data("pxs1").Count > 0 Then
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("SL Line 1 (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("pxs1").Count - 1
                                results.TextOutput += results.Data("pxs1")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("pys1")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("SL Line 2 (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("pxs2").Count - 1
                                results.TextOutput += results.Data("pxs2")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("pys2")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                        End If
                        If results.Data("pxc").Count > 0 Then
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("Critical Line (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("pxc").Count - 1
                                results.TextOutput += results.Data("pxc")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("pyc")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                        End If

                    Case CalculationType.BinaryEnvelopePxy

                        BinaryEnvelopeOptions(0) = "P-x-y"
                        BinaryEnvelopeOptions(1) = MixPressure
                        BinaryEnvelopeOptions(2) = MixTemperature
                        Dim res As Object() = pp.DW_ReturnBinaryEnvelope(BinaryEnvelopeOptions)

                        results.Data.Add("px", DirectCast(res(0), ArrayList).ToDoubleList())
                        results.DataUnits.Add("px", "")
                        results.Data.Add("py1", DirectCast(res(1), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("py1", Units.pressure)
                        results.Data.Add("py2", DirectCast(res(2), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("py2", Units.pressure)
                        results.Data.Add("px1l1", DirectCast(res(3), ArrayList).ToDoubleList())
                        results.DataUnits.Add("px1l1", "")
                        results.Data.Add("px1l2", DirectCast(res(4), ArrayList).ToDoubleList())
                        results.DataUnits.Add("px1l2", "")
                        results.Data.Add("py3", DirectCast(res(5), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("py3", Units.pressure)

                        Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Binary Envelope (Pxy) @ " &
                                            MixTemperature.ConvertFromSI(Units.temperature).ToString(results.NumberFormat) & " " &
                                            Units.temperature, .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                        With model1
                            .TitleFontSize = 14
                            .SubtitleFontSize = 10
                            .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Mole Fraction " & Compounds(0), .FontSize = 12})
                            .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Pressure (" & Units.pressure & ")", .FontSize = 12})
                            .AddLineSeries(results.Data("px").ToArray, results.Data("py1").ToArray)
                            .AddLineSeries(results.Data("px").ToArray, results.Data("py2").ToArray)
                            .Series(0).Title = "Bubble Points"
                            .Series(1).Title = "Dew Points"
                            If results.Data("px1l1").Count > 0 Then
                                .AddLineSeries(results.Data("px1l1").ToArray, results.Data("py3").ToArray)
                                .AddLineSeries(results.Data("px1l2").ToArray, results.Data("py3").ToArray)
                                .Series(2).Title = "Liquid-Liquid (1)"
                                .Series(3).Title = "Liquid-Liquid (2)"
                            End If
                            .LegendFontSize = 10
                            .LegendPosition = LegendPosition.TopCenter
                            .LegendPlacement = LegendPlacement.Outside
                            .LegendOrientation = LegendOrientation.Horizontal
                            .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                        End With

                        results.PlotModels.Add(model1)

                        results.TextOutput += "Binary Envelope calculation results @ " & MixTemperature.ConvertFromSI(Units.temperature).ToString(results.NumberFormat) & " " &
                                        Units.temperature & " for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                        results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                        results.TextOutput += (Compounds(0) & " mole fraction").PadRight(20) & ("Bubble Point (" & Units.pressure & ")").PadRight(20) & "Dew Point (" & Units.pressure & ")" & System.Environment.NewLine
                        For i As Integer = 0 To results.Data("px").Count - 1
                            results.TextOutput += results.Data("px")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("py1")(i).ToString(results.NumberFormat).PadRight(20) &
                                results.Data("py2")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                        Next
                        If results.Data("px1l1").Count > 0 Then
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("LL Line 1 (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("px1l1").Count - 1
                                results.TextOutput += results.Data("px1l1")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("py3")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                            results.TextOutput += System.Environment.NewLine
                            results.TextOutput += (Compounds(0) & " Mole Fraction").PadRight(20) & ("LL Line 2 (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                            For i As Integer = 0 To results.Data("px1l2").Count - 1
                                results.TextOutput += results.Data("px1l2")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("py3")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                            Next
                        End If

                    Case Else

                        Dim res As Object()

                        res = pp.DW_ReturnPhaseEnvelope(PhaseEnvelopeOptions)

                        '{TVB, PB, HB, SB, VB, TVD, PO, HO, SO, VO, TE, PE, TH, PHsI, PHsII, CP, TQ, PQ, TI, PI, TOWF, POWF, HOWF, SOWF, VOWF}</returns>

                        results.Data.Add("TB", DirectCast(res(0), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("TB", Units.temperature)
                        results.Data.Add("PB", DirectCast(res(1), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("PB", Units.pressure)
                        results.Data.Add("HB", DirectCast(res(2), ArrayList).ToDoubleList().ConvertFromSI(Units.enthalpy))
                        results.DataUnits.Add("HB", Units.enthalpy)
                        results.Data.Add("SB", DirectCast(res(3), ArrayList).ToDoubleList().ConvertFromSI(Units.entropy))
                        results.DataUnits.Add("SB", Units.entropy)
                        results.Data.Add("VB", DirectCast(res(4), ArrayList).ToDoubleList().ConvertFromSI(Units.molar_volume))
                        results.DataUnits.Add("VB", Units.molar_volume)

                        results.Data.Add("TD", DirectCast(res(5), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("TD", Units.temperature)
                        results.Data.Add("PD", DirectCast(res(6), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("PD", Units.pressure)
                        results.Data.Add("HD", DirectCast(res(7), ArrayList).ToDoubleList().ConvertFromSI(Units.enthalpy))
                        results.DataUnits.Add("HD", Units.enthalpy)
                        results.Data.Add("SD", DirectCast(res(8), ArrayList).ToDoubleList().ConvertFromSI(Units.entropy))
                        results.DataUnits.Add("SD", Units.entropy)
                        results.Data.Add("VD", DirectCast(res(9), ArrayList).ToDoubleList().ConvertFromSI(Units.molar_volume))
                        results.DataUnits.Add("VD", Units.molar_volume)

                        results.Data.Add("TE", DirectCast(res(10), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("TE", Units.temperature)
                        results.Data.Add("PE", DirectCast(res(11), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("PE", Units.pressure)

                        Dim cpdata As Object = res(15)

                        results.Data.Add("CP", New List(Of Double) From {Convert.ToDouble(cpdata(0)(0).ToString).ConvertFromSI(Units.temperature),
                                                                         Convert.ToDouble(cpdata(0)(1).ToString).ConvertFromSI(Units.pressure),
                                                                         Convert.ToDouble(cpdata(0)(2).ToString).ConvertFromSI(Units.molar_volume)})

                        results.DataUnits.Add("CP", "")

                        results.Data.Add("TQ", DirectCast(res(16), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("TQ", Units.temperature)
                        results.Data.Add("PQ", DirectCast(res(17), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("PQ", Units.pressure)

                        results.Data.Add("TI", DirectCast(res(18), ArrayList).ToDoubleList().ConvertFromSI(Units.temperature))
                        results.DataUnits.Add("TI", Units.temperature)
                        results.Data.Add("PI", DirectCast(res(19), ArrayList).ToDoubleList().ConvertFromSI(Units.pressure))
                        results.DataUnits.Add("PI", Units.pressure)

                        Select Case CalcType

                            Case CalculationType.PhaseEnvelopePT

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Pressure/Temperature diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Temperature (" & Units.temperature & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Pressure (" & Units.pressure & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("TB").ToArray, results.Data("PB").ToArray)
                                    .Series(0).Title = "Bubble Points"
                                    .AddLineSeries(results.Data("TD").ToArray, results.Data("PD").ToArray)
                                    .Series(1).Title = "Dew Points"
                                    .AddScatterSeries(New Double() {results.Data("CP")(0)}, New Double() {results.Data("CP")(1)})
                                    DirectCast(.Series(2), ScatterSeries).MarkerSize = 3
                                    .Series(2).Title = "Critical Point"
                                    If PhaseEnvelopeOptions.PhaseIdentificationCurve Then
                                        .AddLineSeries(results.Data("TI").ToArray, results.Data("PI").ToArray)
                                        .Series(.Series.Count - 1).Title = "Phase Identification Parameter"
                                    End If
                                    If PhaseEnvelopeOptions.QualityLine Then
                                        .AddLineSeries(results.Data("TQ").ToArray, results.Data("PQ").ToArray)
                                        .Series(.Series.Count - 1).Title = "Quality Curve VF = " & PhaseEnvelopeOptions.QualityValue.ToString
                                    End If
                                    If PhaseEnvelopeOptions.StabilityCurve Then
                                        .AddLineSeries(results.Data("TE").ToArray, results.Data("PE").ToArray)
                                        .Series(.Series.Count - 1).Title = "Stability Curve"
                                    End If
                                    If PhaseEnvelopeOptions.OperatingPoint Then
                                        .AddScatterSeries(New Double() {MixTemperature.ConvertFromSI(Units.temperature)}, New Double() {MixPressure.ConvertFromSI(Units.pressure)})
                                        DirectCast(.Series(.Series.Count - 1), ScatterSeries).MarkerSize = 3
                                        .Series(.Series.Count - 1).Title = "Operating Point"
                                    End If

                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += Environment.NewLine
                                results.TextOutput += ("Bubble Temp. (" & Units.temperature & ")").PadRight(20) &
                                    ("Bubble Pressure (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PB").Count - 1
                                    results.TextOutput += results.Data("TB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PB")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                Next
                                results.TextOutput += Environment.NewLine
                                results.TextOutput += ("Dew Temp. (" & Units.temperature & ")").PadRight(20) &
                                    "Dew Pressure (" & Units.pressure & ")" & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PD").Count - 1
                                    results.TextOutput += results.Data("TD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next
                                If PhaseEnvelopeOptions.PhaseIdentificationCurve Then
                                    results.TextOutput += Environment.NewLine
                                    results.TextOutput += ("PIP Temp. (" & Units.temperature & ")").PadRight(20) &
                                    ("PIP Pressure (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                                    For i As Integer = 0 To results.Data("PI").Count - 1
                                        results.TextOutput += results.Data("TI")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PI")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                    Next
                                End If
                                If PhaseEnvelopeOptions.QualityLine Then
                                    results.TextOutput += Environment.NewLine
                                    results.TextOutput += ("Quality Line Temp. (" & Units.temperature & ")").PadRight(20) &
                                    ("Quality Line Pressure (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                                    For i As Integer = 0 To results.Data("PQ").Count - 1
                                        results.TextOutput += results.Data("TQ")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PQ")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                    Next
                                End If
                                If PhaseEnvelopeOptions.StabilityCurve Then
                                    results.TextOutput += Environment.NewLine
                                    results.TextOutput += ("Stability Curve Temp. (" & Units.temperature & ")").PadRight(20) &
                                    ("Stability Curve Pressure (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                                    For i As Integer = 0 To results.Data("PE").Count - 1
                                        results.TextOutput += results.Data("TE")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PE")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                    Next
                                End If

                            Case CalculationType.PhaseEnvelopePH

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Pressure/Enthalpy diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Enthalpy (" & Units.enthalpy & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Pressure (" & Units.pressure & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("HB").ToArray, results.Data("PB").ToArray)
                                    .AddLineSeries(results.Data("HD").ToArray, results.Data("PD").ToArray)
                                    .Series(0).Title = "Bubble Points"
                                    .Series(1).Title = "Dew Points"
                                    If PhaseEnvelopeOptions.OperatingPoint Then
                                        .AddScatterSeries(New Double() {MixEnthalpy.ConvertFromSI(Units.enthalpy)}, New Double() {MixPressure.ConvertFromSI(Units.pressure)})
                                        DirectCast(.Series(.Series.Count - 1), ScatterSeries).MarkerSize = 3
                                        .Series(.Series.Count - 1).Title = "Operating Point"
                                    End If
                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += ("Bubble Enthalpy (" & Units.enthalpy & ")").PadRight(20) &
                                    ("Bubble Press. (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PB").Count - 1
                                    results.TextOutput += results.Data("HB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PB")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                Next
                                results.TextOutput += ("Dew Enthalpy (" & Units.enthalpy & ")").PadRight(20) &
                                    ("Dew Press. (" & Units.pressure & ")") & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PD").Count - 1
                                    results.TextOutput += results.Data("HD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next

                            Case CalculationType.PhaseEnvelopePS

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Pressure/Entropy diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Entropy (" & Units.entropy & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Pressure (" & Units.pressure & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("SB").ToArray, results.Data("PB").ToArray)
                                    .AddLineSeries(results.Data("SD").ToArray, results.Data("PD").ToArray)
                                    .Series(0).Title = "Bubble Points"
                                    .Series(1).Title = "Dew Points"
                                    If PhaseEnvelopeOptions.OperatingPoint Then
                                        .AddScatterSeries(New Double() {MixEntropy.ConvertFromSI(Units.entropy)}, New Double() {MixPressure.ConvertFromSI(Units.pressure)})
                                        DirectCast(.Series(.Series.Count - 1), ScatterSeries).MarkerSize = 3
                                        .Series(.Series.Count - 1).Title = "Operating Point"
                                    End If
                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += ("Bubble Entropy (" & Units.entropy & ")").PadRight(20) &
                                    ("Bubble Press. (" & Units.pressure & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PB").Count - 1
                                    results.TextOutput += results.Data("SB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PB")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                Next
                                results.TextOutput += ("Dew Entropy (" & Units.entropy & ")").PadRight(20) & ("Dew Press. (" & Units.pressure & ")") & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PD").Count - 1
                                    results.TextOutput += results.Data("SD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("PD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next

                            Case CalculationType.PhaseEnvelopeTH

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Temperature/Enthalpy diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Enthalpy (" & Units.enthalpy & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Temperature (" & Units.temperature & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("HB").ToArray, results.Data("TB").ToArray)
                                    .AddLineSeries(results.Data("HD").ToArray, results.Data("TD").ToArray)
                                    .Series(0).Title = "Bubble Points"
                                    .Series(1).Title = "Dew Points"
                                    If PhaseEnvelopeOptions.OperatingPoint Then
                                        .AddScatterSeries(New Double() {MixEnthalpy.ConvertFromSI(Units.enthalpy)}, New Double() {MixTemperature.ConvertFromSI(Units.temperature)})
                                        DirectCast(.Series(.Series.Count - 1), ScatterSeries).MarkerSize = 3
                                        .Series(.Series.Count - 1).Title = "Operating Point"
                                    End If
                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += ("Bubble Enthalpy (" & Units.enthalpy & ")").PadRight(20) &
                                    ("Bubble Temp. (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("TD").Count - 1
                                    results.TextOutput += results.Data("HB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("TD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next
                                results.TextOutput += ("Dew Enthalpy (" & Units.enthalpy & ")").PadRight(20) &
                                   ("Dew Temp. (" & Units.temperature & ")") & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("TD").Count - 1
                                    results.TextOutput += results.Data("HD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("TD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next

                            Case CalculationType.PhaseEnvelopeTS

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Pressure/Entropy diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Entropy (" & Units.entropy & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Temperature (" & Units.temperature & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("SB").ToArray, results.Data("TB").ToArray)
                                    .AddLineSeries(results.Data("SD").ToArray, results.Data("TD").ToArray)
                                    .Series(0).Title = "Bubble Points"
                                    .Series(1).Title = "Dew Points"
                                    If PhaseEnvelopeOptions.OperatingPoint Then
                                        .AddScatterSeries(New Double() {MixEntropy.ConvertFromSI(Units.entropy)}, New Double() {MixTemperature.ConvertFromSI(Units.temperature)})
                                        DirectCast(.Series(.Series.Count - 1), ScatterSeries).MarkerSize = 3
                                        .Series(.Series.Count - 1).Title = "Operating Point"
                                    End If
                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += ("Bubble Entropy (" & Units.entropy & ")").PadRight(20) &
                                    ("Bubble Temp. (" & Units.temperature & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PB").Count - 1
                                    results.TextOutput += results.Data("SB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("TB")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                Next
                                results.TextOutput += ("Dew Entropy (" & Units.entropy & ")").PadRight(20) &
                                 ("Dew Temp. (" & Units.temperature & ")") & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("PD").Count - 1
                                    results.TextOutput += results.Data("SD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("TD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next

                            Case CalculationType.PhaseEnvelopeVT

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Volume/Temperature diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Temperature (" & Units.temperature & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Volume (" & Units.molar_volume & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("TB").ToArray, results.Data("VB").ToArray)
                                    .AddLineSeries(results.Data("TD").ToArray, results.Data("VD").ToArray)
                                    .AddScatterSeries(New Double() {results.Data("CP")(0)}, New Double() {results.Data("CP")(2)})
                                    .Series(0).Title = "Bubble Points"
                                    .Series(1).Title = "Dew Points"
                                    .Series(2).Title = "Critical Point"
                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += ("Bubble Temp. (" & Units.temperature & ")").PadRight(20) &
                                    ("Bubble Volume (" & Units.molar_volume & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("VB").Count - 1
                                    results.TextOutput += results.Data("TB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("VB")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                Next
                                results.TextOutput += ("Dew Temp. (" & Units.temperature & ")").PadRight(20) &
                                 ("Dew Volume (" & Units.molar_volume & ")") & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("VD").Count - 1
                                    results.TextOutput += results.Data("TD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("VD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next

                            Case CalculationType.PhaseEnvelopeVP

                                Dim model1 As New Global.OxyPlot.PlotModel With {.Title = "Volume/Pressure diagram",
                                                                                     .Subtitle = MixName & " " & Compounds.ToArrayString() & " / " & "Model: " & PropertyPackageName}

                                With model1
                                    .TitleFontSize = 14
                                    .SubtitleFontSize = 10
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Bottom, .Title = "Pressure (" & Units.pressure & ")", .FontSize = 12})
                                    .Axes.Add(New LinearAxis() With {.MajorGridlineStyle = LineStyle.Dash, .MinorGridlineStyle = LineStyle.Dot, .Position = AxisPosition.Left, .Title = "Volume (" & Units.molar_volume & ")", .FontSize = 12})
                                    .AddLineSeries(results.Data("PB").ToArray, results.Data("VB").ToArray)
                                    .AddLineSeries(results.Data("PD").ToArray, results.Data("VD").ToArray)
                                    .AddScatterSeries(New Double() {results.Data("CP")(1)}, New Double() {results.Data("CP")(2)})
                                    ' DirectCast(.Series(0), LineSeries).Smooth = True
                                    ' DirectCast(.Series(1), LineSeries).Smooth = True
                                    .Series(0).Title = "Bubble Points"
                                    .Series(1).Title = "Dew Points"
                                    .Series(2).Title = "Critical Point"
                                    .LegendFontSize = 10
                                    .LegendPosition = LegendPosition.TopCenter
                                    .LegendPlacement = LegendPlacement.Outside
                                    .LegendOrientation = LegendOrientation.Horizontal
                                    .TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView
                                End With

                                results.PlotModels.Add(model1)

                                results.TextOutput += "Phase Envelope calculation results for " & MixName & " " & Compounds.ToArrayString & System.Environment.NewLine
                                results.TextOutput += "Property Package: " & PropertyPackageName & System.Environment.NewLine & System.Environment.NewLine
                                results.TextOutput += ("Bubble Press. (" & Units.pressure & ")").PadRight(20) &
                                    ("Bubble Volume (" & Units.molar_volume & ")").PadRight(20) & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("VB").Count - 1
                                    results.TextOutput += results.Data("PB")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("VB")(i).ToString(results.NumberFormat).PadRight(20) & System.Environment.NewLine
                                Next
                                results.TextOutput += ("Dew Press. (" & Units.pressure & ")").PadRight(20) &
                                   ("Dew Volume (" & Units.molar_volume & ")") & System.Environment.NewLine
                                For i As Integer = 0 To results.Data("VD").Count - 1
                                    results.TextOutput += results.Data("PD")(i).ToString(results.NumberFormat).PadRight(20) & results.Data("VD")(i).ToString(results.NumberFormat) & System.Environment.NewLine
                                Next

                        End Select

                End Select

            Catch agex As AggregateException

                results.ExceptionResult = agex.GetBaseException

            Catch ex As Exception

                results.ExceptionResult = ex

            End Try

            Return results

        End Function

    End Class

End Namespace

