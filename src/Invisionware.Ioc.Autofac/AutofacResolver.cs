// ***********************************************************************
// Assembly         : Invisionware.Ioc.Autofac
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AutofacResolver.cs" company="XLabs Team">
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;

namespace Invisionware.IoC.Autofac
{
	/// <summary>
	/// The Autofac resolver.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class AutofacResolver : IResolver
	{
		private readonly IContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutofacResolver"/> class.
		/// </summary>
		/// <param name="container">
		/// The container.
		/// </param>
		public AutofacResolver(IContainer container)
		{
			this.container = container;
		}

		#region IResolver Members

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>An instance of {T} if successful, otherwise null.</returns>
		public T Resolve<T>(dynamic constructorArgs = null) where T : class
		{
			var args = ((object) constructorArgs).ToParameters();

			return args != null ? this.container.ResolveOptional<T>(args) : this.container.ResolveOptional<T>();
		}

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <param name="constructorArgs">The constructor arguments.</param>
		/// <returns>An instance to type if found as <see cref="object" />, otherwise null.</returns>
		public object Resolve(Type type, dynamic constructorArgs = null)
		{
			var args = ((object)constructorArgs).ToParameters();

			return args != null ? this.container.ResolveOptional(type, args) : this.container.ResolveOptional(type);
		}

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>All instances of {T} if successful, otherwise null.</returns>
		public IEnumerable<T> ResolveAll<T>(dynamic constructorArgs = null) where T : class
		{
			var args = ((object)constructorArgs).ToParameters();

			return this.Resolve<IEnumerable<T>>(args);
		}

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <param name="constructorArgs">The constructor arguments.</param>
		/// <returns>All instances of type if found as <see cref="object" />, otherwise null.</returns>
		public IEnumerable<object> ResolveAll(Type type, dynamic constructorArgs = null)
		{
			Type listType = typeof(IEnumerable<>);
			Type[] typeArgs = {type};
			Type typeConstructed = listType.MakeGenericType(typeArgs);

			return (IEnumerable<object>)this.Resolve(typeConstructed, constructorArgs);
		}

		/// <summary>
		/// Determines whether the specified type is registered.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is registered; otherwise, <c>false</c>.</returns>
		public bool IsRegistered(Type type)
		{
			return this.container.IsRegistered(type);
		}

		/// <summary>
		/// Determines whether this instance is registered.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns><c>true</c> if this instance is registered; otherwise, <c>false</c>.</returns>
		public bool IsRegistered<T>() where T : class
		{
			return this.container.IsRegistered<T>();
		}

		/// <summary>
		/// Gets a value indicating whether this instance supports constructor arguments.
		/// </summary>
		/// <value><c>true</c> if this instance is constructor arguments supported; otherwise, <c>false</c>.</value>
		public bool IsConstructorArgsSupported => true;

		#endregion
	}

	internal static class DynamicExtensions
	{
		public static IEnumerable<NamedParameter> ToParameters<T>(this T obj)
		{
			Type cArgsType = obj?.GetType();

			var args = cArgsType?.GetProperties().Select(x =>
				new NamedParameter(x.Name, x.GetValue(obj)));

			return args;
		}
	}
}
