using System;
using System.Drawing;
namespace LFVGame
{
	public interface IGameStage: IDisposable
	{
		bool IsLoad { get; }
		bool IsFinish { get; }
		bool IsDrawing { get; }
		bool IsPaused { get; }
		bool IsStopedMove { get;}
        bool ExitGame { get; }
		bool LockUser { get; }
        StageState State { get; set; }
		Image StageImage { get; }
		void CheckGameInputs();
		void CheckMainInputs();
		void LoadComponents();
		void Redraw(double elapsedTime);
		void UnloadComponents();
		void Update(double elapsedTime);
	}

    public enum StageState
    {
        Loading,
        Pause,
        Finished,
        Running,
        Stop,
        ExitGame
    }
}
