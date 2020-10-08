using System;

namespace Celeste.GameScan
{
    public static class BytesSizeExtension
    {
        public const int Kb = 1024;

        public const int Mb = Kb * Kb;

        public const int Gb = Kb * Mb;

        public static string FormatToBytesSizeThreeNonZeroDigits(double value)
        {
            string[] suffixes =
            {
                "bytes", "KB", "MB", "GB",
                "TB", "PB", "EB", "ZB", "YB"
            };

            for (var i = 0; i < suffixes.Length; i++)
                if (value <= Math.Pow(Kb, i + 1))
                    return ThreeNonZeroDigits(value / Math.Pow(Kb, i)) + " " + suffixes[i];

            return ThreeNonZeroDigits(value / Math.Pow(Kb, suffixes.Length - 1)) + " " +
                   suffixes[suffixes.Length - 1];
        }

        private static string ThreeNonZeroDigits(double value)
        {
            return value >= 100 ? value.ToString("0,0") : value.ToString(value >= 10 ? "0.0" : "0.00");
        }
    }
}
