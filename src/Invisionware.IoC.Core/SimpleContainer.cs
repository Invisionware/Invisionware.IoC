// ***********************************************************************
// Assembly         : Invisionware.Ioc
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SimpleContainer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Invisionware.IoC
{
	/// <summary>
	/// Simple dependency container implementation
	/// </summary>
	public class SimpleContainer : IDependencyContainer
	{
		/// <summary>
		/// The _resolver
		/// </summary>
		private readonly IResolver _resolver;
		/// <summary>
		/// The _services
		/// </summary>
		private readonly Dictionary<Type, List<object>> _services;
		/// <summary>
		/// The _registered services
		/// </summary>
		private readonly Dictionary<Type, List<Func<IResolver, object>>> _registeredServices;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleContainer" /> class.
		/// </summary>
		public SimpleContainer()
		{
			_resolver = new Resolver(ResolveAll);
			_services = new Dictionary<Type, List<object>>();
			_registeredServices = new Dictionary<Type, List<Func<IResolver, object>>>();
		}

		#region IDependencyContainer Members
		/// <summary>
		/// Gets the resolver from the container
		/// </summary>
		/// <returns>An instance of <see cref="IResolver" /></returns>
		public IResolver GetResolver()
		{
			return _resolver;
		}

		/// <summary>
		/// Registers an instance of T to be stored in the container.
		/// </summary>
		/// <typeparam name="T">Type of instance</typeparam>
		/// <param name="instance">Instance of type T.</param>
		/// <returns>An instance of <see cref="SimpleContainer" /></returns>
		public IDependencyContainer Register<T>(T instance) where T : class
		{
			var type = typeof(T);

			if (!_services.TryGetValue(type, out var list))
			{
				list = new List<object>();
				_services.Add(type, list);
			}

			list.Add(instance);
			return this;
		}

		/// <summary>
		/// Registers a type to instantiate for type T.
		/// </summary>
		/// <typeparam name="T">Type of instance</typeparam>
		/// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
		/// <returns>An instance of <see cref="SimpleContainer" /></returns>
		public IDependencyContainer Register<T, TImpl>()
			where T : class
			where TImpl : class, T
		{
			return Register(t => Activator.CreateInstance<TImpl>() as T);
		}

		/// <summary>
		/// Registers a type to instantiate for type T as singleton.
		/// </summary>
		/// <typeparam name="T">Type of instance</typeparam>
		/// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
		/// <returns>An instance of <see cref="IDependencyContainer" /></returns>
		public IDependencyContainer RegisterSingle<T, TImpl>()
			where T : class
			where TImpl : class, T
		{
			var type = typeof(T);

			if (!_services.TryGetValue(type, out var list))
			{
				list = new List<object>();
				_services.Add(type, list);
			}

			var instance = Activator.CreateInstance<TImpl>();

			list.Add(instance);
			return this;
		}

		/// <summary>
		/// Tries to register a type
		/// </summary>
		/// <typeparam name="T">Type of instance</typeparam>
		/// <param name="type">Type of implementation</param>
		/// <returns>An instance of <see cref="SimpleContainer" /></returns>
		public IDependencyContainer Register<T>(Type type) where T : class
		{
			return Register(typeof(T), type);
		}

		/// <summary>
		/// Tries to register a type
		/// </summary>
		/// <param name="type">Type to register.</param>
		/// <param name="impl">Type that implements registered type.</param>
		/// <returns>An instance of <see cref="SimpleContainer" /></returns>
		public IDependencyContainer Register(Type type, Type impl)
		{
			List<Func<IResolver, object>> list;
			if (!_registeredServices.TryGetValue(type, out list))
			{
				list = new List<Func<IResolver, object>>();
				_registeredServices.Add(type, list);
			}

			list.Add(t => Activator.CreateInstance(impl));

			return this;
		}

		/// <summary>
		/// Registers a function which returns an instance of type T.
		/// </summary>
		/// <typeparam name="T">Type of instance.</typeparam>
		/// <param name="func">Function which returns an instance of T.</param>
		/// <returns>An instance of <see cref="SimpleContainer" /></returns>
		public IDependencyContainer Register<T>(Func<IResolver, T> func) where T : class
		{
			var type = typeof(T);
			List<Func<IResolver, object>> list;
			if (!_registeredServices.TryGetValue(type, out list))
			{
				list = new List<Func<IResolver, object>>();
				_registeredServices.Add(type, list);
			}

			list.Add(func);

			return this;
		}

		/// <summary>
		/// Resolves all.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>IEnumerable&lt;System.Object&gt;.</returns>
		private IEnumerable<object> ResolveAll(Type type)
		{
			List<object> list;
			if (_services.TryGetValue(type, out list))
			{
				foreach (var service in list)
				{
					yield return service;
				}
			}

			List<Func<IResolver, object>> getter;
			if (_registeredServices.TryGetValue(type, out getter))
			{
				foreach (var serviceFunc in getter)
				{
					yield return serviceFunc(_resolver);
				}
			}
		}

		#endregion

		/// <summary>
		/// Class Resolver.
		/// </summary>
		private class Resolver : IResolver
		{
			/// <summary>
			/// The _resolve object delegate
			/// </summary>
			private readonly Func<Type, IEnumerable<object>> _resolveObjectDelegate;

			/// <summary>
			/// Initializes a new instance of the <see cref="Resolver"/> class.
			/// </summary>
			/// <param name="resolveObjectDelegate">The resolve object delegate.</param>
			internal Resolver(Func<Type, IEnumerable<object>> resolveObjectDelegate)
			{
				_resolveObjectDelegate = resolveObjectDelegate;
			}

			#region IResolver Members

			/// <summary>
			/// Resolve a dependency.
			/// </summary>
			/// <typeparam name="T">Type of instance to get.</typeparam>
			/// <param name="constructorArgs">The constructor arguments.</param>
			/// <returns>An instance of {T} if successful, otherwise null.</returns>
			public T Resolve<T>(dynamic constructorArgs = null) where T : class
			{
				IEnumerable<T> results = this.ResolveAll<T>(constructorArgs);
				var result = results?.FirstOrDefault();

				return result;
			}

			/// <summary>
			/// Resolve a dependency by type.
			/// </summary>
			/// <param name="type">Type of object.</param>
			/// <param name="constructorArgs">The constructor arguments.</param>
			/// <returns>An instance to type if found as <see cref="object" />, otherwise null.</returns>
			public object Resolve(Type type, dynamic constructorArgs = null)
			{
				IEnumerable<object> results = this.ResolveAll(type, constructorArgs);
				var result = results?.FirstOrDefault();

				return result;
			}

			/// <summary>
			/// Resolve a dependency.
			/// </summary>
			/// <typeparam name="T">Type of instance to get.</typeparam>
			/// <param name="constructorArgs">The constructor arguments.</param>
			/// <returns>All instances of {T} if successful, otherwise null.</returns>
			public IEnumerable<T> ResolveAll<T>(dynamic constructorArgs = null) where T : class
			{
				var result = _resolveObjectDelegate(typeof(T)).Cast<T>();

				return result;
			}

			/// <summary>
			/// Resolve a dependency by type.
			/// </summary>
			/// <param name="type">Type of object.</param>
			/// <param name="constructorArgs">The constructor arguments.</param>
			/// <returns>All instances of type if found as <see cref="object" />, otherwise null.</returns>
			public IEnumerable<object> ResolveAll(Type type, dynamic constructorArgs = null)
			{
				return _resolveObjectDelegate(type);
			}

			/// <summary>
			/// Determines whether the specified type is registered.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns><c>true</c> if the specified type is registered; otherwise, <c>false</c>.</returns>
			public bool IsRegistered(Type type)
			{
				return Resolve(type) != null;
			}

			/// <summary>
			/// Determines whether this instance is registered.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <returns><c>true</c> if this instance is registered; otherwise, <c>false</c>.</returns>
			public bool IsRegistered<T>() where T : class
			{
				return Resolve<T>() != null;
			}

			/// <summary>
			/// Gets a value indicating whether this instance supports constructor arguments.
			/// </summary>
			/// <value><c>true</c> if this instance is constructor arguments supported; otherwise, <c>false</c>.</value>
			public bool IsConstructorArgsSupported => false;

			#endregion
		}
	}
}
