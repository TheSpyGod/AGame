using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGConsole.Engine.Render
{
    public class RenderShapes : 
        IDisposable
	{
        private ShapeDrawer drawer;
		private List<(PointF[] Vertices, Brush FillBrush)> shapes;
		public RenderShapes(Control drawingControl)
        {
            drawer = new ShapeDrawer(drawingControl);
			shapes = new List<(PointF[] Vertices, Brush FillBrush)>();
			AssignShapes();
        }
		private void AssignShapes()
        {
            foreach (var (verticies, brush) in shapes)
            {
                drawer.AddShape(verticies, brush);
            }
        }
        public void ClearShapes()
        {
            drawer.ClearShapes();
        }
	    public void Dispose()
		{
            drawer?.Dispose();
		}

    }
}
