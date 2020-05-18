﻿using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Sample
{
    class SampleProjectSettingProvider : IProjectSettingProvider
    {
        public ProjectSettings? InputCreateSettings(ProjectSettingsEditor editor)
        {
            editor.AssemblyPath.Value = typeof(Project).Assembly.Location;
            editor.ProjectTypeFullName.Value = typeof(Project).FullName ?? throw new Exception();

            if (editor.ProjectType is null || editor.ExporterType is null)
            {
                return null;
            }

            return new ProjectSettings(
                editor.Assembly,
                editor.ProjectType,
                editor.ExporterType,
                editor.DependencyProjects.ToArray());
        }
    }
}
