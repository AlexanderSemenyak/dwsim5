'DWSIM Custom XML Serializer
'Copyright 2012-2014 Daniel Wagner O. de Medeiros
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

Imports System.Collections.Concurrent
Imports System.Globalization
Imports System.Reflection
Imports System.Drawing
Imports DWSIM.Interfaces
Imports FastMember

Public Class XMLSerializer

    'Shared cacheOfFastMemberAccessors As ConcurrentDictionary(Of Type,FastMember.TypeAccessor()) = new ConcurrentDictionary(Of Type,PropertyInfo())

    Shared cacheOfTypeProperties As ConcurrentDictionary(Of Type,PropertyInfo()) = new ConcurrentDictionary(Of Type,PropertyInfo())
    Shared cacheOfNameProperties As ConcurrentDictionary(Of Type,ConcurrentDictionary(Of String,PropertyInfo)) = new ConcurrentDictionary(Of Type,ConcurrentDictionary(Of String,PropertyInfo))

    Shared cacheOfTypeFields As ConcurrentDictionary(Of Type,FieldInfo()) = new ConcurrentDictionary(Of Type,FieldInfo())
    Shared cacheOfNameFields As ConcurrentDictionary(Of Type,ConcurrentDictionary(Of String,FieldInfo)) = new ConcurrentDictionary(Of Type,ConcurrentDictionary(Of String,FieldInfo))

    ''' <summary>
    ''' alexander - cache of FieldInfo for type
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns>not null</returns>
    Shared function GetFieldsForType(t As Type) As FieldInfo()

        Dim result As FieldInfo() = Nothing
        if cacheOfTypeFields.TryGetValue(t, result) Then Return result

        result = t.GetFields()
        cacheOfTypeFields.TryAdd(t, result)
        Return result
    End function

    ''' <summary>
    ''' alexander -> Get FieldInfo from cache
    ''' </summary>
    ''' <param name="t"></param>
    ''' <param name="fieldName"></param>
    ''' <returns>Can be null</returns>
    Shared function GetFieldInfoForFieldName(t As Type, fieldName As string) As FieldInfo
        Dim subCache as ConcurrentDictionary(Of String,FieldInfo)= Nothing
        If Not cacheOfNameFields.TryGetValue(t, subCache) Then
            subCache = new ConcurrentDictionary(Of String,FieldInfo)
            cacheOfNameFields.TryAdd(t, subCache)
        End If

        Dim result As FieldInfo = Nothing
        If subCache.TryGetValue(fieldName, result) then Return result

        Dim fields = GetFieldsForType(t)
        result = fields.FirstOrDefault(Function(x) x.Name = fieldName)

        subCache.TryAdd(fieldName, result)
        Return result
    End function
    
    ''' <summary>
    ''' alexander - cache of PropertyInfo for type
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns>not null</returns>
    Shared function GetPropertiesForType(t As Type) As PropertyInfo()
        Dim result As PropertyInfo() = Nothing
        if cacheOfTypeProperties.TryGetValue(t, result) Then Return result

        result = t.GetProperties()
        cacheOfTypeProperties.TryAdd(t, result)
        Return result
    End function

    ''' <summary>
    ''' alexander -> Get PropertyInfo from cache
    ''' </summary>
    ''' <param name="t"></param>
    ''' <param name="propertyName"></param>
    ''' <returns>Can be null</returns>
    Shared function GetPropertyInfoForPropertyName(t As Type, propertyName As string) As PropertyInfo
        Dim subCache as ConcurrentDictionary(Of String,PropertyInfo)= Nothing
        If Not cacheOfNameProperties.TryGetValue(t, subCache) Then
            subCache = new ConcurrentDictionary(Of String,PropertyInfo)
            cacheOfNameProperties.TryAdd(t, subCache)
        End If

        Dim result As PropertyInfo = Nothing
        If subCache.TryGetValue(propertyName, result) then Return result

        Dim properties = GetPropertiesForType(t)
        result = properties.FirstOrDefault(Function(x) x.Name = propertyName)

        subCache.TryAdd(propertyName, result)
        Return result
    End function


    ''' <summary>
    ''' Deserializes selected properties of an object from XML.
    ''' </summary>
    ''' <param name="obj">The object to be updated with properties from the XML elements.</param>
    ''' <param name="xmlprops">The list of XML elements to deserialize from.</param>
    ''' <returns>True if successful.</returns>
    ''' <remarks> Properties of type Boolean, String, Double, Single, Integer, Nullable(Of Double), 
    ''' Nullable(Of Single), Nullable(Of Integer), ArrayList, Font, Color, [Enum]
    ''' are supported.</remarks>
    Shared Function Deserialize(obj As Object, xmlprops As IEnumerable(Of XElement), Optional ByVal Fields As Boolean = False) As Boolean

        Dim ci As CultureInfo = CultureInfo.InvariantCulture
        Dim skip As Boolean = False
        Dim objType  = obj.GetType
        Dim typeAccessor = FastMember.TypeAccessor.Create(objType)
        If Not Fields Then
            'alexander Dim props As PropertyInfo() = obj.GetType.GetProperties()
            Dim props As PropertyInfo() = GetPropertiesForType(objType)
            For Each prop As PropertyInfo In props
                skip = False
                If prop.CanWrite And prop.CanRead Then
                    Dim propname As String = prop.Name
                    'alexander Dim properties = obj.GetType().GetProperties().Where(Function(p) p.Name = prop.Name)
                    'Dim properties = props.Where(Function(p) p.Name = prop.Name)
                    'Dim attributes As Object() = properties(0).GetCustomAttributes(True)

                    prop = GetPropertyInfoForPropertyName(objType,propname) 'alexander do not modify
                    Dim attributes As Object() = prop.GetCustomAttributes(True)
                    For Each attr As Attribute In attributes
                        If TypeOf attr Is System.Xml.Serialization.XmlIgnoreAttribute Then
                            skip = True
                            Exit For
                        End If
                    Next
                    If Not skip Then
                        'alexander speedup
                        'Dim propertyInfo  = obj.GetType.GetProperty(prop.Name)
                        Dim propertyInfo  = GetPropertyInfoForPropertyName(objType, prop.Name)
                        If propertyInfo IsNot Nothing Then
                            'alexander speedup
                            'Dim propertyValue  =  propertyInfo.GetValue(obj, Nothing)
                            Dim propertyValue  =  typeAccessor(obj,propname)

                            If TypeOf propertyValue Is ICustomXMLSerialization Then
                                'alexander Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val = xel.Descendants.ToArray()'.ToList()
                                    DirectCast(propertyValue, ICustomXMLSerialization).LoadData(val)
                                End If
                            ElseIf TypeOf propertyValue Is Single Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                Dim val As Single = Single.Parse(xel.Value, ci)
                                typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                            ElseIf TypeOf propertyValue Is Double Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Double = Double.Parse(xel.Value, ci)
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf propertyValue Is Nothing Then
                                'nullable type
                                If prop.PropertyType.FullName.Contains("Nullable") Then
                                    'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                    Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                    If prop.PropertyType.FullName.Contains("Double") Then
                                        If Not xel Is Nothing Then
                                            Dim val As Nullable(Of Double)
                                            If xel.Value <> "" Then val = Double.Parse(xel.Value, ci)
                                            If Not val Is Nothing Then typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                        End If
                                    ElseIf prop.PropertyType.FullName.Contains("Integer") Then
                                        If Not xel Is Nothing Then
                                            Dim val As Nullable(Of Integer)
                                            If xel.Value <> "" Then val = Integer.Parse(xel.Value, ci)
                                            If Not val Is Nothing Then typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                        End If
                                    End If
                                End If
                            ElseIf TypeOf propertyValue Is Integer Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Integer = xel.Value
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is Boolean Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Boolean = xel.Value
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is String Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Object = xel.Value
                                    If Not val Is Nothing Then typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is [Enum] Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As String = xel.Value
                                    If Not val Is Nothing Then typeAccessor(obj,propname) = [Enum].Parse(propertyValue.GetType, val) ' propertyInfo.SetValue(obj, [Enum].Parse(propertyValue.GetType, val), Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is Font Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                Try
                                    Dim val As Font = New FontConverter().ConvertFromInvariantString(xel.Value)
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                Catch ex As Exception
                                    propertyInfo.SetValue(obj, New Font("Arial", 8), Nothing)
                                End Try
                            ElseIf TypeOf propertyValue Is Color Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Color = ColorTranslator.FromHtml(xel.Value)
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is SkiaSharp.SKColor Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As SkiaSharp.SKColor = SkiaSharp.SKColors.Black
                                    If SkiaSharp.SKColor.TryParse(xel.Value, val) Then
                                        val = SkiaSharp.SKColor.Parse(xel.Value)
                                    Else
                                        Dim val2 As Color
                                        Try
                                            val2 = Color.FromName(xel.Value)
                                            val = New SkiaSharp.SKColor(val2.R, val2.G, val2.B, val2.A)
                                        Catch ex As Exception
                                        End Try
                                    End If
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is ArrayList Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                Dim val As ArrayList = StringToArray(xel.Value, ci)
                                If Not val Is Nothing Then typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                            ElseIf TypeOf propertyValue Is Byte Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Byte = xel.Value
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is Date Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As Date = Date.Parse(xel.Value, CultureInfo.InvariantCulture)
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is TimeSpan Then
                                Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As TimeSpan = TimeSpan.FromMilliseconds(xel.Value)
                                    obj.GetType.GetProperty(prop.Name).SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is OxyPlot.OxyColor Then
                                'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                If Not xel Is Nothing Then
                                    Dim val As OxyPlot.OxyColor = OxyPlot.OxyColor.Parse(xel.Value)
                                    typeAccessor(obj,propname) = val ' propertyInfo.SetValue(obj, val, Nothing)
                                End If
                            ElseIf TypeOf propertyValue Is List(Of String) Then
                                Try
                                    'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                    Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                    If Not xel Is Nothing Then
                                        Dim list As New List(Of String)
                                        For Each el In xel.Elements
                                            list.Add(el.Value)
                                        Next
                                        typeAccessor(obj,propname) = list ' propertyInfo.SetValue(obj, list, Nothing)
                                    End If
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf propertyValue Is List(Of Double) Then
                                Try
                                    'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                    Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                    If Not xel Is Nothing Then
                                        Dim list As New List(Of Double)
                                        For Each el In xel.Elements
                                            list.Add(Double.Parse(el.Value, ci))
                                        Next
                                        typeAccessor(obj,propname) = list ' propertyInfo.SetValue(obj, list, Nothing)
                                    End If
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf propertyValue Is List(Of Single) Then
                                Try
                                    Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                    If Not xel Is Nothing Then
                                        Dim list As New List(Of Single)
                                        For Each el In xel.Elements
                                            list.Add(Single.Parse(el.Value, ci))
                                        Next
                                        obj.GetType.GetProperty(prop.Name).SetValue(obj, list, Nothing)
                                    End If
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf propertyValue Is Dictionary(Of String, String) Then
                                Try
                                    'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                    Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                    If Not xel Is Nothing Then
                                        Dim list As New Dictionary(Of String, String)
                                        For Each el In xel.Elements
                                            list.Add(el.Attribute("Key"), el.Attribute("Value"))
                                        Next
                                        typeAccessor(obj,propname) = list ' propertyInfo.SetValue(obj, list, Nothing)
                                    End If
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf propertyValue Is Dictionary(Of String, Double) Then
                                Try
                                    'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                                    Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                                    If Not xel Is Nothing Then
                                        Dim list As New Dictionary(Of String, Double)
                                        For Each el In xel.Elements
                                            list.Add(el.Attribute("Key"), Double.Parse(el.Attribute("Value"), ci))
                                        Next
                                        typeAccessor(obj,propname) = list ' propertyInfo.SetValue(obj, list, Nothing)
                                    End If
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                    End If
                End If
            Next
        Else
            'Dim props As FieldInfo() = obj.GetType.GetFields()
            Dim props As FieldInfo() = GetFieldsForType(objType)
            For Each prop As FieldInfo In props
                skip = False
                Dim propname As String = prop.Name
                'Dim properties = obj.GetType().GetFields().Where(Function(p) p.Name = prop.Name)
                'Dim attributes As Object() = properties(0).GetCustomAttributes(True)

                prop = GetFieldInfoForFieldName(objType,propname) 'alexander do not modify
                Dim attributes As Object() = prop.GetCustomAttributes(True)

                For Each attr As Attribute In attributes
                    If TypeOf attr Is System.Xml.Serialization.XmlIgnoreAttribute Then
                        skip = True
                        Exit For
                    End If
                Next

                If Not skip Then
                    'alexander 2Dim fieldInfo = objType.GetField(prop.Name)
                    Dim fieldInfo = prop'objType.GetField(prop.Name)
                    If fieldInfo IsNot Nothing Then
                        'Dim fieldValue  = fieldInfo.GetValue(obj)
                        Dim fieldValue = typeAccessor(obj,propname)  
                        If TypeOf fieldValue Is ICustomXMLSerialization Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val = xel.Descendants.ToArray()'.ToList()
                                DirectCast(fieldValue, ICustomXMLSerialization).LoadData(val)
                            End If
                        ElseIf TypeOf fieldValue Is Single Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            Dim val As Single = Single.Parse(xel.Value, ci)
                            typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                        ElseIf TypeOf fieldValue Is Double Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As Double = Double.Parse(xel.Value, ci)
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf fieldValue Is Nothing Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As Nullable(Of Double)
                                If xel.Value <> "" Then val = Double.Parse(xel.Value, ci)
                                If Not val Is Nothing Then typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf fieldValue Is Integer Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As Integer = xel.Value
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf fieldValue Is Boolean Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As Boolean = xel.Value
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf fieldValue Is String Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As String = xel.Value
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf fieldValue Is [Enum] Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As String = xel.Value
                                If Not val Is Nothing Then typeAccessor(obj,propname) = [Enum].Parse(fieldValue.GetType, val) ' fieldInfo.SetValue(obj, [Enum].Parse(fieldValue.GetType, val))
                            End If
                        ElseIf TypeOf fieldValue Is Font Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Try
                                    Dim val As Font = New FontConverter().ConvertFromInvariantString(xel.Value)
                                    typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                                Catch ex As Exception
                                    typeAccessor(obj,propname) = New Font("Arial", 8) ' fieldInfo.SetValue(obj, New Font("Arial", 8))
                                End Try
                            End If
                        ElseIf TypeOf fieldValue Is Color Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            Dim val As Color = ColorTranslator.FromHtml(xel.Value)
                            typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                        ElseIf TypeOf fieldValue Is SkiaSharp.SKColor Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As SkiaSharp.SKColor = SkiaSharp.SKColors.Black
                                If SkiaSharp.SKColor.TryParse(xel.Value, val) Then
                                    val = SkiaSharp.SKColor.Parse(xel.Value)
                                Else
                                    Dim val2 As Color
                                    Try
                                        val2 = Color.FromName(xel.Value)
                                        val = New SkiaSharp.SKColor(val2.R, val2.G, val2.B, val2.A)
                                    Catch ex As Exception
                                    End Try
                                End If
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf fieldValue Is ArrayList Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            Dim val As ArrayList = StringToArray(xel.Value, ci)
                            If Not val Is Nothing Then typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                        ElseIf TypeOf fieldValue Is Byte Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As Byte = xel.Value
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf fieldValue Is Date Then
                            'Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            Dim xel As XElement = OnitUtilities.GetFilteredXElementsEnumerable(xmlprops, propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As Date = Date.Parse(xel.Value, CultureInfo.InvariantCulture)
                                typeAccessor(obj,propname) = val ' fieldInfo.SetValue(obj, val)
                            End If
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is TimeSpan Then
                            Dim xel As XElement = (From xmlprop In xmlprops Select xmlprop Where xmlprop.Name = propname).FirstOrDefault
                            If Not xel Is Nothing Then
                                Dim val As TimeSpan = TimeSpan.FromMilliseconds(xel.Value)
                                obj.GetType.GetField(prop.Name).SetValue(obj, val)
                            End If
                        End If
                    End If
                End If
            Next
        End If
        Return True

    End Function

    ''' <summary>
    ''' Serializes selected properties of an object to XML.
    ''' </summary>
    ''' <param name="obj">The object to serialize.</param>
    ''' <returns>True if successful.</returns>
    ''' <remarks> Properties of type String, Double, Single, Integer, Nullable(Of Double), 
    ''' Nullable(Of Single), Nullable(Of Integer), ArrayList, Font, Color, [Enum]
    ''' are supported.</remarks>
    Shared Function Serialize(obj As Object, Optional ByVal Fields As Boolean = False) As System.Collections.Generic.List(Of XElement)

        Dim elements As New List(Of System.Xml.Linq.XElement)
        Dim ci As CultureInfo = CultureInfo.InvariantCulture
        Dim skip As Boolean = False
        With elements
            .Add(New XElement("Type", obj.GetType.ToString))
            If Not Fields Then
                Dim props As PropertyInfo() = obj.GetType.GetProperties()
                For Each prop As PropertyInfo In props
                    skip = False
                    Dim propname As String = prop.Name
                    Dim properties = obj.GetType().GetProperties().Where(Function(p) p.Name = prop.Name)
                    Dim attributes As Object() = properties(0).GetCustomAttributes(True)
                    For Each attr As Attribute In attributes
                        If TypeOf attr Is System.Xml.Serialization.XmlIgnoreAttribute Then
                            skip = True
                            Exit For
                        End If
                    Next
                    If Not skip Then
                        If prop.CanRead Then
                            If TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is ICustomXMLSerialization Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), ICustomXMLSerialization).SaveData.ToArray()))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is ArrayList Then
                                .Add(New XElement(prop.Name, ArrayToString(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), ci)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Single Then
                                .Add(New XElement(prop.Name, Single.Parse(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)).ToString("R", ci)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Double Then
                                .Add(New XElement(prop.Name, Double.Parse(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)).ToString("R", ci)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Nullable(Of Double) Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), Nullable(Of Double)).GetValueOrDefault.ToString("R", ci)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Nullable(Of Single) Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), Nullable(Of Single)).GetValueOrDefault.ToString("R", ci)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Nullable(Of Integer) Then
                                .Add(New XElement(prop.Name, obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Integer Then
                                .Add(New XElement(prop.Name, obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Boolean Then
                                .Add(New XElement(prop.Name, obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is String Then
                                .Add(New XElement(prop.Name, obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is [Enum] Then
                                .Add(New XElement(prop.Name, obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Font Then
                                .Add(New XElement(prop.Name, New FontConverter().ConvertToInvariantString(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing))))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Color Then
                                .Add(New XElement(prop.Name, ColorTranslator.ToHtml(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing))))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is SkiaSharp.SKColor Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), SkiaSharp.SKColor).ToString))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Byte Then
                                .Add(New XElement(prop.Name, obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Date Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), Date).ToString(CultureInfo.InvariantCulture)))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is TimeSpan Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), TimeSpan).TotalMilliseconds))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is OxyPlot.OxyColor Then
                                .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), OxyPlot.OxyColor).ToString()))
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is List(Of String) Then
                                Try
                                    Dim inner_elements As New List(Of XElement)
                                    For Each item In DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), List(Of String))
                                        inner_elements.Add(New XElement("Item", item))
                                    Next
                                    .Add(New XElement(prop.Name, inner_elements))
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is List(Of Double) Then
                                Try
                                    Dim inner_elements As New List(Of XElement)
                                    For Each item In DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), List(Of Double))
                                        inner_elements.Add(New XElement("Item", item.ToString(ci)))
                                    Next
                                    .Add(New XElement(prop.Name, inner_elements))
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is List(Of Single) Then
                                Try
                                    Dim inner_elements As New List(Of XElement)
                                    For Each item In DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), List(Of Single))
                                        inner_elements.Add(New XElement("Item", item.ToString(ci)))
                                    Next
                                    .Add(New XElement(prop.Name, inner_elements))
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Dictionary(Of String, String) Then
                                Try
                                    Dim inner_elements As New List(Of XElement)
                                    For Each item In DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), Dictionary(Of String, String))
                                        inner_elements.Add(New XElement("Item", New XAttribute("Key", item.Key), New XAttribute("Value", item.Value)))
                                    Next
                                    .Add(New XElement(prop.Name, inner_elements))
                                Catch ex As Exception
                                End Try
                            ElseIf TypeOf obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing) Is Dictionary(Of String, Double) Then
                                Try
                                    Dim inner_elements As New List(Of XElement)
                                    For Each item In DirectCast(obj.GetType.GetProperty(prop.Name).GetValue(obj, Nothing), Dictionary(Of String, Double))
                                        inner_elements.Add(New XElement("Item", New XAttribute("Key", item.Key), New XAttribute("Value", item.Value.ToString(ci))))
                                    Next
                                    .Add(New XElement(prop.Name, inner_elements))
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                    End If
                Next
            Else
                Dim props As FieldInfo() = obj.GetType.GetFields()
                For Each prop As FieldInfo In props
                    skip = False
                    Dim propname As String = prop.Name
                    Dim properties = obj.GetType().GetFields().Where(Function(p) p.Name = prop.Name)
                    Dim attributes As Object() = properties(0).GetCustomAttributes(True)
                    For Each attr As Attribute In attributes
                        If TypeOf attr Is System.Xml.Serialization.XmlIgnoreAttribute Then
                            skip = True
                            Exit For
                        End If
                    Next
                    If Not skip Then
                        If TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is ICustomXMLSerialization Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), ICustomXMLSerialization).SaveData.ToArray()))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is ArrayList Then
                            .Add(New XElement(prop.Name, ArrayToString(obj.GetType.GetField(prop.Name).GetValue(obj), ci)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Single Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), System.Single).ToString("R", ci)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Double Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), System.Double).ToString("R", ci)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Nullable(Of Double) Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), Nullable(Of Double)).GetValueOrDefault.ToString("R", ci)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Nullable(Of Single) Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), Nullable(Of Single)).GetValueOrDefault.ToString("R", ci)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Nullable(Of Integer) Then
                            .Add(New XElement(prop.Name, obj.GetType.GetField(prop.Name).GetValue(obj)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Integer Then
                            .Add(New XElement(prop.Name, obj.GetType.GetField(prop.Name).GetValue(obj)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Boolean Then
                            .Add(New XElement(prop.Name, obj.GetType.GetField(prop.Name).GetValue(obj)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is String Then
                            .Add(New XElement(prop.Name, obj.GetType.GetField(prop.Name).GetValue(obj)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is [Enum] Then
                            .Add(New XElement(prop.Name, obj.GetType.GetField(prop.Name).GetValue(obj)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Font Then
                            .Add(New XElement(prop.Name, New FontConverter().ConvertToInvariantString(obj.GetType.GetField(prop.Name).GetValue(obj))))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Color Then
                            .Add(New XElement(prop.Name, ColorTranslator.ToHtml(obj.GetType.GetField(prop.Name).GetValue(obj))))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is SkiaSharp.SKColor Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), SkiaSharp.SKColor).ToString))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Byte Then
                            .Add(New XElement(prop.Name, obj.GetType.GetField(prop.Name).GetValue(obj)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is Date Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), Date).ToString(CultureInfo.InvariantCulture)))
                        ElseIf TypeOf obj.GetType.GetField(prop.Name).GetValue(obj) Is TimeSpan Then
                            .Add(New XElement(prop.Name, DirectCast(obj.GetType.GetField(prop.Name).GetValue(obj), TimeSpan).TotalMilliseconds))
                        End If
                    End If
                Next
            End If
        End With
        Return elements

    End Function

    Public Shared Function ArrayToString(sourcearray As ArrayList, ci As CultureInfo) As String

        Dim sb As String = ""

        If sourcearray.Count > 0 Then

            For Each obj As Object In sourcearray
                If TypeOf obj Is Double Then
                    sb += Double.Parse(obj).ToString("R", ci) + ","
                Else
                    sb += obj.ToString + ","
                End If
            Next

            sb = sb.Remove(sb.Length - 1)

        End If

        Return sb

    End Function

    Public Shared Function ArrayToString2(sourcearray As Array, ci As CultureInfo) As String

        Dim sb As String = ""

        If Not sourcearray Is Nothing Then
            If sourcearray.Length > 0 Then

                For Each obj As Object In sourcearray
                    If TypeOf obj Is Double Then
                        sb += Double.Parse(obj).ToString("R", ci) + ","
                    Else
                        sb += obj.ToString + ","
                    End If
                Next

                sb = sb.Remove(sb.Length - 1)

            End If
        End If

        Return sb

    End Function

    Public Shared Function StringToArray(ByVal text As String, ci As CultureInfo) As ArrayList

        If Not text Is Nothing Then
            Dim values() As String = text.Split(",")
            Dim myarr As New ArrayList

            For Each s As String In values
                If Double.TryParse(s, NumberStyles.Any, ci, New Double) Then
                    myarr.Add(Double.Parse(s, ci))
                Else
                    myarr.Add(s)
                End If
            Next
            Return myarr
        Else
            Return New ArrayList()
        End If
    End Function

    Public Shared Function StringToArray2(ByVal text As String, ci As CultureInfo, arraytype As Type) As Array

        If Not text Is Nothing Then
            Dim values() As String = text.Split(",")
            Dim myarr As New ArrayList

            For Each s As String In values
                If Double.TryParse(s, NumberStyles.Any, ci, New Double) Then
                    myarr.Add(Double.Parse(s, ci))
                Else
                    myarr.Add(s)
                End If
            Next

            Return myarr.ToArray(arraytype)
        Else
            Return New ArrayList().ToArray(arraytype)
        End If

    End Function

End Class

