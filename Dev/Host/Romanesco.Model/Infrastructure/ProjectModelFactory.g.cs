﻿// <autogenerated />
#nullable enable
using System;
using System.Collections.Generic;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Save;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.History;
using Romanesco.Model.Commands;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.Serialize;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Infrastructure
{
	internal partial class ProjectModelFactory : IProjectModelFactory
		, IDisposable
	{
		private readonly IProjectContext _iProjectContext;

		public IModelFactory Model { get; }
		public IModelRequirementFactory Requirement { get; }
		public IPluginFactory Plugin { get; }

		private WindowsSaveService? _ResolveProjectSaveServiceCache;

		public ProjectModelFactory(IProjectContext iProjectContext, IModelFactory model, IModelRequirementFactory requirement, IPluginFactory plugin)
		{
			_iProjectContext = iProjectContext;
			Model = model;
			Requirement = requirement;
			Plugin = plugin;
		}

		public NewEditorState ResolveNewEditorStateAsTransient()
		{
			return new NewEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveProjectSaveService(), this, Model.ResolveEditorSession());
		}

		public CleanEditorState ResolveCleanEditorStateAsTransient()
		{
			return new CleanEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveProjectSaveService(), _iProjectContext, this, Model.ResolveEditorSession());
		}

		public DirtyEditorState ResolveDirtyEditorStateAsTransient()
		{
			return new DirtyEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveProjectSaveService(), _iProjectContext, this, Model.ResolveEditorSession());
		}

		public IProjectSaveService ResolveProjectSaveService()
		{
			return _ResolveProjectSaveServiceCache ??= new WindowsSaveService(Model.ResolveStateSerializer(), _iProjectContext);
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

		public CommandAvailability ResolveCommandAvailability()
		{
			return Model.ResolveCommandAvailability();
		}

		public EditorSession ResolveEditorSession()
		{
			return Model.ResolveEditorSession();
		}

		public IProjectModelFactory ResolveProjectModelFactory(IProjectContext projectContext)
		{
			return Model.ResolveProjectModelFactory(_iProjectContext);
		}

		public IEditorFacade ResolveEditorFacade()
		{
			return Model.ResolveEditorFacade();
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