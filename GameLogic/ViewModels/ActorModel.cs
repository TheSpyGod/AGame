using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace Adventure
{
	class ActorModel
	{
		public struct Actor
		{
			public string Name { get; set; }
			public uint Health { get; set; }
			public Vector2 Location { get; set; }
		}
	}
}
