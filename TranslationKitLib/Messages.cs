﻿using System;
using System.Globalization;

namespace TranslationKitLib
{
    public class Messages
    {
        public static string PlaceTime()
        {
            DateTime time = DateTime.Now;
            string result = time.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + " - ";
            return result;
        }
    }
}
