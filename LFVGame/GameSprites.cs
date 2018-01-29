using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LFVGame
{
	public static class GameSprites
	{

		static Color transColor = Color.FromArgb(0, 67, 171);

		//public static SpriteIndex AirPlane_Yellow = GameSprites.InitializeAirPlane_Yellow();
		//public static SpriteIndex AirPlane_Blue = GameSprites.InitializeAirPlane_Blue();
		//public static SpriteIndex AirPlane_LightGreen = GameSprites.InitializeAirPlane_LightGreen();
		//public static SpriteIndex AirPlane_White = GameSprites.InitializeAirPlane_White();
		public static SpriteIndex AirPlane_Green = GameSprites.InitializeAirPlane_Green();

		private static SpriteIndex InitializeAirPlane_Yellow()
		{
			SpritePosition pos = new SpritePosition();
			pos.UPPER = StaticImages.AddSpriteFromGrid(136, 4, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_LEFT = StaticImages.AddSpriteFromGrid(103, 4, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_RIGHT = StaticImages.AddSpriteFromGrid(139, 4, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN = StaticImages.AddSpriteFromGrid(4, 4, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_LEFT = StaticImages.AddSpriteFromGrid(37, 4, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_RIGHT = StaticImages.AddSpriteFromGrid(235, 4, 31, 31, Resource.PlanesGrid, transColor);
			pos.LEFT = StaticImages.AddSpriteFromGrid(169, 4, 31, 31, Resource.PlanesGrid, transColor, RotateFlipType.RotateNoneFlipY);
			pos.RIGHT = StaticImages.AddSpriteFromGrid(169, 4, 31, 31, Resource.PlanesGrid, transColor);
			SpriteIndex index = new SpriteIndex();
			index.SpriteIndexDic.Add(ObjectStatus.Normal, pos);
			return index;
		}

		private static SpriteIndex InitializeAirPlane_Blue()
		{
			SpritePosition pos = new SpritePosition();
			pos.UPPER = StaticImages.AddSpriteFromGrid(136, 37, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_LEFT = StaticImages.AddSpriteFromGrid(103, 37, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_RIGHT = StaticImages.AddSpriteFromGrid(139, 37, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN = StaticImages.AddSpriteFromGrid(4, 37, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_LEFT = StaticImages.AddSpriteFromGrid(37, 37, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_RIGHT = StaticImages.AddSpriteFromGrid(235, 37, 31, 31, Resource.PlanesGrid, transColor);
			pos.LEFT = StaticImages.AddSpriteFromGrid(169, 37, 31, 31, Resource.PlanesGrid, transColor, RotateFlipType.RotateNoneFlipY);
			pos.RIGHT = StaticImages.AddSpriteFromGrid(169, 37, 31, 31, Resource.PlanesGrid, transColor);
			SpriteIndex index = new SpriteIndex();
			index.SpriteIndexDic.Add(ObjectStatus.Normal, pos);
			return index;
		}

		private static SpriteIndex InitializeAirPlane_LightGreen()
		{
			SpritePosition pos = new SpritePosition();
			pos.UPPER = StaticImages.AddSpriteFromGrid(136, 70, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_LEFT = StaticImages.AddSpriteFromGrid(103, 70, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_RIGHT = StaticImages.AddSpriteFromGrid(139, 70, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN = StaticImages.AddSpriteFromGrid(4, 70, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_LEFT = StaticImages.AddSpriteFromGrid(37, 70, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_RIGHT = StaticImages.AddSpriteFromGrid(235, 70, 31, 31, Resource.PlanesGrid, transColor);
			pos.LEFT = StaticImages.AddSpriteFromGrid(169, 70, 31, 31, Resource.PlanesGrid, transColor, RotateFlipType.RotateNoneFlipY);
			pos.RIGHT = StaticImages.AddSpriteFromGrid(169, 70, 31, 31, Resource.PlanesGrid, transColor);
			SpriteIndex index = new SpriteIndex();
			index.SpriteIndexDic.Add(ObjectStatus.Normal, pos);
			return index;
		}

		private static SpriteIndex InitializeAirPlane_White()
		{
			SpritePosition pos = new SpritePosition();
			pos.UPPER = StaticImages.AddSpriteFromGrid(136, 103, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_LEFT = StaticImages.AddSpriteFromGrid(103, 103, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_RIGHT = StaticImages.AddSpriteFromGrid(139, 103, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN = StaticImages.AddSpriteFromGrid(4, 103, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_LEFT = StaticImages.AddSpriteFromGrid(37, 103, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_RIGHT = StaticImages.AddSpriteFromGrid(235, 103, 31, 31, Resource.PlanesGrid, transColor);
			pos.LEFT = StaticImages.AddSpriteFromGrid(169, 103, 31, 31, Resource.PlanesGrid, transColor, RotateFlipType.RotateNoneFlipY);
			pos.RIGHT = StaticImages.AddSpriteFromGrid(169, 103, 31, 31, Resource.PlanesGrid, transColor);
			SpriteIndex index = new SpriteIndex();
			index.SpriteIndexDic.Add(ObjectStatus.Normal, pos);
			return index;
		}

		private static SpriteIndex InitializeAirPlane_Green()
		{
			SpritePosition pos = new SpritePosition();
			pos.UPPER = StaticImages.AddSpriteFromGrid(136, 136, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_LEFT = StaticImages.AddSpriteFromGrid(103, 136, 31, 31, Resource.PlanesGrid, transColor);
			pos.UPPER_RIGHT = StaticImages.AddSpriteFromGrid(139, 136, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN = StaticImages.AddSpriteFromGrid(4, 136, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_LEFT = StaticImages.AddSpriteFromGrid(37, 136, 31, 31, Resource.PlanesGrid, transColor);
			pos.DOWN_RIGHT = StaticImages.AddSpriteFromGrid(235, 136, 31, 31, Resource.PlanesGrid, transColor);
			pos.LEFT = StaticImages.AddSpriteFromGrid(169, 136, 31, 31, Resource.PlanesGrid, transColor, RotateFlipType.RotateNoneFlipY);
			pos.RIGHT = StaticImages.AddSpriteFromGrid(169, 136, 31, 31, Resource.PlanesGrid, transColor);
			SpriteIndex index = new SpriteIndex();
			index.SpriteIndexDic.Add(ObjectStatus.Normal, pos);
			return index;
		}

		public static class ExplosionSprites
		{
			public static int StageOne = StaticImages.AddSpriteFromGrid(70, 169, 31, 31, Resource.PlanesGrid, transColor);
			public static int StageTwo = StaticImages.AddSpriteFromGrid(103, 169, 31, 31, Resource.PlanesGrid, transColor);
			public static int StageThree = StaticImages.AddSpriteFromGrid(136, 169, 31, 31, Resource.PlanesGrid, transColor);
			public static int StageFour = StaticImages.AddSpriteFromGrid(169, 169, 31, 31, Resource.PlanesGrid, transColor);
			public static int StageFive = StaticImages.AddSpriteFromGrid(202, 169, 31, 31, Resource.PlanesGrid, transColor);
			public static int StageSix = StaticImages.AddSpriteFromGrid(235, 169, 31, 31, Resource.PlanesGrid, transColor);

			public static int GetIndex(int stage)
			{
				switch (stage)
				{
					case 0:
						return StageOne;
					case 1:
						return StageTwo;
					case 2:
						return StageThree;
					case 3:
						return StageFour;
					case 4:
						return StageFive;
					default:
						return StageSix;
				}
			}
		}
	}
}
