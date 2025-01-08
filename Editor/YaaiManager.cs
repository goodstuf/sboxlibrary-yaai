using System.Collections.Generic;
using System.Threading.Tasks;
using Editor;
using Sandbox;

namespace YAAI;

public static class YaaiManager
{
	public static EditorMainWindow MainWindow;
	public static bool hasBegun { get; private set; } = false;
	public static readonly int MaxWindows = 16;
	public static readonly int SpawnRate = 3;
	public static List<YaaiWindow> Windows = new(MaxWindows);

	[Event( "editor.created" )]
	public static async void OnEditorCreated(EditorMainWindow mainWindow)
	{
		if ( hasBegun ) return;
		hasBegun = true;
		
		MainWindow = mainWindow;
		await Task.Delay( 5000 );
		mainWindow.Title = "YAAI";
		var MainWidget = new YaaiWidget( SceneOverlayWidget.Active );
		MainWidget.OnPaintOverride = () =>
		{
			if ( !MainWidget.colorchange ) return false;
			var color = MainWidget.color == 0 ? "black" : "white";
			MainWindow.SetStyles( $"background: {color}; color: {color};" );
			
			return false;
		};
		
		MainWidget.Show();
		await Task.Delay( 8000 );
		SpawnWindows();
		
	}
	
	[Event("hotloaded")]
	public static async void OnHotloaded()
	{
		OnEditorCreated(MainWindow);
	}

	static void SpawnWindows()
	{
		for ( int i = 0; i < SpawnRate; i++ )
		{
			if ( Windows.Count >= MaxWindows ) break;
		
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
