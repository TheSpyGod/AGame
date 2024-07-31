using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AGConsole.Engine.Render
{
	/// <summary>
	/// Provides functionality to draw shapes on a Control using polygons.
	/// This class supports:
	/// - Drawing shapes with specified fill and border styles using the Paint event.
	/// - Adding new shapes to be drawn on the control.	
	/// - Clearing all shapes from the drawing.
	/// - Repainting with the Paint Event.
	/// </summary>

	public class ShapeDrawer : IDisposable
    {
        private Control _control;
		private List<(PointF[] Verticies, Brush FillBrush)> _shapes;
        public ShapeDrawer(Control control)
        {
            _control = control;
            _control.Paint += OnPaint;
			_shapes = new List<(PointF[] Vertices, Brush FillBrush)>();
		}
		public void AddShape(PointF[] verticies, Brush fillBrush)
		{
			_shapes.Add((verticies, fillBrush));
			_control.Invalidate();
		}
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }
		private void Draw(Graphics graphics)
		{
			foreach (var (vertices, brush) in _shapes)
			{
				graphics.FillPolygon(brush, vertices);

				using (Pen pen = new Pen(Color.Black, 2))
				{
					graphics.DrawPolygon(pen, vertices);
				}
			}
		}
		public void ClearShapes()
		{
			_shapes.Clear();
			_control.Invalidate();
		}
		public void Dispose()
		{
			if (_control != null)
			{
				_control.Paint -= OnPaint;
				_control = null;
			}
		}
	}
}
