// ***********************************************************************
// Assembly         : Invisionware.Ioc
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IResolver.cs" company="XLabs Team">
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

namespace Invisionware.IoC
{
	/// <summary>
	/// Interface definition for dependency resolver.
	/// </summary>
	public interface IResolver
	{
		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <param name="constructorArgs">The constructor arguments.</param>
		/// <returns>An instance of {T} if successful, otherwise null.</returns>
		T Resolve<T>(dynamic constructorArgs = null) where T : class;

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <param name="constructorArgs">The constructor arguments.</param>
		/// <returns>An instance to type if found as <see cref="object" />, otherwise null.</returns>
		object Resolve(Type type, dynamic constructorArgs = null);

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <param name="constructorArgs">The constructor arguments.</param>
		/// <returns>All instances of {T} if successful, otherwise null.</returns>
		IEnumerable<T> ResolveAll<T>(dynamic constructorArgs = null) where T : class;

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <param name="constructorArgs">The constructor arguments.</param>
		/// <returns>All instances of type if found as <see cref="object" />, otherwise null.</returns>
		IEnumerable<object> ResolveAll(Type type, dynamic constructorArgs = null);

		/// <summary>
		/// Determines whether the specified type is registered.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is registered; otherwise, <c>false</c>.</returns>
		bool IsRegistered(Type type);

		/// <summary>
		/// Determines whether this instance is registered.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns><c>true</c> if this instance is registered; otherwise, <c>false</c>.</returns>
		bool IsRegistered<T>() where T : class;

		/// <summary>
		/// Gets a value indicating whether this instance supports constructor arguments.
		/// </summary>
		/// <value><c>true</c> if this instance is constructor arguments supported; otherwise, <c>false</c>.</value>
		bool IsConstructorArgsSupported { get; }
	}
}
