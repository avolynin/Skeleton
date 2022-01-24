using System;
using System.Diagnostics;

namespace Mallenom.SkeletonLib
{
	/// <summary>Класс для запуска python-скриптов.</summary>
	class PythonRunner : IDisposable
	{
		private Process _procces;

		/// <summary>Директория с python-скриптами.</summary>
		public string WorkingDirectory { get; set; }
		public string VirtualEnvironmentName { get; set; }

		/// <summary>Создает процесс.</summary>
		/// <param name="workingDirectory">Путь рабочей директории.</param>
		public PythonRunner(string workingDirectory, string venvName = null)
		{
			_procces = new Process();
			WorkingDirectory = workingDirectory;
			VirtualEnvironmentName = venvName;
		}

		/// <summary>Запуск python-скрипта.</summary>
		/// <param name="scriptName">Имя файла с раширением py.</param>
		/// <param name="args">Передаваемые аргументы при запуске скрипта.</param>
		public void RunPy(string scriptName, string args = null)
		{
			var startInfo = new ProcessStartInfo("python");

			startInfo.WorkingDirectory = WorkingDirectory;
			startInfo.Arguments = $"{scriptName} {args}";
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			//startInfo.RedirectStandardError = true;
			//startInfo.RedirectStandardOutput = true;

			_procces.StartInfo = startInfo;
			_procces.Start();
		}

		/// <summary>Запуск python-скрипта c помощью Anaconda3 и виртуальным пространством.</summary>
		/// <param name="scriptName">Имя файла с раширением py.</param>
		/// <param name="args">Передаваемые аргументы при запуске скрипта.</param>
		public void RunPyConda(string scriptName, string args = null)
		{
			var startInfo = new ProcessStartInfo("cmd.exe");

			startInfo.WorkingDirectory = WorkingDirectory;
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			//startInfo.RedirectStandardError = true;
			//startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardInput = true;

			_procces.StartInfo = startInfo;
			_procces.Start();

			using var sw = _procces.StandardInput;
			if (sw.BaseStream.CanWrite)
			{
				// Активация Anaconda3
				sw.WriteLine(@"D:\Users\Camputer\anaconda3\Scripts\activate.bat");
				// Активация виртуального окружения
				sw.WriteLine("activate skeleton-env");

				if(args?.Length > 0) sw.WriteLine(args);
				// Запуск скрипта
				sw.WriteLine("python " + scriptName);
			}
		}

		/// <summary>Ожидание выполнения python-скрипта.</summary>
		public void WaitForExit()
		{
			_procces.WaitForExit();
		}

		#region [IDisplosable pattern]

		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				if(disposing)
				{
					// TODO: dispose managed state (managed objects)
				}
				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_procces = null;
				disposedValue = true;
			}
		}

		~PythonRunner()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
