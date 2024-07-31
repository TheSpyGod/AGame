using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGConsole.Engine.Render
{
	public interface IRenderHost :
		IDisposable
	{
		IntPtr HostHandler { get; }
	}
}
