using SkeletonApp.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonApp.Infrastructure.Commands
{
	internal class RelayCommand : Command
	{
		private readonly Func<object, bool> _canExecute;
		private readonly Action<object> _execute;

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			if(execute == null) throw new ArgumentNullException(nameof(execute));

			_execute = execute;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

		public override void Execute(object parameter) => _execute(parameter);
	}
}
