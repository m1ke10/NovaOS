using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.HAL;
using System.Drawing;
using Cosmos.System.Graphics;
namespace NovaOS
{
    public class Kernel : Sys.Kernel
    {
        public Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        public string currentdirectory = @"0:\";
        public string choice;
        Canvas canvas;
        protected override void BeforeRun()
        {
            Console.WriteLine("Setting up filesystem(s)...");
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.Beep();
            Console.WriteLine("Nova booted successfully! :D");
            Console.Clear();
            Console.WriteLine("NovaOS Boot Menu");
            Console.WriteLine();
            Console.WriteLine("1. Run NovaOS CLI");
            Console.WriteLine("2. [VACANT]");
            choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Clear();

                Console.WriteLine("Welcome to NovaOS!");
                Console.WriteLine(@"
******************************************************
*                 Welcome to NovaOS                 *
******************************************************

NovaOS v0.0.1 Alpha
                    
Cosmos 2022/11/21 

For support, visit: www.novaos.com/support

Shell: NSH (nova shell)
                    ");
                Console.Beep();
                Console.WriteLine("iso automatic login (novashell/nsh)");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("This is a placeholder boot option. More is to come soon. ;)");
            }
        }

        protected override void Run()
        {
            if (choice == "1")
            {
                if (currentdirectory == @"0:\" || currentdirectory == @"0:")
                {
                    Console.Write("root@novaISO ~ $ ");
                }
                else
                {
                    Console.Write("root@novaISO " + currentdirectory + " $ ");
                }
                var input = Console.ReadLine();
                Shell(input);
            }
            else
            {

            }

        }
        public void ZapDisk()
        {
            try
            {
                Directory.Delete(@"0:\");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void Shell(string cmdlol)
        {
            string[] cmd = cmdlol.Split('*');
            if (cmd[0] == "clear")
            {
                Console.Clear();
            }
            else if (cmd[0] == "exit")
            {
                Sys.Power.Shutdown();
            }
            else if (cmd[0] == "reboot")
            {
                Sys.Power.Reboot();
            }
            else if (cmd[0] == "freedisk")
            {
                var available_space = fs.GetAvailableFreeSpace(@"0:\");
                Console.WriteLine("Available Free Space: " + available_space);
            }
            else if (cmd[0] == "fst")
            {
                var fs_type = fs.GetFileSystemType(@"0:\");
                Console.WriteLine("File System Type: " + fs_type);
            }
            else if (cmd[0] == "ls")
            {
                var files_list = Directory.GetFiles(currentdirectory);
                var directory_list = Directory.GetDirectories(currentdirectory);

                foreach (var file in files_list)
                {
                    Console.WriteLine(file);
                }
                foreach (var directory in directory_list)
                {
                    Console.WriteLine(directory);
                }
            }
            else if (cmd[0] == "mkfile")
            {
                try
                {
                    var file_stream = File.Create(currentdirectory + cmd[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else if (cmd[0] == "mkdir")
            {
                try
                {
                    Directory.CreateDirectory(currentdirectory + cmd[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else if (cmd[0] == "zapdisk")
            {
                Console.WriteLine("Your disk will be WIPED AND SET UP WITH THE OPERATING SYSTEM DIRECTORY!!!! Are you sure you want to continue (y/n) ?");
                var input = Console.ReadLine();
                if (input == "y")
                {
                    ZapDisk();
                }
                else
                {
                    Console.WriteLine("Operation aborted.");
                }
            }
            else if (cmd[0] == "neofetch")
            {
                Console.WriteLine("Nova OS 0.2 | 1080p |  Andromeda");
            }
            else if (cmd[0] == "cd")
            {
                string targetDirectory = cmd[1];

                if (targetDirectory == "..")
                {
                    // Move up one directory level (no additional path validation needed)
                    int lastSlashIndex = currentdirectory.LastIndexOf(@"\") + 1;
                    if (lastSlashIndex > 0)
                    {
                        currentdirectory = currentdirectory.Substring(0, lastSlashIndex - 1);
                        Console.WriteLine("Directory changed to: " + currentdirectory);
                    }
                    else
                    {
                        Console.WriteLine("Already at the root directory.");
                    }
                }
                else
                {
                    if (cmd.Length != 1)
                    {
                        // Handle other directory changes
                        if (!currentdirectory.EndsWith(@"\"))
                        {
                            currentdirectory += @"\"; // Append trailing slash if needed
                        }

                        if (Directory.Exists(currentdirectory + targetDirectory))
                        {
                            currentdirectory += targetDirectory;
                            Console.WriteLine("Directory changed to: " + currentdirectory);
                        }
                        else
                        {
                            Console.WriteLine("Invalid directory: " + targetDirectory);
                        }
                    }
                    else
                    {
                        currentdirectory = @"0:\";
                    }
                }
                Console.WriteLine("Current Directory is: " + currentdirectory);
            }

            // Print the correct current directory
            else if (cmd[0] == "cat")
            {
                string filename = cmd[1]; // Get the filename from the command

                try
                {
                    string content = File.ReadAllText(currentdirectory + filename);
                    Console.WriteLine(content);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading file: " + e.Message);
                }
            }
            else if (cmd[0] == "help")
            {
                Console.WriteLine("Commands are: clear, exit, reboot, freedisk, fst, ls, mkfile, mkdir, zapdisk, neofetch, cd, cat");
            }
            else if (cmd[0] == "currentshell")
            {
                Console.WriteLine("Nova Shell version 1.0");
            }
        }
    }
}
