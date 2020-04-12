Imports System.Runtime.CompilerServices
Imports System.Runtime.Remoting.Messaging

Public Class OnitUtilities
    Shared emptyElements As XElement() = new XElement(){}

    Public Shared Function GetFilteredXElements(data As IEnumerable(Of XElement), elementName As string) As XElement()

        Dim xel_d As List(Of XElement)
        For Each xel2 As XElement In data
            if String.Equals(xel2.Name.LocalName, elementName, StringComparison.Ordinal) then 
                if (xel_d is Nothing) Then xel_d = New List(Of XElement)()
                xel_d.Add(xel2)
                'Yield xel2
            End If
        Next

        If xel_d is Nothing  Then Return emptyElements
        Return xel_d.ToArray()
    End Function
    
    Public Shared Iterator Function GetFilteredXElementsEnumerable(data As IEnumerable(Of XElement), elementName As string) As IEnumerable(Of XElement)

'        Dim xel_d As List(Of XElement)
    Try
        For Each xel2 As XElement In data
            if String.Equals(xel2.Name.LocalName, elementName, StringComparison.Ordinal) then 
                'if (xel_d is Nothing) Then xel_d = New List(Of XElement)()
                'xel_d.Add(xel2)
                Yield xel2
            End If
        Next
    Catch ex As Exception
        Console.WriteLine(ex)
    End Try
        'If xel_d is Nothing  Then Return emptyElements
        'Return xel_d.ToArray()
    End Function
End Class
