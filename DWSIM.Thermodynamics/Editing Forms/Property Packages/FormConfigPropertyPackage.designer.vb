<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormConfigPropertyPackage
    Inherits FormConfigPropertyPackageBase

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormConfigPropertyPackage))
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.FaTabStrip1 = New FarsiLibrary.Win.FATabStrip()
        Me.FaTabStripItem2 = New FarsiLibrary.Win.FATabStripItem()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.KryptonDataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LabelWithDivider2 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FaTabStripItem1 = New FarsiLibrary.Win.FATabStripItem()
        Me.TabStripUNIFAC = New FarsiLibrary.Win.FATabStripItem()
        Me.IPGrid = New System.Windows.Forms.DataGridView()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.FaTabStrip1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FaTabStrip1.SuspendLayout()
        Me.FaTabStripItem2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.KryptonDataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabStripUNIFAC.SuspendLayout()
        CType(Me.IPGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FaTabStrip1
        '
        Me.FaTabStrip1.AlwaysShowClose = False
        Me.FaTabStrip1.AlwaysShowMenuGlyph = False
        resources.ApplyResources(Me.FaTabStrip1, "FaTabStrip1")
        Me.FaTabStrip1.Items.AddRange(New FarsiLibrary.Win.FATabStripItem() {Me.FaTabStripItem2, Me.FaTabStripItem1, Me.TabStripUNIFAC})
        Me.FaTabStrip1.Name = "FaTabStrip1"
        Me.FaTabStrip1.SelectedItem = Me.FaTabStripItem1
        '
        'FaTabStripItem2
        '
        Me.FaTabStripItem2.CanClose = False
        Me.FaTabStripItem2.Controls.Add(Me.GroupBox2)
        Me.FaTabStripItem2.Controls.Add(Me.Label2)
        Me.FaTabStripItem2.IsDrawn = True
        Me.FaTabStripItem2.Name = "FaTabStripItem2"
        resources.ApplyResources(Me.FaTabStripItem2, "FaTabStripItem2")
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.Button3)
        Me.GroupBox2.Controls.Add(Me.KryptonDataGridView2)
        Me.GroupBox2.Controls.Add(Me.LabelWithDivider2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Button3
        '
        resources.ApplyResources(Me.Button3, "Button3")
        Me.Button3.Name = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'KryptonDataGridView2
        '
        Me.KryptonDataGridView2.AllowUserToAddRows = False
        Me.KryptonDataGridView2.AllowUserToDeleteRows = False
        Me.KryptonDataGridView2.AllowUserToResizeColumns = False
        Me.KryptonDataGridView2.AllowUserToResizeRows = False
        resources.ApplyResources(Me.KryptonDataGridView2, "KryptonDataGridView2")
        Me.KryptonDataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.KryptonDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.KryptonDataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column3, Me.Column4, Me.Column5})
        Me.KryptonDataGridView2.Name = "KryptonDataGridView2"
        Me.KryptonDataGridView2.RowHeadersVisible = False
        Me.KryptonDataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N5"
        Me.KryptonDataGridView2.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.KryptonDataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        '
        'Column3
        '
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle4
        resources.ApplyResources(Me.Column3, "Column3")
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle5
        resources.ApplyResources(Me.Column4, "Column4")
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        resources.ApplyResources(Me.Column5, "Column5")
        Me.Column5.Name = "Column5"
        '
        'LabelWithDivider2
        '
        resources.ApplyResources(Me.LabelWithDivider2, "LabelWithDivider2")
        Me.LabelWithDivider2.Name = "LabelWithDivider2"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'FaTabStripItem1
        '
        Me.FaTabStripItem1.CanClose = False
        Me.FaTabStripItem1.IsDrawn = True
        Me.FaTabStripItem1.Name = "FaTabStripItem1"
        Me.FaTabStripItem1.Selected = True
        resources.ApplyResources(Me.FaTabStripItem1, "FaTabStripItem1")
        '
        'TabStripUNIFAC
        '
        Me.TabStripUNIFAC.CanClose = False
        Me.TabStripUNIFAC.Controls.Add(Me.IPGrid)
        Me.TabStripUNIFAC.IsDrawn = True
        Me.TabStripUNIFAC.Name = "TabStripUNIFAC"
        resources.ApplyResources(Me.TabStripUNIFAC, "TabStripUNIFAC")
        '
        'IPGrid
        '
        Me.IPGrid.AllowUserToAddRows = False
        Me.IPGrid.AllowUserToDeleteRows = False
        Me.IPGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.IPGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.IPGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.IPGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column6})
        resources.ApplyResources(Me.IPGrid, "IPGrid")
        Me.IPGrid.MultiSelect = False
        Me.IPGrid.Name = "IPGrid"
        Me.IPGrid.ReadOnly = True
        '
        'Column6
        '
        resources.ApplyResources(Me.Column6, "Column6")
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'FormConfigPropertyPackage
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.FaTabStrip1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "FormConfigPropertyPackage"
        Me.TopMost = True
        CType(Me.FaTabStrip1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FaTabStrip1.ResumeLayout(False)
        Me.FaTabStripItem2.ResumeLayout(False)
        Me.FaTabStripItem2.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.KryptonDataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabStripUNIFAC.ResumeLayout(False)
        CType(Me.IPGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents KryptonDataGridView2 As System.Windows.Forms.DataGridView
    Public WithEvents LabelWithDivider2 As System.Windows.Forms.Label
    Private WithEvents FaTabStrip1 As FarsiLibrary.Win.FATabStrip
    Private WithEvents FaTabStripItem1 As FarsiLibrary.Win.FATabStripItem
    Private WithEvents FaTabStripItem2 As FarsiLibrary.Win.FATabStripItem
    Public WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Public WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Public WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Public WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents IPGrid As System.Windows.Forms.DataGridView
    Private WithEvents TabStripUNIFAC As FarsiLibrary.Win.FATabStripItem
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
