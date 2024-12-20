using System;
using System.Threading.Tasks;
using Editor;
using Sandbox;

namespace YAAI;

public sealed class YaaiWindow : Window
{
	public static int Count = 0;
	public int ID = 0;

	public Task RunningTask;
	private bool IsRunning = false;
	private SoundHandle yaaiSound;

	public readonly int MinimumSpeed = 20;
	public readonly int MaximumSpeed= 70;
	
	public YaaiWindow()
	{
		ID = Count++;
		WindowTitle = $"YAAI - {ID} ";

		Width = 250;
		Height = 200;
		HasMaximizeButton = false;
		DeleteOnClose = true;
		Layout = Layout.Column();
		Layout.Add( new YaaiWidget(this ) );
		yaaiSound = Sound.PlayFile( SoundFile.Load( "yaaisound.mp3" ) );
		Show();
		RunningTask = Move();
	}

	protected override bool OnClose()
	{
		yaaiSound.Stop(  );
		yaaiSound.Dispose();
		
		// Dispose of Task
		IsRunning = false;
		RunningTask.Wait();
		RunningTask.Dispose();
		
		
		YaaiManager.OnWindowClose( this );
		
		return base.OnClose();
	}

	async Task Move()
	{
		IsRunning = true;
		
		Random random = new Random();
		bool moveDown = random.Int( 0,1 ) == 1;
		bool moveRight = random.Int( 0,1 ) == 1;
		int Speed = random.Int(MinimumSpeed, MaximumSpeed);
		
		while ( IsWindow && IsRunning )
		{
			IntPtr hWnd = WindowHelper.FindWindow( null, WindowTitle );
			if ( !WindowHelper.GetWindowRect( hWnd, out WindowHelper.RECT rect ) ) return;
			
            // Keep the window always on top
			WindowHelper.SetWindowPos(hWnd, WindowHelper.HWND_TOPMOST, 0, 0, 0, 0, WindowHelper.SWP_NOMOVE | WindowHelper.SWP_NOSIZE | WindowHelper.SWP_SHOWWINDOW);
			
			int x = rect.Left;
			int y = rect.Top;
			int currentWidth = rect.Right - rect.Left;
			int currentHeight = rect.Bottom - rect.Top;

			int nx = moveRight ? x + Speed/2 : x - Speed/2;
			int ny = moveDown ? y + Speed : y - Speed;

			bool shouldChangeSpeed = false;
			if ( rect.Right >= ScreenGeometry.Width )
			{
				moveRight = false;
				shouldChangeSpeed = true;
			}
			else if ( rect.Left <= 0 )
			{
				moveRight = true;
				shouldChangeSpeed = true;
			}

			if ( rect.Bottom >= ScreenGeometry.Height )
			{
				moveDown = false;
				shouldChangeSpeed = true;
			}
			else if ( rect.Top <= 0 )
			{
				moveDown = true;
				shouldChangeSpeed = true;
			}
			
			if (shouldChangeSpeed)
				Speed =  random.Int( MinimumSpeed, MaximumSpeed);
			
			WindowHelper.MoveWindow( hWnd, nx, ny, currentWidth, currentHeight, false);
			await Task.Delay( 25 );
		}
		
	}

}
