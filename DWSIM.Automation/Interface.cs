using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DWSIM.Interfaces;
using DWSIM.FlowsheetSolver;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DWSIM.ExtensionMethods;
using DWSIM.UnitOperations.SpecialOps;
using cv = DWSIM.SharedClasses.SystemsOfUnits.Converter;
namespace DWSIM.Automation
{

    [Guid("ed615e8f-da69-4c24-80e2-bfe342168060")]
    public interface AutomationInterface
    {
        Interfaces.IFlowsheet LoadFlowsheet(string filepath);
        void SaveFlowsheet(IFlowsheet flowsheet, string filepath, bool compressed);
        void CalculateFlowsheet(IFlowsheet flowsheet, ISimulationObject sender);

        /// <summary>
        /// Выполнить подстройку
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="adjust">Регулятор</param>
        /// <param name="minValue">Минимальная граница манипулируемого объекта</param>
        /// <param name="maxValue">Максимальная граница манипулируемого объекта</param>
        /// <param name="tolerance">Допустимая погрешность</param>
        /// <returns></returns>
        bool Adjust(IFlowsheet sheet, Adjust adjust, double? minValue, double? maxValue, double? tolerance, out string errorText);

        double ConvertFromSI(double d, string units);
        double ConvertToSI(double d, string units);
    }

    [Guid("37437090-e541-4f2c-9856-d1e27df32ecb"), ClassInterface(ClassInterfaceType.None)]
    public partial class Automation : AutomationInterface
    {

        FormMain fm = null;

        public Automation()
        {
            GlobalSettings.Settings.AutomationMode = true;
            fm = new FormMain();
        }

        public Interfaces.IFlowsheet LoadFlowsheet(string filepath)
        {
            var ext = System.IO.Path.GetExtension(filepath).ToLower();
            if (ext.Contains("dwxmz") || ext.Contains("armgz"))
            {
                return fm.LoadAndExtractXMLZIP(filepath, null, true);
            }
            else
            {
                return fm.LoadXML(filepath, null, "", true);
            }
        }

        public void SaveFlowsheet(IFlowsheet flowsheet, string filepath, bool compressed)
        {
            if (compressed)
            {
                fm.SaveXMLZIP(filepath, (FormFlowsheet) flowsheet);
            }
            else
            {
                fm.SaveXML(filepath, (FormFlowsheet) flowsheet);
            }
        }

        public void CalculateFlowsheet(IFlowsheet flowsheet, ISimulationObject sender)
        {
            GlobalSettings.Settings.SolverBreakOnException = true;
            GlobalSettings.Settings.SolverMode = 0;
            GlobalSettings.Settings.SolverTimeoutSeconds = 220;
            GlobalSettings.Settings.EnableGPUProcessing = false;
            GlobalSettings.Settings.EnableParallelProcessing = true;

            if ((sender != null))
            {
                FlowsheetSolver.FlowsheetSolver.CalculateObject(flowsheet, sender.Name);
            }
            else
            {
                FlowsheetSolver.FlowsheetSolver.SolveFlowsheet(flowsheet, GlobalSettings.Settings.SolverMode);
            }
        }

        private double GetRefVarValue(IFlowsheet formC, Adjust myADJ, IUnitsOfMeasure su)
        {
            var cod = myADJ.ControlledObjectData;
            return Convert.ToDouble(formC.SimulationObjects[cod.ID].GetPropertyValue(cod.Name, su) ?? 0);
        }

        private double GetMnpVarValue(IFlowsheet formC, Adjust myADJ)
        {
            var mod = myADJ.ManipulatedObjectData;
            return Convert.ToDouble(formC.SimulationObjects[mod.ID].GetPropertyValue(mod.PropertyName) ?? 0);
        }

        private void SetMnpVarValue(IFlowsheet formC, Adjust myADJ, double val)
        {
            var md = myADJ.ManipulatedObjectData;
            formC.SimulationObjects[md.ID].SetPropertyValue(md.PropertyName, val);
        }

        private double GetCtlVarValue(IFlowsheet formC, Adjust myADJ, IUnitsOfMeasure su)
        {
            var cod = myADJ.ControlledObjectData;
            return Convert.ToDouble(formC.SimulationObjects[cod.ID].GetPropertyValue(cod.PropertyName, su) ?? 0);
        }
    }
}
