﻿using Romanesco.Model.Services;

namespace Romanesco.Model.EditorComponents
{
    class EditorContext
    {
        public Editor Editor { get; }
        public ObjectInterpreter Interpreter { get; set; }
        public IProjectSettingProvider SettingProvider { get; }

        public EditorContext(Editor editor, ObjectInterpreter interpreter, IProjectSettingProvider settingProvider)
        {
            Editor = editor;
            Interpreter = interpreter;
            SettingProvider = settingProvider;
        }
    }
}
