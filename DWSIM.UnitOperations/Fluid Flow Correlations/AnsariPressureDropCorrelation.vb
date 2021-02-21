Imports DWSIM.Interfaces.My.Resources

Namespace FlowPackages

    <Serializable()> Public Class AnsariPressureDropCorrelation
        Inherits FPBaseClass

        Public Const kErrGradcalc = 520 + vbObjectError
        Public Const const_conver_day_sec = 86400   ' updated for test  rnt21
        Public Const const_conver_sec_day = 1 / const_conver_day_sec
        Public Const const_g = 9.81

        Public Overrides Function CalculateDeltaP(D As Double, L As Double, deltaz As Double, k As Double, qv As Double, ql As Double, muv As Double, mul As Double, rhov As Double, rhol As Double, surft As Double, ByVal pressureIn As Double) As Object
            
            Dim IObj As Inspector.InspectorItem = Inspector.Host.GetNewInspectorItem()

            IObj?.Paragraphs.Add(SolutionInspector.Input_Parameters)

            IObj?.Paragraphs.Add("<mi>Pin</mi> = " & pressureIn & " Pa")
            IObj?.Paragraphs.Add("<mi>D</mi> = " & D & " m")
            IObj?.Paragraphs.Add("<mi>L</mi> = " & L & " m")
            IObj?.Paragraphs.Add("<mi>H</mi> = " & deltaz & " m")
            IObj?.Paragraphs.Add("<mi>k</mi> = " & k & " m")
            IObj?.Paragraphs.Add("<mi>Q_V</mi> = " & qv & " m3/d actual")
            IObj?.Paragraphs.Add("<mi>Q_L</mi> = " & ql & " m3/d actual")
            IObj?.Paragraphs.Add("<mi>\mu _V</mi> = " & muv & " cP")
            IObj?.Paragraphs.Add("<mi>\mu _L</mi> = " & mul & " cP")
            IObj?.Paragraphs.Add("<mi>\rho _V</mi> = " & rhov & " kg/m3")
            IObj?.Paragraphs.Add("<mi>\rho _L</mi> = " & rhol & " kg/m3")
            IObj?.Paragraphs.Add("<mi>\sigma</mi> = " & surft & " N/m")

            IObj?.Paragraphs.Add(SolutionInspector.Calculated_Parameters)

            CalculateDeltaP = Nothing

            Dim ResVector(4) As Object

            If qv = 0.0# Then

                ResVector = Me.CalculateDeltaPLiquid(D, L, deltaz, k, ql, mul, rhol)
                CalculateDeltaP = ResVector

            ElseIf ql = 0.0# Then

                ResVector = Me.CalculateDeltaPGas(D, L, deltaz, k, qv, muv, rhov)
                CalculateDeltaP = ResVector

            Else
                'IObj?.Paragraphs.Add("<mi>D</mi> = " & D & " m")
                'IObj?.Paragraphs.Add("<mi>L</mi> = " & L & " m")
                'IObj?.Paragraphs.Add("<mi>H</mi> = " & deltaz & " m")
                'IObj?.Paragraphs.Add("<mi>k</mi> = " & k & " m")
                'IObj?.Paragraphs.Add("<mi>Q_V</mi> = " & qv & " m3/d actual")
                'IObj?.Paragraphs.Add("<mi>Q_L</mi> = " & ql & " m3/d actual")
                'IObj?.Paragraphs.Add("<mi>\mu _V</mi> = " & muv & " cP")
                'IObj?.Paragraphs.Add("<mi>\mu _L</mi> = " & mul & " cP")
                'IObj?.Paragraphs.Add("<mi>\rho _V</mi> = " & rhov & " kg/m3")
                'IObj?.Paragraphs.Add("<mi>\rho _L</mi> = " & rhol & " kg/m3")
                'IObj?.Paragraphs.Add("<mi>\sigma</mi> = " & surft & " N/m")

                ' d_m - диаметр трубы в которой идет поток
                ' p_atma - давление в точке расчета
                ' Ql_rc_m3day - дебит жидкости в рабочих условиях
                ' Qg_rc_m3day - дебит газа в рабочих условиях
                ' mu_oil_cP - вязкость нефти в рабочих условиях
                ' mu_gas_cP - вязкость газа в рабочих условиях
                ' sigma_oil_gas_Nm - поверхностное натяжение
                '              жидкость газ
                ' rho_lrc_kgm3 - плотность нефти
                ' rho_grc_kgm3 - плотность газа
                ' eps_m     - шероховатость
                ' theta_deg - угол от горизонтали
                ' hcorr  - тип корреляции
                ' param_out - параметр для вывода
                ' c_calibr_grav - калибровка гравитации
                ' c_calibr_fric - калибровка трения
                'description_end

                Dim d_m = D
                Dim theta_deg = Math.Atan(deltaz / (L ^ 2 - deltaz ^ 2) ^ 0.5) * 180 / Math.PI
                Dim eps_m = k
                Dim Ql_rc_m3day = ql
                Dim Qg_rc_m3day = qv
                Dim mu_oil_cP = mul
                Dim mu_gas_cP = muv
                Dim sigma_oil_gas_Nm = surft
                Dim rho_lrc_kgm3 = rhol
                Dim rho_grc_kgm3 = rhov
                Dim p_atma = pressureIn/101325#  ' Pa to atm
                DIm c_calibr_grav = 1
                DIm c_calibr_fric = 1

                Dim PrGrad = unf_AnsariGradient(d_m, theta_deg, eps_m, Ql_rc_m3day, Qg_rc_m3day, mu_oil_cP, mu_gas_cP, sigma_oil_gas_Nm,rho_lrc_kgm3,rho_grc_kgm3, p_atma, c_calibr_grav, c_calibr_fric)
                'PrGrad(0)=dPdLg_out_atmm * c_calibr_grav + dPdLf_out_atmm * c_calibr_fric   'Delta P_{total} atm
                'PrGrad(1)=dPdLg_out_atmm * c_calibr_grav ' Delta P_{elevation} atm 
                'PrGrad(2)=dPdLf_out_atmm * c_calibr_fric ' Delta P_{friction} atm
                'PrGrad(3)=dPdLa_out_atmm ' equals (2) without correct
                'PrGrad(4)=Vsl_msec
                'PrGrad(5)=Vsg_msec
                'PrGrad(6)=Hl_out_fr 'Liquid holdup fraction
                'PrGrad(7)=fpat_out_num


                Dim fpat_out_num = PrGrad(7) 'flow regime code
                Select Case fpat_out_num
                    Case 100: fpat_out_num = " liq" '= liquid
                    Case 101: fpat_out_num = " gas" ' = gas
                    Case 105: fpat_out_num = "anul"
                    Case 104: fpat_out_num = "dbub"' = dispersed bubble
                    Case 103: fpat_out_num = "slug" '= slug
                    Case 102: fpat_out_num = "bubl" '= bubbly
                    Case 199: fpat_out_num = "  na" '
                End Select '(fpat)


                ResVector(0) = fpat_out_num 'flow regime 
                ResVector(1) = PrGrad(6) 'Liquid holdup fraction
                ResVector(2) = PrGrad(2) * 101325.0 * L ' Delta P_{friction} Pa
                ResVector(3) = PrGrad(1) * 101325.0 * L ' Delta P_{elevation} Pa
                ResVector(4) = PrGrad(1) * L * 101325# + PrGrad(2)* L * 101325# ' Delta P_{total} Pa

                CalculateDeltaP = ResVector

                IObj?.Paragraphs.Add(SolutionInspector.Flow_Regime & ResVector(0))
                IObj?.Paragraphs.Add("<mi>e_L</mi> = " & ResVector(1))
                IObj?.Paragraphs.Add("<mi>\Delta P_{friction}</mi> = " & ResVector(2) & " Pa")
                IObj?.Paragraphs.Add("<mi>\Delta P_{elevation}</mi> = " & ResVector(3) & " Pa")
                IObj?.Paragraphs.Add("<mi>\Delta P_{total}</mi> = " & ResVector(4) & " Pa")


            End If
        End Function


