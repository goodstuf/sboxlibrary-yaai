using System.Collections.Generic;
using Editor;
using Sandbox;

namespace YAAI;

public static class YaaiManager 
{
	static bool FirstRun = true;
	private static RealTimeUntil ShouldBegin;
	static bool SpawnedWindows = false;
	
	public static readonly int MaxWindows = 16;
	public static readonly int SpawnRate = 3;
	public static List<YaaiWindow> Windows = new(MaxWindows);

	
	[EditorEvent.FrameAttribute]
	public static void Frame()
	{
		if ( SpawnedWindows ) return;
		if ( FirstRun )
		{
			ShouldBegin = 5;
			FirstRun = false;
			return;
		}

		if ( !ShouldBegin ) return;
		if ( Windows.Count > 0 ) return;
		SpawnWindows();
	}


	public static void SpawnWindows()
	{
		SpawnedWindows = true;
		
		for ( int i = 0; i < SpawnRate; i++ )
		{
			if ( Windows.Count+1 >= MaxWindows ) break;
		
			var window = new YaaiWindow();
			Windows.Add( window );
		}
	}

	public static void OnWindowClose( YaaiWindow window )
	{
		Windows.Remove( window );
		
		if (EditorWindow.IsWindow)
			SpawnWindows();
	}
}
