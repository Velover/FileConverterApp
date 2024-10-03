using FileConverterCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverterApp.Models
{
	internal class ConvertionDataModel
	{
		public string FilePath { get; set; }
		public string ToFormat { get; set; }
		public EFileType FileType { get; set; }

	}
}
