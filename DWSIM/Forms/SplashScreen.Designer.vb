<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SplashScreen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SplashScreen))
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.LabelLicense = New System.Windows.Forms.Label()
        Me.lblCopyright = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblAppVersion = New System.Windows.Forms.Label()
        Me.lblFrameworkVersion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblVersion
        '
        resources.ApplyResources(Me.lblVersion, "lblVersion")
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.lblVersion.Name = "lblVersion"
        '
        'LabelLicense
        '
        resources.ApplyResources(Me.LabelLicense, "LabelLicense")
        Me.LabelLicense.BackColor = System.Drawing.Color.Transparent
        Me.LabelLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.LabelLicense.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.LabelLicense.Name = "LabelLicense"
        '
        'lblCopyright
        '
        resources.ApplyResources(Me.lblCopyright, "lblCopyright")
        Me.lblCopyright.BackColor = System.Drawing.Color.Transparent
        Me.lblCopyright.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.lblCopyright.Name = "lblCopyright"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Name = "Label1"
        '
        'lblAppVersion
        '
        resources.ApplyResources(Me.lblAppVersion, "lblAppVersion")
        Me.lblAppVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblAppVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblAppVersion.ForeColor = System.Drawing.Color.White
        Me.lblAppVersion.Name = "lblAppVersion"
        '
        'lblFrameworkVersion
        '
        resources.ApplyResources(Me.lblFrameworkVersion, "lblFrameworkVersion")
        Me.lblFrameworkVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblFrameworkVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblFrameworkVersion.ForeColor = System.Drawing.Color.White
        Me.lblFrameworkVersion.Name = "lblFrameworkVersion"
        '
        'SplashScreen
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DWSIM.My.Resources.Resources.DWSIM_splash_limpo
        Me.ControlBox = False
        Me.Controls.Add(Me.lblFrameworkVersion)
        Me.Controls.Add(Me.lblAppVersion)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCopyright)
        Me.Controls.Add(Me.LabelLicense)
        Me.Controls.Add(Me.lblVersion)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashScreen"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblVersion As System.Windows.Forms.Label
    Public WithEvents LabelLicense As System.Windows.Forms.Label
    Public WithEvents lblCopyright As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblAppVersion As System.Windows.Forms.Label
    Public WithEvents lblFrameworkVersion As System.Windows.Forms.Label

End Class
