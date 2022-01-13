using System;
using Mallenom.SkeletonLib.NamedPipe;

namespace Mallenom.SkeletonLib
{
	public class Skeleton
	{
		public void SendImage(byte[] image)
		{
			using var server = new PipeServer("testpipe");
			server.Run();
			server.Send(image);
		}
	}
}
