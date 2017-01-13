using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundPhoto.Core
{
    public class Constants
    {
        public static int HeaderLength = 4;
        public static int MagicPatternLength = 10;
        public static char[] HeaderId = { 'B', 'H', 'A', 'U' };
        public static char[] MagicPattern = { 'B', 'H', 'A', 'U', 'R', 'O', 'C', 'K', 'S', 'S' };
    }
}
