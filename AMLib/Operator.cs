using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMLib
{
    [Serializable]
    public class Operator
    {
        private string _name;
        private string _algorithmModel;
        private string _operatorModel;
        //*************************
        private int _porNumber;
        private string _realization;
        //private string _function;
        private string[] _function;
        private string[] _inParams;
        private string[] _outParams;
        private int _inCount;
        private int _outCount;


        public Operator()
        {
            _name = null;
            _algorithmModel = null;
            _operatorModel = null;
        }

        public void SetFunctions(string s)
        {
            string[] sArr = s.Split(';');
            int N = sArr.GetLength(0);
            _function = new string[N];
            for (int i = 0; i < N; i++)
            {
                _function[i] = sArr[i];
            }
        }

        public void SetInParams(string s)
        {
            string[] sArr = s.Split(';');
            _inParams = new string[_inCount];
            for (int i = 0; i < _inCount; i++)
            {
                _inParams[i] = sArr[i];
            }
        }

        public void SetOutParams(string s)
        {
            string[] sArr = s.Split(';');
            _outParams = new string[_outCount];
            for (int i = 0; i < _outCount; i++)
            {
                _outParams[i] = sArr[i];
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string AlgorithmModel
        {
            get { return _algorithmModel; }
            set { _algorithmModel = value; }
        }

        public string OperatorModel
        {
            get { return _operatorModel; }
            set { _operatorModel = value; }
        }

        public string Realization
        {
            get { return _realization; }
            set { _realization = value; }
        }

        /*public string Function
        {
            get { return _function; }
            set { _function = value; }
        }*/

        public int In
        {
            get { return _inCount; }
            set { _inCount = value; }
        }

        public int PorNubmer
        {
            get { return _porNumber; }
            set { _porNumber = value; }
        }

        public int Out
        {
            get { return _outCount; }
            set { _outCount = value; }
        }

        public override string ToString()
        {
            return _algorithmModel;
        }

        public string GetInParam(int i)
        {
            if (i >= 0 && i < _inCount)
                return _inParams[i];
            else
                return null;
        }

        public string GetOutParam(int i)
        {
            if (i >= 0 && i < _outCount)
                return _outParams[i];
            else
                return null;
        }

        public string GetFunction(int i)
        {
            if (i >= 0 && i < 3)
                return _function[i];
            else
                return null;
        }
    }
}
