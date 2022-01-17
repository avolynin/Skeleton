using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mallenom.SkeletonLib
{
	class PythonRunner
	{
		public Process _pyProcces;
		public string WorkingDirectory { get; set; }

		public PythonRunner(string workingDirectory)
		{
			WorkingDirectory = workingDirectory;
		}

		public void Run(string scriptName, string args = null)
		{
			var startInfo = new ProcessStartInfo("python");
			_pyProcces = new Process();

			startInfo.WorkingDirectory = WorkingDirectory;
			startInfo.Arguments = $"{scriptName} {args}";
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;

			_pyProcces.StartInfo = startInfo;
			_pyProcces.Start();
		}

		public void Kill()
		{
			_pyProcces.Kill();
		}
	}
}
