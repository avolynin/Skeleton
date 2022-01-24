using System;
using System.Drawing;
using System.IO;
using Mallenom.SkeletonLib;

namespace test
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = @"D:\Users\Camputer\source\repos\Skeleton\src\dataset\test\steth.jpg";
			var imgdata = System.IO.File.ReadAllBytes(path);

			var s = new Skeleton(imgdata);
			var b = s.GetImageWithBones();

			var c = (Image)new ImageConverter().ConvertFrom(b);
			c.Save(@"D:\Users\Camputer\source\repos\Skeleton\src\img\stethWithBones.jpg");
		}
	}
}
