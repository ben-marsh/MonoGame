// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MonoGame.Content.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MonoGame.Tools.Pipeline
{
    public static class Program
    {
        class ImportOptions
        {
            [CommandLineParameter(
                Name = "import",
                ValueName = "path",
                Description = "XNA content project to import")]
            public List<string> ImportProjects { get; set; } = new List<string>();
        }

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
            ImportOptions options = new ImportOptions();

            MGBuildParser parser = new MGBuildParser(options);
            if(!parser.Parse(args))
            {
                return 1;
            }

            // Handle the contentproj import
            foreach(string importPath in options.ImportProjects)
            {
                string outputPath = Path.GetFullPath(importPath.Remove(importPath.LastIndexOf('.')) + ".mgcb");

                FileInfo importFile = new FileInfo(importPath);
                FileInfo outputFile = new FileInfo(outputPath);

                if(!importFile.Exists || !outputFile.Exists || importFile.LastWriteTimeUtc > outputFile.LastWriteTimeUtc)
                {
                    PipelineProject project = new PipelineProject();

                    PipelineProjectParser projectParser = new PipelineProjectParser(new Observer(), project);
                    projectParser.ImportProject(importPath);

                    project.OriginalPath = outputPath;

                    PipelineTypes.Load(project);
                    foreach (var i in project.ContentItems)
                    {
                        i.ResolveTypes();
                    }

                    projectParser.SaveProject();

                    Console.WriteLine("{0} -> {1}", importPath, outputPath);
                }
            }

            return 0;
        }
    }
}