Public Function unf_AnsariGradient(ByVal arr_d_m As Double, _
                                  ByVal arr_theta_deg As Double, ByVal eps_m As Double, _
                                  ByVal Ql_rc_m3day As Double, ByVal Qg_rc_m3day As Double, _
                                  ByVal Mul_rc_cP As Double, ByVal Mug_rc_cP As Double, _
                                  ByVal sigma_l_Nm As Double, _
                                  ByVal rho_lrc_kgm3 As Double, _
                                  ByVal rho_grc_kgm3 As Double, _
                                  ByVal p_atma As Double, _
                                  Optional c_calibr_grav As Double = 1, _
                                  Optional c_calibr_fric As Double = 1) As Object
  
     Dim dPdLg_out_atmm As Double
     Dim dPdLf_out_atmm As Double
     Dim Hl_out_fr As Double
     Dim fpat_out_num as Object
     Dim dPdLa_out_atmm As Double
     Dim dPdL_out_atmm As Double
     Dim pgf_out_atmm As Double
     Dim pge_out_atmm As Double
     Dim pga_out_atmm As Double
     Dim pgt_out_atmm As Double
    
     ' znlf - calculates zero net liquid flow - gas flow through liquid column
     '=================
     
     Dim roughness_d As Double
     Dim Ap_m2 As Double ' площадь трубы
     Dim lambda_l   As Double
     Dim Vsl_msec As Double, Vsg_msec As Double
     Dim flow_pattern As Integer
     Dim iErr
     Dim ang1 As Double
     Dim timeStamp
     
    Dim IObj As Inspector.InspectorItem = Inspector.Host.CurrentItem

     'timeStamp = Time()
On Error GoTo err1:
     roughness_d = eps_m / arr_d_m
     Ap_m2 = Math.PI * arr_d_m ^ 2 / 4
     If Ql_rc_m3day = 0 Then
        lambda_l = 1
     Else
         lambda_l = Ql_rc_m3day / (Ql_rc_m3day + Qg_rc_m3day)
     End If
     Vsl_msec = const_conver_sec_day * Ql_rc_m3day / Ap_m2 'alexander https://en.wikipedia.org/wiki/Superficial_velocity
     Vsg_msec = const_conver_sec_day * Qg_rc_m3day / Ap_m2 
     ang1 = arr_theta_deg
     If arr_theta_deg < 0 Then
        ' Ansari not working for downward flow
         IObj?.Paragraphs.Add("AnsariGradient: arr_theta_deg = " & arr_theta_deg & " negative. Ansari not for downward flow. Angle inverted")
        arr_theta_deg = -arr_theta_deg
     End If
     If arr_theta_deg < 75 Then
        ang1 = 75
     '   addLogMsg "AnsariGradient: arr_theta_deg = " & arr_theta_deg & " less than 75 degrees. 75 used for calc"
     End If
     If arr_theta_deg > 90 Then
        ang1 = 90
         IObj?.Paragraphs.Add("AnsariGradient: arr_theta_deg = " & arr_theta_deg & " greater than 90 degrees. 90 deg used for calc")
     End If
     Call Ansari(ang1, arr_d_m, roughness_d, p_atma, Vsl_msec, Vsg_msec, lambda_l, _
                        rho_grc_kgm3, rho_lrc_kgm3, Mug_rc_cP, Mul_rc_cP, _
                        sigma_l_Nm, _
                        Hl_out_fr, pgf_out_atmm, pge_out_atmm, pga_out_atmm, pgt_out_atmm, fpat_out_num)
      
     dPdL_out_atmm = (pge_out_atmm * Math.Sin(arr_theta_deg * Math.PI / 180) / Math.Sin(ang1 * Math.PI / 180) + pgf_out_atmm + pga_out_atmm)
     dPdLg_out_atmm = pge_out_atmm * Math.Sin(arr_theta_deg * Math.PI / 180) / Math.Sin(ang1 * Math.PI / 180)
     dPdLf_out_atmm = pgf_out_atmm
     dPdLa_out_atmm = pga_out_atmm
      
               Select Case fpat_out_num
                  Case " liq": fpat_out_num = 100 ' " liq" = liquid
                  Case " gas": fpat_out_num = 101 ' " gas" = gas
                  Case "anul": fpat_out_num = 105 ' "anul" = annular
                  Case "dbub": fpat_out_num = 104 ' "dbub" = dispersed bubble
                  Case "slug": fpat_out_num = 103 ' "slug" = slug
                  Case "bubl": fpat_out_num = 102 ' "bubl" = bubbly
                  Case "  na": fpat_out_num = 199
              End Select '(fpat)
      
      
    Dim ResVector(8) As Object
    ResVector(0)=dPdLg_out_atmm * c_calibr_grav + dPdLf_out_atmm * c_calibr_fric
    ResVector(1)=dPdLg_out_atmm * c_calibr_grav
    ResVector(2)=dPdLf_out_atmm * c_calibr_fric
    ResVector(3)=dPdLa_out_atmm
    ResVector(4)=Vsl_msec
    ResVector(5)=Vsg_msec
    ResVector(6)=Hl_out_fr
    ResVector(7)=fpat_out_num

    'unf_AnsariGradient = new Array(dPdLg_out_atmm * c_calibr_grav + dPdLf_out_atmm * c_calibr_fric, _
                                'dPdLg_out_atmm * c_calibr_grav, _
                                'dPdLf_out_atmm * c_calibr_fric, _
                                'dPdLa_out_atmm, _
                                'Vsl_msec, _
                                'Vsg_msec, _
                                'Hl_out_fr, _
                                'fpat_out_num)
    unf_AnsariGradient = ResVector
    Exit Function
err1:
    IObj?.Paragraphs.Add(Err.Description)
    unf_AnsariGradient = "error"
End Function


'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     comprehensive mechanistic model for pressure gradient, liquid
'     holdup and flow pattern predictions
'     written by,    asfandiar m. ansari
'     revised by,    asfandiar m. ansari
'     revised by,    tuffp                  last revision: november 89
'              * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     this subroutine calculates two phase liquid holdup, flow pattern
'     and pressure gradient using the mechanistic approach developed from
'     the separate models for flow pattern prediction and flow behavior
'     prediction of the individual flow patterns. the english system of
'     units is used for the input data but converted to si units for the
'     subsequent calculations.
'                               reference
'                               ---------
'     1.  ansari, a. m., " mechanistic model for two-phase upward flow."
'         m.s thesis, the university of tulsa (1988).
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                   input/output logical file variables
'                   -----------------------------------
'     ioerr = output file for error messages when input values passed
'             to the subroutine are out of range or error occurs in
'             the calculation.
'                          subsubroutines called
'                          ------------------
'     upfpdet = this subroutine predicts flow pattern only for upward
'               flow using taitel, barnea, & dukler model.
'     single = this subroutine calculates pressure gradient for single
'              phase flow of liquid or gas.
'     bubble = this subroutine calculates pressure gradient both for
'              dispersed bubble and bubbly flows.
'     slug   = this subroutine calculates pressure gradient for slug
'              flow.
'     anmist = this subroutine calculates pressure gradient for
'              annular-mist flow.
'                       variable description
'                       --------------------
'     *ang   = angle of flow from horizontal. (deg.)
'      angr  = angle of flow from horizontal. (rad)
'     *deng  = gas density. (lbm/ft^3)
'     *denl  = liquid density. (lbm/ft^3)
'     *di    = inside pipe diameter. (m)
'      e     = liquid holdup fraction.
'     *ed    = relative pipe roughness.
'     *ens   = no-slip liquid holdup fraction.
'      fpat  = flow pattern, (chr)
'                 " liq" = liquid
'                 " gas" = gas
'                 "bubl" = bubbly
'                 "slug" = slug
'                 "dbub" = dispersed bubble
'                 "anul" = annular
'     *p     = pressure. (psia)
'      pga   = acceleration pressure gradient. (psi/ft)
'      pge   = elevation pressure gradient. (psi/ft)
'      pgf   = friction pressure gradient. (psi/ft)
'      pgt   = total pressure gradient. (psi/ft)
'     *surl  = gas-liquid surface tension. (dynes/cm)
'     *visg  = gas viscosity. (cp)
'     *visl  = liquid viscosity. (cp)
'     *vm    = mixture velocity. (ft/sec)
'      vsg   = superficial gas velocity. (ft/sec)
'      vsl   = superficial liquid velocity. (ft/sec)
'      (*indicates input variables)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Private Sub Ansari(ByRef ang#, ByRef di_m#, ByRef ed#, ByRef p_atma#, ByRef vsl_m3sec#, ByRef vsg_m3sec#, ByRef ens#, ByRef deng_kgm3#, ByRef denl_kgm3#, _
                   ByRef visg#, ByRef visl#, ByRef surl#, ByRef e#, ByRef pgf#, ByRef pge#, ByRef pga#, ByRef pgt#, byref fpat As string, _
            Optional ByVal znlf As Boolean = False)

    '     --------------------------------
    '     initialize the output variables.
    '     --------------------------------
    e = 0#
    pgf = 0#
    pge = 0#
    pga = 0#
    pgt = 0#
    fpat = "    "
    '     --------------------------------------
    '     convert input variables into si units.
    '     --------------------------------------
    Dim angr As Double, p_Pa As Double
    angr = ang * 3.1416 / 180#
    p_Pa = p_atma * 101325# '/ 14.7
    visg = visg * 0.001
    visl = visl * 0.001
    '     ------------------------------------------
    '     check for single phase gas or liquid flow.
    '     ------------------------------------------
    If (ens > 0.99999) Then '        single phase liquid flow.
        fpat = " liq"
        Call single1(angr, di_m, ed, vsl_m3sec, denl_kgm3, visl, p_Pa, pgf, pge, pga, pgt)
        e = 1#
    ElseIf (ens < 0.00001) And Not znlf Then        '        single phase gas flow.
        fpat = " gas"
        Call single1(angr, di_m, ed, vsg_m3sec, deng_kgm3, visg, p_Pa, pgf, pge, pga, pgt)
        e = 0#
    Else
        '        -----------------------------------------------------------
        '        determine flow pattern using taitel, barnea & dukler model.
        '        -----------------------------------------------------------
        Call fpup(vsl_m3sec, vsg_m3sec, di_m, ed, denl_kgm3, deng_kgm3, visl, visg, ang, surl, fpat)
        If (fpat = "anul") Then '           annular-mist flow exists.
            Call anmist(angr, di_m, ed, denl_kgm3, deng_kgm3, visl, visg, vsl_m3sec, vsg_m3sec, _
                               surl, fpat, e, pgf, pge, pga, pgt)
            If (fpat = "slug") Then '              annular flow not confirmed. slug flow persists.
                Call slug(angr, di_m, ed, denl_kgm3, deng_kgm3, visl, visg, vsl_m3sec, vsg_m3sec, _
                                    surl, e, pgf, pge, pga, pgt)
            End If
        ElseIf (fpat = "slug") Then '           slug flow exists.
            Call slug(angr, di_m, ed, denl_kgm3, deng_kgm3, visl, visg, vsl_m3sec, vsg_m3sec, _
                             surl, e, pgf, pge, pga, pgt)
        ElseIf (fpat = "bubl" Or fpat = "dbub") Then '           bubble flow exists.
            Call bubble(angr, di_m, ed, denl_kgm3, deng_kgm3, visl, visg, vsl_m3sec, vsg_m3sec, _
                                   surl, fpat, e, pgf, pge, pga, pgt)
        Else
            fpat = "  na"
            Throw New Exception("ansari: error in flow pattern detection")
            Exit Sub
        End If
    End If
    '     -----------------------------------------------------------
    '     convert pressure gradients and diameter into english units.
    '     -----------------------------------------------------------
    pge = pge / 101325#
    pgf = pgf / 101325#
    pga = pga / 101325#
    pgt = pgt / 101325#
    visg = visg * 1000#
    visl = visl * 1000#
