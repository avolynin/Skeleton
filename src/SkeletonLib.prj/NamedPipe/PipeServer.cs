using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Mallenom.SkeletonLib.NamedPipe
{
	public static class PipeServer
	{
		public static void Run()
		{
			using var pipeServer = new NamedPipeServerStream
				("testpipe",
				PipeDirection.InOut,
				-1,
				PipeTransmissionMode.Byte);
			Console.WriteLine("NamedPipeServerStream object created.");

			// Wait for a client to connect
			pipeServer.WaitForConnection();

			//Client connected
			try
			{
				// Read user input and send that to the client process.
				using var sw = new StreamWriter(pipeServer);
				sw.AutoFlush = true;
				Console.Write("Enter text: ");
				sw.WriteLine(Console.ReadLine());
			}
			// Catch the IOException that is raised if the pipe is broken
			// or disconnected.
			catch(IOException e)
			{
				Console.WriteLine("ERROR: {0}", e.Message);
			}
		}
	}
}
