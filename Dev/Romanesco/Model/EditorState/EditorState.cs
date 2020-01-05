﻿using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    abstract class EditorState
    {
        protected EditorContext Context { get; }

        public abstract string Title { get; }

        protected EditorState(EditorContext context)
        {
            this.Context = context;
        }

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
