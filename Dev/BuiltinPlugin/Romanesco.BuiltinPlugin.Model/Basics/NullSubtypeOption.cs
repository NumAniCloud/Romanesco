﻿using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.Basics
{
	public class NullSubtypeOption : ISubtypeOption
	{
		private readonly ValueStorage _storage;

		public NullSubtypeOption(ValueStorage storage)
		{
			_storage = storage;
		}

		public string OptionName => "<null>";

		public IFieldState MakeState() => new ClassState(_storage, Array.Empty<IFieldState>());

		public bool IsTypeOf(Type type) => false;
	}
}
