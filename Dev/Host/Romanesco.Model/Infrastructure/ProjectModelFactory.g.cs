﻿// <autogenerated />
#nullable enable
using System;
using System.Collections.Generic;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.History;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.Serialize;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.Commands;
using Romanesco.Model.States;

namespace Romanesco.Model.Infrastructure
{
	internal partial class ProjectModelFactory : IProjectModelFactory
		, IDisposable
	{
		private readonly IProjectContext _iProjectContext;

		public IModelFactory Model { get; }
		public IModelRequirementFactory Requirement { get; }
		public IPluginFactory Plugin { get; }

		private WindowsSaveService? _ResolveSaveServiceCache;

		public ProjectModelFactory(IProjectContext iProjectContext, IModelFactory model, IModelRequirementFactory requirement, IPluginFactory plugin)
		{
			_iProjectContext = iProjectContext;
			Model = model;
			Requirement = requirement;
			Plugin = plugin;
		}

		public NewEditorState ResolveNewEditorStateAsTransient()
		{
			return new NewEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveSaveService());
		}

		public CleanEditorState ResolveCleanEditorStateAsTransient()
		{
			return new CleanEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveSaveService(), _iProjectContext);
		}

		public DirtyEditorState ResolveDirtyEditorStateAsTransient()
		{
			return new DirtyEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveSaveService(), _iProjectContext);
		}

		public IProjectSaveService ResolveSaveService()
		{
			return _ResolveSaveServiceCache ??= new WindowsSaveService(Model.ResolveStateSerializer(), _iProjectContext);
		}

		public Editor ResolveEditor()
		{
			return Model.ResolveEditor();
		}

		public IEditorStateChanger ResolveEditorStateChanger()
		{
			return Model.ResolveEditorStateChanger();
		}

		public IEditorState ResolveEditorState()
		{
			return Model.ResolveEditorState();
		}

		public EmptyEditorState ResolveEmptyEditorStateAsTransient()
		{
			return Model.ResolveEmptyEditorStateAsTransient();
		}

		public IProjectLoadService ResolveProjectLoadService()
		{
			return Model.ResolveProjectLoadService();
		}

		public IProjectHistoryService ResolveProjectHistoryService()
		{
			return Model.ResolveProjectHistoryService();
		}

		public ProjectSaveServiceFactory ResolveProjectSaveServiceFactory()
		{
			return Model.ResolveProjectSaveServiceFactory();
		}

		public IProjectModelFactory ResolveProjectModelFactoryAsTransient(IProjectContext projectContext)
		{
			return Model.ResolveProjectModelFactoryAsTransient(projectContext);
		}

		public CommandContext ResolveCommandContext()
		{
			return Model.ResolveCommandContext();
		}

		public IProjectSwitcher ResolveProjectSwitcher()
		{
			return Model.ResolveProjectSwitcher();
		}

		public IStorageCloneService ResolveStorageCloneService()
		{
			return Model.ResolveStorageCloneService();
		}

		public IEditorFacade ResolveEditorFacade()
		{
			return ResolveEditor();
		}

		public IStateSerializer ResolveStateSerializer()
		{
			return Model.ResolveStateSerializer();
		}

		public IStateDeserializer ResolveStateDeserializer()
		{
			return Model.ResolveStateDeserializer();
		}

		public ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient()
		{
			return Model.ResolveProjectSettingsEditorAsTransient();
		}

		public ObjectInterpreter ResolveObjectInterpreter()
		{
			return Model.ResolveObjectInterpreter();
		}

		public IObjectInterpreter ResolveIObjectInterpreter()
		{
			return Model.ResolveIObjectInterpreter();
		}

		public void Dispose()
		{
		}
	}
}