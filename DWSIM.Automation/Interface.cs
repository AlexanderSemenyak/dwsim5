using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DWSIM.Interfaces;
using DWSIM.FlowsheetSolver;
using System.Runtime.InteropServices;
using DWSIM.UI.Desktop.Shared;
using System.Xml.Linq;
using DWSIM.UnitOperations.UnitOperations.Auxiliary;
using System.IO;

namespace DWSIM.Automation
{

    [Guid("ed615e8f-da69-4c24-80e2-bfe342168060")]
    public interface AutomationInterface
    {
        /// <summary>
        /// Add userdefined UOMs for automation
        /// </summary>
        /// <param name="serializedUnits"></param>
        Units AddUnits(byte[] serializedUnits);

        /// <summary>
        /// Загрузить из потока не сохраняя на диск
        /// </summary>
        /// <param name="fsFlowsheet"></param>
        /// <param name="filepathVirtual"></param>
        /// <param name="errorText"></param>
        /// <returns></returns>
        IFlowsheet LoadFlowsheet(Stream fsFlowsheet, string filepathVirtual, out string errorText);

        /// <summary>
        /// Загрузить модель из файла
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="errorText"></param>
        /// <returns></returns>
        IFlowsheet LoadFlowsheet(string filepath, out string errorText);

        void SaveFlowsheet(IFlowsheet flowsheet, string filepath, bool compressed);
        void SaveFlowsheet2(IFlowsheet flowsheet, string filepath);
        void CloseWithoutSave(IFlowsheet flowsheet);

        List<Exception> CalculateFlowsheet(IFlowsheet flowsheet, ISimulationObject sender);

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
    public partial class Automation : AutomationInterface, IDisposable
    {
        FormMain fm = null;

        public Automation()
        {
            SetupCalculationMode();

            var asm = typeof(FormMain).Assembly;
            var myProjectType = asm.GetType("DWSIM.My.MyProject");
            PropertyInfo pForms = myProjectType.GetProperty("Forms", BindingFlags.Static | BindingFlags.NonPublic);
            var myForms = pForms.GetValue(null);
            //DWSIM.My.MyProject.MyForms
            var myForms_pMainForm = myForms.GetType().GetProperty("FormMain");
            fm= (FormMain)myForms_pMainForm.GetValue(myForms); //(!) так мы инстанцируем форму аналогично как при старте VB.NET-проекта - т.е. без повторного ее создания в коде DWSIM
            //fm = new FormMain();

            fm.m_SupressMessages = true;
        }

        public Interfaces.IFlowsheet LoadFlowsheet(string filepath, out string errorText)
        {
            return this.LoadFlowsheet(null, filepath, out errorText);
        }

        /// <summary>
        /// Загрузить из потока не создавая промежуточный файл
        /// </summary>
        /// <param name="fsFlowsheet"></param>
        /// <param name="filepath"></param>
        /// <param name="errorText"></param>
        /// <returns></returns>
        public Interfaces.IFlowsheet LoadFlowsheet(Stream fsFlowsheet, string filepath, out string errorText)
        {
            GlobalSettings.Settings.AutomationMode = true;
            var ext = System.IO.Path.GetExtension(filepath).ToLower();
            IFlowsheet fs = null;
            if (ext.Contains("dwxmz") || ext.Contains("armgz"))
            {
                fs = fm.LoadAndExtractXMLZIP(fsFlowsheet, filepath, null, true, Path.GetDirectoryName(filepath)/*for exclude blocking by parallel access*/);
            }
            else
            {
                fs = fm.LoadXML(fsFlowsheet, filepath, null, "", true);
            }

            errorText = fs == null ? fm.m_LastError : null;
            return fs;
        }

        public void SaveFlowsheet(IFlowsheet flowsheet, string filepath, bool compressed)
        {
            GlobalSettings.Settings.AutomationMode = true;
            if (compressed)
            {
                fm.SaveXMLZIP(filepath, (FormFlowsheet) flowsheet);
            }
            else
            {
                fm.SaveXML(filepath, (FormFlowsheet) flowsheet);
            }
        }

