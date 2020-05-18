﻿using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
    abstract class EditorState
    {
        public abstract string Title { get; }

        public abstract IProjectLoadService GetLoadService();
        public abstract IProjectSaveService GetSaveService();
        public abstract IProjectHistoryService GetHistoryService();

        public virtual void OnCreate()
        {
        }

        public virtual void OnOpen()
        {
        }

        public virtual void OnSave()
        {
        }

        public virtual void OnSaveAs()
        {
        }

        public virtual void OnExport()
        {
        }

        public virtual void OnUndo()
        {
        }

        public virtual void OnRedo()
        {
        }

        public virtual void OnEdit()
        {
        }
    }
}
