﻿using System;
using System.IO;
using System.Linq;
using Mp3TagLib;
using Mp3TagLib.Operations;

namespace mp3tager.Operations
{
    class Analysis:Operation
    {
        public const int ID = 5;

        public Analysis()
        {
            OperationId = ID;
        }
      
        public override void Call()
        {
            Menu.PrintHelp();

            var path = Menu.GetUserInput("path:");          //@"C:\Users\Alexandr\Desktop\TEST";
            var analyzer = new Analyzer(new Tager(new FileLoader()));
            var mask = new Mask(Menu.GetUserInput("mask:"));
          
            var plan=analyzer.BuildPlan(Directory.GetFiles(path), mask);


            if (!analyzer.NotSynchronizedFiles.Any()&&!analyzer.ErrorFiles.Keys.Any())
            {
                Menu.PrintMessage("All files is OK");
                Menu.GetUserInput("Press enter...");
                return;
            }


            if (analyzer.ErrorFiles.Keys.Any())
            Menu.PrintCollection(
                       string.Format("{0} files can't be synchronized", analyzer.ErrorFiles.Count()),
                       analyzer.ErrorFiles,ConsoleColor.Red);



            Menu.PrintCollection("Not synchronized files:", (from file in analyzer.NotSynchronizedFiles select file.Key.Name + ".mp3 with "+file.Value), ConsoleColor.Red);
            Menu.PrintCollection("Synchronized files:", (from file in analyzer.SynchronizedFiles select file.Name+".mp3"), ConsoleColor.Green);



            Menu.PrintPlan(plan);
            if (Menu.GetYesNoAnswer("Save this plan?\nY/N:"))
            {
                PlanProvider provider=new PlanProvider();
                provider.Save(plan,Menu.GetUserInput("path:"));
            }
        }

        public override void Cancel()
        {
            //nothing to cancel
        }
    }
}
