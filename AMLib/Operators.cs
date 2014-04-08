using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMLib
{
    [Serializable]
    public class Operators
    {
        private List<Operator> _opList;

        public int Size
        {
            get { return _opList.Count; }
        }

        public Operators()
        {
            _opList = new List<Operator>();
        }

        public bool AddToEnd(Operator op)
        {
            int sBefore = 0, sAfter = 0;
            sBefore = _opList.Count;

            _opList.Add(op);

            sAfter = _opList.Count;
            if (sAfter == sBefore)
                return false;
            return true;
        }

        public bool AddBeforeCurrent(int position, Operator op)
        {
            if (position > Size) return false;
            int sBefore = 0, sAfter = 0;
            sBefore = _opList.Count;

            _opList.Insert(position - 1, op);

            sAfter = _opList.Count;
            if (sAfter == sBefore)
                return false;
            return true;
        }

        public bool ReplaceCurrent(int position, Operator op)
        {
            if (position > Size)   
                return false;
            _opList.RemoveAt(position - 1);
            _opList.Insert(position - 1, op);
            return true;
        }

        public List<Operator> GetList()
        {
            return _opList;
        }

        public void Clear()
        {
            _opList.RemoveRange(0, Size);
        }

        public void DeleteCurrent(int position)
        {
            if (position > 0 && position <= Size)
            {
                _opList.RemoveAt(position-1);
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (Operator op in _opList)
            {
                str.Append(op.ToString());
                str.Append(" ");
            }
            return str.ToString();
        }

        public Operator GetOperator(int pos)
        {
            return _opList.ElementAt<Operator>(pos);
        }

    }
}
