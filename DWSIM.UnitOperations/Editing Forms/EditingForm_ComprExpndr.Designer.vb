﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditingForm_ComprExpndr
    Inherits SharedClasses.ObjectEditorForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditingForm_ComprExpndr))
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.lblTag = New System.Windows.Forms.TextBox()
        Me.lblConnectedTo = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.rtbAnnotations = New Extended.Windows.Forms.RichTextBoxExtended()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cbFlashAlg = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbPropPack = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cbDeltaT = New System.Windows.Forms.ComboBox()
        Me.tbDeltaT = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbTemp = New System.Windows.Forms.ComboBox()
        Me.tbTemp = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cbPower = New System.Windows.Forms.ComboBox()
        Me.tbPower = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbPress = New System.Windows.Forms.ComboBox()
        Me.tbOutletPressure = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbEfficiency = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbPressureDropU = New System.Windows.Forms.ComboBox()
        Me.tbPressureDrop = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbCalcMode = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbEnergy = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbOutlet1 = New System.Windows.Forms.ComboBox()
        Me.cbInlet1 = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCreateAndConnectEnergy = New System.Windows.Forms.Button()
        Me.btnCreateAndConnectOutlet1 = New System.Windows.Forms.Button()
        Me.btnCreateAndConnectInlet1 = New System.Windows.Forms.Button()
        Me.btnDisconnectEnergy = New System.Windows.Forms.Button()
        Me.btnDisconnectOutlet1 = New System.Windows.Forms.Button()
        Me.btnDisconnect1 = New System.Windows.Forms.Button()
        Me.chkActive = New System.Windows.Forms.CheckBox()
        Me.btnConfigureFlashAlg = New System.Windows.Forms.Button()
        Me.btnConfigurePP = New System.Windows.Forms.Button()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox5
        '
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Controls.Add(Me.lblTag)
        Me.GroupBox5.Controls.Add(Me.chkActive)
        Me.GroupBox5.Controls.Add(Me.lblConnectedTo)
        Me.GroupBox5.Controls.Add(Me.lblStatus)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox5, resources.GetString("GroupBox5.ToolTip"))
        '
        'lblTag
        '
        resources.ApplyResources(Me.lblTag, "lblTag")
        Me.lblTag.Name = "lblTag"
        Me.ToolTip1.SetToolTip(Me.lblTag, resources.GetString("lblTag.ToolTip"))
        '
        'lblConnectedTo
        '
        resources.ApplyResources(Me.lblConnectedTo, "lblConnectedTo")
        Me.lblConnectedTo.Name = "lblConnectedTo"
        Me.ToolTip1.SetToolTip(Me.lblConnectedTo, resources.GetString("lblConnectedTo.ToolTip"))
        '
        'lblStatus
        '
        resources.ApplyResources(Me.lblStatus, "lblStatus")
        Me.lblStatus.Name = "lblStatus"
        Me.ToolTip1.SetToolTip(Me.lblStatus, resources.GetString("lblStatus.ToolTip"))
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        Me.ToolTip1.SetToolTip(Me.Label13, resources.GetString("Label13.ToolTip"))
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        Me.ToolTip1.SetToolTip(Me.Label12, resources.GetString("Label12.ToolTip"))
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        Me.ToolTip1.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip"))
        '
        'GroupBox4
        '
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Controls.Add(Me.rtbAnnotations)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip"))
        '
        'rtbAnnotations
        '
        resources.ApplyResources(Me.rtbAnnotations, "rtbAnnotations")
        Me.rtbAnnotations.Name = "rtbAnnotations"
        Me.rtbAnnotations.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang1046{\fonttbl{\f0\fnil\fcharset0 Microsoft S" & _
    "ans Serif;}}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\viewkind4\uc1\pard\f0\fs17\par" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.rtbAnnotations.ShowRedo = False
        Me.rtbAnnotations.ShowUndo = False
        Me.ToolTip1.SetToolTip(Me.rtbAnnotations, resources.GetString("rtbAnnotations.ToolTip"))
        '
        'GroupBox3
        '
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Controls.Add(Me.btnConfigureFlashAlg)
        Me.GroupBox3.Controls.Add(Me.btnConfigurePP)
        Me.GroupBox3.Controls.Add(Me.cbFlashAlg)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.cbPropPack)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox3, resources.GetString("GroupBox3.ToolTip"))
        '
        'cbFlashAlg
        '
        resources.ApplyResources(Me.cbFlashAlg, "cbFlashAlg")
        Me.cbFlashAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFlashAlg.FormattingEnabled = True
        Me.cbFlashAlg.Name = "cbFlashAlg"
        Me.ToolTip1.SetToolTip(Me.cbFlashAlg, resources.GetString("cbFlashAlg.ToolTip"))
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        Me.ToolTip1.SetToolTip(Me.Label10, resources.GetString("Label10.ToolTip"))
        '
        'cbPropPack
        '
        resources.ApplyResources(Me.cbPropPack, "cbPropPack")
        Me.cbPropPack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPropPack.FormattingEnabled = True
        Me.cbPropPack.Name = "cbPropPack"
        Me.ToolTip1.SetToolTip(Me.cbPropPack, resources.GetString("cbPropPack.ToolTip"))
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        Me.ToolTip1.SetToolTip(Me.Label9, resources.GetString("Label9.ToolTip"))
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.cbDeltaT)
        Me.GroupBox2.Controls.Add(Me.tbDeltaT)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.cbTemp)
        Me.GroupBox2.Controls.Add(Me.tbTemp)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.cbPower)
        Me.GroupBox2.Controls.Add(Me.tbPower)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.cbPress)
        Me.GroupBox2.Controls.Add(Me.tbOutletPressure)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.tbEfficiency)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.cbPressureDropU)
        Me.GroupBox2.Controls.Add(Me.tbPressureDrop)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.cbCalcMode)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip"))
        '
        'cbDeltaT
        '
        resources.ApplyResources(Me.cbDeltaT, "cbDeltaT")
        Me.cbDeltaT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDeltaT.FormattingEnabled = True
        Me.cbDeltaT.Items.AddRange(New Object() {resources.GetString("cbDeltaT.Items"), resources.GetString("cbDeltaT.Items1"), resources.GetString("cbDeltaT.Items2")})
        Me.cbDeltaT.Name = "cbDeltaT"
        Me.ToolTip1.SetToolTip(Me.cbDeltaT, resources.GetString("cbDeltaT.ToolTip"))
        '
        'tbDeltaT
        '
        resources.ApplyResources(Me.tbDeltaT, "tbDeltaT")
        Me.tbDeltaT.Name = "tbDeltaT"
        Me.ToolTip1.SetToolTip(Me.tbDeltaT, resources.GetString("tbDeltaT.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'cbTemp
        '
        resources.ApplyResources(Me.cbTemp, "cbTemp")
        Me.cbTemp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTemp.FormattingEnabled = True
        Me.cbTemp.Items.AddRange(New Object() {resources.GetString("cbTemp.Items"), resources.GetString("cbTemp.Items1"), resources.GetString("cbTemp.Items2")})
        Me.cbTemp.Name = "cbTemp"
        Me.ToolTip1.SetToolTip(Me.cbTemp, resources.GetString("cbTemp.ToolTip"))
        '
        'tbTemp
        '
        resources.ApplyResources(Me.tbTemp, "tbTemp")
        Me.tbTemp.Name = "tbTemp"
        Me.ToolTip1.SetToolTip(Me.tbTemp, resources.GetString("tbTemp.ToolTip"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'cbPower
        '
        resources.ApplyResources(Me.cbPower, "cbPower")
        Me.cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPower.FormattingEnabled = True
        Me.cbPower.Items.AddRange(New Object() {resources.GetString("cbPower.Items"), resources.GetString("cbPower.Items1"), resources.GetString("cbPower.Items2")})
        Me.cbPower.Name = "cbPower"
        Me.ToolTip1.SetToolTip(Me.cbPower, resources.GetString("cbPower.ToolTip"))
        '
        'tbPower
        '
        resources.ApplyResources(Me.tbPower, "tbPower")
        Me.tbPower.Name = "tbPower"
        Me.ToolTip1.SetToolTip(Me.tbPower, resources.GetString("tbPower.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'cbPress
        '
        resources.ApplyResources(Me.cbPress, "cbPress")
        Me.cbPress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPress.FormattingEnabled = True
        Me.cbPress.Items.AddRange(New Object() {resources.GetString("cbPress.Items"), resources.GetString("cbPress.Items1"), resources.GetString("cbPress.Items2")})
        Me.cbPress.Name = "cbPress"
        Me.ToolTip1.SetToolTip(Me.cbPress, resources.GetString("cbPress.ToolTip"))
        '
        'tbOutletPressure
        '
        resources.ApplyResources(Me.tbOutletPressure, "tbOutletPressure")
        Me.tbOutletPressure.Name = "tbOutletPressure"
        Me.ToolTip1.SetToolTip(Me.tbOutletPressure, resources.GetString("tbOutletPressure.ToolTip"))
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'tbEfficiency
        '
        resources.ApplyResources(Me.tbEfficiency, "tbEfficiency")
        Me.tbEfficiency.Name = "tbEfficiency"
        Me.ToolTip1.SetToolTip(Me.tbEfficiency, resources.GetString("tbEfficiency.ToolTip"))
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'cbPressureDropU
        '
        resources.ApplyResources(Me.cbPressureDropU, "cbPressureDropU")
        Me.cbPressureDropU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPressureDropU.FormattingEnabled = True
        Me.cbPressureDropU.Items.AddRange(New Object() {resources.GetString("cbPressureDropU.Items"), resources.GetString("cbPressureDropU.Items1"), resources.GetString("cbPressureDropU.Items2")})
        Me.cbPressureDropU.Name = "cbPressureDropU"
        Me.ToolTip1.SetToolTip(Me.cbPressureDropU, resources.GetString("cbPressureDropU.ToolTip"))
        '
        'tbPressureDrop
        '
        resources.ApplyResources(Me.tbPressureDrop, "tbPressureDrop")
        Me.tbPressureDrop.Name = "tbPressureDrop"
        Me.ToolTip1.SetToolTip(Me.tbPressureDrop, resources.GetString("tbPressureDrop.ToolTip"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'cbCalcMode
        '
        resources.ApplyResources(Me.cbCalcMode, "cbCalcMode")
        Me.cbCalcMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCalcMode.FormattingEnabled = True
        Me.cbCalcMode.Items.AddRange(New Object() {resources.GetString("cbCalcMode.Items"), resources.GetString("cbCalcMode.Items1"), resources.GetString("cbCalcMode.Items2"), resources.GetString("cbCalcMode.Items3")})
        Me.cbCalcMode.Name = "cbCalcMode"
        Me.ToolTip1.SetToolTip(Me.cbCalcMode, resources.GetString("cbCalcMode.ToolTip"))
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        Me.ToolTip1.SetToolTip(Me.Label8, resources.GetString("Label8.ToolTip"))
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.btnCreateAndConnectEnergy)
        Me.GroupBox1.Controls.Add(Me.btnCreateAndConnectOutlet1)
        Me.GroupBox1.Controls.Add(Me.btnCreateAndConnectInlet1)
        Me.GroupBox1.Controls.Add(Me.btnDisconnectEnergy)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.cbEnergy)
        Me.GroupBox1.Controls.Add(Me.btnDisconnectOutlet1)
        Me.GroupBox1.Controls.Add(Me.btnDisconnect1)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.cbOutlet1)
        Me.GroupBox1.Controls.Add(Me.cbInlet1)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        Me.ToolTip1.SetToolTip(Me.Label14, resources.GetString("Label14.ToolTip"))
        '
        'cbEnergy
        '
        resources.ApplyResources(Me.cbEnergy, "cbEnergy")
        Me.cbEnergy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEnergy.FormattingEnabled = True
        Me.cbEnergy.Name = "cbEnergy"
        Me.ToolTip1.SetToolTip(Me.cbEnergy, resources.GetString("cbEnergy.ToolTip"))
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        '
        'cbOutlet1
        '
        resources.ApplyResources(Me.cbOutlet1, "cbOutlet1")
        Me.cbOutlet1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbOutlet1.FormattingEnabled = True
        Me.cbOutlet1.Name = "cbOutlet1"
        Me.ToolTip1.SetToolTip(Me.cbOutlet1, resources.GetString("cbOutlet1.ToolTip"))
        '
        'cbInlet1
        '
        resources.ApplyResources(Me.cbInlet1, "cbInlet1")
        Me.cbInlet1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbInlet1.FormattingEnabled = True
        Me.cbInlet1.Name = "cbInlet1"
        Me.ToolTip1.SetToolTip(Me.cbInlet1, resources.GetString("cbInlet1.ToolTip"))
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        Me.ToolTip1.SetToolTip(Me.Label19, resources.GetString("Label19.ToolTip"))
        '
        'btnCreateAndConnectEnergy
        '
        resources.ApplyResources(Me.btnCreateAndConnectEnergy, "btnCreateAndConnectEnergy")
        Me.btnCreateAndConnectEnergy.BackgroundImage = Global.DWSIM.UnitOperations.My.Resources.Resources.bullet_lightning
        Me.btnCreateAndConnectEnergy.Name = "btnCreateAndConnectEnergy"
        Me.ToolTip1.SetToolTip(Me.btnCreateAndConnectEnergy, resources.GetString("btnCreateAndConnectEnergy.ToolTip"))
        Me.btnCreateAndConnectEnergy.UseVisualStyleBackColor = True
        '
        'btnCreateAndConnectOutlet1
        '
        resources.ApplyResources(Me.btnCreateAndConnectOutlet1, "btnCreateAndConnectOutlet1")
        Me.btnCreateAndConnectOutlet1.BackgroundImage = Global.DWSIM.UnitOperations.My.Resources.Resources.bullet_lightning
        Me.btnCreateAndConnectOutlet1.Name = "btnCreateAndConnectOutlet1"
        Me.ToolTip1.SetToolTip(Me.btnCreateAndConnectOutlet1, resources.GetString("btnCreateAndConnectOutlet1.ToolTip"))
        Me.btnCreateAndConnectOutlet1.UseVisualStyleBackColor = True
        '
        'btnCreateAndConnectInlet1
        '
        resources.ApplyResources(Me.btnCreateAndConnectInlet1, "btnCreateAndConnectInlet1")
        Me.btnCreateAndConnectInlet1.BackgroundImage = Global.DWSIM.UnitOperations.My.Resources.Resources.bullet_lightning
        Me.btnCreateAndConnectInlet1.Name = "btnCreateAndConnectInlet1"
        Me.ToolTip1.SetToolTip(Me.btnCreateAndConnectInlet1, resources.GetString("btnCreateAndConnectInlet1.ToolTip"))
        Me.btnCreateAndConnectInlet1.UseVisualStyleBackColor = True
        '
        'btnDisconnectEnergy
        '
        resources.ApplyResources(Me.btnDisconnectEnergy, "btnDisconnectEnergy")
        Me.btnDisconnectEnergy.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.disconnect
        Me.btnDisconnectEnergy.Name = "btnDisconnectEnergy"
        Me.ToolTip1.SetToolTip(Me.btnDisconnectEnergy, resources.GetString("btnDisconnectEnergy.ToolTip"))
        Me.btnDisconnectEnergy.UseVisualStyleBackColor = True
        '
        'btnDisconnectOutlet1
        '
        resources.ApplyResources(Me.btnDisconnectOutlet1, "btnDisconnectOutlet1")
        Me.btnDisconnectOutlet1.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.disconnect
        Me.btnDisconnectOutlet1.Name = "btnDisconnectOutlet1"
        Me.ToolTip1.SetToolTip(Me.btnDisconnectOutlet1, resources.GetString("btnDisconnectOutlet1.ToolTip"))
        Me.btnDisconnectOutlet1.UseVisualStyleBackColor = True
        '
        'btnDisconnect1
        '
        resources.ApplyResources(Me.btnDisconnect1, "btnDisconnect1")
        Me.btnDisconnect1.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.disconnect
        Me.btnDisconnect1.Name = "btnDisconnect1"
        Me.ToolTip1.SetToolTip(Me.btnDisconnect1, resources.GetString("btnDisconnect1.ToolTip"))
        Me.btnDisconnect1.UseVisualStyleBackColor = True
        '
        'chkActive
        '
        resources.ApplyResources(Me.chkActive, "chkActive")
        Me.chkActive.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.bullet_tick
        Me.chkActive.Name = "chkActive"
        Me.ToolTip1.SetToolTip(Me.chkActive, resources.GetString("chkActive.ToolTip"))
        Me.chkActive.UseVisualStyleBackColor = True
        '
        'btnConfigureFlashAlg
        '
        resources.ApplyResources(Me.btnConfigureFlashAlg, "btnConfigureFlashAlg")
        Me.btnConfigureFlashAlg.BackgroundImage = Global.DWSIM.UnitOperations.My.Resources.Resources.cog
        Me.btnConfigureFlashAlg.Name = "btnConfigureFlashAlg"
        Me.ToolTip1.SetToolTip(Me.btnConfigureFlashAlg, resources.GetString("btnConfigureFlashAlg.ToolTip"))
        Me.btnConfigureFlashAlg.UseVisualStyleBackColor = True
        '
        'btnConfigurePP
        '
        resources.ApplyResources(Me.btnConfigurePP, "btnConfigurePP")
        Me.btnConfigurePP.BackgroundImage = Global.DWSIM.UnitOperations.My.Resources.Resources.cog
        Me.btnConfigurePP.Name = "btnConfigurePP"
        Me.ToolTip1.SetToolTip(Me.btnConfigurePP, resources.GetString("btnConfigurePP.ToolTip"))
        Me.btnConfigurePP.UseVisualStyleBackColor = True
        '
        'EditingForm_ComprExpndr
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "EditingForm_ComprExpndr"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Public WithEvents chkActive As System.Windows.Forms.CheckBox
    Public WithEvents lblConnectedTo As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Public WithEvents rtbAnnotations As Extended.Windows.Forms.RichTextBoxExtended
    Public WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Public WithEvents cbFlashAlg As System.Windows.Forms.ComboBox
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents cbPropPack As System.Windows.Forms.ComboBox
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents cbCalcMode As System.Windows.Forms.ComboBox
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents cbPressureDropU As System.Windows.Forms.ComboBox
    Public WithEvents tbPressureDrop As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents btnConfigureFlashAlg As System.Windows.Forms.Button
    Public WithEvents btnConfigurePP As System.Windows.Forms.Button
    Public WithEvents cbPower As System.Windows.Forms.ComboBox
    Public WithEvents tbPower As System.Windows.Forms.TextBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents cbPress As System.Windows.Forms.ComboBox
    Public WithEvents tbOutletPressure As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents tbEfficiency As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents btnDisconnectEnergy As System.Windows.Forms.Button
    Public WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents cbEnergy As System.Windows.Forms.ComboBox
    Public WithEvents btnDisconnectOutlet1 As System.Windows.Forms.Button
    Public WithEvents btnDisconnect1 As System.Windows.Forms.Button
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents cbOutlet1 As System.Windows.Forms.ComboBox
    Public WithEvents cbInlet1 As System.Windows.Forms.ComboBox
    Public WithEvents Label19 As System.Windows.Forms.Label
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents lblTag As System.Windows.Forms.TextBox
    Public WithEvents cbDeltaT As System.Windows.Forms.ComboBox
    Public WithEvents tbDeltaT As System.Windows.Forms.TextBox
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents cbTemp As System.Windows.Forms.ComboBox
    Public WithEvents tbTemp As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents btnCreateAndConnectEnergy As System.Windows.Forms.Button
    Public WithEvents btnCreateAndConnectOutlet1 As System.Windows.Forms.Button
    Public WithEvents btnCreateAndConnectInlet1 As System.Windows.Forms.Button
End Class
