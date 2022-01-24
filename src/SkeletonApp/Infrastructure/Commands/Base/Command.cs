using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkeletonApp.Infrastructure.Commands.Base
{
	/// <summary>Базовый класс команды.</summary>
	internal abstract class Command : ICommand
	{
		/// <summary>
		/// Выполняется во время изменения результата CanExecute.
		/// Передает методы в CommandManager.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		/// <summary>Проверка на возможность выполнения команды.</summary>
		/// <param name="parameter">Данные используемые командой.</param>
		/// <returns>
		/// true, если команда может выполниться.
		/// false, если команда не может выполниться.
		/// </returns>
		public abstract bool CanExecute(object parameter);

		/// <summary>Выполнение команды.</summary>
		/// <param name="parameter">Данные используемые командой.</param>
		public abstract void Execute(object parameter);
	}
}
