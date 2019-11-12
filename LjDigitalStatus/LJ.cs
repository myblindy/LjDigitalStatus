using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LjDigitalStatus
{
    internal static class LJ
    {
        [DllImport("ljackuw.dll", CharSet = CharSet.Ansi)]
        private static extern int DigitalIO(ref int idnum, int demo, ref int trisD, int trisIO, ref int stateD, ref int stateIO, int updateDigital, ref int outputD);

        public static ulong ReadDigitalInputs()
        {
            var ljID = 0;
            var outputd = 0;
            var trisd = 0;
            var stateio = 0;
            var stated = 0;

            var res = DigitalIO(ref ljID, 0, ref trisd, 0, ref stated, ref stateio, 0, ref outputd);
            if (res != 0)
                //throw new Exception("Error reading digital inputs");
                return 0;

            return (uint)stated | ((ulong)stateio << 16);
        }
    }
}
