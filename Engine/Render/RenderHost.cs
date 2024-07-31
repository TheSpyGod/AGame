using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGConsole.Engine.Render
{
	/// <summary>
	/// Class for getting a HostHandler of a window
	/// </summary>
	public class RenderHost :
		IRenderHost
	{
		#region // handle

		public IntPtr HostHandler { get; private set; }

        #endregion
        #region // ctor
        public RenderHost(IntPtr hostHandle)
        {
			HostHandler = hostHandle;
        }
        public void Dispose()
		{
			HostHandler = default;
		}
		#endregion
	}
}
