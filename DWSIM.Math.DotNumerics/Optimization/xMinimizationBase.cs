//Copyright © 2009 Jose Antonio De Santiago-Castillo 
//E-mail:JAntonioDeSantiago@gmail.com
//Web: www.DotNumerics.com
//

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace DotNumerics.Optimization
{
    /// <summary>
    /// Represents a base class for optimization classes. 
    /// </summary>
    public abstract class xMinimizationBase 
    {
        /// <summary>
        /// All the variables.
        /// </summary>
        protected double[] _Variables;

        /// <summary>
        /// The initial guess .
        /// </summary>
        protected double[] _FreeVariables;

        /// <summary>
        /// Desired accuracy for the solution.
        /// </summary>
        protected double _Tolerance = 1e-6;
        /// <summary>
        /// Maximum number of function evaluations.
        /// </summary>
        protected int _MaxFunEvaluations = 3000;

        /// <summary>
        /// The number of function evaluations used to compute the minimum.
        /// </summary>
        protected int _FunEvaluations = 0;


        /// <summary>
        /// The number of variables.
        /// </summary>
        protected int _NumFreeVariables = 0;


        //protected bool MeAreOptVariables = false;
        //protected bool MeAreBoundVariables = false;

        /// <summary>
        /// Maximum number of function evaluations.
        /// </summary>
        public int MaxFunEvaluations
        {
            get { return _MaxFunEvaluations; }
            set { _MaxFunEvaluations = value; }
        }

        /// <summary>
        /// The number of function evaluations used to compute the minimum.
        /// </summary>
        public int FunEvaluations
        {
            get { return _FunEvaluations; }
            set { _FunEvaluations = value; }
        }

        /// <summary>
        /// Desired accuracy for the solution.
        /// </summary>
        public double Tolerance
        {
            get { return _Tolerance; }
            set 
            {
                _Tolerance = value;
            }
        }


        //internal 
    }
}
