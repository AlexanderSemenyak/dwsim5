﻿'    DWSIM Nested Loops Global Flash Algorithm (SVLLE)
'    Copyright 2018 Daniel Wagner O. de Medeiros
'
'    This file is part of DWSIM.
'
'    DWSIM is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    DWSIM is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with DWSIM.  If not, see <http://www.gnu.org/licenses/>.

Imports System.Math

Namespace PropertyPackages.Auxiliary.FlashAlgorithms

    Public Class NestedLoopsSVLLE

        Inherits FlashAlgorithm

        Dim nl1 As New NestedLoops
        Dim nl2 As New NestedLoops3PV3
        Dim nl3 As New NestedLoopsSLE With {.SolidSolution = False}

        Public Sub New()
            MyBase.New
            Order = 0
        End Sub
        Public Sub ClearEstimates()
            nl2?.ClearEstimates()
        End Sub

        Public Overrides ReadOnly Property AlgoType As Interfaces.Enums.FlashMethod
            Get
                Return Interfaces.Enums.FlashMethod.Nested_Loops_SVLLE
            End Get
        End Property

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Global Equilibrium Flash Algorithm, can calculate equilibria between one solid, one vapor and two liquid phases (SVLLE)."
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Nested Loops SVLLE"
            End Get
        End Property

        Public Overrides ReadOnly Property MobileCompatible As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Function Flash_PT(Vz() As Double, P As Double, T As Double, PP As PropertyPackage, Optional ReuseKI As Boolean = False, Optional PrevKi() As Double = Nothing) As Object

            nl1.FlashSettings = FlashSettings
            nl2.FlashSettings = FlashSettings

            Dim d1, d2 As Date, dt As TimeSpan

            d1 = Date.Now

            Dim IObj As Inspector.InspectorItem = Inspector.Host.GetNewInspectorItem()

            Inspector.Host.CheckAndAdd(IObj, "", "Flash_PT", Name & " (PT Flash)", "Pressure-Temperature Flash Algorithm Routine", True)

            IObj?.Paragraphs.Add(String.Format("<h2>Input Parameters</h2>"))

            IObj?.Paragraphs.Add(String.Format("Temperature: {0} K", T))
            IObj?.Paragraphs.Add(String.Format("Pressure: {0} Pa", P))
            IObj?.Paragraphs.Add(String.Format("Compounds: {0}", PP.RET_VNAMES.ToMathArrayString))
            IObj?.Paragraphs.Add(String.Format("Mole Fractions: {0}", Vz.ToMathArrayString))

            Dim V, L1, L2, S, Vy(), Vx1(), Vx2(), Vs() As Double

            Dim result As Object

            Dim names = PP.RET_VNAMES

            Dim n = Vz.Length - 1

            If PP.ForcedSolids.Count > 0 Then

                'we have forced solids

                Dim Vzns As Double() = Vz.Clone
                Vs = PP.RET_NullVector
                For Each item In PP.ForcedSolids
                    Dim index = names.ToList.IndexOf(item)
                    Vs(index) = Vz(index)
                    Vzns(index) = 0.0
                Next
                S = Vs.Sum
                Vzns = Vzns.NormalizeY
                Vs = Vs.NormalizeY

                IObj?.SetCurrent

                result = nl1.Flash_PT(Vzns, P, T, PP)

                L1 = result(0) * (1 - S)
                V = result(1) * (1 - S)
                Vx1 = result(2)
                Vy = result(3)
                L2 = result(5) * (1 - S)
                Vx2 = result(6)

            Else

                Dim Vz2 = Vz.Clone
                Vs = PP.RET_NullVector

                Dim VTfus As Double() = PP.RET_VTF

                For i = 0 To n
                    If Vz(i) > 0.0 And T < VTfus(i) Then
                        Vs(i) = Vz(i)
                        Vz2(i) = 0.0
                    End If
                Next

                S = Vs.Sum

                IObj?.SetCurrent

                result = nl1.Flash_PT(Vz2, P, T, PP)

                L1 = result(0) * (1 - S)
                V = result(1) * (1 - S)
                Vx1 = result(2)
                Vy = result(3)
                L2 = result(5) * (1 - S)
                Vx2 = result(6)

            End If

            Dim GoneThrough As Boolean = False

            If L1 = 0 And S = 0.0 And (FlashSettings(Interfaces.Enums.FlashSetting.CheckIncipientLiquidForStability)) Then

                Dim stresult As Object = StabTest(T, P, Vx1, PP.RET_VTC, PP)

                If stresult(0) = False Then

                    Dim nlflash As New NestedLoops()

                    Dim m As Double = UBound(stresult(1), 1)

                    Dim trialcomps As New List(Of Double())
                    Dim results As New List(Of Object)

                    For j = 0 To m
                        Dim vxtrial(n) As Double
                        For i = 0 To n
                            vxtrial(i) = stresult(1)(j, i)
                        Next
                        trialcomps.Add(vxtrial)
                    Next

                    For Each tcomp In trialcomps
                        Try
                            Dim r2 = nlflash.Flash_PT(Vz, P, T, PP, True, Vy.DivideY(tcomp))
                            results.Add(r2)
                        Catch ex As Exception
                        End Try
                    Next

                    If results.Where(Function(r) r(0) > 0.0).Count > 0 Then

                        Dim validresult = results.Where(Function(r) r(0) > 0.0).First

                        L1 = validresult(0)
                        V = validresult(1)
                        Vx1 = validresult(2)
                        Vy = validresult(3)

                        result = New Object() {L1, V, Vx1, Vy, 0, 0.0#, PP.RET_NullVector, 0.0#, PP.RET_NullVector}

                        GoneThrough = True

                    End If

                End If

            End If

            If L1 > 0 Then

                Dim stresult = StabTest(T, P, Vx1, PP.RET_VTC, PP)

                IObj?.SetCurrent

                Dim nonsolids = Vz.Count - PP.ForcedSolids.Count

                If stresult(0) = False And Not GoneThrough And nonsolids > 1 Then

                    ' liquid phase NOT stable. proceed to three-phase flash.

                    Dim k As Integer = 0

                    Dim vx2est(n), fcl(n), fcv(n) As Double
                    Dim m As Double = UBound(stresult(1), 1)
                    Dim gl, gli As Double

                    gli = 0
                    For j = 0 To m
                        For i = 0 To n
                            vx2est(i) = stresult(1)(j, i)
                        Next
                        IObj?.SetCurrent
                        fcl = PP.DW_CalcFugCoeff(vx2est, T, P, State.Liquid)
                        gl = 0.0#
                        For i = 0 To n
                            If vx2est(i) <> 0.0# Then gl += vx2est(i) * Log(fcl(i) * vx2est(i))
                        Next
                        If gl <= gli Then
                            gli = gl
                            k = j
                        End If
                    Next
                    For i = 0 To Vz.Length - 1
                        vx2est(i) = stresult(1)(k, i)
                    Next

                    Dim vx1e(Vz.Length - 1), vx2e(Vz.Length - 1) As Double

                    Dim maxl As Double = MathEx.Common.Max(vx2est)
                    Dim imaxl As Integer = Array.IndexOf(vx2est, maxl)

                    V = result(1)
                    L2 = result(3)(imaxl)
                    L1 = 1 - L2 - V

                    If L1 < 0.0# Then
                        L1 = Abs(L1)
                        L2 = 1 - L1 - V
                    End If

                    If L2 < 0.0# Then
                        V += L2
                        L2 = Abs(L2)
                    End If

                    For i = 0 To n
                        If i <> imaxl Then
                            vx1e(i) = Vz(i) - V * result(3)(i) - L2 * vx2est(i)
                        Else
                            vx1e(i) = Vz(i) * L2
                        End If
                    Next

                    Dim sumvx2 As Double
                    For i = 0 To n
                        sumvx2 += Abs(vx1e(i))
                    Next

                    For i = 0 To n
                        vx1e(i) = Abs(vx1e(i)) / sumvx2
                    Next

                    result = nl2.Flash_PT_3P(Vz, V, L1, L2, Vy, vx1e, vx2est, P, T, PP)

                    IObj?.SetCurrent

                    L1 = result(0)
                    V = result(1)
                    Vx1 = result(2)
                    Vy = result(3)
                    L2 = result(5)
                    Vx2 = result(6)

                    If L1 = 0.0 And L2 > 0.0 Then
                        L1 = L2
                        L2 = 0.0
                        Vx1 = Vx2
                        Vx2 = PP.RET_NullVector
                    End If

                End If

                IObj?.SetCurrent

                If PP.RET_VTF.SumY > 0.0 And S = 0 Then

                    result = nl3.Flash_SL(Vx1, P, T, PP)

                    IObj?.SetCurrent

                    'Return New Object() {L, 1 - L, 0.0#, Vx, Vs, L - L_old, ecount, d2 - d1}

                    S = result(1) * L1
                    L1 = result(0) * L1

                    Vx1 = result(3)
                    Vs = result(4)

                End If

                If L2 > 0 Then

                    If PP.RET_VTF.SumY > 0.0 OrElse PP.ForcedSolids.Count > 0 Then

                        result = nl3.Flash_SL(Vx2, P, T, PP)

                        IObj?.SetCurrent

                        'Return New Object() {L, 1 - L, 0.0#, Vx, Vs, L - L_old, ecount, d2 - d1}

                        Vx2 = result(3)
                        Vs = Vs.MultiplyConstY(S).AddY(DirectCast(result(4), Double()).MultiplyConstY(result(1))).NormalizeY()

                        S = S + result(1) * L2
                        L2 = result(0) * L2

                    End If

                End If

            ElseIf S = 0 Then

                IObj?.SetCurrent

                'Return New Object() {L, V, Vx, Vy, ecount, 0.0#, PP.RET_NullVector, 0.0#, PP.RET_NullVector}

                result = nl3.Flash_PT(Vz, P, T, PP)

                IObj?.SetCurrent

                L1 = result(0)
                V = result(1)
                Vx1 = result(2)
                Vy = result(3)
                Vs = result(8)
                S = result(7)

            End If

            d2 = Date.Now

            dt = d2 - d1

            IObj?.Paragraphs.Add("PT Flash [NL-SVLLE]: Converged successfully. Time taken: " & dt.TotalMilliseconds & " ms")

            IObj?.Close()

            Return New Object() {L1, V, Vx1, Vy, 0, L2, Vx2, S, Vs}

        End Function

        Public Overrides Function Flash_PH(ByVal Vz As Double(), ByVal P As Double, ByVal H As Double, ByVal Tref As Double, ByVal PP As PropertyPackages.PropertyPackage, Optional ByVal ReuseKI As Boolean = False, Optional ByVal PrevKi As Double() = Nothing) As Object

            Dim nl = New NestedLoops
            nl.FlashSettings = FlashSettings
            nl.PTFlashFunction = AddressOf Flash_PT

            Return nl.Flash_PH(Vz, P, H, Tref, PP, ReuseKI, PrevKi)

        End Function

        Public Overrides Function Flash_PS(ByVal Vz As Double(), ByVal P As Double, ByVal S As Double, ByVal Tref As Double, ByVal PP As PropertyPackages.PropertyPackage, Optional ByVal ReuseKI As Boolean = False, Optional ByVal PrevKi As Double() = Nothing) As Object

            Dim nl = New NestedLoops
            nl.FlashSettings = FlashSettings
            nl.PTFlashFunction = AddressOf Flash_PT

            Return nl.Flash_PS(Vz, P, S, Tref, PP, ReuseKI, PrevKi)

        End Function

        Public Overrides Function Flash_PV(Vz() As Double, P As Double, V As Double, Tref As Double, PP As PropertyPackage, Optional ReuseKI As Boolean = False, Optional PrevKi() As Double = Nothing) As Object

            If PP.ForcedSolids.Count > 0 Then

                'we have forced solids

                PP.Flowsheet?.ShowMessage("Warning: when compounds are marked as forced solids, partial or full vaporization calculations are done solids-free. Specified and calculated vapor fractions won't match.", Interfaces.IFlowsheet.MessageType.Warning)

                Dim names = PP.RET_VNAMES
                Dim Vs = PP.RET_NullVector
                Dim Vzns As Double() = Vz.Clone
                Dim S As Double = 0.0
                For Each item In PP.ForcedSolids
                    Dim index = names.ToList.IndexOf(item)
                    Vs(index) = Vz(index)
                    Vzns(index) = 0.0
                Next
                S = Vs.Sum
                Vs = Vs.NormalizeY
                Vzns = Vzns.NormalizeY

                'Return New Object() {L, V, Vx, Vy, T, ecount, Ki, 0.0#, PP.RET_NullVector, 0.0#, PP.RET_NullVector}

                Dim result = nl2.Flash_PV(Vzns, P, V, Tref, PP, ReuseKI, PrevKi)

                Dim T, L1, L2, Vx(), Vx2(), Vy() As Double

                L1 = result(0)
                Vx = result(2)
                Vy = result(3)
                T = result(4)
                L2 = result(7)
                Vx2 = result(8)

                Return New Object() {L1 * (1 - S), V * (1 - S), Vx, Vy, T, result(5), result(6), L2 * (1 - S), Vx2, S, Vs}

            Else

                Return nl2.Flash_PV(Vz, P, V, Tref, PP, ReuseKI, PrevKi)

            End If

        End Function

        Public Overrides Function Flash_TV(Vz() As Double, T As Double, V As Double, Pref As Double, PP As PropertyPackage, Optional ReuseKI As Boolean = False, Optional PrevKi() As Double = Nothing) As Object
            Return nl2.Flash_TV(Vz, T, V, Pref, PP, ReuseKI, PrevKi)
        End Function

    End Class

End Namespace


