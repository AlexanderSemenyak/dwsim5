Imports System.Runtime.CompilerServices
Imports System.Text

Public Class StringListFastComparable 
    Implements IList(Of string)
    
    Private list As List(Of String)
    Private hash As Dictionary(Of String, integer)
    Private toArrayStringCache As String
    Public Sub New(capacity As Integer)
        Me.list = new List(Of String)(capacity)
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of String) Implements IEnumerable(Of String).GetEnumerator
        Return Me.list.GetEnumerator()
    End Function

    Public Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.list.GetEnumerator()
    End Function

    Public Sub Add(item As String) Implements ICollection(Of String).Add
        Me.list.Add(item)
        Me.ResetHash()
    End Sub

    Public Sub Clear() Implements ICollection(Of String).Clear
        Me.list.Clear()
        Me.ResetHash()
        Me.toArrayStringCache = Nothing
    End Sub


    Public Function Contains(item As String) As Boolean Implements ICollection(Of String).Contains
        Dim hashLocal as HashSet(Of String) = Me.GetOrUpdateHash()
        Return hashLocal.Contains(item)
    End Function

    Private Sub ResetHash()
        Me.hash = nothing
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function GetOrUpdateHash() As IDictionary(Of String, integer)
        
        Dim hashLocal As Dictionary(Of String, integer) = Me.hash
        if hashLocal IsNot nothing Then return hashLocal
        hashLocal = new Dictionary(Of String,Integer)
        For i As Integer = Me.Count-1  To 0 step -1
            hashLocal(Me(i)) = i
        Next i
        Me.hash = hashLocal
        Return hashLocal
    End Function

    Public Sub CopyTo(array As String(), arrayIndex As Integer) Implements ICollection(Of String).CopyTo
        Me.list.CopyTo(array, arrayIndex)
    End Sub

    Public Function Remove(item As String) As Boolean Implements ICollection(Of String).Remove
        Me.list.Remove(item)
        Me.ResetHash()
    End Function

    Public ReadOnly Property Count As Integer Implements ICollection(Of String).Count
        Get
            Return Me.list.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of String).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function IndexOf(item As String) As Integer Implements IList(Of String).IndexOf
        Dim index As integer = -1
        if Me.GetOrUpdateHash().TryGetValue(item, index) then Return index
        Return -1
    End Function

    Public Sub Insert(index As Integer, item As String) Implements IList(Of String).Insert
        Me.list.Insert(item, index)
        Me.ResetHash()
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of String).RemoveAt
        Me.RemoveAt(index)
        Me.ResetHash()
    End Sub

    Default Public Property Item(index As Integer) As String Implements IList(Of String).Item
        Get
            Return Me.list(index)
        End Get
        Set
            Me.list(index) = value
            Me.ResetHash()
        End Set
    End Property

    Public Function Clone() As IList(Of String)
        Dim clon = new StringListFastComparable(Me.list.Count)

        Dim hashLocal As Dictionary(Of String, integer) = Me.hash
        clon.hash = hashLocal
        clon.list = New List(Of String)(Me.list)
        Return clon
    End Function

    Public Function ToList() As IList(Of String)
        Return Me.Clone()
    End Function

    Public Function ToArrayString() As String
        if Me.toArrayStringCache IsNot nothing then Return Me.toArrayStringCache

        Dim retstr = new StringBuilder("{ ")
        For Each s In Me
            retstr.Append(s)
            retstr.Append( ", ")
        Next
        if Me.Count>0 then
            retstr.Remove(retstr.Length-2,2) 'alexander remove last ", "
        end if
        retstr.Append("}")

        Me.toArrayStringCache = retstr.ToString()

        Return Me.toArrayStringCache
        End Function
End Class

Public Class CompoundsDictionary
    Inherits VirtualDictionary(Of String, Interfaces.ICompound)

    Private Sub Test
        For Each compound As ICompound In Me.Values
            
        Next
    End Sub

    Private retVNames As StringListFastComparable 

    Public Sub New()
    End Sub

    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub

    Public Sub New(ByVal comparer As IEqualityComparer(Of String))
        MyBase.New(comparer)
    End Sub

    Public Sub New(ByVal capacity As Integer, ByVal comparer As IEqualityComparer(Of String))
        MyBase.New(capacity, comparer)
    End Sub

    Public Function RET_VNAMES() As StringListFastComparable
        try
        if Me.retVNames Isnot nothing then 
            return Me.retVNames
        End If

        Dim cache As StringListFastComparable  = new StringListFastComparable (Me.Count)

        Dim i As Integer = 0
        For Each pair As KeyValuePair(Of String,ICompound) In Me
            cache.Add(pair.Value.ConstantProperties.Name)
            i += 1
        Next

        Me.retVNames = cache
        return retVNames
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

    End Function

    Protected Overrides Sub OnChanged()
        MyBase.OnChanged()
        Me.ResetCaches()
    End Sub

    Private Sub ResetCaches()
        Me.retVNames = nothing
    End Sub
End Class


