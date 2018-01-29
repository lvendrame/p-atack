using System;
using System.Collections.Generic;
using System.Text;

namespace LFVIA.Tree
{
    public class CellTree
    {
        private Cell celHead;
        public Cell Head
        {
            get { return celHead; }
            set { celHead = value; }
        }

        public void Add(Cell parent, Cell item)
        {
            parent.Add(item);
            item.Parent = parent;
        }

        public void Remove(Cell item)
        {
            Cell parent = item.Parent;
            parent.Remove(item);
            item.Parent = null;
            parent.AddRangeAndSetParent(item.Children);            
        }

        public List<Cell> FindPath(Cell startPath, Cell finishPath)
        {
            List<Cell> lstBestPath = new List<Cell>();
            List<Cell> usedsPaths = new List<Cell>();

            double minHeight = -1;
            double height = FindBestPath(startPath, finishPath, usedsPaths, lstBestPath, 0.0, ref minHeight);
        }

        private double FindBestPath(Cell startPath, Cell finishPath, List<Cell> usedsPaths, List<Cell> lstBestPath, double acumulatedHeight, ref double minHeight)
        {
            acumulatedHeight += startPath.Height;
            foreach (Cell item in startPath)
            {
                usedsPaths.Add(item);

                if (item == finishPath)
                {
                    lstBestPath.Add(item);
                    return item.Height;
                }
            }
        }

    }
}
