using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mp3lib
{
	public class Mp3FileAnalyser
	{
		private readonly string _mask;
		private readonly IEnumerable<IMp3File> _files;

		public Mp3FileAnalyser(IEnumerable<IMp3File> files, string mask)
		{
			_mask = mask;
			_files = files;
		}

		//TODO: tests {READY}
		//TODO: return detailed information {READY}
		public IEnumerable<FileDifferences> GetDifferences()
		{
			var list = new List<FileDifferences>();
			foreach (var mp3 in _files)
			{
				var mp3Data = mp3.GetId3Data();

				var extracter = new DataExtracter(_mask);
				var tags = extracter.GetTags();
				var prefixes = extracter.FindAllPrefixes(tags);
				var diffs = new FileDifferences(mp3);
				var data = extracter.GetFullDataFromString(prefixes, Path.GetFileNameWithoutExtension(mp3.FilePath), tags);

				var differenceItems = data.Where(item => mp3Data[item.Key] != item.Value).ToArray();

				if (!differenceItems.Any()) continue;

				foreach (var item in differenceItems)
				{
					diffs.Add(item.Key, new Diff { FileNameValue = item.Value, TagValue = mp3Data[item.Key] });
				}

				list.Add(diffs);
			}

			return list.ToArray();
		}

		public void ShowDifferences(IEnumerable<FileDifferences> differences, IRequestable requestable)
		{
			foreach (var file in differences)
			{
				requestable.SendMessage(string.Format("This file has differences: {0}", file.Mp3File));
				foreach (var diff in file.Diffs)
				{
					requestable.SendMessage(string.Format("{0} in Mp3File Name: {1}", diff.Key, diff.Value.FileNameValue));
					requestable.SendMessage(string.Format("{0} in Tags: {1}", diff.Key, diff.Value.TagValue));
				}
			}
		}
	}
}