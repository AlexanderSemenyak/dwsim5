Imports System.Reflection
Imports System.Runtime.Serialization
Imports DWSIM.SharedClasses.SystemsOfUnits

Public NotInheritable Class VersionDeserializationBinder

    Inherits SerializationBinder

    Public Overrides Function BindToType(assemblyName As String, typeName As String) As Type
        If Not String.IsNullOrEmpty(assemblyName) AndAlso Not String.IsNullOrEmpty(typeName) Then
            Dim typeToDeserialize As Type = Nothing
            If assemblyName.Contains("DWSIM,") Then
                assemblyName = Assembly.GetExecutingAssembly().FullName
                typeToDeserialize = Type.[GetType]([String].Format("{0}, {1}", typeName, assemblyName))
            Else
                typeToDeserialize = Type.[GetType]([String].Format("{0}, {1}", typeName, assemblyName))
            End If
            Return typeToDeserialize
        End If
        Return Nothing
    End Function

End Class

''' <summary>
''' Наш класс для десериализации единиц измерения
''' </summary>
Public Class UnitsVersionBinder
    Inherits SerializationBinder

    Shared binder As Dictionary(Of String, Type) = New Dictionary(Of String, Type)()
    Public Shared Instance As UnitsVersionBinder = New UnitsVersionBinder()

    Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As Type
        assemblyName = GetType(Units).Assembly.FullName
        Dim key = typeName & "," & assemblyName
        Dim type = Nothing

        If binder.TryGetValue(key, type) Then
            Return type
        End If

        Dim t = System.Type.[GetType]([String].Format("{0}, {1}", typeName, assemblyName))
        If t Is Nothing Then Return Nothing
        binder(key) = t
        Return t
    End Function
End Class
