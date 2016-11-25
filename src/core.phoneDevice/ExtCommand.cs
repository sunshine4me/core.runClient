using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace core.phoneDevice
{
    public class ExtCommand {
        public static async Task<string> Shell(string fileName, string arguments) {

            return await Task.Run(() => {
                try {
                    var psi = new ProcessStartInfo(fileName, arguments);
                    psi.RedirectStandardOutput = true;

                    using (var process = Process.Start(psi)) {
                        return process.StandardOutput.ReadToEnd();
                    }
                } catch (Exception e) {

                    return e.StackTrace; ;
                }
                
            });
        }

        public static async Task<string> Shell(string script) {

            var index = script.IndexOf(" ");
            var fileName = script.Substring(0, index);
            var arguments = script.Substring(index + 1, script.Length - index-1);
            return await Shell(fileName, arguments);
        }



    }
}
