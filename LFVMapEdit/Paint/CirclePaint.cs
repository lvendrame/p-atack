using System;
using System.Collections.Generic;
using System.Text;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public class CirclePaint: AbstractPaint
	{
		bool plota = false;
		int firstY = -1;
		int firstX = -1;

		int subCellIndex = -1;
		public override void MouseLeftDraw(int x, int y, Brick pbrk_NewBrick)
		{
			if (plota)
			{
				this.Map.SubCells.RemoveAt(subCellIndex);
				subCellIndex = -1;

				int raio = Convert.ToInt32(CalculaDistancia(firstX, firstY, x, y));
				this.TracaCirculo(firstX, firstY, raio, pbrk_NewBrick);
				plota = false;
			}
			else
			{
				firstX = x;
				firstY = y;
				plota = true;

				subCellIndex = this.Map.AddSubCell();
			}
		}

		public override void MouseRightDraw(int x, int y, Brick pbrk_NewBrick)
		{
			if (plota)
			{
				this.Map.SubCells.RemoveAt(subCellIndex);
				subCellIndex = -1;
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
				this.Map.ClearSubCell(subCellIndex);

				int raio = Convert.ToInt32(CalculaDistancia(firstX, firstY, x, y));
				this.TracaCirculo(firstX, firstY, raio, pbrk_NewBrick);
			}
		}

		public double CalculaDistancia(int x1, int y1, int x2, int y2)
		{
			int x = x2 - x1;
			int y = y2 - y1;
			x = x * x;
			y = y * y;
			return Math.Sqrt(x + y);
		}

		public void TracaCirculo(int xE, int yE, int raioEnt, Brick pbrk_Brick)
		{
			int raio = raioEnt;
			int y = raio;
			for (int x = 0; x <= y; x++)
			{
				PintaTotal(x, y, xE, yE, pbrk_Brick);
				y -= DecY(x, y, raio);
			}
		}

		public double CalcD(int xP, int yP, int raio)
		{
			double part1 = xP + 1;
			double part2 = (double)yP - 0.5;
			return (part1 * part1 + part2 * part2 - raio * raio);
		}

		public int DecY(int xP, int yP, int raio)
		{
			return (CalcD(xP, yP, raio) > 0) ? 1 : 0;
		}

		public void PintaTotal(int x, int y, int xC, int yC, Brick pbrk_Brick)
		{
			PintaPontos(x, y, xC, yC, pbrk_Brick);
			PintaPontos(y, x, xC, yC, pbrk_Brick);
		}

		public void PintaPontos(int x, int y, int xC, int yC, Brick pbrk_Brick)
		{
			PintaPonto(x + xC, y + yC, pbrk_Brick);
			PintaPonto(-x + xC, y + yC, pbrk_Brick);
			PintaPonto(x + xC, -y + yC, pbrk_Brick);
			PintaPonto(-x + xC, -y + yC, pbrk_Brick);
		}

		public void PintaPonto(int x, int y, Brick pbrk_Brick)
		{
			if (Util.IsValidPoint(this.Map, x, y))
			{
				if (subCellIndex == -1)
					this.Map.Cells[x, y] = Util.GetMapCell(pbrk_Brick);
				else
					this.Map.SubCells[subCellIndex][x, y] = Util.GetMapCell(pbrk_Brick);
			}
		}
	}
}
