using Editor;
using Sandbox;
using FileSystem = Editor.FileSystem;

namespace YAAI;

class YaaiWidget : Widget
{
	string blackimage = FileSystem.Mounted.GetFullPath("idiotblack.jpg");
	string whiteimage = FileSystem.Mounted.GetFullPath("idiotwhite.jpg");

	internal RealTimeUntil colorchange;
	internal int color = 0;

	public YaaiWidget( Widget parent = null )
	{
		Parent = parent;
		colorchange = 1;
	}

	protected override void OnPaint()
	{
		if ( colorchange )
		{
			color = color == 1 ? 0 : 1;
			colorchange = 1;
		}
		
		Update();
		var img = color == 0 ? whiteimage : blackimage;
		Paint.Draw(Parent.LocalRect, img);
		
	}
}