        public List<Exception> CalculateFlowsheet(IFlowsheet flowsheet, ISimulationObject sender)
        {
            SetupCalculationMode();

            if (sender != null)
            {
                return FlowsheetSolver.FlowsheetSolver.CalculateObject(flowsheet, sender.Name);
            }

            return FlowsheetSolver.FlowsheetSolver.SolveFlowsheet(flowsheet, GlobalSettings.Settings.SolverMode);
        }

        private static void SetupCalculationMode()
        {
            GlobalSettings.Settings.AutomationMode = true;
            GlobalSettings.Settings.SolverBreakOnException = true;
            GlobalSettings.Settings.SolverMode = 0;
            GlobalSettings.Settings.SolverTimeoutSeconds = 360;
            GlobalSettings.Settings.EnableGPUProcessing = false;
            GlobalSettings.Settings.EnableParallelProcessing = true;
            Console.WriteLine("Solving Flowsheet, please wait...");
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

        public void Dispose()
        {
            fm?.Dispose();
        }
        public void SaveFlowsheet2(IFlowsheet flowsheet, string filepath)
        {
            SaveFlowsheet(flowsheet, filepath, true);
        }
    }
    [Guid("22694b87-1ba6-4341-81dd-8d33f48643d7"), ClassInterface(ClassInterfaceType.None)]
    public class Automation2 : AutomationInterface
    {

        Eto.Forms.Application app;
        UI.Forms.Flowsheet fm;

        public Automation2()
        {
            GlobalSettings.Settings.AutomationMode = true;
            Console.WriteLine("Initializing DWSIM in Automation Mode, please wait...");
            app = UI.Desktop.Program.MainApp(null);
            app.Attach(this);
        }

        public Interfaces.IFlowsheet LoadFlowsheet(string filepath)
        {
            GlobalSettings.Settings.AutomationMode = true;
            Console.WriteLine("Initializing the Flowsheet, please wait...");
            fm = new UI.Forms.Flowsheet();
            Console.WriteLine("Loading Flowsheet data, please wait...");
            LoadSimulation(filepath);
            return fm.FlowsheetObject;
        }

        public void SaveFlowsheet(IFlowsheet flowsheet, string filepath, bool compressed)
        {
            Console.WriteLine("Saving the Flowsheet, please wait...");
            fm.FlowsheetObject = (Flowsheet)flowsheet;
            fm.SaveSimulation(filepath);
        }

        public void CalculateFlowsheet(IFlowsheet flowsheet, ISimulationObject sender)
        {
            GlobalSettings.Settings.SolverBreakOnException = true;
            GlobalSettings.Settings.EnableGPUProcessing = false;
            GlobalSettings.Settings.EnableParallelProcessing = true;
            Console.WriteLine("Solving Flowsheet, please wait...");
            fm.FlowsheetObject.SolveFlowsheet2();
        }

        private void LoadSimulation(string path)
        {
            if (System.IO.Path.GetExtension(path).ToLower() == ".dwxmz")
            {
                var xdoc = fm.FlowsheetObject.LoadZippedXML(path);
            }
            else if (System.IO.Path.GetExtension(path).ToLower() == ".dwxml")
            {
                fm.FlowsheetObject.LoadFromXML(XDocument.Load(path));
            }
            else if (System.IO.Path.GetExtension(path).ToLower() == ".xml")
            {
                fm.FlowsheetObject.LoadFromMXML(XDocument.Load(path));
            }
            fm.FlowsheetObject.FilePath = path;
            fm.FlowsheetObject.FlowsheetOptions.FilePath = path;
        }

        public void SaveFlowsheet2(IFlowsheet flowsheet, string filepath)
        {
            SaveFlowsheet(flowsheet, filepath, true);
        }
    }


}
