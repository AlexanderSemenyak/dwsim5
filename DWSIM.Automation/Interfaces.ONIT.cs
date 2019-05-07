using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using DWSIM.ExtensionMethods;
using DWSIM.Interfaces;
using DWSIM.SharedClasses.SystemsOfUnits;
using DWSIM.UnitOperations.SpecialOps;

namespace DWSIM.Automation
{
    partial class Automation
    {
        /// <summary>
        /// Add userdefined UOMs for automation
        /// </summary>
        /// <param name="serializedUnits"></param>
        public Units AddUnits(byte[] serializedUnits)
        {
            using (var ms = new MemoryStream(serializedUnits))
            {
                var su = new Units();
                var mySerializer = new BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext());
                mySerializer.Binder = UnitsVersionBinder.Instance;

                try
                {
                    //Debugger.Break();
                    su = (Units) mySerializer.Deserialize(ms);
                    if (su != null)
                    {
                        Units.PredefinedUserUnits[su.Name] = su;
                        return su;
                    }
                }
                catch (Exception e)
                {
                    //Debugger.Break();

                    throw;
                }
            }

            return null;
        }

        public void CloseWithoutSave(IFlowsheet flowsheet)
        {
            GlobalSettings.Settings.AutomationMode = true;
            if (flowsheet == null) throw new ArgumentNullException(nameof(flowsheet));

           
            if (flowsheet is FormFlowsheet ffs)
            {
                //flowsheet.Reset(); - Not Implemented in this version of IFlowsheet
                ffs.m_overrideCloseQuestion = true; //чтобы диалог о закрытии не появлялся 
                ffs.Close();
            }
            else
            {
                //flowsheet.Reset(); 
                throw new NotSupportedException("[Automation.CloseWithoutSave]flowsheet type = "+ flowsheet.GetType());
            }
        }

