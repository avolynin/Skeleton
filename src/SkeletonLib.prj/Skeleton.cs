using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mallenom.SkeletonLib.NamedPipe;

namespace Mallenom.SkeletonLib
{
	/// <summary>Класс для обнаружения на изображении человека костей.</summary>
	public class Skeleton
	{
		#region Fields

		/// <summary>Изображение в виде массива байтов.</summary>
		private byte[] _byteImage;

		/// <summary>Изображение в виде массива байтов.</summary>
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

		#endregion

		/// <param name="image">Исходное изображение.</param>
		public Skeleton(byte[] image)
		{
			ByteImage = image;
		}

		/// <summary>Добавляет кости на изображение.</summary>
		/// <returns>Исходное изображение с наложенными костями в виде массива байтов.</returns>
		public byte[] GetImageWithBones()
		{
			using var pyRun = new PythonRunner(@"D:\Users\Camputer\source\repos\Skeleton\src\python\")
			{
				VirtualEnvironmentName = "skeleton-env"
			};
			using var server = new PipeServer("testpipe");

			pyRun.RunPyConda("main.py");
			
			server.Send(ByteImage);
			var result = server.Listen();

			pyRun.WaitForExit();
			return result;
		}

		public async Task<byte[]> GetImageWithBonesAsync()
		{
			return await Task.Run<byte[]>(() =>
			{
				using var pyRun = new PythonRunner(@"D:\Users\Camputer\source\repos\Skeleton\src\python\")
				{
					VirtualEnvironmentName = "skeleton-env"
				};
				using var server = new PipeServer("testpipe");

				pyRun.RunPyConda("main.py");

				server.Send(ByteImage);
				var result = server.Listen();

				pyRun.WaitForExit();
				return result;
			});
		}

		/// <summary>Проверка массива байтов на соответствие формату изображения.</summary>
		/// <param name="bytes">Массив байтов для проверки.</param>
		/// <returns>true, если массив байтов представляет изображение.</returns>
		private static bool IsValidImage(in byte[] bytes)
		{
			try
			{
				using var ms = new MemoryStream(bytes);
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
