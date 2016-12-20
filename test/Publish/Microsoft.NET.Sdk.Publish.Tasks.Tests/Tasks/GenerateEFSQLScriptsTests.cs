﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;

namespace Microsoft.NET.Sdk.Publish.Tasks.Tests.Tasks
{
    public class GenerateEFSQLScriptsTests
    {
        private static readonly ITaskItem DefaultContext = new TaskItem("DefaultContext", new Dictionary<string, string>() { { "Value", @"Server=(localdb)\mssqllocaldb; Database=defaultDB;Trusted_Connection=True;MultipleActiveResultSets=true" } });
        private static readonly ITaskItem CarContext = new TaskItem("CarContext", new Dictionary<string, string>() { { "Value", @"Server=(localdb)\mssqllocaldb; Database=CarDB;Trusted_Connection=True;MultipleActiveResultSets=true" } });
        private static readonly ITaskItem PersonContext = new TaskItem("PersonContext", new Dictionary<string, string>() { { "Value", @"Server=(localdb)\mssqllocaldb; Database=PersonDb;Trusted_Connection=True;MultipleActiveResultSets=true" } });

        private static readonly List<object[]> testData = new List<object[]>
        {
            new object[] {new ITaskItem[] { DefaultContext } },
            new object[] {new ITaskItem[] { DefaultContext, CarContext, PersonContext } }
        };

        public static IEnumerable<object[]> EFMigrations
        {
            get { return testData; }
        }


        [Theory]
        [MemberData("EFMigrations")]
        public void GenerateEFScripts_ReturnsFalse_forInValidContexts(ITaskItem[] efMigrationsData)
        {
            //Arrange
            string projectFolder = Path.Combine(Path.GetTempPath(), "ProjectFolder");
            string publishDir = Path.Combine(Path.GetTempPath(), "PublishDirectory");
            if (!Directory.Exists(publishDir))
            {
                Directory.CreateDirectory(publishDir);
            }

            // Act
            GenerateEFSQLScripts task = new GenerateEFSQLScripts()
            {
                ProjectDirectory = projectFolder,
                EFPublishDirectory = publishDir,
                EFMigrations = efMigrationsData
            };

            bool isSucces = task.GenerateEFSQLScriptsInternal(false);

            // Assert
            Assert.False(isSucces);
        }
    }
}
