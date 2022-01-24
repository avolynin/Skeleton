using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SkeletonApp.ViewModels.Base
{
	/// <summary>Базовый класс модели-представления.</summary>
	internal abstract class ViewModel : INotifyPropertyChanged
	{
		/// <summary>Отслеживание изменений свойств объектов.</summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Вызывает событие при изменении свойств объекта.</summary>
		/// <param name="propertyName">Имя свойства (если не указывать имя, возьмется имя вызывающего объекта.</param>
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>Присваивание полю переданного занчения value.</summary>
		/// <typeparam name="T">Any.</typeparam>
		/// <param name="field">Ссылка на поле свойства.</param>
		/// <param name="value">Новое значение.</param>
		/// <param name="propertyName">Имя свойства (если не указывать имя, возьмется имя вызывающего объекта.</param>
		/// <returns>true, если значение успшено присвоено полю. false, когда поле равно значению.</returns>
		protected virtual bool Set<T>(ref T field, T value,[CallerMemberName] string propertyName = null)
		{
			// Избежание рекурсивных присваиваний
			if(Equals(field, value)) return false;

			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
