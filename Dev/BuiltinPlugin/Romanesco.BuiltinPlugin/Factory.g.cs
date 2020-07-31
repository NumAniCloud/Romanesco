﻿// <autogenerated />
#nullable enable
using System;
using System.Collections.Generic;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.ViewModel.Factories;
using Romanesco.BuiltinPlugin.View.Factories;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Deptorygen.GenericHost;
using Microsoft.Extensions.DependencyInjection;

namespace Romanesco.BuiltinPlugin
{
	public partial class Factory : IFactory
		, IDisposable
		, IDeptorygenFactory
	{
		private readonly MasterListContext _masterListContext;

		public IApiFactory Host { get; }

		private DynamicStateFactory? _ResolveDynamicStateFactoryCache;
		private IdStateFactory? _ResolveIdStateFactoryCache;
		private ListStateFactory? _ResolveListStateFactoryCache;
		private PrimitiveStateFactory? _ResolvePrimitiveStateFactoryCache;
		private EnumStateFactory? _ResolveEnumStateFactoryCache;
		private SubtypingStateFactory? _ResolveSubtypingStateFactoryCache;
		private ClassStateFactory? _ResolveClassStateFactoryCache;
		private IdViewModelFactory? _ResolveIdViewModelFactoryCache;
		private PrimitiveViewModelFactory? _ResolvePrimitiveViewModelFactoryCache;
		private EnumViewModelFactory? _ResolveEnumViewModelFactoryCache;
		private SubtypingClassViewModelFactory? _ResolveSubtypingClassViewModelFactoryCache;
		private ClassViewModelFactory? _ResolveClassViewModelFactoryCache;
		private ListViewModelFactory? _ResolveListViewModelFactoryCache;
		private IdViewFactory? _ResolveIdViewFactoryCache;
		private PrimitiveViewFactory? _ResolvePrimitiveViewFactoryCache;
		private EnumViewFactory? _ResolveEnumViewFactoryCache;
		private ClassViewFactory? _ResolveClassViewFactoryCache;
		private ArrayViewFactory? _ResolveArrayViewFactoryCache;
		private SubtypingViewFactory? _ResolveSubtypingViewFactoryCache;

		public Factory(MasterListContext masterListContext, IApiFactory host)
		{
			_masterListContext = masterListContext;
			Host = host;
		}

		public DynamicStateFactory ResolveDynamicStateFactory()
		{
			return _ResolveDynamicStateFactoryCache ??= new DynamicStateFactory(ResolvePrimitiveStateFactory());
		}

		public IdStateFactory ResolveIdStateFactory()
		{
			return _ResolveIdStateFactoryCache ??= new IdStateFactory(_masterListContext);
		}

		public ListStateFactory ResolveListStateFactory()
		{
			return _ResolveListStateFactoryCache ??= new ListStateFactory(_masterListContext, Host.ResolveCommandHistory());
		}

		public PrimitiveStateFactory ResolvePrimitiveStateFactory()
		{
			return _ResolvePrimitiveStateFactoryCache ??= new PrimitiveStateFactory(Host.ResolveCommandHistory());
		}

		public EnumStateFactory ResolveEnumStateFactory()
		{
			return _ResolveEnumStateFactoryCache ??= new EnumStateFactory(Host.ResolveCommandHistory());
		}

		public SubtypingStateFactory ResolveSubtypingStateFactory()
		{
			return _ResolveSubtypingStateFactoryCache ??= new SubtypingStateFactory(Host.ResolveDataAssemblyRepository(), Host.ResolveObjectInterpreter());
		}

		public ClassStateFactory ResolveClassStateFactory()
		{
			return _ResolveClassStateFactoryCache ??= new ClassStateFactory(Host.ResolveDataAssemblyRepository());
		}

		public IdViewModelFactory ResolveIdViewModelFactory()
		{
			return _ResolveIdViewModelFactoryCache ??= new IdViewModelFactory();
		}

		public PrimitiveViewModelFactory ResolvePrimitiveViewModelFactory()
		{
			return _ResolvePrimitiveViewModelFactoryCache ??= new PrimitiveViewModelFactory();
		}

		public EnumViewModelFactory ResolveEnumViewModelFactory()
		{
			return _ResolveEnumViewModelFactoryCache ??= new EnumViewModelFactory();
		}

		public SubtypingClassViewModelFactory ResolveSubtypingClassViewModelFactory()
		{
			return _ResolveSubtypingClassViewModelFactoryCache ??= new SubtypingClassViewModelFactory();
		}

		public ClassViewModelFactory ResolveClassViewModelFactory()
		{
			return _ResolveClassViewModelFactoryCache ??= new ClassViewModelFactory();
		}

		public ListViewModelFactory ResolveListViewModelFactory()
		{
			return _ResolveListViewModelFactoryCache ??= new ListViewModelFactory();
		}

		public IdViewFactory ResolveIdViewFactory()
		{
			return _ResolveIdViewFactoryCache ??= new IdViewFactory();
		}

		public PrimitiveViewFactory ResolvePrimitiveViewFactory()
		{
			return _ResolvePrimitiveViewFactoryCache ??= new PrimitiveViewFactory();
		}

		public EnumViewFactory ResolveEnumViewFactory()
		{
			return _ResolveEnumViewFactoryCache ??= new EnumViewFactory();
		}

		public ClassViewFactory ResolveClassViewFactory()
		{
			return _ResolveClassViewFactoryCache ??= new ClassViewFactory();
		}

		public ArrayViewFactory ResolveArrayViewFactory()
		{
			return _ResolveArrayViewFactoryCache ??= new ArrayViewFactory();
		}

		public SubtypingViewFactory ResolveSubtypingViewFactory()
		{
			return _ResolveSubtypingViewFactoryCache ??= new SubtypingViewFactory();
		}

		public IEnumerable<IStateFactory> ResolveStateFactories()
		{
			return new IStateFactory[]
			{
				ResolveDynamicStateFactory(),
				ResolveIdStateFactory(),
				ResolveListStateFactory(),
				ResolvePrimitiveStateFactory(),
				ResolveEnumStateFactory(),
				ResolveSubtypingStateFactory(),
				ResolveClassStateFactory()
			};
		}
		public IEnumerable<IStateViewModelFactory> ResolveViewModelFactories()
		{
			return new IStateViewModelFactory[]
			{
				ResolveIdViewModelFactory(),
				ResolvePrimitiveViewModelFactory(),
				ResolveEnumViewModelFactory(),
				ResolveSubtypingClassViewModelFactory(),
				ResolveClassViewModelFactory(),
				ResolveListViewModelFactory()
			};
		}
		public IEnumerable<IViewFactory> ResolveViewFactories()
		{
			return new IViewFactory[]
			{
				ResolveIdViewFactory(),
				ResolvePrimitiveViewFactory(),
				ResolveEnumViewFactory(),
				ResolveArrayViewFactory(),
				ResolveSubtypingViewFactory()
			};
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<IFactory>(provider => this);
			services.AddTransient<DynamicStateFactory>(provider => ResolveDynamicStateFactory());
			services.AddTransient<IdStateFactory>(provider => ResolveIdStateFactory());
			services.AddTransient<ListStateFactory>(provider => ResolveListStateFactory());
			services.AddTransient<PrimitiveStateFactory>(provider => ResolvePrimitiveStateFactory());
			services.AddTransient<EnumStateFactory>(provider => ResolveEnumStateFactory());
			services.AddTransient<SubtypingStateFactory>(provider => ResolveSubtypingStateFactory());
			services.AddTransient<ClassStateFactory>(provider => ResolveClassStateFactory());
			services.AddTransient<IdViewModelFactory>(provider => ResolveIdViewModelFactory());
			services.AddTransient<PrimitiveViewModelFactory>(provider => ResolvePrimitiveViewModelFactory());
			services.AddTransient<EnumViewModelFactory>(provider => ResolveEnumViewModelFactory());
			services.AddTransient<SubtypingClassViewModelFactory>(provider => ResolveSubtypingClassViewModelFactory());
			services.AddTransient<ClassViewModelFactory>(provider => ResolveClassViewModelFactory());
			services.AddTransient<ListViewModelFactory>(provider => ResolveListViewModelFactory());
			services.AddTransient<IdViewFactory>(provider => ResolveIdViewFactory());
			services.AddTransient<PrimitiveViewFactory>(provider => ResolvePrimitiveViewFactory());
			services.AddTransient<EnumViewFactory>(provider => ResolveEnumViewFactory());
			services.AddTransient<ClassViewFactory>(provider => ResolveClassViewFactory());
			services.AddTransient<ArrayViewFactory>(provider => ResolveArrayViewFactory());
			services.AddTransient<SubtypingViewFactory>(provider => ResolveSubtypingViewFactory());
			services.AddTransient<IStateFactory>(provider => ResolveDynamicStateFactory());
			services.AddTransient<IStateFactory>(provider => ResolveIdStateFactory());
			services.AddTransient<IStateFactory>(provider => ResolveListStateFactory());
			services.AddTransient<IStateFactory>(provider => ResolvePrimitiveStateFactory());
			services.AddTransient<IStateFactory>(provider => ResolveEnumStateFactory());
			services.AddTransient<IStateFactory>(provider => ResolveSubtypingStateFactory());
			services.AddTransient<IStateFactory>(provider => ResolveClassStateFactory());
			services.AddTransient<IStateViewModelFactory>(provider => ResolveIdViewModelFactory());
			services.AddTransient<IStateViewModelFactory>(provider => ResolvePrimitiveViewModelFactory());
			services.AddTransient<IStateViewModelFactory>(provider => ResolveEnumViewModelFactory());
			services.AddTransient<IStateViewModelFactory>(provider => ResolveSubtypingClassViewModelFactory());
			services.AddTransient<IStateViewModelFactory>(provider => ResolveClassViewModelFactory());
			services.AddTransient<IStateViewModelFactory>(provider => ResolveListViewModelFactory());
			services.AddTransient<IViewFactory>(provider => ResolveIdViewFactory());
			services.AddTransient<IViewFactory>(provider => ResolvePrimitiveViewFactory());
			services.AddTransient<IViewFactory>(provider => ResolveEnumViewFactory());
			services.AddTransient<IViewFactory>(provider => ResolveArrayViewFactory());
			services.AddTransient<IViewFactory>(provider => ResolveSubtypingViewFactory());
		}

		public void Dispose()
		{
		}
	}
}