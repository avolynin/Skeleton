using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mallenom.SkeletonLib.NamedPipe;

namespace Mallenom.SkeletonLib
{
	public class Skeleton
	{
		private PipeServer _server;
		private PythonRunner _pyRunner;
		private byte[] _byteImage;

		private byte[] ByteImage
		{
			get { return _byteImage; }
			set 
			{
				if(!IsValidImage(value)) 
					throw new ArgumentException("Значение не соответсвует формату изображения");
				_byteImage = value; 
			}
		}

		public Skeleton(byte[] image)
		{
			ByteImage = image;
		}

		public byte[] GetImageWithBones()
		{
			_server = new PipeServer("testpipe");

			_server.PipeMessage +=
				(message) =>
				{
					Console.WriteLine(message);
				};

			_pyRunner = new PythonRunner(@"D:\Users\Camputer\source\repos\Skeleton\src\python\");
			_pyRunner.Run("pipe.py");

			_server.SendAsync(ByteImage);
			_server.ListenAsync();

			_pyRunner._pyProcces.WaitForExit();
			_pyRunner.Kill();
			return null;
		}

		public byte[] GetImageWithBonesAsync()
		{
			_server = new PipeServer("testpipe");

			_server.PipeMessage +=
				(message) =>
				{
					Console.WriteLine(message);
				};

			_pyRunner = new PythonRunner(@"D:\Users\Camputer\source\repos\Skeleton\src\python\");
			_pyRunner.Run("pipe.py");

			_server.SendAsync(ByteImage);
			_server.ListenAsync();

			_pyRunner._pyProcces.WaitForExit();
			_pyRunner.Kill();
			_server.Kill();
			return null;
		}

		private static bool IsValidImage(byte[] bytes)
		{
			try
			{
				using(MemoryStream ms = new MemoryStream(bytes))
					Image.FromStream(ms);
			}
			catch(ArgumentException)
			{
				return false;
			}
			return true;
		}
	}
}
