using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using AGConsole.Engine.Render;

namespace AGConsole.Client
{
    /// <summary>
    /// Class that creates a Windows Form window.
    /// Functionality:
    /// - Creates Form window and assigns it to a readOnly List
    /// - Buffered Panel to fix flickering when repainting 
    /// - Readds focus when mouse hovers over the window
    /// - Closes the Application when the Form window closes
    /// </summary>
	public static class CreateWindows
	{
        public static IReadOnlyList<IRenderHost> SeedWindows()
        {
            var size = new System.Drawing.Size(800, 600);
            var renderHosts = new[]
            {
               CreateWindowsForm(size, "WindowForm", h => new RenderHost(h)),
            };
            return renderHosts;
        }

        private static IRenderHost CreateWindowsForm(System.Drawing.Size size, string title, Func<IntPtr, IRenderHost> ctorRanderHost)
        {
            var window = new Form
            {
                Size = size,
                Text = title,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				MaximizeBox = false,
                MinimizeBox = false,
				StartPosition = FormStartPosition.CenterScreen
			};

            var hControl = new BufferedPanel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
            };
            window.Controls.Add(hControl);
            hControl.MouseEnter += (sender, args) =>
            {
                if (Form.ActiveForm != window) window.Activate();
                if (!hControl.Focused) hControl.Focus();
            };
            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();
            return ctorRanderHost(hControl.Handle);
        }
	}
}
