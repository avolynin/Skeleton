using System;
using System.IO;
using Mallenom.SkeletonLib;

namespace test
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = @"D:\Users\Camputer\source\repos\MemoryTest\image.jpg";
			var imgdata = System.IO.File.ReadAllBytes(path);

			var s = new Skeleton(imgdata);
			var b = s.GetImageWithBonesAsync();
		}
	}
}
