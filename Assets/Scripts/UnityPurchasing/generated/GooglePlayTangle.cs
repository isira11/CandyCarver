// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("XRGn4rae3sdqcYLm4jQQxzGuS/EaqCsIGicsIwCsYqzdJysrKy8qKagrJSoaqCsgKKgrKyqpxicaINH/GEHkdIc8hpH4z7lo1GzdrBSTLxb4S8Q7aRxtRxxpqwIPcKR/4CyA8ghNu92M6pAldkNI86r/BPSjpckSLwao1iXRkTweYGzSEZlcbmZ68sR0aEOGEGrGS9/FOVkunVLPqKH2eYouv99e9grpS2Lk5hDq7yY0SRS8RLESNfkGrdvvGkWp8LUyyBU2EkX8Exzq9uuxAZ8rWEBj93a63A6FTCdSBIkRQlX+Sx+d5rRNqEX8q3V7+xNPEWOfizHWpv0RVoyUE0ivjdnc0nAeiQ949HBOm3SmxtK+zlmTZifuRQGMXrbCsygpKyor");
        private static int[] order = new int[] { 5,5,5,13,8,11,11,11,8,13,13,12,13,13,14 };
        private static int key = 42;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
