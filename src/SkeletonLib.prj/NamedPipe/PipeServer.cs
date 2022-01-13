using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Mallenom.SkeletonLib.NamedPipe
{
	public class PipeServer : IDisposable
	{
		private NamedPipeServerStream _pipeServer;
		private bool disposedValue;

		public string Name { get; set; }

		public PipeServer(string name)
		{
			Name = name;
		}

		public void Run()
		{
			_pipeServer = new NamedPipeServerStream
				(Name,
				PipeDirection.InOut,
				-1,
				PipeTransmissionMode.Byte);

			// Wait for a client to connect
			_pipeServer.WaitForConnection();
		}

		public void Send(byte[] bytes)
		{
			try
			{
				// Send that to the client process.
				using var bw = new BinaryWriter(_pipeServer);
				//bw.AutoFlush = true;
				bw.Write(bytes);
			}
			catch(IOException)
			{
				throw;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				if(disposing)
				{
					_pipeServer.Dispose();
				}
				disposedValue = true;
			}
		}

		~PipeServer()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
