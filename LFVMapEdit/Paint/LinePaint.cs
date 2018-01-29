using System;
using System.Collections.Generic;
using System.Text;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public class LinePaint: AbstractPaint
	{
		bool plota = false;
		int firstY = -1;
		int firstX = -1;

		int subMatIndex = -1;
		public override void MouseLeftDraw(int x, int y, Brick pbrk_NewBrick)
		{
			if (plota)
			{
				Map.SubCells.RemoveAt(subMatIndex);
				subMatIndex = -1;

				if (firstX == x)
				{
					if (firstY <= y)
						this.TracaEmX(firstX, firstY, y, pbrk_NewBrick);
					else
						this.TracaEmX(firstX, y, firstY, pbrk_NewBrick);
				}
				else if (firstY == y)
				{
					if (firstX <= x)
						this.TracaEmY(firstY, firstX, x, pbrk_NewBrick);
					else
						this.TracaEmY(firstY, x, firstX, pbrk_NewBrick);
				}
				else
					this.TracaReta(firstX, firstY, x, y, pbrk_NewBrick);
				plota = false;
			}
			else
			{
				firstX = x;
				firstY = y;
				plota = true;
				subMatIndex = this.Map.AddSubCell();
			}
		}

		public override void MouseRightDraw(int x, int y, Brick pbrk_NewBrick)
		{
			if (plota)
			{
				Map.SubCells.RemoveAt(subMatIndex);
				subMatIndex = -1;
				plota = false;
			}
		}

		public override void MouseLeftMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
		}

		public override void MouseRightMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
		}

		public override void MouseMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
			if (plota)
			{
				this.Map.ClearSubCell(subMatIndex);
				if (firstX == x)
				{
					if (firstY <= y)
						this.TracaEmX(firstX, firstY, y, pbrk_NewBrick);
					else
						this.TracaEmX(firstX, y, firstY, pbrk_NewBrick);
				}
				else if (firstY == y)
				{
					if (firstX <= x)
						this.TracaEmY(firstY, firstX, x, pbrk_NewBrick);
					else
						this.TracaEmY(firstY, x, firstX, pbrk_NewBrick);
				}
				else
					this.TracaReta(firstX, firstY, x, y, pbrk_NewBrick);
			}
		}

		public double CalcCofAngular(int x1, int y1, int x2, int y2)
		{
			double dividendo = y2 - y1;
			double divisor = x2 - x1;
			return dividendo / divisor;
		}

		public double CalcB(double cofAngular, int x, int y)
		{
			return (double)y - cofAngular * (double)x;
		}

		public double CalcD_Bresenham(double cofAngular, double b, int xP, int yP)
		{
			return cofAngular * (double)(xP + 1) + b - ((double)yP + 0.5);
		}

		public int IncY(int x1, int y1, int x2, int y2, int xP, int yP)
		{
			double cofAngular = CalcCofAngular(x1, y1, x2, y2);
			double b = CalcB(cofAngular, x1, y1);
			double d = CalcD_Bresenham(cofAngular, b, xP, yP);

			return d > 0 ? 1 : 0;
		}

		public void TracaReta(int x1, int y1, int x2, int y2, Brick pbrk_Brick)
		{
			int pX1 = x1;
			int pY1 = y1;
			int pX2 = x2;
			int pY2 = y2;

			if (x1 > x2)
			{
				pX1 = x2;
				pY1 = y2;
				pX2 = x1;
				pY2 = y1;
			}

			bool yInvertido = false;
			bool xTrocaY = false;
			double cofAngular = CalcCofAngular(pX1, pY1, pX2, pY2);
			bool varYmaiorX = ((pY2 - pY1) < -1 ? (pY1 - pY2) : (pY2 - pY1)) > ((pX2 - pX1) < -1 ? (pX1 - pX2) : (pX2 - pX1));

			if (cofAngular > 1 || varYmaiorX)
			{
				xTrocaY = true;
				pX1 = y1;
				pY1 = x1;
				pX2 = y2;
				pY2 = x2;
				cofAngular = CalcCofAngular(pX1, pY1, pX2, pY2);
			}

			if (cofAngular < 0)
			{
				yInvertido = true;
				pY1 = -pY1;
				pY2 = -pY2;
				cofAngular = CalcCofAngular(pX1, pY1, pX2, pY2);
			}

			if (pX1 > pX2)
			{
				int xA = pX1;
				int yA = pY1;
				pX1 = pX2;
				pY1 = pY2;
				pX2 = xA;
				pY2 = yA;
			}

			int pYPlota = pY1;
			for (int i = pX1; i <= pX2; i++)
			{
				PlotaPonto(yInvertido, xTrocaY, i, pYPlota, pbrk_Brick);
				pYPlota += IncY(pX1, pY1, pX2, pY2, i, pYPlota);
			}
		}

		public void PlotaPonto(bool yInvertido, bool xTrocaY, int x, int y, Brick pbrk_Brick)
		{
			if (yInvertido)
				y = -y;

			if (xTrocaY)
			{
				int aux = y;
				y = x;
				x = aux;
			}

			if(subMatIndex == -1)
				Map.Cells[x, y] = Util.GetMapCell(pbrk_Brick);
			else
				Map.SubCells[subMatIndex][x, y] = Util.GetMapCell(pbrk_Brick);
		}

		public void TracaEmX(int x, int y1, int y2, Brick pbrk_Brick)
		{
			for (int y = y1; y <= y2; y++)
			{
				if (subMatIndex == -1)
					Map.Cells[x, y] = Util.GetMapCell(pbrk_Brick);
				else
					Map.SubCells[subMatIndex][x, y] = Util.GetMapCell(pbrk_Brick);
			}
		}

		public void TracaEmY(int y, int x1, int x2, Brick pbrk_Brick)
		{
			for (int x = x1; x <= x2; x++)
			{
				if (subMatIndex == -1)
					Map.Cells[x, y] = Util.GetMapCell(pbrk_Brick);
				else
					Map.SubCells[subMatIndex][x, y] = Util.GetMapCell(pbrk_Brick);
			}
		}
	}
}
