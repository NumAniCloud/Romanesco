﻿using System;
using System.Linq;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Editor;

namespace Romanesco.ViewModel.Project
{
	internal class VmProjectSettingsProvider : IProjectSettingProvider
    {
		private readonly IServiceLocator serviceLocator;

		public VmProjectSettingsProvider(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

        public ProjectSettings? InputCreateSettings(ProjectSettingsEditor editor)
        {
            serviceLocator.GetService<IEditorViewModel>()
                .ShowProjectSetting(editor);

            if (editor.Succeeded)
            {
                if (editor.ProjectType is null || editor.ExporterType is null)
                {
                    throw new InvalidOperationException("プロジェクト設定、またはプラグイン構成が無効です。");
                }
                else
                {
                    return new ProjectSettings(
                        editor.Assembly,
                        editor.ProjectType,
                        editor.ExporterType,
                        editor.DependencyProjects.ToArray());
                }
            }
            else
            {
                return null;
            }
        }
    }
}