L999:

End Sub

        '================================================ Ansari =======================================================================

'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     mechanistic model for pressure gradient in single phase (liquid
'     or gas) flow.
'     written by,    asfandiar m. ansari
'     revised by,    asfandiar m. ansari     last revision: march 1989
'              * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     this subroutine calculates single phase pressure gradient using
'     simple mechanistic approach. an explicit equation developed by
'     zigrang and sylvester is used for friction factor.the si system
'     of units is used.
'                       variable description
'                       --------------------
'     *angr  = angle of flow from horizontal. (rad)
'     *den   = density of liquid or gas. (kg/cum)
'     *di    = inside pipe diameter. (m)
'     *ed    = relative pipe roughness.
'      ekk   = kinetic energy term used to determine if critical flow
'              exists.
'      ff    = friction factor.
'      icrit = critical flow indicator (0-noncritical, 1-critical)
'      ierr  = error code. (0=ok, 1=input variables out of range,
'              2=extrapolation of correlation occurring)
'     *ioerr = output file for error messages when input values
'              passed to the subroutine are out of range.
'     *p     = pressure. (pa)
'      pga   = acceleration pressure gradient. (pa/m)
'      pge   = elevation pressure gradient. (pa/m)
'      pgf   = friction pressure gradient. (pa/m)
'      pgt   = total pressure gradient. (pa/m)
'      re    = reynolds number for liquid or gas.
'     *vis   = viscosity. of liquid or gas (kg/m-s)
'     *v     = velocity. of liquid or gas (m/s)
'      (*indicates input variables)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


