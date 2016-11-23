using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace core.phoneDevice
{
    public class ExtCommand {
        public static string Shell(string fileName, string arguments) {
            var psi = new ProcessStartInfo(fileName, arguments);
            psi.RedirectStandardOutput = true;

            using (var process = Process.Start(psi)) {
                return process.StandardOutput.ReadToEnd();
            }
        }

        public static string Shell(string script) {

            var index = script.IndexOf(" ");
            var fileName = script.Substring(0, index);
            var arguments = script.Substring(index + 1, script.Length - index-1);
            return Shell(fileName, arguments);
        }



    }
}
