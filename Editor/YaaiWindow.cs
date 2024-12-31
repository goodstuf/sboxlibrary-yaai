using System;
using System.Threading.Tasks;
using Editor;
using Sandbox;

namespace YAAI;

public sealed class YaaiWindow : Window
{
	public Task RunningTask;
	private bool IsRunning = false;
	private SoundHandle yaaiSound;
	
	
	public readonly int MinimumSpeed = 10;
	public readonly int MaximumSpeed = 30;
	
	private Random random = new Random();
	private float RandomSpeed => random.Float( MinimumSpeed, MaximumSpeed );
	public YaaiWindow()
	{
		WindowTitle = "YAAI";

		Width = 250;
		Height = 200;
		HasMaximizeButton = false;
		DeleteOnClose = true;
		WindowFlags = WindowFlags.WithFlag( WindowFlags.WindowStaysOnTopHint, true );
		WindowFlags = WindowFlags.WithFlag( WindowFlags.MinimizeButton, false);
		
		new YaaiWidget(this );
		
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
		
		bool moveDown = random.Int( 0,1 ) == 1;
		bool moveRight = random.Int( 0,1 ) == 1;
		float Speed = RandomSpeed;
		
		while ( IsWindow && IsRunning )
		{
			float nx = moveRight ? Position.x + Speed/2 : Position.x - Speed/2;
			float ny = moveDown ? Position.y + Speed : Position.y - Speed;

			bool shouldChangeSpeed = false;
			if ( Position.x + Width > ScreenGeometry.Width )
			{
				shouldChangeSpeed = true;
				moveRight = false;
			}
			else if ( Position.x < 0)
			{
				shouldChangeSpeed = true;
				moveRight = true;
			}
			
			if ( Position.y + Height > ScreenGeometry.Height )
			{
				shouldChangeSpeed = true;
				moveDown = false;
			}
			else if ( Position.y < 0)
			{
				shouldChangeSpeed = true;
				moveDown = true;
			}

			if ( shouldChangeSpeed )
				Speed = RandomSpeed;

			Position = Position.WithX( nx ).WithY( ny );
			await Task.Delay( 25 );
		}
		
	}

}
