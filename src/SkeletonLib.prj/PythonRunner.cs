﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mallenom.SkeletonLib
{
	/// <summary>Класс для запуска python-скриптов.</summary>
	class PythonRunner : IDisposable
	{
		private Process _procces;

		/// <summary>Директория с python-скриптами.</summary>
		public string WorkingDirectory { get; set; }

		/// <summary>Создает процесс.</summary>
		/// <param name="workingDirectory">Путь рабочей директории.</param>
		public PythonRunner(string workingDirectory)
		{
			_procces = new Process();
			WorkingDirectory = workingDirectory;
		}

		/// <summary>Запуск python-скрипта.</summary>
		/// <param name="scriptName">Имя файла с раширением py.</param>
		/// <param name="args">Передаваемые аргументы при запуске скрипта.</param>
		public void Run(string scriptName, string args = null)
		{
			var startInfo = new ProcessStartInfo("python");

			startInfo.WorkingDirectory = WorkingDirectory;
			startInfo.Arguments = $"{scriptName} {args}";
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;

			_procces.StartInfo = startInfo;
			_procces.Start();
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
