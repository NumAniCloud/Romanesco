using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;

namespace Romanesco.BuiltinPlugin.Model.States
{
    public class IntIdChoiceState : IFieldState
    {
        private readonly Subject<Exception> onErrorSubject = new Subject<Exception>();
        public List<IDisposable> Disposables { get; } = new List<IDisposable>();

        public ReactiveProperty<string> Title { get; }
        public IReadOnlyReactiveProperty<string> FormattedString { get; }
        public Type Type => typeof(int);
        public ValueStorage Storage { get; }
        public IObservable<Exception> OnError => onErrorSubject;
        public ReactiveProperty<MasterList?> Master { get; } = new ReactiveProperty<MasterList?>();
        public ReactiveProperty<IFieldState> SelectedItem { get; } = new ReactiveProperty<IFieldState>();
        public IObservable<Unit> OnEdited => Storage.OnValueChanged.Select(x => Unit.Default);

        public IntIdChoiceState(ValueStorage storage, string masterName, MasterListContext context)
        {
            Title = new ReactiveProperty<string>(storage.MemberName);
            Storage = storage;

            // IDisposableをハンドリングするべき。リスト内にこれがある場合、要素削除のときにリークするおそれがある
            context.OnKeyAdded.Where(key => masterName == key)
                .Subscribe(key => UpdateChoices(key, context))
                .AddTo(Disposables);

            UpdateChoices(masterName, context);

            FormattedString = SelectedItem.Select(item =>
            {
                try
                {
                    return item?.ToString() ?? "<null>";
                }
                catch (Exception ex)
                {
                    onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
                    return "";
                }
            }).ToReactiveProperty(Storage.GetValue()?.ToString() ?? "");

            // 値の変化を生データへ反映
            SelectedItem.Subscribe(x =>
            {
                var id = Master.Value == null ? -1
                    : x?.Storage.GetValue() is { } value ? Master.Value.GetId(value)
                    : -1;
                Storage.SetValue(id);
            }).AddTo(Disposables);
        }

        // 編集中は呼ばれないが、ロード中はこのStateより後にマスターが読み込まれる場合があるので遅延できるように
        private void UpdateChoices(string masterName, MasterListContext context)
        {
            if (context.Masters.TryGetValue(masterName, out var list) && list.IdType == typeof(int))
            {
                Master.Value = list;
                // 初期値をロード
                SelectedItem.Value = list.GetById(Storage.GetValue() ?? -1);
            }
            else
            {
                Master.Value = null;
            }
        }

        public void Dispose()
        {
	        Title.Dispose();
            FormattedString.Dispose();
            Master.Dispose();
            SelectedItem.Dispose();
            foreach (var disposable in Disposables)
            {
	            disposable.Dispose();
            }
        }
    }
}