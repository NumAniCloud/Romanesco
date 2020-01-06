﻿using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
    class EmptyEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;

        public override string Title => "Romanesco - プロジェクトなし";

        public EmptyEditorState(EditorContext context) : base(context)
        {
            var deserializer = new Services.Serialize.NewtonsoftStateDeserializer();
            loadService = new WindowsLoadService(context, deserializer);
        }

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => new NullSaveService();

        public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
    }
}
