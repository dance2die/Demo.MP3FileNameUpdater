using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MP3FileNameUpdater
{
	public class Program
	{
		private const string INPUT_DIR = @"C:\Users\dance2die\Desktop\Music";
		private const string OUTPUT_DIR = @"C:\Users\dance2die\Desktop\Music Output";
		private const string ERROR_DIR = @"C:\Users\dance2die\Desktop\Music Error";


		public static void Main(string[] args)
		{
			var files = GetFiles();
			RenameFiles(files);
		}

		private static void RenameFiles(IEnumerable<string> files)
		{
			foreach (string file in files)
			{
				Console.WriteLine(file);

				var fileName = Path.GetFileName(file);
				string newFileName = RenameFile(file);

				if (string.IsNullOrWhiteSpace(newFileName))
				{
					MoveToErrorFolder(file);
					Console.WriteLine($"Moved \"{fileName}\" to Error Folder....");
				}
				//else
				//{
				//	MoveToOutputFolder(file, newFileName);
				//	Console.WriteLine($"Successfully renamed {fileName}");
				//}
			}
		}

		private static void MoveToErrorFolder(string file)
		{
			File.Copy(file, Path.Combine(ERROR_DIR, Path.GetFileName(file)), overwrite: true);
		}

		private static void MoveToOutputFolder(string file, string newFileName)
		{
			var newFile = Path.Combine(OUTPUT_DIR, newFileName) + ".mp3";
			File.Copy(file, newFile, overwrite: true);
		}

		private static string RenameFile(string file)
		{
			TagLib.File mp3 = TagLib.File.Create(file);
			var title = mp3.Tag.Title;
			//var album = mp3.Tag.Album;
			//var length = mp3.Properties.Duration.ToString();

			//Console.WriteLine($"Processing => Title: {title}");

			return CleanFileName(title);
		}

		private static IEnumerable<string> GetFiles()
		{
			return Directory.EnumerateFiles(INPUT_DIR, "*.mp3", SearchOption.AllDirectories);
		}

		/// <summary>
		/// https://stackoverflow.com/a/146162/4035
		/// </summary>
		private static string CleanFileName(string fileName)
		{
			if (fileName == null) return "";

			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(fileName, "");
		}
	}
}