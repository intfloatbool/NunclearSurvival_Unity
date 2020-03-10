/// <summary>
/// See https://developers.google.com/maps/documentation/utilities/polylinealgorithm
/// </summary>
/// 

//This class is an edited version of the script found here: https://gist.github.com/shinyzhu/4617989
//Thanks shinyzhu

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using GoShared;

namespace GoDirections {

	public static class GOPolylineConverter
	{
	    /// <summary>
	    /// Decode google style polyline coordinates.
	    /// </summary>
	    /// <param name="encodedPoints"></param>
	    /// <returns></returns>
		public static IEnumerable<Coordinates> Decode(string encodedPoints)
	    {
			if (string.IsNullOrEmpty (encodedPoints)) {
	//			throw new ArgumentNullException("encodedPoints");
			}

	        char[] polylineChars = encodedPoints.ToCharArray();
	        int index = 0;

	        int currentLat = 0;
	        int currentLng = 0;
	        int next5bits;
	        int sum;
	        int shifter;

	        while (index < polylineChars.Length)
	        {
	            // calculate next latitude
	            sum = 0;
	            shifter = 0;
	            do
	            {
	                next5bits = (int)polylineChars[index++] - 63;
	                sum |= (next5bits & 31) << shifter;
	                shifter += 5;
	            } while (next5bits >= 32 && index < polylineChars.Length);

	            if (index >= polylineChars.Length)
	                break;

	            currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

	            //calculate next longitude
	            sum = 0;
	            shifter = 0;
	            do
	            {
	                next5bits = (int)polylineChars[index++] - 63;
	                sum |= (next5bits & 31) << shifter;
	                shifter += 5;
	            } while (next5bits >= 32 && index < polylineChars.Length);

	            if (index >= polylineChars.Length && next5bits >= 32)
	                break;

	            currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

				Coordinates coords = new Coordinates (Convert.ToDouble (currentLat) / 1E5, Convert.ToDouble (currentLng) / 1E5, 0);
				yield return coords;
	//
	//			yield return new Coordinates
	//            {
	//                Latitude = Convert.ToDouble(currentLat) / 1E5,
	//                Longitude = Convert.ToDouble(currentLng) / 1E5
	//            };


	        }
	    }

	    /// <summary>
	    /// Encode it
	    /// </summary>
	    /// <param name="points"></param>
	    /// <returns></returns>
		public static string Encode(IEnumerable<Coordinates> points)
	    {
	        var str = new StringBuilder();

	        var encodeDiff = (Action<int>)(diff =>
	        {
	            int shifted = diff << 1;
	            if (diff < 0)
	                shifted = ~shifted;

	            int rem = shifted;

	            while (rem >= 0x20)
	            {
	                str.Append((char)((0x20 | (rem & 0x1f)) + 63));

	                rem >>= 5;
	            }

	            str.Append((char)(rem + 63));
	        });

	        int lastLat = 0;
	        int lastLng = 0;

	        foreach (var point in points)
	        {
				int lat = (int)Math.Round(point.latitude * 1E5);
				int lng = (int)Math.Round(point.longitude * 1E5);

	            encodeDiff(lat - lastLat);
	            encodeDiff(lng - lastLng);

	            lastLat = lat;
	            lastLng = lng;
	        }

	        return str.ToString();
	    }
	}

}