// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MonoGame.Content.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MonoGame.Tools.Pipeline
{
    public static class Program
    {
        class Observer : IContentItemObserver
        {
            public void OnItemModified(ContentItem item)
            { }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  mgcb-importer <InputFile> <OutputFile>");
                Console.WriteLine();
                Console.WriteLine("Converts XNA content project to MGCB format.");
                return 0;
            }

            FileInfo importFile = new FileInfo(Path.GetFullPath(args[0]));
            if(!importFile.Exists)
            {
                Console.Error.WriteLine($"Input file '{importFile.FullName}' does not exist");
                return 1;
            }

            FileInfo toolFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
            DateTime inputTime = (toolFile.LastWriteTimeUtc > importFile.LastWriteTimeUtc)? toolFile.LastWriteTimeUtc : importFile.LastWriteTimeUtc;

            FileInfo outputFile = new FileInfo(Path.GetFullPath(args[1]));
            if(!outputFile.Exists || inputTime > outputFile.LastWriteTimeUtc)
            {
                PipelineProject project = new PipelineProject();

                PipelineProjectParser projectParser = new PipelineProjectParser(new Observer(), project);
                projectParser.ImportProject(importFile.FullName);

                project.OriginalPath = outputFile.FullName;

                PipelineTypes.Load(project);
                foreach (var i in project.ContentItems)
                {
                    i.ResolveTypes();
                }

                projectParser.SaveProject();

                Console.WriteLine("{0} -> {1}", importFile.Name, outputFile.FullName);
            }

            return 0;
        }
    }
}
