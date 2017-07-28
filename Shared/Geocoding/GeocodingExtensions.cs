
using System;
using Carto.Geocoding;

namespace Shared
{
    public static class GeocodingExtensions
    {
        public static string GetPrettyAddress(this GeocodingResult result)
        {
            var parsed = "";
            var address = result.Address;

            if (address.Name.IsNotEmpty())
            {
                parsed += address.Name;
            }

            if (address.Street.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Street;
            }

            if (address.HouseNumber.IsNotEmpty())
            {
                parsed += " " + address.HouseNumber;
            }

            if (address.Neighbourhood.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Neighbourhood;
            }

            if (address.Locality.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Locality;
            }

            if (address.County.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.County;
            }

            if (address.Region.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Region;
            }

            if (address.Country.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Country;
            }

            return parsed;
        }

        public static string AddCommaIfNecessary(this string original)
        {
            if (original.Length > 0)
            {
                return ", ";
            }

            return "";
        }

        public static bool IsNotEmpty(this string original)
        {
            return !string.IsNullOrWhiteSpace(original);
        }
    }
}
