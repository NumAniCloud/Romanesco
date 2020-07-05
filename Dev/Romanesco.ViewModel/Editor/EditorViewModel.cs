﻿using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Romanesco.Common.Model;
using Romanesco.Model.EditorComponents;
using Romanesco.ViewModel.States;
using Livet.Messaging;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.ViewModel.Editor;

namespace Romanesco.ViewModel
{
	internal class EditorViewModel : Livet.ViewModel, IEditorViewModel
    {
		private readonly ViewModelInterpreter interpreter;

		public IEditorFacade Editor { get; set; }
        public ReactiveProperty<IStateViewModel[]> Roots { get; } = new ReactiveProperty<IStateViewModel[]>();

        public BooleanUsingScopeSource CommandExecution { get; }

        public ReactiveCommand CreateCommand { get; set; }
        public ReactiveCommand OpenCommand { get; }
        public ReactiveCommand SaveCommand { get; set; }
        public ReactiveCommand SaveAsCommand { get; set; }
        public ReactiveCommand ExportCommand { get; set; }
        public ReactiveCommand Undo { get; }
        public ReactiveCommand Redo { get; }
        public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();
        public List<IDisposable> Disposables => Editor.Disposables;

        public EditorViewModel(IEditorFacade editor, ViewModelInterpreter interpreter)
        {
            ReactiveCommand ToEditorCommand(EditorCommandType type)
            {
                var canExecute = editor.CanExecuteObservable
                    .Where(x => x.command == type)
                    .Select(x => x.canExecute)
                    .Concat(CommandExecution.IsUsing.Select(x => !x));
                return new ReactiveCommand(canExecute);
            }

            Editor = editor;
			this.interpreter = interpreter;
			CommandExecution = new BooleanUsingScopeSource();

            CreateCommand = ToEditorCommand(EditorCommandType.Create);
            OpenCommand = ToEditorCommand(EditorCommandType.Open);
            SaveCommand = ToEditorCommand(EditorCommandType.Save);
            SaveAsCommand = ToEditorCommand(EditorCommandType.SaveAs);
            ExportCommand = ToEditorCommand(EditorCommandType.Export);
            Undo = ToEditorCommand(EditorCommandType.Undo);
            Redo = ToEditorCommand(EditorCommandType.Redo);

            CreateCommand.SubscribeSafe(x => CreateAsync().Forget()).AddTo(editor.Disposables);
            OpenCommand.SubscribeSafe(x => OpenAsync().Forget()).AddTo(editor.Disposables);
            ExportCommand.SubscribeSafe(x => ExportAsync().Forget()).AddTo(editor.Disposables);
            SaveCommand.SubscribeSafe(x => SaveAsync().Forget()).AddTo(editor.Disposables);
            SaveAsCommand.SubscribeSafe(x => SaveAsAsync().Wait()).AddTo(editor.Disposables);
            Undo.SubscribeSafe(x => Editor.Undo()).AddTo(editor.Disposables);
            Redo.SubscribeSafe(x => Editor.Redo()).AddTo(editor.Disposables);

            GcDebugCommand.SubscribeSafe(x => GC.Collect()).AddTo(editor.Disposables);

            //Messenger.Raise(new TransitionMessage())
        }

        public void ShowProjectSetting(ProjectSettingsEditor editor)
        {
            var vm = new ProjectSettingsEditorViewModel(editor);
            Messenger.Raise(new TransitionMessage(vm, "CreateProject"));
        }

        private async Task CreateAsync()
        {
            using (CommandExecution.Create())
            {
                var projectContext = await Editor.CreateAsync();
                if (projectContext != null)
                {
                    Roots.Value = projectContext.Project.Root.States
                        .Select(s => interpreter.InterpretAsViewModel(s))
                        .ToArray();
                }
            }
        }

        private async Task OpenAsync()
        {
            using (CommandExecution.Create())
            {
                var projectContext = await Editor.OpenAsync();
                if (projectContext == null)
                {
                    return;
                }

                Roots.Value = projectContext.Project.Root.States
                    .Select(s => interpreter.InterpretAsViewModel(s))
                    .ToArray();
            }
        }

        private async Task ExportAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.ExportAsync();
            }
        }

        private async Task SaveAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.SaveAsync();
            }
        }

        private async Task SaveAsAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.SaveAsAsync();
            }
        }
    }
}
