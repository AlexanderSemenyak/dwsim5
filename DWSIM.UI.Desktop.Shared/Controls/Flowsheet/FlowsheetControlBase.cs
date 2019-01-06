﻿using System;
using DWSIM.Interfaces.Enums.GraphicObjects;
using Eto.Forms;
namespace DWSIM.UI.Controls
{
    // control to use in your eto.forms code
    [Eto.Handler(typeof(IFlowsheetSurface))]
    public class FlowsheetSurfaceControlBase : Eto.Forms.Control
    {

        public DWSIM.Drawing.SkiaSharp.GraphicsSurface FlowsheetSurface;
        public DWSIM.UI.Desktop.Shared.Flowsheet FlowsheetObject;
                
        // interface to the platform implementations

        public interface IFlowsheetSurface : Eto.Forms.Control.IHandler
        {
        }

        public interface IFlowsheetSurface_OpenGL : Eto.Forms.Control.IHandler
        {
        }

        public void AddObject(string objtname, int x, int y)
        {

            string prep;
            var count = FlowsheetObject.SimulationObjects.Count;
            ObjectType objtype = ObjectType.Nenhum;
            switch (objtname)
            {
                case "Material Stream":
                    objtype = ObjectType.MaterialStream;
                    prep = "МАТП-";
                    break;
                case "Energy Stream":
                    objtype = ObjectType.EnergyStream;
                    prep = "ЭНРГ-";
                    break;
                case "Vessel":
                    objtype = ObjectType.Vessel;
                    prep = "СЕП-";
                    break;
                case "Mixer":
                    objtype = ObjectType.NodeIn;
                    prep = "СМЕСЬ-";
                    break;
                case "Splitter":
                    objtype = ObjectType.NodeOut;
                    prep = "РАЗД-";
                    break;
                case "Pump":
                    objtype = ObjectType.Pump;
                    prep = "НАСОС-";
                    break;
                case "Valve":
                    objtype = ObjectType.Valve;
                    prep = "КРАН-";
                    break;
                case "Heat Exchanger":
                    objtype = ObjectType.HeatExchanger;
                    prep = "HXC-";
                    break;
                case "Heater/Cooler":
                    objtype = ObjectType.HeaterCooler;
                    prep = "HCR-";
                    break;
                case "Compressor/Expander":
                    objtype = ObjectType.CompressorExpander;
                    prep = "КМПР-";
                    break;
                case "Recycle":
                    objtype = ObjectType.OT_Recycle;
                    prep = "УТИЛ-";
                    break;
                case "Shortcut Column":
                    objtype = ObjectType.ShortcutColumn;
                    prep = "SCN-";
                    break;
                case "Compound Separator":
                    objtype = ObjectType.ComponentSeparator;
                    prep = "СЕП-";
                    break;
                case "Conversion Reactor":
                    objtype = ObjectType.RCT_Conversion;
                    prep = "CR-";
                    break;
                case "Equilibrium Reactor":
                    objtype = ObjectType.RCT_Equilibrium;
                    prep = "ER-";
                    break;
                case "Gibbs Reactor":
                    objtype = ObjectType.RCT_Gibbs;
                    prep = "GR-";
                    break;
                case "CSTR":
                    objtype = ObjectType.RCT_CSTR;
                    prep = "CSTR-";
                    break;
                case "PFR":
                    objtype = ObjectType.RCT_PFR;
                    prep = "PFR-";
                    break;
                case "Distillation Column":
                    objtype = ObjectType.DistillationColumn;
                    prep = "DC-";
                    break;
                case "Absorption Column":
                    objtype = ObjectType.AbsorptionColumn;
                    prep = "AC-";
                    break;
                case "Pipe Segment":
                    objtype = ObjectType.Pipe;
                    prep = "ТРУБА-";
                    break;
                case "Adjust":
                    objtype = ObjectType.OT_Adjust;
                    prep = "НАСТР-";
                    break;
                case "Solids Separator":
                    objtype = ObjectType.SolidSeparator;
                    prep = "SS-";
                    break;
                default:
                    objtype = ObjectType.Nenhum;
                    prep = "";
                    break;
            }
            if (objtype == ObjectType.Nenhum)
            {
                if (objtname == "Text")
                {
                    var gobj = new DWSIM.Drawing.SkiaSharp.GraphicObjects.TextGraphic(x, y, "TEXT");
                    gobj.Name = Guid.NewGuid().ToString();
                    FlowsheetObject.AddGraphicObject(gobj);
                }
                else if (objtname == "Property Table")
                {
                    var gobj = new DWSIM.Drawing.SkiaSharp.GraphicObjects.Tables.TableGraphic(x, y);
                    gobj.Name = Guid.NewGuid().ToString();
                    gobj.Flowsheet = FlowsheetObject;
                    FlowsheetObject.AddGraphicObject(gobj);
                }
                else if (objtname == "Spreadsheet Table")
                {
                    var gobj = new DWSIM.Drawing.SkiaSharp.GraphicObjects.Tables.SpreadsheetTableGraphic(x, y);
                    gobj.Name = Guid.NewGuid().ToString();
                    gobj.Flowsheet = FlowsheetObject;
                    FlowsheetObject.AddGraphicObject(gobj);
                }
                else if (objtname == "Master Property Table")
                {
                    var gobj = new DWSIM.Drawing.SkiaSharp.GraphicObjects.Tables.MasterTableGraphic(x, y);
                    gobj.Name = Guid.NewGuid().ToString();
                    gobj.Flowsheet = FlowsheetObject;
                    FlowsheetObject.AddGraphicObject(gobj);
                }
                else if (objtname == "Chart Object")
                {
                    var gobj = new DWSIM.Drawing.SkiaSharp.GraphicObjects.Charts.OxyPlotGraphic(x, y);
                    gobj.Name = Guid.NewGuid().ToString();
                    gobj.Flowsheet = FlowsheetObject;
                    FlowsheetObject.AddGraphicObject(gobj);
                }
            }
            else
            {
                FlowsheetObject.AddObject(objtype, x, y, prep + (count + 1).ToString("000"));
            }
            Invalidate();

        }
    }
}


