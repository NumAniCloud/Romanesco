﻿using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal interface IEditorState
	{
		string Title { get; }

		IProjectLoadService GetLoadService();
		IProjectSaveService GetSaveService();
		IProjectHistoryService GetHistoryService();

		void OnCreate(IProjectContext project);
		void OnOpen(IProjectContext project);
		void OnSave();
		void OnSaveAs();
		void OnExport();
		void OnUndo();
		void OnRedo();
		void OnEdit();

		// このへんはCommandAvailabilityの責務かも？
		// あるいはEditor側がCommandAvailabilityを使うようにするとよいか？
		// EditorとEditorStateの両方に、CommandAvailabilityを注入するという手がある
		// その場合EditorViewModelにもCommandAvailabilityを注入すれば実装可能そう
		void Undo(CommandAvailability availability);
		void Redo(CommandAvailability availability);
		void UpdateHistoryAvailability(CommandAvailability availability);
		void UpdateCanExecute(CommandAvailability availability);
	}
}
