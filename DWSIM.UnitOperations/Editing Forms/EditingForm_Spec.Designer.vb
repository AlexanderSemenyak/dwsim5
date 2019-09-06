<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditingForm_Spec

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditingForm_Spec))
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.lblTag = New System.Windows.Forms.TextBox()
        Me.chkActive = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.rtbAnnotations = New Extended.Windows.Forms.RichTextBoxExtended()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbExpression = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbSourceProp2 = New System.Windows.Forms.ComboBox()
        Me.cbSourceObj2 = New System.Windows.Forms.ComboBox()
        Me.lblSourceVal2 = New System.Windows.Forms.Label()
        Me.lblSrcVal2 = New System.Windows.Forms.Label()
        Me.lblSrcProp2 = New System.Windows.Forms.Label()
        Me.lblSrcObj2 = New System.Windows.Forms.Label()
        Me.lblTargetVal = New System.Windows.Forms.Label()
        Me.lblSourceVal = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbTargetProp = New System.Windows.Forms.ComboBox()
        Me.cbTargetObj = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbSourceProp = New System.Windows.Forms.ComboBox()
        Me.cbSourceObj = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTipChangeTag = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox5.SuspendLayout
        Me.GroupBox4.SuspendLayout
        Me.GroupBox2.SuspendLayout
        Me.GroupBox1.SuspendLayout
        Me.SuspendLayout
        '
        'GroupBox5
        '
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Controls.Add(Me.lblTag)
        Me.GroupBox5.Controls.Add(Me.chkActive)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = false
        Me.ToolTipChangeTag.SetToolTip(Me.GroupBox5, resources.GetString("GroupBox5.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.GroupBox5, resources.GetString("GroupBox5.ToolTip1"))
        Me.ToolTip1.SetToolTip(Me.GroupBox5, resources.GetString("GroupBox5.ToolTip2"))
        '
        'lblTag
        '
        resources.ApplyResources(Me.lblTag, "lblTag")
        Me.lblTag.Name = "lblTag"
        Me.ToolTipValues.SetToolTip(Me.lblTag, resources.GetString("lblTag.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.lblTag, resources.GetString("lblTag.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblTag, resources.GetString("lblTag.ToolTip2"))
        '
        'chkActive
        '
        resources.ApplyResources(Me.chkActive, "chkActive")
        Me.chkActive.Image = Global.DWSIM.UnitOperations.My.Resources.Resources.bullet_tick
        Me.chkActive.Name = "chkActive"
        Me.ToolTip1.SetToolTip(Me.chkActive, resources.GetString("chkActive.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.chkActive, resources.GetString("chkActive.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.chkActive, resources.GetString("chkActive.ToolTip2"))
        Me.chkActive.UseVisualStyleBackColor = true
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        Me.ToolTip1.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip2"))
        '
        'GroupBox4
        '
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Controls.Add(Me.rtbAnnotations)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = false
        Me.ToolTipChangeTag.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip1"))
        Me.ToolTip1.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip2"))
        '
        'rtbAnnotations
        '
        resources.ApplyResources(Me.rtbAnnotations, "rtbAnnotations")
        Me.rtbAnnotations.Name = "rtbAnnotations"
        Me.rtbAnnotations.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1046{\fonttbl{\f0\fnil\fcharset20"& _ 
    "4 Microsoft Sans Serif;}}"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"{\*\generator Riched20 10.0.17134}\viewkind4\uc1 "&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"\p"& _ 
    "ard\f0\fs17\lang1049\par"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"}"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        Me.rtbAnnotations.ShowRedo = false
        Me.rtbAnnotations.ShowUndo = false
        Me.ToolTipValues.SetToolTip(Me.rtbAnnotations, resources.GetString("rtbAnnotations.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.rtbAnnotations, resources.GetString("rtbAnnotations.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.rtbAnnotations, resources.GetString("rtbAnnotations.ToolTip2"))
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.lblResult)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.tbExpression)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = false
        Me.ToolTipChangeTag.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip1"))
        Me.ToolTip1.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip2"))
        '
        'lblResult
        '
        resources.ApplyResources(Me.lblResult, "lblResult")
        Me.lblResult.Name = "lblResult"
        Me.ToolTip1.SetToolTip(Me.lblResult, resources.GetString("lblResult.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblResult, resources.GetString("lblResult.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblResult, resources.GetString("lblResult.ToolTip2"))
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip2"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip2"))
        '
        'tbExpression
        '
        resources.ApplyResources(Me.tbExpression, "tbExpression")
        Me.tbExpression.Name = "tbExpression"
        Me.ToolTipValues.SetToolTip(Me.tbExpression, resources.GetString("tbExpression.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.tbExpression, resources.GetString("tbExpression.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.tbExpression, resources.GetString("tbExpression.ToolTip2"))
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.cbSourceProp2)
        Me.GroupBox1.Controls.Add(Me.cbSourceObj2)
        Me.GroupBox1.Controls.Add(Me.lblSourceVal2)
        Me.GroupBox1.Controls.Add(Me.lblSrcVal2)
        Me.GroupBox1.Controls.Add(Me.lblSrcProp2)
        Me.GroupBox1.Controls.Add(Me.lblSrcObj2)
        Me.GroupBox1.Controls.Add(Me.lblTargetVal)
        Me.GroupBox1.Controls.Add(Me.lblSourceVal)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cbTargetProp)
        Me.GroupBox1.Controls.Add(Me.cbTargetObj)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.cbSourceProp)
        Me.GroupBox1.Controls.Add(Me.cbSourceObj)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = false
        Me.ToolTipChangeTag.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip1"))
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip2"))
        '
        'cbSourceProp2
        '
        resources.ApplyResources(Me.cbSourceProp2, "cbSourceProp2")
        Me.cbSourceProp2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSourceProp2.FormattingEnabled = true
        Me.cbSourceProp2.Name = "cbSourceProp2"
        Me.ToolTip1.SetToolTip(Me.cbSourceProp2, resources.GetString("cbSourceProp2.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.cbSourceProp2, resources.GetString("cbSourceProp2.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.cbSourceProp2, resources.GetString("cbSourceProp2.ToolTip2"))
        '
        'cbSourceObj2
        '
        resources.ApplyResources(Me.cbSourceObj2, "cbSourceObj2")
        Me.cbSourceObj2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSourceObj2.FormattingEnabled = true
        Me.cbSourceObj2.Name = "cbSourceObj2"
        Me.ToolTip1.SetToolTip(Me.cbSourceObj2, resources.GetString("cbSourceObj2.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.cbSourceObj2, resources.GetString("cbSourceObj2.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.cbSourceObj2, resources.GetString("cbSourceObj2.ToolTip2"))
        '
        'lblSourceVal2
        '
        resources.ApplyResources(Me.lblSourceVal2, "lblSourceVal2")
        Me.lblSourceVal2.Name = "lblSourceVal2"
        Me.ToolTip1.SetToolTip(Me.lblSourceVal2, resources.GetString("lblSourceVal2.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblSourceVal2, resources.GetString("lblSourceVal2.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblSourceVal2, resources.GetString("lblSourceVal2.ToolTip2"))
        '
        'lblSrcVal2
        '
        resources.ApplyResources(Me.lblSrcVal2, "lblSrcVal2")
        Me.lblSrcVal2.Name = "lblSrcVal2"
        Me.ToolTip1.SetToolTip(Me.lblSrcVal2, resources.GetString("lblSrcVal2.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblSrcVal2, resources.GetString("lblSrcVal2.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblSrcVal2, resources.GetString("lblSrcVal2.ToolTip2"))
        '
        'lblSrcProp2
        '
        resources.ApplyResources(Me.lblSrcProp2, "lblSrcProp2")
        Me.lblSrcProp2.Name = "lblSrcProp2"
        Me.ToolTip1.SetToolTip(Me.lblSrcProp2, resources.GetString("lblSrcProp2.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblSrcProp2, resources.GetString("lblSrcProp2.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblSrcProp2, resources.GetString("lblSrcProp2.ToolTip2"))
        '
        'lblSrcObj2
        '
        resources.ApplyResources(Me.lblSrcObj2, "lblSrcObj2")
        Me.lblSrcObj2.Name = "lblSrcObj2"
        Me.ToolTip1.SetToolTip(Me.lblSrcObj2, resources.GetString("lblSrcObj2.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblSrcObj2, resources.GetString("lblSrcObj2.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblSrcObj2, resources.GetString("lblSrcObj2.ToolTip2"))
        '
        'lblTargetVal
        '
        resources.ApplyResources(Me.lblTargetVal, "lblTargetVal")
        Me.lblTargetVal.Name = "lblTargetVal"
        Me.ToolTip1.SetToolTip(Me.lblTargetVal, resources.GetString("lblTargetVal.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblTargetVal, resources.GetString("lblTargetVal.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblTargetVal, resources.GetString("lblTargetVal.ToolTip2"))
        '
        'lblSourceVal
        '
        resources.ApplyResources(Me.lblSourceVal, "lblSourceVal")
        Me.lblSourceVal.Name = "lblSourceVal"
        Me.ToolTip1.SetToolTip(Me.lblSourceVal, resources.GetString("lblSourceVal.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.lblSourceVal, resources.GetString("lblSourceVal.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.lblSourceVal, resources.GetString("lblSourceVal.ToolTip2"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip2"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip2"))
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip2"))
        '
        'cbTargetProp
        '
        resources.ApplyResources(Me.cbTargetProp, "cbTargetProp")
        Me.cbTargetProp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTargetProp.FormattingEnabled = true
        Me.cbTargetProp.Name = "cbTargetProp"
        Me.ToolTip1.SetToolTip(Me.cbTargetProp, resources.GetString("cbTargetProp.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.cbTargetProp, resources.GetString("cbTargetProp.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.cbTargetProp, resources.GetString("cbTargetProp.ToolTip2"))
        '
        'cbTargetObj
        '
        resources.ApplyResources(Me.cbTargetObj, "cbTargetObj")
        Me.cbTargetObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTargetObj.FormattingEnabled = true
        Me.cbTargetObj.Name = "cbTargetObj"
        Me.ToolTip1.SetToolTip(Me.cbTargetObj, resources.GetString("cbTargetObj.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.cbTargetObj, resources.GetString("cbTargetObj.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.cbTargetObj, resources.GetString("cbTargetObj.ToolTip2"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip2"))
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip2"))
        '
        'cbSourceProp
        '
        resources.ApplyResources(Me.cbSourceProp, "cbSourceProp")
        Me.cbSourceProp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSourceProp.FormattingEnabled = true
        Me.cbSourceProp.Name = "cbSourceProp"
        Me.ToolTip1.SetToolTip(Me.cbSourceProp, resources.GetString("cbSourceProp.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.cbSourceProp, resources.GetString("cbSourceProp.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.cbSourceProp, resources.GetString("cbSourceProp.ToolTip2"))
        '
        'cbSourceObj
        '
        resources.ApplyResources(Me.cbSourceObj, "cbSourceObj")
        Me.cbSourceObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSourceObj.FormattingEnabled = true
        Me.cbSourceObj.Name = "cbSourceObj"
        Me.ToolTip1.SetToolTip(Me.cbSourceObj, resources.GetString("cbSourceObj.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me.cbSourceObj, resources.GetString("cbSourceObj.ToolTip1"))
        Me.ToolTipChangeTag.SetToolTip(Me.cbSourceObj, resources.GetString("cbSourceObj.ToolTip2"))
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        Me.ToolTip1.SetToolTip(Me.Label19, resources.GetString("Label19.ToolTip"))
        Me.ToolTipChangeTag.SetToolTip(Me.Label19, resources.GetString("Label19.ToolTip1"))
        Me.ToolTipValues.SetToolTip(Me.Label19, resources.GetString("Label19.ToolTip2"))
        '
        'ToolTipChangeTag
        '
        Me.ToolTipChangeTag.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTipChangeTag.ToolTipTitle = "Info"
        '
        'EditingForm_Spec
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "EditingForm_Spec"
        Me.ToolTipChangeTag.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.ToolTipValues.SetToolTip(Me, resources.GetString("$this.ToolTip1"))
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip2"))
        Me.GroupBox5.ResumeLayout(false)
        Me.GroupBox5.PerformLayout
        Me.GroupBox4.ResumeLayout(false)
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Public WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Public WithEvents chkActive As System.Windows.Forms.CheckBox
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Public WithEvents rtbAnnotations As Extended.Windows.Forms.RichTextBoxExtended
    Public WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents tbExpression As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents cbSourceProp As System.Windows.Forms.ComboBox
    Public WithEvents cbSourceObj As System.Windows.Forms.ComboBox
    Public WithEvents Label19 As System.Windows.Forms.Label
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents lblTag As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents cbTargetProp As System.Windows.Forms.ComboBox
    Public WithEvents cbTargetObj As System.Windows.Forms.ComboBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents lblTargetVal As System.Windows.Forms.Label
    Public WithEvents lblSourceVal As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents lblResult As System.Windows.Forms.Label
    Public WithEvents lblSourceVal2 As Label
    Public WithEvents lblSrcVal2 As Label
    Public WithEvents lblSrcProp2 As Label
    Public WithEvents lblSrcObj2 As Label
    Public WithEvents cbSourceObj2 As ComboBox
    Public WithEvents cbSourceProp2 As ComboBox
    Friend WithEvents ToolTipChangeTag As ToolTip
End Class
