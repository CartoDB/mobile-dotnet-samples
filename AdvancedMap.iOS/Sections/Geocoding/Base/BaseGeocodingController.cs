﻿
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
    public class BaseGeocodingController : PackageDownloadBaseController
    {
        public Geocoding Geocoding { get { return Client as Geocoding; } }

		protected const string ApiKey = Sources.MapzenApiKey;

		public BaseGeocodingController()
        {
            string baseFolder = Utils.GetDocumentDirectory();
            string folder = GetPackageFolder(Geocoding.PackageFolder);
            Client = new Geocoding(baseFolder);
            Geocoding.ApiKey = ApiKey;
        }

	}
}