Public Class VirtualDictionary(Of TKey, TValue)
    Implements IDictionary(Of TKey,TValue), IDictionary

    Protected wrappedDictionary As IDictionary(Of TKey, TValue)

    Public Sub New()
        Me.New(1)
    End Sub

    Public Sub New(ByVal capacity As Integer)
        wrappedDictionary = New Dictionary(Of TKey, TValue)(capacity)
    End Sub

    Public Sub New(ByVal comparer As IEqualityComparer(Of TKey))
        wrappedDictionary = New Dictionary(Of TKey, TValue)(comparer)
    End Sub

    Public Sub New(ByVal capacity As Integer, ByVal comparer As IEqualityComparer(Of TKey))
        wrappedDictionary = New Dictionary(Of TKey, TValue)(capacity, comparer)
    End Sub

    Public Sub New(ByVal dictionary As IDictionary(Of TKey, TValue))
        wrappedDictionary = New Dictionary(Of TKey, TValue)(dictionary)
    End Sub

    Public Sub New(ByVal dictionary As IDictionary(Of TKey, TValue), ByVal comparer As IEqualityComparer(Of TKey))
        wrappedDictionary = New Dictionary(Of TKey, TValue)(dictionary, comparer)
    End Sub

    Protected Overridable Sub OnChanged()

    End Sub

    Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey,TValue).ContainsKey
        return wrappedDictionary.ContainsKey(key)
    End Function

    Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey,TValue).Add
        wrappedDictionary.Add(key,value)
    End Sub

    Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey,TValue).Remove
        Return wrappedDictionary.Remove(key)
    End Function

    Public Function TryGetValue(key As TKey, <Out> ByRef value As TValue) As Boolean Implements IDictionary(Of TKey,TValue).TryGetValue
        Return wrappedDictionary.TryGetValue(key, value)
    End Function

    Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey,TValue).Item
        Get
            Return wrappedDictionary(key)
        End Get
        Set(ByVal value As TValue)
            wrappedDictionary(key) = value
        End Set
    End Property

    Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey,TValue).Keys
        Get
            Return wrappedDictionary.Keys
        End Get
    End Property

    Public ReadOnly Property IDictionary_Values As ICollection Implements IDictionary.Values
        Get
            Dim id As IDictionary = wrappedDictionary
            Return id.Values
        End Get
    End Property
    Public ReadOnly Property IDictionary_Keys As ICollection Implements IDictionary.Keys
        Get
            Dim id As IDictionary = wrappedDictionary
            Return id.Keys
        End Get
    End Property
    Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey,TValue).Values
        Get
            Return wrappedDictionary.Values
        End Get
    End Property

    Public ReadOnly Property IDictionary_IsReadOnly As Boolean Implements IDictionary.IsReadOnly
        Get
            Return Me.IsReadOnly
        End Get
    End Property

    Public Function Contains(item As KeyValuePair(Of TKey,TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey,TValue)).Contains
        Return wrappedDictionary.ContainsKey(item.Key)
    End Function

    Public Overridable Sub CopyTo(ByVal array As KeyValuePair(Of TKey, TValue)(), ByVal arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey,TValue)).CopyTo
        wrappedDictionary.CopyTo(array, arrayIndex)
    End Sub

    Public Function Remove(item As KeyValuePair(Of TKey,TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey,TValue)).Remove
        Return wrappedDictionary.Remove(item.Key)
    End Function

    Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
        Dim id As IDictionary = wrappedDictionary
        id.CopyTo(array, index)
    End Sub

    Public ReadOnly Property ICollection_Count As Integer Implements ICollection.Count
        Get
            Return Me.Count
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey,TValue)).Count
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return wrappedDictionary.Count
        End Get
    End Property

    Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
        Get
            Dim id As IDictionary = wrappedDictionary
            Return id.SyncRoot
        End Get
    End Property
    Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
    Get
        Dim id As IDictionary = wrappedDictionary
        Return id.IsSynchronized
    End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey,TValue)).IsReadOnly
        Get
            Return wrappedDictionary.IsReadOnly
        End Get
    End Property

    Public ReadOnly Property IsFixedSize As Boolean Implements IDictionary.IsFixedSize
        Get
            Dim id As IDictionary = wrappedDictionary
            Return id.IsFixedSize
        End Get
    End Property

    Public Overridable Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return wrappedDictionary.GetEnumerator()
    End Function

    Public Sub IDictionary_Clear() Implements IDictionary.Clear
        Me.Clear()
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey,TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey,TValue)).GetEnumerator
        Return wrappedDictionary.GetEnumerator()
    End Function

    Public Sub IDictionary_Remove(key As Object) Implements IDictionary.Remove
        Me.Remove(key)
    End Sub

    Public Property IDictionary_Item(key As Object) As Object Implements IDictionary.Item
        Get
            Return Me.Item(key)
        End Get
        Set
            Me.Item(key) = value
        End Set
    End Property

    Public Sub Add(item As KeyValuePair(Of TKey,TValue)) Implements ICollection(Of KeyValuePair(Of TKey,TValue)).Add
        wrappedDictionary.Add(item.key, item.Value)
    End Sub

    Public Function IDictionary_Contains(key As Object) As Boolean Implements IDictionary.Contains
        'Dim id As IDictionary = wrappedDictionary
        Return Me.Contains(key)
    End Function

    Public Sub IDictionary_Add(key As Object, value As Object) Implements IDictionary.Add
        Me.Add(key,value)
    End Sub

    Public Overridable Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey,TValue)).Clear
        wrappedDictionary.Clear()
    End Sub

    Public Function IDictionary_GetEnumerator() As IDictionaryEnumerator Implements IDictionary.GetEnumerator
        Dim id As IDictionary = wrappedDictionary
        Return id.GetEnumerator()
    End Function
End Class
