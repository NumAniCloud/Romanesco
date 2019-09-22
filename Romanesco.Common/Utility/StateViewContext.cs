﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.Common.Utility
{
    public class StateViewContext
    {
        public UserControl InlineControl { get; }
        public UserControl BlockControl { get; }
        public IStateViewModel ViewModel { get; set; }
        public IObservable<Exception> OnError { get; set; }

        public StateViewContext(UserControl inlineControl, UserControl blockControl, IStateViewModel viewModel)
        {
            InlineControl = inlineControl;
            BlockControl = blockControl;
            ViewModel = viewModel;
            OnError = ViewModel.OnError;
        }
    }
}
