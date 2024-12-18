using System;
using System.Runtime.InteropServices;

public static class WindowHelper
{
	
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}	
	
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
	// Special value to keep window always on top
	public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
	public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

	// Flags for SetWindowPos
	public const uint SWP_NOMOVE = 0x0002;
	public const uint SWP_NOSIZE = 0x0001;
	public const uint SWP_SHOWWINDOW = 0x0040;
}
