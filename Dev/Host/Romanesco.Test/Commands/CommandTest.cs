﻿using System;
using System.Linq.Expressions;
using Moq;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.Test.Commands
{
	public class CommandTest
	{
		[Theory]
		[InlineData(Create)]
		[InlineData(Open)]
		[InlineData(Save)]
		[InlineData(SaveAs)]
		[InlineData(Export)]
		[InlineData(Undo)]
		[InlineData(Redo)]
		public void コマンドの実行可能性が通知される(EditorCommandType type)
		{
			var editorState = MockHelper.GetEditorStateMock();
			var availability = new CommandAvailability(editorState.Object);

			ICommandModel command = type switch
			{
				Create => new CreateCommand(availability.CanCreate, editorState.Object),
				Open => new OpenCommand(availability.CanOpen, editorState.Object),
				Save => new SaveCommand(availability.CanSave, editorState.Object),
				SaveAs => new SaveAsCommand(availability.CanSaveAs, editorState.Object),
				Export => new ExportCommand(availability.CanExport, editorState.Object),
				Undo => new UndoCommand(availability.CanUndo, editorState.Object),
				Redo => new RedoCommand(availability.CanRedo, editorState.Object),
				_ => throw new NotImplementedException(),
			};

			Assert.False(command.CanExecute.Value);

			{
				using var once = command.CanExecute.ExpectAtLeastOnce();
				availability.UpdateCanExecute(type, true);
			}

			Assert.True(command.CanExecute.Value);

			{
				using var once = command.CanExecute.ExpectAtLeastOnce();
				availability.UpdateCanExecute(type, false);
			}

			Assert.False(command.CanExecute.Value);
		}

		[Fact]
		public void プロジェクトを作成するサービスを実行できる()
		{
			AssertCommandExecution(x => x.CreateAsync(),
				(p, s) =>
				{
					var command = new CreateCommand(p.CanCreate, s);
					_ = command.Execute().Result;
				});
		}

		[Fact]
		public void プロジェクトを開くサービスを実行できる()
		{
			AssertCommandExecution(x => x.OpenAsync(),
				(p, s) =>
				{
					var command = new OpenCommand(p.CanOpen, s);
					_ = command.Execute().Result;
				});
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			AssertCommandExecution(x => x.SaveAsync(),
				(p, s) =>
				{
					var command = new SaveCommand(p.CanSave, s);
					command.Execute().Wait();
				});
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			AssertCommandExecution(x => x.SaveAsAsync(),
				(p, s) =>
				{
					var command = new SaveAsCommand(p.CanSaveAs, s);
					command.Execute().Wait();
				});
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			AssertCommandExecution(x => x.ExportAsync(),
				(p, s) =>
				{
					var command = new ExportCommand(p.CanExport, s);
					command.Execute().Wait();
				});
		}

		[Fact]
		public void Undoを呼び出せる()
		{
			AssertCommandExecution(x => x.Undo(),
				(p, s) =>
				{
					var command = new UndoCommand(p.CanUndo, s);
					command.Execute();
				});
		}

		[Fact]
		public void Redoを呼び出せる()
		{
			AssertCommandExecution(x => x.Redo(),
				(p, s) =>
				{
					var command = new RedoCommand(p.CanRedo, s);
					command.Execute();
				});
		}

		[Fact]
		public void プロジェクト作成時にイベントが発行される()
		{
			AssertEvent((p, s) =>
			{
				var command = new CreateCommand(p.CanCreate, s);
				using var once = command.OnExecuted.ExpectAtLeastOnce();
				_ = command.Execute().Result;
			});
		}
		
		[Fact]
		public void プロジェクト作成時にステートのイベントを呼び出す()
		{
			AssertStateEvent(x => x.OnCreate(It.IsAny<IProjectContext>()),
				(p, s) =>
				{
					var command = new CreateCommand(p.CanCreate, s);
					_ = command.Execute().Result;
				});
		}

		[Fact]
		public void プロジェクトを開くときにイベントが発行される()
		{
			AssertEvent((p, s) =>
			{
				var command = new OpenCommand(p.CanOpen, s);
				using var once = command.OnExecuted.ExpectAtLeastOnce();
				_ = command.Execute().Result;
			});
		}
		
		[Fact]
		public void プロジェクトを開くときにステートのイベントを呼び出す()
		{
			var loadService = MockHelper.GetLoaderServiceMock(Mock.Of<IProjectContext>());
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			editorState.Setup(x => x.OnOpen(It.IsAny<IProjectContext>()))
				.Callback(() => { });

			var commandAvailability = new CommandAvailability(editorState.Object);
			var command = new OpenCommand(commandAvailability.CanOpen, editorState.Object);

			_ = command.Execute().Result;

			editorState.Verify(x => x.OnOpen(It.IsAny<IProjectContext>()), Times.Once);
		}

		class CommandTestSuite<TService> where TService : class
		{
			public Mock<TService> Service { get; }
			public Mock<IEditorState> EditorStateMock { get; }
			public CommandAvailability Commands { get; }

			public CommandTestSuite(Mock<TService> service)
			{
				var serviceObject = service.Object;
				Service = service;

				EditorStateMock = serviceObject switch
				{
					IProjectLoadService load => MockHelper.GetEditorStateMock(loadService: load),
					IProjectSaveService save => MockHelper.GetEditorStateMock(saveService: save),
					IProjectHistoryService history => MockHelper.GetEditorStateMock(historyService: history),
					_ => throw new NotImplementedException(),
				};

				Commands = new CommandAvailability(EditorStateMock.Object);
			}

			public void Run(Action<CommandAvailability, IEditorState> execution)
			{
				execution(Commands, EditorStateMock.Object);
			}

			public static CommandTestSuite<IProjectLoadService> CreateLoad()
			{
				return new CommandTestSuite<IProjectLoadService>(MockHelper.GetLoaderServiceMock(Mock.Of<IProjectContext>()));
			}
		}

		private static void AssertStateEvent(
			Expression<Action<IEditorState>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var suite = CommandTestSuite<IProjectLoadService>.CreateLoad();
			suite.EditorStateMock.Setup(methodToVerify)
				.Callback(() => { });

			suite.Run(execution);

			suite.EditorStateMock.Verify(methodToVerify, Times.Once);
		}

		private static void AssertEvent(
			Action<CommandAvailability, IEditorState> execution)
		{
			var suite = CommandTestSuite<IProjectLoadService>.CreateLoad();
			suite.Run(execution);
		}

		private static void AssertCommandExecution<TCommandResult>(
			Expression<Func<IProjectLoadService, TCommandResult>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);

			execution(commandAvailability, editorState.Object);
			
			loadService.Verify(methodToVerify, Times.Once);
		}
		
		private static void AssertCommandExecution<TCommandResult>(
			Expression<Func<IProjectSaveService, TCommandResult>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);

			execution(commandAvailability, editorState.Object);
			
			saveService.Verify(methodToVerify, Times.Once);
		}
		
		private static void AssertCommandExecution(
			Expression<Action<IProjectHistoryService>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var historyService = MockHelper.CreateHistoryMock();
			var editorState = MockHelper.GetEditorStateMock(historyService: historyService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);

			execution(commandAvailability, editorState.Object);
			
			historyService.Verify(methodToVerify, Times.Once);
		}
	}
}
