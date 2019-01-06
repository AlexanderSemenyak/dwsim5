Imports System

Namespace DWSIM.Interfaces
    ''' <summary>
    ''' Property names for MaterialStream and etc.
    ''' </summary>
    Public Class PropertyNames
        Public Class Basis
            Public Const molar As String = NameOf(molar)
            Public Const mass As String = NameOf(mass)
        End Class

        Public Class Phase
            Public Const overall As String = NameOf(overall)
        End Class

        ''' <summary>
        ''' Property names for MaterialStream (Cape OPEN) and etc.
        ''' </summary>
        Public Class MaterialStream
            Public Const compressibilityfactor As String = NameOf(compressibilityfactor)
            Public Const heatcapacity As String = NameOf(heatcapacity)
            Public Const heatcapacitycp As String = NameOf(heatcapacitycp)
            Public Const heatofvaporization As String = NameOf(heatofvaporization)
            Public Const heatcapacitycv As String = NameOf(heatcapacitycv)
            Public Const excessenthalpy As String = NameOf(excessenthalpy)
            Public Const excessentropy As String = NameOf(excessentropy)
            Public Const viscosity As String = NameOf(viscosity)
            Public Const thermalconductivity As String = NameOf(thermalconductivity)
            Public Const fugacity As String = NameOf(fugacity)
            Public Const fugacitycoefficient As String = NameOf(fugacitycoefficient)
            Public Const activitycoefficient As String = NameOf(activitycoefficient)
            Public Const logfugacitycoefficient As String = NameOf(logfugacitycoefficient)
            Public Const density As String = NameOf(density)
            Public Const volume As String = NameOf(volume)
            Public Const enthalpy As String = NameOf(enthalpy)
            Public Const enthalpynf As String = NameOf(enthalpynf)
            Public Const entropy As String = NameOf(entropy)
            Public Const entropynf As String = NameOf(entropynf)
            Public Const enthalpyf As String = NameOf(enthalpyf)
            Public Const entropyf As String = NameOf(entropyf)
            Public Const moles As String = NameOf(moles)
            Public Const mass As String = NameOf(mass)
            Public Const molecularweight As String = NameOf(molecularweight)
            Public Const temperature As String = NameOf(temperature)
            Public Const pressure As String = NameOf(pressure)
            Public Const flow As String = NameOf(flow)
            Public Const fraction As String = NameOf(fraction)
            Public Const phasefraction As String = NameOf(phasefraction)
            Public Const totalflow As String = NameOf(totalflow)
            Public Const kvalue As String = NameOf(kvalue)
            Public Const logkvalue As String = NameOf(logkvalue)
            Public Const surfacetension As String = NameOf(surfacetension)
        End Class
    End Class
End Namespace
