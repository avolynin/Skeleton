using System;
using Mallenom.SkeletonLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkeletonApp.Infrastructure.Commands;
using SkeletonApp.ViewModels.Base;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

namespace SkeletonApp.ViewModels
{
	internal class MainWindowViewModel : ViewModel
	{
		#region Fields and Properties

		private string _title = "SkeletonApp";
		private Image _inImage = new Image();
		private Image _outImage = new Image();

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

		public string FileName { get; private set; }

		#endregion

		#region Commands

		#region CloseApplicationCommand

		public ICommand ConvertImageCommand { get; }

		private async void OnConvertImageCommandExecuted(object param)
		{
			var imgdata = getJPGFromImageControl(InImage.Source as BitmapImage);

			var skeleton = new Skeleton(imgdata);
			//var bytesImg = skeleton.GetImageWithBones();
			var bytesImg = await skeleton.GetImageWithBonesAsync();

			OutImage.Source = LoadImage(bytesImg);
		}
		private bool CanConvertImageCommandExecute(object param) => true;

		#endregion

		#region CloseApplicationCommand

		public ICommand OpenExplorerCommand { get; }

		private void OnOpenExplorerCommandExecuted(object param)
		{
			var fileDialog = new Microsoft.Win32.OpenFileDialog();

			fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

			Nullable<bool> result = fileDialog.ShowDialog();
			if(result.HasValue && result.Value)
			{
				FileName = fileDialog.FileName;
				Uri fileUri = new Uri(FileName);
				InImage.Source = new BitmapImage(fileUri);
			}
		}
		private bool CanOpenExplorerCommandExecute(object param) => true;

		#endregion

		#endregion

		public MainWindowViewModel()
		{
			Uri fileUri = new Uri(@"D:\Users\Camputer\source\repos\Skeleton\src\img\default-img.jpg");
			OutImage.Source = new BitmapImage(fileUri);
			InImage.Source = new BitmapImage(fileUri);
			ConvertImageCommand = new RelayCommand(OnConvertImageCommandExecuted, CanConvertImageCommandExecute);
			OpenExplorerCommand = new RelayCommand(OnOpenExplorerCommandExecuted, CanOpenExplorerCommandExecute);
		}

		public byte[] getJPGFromImageControl(BitmapImage imageC)
		{
			MemoryStream memStream = new MemoryStream();
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(imageC));
			encoder.Save(memStream);
			return memStream.ToArray();
		}

		// BitmapImage --> byte[]
		public static byte[] BitmapImageToByteArray(BitmapImage bmp)
		{
			byte[] bytearray = null;
			try
			{
				Stream smarket = bmp.StreamSource;
				if(smarket != null && smarket.Length > 0)
				{
					smarket.Position = 0;
					using(BinaryReader br = new BinaryReader(smarket))
					{
						bytearray = br.ReadBytes((int)smarket.Length);
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
			return bytearray;
		}

		public byte[] BufferFromImage(BitmapImage imageSource)
		{
			Stream stream = imageSource.StreamSource;
			byte[] buffer = null;

			if(stream != null && stream.Length > 0)
			{
				using(BinaryReader br = new BinaryReader(stream))
				{
					buffer = br.ReadBytes((Int32)stream.Length);
				}
			}

			return buffer;
		}

		private static BitmapImage LoadImage(byte[] imageData)
		{
			if(imageData == null || imageData.Length == 0) return null;
			var image = new BitmapImage();
			using(var mem = new MemoryStream(imageData))
			{
				mem.Position = 0;
				image.BeginInit();
				image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.UriSource = null;
				image.StreamSource = mem;
				image.EndInit();
			}
			image.Freeze();
			return image;
		}
	}
}
