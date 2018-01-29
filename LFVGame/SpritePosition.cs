using System;
using System.Collections.Generic;
using System.Text;

namespace LFVGame
{
	public class SpriteIndex
	{
		private Dictionary<ObjectStatus, SpritePosition> dicSpritesIndex = new Dictionary<ObjectStatus,SpritePosition>();
		public Dictionary<ObjectStatus, SpritePosition> SpriteIndexDic
		{
			get { return dicSpritesIndex; }
		}

		public int GetSpriteIndex(SpritePositionEnum spritePosition, ObjectStatus status)
		{
			if (dicSpritesIndex.ContainsKey(status))
			{
				SpritePosition pos = dicSpritesIndex[status];
				switch (spritePosition)
				{
					case SpritePositionEnum.UPPER:
						return pos.UPPER;
					case SpritePositionEnum.DOWN:
						return pos.DOWN;
					case SpritePositionEnum.LEFT:
						return pos.LEFT;
					case SpritePositionEnum.RIGHT:
						return pos.RIGHT;
					case SpritePositionEnum.DOWN_LEFT:
						return pos.DOWN_LEFT;
					case SpritePositionEnum.DOWN_RIGHT:
						return pos.DOWN_RIGHT;
					case SpritePositionEnum.UPPER_LEFT:
						return pos.UPPER_LEFT;
					case SpritePositionEnum.UPPER_RIGHT:
						return pos.UPPER_RIGHT;
				}
			}
			return -1;
		}
	}

	public class SpritePosition
	{
		public int UPPER, UPPER_LEFT, UPPER_RIGHT;
		public int DOWN, DOWN_LEFT, DOWN_RIGHT;
		public int LEFT, RIGHT;
	}

	public enum SpritePositionEnum
	{
		UPPER, UPPER_LEFT, UPPER_RIGHT,
		DOWN, DOWN_LEFT, DOWN_RIGHT,
		LEFT, RIGHT
	}

	public enum ObjectStatus
	{
		Stop,
		Normal,
		Move,
		Atack,
		Defense,
		Born,
		Die
	}
}
