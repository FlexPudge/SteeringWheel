using Microsoft.Win32;
using SteeringWheel.FormatCommands;
using System;
using System.Collections.Generic;
using System.IO;

namespace SteeringWheel.Service
{
    public static class SavePositionInFile
    {
        static List<MainCommand> commands = new List<MainCommand>();

        public static void AddPositionFiles(double x, double y, double z)
        {
            if (x != 0)
            {
                commands.Add(new SetXCommand((int)x));
            }
            if (y != 0)
            {
                commands.Add(new SetYCommand((int)y));
            }
            if (z != 0)
            {
                commands.Add(new SetZCommand((int)z));
            }
            commands.Add(new StartRapidMove(300));
        }
        public static void SaveFiles()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == true)
            {
                List<string> write = new List<string>();
                foreach (var comm in commands)
                {
                    write.Add(comm.Argument.ToString());
                }
                File.WriteAllText(saveFileDialog.FileName, String.Join("; \n", write));

            }
        }
    }
}