Private Sub single1(ByRef angr#, ByRef Di#, ByRef ed#, ByRef V#, ByRef den#, ByRef vis#, ByRef p#, ByRef pgf#, ByRef pge#, ByRef pga#, ByRef pgt#)
    Dim Re As Double, FF As Double
    Dim ekk As Double
    
    pge = den * Math.Sin(angr) * 9.81 '     calculate elevation pressure gradient.
    If V > 0 Then
        Re = Di * den * V / vis
        FF = unf_friction_factor(Re, ed, 2) '     calculate frictional pressure gradient.
    Else
        FF = 0
    End If
    pgf = 0.5 * den * FF * V * V / Di
    ekk = den * V * V / p
    If (ekk > 0.95) Then ekk = 0.95
    pgt = (pge + pgf) / (1# - ekk)
    pga = pgt * ekk '     calculate accelerational pressure gradient.
    pgt = (pge + pgf)
End Sub

Function unf_friction_factor(ByVal n_re As Double, _
                             ByVal roughness_d As Double, _
                    Optional ByVal friction_corr_type As Integer = 3, _
                    Optional ByVal smoth_transition As Boolean = False)
    'Calculates friction factor given pipe relative roughness and Reinolds number
    'Parameters
    'n_re - Reinolds number
    'roughness_d - pipe relative roughness
    'friction_corr_type - flag indicating correlation type selection
    ' 0 - Colebrook equation solution
    ' 1 - Drew correlation for smooth pipes
    '
    
    Dim f_n, f_n_new, f_int As Double
    Dim i As Integer
    Dim ed As Double
    Dim Svar As Double
    Dim f_1 As Double
    Dim Re_save As Double
    Const lower_Re_lim = 2000#
    Const upper_Re_lim = 4000#
    
    ed = roughness_d
    
    If n_re = 0 Then
        f_n = 0
    ElseIf n_re < lower_Re_lim Then 'laminar flow
        f_n = 64 / n_re
    Else 'turbulent flow
        Re_save = -1
        If smoth_transition And (n_re > lower_Re_lim And n_re < upper_Re_lim) Then
        ' be ready to interpolate for smooth transition
            Re_save = n_re
            n_re = upper_Re_lim
        End If
        Select Case friction_corr_type
            Case 0
                'calculate friction factor for rough pipes according to Moody method - Payne et all modification for Beggs&Brill correlation
                ' Zigrang and Sylvester  1982  https://en.wikipedia.org/wiki/Darcy_friction_factor_formulae
                f_n = (2 * Math.Log10(2 / 3.7 * ed - 5.02 / n_re * Math.Log10(2 / 3.7 * ed + 13 / n_re))) ^ -2
                  
                i = 0
                Do 'iterate until error in friction factor is sufficiently small
                       'https://en.wikipedia.org/wiki/Darcy_friction_factor_formulae
                       ' expanded form  of the Colebrook equation
                      f_n_new = (1.7384 - 2 * Math.Log10(2 * ed + 18.574 / (n_re * f_n ^ 0.5))) ^ -2
                      i = i + 1
                      f_int = f_n
                      f_n = f_n_new
                      'stop when error is sufficiently small or max number of iterations exceedied
                Loop Until (Math.Abs(f_n_new - f_int) <= 0.001 Or i > 19)
            Case 1
                'Calculate friction factor for smooth pipes using Drew correlation - original Begs&Brill with no modification
                f_n = 0.0056 + 0.5 * n_re ^ -0.32
            
            Case 2
                ' Zigrang and Sylvester  1982  https://en.wikipedia.org/wiki/Darcy_friction_factor_formulae
                f_n = (2 * Math.Log10(1 / 3.7 * ed - 5.02 / n_re * Math.Log10(1 / 3.7 * ed + 13 / n_re))) ^ -2
            Case 3
                ' Brkic shows one approximation of the Colebrook equation based on the Lambert W-function
                '  Brkic, Dejan (2011). "An Explicit Approximation of Colebrook's equation for fluid flow friction factor" (PDF). Petroleum Science and Technology. 29 (15): 1596–1602. doi:10.1080/10916461003620453
                ' http://hal.archives-ouvertes.fr/hal-01586167/file/article.pdf
                ' https://en.wikipedia.org/wiki/Darcy_friction_factor_formulae
                ' http://www.math.bas.bg/infres/MathBalk/MB-26/MB-26-285-292.pdf
                Svar = Math.Log(n_re / (1.816 * Math.Log(1.1 * n_re / (Math.Log(1 + 1.1 * n_re)))))
                f_1 = -2 * Math.Log10(ed / 3.71 + 2 * Svar / n_re)
                f_n = 1 / (f_1 ^ 2)
            Case 4
                ' from unified TUFFP model
                ' Haaland equation   Haaland, SE (1983). "Simple and Explicit Formulas for the Friction Factor in Turbulent Flow". Journal of Fluids Engineering. 105 (1): 89–90. doi:10.1115/1.3240948
                ' with smooth transition zone
                Dim fr2 As Double, fr3 As Double
                fr2 = 16# / 2000#
                fr3 = 1# / (3.6 * Math.Log10(6.9 / 3000# + (ed / 3.7) ^ 1.11)) ^ 2
                If n_re = 0 Then
                    f_n = 0
                ElseIf (n_re < 2000#) Then
                    f_n = 16# / n_re
                ElseIf (n_re > 3000#) Then
                    f_n = 1# / (3.6 * Math.Log10(6.9 / n_re + (ed / 3.7) ^ 1.11)) ^ 2
                ElseIf (n_re >= 2000# And n_re <= 3000#) Then
                    f_n = fr2 + (fr3 - fr2) * (n_re - 2000#) / 1000#
                End If
                f_n = 4 * f_n
            Case 5
                ' from unified TUFFP model
                ' Haaland equation   Haaland, SE (1983). "Simple and Explicit Formulas for the Friction Factor in Turbulent Flow". Journal of Fluids Engineering. 105 (1): 89–90. doi:10.1115/1.3240948

                f_n = 4# / (3.6 * Math.Log10(6.9 / n_re + (ed / 3.7) ^ 1.11)) ^ 2



        End Select
        
        Dim x1 As Double, x2 As Double, y1 As Double, y2 As Double
        
        If smoth_transition And Re_save > 0 Then
          x1 = lower_Re_lim
          y1 = 64# / lower_Re_lim
          x2 = n_re
          y2 = f_n
          f_n = ((y2 - y1) * Re_save + (y1 * x2 - y2 * x1)) / (x2 - x1)
        End If
    
    End If
    
    unf_friction_factor = f_n

End Function


        Private Sub fpup(ByRef vsl#, ByRef vsg#, ByRef Di#, ByRef ed#, ByRef denl#, ByRef deng#, ByRef visl#, ByRef visg#, ByRef ang#, ByRef surl#, byref fpat As string)
            ' sub parameter : do not dim ! Dim fpat as string * 4
            Dim alfa#, vsgo#, vsg1#, vsl1#, vsg2#, vsl2#, vsg3#, vsl3#
            Dim vslb As Double
            Dim vsgb As Double
    
            alfa = 0.0174533 * ang
            Call mpoint(Di, ed, denl, deng, visl, visg, ang, surl, vsgo, vsg1, vsl1, vsg2, vsl2, vsg3, vsl3) '     calculate points on transition boundaries
            '     ----------------------
            '     check for annular flow
            '     ----------------------
            If Not (vsg < vsg3) Then
                fpat = "anul"
                Exit Sub
            End If
            '     -----------------------------------------
            '     check for bubble/slug or dispersed-bubble
            '     -----------------------------------------
            If Not (vsg > vsg2) Then
                Call dbtran(0#, vslb, vsg, Di, ed, denl, deng, visl, visg, ang, surl)
                If (vsl < vslb) Then
                    If (vsgo > 0#) Then
                        vsgb = (vsl + 1.15 * (const_g * (denl - deng) * surl / denl ^ 2) ^ 0.25 * Math.Sin(alfa)) / 3#
                        If (vsg > vsgb) Then
                            fpat = "slug"
                        Else
                            fpat = "bubl"
                        End If
                    Else
                        fpat = "slug"
                    End If
                Else
                    fpat = "dbub"
                End If
            Else ' (vsg > vsg2)
                vslb = vsg / 0.76 - vsg
                If (vsl >= vslb) Then
                    fpat = "dbub"
                Else
                    fpat = "slug"
                End If
            End If
        End Sub

Private Sub anmist(ByRef angr#, ByRef Di#, ByRef ed#, ByRef denl#, ByRef deng#, ByRef visl#, ByRef visg#, ByRef vsl#, ByRef vsg#, ByRef surl#, byref fpat  As string, ByRef e#, ByRef pgf#, ByRef pge#, ByRef pga#, ByRef pgt#)
    
    Dim nf As Double
    Dim NC As Double
    '     --------------------------------------
    '     calculate fe using wallis correlation.
    '     --------------------------------------
    Dim X As Double, fe As Double
    Dim c As Double
    Dim alfc As Double, vsc#, denc#, visc#, recs#, ffcs#, rels#, ffls#, relf#, fflf#, a#
    Dim pgfcs#, pgfls#, xm2#, xmo2#, ym#
    Dim deldmx#, deldmn#, deldac#, deld#, ec#, phic2#, phif2#
    
    X = (deng / denl) ^ 0.5 * 10000# * vsg * visg / surl
    fe = 1# - Math.Exp(-0.125 * (X - 1.5))
    If (fe <= 0#) Then fe = 0#
    If (fe >= 1#) Then fe = 1#
    '     -----------------------------------------------------------
    '     use appropriate correlation factor for interfacial friction
    '     according to the entrainment fraction.
    '     -----------------------------------------------------------
    If (fe > 0.9) Then
        c = 300# '        use wallis correlation factor.
    Else
        c = 24# * (denl / deng) ^ (1# / 3#) '        use whalley correlation factor.
    End If
    '     ---------------------------------------------------
    '     calculate superficial pressure gradients for entire
    '     liquid and gas-liquid core.
    '     ---------------------------------------------------
    alfc = 1# / (1# + fe * vsl / vsg)
    vsc = vsg + fe * vsl
    denc = deng * alfc + denl * (1# - alfc)
    visc = visg * alfc + visl * (1# - alfc)
    recs = denc * vsc * Di / visc
    ffcs = unf_friction_factor(recs, ed, 2) / 4  '  Fanning friction factor required
    rels = denl * vsl * Di / visl
    ffls = unf_friction_factor(rels, ed, 2) / 4  '  Fanning friction factor required
    If (fe < 0.9999) Then
        relf = denl * vsl * (1 - fe) * Di / visl
        fflf = unf_friction_factor(relf, ed, 2) / 4  '  Fanning friction factor required
        a = (1# - fe) ^ 2 * (fflf / ffls)
    Else
        a = 1#
    End If
     
    pgfcs = 4# * ffcs * denc * vsc * vsc / (2# * Di)
    pgfls = 4# * ffls * denl * vsl * vsl / (2# * Di)
    '     --------------------------------------------------------
    '     calculate modified lockhart and martinelli parameters as
    '     defined by alves including entrainment fraction.
    '     --------------------------------------------------------
    xm2 = pgfls / pgfcs
    xmo2 = xm2 * a
    ym = 9.81 * Math.Sin(angr) * (denl - denc) / pgfcs
    '     ------------------------------------------------------------
    '     calculate film thickness if entrainment is less than 99.99%.
    '     ------------------------------------------------------------
    If (fe < 0.9999) Then
        deldmx = 0.499
        deldmn = 0.000001
        deldac = 0.000001
        deld = itsafe(xmo2, ym, c, 0#, 0#, 0#, 4, deldmn, deldmx, deldac)
        '        ----------------------------------------------
        '        check whether annular flow could exist or not.
        '        ----------------------------------------------
        ec = 1# - alfc
        Call chkan(xmo2, ym, deld, ec, e, fpat)
        If (fpat = "slug") Then
            '            -----------------------------------------------------
            '            annular flow not confirmed by barnea"s criteria. slug
            '            flow continues to exist.
            '            -----------------------------------------------------
            Exit Sub
        End If
        '        ------------------------------------------------------
        '        calculate dimensionless groups defined by alves.
        '        ------------------------------------------------------
        phic2 = (1# + c * deld) / (1# - 2# * deld) ^ 5
        phif2 = (phic2 - ym) / xm2
    Else
        '        ------------------------------------------------------
        '        assume 100 % entrainment and therefore no liquid film.
        '        ------------------------------------------------------
        fe = 1#
        deld = 0#
        phic2 = 1#
        phif2 = 0#
        e = 1# - alfc
        If (e > 0.12) Then
            fpat = "slug"
            Exit Sub
        End If
    End If
    '     -----------------------------------------
    '     calculate pressure gradients in the core.
    '     -----------------------------------------
    Dim pgec#, pgfc#, pgtc#, pgef#, pgff#, pgtf#
    pgec = 9.81 * denc * Math.Sin(angr)
    pgfc = pgfcs * phic2
    pgtc = pgec + pgfc
    '     -----------------------------------------
    '     calculate pressure gradients in the film.
    '     -----------------------------------------
    pgef = 9.81 * denl * Math.Sin(angr)
    pgff = pgfls * phif2
    pgtf = pgef + pgff
    '     --------------------------------------------------------------
    '     assume core pressure gradients to be the gradients for annular
    '     flow pattern. the total pressure gradient can be that of film
    '     or core.
    '     --------------------------------------------------------------
    pge = pgec
    pgf = pgfc
    pgt = pgtc
    pga = 0#
    
End Sub

Private Function itsafe(ByRef a#, ByRef b#, ByRef c#, ByRef d#, ByRef e#, ByRef g_#, ByRef i As Integer, ByRef x1#, ByRef x2#, ByRef xacc#) As Double
    '     ---------------------------------------------------------
    '     calculate values of the function and its derivative at x1
    '     and x2.
    '     ---------------------------------------------------------
    Dim fl#, df#, fh#, xl#, xh#, swap#
    Dim dxold#, DX#
    Dim f#
    dim j As Integer, temp As Double
    
    Call Func(a, b, c, d, e, g_, i, x1, fl, df)
    Call Func(a, b, c, d, e, g_, i, x2, fh, df)
    '     ---------------------------------------------------------
    '     interchange values of the function if it varies inversely
    '     with the variable.
    '     ---------------------------------------------------------
    If (fl < 0#) Then
        xl = x1
        xh = x2
    Else
        xh = x1
        xl = x2
        swap = fl
        fl = fh
        fh = swap
    End If
    '     -------------------------------------------------
    '     take the average of x1 and x2 as the first guess.
    '     -------------------------------------------------
    itsafe = 0.5 * (x1 + x2)
    '     ---------------------------------------------------
    '     define the difference in the two successive values.
    '     ---------------------------------------------------
    dxold = Math.Abs(x2 - x1)
    DX = dxold
    '     ----------------------------------------
    '     call func again to use the guessed value.
    '     ----------------------------------------
    Call Func(a, b, c, d, e, g_, i, itsafe, f, df)
    '     -----------------------------------------------------
    '     carry out iteration by using newton-raphson method
    '     together with bisection approach to keep the variable
    '     within its limits.
    '     -----------------------------------------------------
    For j = 1 To 100'MAXIT
        If (((itsafe - xh) * df - f) * ((itsafe - xl) * df - f) >= 0# Or Math.Abs(2# * f) > Math.Abs(dxold * df)) Then
            dxold = DX
            DX = 0.5 * (xh - xl)
            itsafe = xl + DX
            If (xl = itsafe) Then Exit Function
        Else
            dxold = DX
            DX = f / df
            temp = itsafe
            itsafe = itsafe - DX
            If (temp = itsafe) Then Exit Function
        End If
        If (Math.Abs(DX) < xacc Or Math.Abs(f) < xacc) Then Exit Function
        Call Func(a, b, c, d, e, g_, i, itsafe, f, df)
        If (f < 0#) Then
            xl = itsafe
            fl = f
        Else
            xh = itsafe
            fh = f
        End If
        If (f = 0#) Then Exit Function
    Next j

    Dim ex as Exception = new Exception(" itsafe: no convergence even after 100 iterations")
    throw ex

End Function

'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     this subroutine defines a function and its derivative to be used
'     for iteration.
'     written by,    asfandiar m. ansari
'     revised by,    asfandiar m. ansari     last revision: march 1989
'              * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'     this subroutine is called by itsafe to get standard function and
'     its derivative for newton-raphson iterative technique. the number
'     arguments for this subroutine are based on the number of variables
'     involved in the most complex function for which the subroutine is
'     called. for simpler functions most of the arguments are taken as
'     zero. the function to be used by itsafe is recognized by indicator
'     i.
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                           variable description
'                           --------------------
'     df   = derivative of f.
'     f    = function to be defined for iteration.
'     i    = indicator to select f.
'     x    = variable to be iterated.
'     a,b,c,d,e and f are input variables that define f.
'     t"s and dt"s are dummy variables and their derivatives.
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Private Sub Func(ByRef a#, ByRef b#, ByRef c#, ByRef d#, ByRef e#, ByRef g_#, ByRef i As Integer, X#, ByRef f#, ByRef df#)
'     ----------------------------------------------------
'     initialize dummy variables that are repeatedly used.
'     ----------------------------------------------------
Dim t1#, dt1#, t2#, dt2#, t3#, dt3#, t4#, dt4#
t1 = 0#
dt1 = 0#
t2 = 0#
dt2 = 0#
t3 = 0#
dt3 = 0#
t4 = 0#
dt4 = 0#
Dim an#, t5#, t6#, t7#, t8#, t9#, t10#, t11#, t12#, t13#, t14#, dt5#, dt6#, dt7#, dt8#, dt9#, dt10#, dt11#, dt12#, dt13#, dt14#, z#, dz#
If (i = 1) Then
    '        ------------------------------------------------
    '        define f and df to iterate for holdup in bubble.
    '        ------------------------------------------------
    an = 0.5
    f = c * X ^ an + 1.2 * (a + b) - b / (1# - X)
    df = an * c * X ^ (an - 1#) - b / (1# - X) ^ 2
ElseIf (i = 2) Then
    '        ------------------------------------------------------
    '        define f and df to iterate for void fraction in taylor
    '         bubble in slug.
    '        ------------------------------------------------------
    t2 = Math.Sqrt(9.81 * g_ * (1# - Math.Sqrt(X)))
    t3 = 1# - X
    t4 = 1# - e
    t5 = 9.961 * t2
    t6 = c - (c + t5) * t3 / t4
    t7 = 1.2 * (a + b) + d * t4 ^ 0.5
    If (e > 0.25) Then t7 = t6
    t8 = c * (1# - e / X) + t7 * e / X
    t9 = t6 * t4 - a
    t10 = t5 * t3 + t6 * t4
    t11 = b - e * t7
    t12 = X * t8 - e * t7
    t13 = t9 / t10
    t14 = t11 / t12
    f = t13 - t14
    dt2 = -0.25 * 9.81 * g_ / t2 / Math.Sqrt(X)
    dt5 = 9.961 * dt2
    dt6 = -(dt5 * t3 / t4 - (c + t5) / t4)
    dt7 = 0#
    If (e > 0.25) Then dt7 = dt6
    dt8 = c * (e / X ^ 2) + dt7 * e / X - t7 * e / X ^ 2
    dt9 = dt6 * t4
    dt10 = dt5 * t3 - t5 + dt6 * t4
    dt11 = -e * dt7
    dt12 = t8 + X * dt8 - e * dt7
    dt13 = dt9 / t10 - t9 * dt10 / t10 ^ 2
    dt14 = dt11 / t12 - t11 * dt12 / t12 ^ 2
    df = dt13 - dt14
ElseIf (i = 3) Then
    '        --------------------------------------------------------------
    '        define f and df to iterate for nusselt film thickness in slug.
    '        --------------------------------------------------------------
    t1 = e / (0.425 + 2.65 * (d + e))
    t2 = (1# - 2# * X / a) ^ 2
    t3 = (b * t2 - (b - c) * t1) / t2
    t4 = (t3 * t2 - (d + e)) / (1# - t2)
    f = X ^ 3 - 0.75 * a * g_ * t4 * (1# - t2)
    dt2 = -4# * (1# - 2# * X / a) / a
    dt3 = (b - c) * t1 * dt2 / t2 ^ 2
    dt4 = (dt3 * t2 + t3 * dt2) / (1# - t2) + (t3 * t2 - (d + e)) * dt2 / (1# - t2) ^ 2
    df = 3# * X ^ 2 - 0.75 * a * g_ * (dt4 * (1# - t2) - t4 * dt2)
ElseIf (i = 4) Then
    '        --------------------------------------------------------
    '        define f and df to iterate for film thickness in anmist.
    '        --------------------------------------------------------
    t1 = 4# * X * (1# - X)
    z = 1# + c * X
    dt1 = 4# * (1# - 2# * X)
    dz = c
    f = b - z / t1 / (1# - t1) ^ 2.5 + a / t1 ^ 3
    df = -dz / t1 / (1# - t1) ^ 2.5 - 2.5 * z * dt1 / t1 / (1# - t1) ^ 3.5 + z * dt1 / t1 ^ 2 / (1# - t1) ^ 2.5 - 3# * a / t1 ^ 4 * dt1
ElseIf (i = 5) Then
    '         -----------------------------------------------------------
    '         define f and df to iterate  stable film thickness in chkan.
    '         -----------------------------------------------------------
    t1 = 1# - (1# - 2# * X) ^ 2
    dt1 = 4# * (1# - 2# * X)
    '         ---------------------------------------------------
    '         to avoid division of f by 0, adjust x if necessary.
    '         ---------------------------------------------------
    t2 = 1# / t1
    If (t2 = 1.5) Then X = X + 0.001
    t1 = 1# - (1# - 2# * X) ^ 2
    dt1 = 4# * (1# - 2# * X)
    f = b - (2# - 1.5 * t1) * a / t1 ^ 3 / (1# - 1.5 * t1)
    df = 1.5 * dt1 * a / t1 ^ 3 / (1# - 1.5 * t1) + 3# * a * dt1 * (1# - 2# * t1) _
             * (2# - 1.5 * t1) / t1 ^ 4 / (1# - 1.5 * t1) ^ 2
End If
Exit Sub
End Sub


'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'     this subroutine checks the taitel,barnea & dukler prediction of an-
'     nular flow by using criteria developed by barnea. it checks annular
'     flow bridging caused by liquid holdup of greater than 0.24. it also
'     calculates maximum stable film thickness for annular flow and com-
'     pares it with the existing film thickness for the stability of the
'     annular flow. the subroutine calls itasfe to calculate maximum
'     stable film thickness iteratively. it uses dimensionless parameters
'     as input.
'                              references
'                              ----------
'     1. barnea, d., " transition from annular flow and from dispersed
'        bubble flow - unified models for the whole range of pipe in-
'        clinations ", int. j. of multiphase flow, vol.12, (1986),
'        733-744.
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                   input/output logical file variables
'                   -----------------------------------
'     ioerr = output file for error messages when input values passed
'             to the subroutine are out of range or error occurs in
'             the calculation.
'                           subsubroutines called
'                           ------------------
'     itsafe = this subroutine iterate safely within the specified
'              limits of the variable.
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                           variable description
'                           --------------------
'    *deld   = ratio of film thickness to pipe diameter.
'     e      = liquid holdup fraction at a pipe cross-section.
'     ec     = liquid holdup for core with respect to pipe cross-
'              section.
'     ef    = liquid holdup for film with respect to pipe cross-
'              section.
'    *ensc   = no-slip holdup for core with respect to core cross-
'              section.
'      fpat  = flow pattern, (chr)
'                 "anul" = annular
'                 "slug" = slug
'     ierr   = error code, (0=ok, 1=input variable out of range.)
'    *ioerr  = error message file.
'    *xmo2   = dimensionless group defined in  anmist.
'    *ym     = dimensionless group defined in  anmist.
'     (* indicates input variables)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Private Sub chkan(ByRef xmo2#, ByRef ym#, ByRef deld#, ByRef ensc#, ByRef e#, ByRef fpat As string)
' sub parameter : do not dim ! Dim fpat as string * 4
'Dim itsafe As Double
'     ------------------------------------------------------
'     calculate total liquid holdup at a pipe cross-section.
'     ------------------------------------------------------
    Dim ef#, ec#, deldmx#, deldmn#, deldac#, delds#
    ef = 4# * deld * (1# - deld)
    ec = ensc * (1# - 2# * deld) ^ 2
    e = ef + ec
    If (e > 0.12) Then
        fpat = "slug" '        annular flow is bridged resulting in slug flow.
    Else
        '        ----------------------------------------------------
        '        calculate maximum stable film thickness using itsafe
        '        for iteration.
        '        ----------------------------------------------------
        deldmx = 0.499
        deldmn = 0.00001
        deldac = 0.00001
        delds = itsafe(xmo2, ym, 0#, 0#, 0#, 0#, 5, deldmn, deldmx, deldac)
        If (delds < deld) Then
            fpat = "slug" '            film is unstable causing slug flow.
        Else
            fpat = "anul" '            annular flow is confirmed by barnea criteria.
        End If
    End If
End Sub

'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     dispersed bubble transition.
'     written by,  caetano, triggia and shoham
'     revised by,  lorri jefferson                      march 1989
'     revised by,  guohua zheng         last revision:  april 1989
'               * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     this subroutine determines dispersed bubble transition boundaries.
'     the si system of units is used.
'                                references
'                                ----------
'     1.  e.f. caetano, o. shoham and a.a. triggia, "gas liquid
'            two-phase flow pattern prediction computer library",
'            journal of pipelines, 5 (1986) 207-220.
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                          variable description
'                          --------------------
'    *ang    = angle of flow from horizontal. (deg)
'    *deng   = gas density. (kg/m^3)
'    *denl   = liquid density. (kg/m^3)
'    *di     = inside pipe diameter. (m)
'    *ed     = relative pipe roughness
'    *hgg    = guessed gas void fraction
'    *visg   = gas viscosity. (cp)
'    *visl   = liquid viscosity. (cp)
'    *vsg    = superficial gas velocity. (m/sec)
'    *vsl    = superficial liquid velocity. (m/sec)
'     (*indicates input variables, vsg and vsl Close #ioerr:exit subed to calling
'      subroutine)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Private Sub dbtran(ByRef hgg#, ByRef vsl#, ByRef vsg#, ByRef Di#, ByRef ed#, ByRef denl#, ByRef deng#, ByRef visl#, ByRef visg#, ByRef ang#, ByRef surl#)
    Dim vmc As Double, ratio As Double
    Dim c As Double, vme As Double, iter As Long
    Dim Hg As Double
    Dim rhom, vism, Re, FFM
    Dim VM As Double

    Dim IObj As Inspector.InspectorItem = Host.CurrentItem

    IObj?.Paragraphs.Add("start dispersed bubble transition (dbtran)")

    '     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    '     trial and error calculation of dispersed bubble transition
    '     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    c = 2# * ((0.4 * surl) / ((denl - deng) * const_g)) ^ 0.5 * (denl / surl) ^ 0.6 * (2# / Di) ^ 0.4
    vme = vsg + 1.5 '     estimate a mixture velocity
    iter = 0
    For iter = 0 To 50
        If (hgg = 0#) Then
            Hg = vsg / vme
        Else
            Hg = hgg
            vsg = Hg * vme
            vsl = vme - vsg
        End If
        rhom = denl * (1# - Hg) + deng * Hg
        vism = visl * (1# - Hg) + visg * Hg
        Re = Di * rhom * vme / vism
        FFM = unf_friction_factor(Re, ed) '     get frictional factor
        vmc = ((0.725 + 4.15 * Math.Sqrt(Hg)) / c / (FFM / 4#) ^ 0.4) ^ 0.8333 '     calculate new mixture velocity
        ratio = vmc / vme
        If (ratio >= 0.99 And ratio <= 1.01) Then '     check for convergence
            Exit For
        End If
        vme = (vmc + vme) / 2#
    Next iter
    If ratio < 0.8 Then
        IObj?.Paragraphs.Add("dbtran: calculation proceeds without convergence on vm after 50 iterations. ratio = " & Format(ratio, "0.000"))
    End If
    VM = (vmc + vme) / 2
    vsl = VM * (1# - hgg)
    If (hgg > 0#) Then vsg = VM * hgg

End Sub

'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     transition boundaries for upward vertical flow
'     written by,  caetano, shoham, and triggia
'     revised by,  lorri jefferson                      march 1989
'     revised by,  guohua zheng         last revision:  april 1989
'               * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'     this subroutine calculates points on the transition boundaries for
'     upward vertical flow.  the si system of units is used.
'                                references
'                                ----------
'     1.  e.f. caetano, o. shoham and a.a. triggia, "gas liquid
'            two-phase flow pattern prediction computer library",
'            journal of pipelines, 5 (1986) 207-220.
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                          variable description
'                          --------------------
'     alfa   = angle of flow from horizontal. (radians)
'    *ang    = angle of flow from horizontal. (deg)
'    *deng   = gas density. (kg/m^3)
'    *denl   = liquid density. (kg/m^3)
'    *di     = inside pipe diameter. (m)
'    *ed     = relative pipe roughness
'    *visg   = gas viscosity. (cp)
'    *visl   = liquid viscosity. (cp)
'     vsgs   = superficial gas velocity on transition boundaries.
'              (m/sec)
'     vsls   = superficial liquid velocity on transition boundaries.
'              (m/sec)
'    *surl   = gas-liquid surface tension. (dynes/cm)
'     (*indicates input variables)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Private Sub mpoint(ByRef Di#, ByRef ed#, ByRef denl#, ByRef deng#, ByRef visl#, ByRef visg#, ByRef ang#, ByRef surl#, ByRef vsgo#, ByRef vsg1#, ByRef vsl1#, ByRef vsg2#, ByRef vsl2#, ByRef vsg3#, ByRef vsl3#)
 '   iErr = 0

    Dim alfa#, DMin#
    Dim vsl As Double
    
    alfa = 0.0174533 * ang
    '     -----------------------------------------------------------
    '     calculate vsgo
    '     minimum pipe diameter and inclination angle for bubble flow
    '     existed at low liquid flow rates
    '     -----------------------------------------------------------
    DMin = 19# * Math.Sqrt((denl - deng) * surl / (denl ^ 2 * const_g))
    If (ang > 70# And Di > DMin * 0.95) Then
        vsl = 0.001
        vsgo = (vsl + 1.15 * (const_g * (denl - deng) * surl / denl ^ 2) ^ 0.25 * Math.Sin(alfa)) / 3#
    Else
        vsgo = -1#
    End If
    vsg3 = 3.1 * (surl * const_g * Math.Sin(alfa) * (denl - deng)) ^ 0.25 / Math.Sqrt(deng) '     calculate vsg3
    '     --------------
    '     calculate vsg1
    '     --------------
    vsg1 = -1#
    vsl1 = -1#
    If (vsgo > 0#) Then Call dbtran(0.25, vsl1, vsg1, Di, ed, denl, deng, visl, visg, ang, surl)
    '     --------------
    '     calculate vsg2
    '     --------------
    vsg2 = 0.2
    Call dbtran(0.76, vsl2, vsg2, Di, ed, denl, deng, visl, visg, ang, surl)
    If (vsg2 >= vsg3) Then
        vsg2 = vsg3
        Call dbtran(0#, vsl2, vsg2, Di, ed, denl, deng, visl, visg, ang, surl)
        vsl3 = vsl2
        If (vsg1 < vsg2) Then GoTo L999
        vsg1 = vsg2
        GoTo L999
    End If
    '     -----------------------------------
    '     calculate vsl3 on boundary line "c"
    '     -----------------------------------
    vsl3 = (vsg3 / 0.76 - vsg3)
L999:

End Sub

'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'     mechanistic model for pressure gradient and liquid holdup in
'     slug flow.
'     written by,    asfandiar m. ansari
'     revised by,    asfandiar m. ansari       last revision: nov 1988
'              * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'     this subroutine calculates liquid holdup and pressure gradient
'     for slug flow based on the flow mechanics. the concept of develop-
'     ing slug flow adopted by e.f. caetano is incorporated in the model.
'     the si system of units is used.
'                               references
'                               ----------
'     1.  sylvester, n. d., " a mechanistic model for two-phase
'         vertical slug flow in pipes ", asme j.energy resources tech.,
'         vol. 109,(1987),206-213.
'     2.  caetano, e. f., " upward vertical two-phase flow through an
'         annulus ",phd dissertation, the university of tulsa (1985)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'                       variable description
'                       --------------------
'     alfacc = accuracy required in iteration for alftb.
'     alfls  = void fraction in liquid slug.
'     alfns  = no-slip void fraction in liquid slug.
'     alfmax = upper limit for alftb during iteration.
'     alfmin = lower limit for alftb during iteration.
'     alftb  = av. void fraction in taylor bubble for a developed flow or
'              the local void fraction at the bubble tail for a develop-
'              ing slug flow.
'     alftba = av.void fraction in taylor bubble for a developing flow.
'     alftbn = void fraction in taylor bubble at nusselt thickness.
'     alfsu  = void fraction in slug unit for developed slug flow.
'    *angr   = angle of flow from horizontal. (rad)
'     beta   = ratio of ltb and lsu.
'     delacc = accuracy required in iteration for deln. (m)
'     delmax = upper limit for deln during iteration. (m)
'     delmin = lower limit for deln during iteration. (m)
'     deln   = nusselt film thickness. (m)
'    *deng   = gas density. (kg/cum)
'    *denl   = liquid density. (kg/cum)
'     denns  = no-slip density. (kg/cum)
'     dens   = slip density. kg.cum)
'    *di     = inside pipe diameter. (m)
'     e      = liquid holdup fraction.
'     esu    = liquid holdup fraction for a slug unit.
'    *ed     = relative pipe roughness.
'     ff     = friction factor
'     ffls   = friction factor for liquid slug.
'     ind    = indicator for the flow,
'                0 = developed flow.
'                + = developing flow.
'     lc     = length of taylor bubble cap in developing slug flow.(m)
'     lls    = length of liquid slug. (m)
'     lsu    = length of slug unit for developed slug flow. (m)            slug
'     lsua   = length of slug unit for developing slug flow. (m)
'     ltb    = length of taylor bubble in developed slug flow. (m)
'     ltba   = length of taylor bubble in developing slug flow. (m)
'     pga    = acceleration pressure gradient. (pa/m)
'     pgels  = elevation pressure gradient for liquid slug. (pa/m)
'     pgfls  = friction pressure gradient for liquid slug. (pa/m)
'     pgt    = total pressure gradient. (pa/m)
'     rels   = reynolds number for liquid slug.
'     vgls   = velocity of gas in liquid slug. (m/s)
'     vgtb   = velocity of gas in taylor bubble. (m/s)
'    *visg   = gas viscosity. (kg/m-s)
'    *visl   = liquid viscosity. (kg/m-s)
'     visns  = no-slip viscosity. (kg/m-s)
'     vlls   = velocity of liquid in  liquid slug. (m/s)
'     vltb   = velocity of liquid in taylor bubble. (m/s)
'     vmls   = velocity of mixture in liquid slug. (m/s)
'     vs     = slip velocity. (m/s)
'    *vsg    = superficial gas velocity. (m/s)
'     vsgls  = superficial gas velocity in liquid slug. (m/s)
'    *vsl    = superficial liquid velocity. (m/s)
'     vslls  = superficial liquid velocity in liquid slug. (m/s)
'     c,f and df are dummy variables.
'     (*indicates input variables)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Private Sub slug(ByRef angr#, ByRef Di#, ByRef ed#, ByRef denl#, ByRef deng#, ByRef visl#, ByRef visg#, ByRef vsl#, ByRef vsg#, ByRef surl#, ByRef e#, ByRef pgf#, ByRef pge#, ByRef pga#, pgt#)

    
    Dim lls As Double
    Dim lsu As Double
    Dim ltb As Double
    Dim ltb1 As Double
    Dim ltb2 As Double
    Dim LC As Double
    Dim msg As String
    Dim ind As Integer, c#, d#, f#, h#, alftba#, alfsu#, esu#, dens#, pgels#
    Dim vtb#, vs#, alfls#, alfmin#, alfmax#, alfacc#, alftb#, vltb#, vlls#, vgls#, vgtb#, beta1#, beta2#, Beta#, g_#, delmin#, delmax#, delacc#, deln#, alftbn#, vgtbn#, vltbn#
    '     -----------------------------------------------
    '     assume lls to be 30 times the diameter of pipe.
    '     -----------------------------------------------
    lls = 30# * Di
    '     -----------------------------------------------------
    '     calculate void fraction in taylor bubble using itsafe
    '     and assuming developed slug flow.
    '     -----------------------------------------------------
    vtb = 1.2 * (vsl + vsg) + 0.35 * Math.Sqrt(9.81 * Di * (denl - deng) / denl)
    vs = 1.53 * (9.81 * surl * (denl - deng) / denl ^ 2) ^ 0.25
    alfls = vsg / (0.425 + 2.65 * (vsl + vsg))
    alfmin = 0.7
    alfmax = 0.9999
    alfacc = 0.000001

    alftb = itsafe(vsl, vsg, vtb, vs, alfls, Di, 2, alfmin, alfmax, alfacc)
    '     --------------------------------
    '     calculate additional parameters.
    '     --------------------------------
    vltb = 9.916 * Math.Sqrt(9.81 * Di * (1# - Math.Sqrt(alftb)))
    vlls = vtb - (vtb + vltb) * (1# - alftb) / (1# - alfls)
    vgls = 1.2 * (vsl + vsg) + 1.53 * (9.81 * surl * (denl - deng) / denl ^ 2) ^ 0.25 * (1# - alfls) ^ 0.5
    If (alfls > 0.25) Then vgls = vlls
    vgtb = vtb * (1# - alfls / alftb) + vgls * alfls / alftb
    beta1 = (vlls * (1# - alfls) - vsl) / (vltb * (1# - alftb) + vlls * (1# - alfls))
    beta2 = (vsg - alfls * vgls) / (alftb * vgtb - alfls * vgls)
    If (Math.Abs(beta1 - beta2) > 0.1) Then
        msg = "   slug: error in beta conv."
        GoTo L999
    End If
    Beta = (beta1 + beta2) / 2#
    If (Beta <= 0# Or Beta >= 1#) Then
        msg = "   slug: unreal value for beta"
        GoTo L999
    End If
    lsu = lls / (1# - Beta)
    ltb = lsu - lls
    '     ---------------------------------------------------------
    '     calculate nusselt film thickness iteratively using itsafe
    '     ---------------------------------------------------------
    g_ = visl / (9.81 * (denl - deng))
    delmin = 0.00001
    delmax = 0.499 * Di
    delacc = 0.000001
    deln = itsafe(Di, vtb, vgls, vsl, vsg, g_, 3, delmin, delmax, delacc)
    '     --------------------------------------------------------
    '     calculate lc using the values of the parameters at deln.
    '     --------------------------------------------------------
    alftbn = (1# - 2# * deln / Di) ^ 2
    vgtbn = (vtb * alftbn - (vtb - vgls) * alfls) / alftbn
    vltbn = (vgtbn * alftbn - (vsl + vsg)) / (1# - alftbn)
    LC = (vltbn + vtb) ^ 2 / (2# * 9.81)
    '     ---------------------------------
    '     check for the nature of the flow.
    '     ---------------------------------
    If (LC > (0.75 * ltb)) Then
        '        --------------------------------------------------------
        '        developing slug flow exists. calculate new values for
        '        slug flow parameters starting with the length of
        '        taylor  bubble by solving a quadratic equation.
        '        --------------------------------------------------------
        ind = 1
        c = (vsg - vgls * alfls) / vtb
        d = 1# - vsg / vtb
        e = vtb - vlls
        f = (-2# * d * c * lls - 2# * (e * (1# - alfls)) ^ 2 / 9.81) / d ^ 2
        g_ = (c * lls / d) ^ 2
        h = f * f - 4# * g_
        If (h <= 0#) Then
            msg = "   slug: error in solving for ltb"
            GoTo L999
        End If
        ltb1 = (-f + Math.Sqrt(h)) / 2#
        ltb2 = (-f - Math.Sqrt(h)) / 2#
        If (ltb1 <= 0# And ltb2 <= 0#) Then
            msg = "   slug: error in ltb root"
            GoTo L999
        End If
        If (ltb1 > ltb2) Then ltb = ltb1
        If (ltb2 > ltb1) Then ltb = ltb2
        alftba = 1# - 2# * (vtb - vlls) * (1# - alfls) / Math.Sqrt(2# * 9.81 * ltb)
        lsu = ltb + lls
        Beta = ltb / lsu
    Else
        '     ---------------------------------------------------------------
        '     developed slug flow exists. no new values of the parameters are
        '     required.
        '     ---------------------------------------------------------------
        ind = 0
    End If
    '     ----------------------------------------
    '     calculate liquid holdup for a slug unit.
    '     ----------------------------------------
    alfsu = alftb * Beta + alfls * (1# - Beta)
    If (ind = 1) Then alfsu = alftba * Beta + alfls * (1# - Beta)
    esu = 1# - alfsu
    '     -----------------------------------------------------
    '     calculate elevation pressure gradient for liquid slug
    '     using its slip density.
    '     -----------------------------------------------------
    dens = denl * (1# - alfls) + deng * alfls
    pgels = 9.81 * Math.Sin(angr) * dens
    '     -----------------------------------------------------------
    '     calculate elevation pressure gradient for taylor  bubble
    '     using its average void fraction.
    '     -----------------------------------------------------------
    Dim pgetb#, vslls#, vsgls#, vmls#, alfns#, visns#, rels#, ffls#, pgfls#
    If (ind = 1) Then
        pgetb = 9.81 * Math.Sin(angr) * (deng * alftba + denl * (1 - alftba))
    Else
        pgetb = 9.81 * Math.Sin(angr) * deng
    End If
    '     --------------------------------------------------
    '     calculate frictional pressure gradient for liquid
    '     slug.
    '     --------------------------------------------------
    vslls = vlls * (1# - alfls)
    vsgls = vgls * alfls
    vmls = vslls + vsgls
    alfns = vsgls / vmls
    visns = visl * (1# - alfns) + visg * alfns
    rels = dens * vmls * Di / visns
    ffls = unf_friction_factor(rels, ed, 2)
    pgfls = dens * vmls ^ 2 * ffls / (2# * Di)
    pga = 0#    '     acceleration pressure gradient over a slug unit is zero.
    '     ---------------------------------------------------------
    '     assume constant pressure gradients and holdup for all the
    '     slug units within one pipe increment.
    '     ---------------------------------------------------------
    e = esu
    pge = pgels * (1# - Beta) + pgetb * Beta
    pgf = pgfls * (1# - Beta)
    pgt = pge + pgf + pga
    Exit Sub
L999:
    throw New Exception(msg)

End Sub

'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'     mechanistic model for pressure gradient and liquid holdup in
'     bubble flow.
'     written by,    asfandiar m. ansari
'     revised by,    asfandiar m. ansari     last revision: march 1989
'              * *  tulsa university fluid flow projects  * *
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'     this subroutine calculates liquid holdup and pressure gradient
'     both for dispersed bubble and bubbly flows using a mechanistic
'     approach. for dispersed bubble flow the subroutine assumes no
'     slippage, whereas for bubbly flow slippage is considered between
'     the two phases. an explicit equation developed by zigrang and
'     sylvester is used for friction factor. the si system of units is
'     used.
'                               references
'                               ----------
'     1.  ansari, a. m. and sylvester, n. d., " a mechanistic model for
'         upward bubble flow in pipes ", aiche j., 8, 34, 1392-1394,
'         (aug 1988).
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*
'                       variable description
'                       --------------------
'     *angr  = angle of flow from horizontal. (rad)
'      den   = slip / no-slip density (kg/cum)
'     *deng  = gas density. (kg/cum)
'     *denl  = liquid density. (kg/cum)
'      df    = derivative of the function used in newton-raphson method.
'     *di    = inside pipe diameter. (m)
'      e     = liquid holdup fraction.
'      eacc  = accuracy required in iteration for e.
'     *ed    = relative pipe roughness.
'      emax  = upper limit for e during iteration.
'      emin  = lower limit for e during iteration.
'      ens   = no-slip liquid holdup fraction.
'      emin  = lower limit for e during iteration.
'      f     = function defined for newton-raphson method.
'      ff    = friction factor
'      fpat  = flow pattern, (chr)
'                 "dbub" = dispersed bubble
'                 "bubl" = bubbly
'      ierr  = error code. (0=ok, 1=input variables out of range,
'              2=extrapolation of correlation occurring)
'     *ioerr = output file for error messages when input values
'              passed to the subroutine are out of range.
'      pga   = acceleration pressure gradient. (pa/m)
'      pge   = elevation pressure gradient. (pa/m)
'      pgf   = friction pressure gradient. (pa/m)
'      pgt   = total pressure gradient. (pa/m)
'      re    = reynolds number.
'     *visg  = gas viscosity. (kg/m-s)
'     *visl  = liquid viscosity. (kg/m-s)
'      visns = no-slip viscosity. (kg/m-s)
'      vs    = slip velocity (m/s)
'     *vsg   = superficial gas velocity. (m/s)
'     *vsl   = superficial liquid velocity. (m/s)
'      (*indicates input variables)
'     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Private Sub bubble(ByRef angr#, ByRef Di#, ByRef ed#, ByRef denl#, ByRef deng#, ByRef visl#, ByRef visg#, ByRef vsl#, ByRef vsg#, ByRef surl#, ByRef fpat As string, ByRef e#, ByRef pgf#, ByRef pge#, ByRef pga#, ByRef pgt#)

    '     --------------------------------------
    '     calculate slip and no-slip parameters.
    '     --------------------------------------
    Dim ens#, visns#, vs#
    ens = vsl / (vsl + vsg)
    visns = visl * ens + visg * (1# - ens)
    vs = 1.53 * (surl * 9.81 * (denl - deng) / denl ^ 2) ^ 0.25
    If (fpat = "dbub") Then
        e = ens '        dispersed  bubble flow exists, calculate no-slip holdup.
    Else
        '        --------------------------------------------------
        '        bubbly flow exists, calculate actual liquid holdup
        '        using function itsafe for iteration.
        '        --------------------------------------------------
        Dim emin#, emax#, eacc#
        emin = ens
        emax = 0.999
        eacc = 0.001
        e = itsafe(vsl, vsg, vs, 0#, 0#, 0#, 1, emin, emax, eacc)
    End If
    '     --------------------------------------
    '     calculate elevation pressure gradient.
    '     --------------------------------------
    Dim den#
    den = denl * e + deng * (1# - e)
    pge = den * Math.Sin(angr) * 9.81
    '     ---------------------------------------
    '     calculate frictional pressure gradient.
    '     ---------------------------------------
    Dim Re#, FF#
    Re = den * (vsl + vsg) * Di / visns
    FF = unf_friction_factor(Re, ed, 2)
    pgf = 0.5 * den * FF * (vsl + vsg) ^ 2 / Di
    '     ---------------------------------------------------------
    '     calculate total pressure gradient neglecting acceleration
    '     component.
    '     ---------------------------------------------------------
    pga = 0#
    pgt = pge + pgf + pga

End Sub

    End Class
End Namespace