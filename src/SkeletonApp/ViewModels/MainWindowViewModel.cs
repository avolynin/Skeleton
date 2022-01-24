using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using SkeletonApp.ViewModels.Base;

namespace SkeletonApp.ViewModels
{
	internal class MainWindowViewModel : ViewModel
	{
		private string _title = "SkeletonApp";
		private Image _inImage;
		private Image _outImage;

		/// <summary>Заголовок окна.</summary>
		public string Title
		{
			get { return _title; }
			set { Set<string>(ref _title, value); }
		}
		/// <summary>Входящее изображение.</summary>
		public Image InImage
		{
			get { return _inImage; }
			set { Set<Image>(ref _inImage, value); }
		}
		/// <summary>Исходящее изображение.</summary>
		public Image OutImage
		{
			get { return _outImage; }
			set { Set<Image>(ref _outImage, value); }
		}


	}
}
