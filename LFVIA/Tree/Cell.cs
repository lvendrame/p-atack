using System;
using System.Collections.Generic;
using System.Text;

namespace LFVIA.Tree
{
    public class Cell: List<Cell>
    {

        public Cell()
        {
        }

        public Cell(double pdblHeight)
        {
            this.dblHeight = pdblHeight;
            this.celParent = null;
        }

        public Cell(double pdblHeight, Cell parent)
        {
            this.dblHeight = pdblHeight;
            this.celParent = parent;
        }

        public Cell(double pdblHeight, Cell parent, params Cell[] child)
        {
            this.dblHeight = pdblHeight;
            this.celParent = parent;
            this.Children.AddRangeAndSetParent(child);
        }

        public Cell(double pdblHeight, Cell parent, IEnumerable<Cell> child)
        {
            this.dblHeight = pdblHeight;
            this.celParent = parent;
            this.Children.AddRangeAndSetParent(child);
        }

        public Cell(double pdblHeight, params Cell[] child)
        {
            this.dblHeight = pdblHeight;
            this.celParent = null;
            this.Children.AddRangeAndSetParent(child);
        }

        public Cell(double pdblHeight, Cell parent, IEnumerable<Cell> child)
        {
            this.dblHeight = pdblHeight;
            this.celParent = null;
            this.Children.AddRangeAndSetParent(child);
        }

        private double dblHeight;
        public double Height
        {
            get { return dblHeight; }
            set { dblHeight = value; }
        }

        private Cell celParent = null;
        public Cell Parent
        {
            get { return celParent; }
            set { celParent = value; }
        }
        
        public List<Cell> Children
        {
            get { return this; }
        }

        public void AddRangeAndSetParent(IEnumerable<Cell> values)
        {
            foreach (Cell item in values)
            {
                item.Parent = this;
                this.Add(item);
            }
        }

    }
}
