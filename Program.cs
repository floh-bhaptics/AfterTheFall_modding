using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;

namespace AfterTheFallModding
{
    internal class Program
    {
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        static void Main(string[] args)
        {
            //Dependency checks
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "AfterTheFall.exe")) ||
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "GameAssembly.dll")))
            {
                Console.WriteLine("Game file(s) not found.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Process.GetCurrentProcess().Kill();
            }
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "BepInEx")))
            {
                Console.WriteLine("BepInEx not found.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Process.GetCurrentProcess().Kill();
            }

            //Deal with doorstop
            Console.WriteLine("Checking doorstop...");
            if(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "winhttp_alt.dll")))
            {
                Console.WriteLine("Reverting doorstop...");
                File.Move(Path.Combine(Directory.GetCurrentDirectory(), "winhttp_alt.dll"), Path.Combine(Directory.GetCurrentDirectory(), "winhttp.dll"));
            }

            //Launch zenith
            Console.WriteLine("Attempting to launch After The Fall...");
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "AfterTheFall.exe");
                    myProcess.Start();
                    System.Threading.Thread.Sleep(100);
                    Console.WriteLine("Updating doorstop...");
                    File.Move(Path.Combine(Directory.GetCurrentDirectory(), "winhttp.dll"), Path.Combine(Directory.GetCurrentDirectory(), "winhttp_alt.dll"));
                    System.Threading.Thread.Sleep(100);
                    IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

                    //ShowWindow(handle, 6); <---- Minimize window
                    myProcess.WaitForExit();
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
