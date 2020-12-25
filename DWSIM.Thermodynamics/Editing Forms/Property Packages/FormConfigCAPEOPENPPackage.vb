﻿Imports DWSIM.Thermodynamics.BaseClasses
Imports Cudafy
Imports Cudafy.Host
Imports System.Drawing
Imports System.Reflection
Imports DWSIM.Thermodynamics.PropertyPackages
Imports System.IO

Public Class FormConfigCAPEOPENPPackage

    Inherits FormConfigPropertyPackageBase

    Public loaded As Boolean = False

    Dim ACSC1 As AutoCompleteStringCollection

    Public FlashAlgorithms As New Dictionary(Of String, Thermodynamics.PropertyPackages.Auxiliary.FlashAlgorithms.FlashAlgorithm)

    Private Sub FormConfigCAPEOPENPPackage_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            Dim inifile As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & Path.DirectorySeparatorChar & "DWSIM Application Data" & Path.DirectorySeparatorChar & "config.ini"
            GlobalSettings.Settings.SaveExcelSettings(inifile)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FormConfigCAPEOPEN2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.TopMost = True

        Application.DoEvents()

        Me.cbGPU.Items.Clear()

        CudafyModes.Target = eGPUType.Emulator
        Try
            For Each prop As GPGPUProperties In CudafyHost.GetDeviceProperties(CudafyModes.Target, False)
                Me.cbGPU.Items.Add("Emulator | " & prop.Name & " (" & prop.DeviceId & ")")
            Next
        Catch ex As Exception
        End Try
        Try
            CudafyModes.Target = eGPUType.Cuda
            For Each prop As GPGPUProperties In CudafyHost.GetDeviceProperties(CudafyModes.Target, False)
                Me.cbGPU.Items.Add("CUDA | " & prop.Name & " (" & prop.DeviceId & ")")
            Next
        Catch ex As Exception
        End Try
        Try
            CudafyModes.Target = eGPUType.OpenCL
            For Each prop As GPGPUProperties In CudafyHost.GetDeviceProperties(CudafyModes.Target, False)
                Me.cbGPU.Items.Add("OpenCL | " & prop.Name & " (" & prop.DeviceId & ")")
            Next
        Catch ex As Exception
        End Try

        CudafyModes.Target = GlobalSettings.Settings.CudafyTarget

        Dim i As Integer = 0
        Me.cbParallelism.Items.Clear()
        Me.cbParallelism.Items.Add("Default")
        For i = 1 To System.Environment.ProcessorCount
            Me.cbParallelism.Items.Add(i.ToString)
        Next
        If GlobalSettings.Settings.MaxDegreeOfParallelism = -1 Then
            Me.cbParallelism.SelectedIndex = 0
        ElseIf GlobalSettings.Settings.MaxDegreeOfParallelism <= System.Environment.ProcessorCount Then
            Me.cbParallelism.SelectedItem = GlobalSettings.Settings.MaxDegreeOfParallelism.ToString
        Else
            Me.cbParallelism.SelectedIndex = Me.cbParallelism.Items.Count - 1
        End If

        Me.chkEnableParallelCalcs.Checked = GlobalSettings.Settings.EnableParallelProcessing
        Me.chkEnableGPUProcessing.Checked = GlobalSettings.Settings.EnableGPUProcessing
        Me.cbGPU.Enabled = Me.chkEnableGPUProcessing.Checked
        Me.tbGPUCaps.Enabled = Me.chkEnableGPUProcessing.Checked
        Me.cbParallelism.Enabled = Me.chkEnableParallelCalcs.Checked
        Me.cbSIMD.Checked = GlobalSettings.Settings.UseSIMDExtensions

        tbOctavePath.Text = GlobalSettings.Settings.OctavePath

        Me.TextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource

        Me.lblName.Text = _pp.ComponentName
        Me.lblDescription.Text = _pp.ComponentDescription
        Me.tbErrorLog.Text = _pp.ExceptionLog

        Dim comp As ConstantProperties

        If Not loaded Then

            ACSC1 = New AutoCompleteStringCollection

            For Each comp In _pp._selectedcomps.Values
                Me.ListViewA.Items.Add(comp.Name, comp.Name, 0).Tag = comp.Name
            Next
            For Each comp In _pp._availablecomps.Values
                Dim idx As Integer = Me.AddCompToGrid(comp)
                If Not idx = -1 Then
                    ACSC1.Add(comp.Name)
                End If
            Next

            Try
                Me.TextBox1.AutoCompleteCustomSource = ACSC1
            Catch ex As Exception

            End Try

        Else

            For Each r As DataGridViewRow In ogc1.Rows
                If _pp._availablecomps.ContainsKey(r.Cells(0).Value) Then
                    comp = _pp._availablecomps(r.Cells(0).Value)
                End If
            Next

            Try
                Me.ogc1.Sort(ogc1.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
            Catch ex As Exception
            End Try

        End If

        If Not TypeOf _pp Is SeawaterPropertyPackage And
            Not TypeOf _pp Is SteamTablesPropertyPackage And
            Not TypeOf _pp Is SourWaterPropertyPackage Then

            AddFlashAlgorithms()

            ComboBoxFlashAlg.Items.Clear()
            ComboBoxFlashAlg.Items.AddRange(FlashAlgorithms.Keys.ToArray)
            ComboBoxFlashAlg.SelectedItem = _pp.FlashBase.Name

        Else

            ComboBoxFlashAlg.Enabled = False
            btnConfigFlashAlg.Enabled = False

        End If

        Me.loaded = True

    End Sub

    Private Sub ComboBoxFlashAlg_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxFlashAlg.SelectedIndexChanged

        _pp.FlashAlgorithm = FlashAlgorithms(ComboBoxFlashAlg.SelectedItem)

    End Sub

    Public Function AddCompToGrid(ByRef comp As ConstantProperties) As Integer

        Dim contains As Boolean = False
        For Each r As DataGridViewRow In ogc1.Rows
            If r.Cells(0).Value = comp.Name Then contains = True
        Next

        If Not contains Then
            Dim r As New DataGridViewRow
            r.CreateCells(ogc1, New Object() {comp.Name, comp.Name, comp.OriginalDB, comp.Formula})
            ogc1.Rows.Add(r)
            Return ogc1.Rows.Count - 1
        Else
            Return -1
        End If

    End Function

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If Me.ogc1.SelectedRows.Count > 0 Then
            Me.AddCompToSimulation(Me.ogc1.SelectedRows(0).Index)
        End If
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If Me.ListViewA.SelectedItems.Count > 0 Then
            Me.RemoveCompFromSimulation(Me.ListViewA.SelectedItems(0).Tag)
        End If
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        For Each lvi As ListViewItem In Me.ListViewA.Items
            Me.RemoveCompFromSimulation(lvi.Tag)
        Next
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ogc1.Sort(ogc1.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
    End Sub

    Sub AddComponent(ByVal compID As String)
        If Not _pp._selectedcomps.ContainsKey(compID) Then
            Dim tmpcomp As New ConstantProperties
            tmpcomp = _pp._availablecomps(compID)
            _pp._selectedcomps.Add(tmpcomp.Name, tmpcomp)
            _pp._availablecomps.Remove(tmpcomp.Name)
            Me.ListViewA.Items.Add(tmpcomp.Name, (tmpcomp.Name), 0).Tag = tmpcomp.Name
        End If
    End Sub

    Sub RemoveComponent(ByVal compID As String)
        Me.RemoveCompFromSimulation(compID)
    End Sub

    Sub AddCompToSimulation(ByVal index As Integer)

        If Me.loaded Then
            If Not _pp._selectedcomps.ContainsKey(ogc1.Rows(index).Cells(0).Value) Then
                Dim tmpcomp As New ConstantProperties
                tmpcomp = _pp._availablecomps(ogc1.Rows(index).Cells(0).Value)
                _pp._selectedcomps.Add(tmpcomp.Name, tmpcomp)
                _pp._availablecomps.Remove(tmpcomp.Name)
                Me.ListViewA.Items.Add(tmpcomp.Name & " (" & tmpcomp.OriginalDB & ")", tmpcomp.Name).Tag = tmpcomp.Name
                Me.ogc1.Rows.RemoveAt(index)
            End If
        End If

    End Sub

    Sub RemoveCompFromSimulation(ByVal compid As String)

        Dim tmpcomp As New ConstantProperties
        Dim nm As String = compid
        tmpcomp = _pp._selectedcomps(nm)
        _pp._selectedcomps.Remove(tmpcomp.Name)
        Me.ListViewA.Items.RemoveByKey(tmpcomp.Name)
        _pp._availablecomps.Add(tmpcomp.Name, tmpcomp)
        Me.AddCompToGrid(tmpcomp)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        _pp.DisplayEditingForm()

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        For Each r As DataGridViewRow In ogc1.Rows
            If Not r.Cells(1).Value Is Nothing Then
                If r.Cells(1).Value.ToString = Me.TextBox1.Text Then
                    r.Selected = True
                    If r.Visible Then ogc1.FirstDisplayedScrollingRowIndex = r.Index
                Else
                    r.Selected = False
                End If
            End If
        Next
        If TextBox1.Text = "" Then
            ogc1.FirstDisplayedScrollingRowIndex = 0
            For Each r As DataGridViewRow In ogc1.Rows
                r.Selected = False
            Next
        End If
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call Button7_Click(sender, e)
            Me.TextBox1.Text = ""
        End If
    End Sub

    Public Sub GetCUDACaps(prop As GPGPUProperties)

        Dim i As Integer = 0

        Me.tbGPUCaps.Text = ""

        Me.tbGPUCaps.AppendText(String.Format("   --- General Information for device {0} ---", i) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Name:  {0}", prop.Name) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Device Id:  {0}", prop.DeviceId) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Compute capability:  {0}.{1}", prop.Capability.Major, prop.Capability.Minor) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Clock rate: {0}", prop.ClockRate) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Simulated: {0}", prop.IsSimulated) & vbCrLf)

        Me.tbGPUCaps.AppendText(String.Format("   --- Memory Information for device {0} ---", i) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Total global mem:  {0}", prop.TotalMemory) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Total constant Mem:  {0}", prop.TotalConstantMemory) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Max mem pitch:  {0}", prop.MemoryPitch) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Texture Alignment:  {0}", prop.TextureAlignment) & vbCrLf)

        Me.tbGPUCaps.AppendText(String.Format("   --- MP Information for device {0} ---", i) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Shared mem per mp: {0}", prop.SharedMemoryPerBlock) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Registers per mp:  {0}", prop.RegistersPerBlock) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Threads in warp:  {0}", prop.WarpSize) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Max threads per block:  {0}", prop.MaxThreadsPerBlock) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Max thread dimensions:  ({0}, {1}, {2})", prop.MaxThreadsSize.x, prop.MaxThreadsSize.y, prop.MaxThreadsSize.z) & vbCrLf)
        Me.tbGPUCaps.AppendText(String.Format("Max grid dimensions:  ({0}, {1}, {2})", prop.MaxGridSize.x, prop.MaxGridSize.y, prop.MaxGridSize.z) & vbCrLf)

        Me.tbGPUCaps.SelectionStart = 0
        Me.tbGPUCaps.SelectionLength = 0
        Me.tbGPUCaps.ScrollToCaret()

    End Sub

    Private Sub chkEnableParallelCalcs_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkEnableParallelCalcs.CheckedChanged
        GlobalSettings.Settings.EnableParallelProcessing = Me.chkEnableParallelCalcs.Checked
        Me.cbParallelism.Enabled = Me.chkEnableParallelCalcs.Checked
    End Sub

    Private Sub cbParallelism_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbParallelism.SelectedIndexChanged
        If Me.cbParallelism.SelectedIndex = 0 Then
            GlobalSettings.Settings.MaxDegreeOfParallelism = -1
        Else
            GlobalSettings.Settings.MaxDegreeOfParallelism = Me.cbParallelism.SelectedItem
        End If
    End Sub

    Private Sub cbGPU_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbGPU.SelectedIndexChanged
        If loaded Then
            If cbGPU.SelectedItem.ToString.Contains("Emulator") Then
                GlobalSettings.Settings.CudafyTarget = eGPUType.Emulator
            ElseIf cbGPU.SelectedItem.ToString.Contains("CUDA") Then
                GlobalSettings.Settings.CudafyTarget = eGPUType.Cuda
            Else
                GlobalSettings.Settings.CudafyTarget = eGPUType.OpenCL
            End If
            Try
                For Each prop As GPGPUProperties In CudafyHost.GetDeviceProperties(CudafyModes.Target, False)
                    If Me.cbGPU.SelectedItem.ToString.Split("|")(1).Contains(prop.Name) Then
                        GlobalSettings.Settings.SelectedGPU = Me.cbGPU.SelectedItem.ToString
                        GlobalSettings.Settings.CudafyDeviceID = prop.DeviceId
                        GetCUDACaps(prop)
                        Exit For
                    End If
                Next
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub chkEnableGPUProcessing_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkEnableGPUProcessing.CheckedChanged
        Me.cbGPU.Enabled = chkEnableGPUProcessing.Checked
        Me.tbGPUCaps.Enabled = chkEnableGPUProcessing.Checked
        GlobalSettings.Settings.EnableGPUProcessing = chkEnableGPUProcessing.Checked
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnConfigFlashAlg.Click

        Dim fa As Auxiliary.FlashAlgorithms.FlashAlgorithm = _pp.FlashBase
        Dim f As New Thermodynamics.FlashAlgorithmConfig() With {.Settings = fa.FlashSettings}

        f.ShowDialog(Me)
        fa.FlashSettings = f.Settings
        f.Dispose()
        f = Nothing

    End Sub

    Private Sub cbSIMD_CheckedChanged(sender As Object, e As EventArgs) Handles cbSIMD.CheckedChanged
        GlobalSettings.Settings.UseSIMDExtensions = Me.cbSIMD.Checked
    End Sub

    Private Sub ogc1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ogc1.CellDoubleClick
        If e.RowIndex > -1 Then AddCompToSimulation(e.RowIndex)
    End Sub

    Sub AddFlashAlgorithms()

        Dim calculatorassembly = Assembly.GetExecutingAssembly
        Dim availableTypes As New List(Of Type)()

        availableTypes.AddRange(calculatorassembly.GetTypes().Where(Function(x) If(x.GetInterface("DWSIM.Interfaces.IFlashAlgorithm") IsNot Nothing, True, False)))

        For Each item In availableTypes.OrderBy(Function(x) x.Name)
            If Not item.IsAbstract Then
                Dim obj = DirectCast(Activator.CreateInstance(item), Interfaces.IFlashAlgorithm)
                obj.Tag = obj.Name
                If Not obj.InternalUseOnly Then FlashAlgorithms.Add(obj.Name, obj)
                If obj.Name.Contains("Gibbs") Then
                    Dim obj2 = DirectCast(Activator.CreateInstance(item), Interfaces.IFlashAlgorithm)
                    obj2.Tag = obj2.Name
                    DirectCast(obj2, Auxiliary.FlashAlgorithms.GibbsMinimization3P).ForceTwoPhaseOnly = True
                    FlashAlgorithms.Add(obj2.Name, obj2)
                End If
                If obj.Name.Contains("SLE") Then
                    Dim obj2 = DirectCast(Activator.CreateInstance(item), Interfaces.IFlashAlgorithm)
                    obj2.Tag = obj2.Name
                    DirectCast(obj2, Auxiliary.FlashAlgorithms.NestedLoopsSLE).SolidSolution = True
                    FlashAlgorithms.Add(obj2.Name, obj2)
                End If
            End If
        Next

    End Sub

    Private Sub btnSelectOctavePath_Click(sender As Object, e As EventArgs) Handles btnSelectOctavePath.Click
        FolderBrowserDialog1.SelectedPath = tbOctavePath.Text
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            tbOctavePath.Text = FolderBrowserDialog1.SelectedPath
            GlobalSettings.Settings.OctavePath = tbOctavePath.Text
        End If
    End Sub
End Class