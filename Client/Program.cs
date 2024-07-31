using System;
using System.Windows.Forms;
using AGConsole.Engine.Render;
namespace AGConsole.Client
{
	/// <summary>
	/// Main file that starts with the Application
	/// Functionality:
	/// - Applying Control from the Window List to needed Methods
	/// - Adding KeyDown Event to Control
	/// </summary>
	internal class Program :
		System.Windows.Application,
		IDisposable
	{
		private IRenderHost _renderHost;
		private Scene _scene;
		private ShapeDrawer _shapeDrawer;
		private RenderShapes shapes; 
		#region // ctor
		public Program()
		{
			Startup += (sender, args) => Ctor();
			Exit += (sender, args) => Dispose();
		}
		private void Ctor()
		{
			var readOnlyList = CreateWindows.SeedWindows();
			if (readOnlyList.Count > 0)
			{
				_renderHost = readOnlyList[0];
				var control = Control.FromHandle(_renderHost.HostHandler);
				if (control != null)
				{
					_shapeDrawer = new ShapeDrawer(control);
					shapes = new RenderShapes(control);
					_scene = new Scene(control);
					control.KeyDown += OnKeyDown;
				}
			}
		}
		public void Dispose()
		{
		}
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			_scene.HandleInput(e.KeyCode);
		}

		#endregion

	}
}
