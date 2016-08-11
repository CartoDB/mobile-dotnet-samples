﻿using System;
using System.IO;

namespace CartoMobileSample
{
	public class Utils
	{
		public static string GetDocumentDirectory(string withFolder = null)
		{
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			if (withFolder == null)
			{
				return documents;
			}

			return Path.Combine(documents, withFolder + "/");
		}

	}
}

