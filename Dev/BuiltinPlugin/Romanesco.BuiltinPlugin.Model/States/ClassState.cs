﻿using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class ClassState : IFieldState
	{
		private readonly Subject<Exception> onErrorSubject = new Subject<Exception>();

		public ReactiveProperty<string> Title { get; }
		public IReadOnlyReactiveProperty<string> FormattedString { get; }
		public Type Type => Storage.Type;
		public ValueStorage Storage { get; }
		public IFieldState[] Fields { get; }
		public IObservable<Exception> OnError => onErrorSubject;
		public IObservable<Unit> OnEdited { get; }
		public List<IDisposable> Disposables { get; } = new List<IDisposable>();

		public ClassState(ValueStorage storage, IFieldState[] fields)
		{
			Title = new ReactiveProperty<string>(storage.MemberName);
			Storage = storage;
			Fields = fields;

			OnEdited = fields.Select(x => x.OnEdited).Merge();

			FormattedString = OnEdited.Select(_ =>
			{
				try
				{
					return Storage.GetValue()?.ToString() ?? "";
				}
				catch (Exception ex)
				{
					onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
					return "";
				}
			}).ToReadOnlyReactiveProperty(storage.GetValue()?.ToString() ?? "");
		}

		public void Dispose()
		{
			Title.Dispose();
			FormattedString.Dispose();
			foreach (var state in Fields)
			{
				state.Dispose();
			}
			foreach (var disposable in Disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