        /// <summary>
        /// Выполнить подстройку
        /// </summary>
        /// <param name="formC"></param>
        /// <param name="myADJ">Регулятор</param>
        /// <param name="minValue">Минимальная граница манипулируемого объекта</param>
        /// <param name="maxValue">Максимальная граница манипулируемого объекта</param>
        /// <param name="tolerance">Допустимая погрешность</param>
        /// <returns></returns>
        public bool Adjust(IFlowsheet formC, Adjust myADJ, double? minValue, double? maxValue, double? tolerance, out string errorText)
        {
            GlobalSettings.Settings.AutomationMode = true;
            IUnitsOfMeasure su = myADJ.FlowSheet.FlowsheetOptions.SelectedUnitSystem;
            double? mvVal, rfVal = null;
            double cvVal, maxval, minval;
            if (formC.SimulationObjects[myADJ.ControlledObjectData.ID].GraphicObject.Calculated)
            {
                cvVal = this.GetCtlVarValue(formC, myADJ, su);
            }

            if (formC.SimulationObjects[myADJ.ManipulatedObjectData.ID].GraphicObject.Calculated)
            {
                mvVal = this.GetMnpVarValue(formC, myADJ);
            }

            //maxval = myADJ.MaxVal ?? 0f;
            //minval = myADJ.MinVal ?? 0f;

            if (myADJ.Referenced)
            { if (formC.SimulationObjects[myADJ.ReferencedObjectData.ID].GraphicObject.Calculated)
                {
                    rfVal = this.GetRefVarValue(formC, myADJ, su);
                }
            }

            double tol, maxit, adjval, stepsize, max, min;

            if (myADJ.Referenced)
            {
                if (rfVal != null)
                {
                    adjval = (rfVal??0) + Converter.ConvertFromSI(myADJ.ControlledObject.GetPropertyUnit(myADJ.ControlledObjectData.PropertyName, su),myADJ.AdjustValue);
                }
                else
                {
                    errorText = "[Adjust]GetRefVarValue == null";
                    return false;
                }
            }
            else
            {
                adjval = Converter.ConvertFromSI(myADJ.ControlledObject.GetPropertyUnit(myADJ.ControlledObjectData.PropertyName, su), myADJ.AdjustValue);
            }

            maxit = myADJ.MaximumIterations;
            //stepsize = myADJ.StepSize;
            tol = tolerance ?? myADJ.Tolerance;
            //min = (myADJ.MinVal ?? 0).ConvertToSI(myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su));
            //max = (myADJ.MaxVal ?? 0).ConvertToSI(myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su));
        
            minval = (minValue ?? (myADJ.MinVal ?? 0f)).ConvertToSI(myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su));
            maxval = (maxValue ?? (myADJ.MaxVal ?? 0f)).ConvertToSI(myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su));

            int l = 0;
            int i = 0;

            double f, f_inf, nsub, delta;
            nsub = 5;
            delta = (maxval - minval) / nsub;

            List<double> py1 = new List<double>(), py2 = new List<double>(), px = new List<double>();

            bool cancelar = false;
            do
            {
                px.Add(minval.ConvertFromSI(myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su)));
                this.SetMnpVarValue(formC, myADJ, minval);

                FlowsheetSolver.FlowsheetSolver.CalculateObject(formC, myADJ.ManipulatedObject.GraphicObject.Name);
                cvVal = this.GetCtlVarValue(formC, myADJ, su);
                f = cvVal.ConvertToSI(myADJ.ControlledObject.GetPropertyUnit(myADJ.ControlledObjectData.PropertyName, su)) -
                    adjval.ConvertToSI(myADJ.ControlledObject.GetPropertyUnit(myADJ.ControlledObjectData.PropertyName, su));
                //Me.lblStatus.Text = formC.GetTranslatedString("Ajustando")
                //Me.lblItXdeY.Text = formC.GetTranslatedString("Procurandosubinterva")
                //Me.tbErro.Text = f
                py1.Add(adjval);
                py2.Add(cvVal);
                //AtualizaGrafico() //обновить график новыми значениями подстройки
                minval = minval + delta;
                px.Add(minval.ConvertFromSI(myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su)));
                this.SetMnpVarValue(formC, myADJ, minval);

                FlowsheetSolver.FlowsheetSolver.CalculateObject(formC, myADJ.ManipulatedObject.GraphicObject.Name);
                cvVal = this.GetCtlVarValue(formC, myADJ, su);
                f_inf = cvVal.ConvertToSI(myADJ.ControlledObject.GetPropertyUnit(myADJ.ControlledObjectData.PropertyName, su)) -
                        adjval.ConvertToSI(myADJ.ControlledObject.GetPropertyUnit(myADJ.ControlledObjectData.PropertyName, su));
                //Me.lblStatus.Text = formC.GetTranslatedString("Ajustando")
                //Me.lblItXdeY.Text = formC.GetTranslatedString("Procurandosubinterva")
                //Me.tbErro.Text = f_inf
                py1.Add(adjval);
                py2.Add(cvVal);
                //AtualizaGrafico()
                l += 1;
                if (l > 5)
                {
                    errorText = formC.GetTranslatedString("Oajustenoencontrouum") + "\r\n" + formC.GetTranslatedString("Semsoluo");
                    //Me.lblStatus.Text = formC.GetTranslatedString("Noexistesoluonointer")
                    //Me.lblItXdeY.Text = ""
                    cancelar = false;
                    //Me.btnIniciar.Enabled = True
                    myADJ.GraphicObject.Calculated = false;
                    return false;
                }

                if (f == f_inf)
                {
                    //The manipulated variable does not seem to have effect on the controlled variable
                    errorText = formC.GetTranslatedString("Avarivelmanipuladano") + formC.GetTranslatedString("Desejacontinuaroproc");
                    //formC.GetTranslatedString("Problemasnaconvergnc"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    //If msgres = MsgBoxResult.No Then
                    //Me.lblStatus.Text = formC.GetTranslatedString("Ajustecanceladopelou")
                    //Me.btnIniciar.Enabled = True
                    myADJ.GraphicObject.Calculated = false;
                    return false;
                }

                if (cancelar) break;
            } while (!(f * f_inf < 0));//loop работает обратно while

            maxval = minval;
            minval = minval - delta;

            //método de Brent
            double aaa=0, bbb, ccc, ddd=0, eee=0, min11, min22, faa, fbb, fcc, ppp, qqq, rrr, sss, tol11, xmm;
            double ITMAX2 = maxit;
            int iter2;
            aaa = minval;
            bbb = maxval;
            ccc = maxval;
            faa = f;
            fbb = f_inf;
            fcc = fbb;
            iter2 = 0;

            do
            {
                if (cancelar) break;
                var status = formC.GetTranslatedString("Ajustando");
                var XY = formC.GetTranslatedString("Iterao") + " " + (iter2 + l + 1) + " " +
                         formC.GetTranslatedString("de") + " " + maxit;
                var err = fbb;
                //Application.DoEvents();

                if ((fbb > 0 && fcc > 0) || (fbb < 0 && fcc < 0))
                {
                    ccc = aaa;
                    fcc = faa;
                    ddd = bbb - aaa;
                    eee = ddd;
                }

                if (Math.Abs(fcc) < Math.Abs(fbb))
                {
                    aaa = bbb;
                    bbb = ccc;
                    ccc = aaa;
                    faa = fbb;
                    fbb = fcc;
                    fcc = faa;
                }

                tol11 = tol;
                xmm = 0.5 * (ccc - bbb);
                if (Math.Abs(fbb) < tol) goto Final3;
                if ((Math.Abs(eee) >= tol11) && (Math.Abs(faa) > Math.Abs(fbb)))
                {
                    sss = fbb / faa;
                    if (aaa == ccc)
                    {
                        ppp = 2 * xmm * sss;
                        qqq = 1 - sss;
                    }
                    else
                    {
                        qqq = faa / fcc;
                        rrr = fbb / fcc;
                        ppp = sss * (2 * xmm * qqq * (qqq - rrr) - (bbb - aaa) * (rrr - 1));
                        qqq = (qqq - 1) * (rrr - 1) * (sss - 1);
                    }

                    if (ppp > 0) qqq = -qqq;
                    ppp = Math.Abs(ppp);
                    min11 = 3 * xmm * qqq - Math.Abs(tol11 * qqq);
                    min22 = Math.Abs(eee * qqq);
                    double tvar2=0;
                    if (min11 < min22) tvar2 = min11;
                    if (min11 > min22) tvar2 = min22;
                    if (2 * ppp < tvar2)
                    {
                        eee = ddd;
                        ddd = ppp / qqq;
                    }
                    else
                    {
                        ddd = xmm;
                        eee = ddd;
                    }
                }
                else
                {
                    ddd = xmm;
                    eee = ddd;
                }

                aaa = bbb;
                faa = fbb;
                if (Math.Abs(ddd) > tol11)
                {
                    bbb += ddd;
                }
                else
                {
                    bbb += Math.Sign(xmm) * tol11;
                }

                this.SetMnpVarValue(formC, myADJ, bbb);
                FlowsheetSolver.FlowsheetSolver.CalculateObject(formC, myADJ.ManipulatedObject.GraphicObject.Name);
                cvVal = this.GetCtlVarValue(formC, myADJ, su);
                fbb = cvVal - adjval;
                //Me.tbErro.Text = fbb
                iter2 += 1;

                px.Add(bbb.ConvertFromSI(
                    myADJ.ManipulatedObject.GetPropertyUnit(myADJ.ManipulatedObjectData.PropertyName, su)));
                py1.Add(adjval);
                py2.Add(cvVal);
                //AtualizaGrafico()

                if ((iter2 + l - 1) >= maxit)
                {
                    //The maximum number of iterations has been reached. Continue?
                    errorText = formC.GetTranslatedString("Onmeromximodeiteraes"); //formC.GetTranslatedString("Nmeromximodeiteraesa3"), _MessageBoxButtons.YesNo, MessageBoxIcon.Question)If msgres = MsgBoxResult.No Then
                    cancelar = true;
                    //Me.lblStatus.Text = formC.GetTranslatedString("Mximodeiteraesatingi")
                    //Me.btnIniciar.Enabled = True
                    myADJ.GraphicObject.Calculated = false;
                    return false;
                    //}
                    //else
                    //{
                    //    iter2 = 1;
                    //}
                }

                if (cancelar) break;
            } while (iter2 < ITMAX2); //hint until работает наоборот от while Loop Until iter2 >= ITMAX2
            Final3:
            if (cancelar)
            {
                //Adjustment process cancelled by the user.
                errorText = formC.GetTranslatedString("Ajustecanceladopelou");
                myADJ.GraphicObject.Calculated = false;
                return false;
            }

            //Value adjusted successfully. Me.lblStatus.Text = formC.GetTranslatedString("Valorajustadocomsuce")
            myADJ.GraphicObject.Calculated = true;
            errorText = null;
            return true;
        }

        public double ConvertFromSI(double d, string units)
        {
            return Converter.ConvertFromSI(units, d);
        }

        public double ConvertToSI(double d, string units)
        {
            return Converter.ConvertToSI(units, d);
        }
    }
}
