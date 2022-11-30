﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace NPOI.OpenXmlFormats.Spreadsheet
{

    [Serializable]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    public class CT_Cols
    {

        private List<CT_Col> colField = new List<CT_Col>(); // required

        //public CT_Cols()
        //{
        //    this.colField = new List<CT_Col>();
        //}
        public void SetColArray(List<CT_Col> array)
        {
            colField = array;
        }
        public CT_Col AddNewCol()
        {
            if (null == colField)
            {
                colField = new List<CT_Col>();
            }

            CT_Col newCol = new CT_Col();
            this.colField.Add(newCol);
            return newCol;
        }

        public CT_Col InsertNewCol(int index)
        {
            if (null == colField)
            {
                colField = new List<CT_Col>();
            }

            CT_Col newCol = new CT_Col();
            this.colField.Insert(index, newCol);
            return newCol;
        }
        public void RemoveCol(int index)
        {
            this.colField.RemoveAt(index);
        }
        public void RemoveCols(IList<CT_Col> toRemove)
        {
            if (colField == null) return;
            foreach (CT_Col c in toRemove)
            {
                colField.Remove(c);
            }
        }
        public int sizeOfColArray()
        {
            return col.Count;
        }
        public CT_Col GetColArray(int index)
        {
            return colField[index];
        }


        public List<CT_Col> GetColList()
        {
            return colField;
        }
        [XmlElement]
        public List<CT_Col> col
        {
            get
            {
                return this.colField;
            }
            set
            {
                this.colField = value;
            }
        }

        public static CT_Cols Parse(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            if (node == null)
                return null;
            CT_Cols ctObj = new CT_Cols();
            ctObj.col = new List<CT_Col>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.LocalName == "col")
                {
                    CT_Col ctCol = CT_Col.Parse(childNode, namespaceManager);

                    if (ctCol.min != ctCol.max)
                    {
                        BreakUpCtCol(ctObj, ctCol);
                    }
                    else
                    {
                        ctObj.col.Add(ctCol);
                    }
                }
            }

            return ctObj;
        }

        /// <summary>
        /// For ease of use of columns in NPOI break up <see cref="CT_Col"/>s
        /// that span over multiple physical columns into individual
        /// <see cref="CT_Col"/>s for each physical column.
        /// </summary>
        /// <param name="ctObj"></param>
        /// <param name="ctCol"></param>
        private static void BreakUpCtCol(CT_Cols ctObj, CT_Col ctCol)
        {
            for (int i = (int)ctCol.min; i <= (int)ctCol.max; i++)
            {
                CT_Col breakOffCtCol = ctCol.Copy();
                breakOffCtCol.min = (uint)i;
                breakOffCtCol.max = (uint)i;

                ctObj.col.Add(breakOffCtCol);
            }
        }

        internal void Write(StreamWriter sw, string nodeName)
        {
            sw.Write(string.Format("<{0}", nodeName));
            sw.Write(">");
            if (this.col != null)
            {
                foreach (CT_Col x in this.col)
                {
                    x.Write(sw, "col");
                }
            }
            sw.Write(string.Format("</{0}>", nodeName));
        }



        public void SetColArray(CT_Col[] colArray)
        {
            this.colField = new List<CT_Col>(colArray);
        }
    }
}
