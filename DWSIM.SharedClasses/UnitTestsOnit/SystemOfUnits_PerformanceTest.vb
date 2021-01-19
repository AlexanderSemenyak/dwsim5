Imports System.Security.Cryptography.X509Certificates
Imports NUnit.Framework

#If DEBUG

<TestFixture>
Public Class SystemOfUnits_PerformanceTest

    Dim testUnits As string() ={
                "[kg/s]/[Pa^0.5]",
                "[lbm/h]/[psi^0.5]",
                "[kg/h]/[atm^0.5]",
                "[kg/h]/[bar^0.5]",
                "[kg/h]/[[kgf/cm2]^0.5]",
                "K/Pa",
                "C/atm",
                "F/psi",
                "1/kPa",
                "1/MPa",
                "1/psi",
                "1/bar",
                "1/atm",
                "ft3/bbl",
                "g",
                "lb",
                "m/s",
                "cm/s",
                "mm/s",
                "km/h",
                "ft/h",
                "ft/min",
                "ft/s",
                "in/s",
                "kPa",
                "ftH2O",
                "inH2O",
                "inHg",
                "lbf/ft2",
                "mbar",
                "mH2O",
                "mmH2O",
                "mmHg",
                "MPa",
                "psi", "psia",
                "bar",
                "kPag",
                "barg",
                "kgf/cm2g",
                "psig",
                "kg/d",
                "kg/min",
                "lb/h", "lbm/h",
                "lb/min",
                "lb/s",
                "g",
                "lb", "lbm",
                "mol/h",
                "mol/d",
                "kmol/s",
                "kmol/h",
                "kmol/d",
                "kmol/[kg.s]",
                "kmol/[kg.min.]",
                "kmol/[kg.h]",
                "mol/[kg.s]",
                "mol/[kg.min.]",
                "mol/[kg.h]",
                "lbmol/[lbm.h]",
                "bbl/h",
                "bbl/d",
                "ft3/min",
                "ft3/d",
                "ft3/s",
                "gal[UK]/h",
                "gal[UK]/s",
                "gal[US]/h",
                "gal[US]/min",
                "L/h",
                "L/min",
                "L/s",
                "m3/d","BTU/h",
                "BTU/s",
                "cal/s",
                "HP",
                "kcal/h",
                "kJ/h",
                "kJ/d",
                "MW",
                "W",
                "MJ/h",
                "BTU/lb",
                "cal/g",
                "kcal/kg",
                "kJ/kmol", "J/mol",
                "cal/mol",
                "BTU/lbmol",
                "kJ/[kmol.K]",
                "cal/[mol.°C]",
                "cal/[mol.C]",
                "BTU/[lbmol.R]",
                "K.m2/W",
                "C.cm2.s/cal",
                "ft2.h.F/BTU",
                "m/kg",
                "ft/lb", "ft/lbm",
                "cm/g",
                "m-1",
                "ft-1",
                "cm-1",
                "m2","cm2",
                "ft2",
                "h","s",
                "min.",
                "ft3","m3",
                "cm3",
                "L",
                "cm3/mol","m3/kmol",
                "m3/mol",
                "ft3/lbmol",
                "mm","in.", "in",
                "dyn","N",
                "lbf",
                "mol/L","mol/m3",
                "kmol/m3",
                "mol/cm3",
                "mol/mL",
                "lbmol/ft3",
                "g/L","kg/m3",
                "g/cm3",
                "g/mL",
                "m2/s","ft/s2",
                "cm2/s",
                "W/[m2.K]","BTU/[ft2.h.R]",
                "cal/[cm.s.°C]",
                "cal/[cm.s.C]",
                "m3/kg","ft3/lbm",
                "cm3/g",
                "kmol/[m3.s]",
                "kmol/[m3.min.]",
                "kmol/[m3.h]",
                "mol/[m3.s]",
                "mol/[m3.min.]",
                "mol/[m3.h]",
                "mol/[L.s]",
                "mol/[L.min.]",
                "mol/[L.h]",
                "mol/[cm3.s]",
                "mol/[cm3.min.]",
                "mol/[cm3.h]",
                "lbmol/[ft3.h]",
                "°C","C","°C.",
                "C.",
                "atm",
                "g/s",
                "mol/s",
                "kmol/s",
                "cal/g",
                "g/cm3",
                "dyn/cm",
                "dyn/cm2",
                "cal/[cm.s.°C]",
                "cal/[cm.s.C]",
                "cm3/s",
                "cal/[g.°C]",
                "cal/[g.C]",
                "cSt",
                "mm2/s",
                "Pa.s",
                "cP",
                "lbm /[ft.h]",
                "kcal/h",
                "m",
                "R",
                "R.",
                "lbf/ft2",
                "lbm/h",
                "lbmol/h",
                "BTU/lbm",
                "lbm/ft3",
                "lbf/in",
                "BTU/[ft.h.R]",
                "ft3/s",
                "BTU/[lbm.R]",
                "ft2/s",
                "lbm/[ft.s]",
                "BTU/h",
                "ft",
                "kgf/cm2_a",
                "kgf/cm2",
                "kgf/cm2_g",
                "kg/h",
                "kg/d",
                "m3/h",
                "m3/d",
                "m3/d @ BR", "m3/d @ 20 C, 1 atm",
                "m3/d @ CNTP",
                "m3/d @ NC", "m3/d @ 0 C, 1 atm",
                "m3/d @ SC", "m3/d @ 15.56 C, 1 atm",
                "ft3/d @ 60 F, 14.7 psia",
                "ft3/d @ 0 C, 1 atm",
                "°F",
                "°F.",
                "F",
                "F.",
                "cm",
                "cal/[mol.°C]",
                "cal/[mol.C]",
                "BTU/[lbmol.R]",
                "lb/d",
                "Mg/s",
                "Mg/h",
                "Mg/d",
                "Mm3/d @ BR",
                "Mm3/d @ NC",
                "Mm3/d @ SC",
                "MMSCFD",
                "SCFD",
                "SCFM",
                "MMBTU/h",
                "BTU/d",
                "MMBTU/d"
                               }


    ''' <summary>
    ''' пример результатов не сильно впечатлил - выигрыша особого нет - в 2-3 раза:
    ''' get MMBTU/d original:627
    ''' get MMBTU/d dict string:206
    ''' get MMBTU/d dict hash:195
    ''' </summary>
    <Test>
    public sub ConvertFromSI_Switch_vs_Dictionary()

        Dim dict = new Dictionary(Of String, Func(Of Double, Double))(StringComparer.Ordinal)
        Dim dict2 = new Dictionary(Of Int32, Func(Of Double, Double))

        Dim testUom = "MMBTU/d"

        Console.WriteLine("Duplicate cases in SystemsOfUnits.Converter.ConvertFromSI:")
        For Each unit As String In testUnits
            try
                If (unit =testUom)
                    dict.Add(unit,AddressOf M3_Function)
                else
                    dict.Add(unit,AddressOf NullFunction)
                End If
            Catch ex As Exception
                Console.WriteLine(unit)
            End Try
        Next

        For Each unit As String In testUnits
            try
                If (unit =testUom)
                    dict2.Add(unit.GetHashCode(),AddressOf M3_Function)
                else
                    dict2.Add(unit.GetHashCode(),AddressOf NullFunction)
                End If
            Catch ex As Exception
                Console.Write("Duplicate by hash:"+unit)
            End Try
        Next


        Dim testCount = 10000000

        Dim sw = new StopWatch()

        sw.Start()
        'Test get m3 original
        Dim m3 as Double
        For i As Integer = 0 To testCount
            m3 = SystemsOfUnits.Converter.ConvertFromSI(testUom, 0)
        Next
        sw.Stop()
        Console.WriteLine($"Test get {testUom} original:" + sw.ElapsedMilliseconds.ToString())

        'Test get m3 dict string
        sw.Restart()
        Dim f as Func(Of Double, Double)
        For i As Integer = 0 To testCount
            if dict.TryGetValue(testUom, f) then 
                m3 = f(0)
            end if
        Next
        sw.Stop()
        Console.WriteLine($"Test get {testUom} dict string:" + sw.ElapsedMilliseconds.ToString())

        'Test get m3 dict hash
        sw.Restart()
        For i As Integer = 0 To testCount
            if dict2.TryGetValue(testUom.GetHashCode(), f) then 
              m3 = f(0)
            end if
        Next
        sw.Stop()
        Console.WriteLine($"Test get {testUom} dict hash:" + sw.ElapsedMilliseconds.ToString())

    End sub

    Private Shared Function NullFunction(uom As double) As Double
        
    End Function

    Private Shared Function M3_Function(forConvert As double) As Double
        Return forConvert
    End Function


End Class

#End if