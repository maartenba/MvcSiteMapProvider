// (c) Copyright Telerik Corp. 
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.

// Modified by Maarten Balliauw - http://mvcsitemap.codeplex.com

#region Using directives

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;

#endregion

namespace Telerik.Web.Mvc.Infrastructure.Implementation
{
    /// <summary>
    /// AuthorizeAttributeBuilder class
    /// </summary>
    public class AuthorizeAttributeBuilder
    {
        private static readonly Type AuthorizeAttributeType = typeof(IAuthorizeAttribute);
        private static readonly ModuleBuilder Module = CreateModuleBuilder();

        /// <summary>
        /// Builds the specified parent type.
        /// </summary>
        /// <param name="parentType">Type of the parent.</param>
        /// <returns>Constructor information.</returns>
        public ConstructorInfo Build(Type parentType)
        {
            var typeName = "$" + parentType.FullName.Replace(".", string.Empty);

            var definedType = Module.GetType(typeName);
            if (definedType == null)
            {
                var typeBuilder = Module.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit, parentType, new[] { AuthorizeAttributeType });
                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
                typeBuilder.AddInterfaceImplementation(AuthorizeAttributeType);

                WriteProperty(parentType, typeBuilder, "Order", typeof(int));
                WriteProperty(parentType, typeBuilder, "Roles", typeof(string));
                WriteProperty(parentType, typeBuilder, "Users", typeof(string));
                WriteIsAuthorized(parentType, typeBuilder);

                definedType = typeBuilder.CreateType();
            }

            return definedType.GetConstructor(Type.EmptyTypes);
        }

        /// <summary>
        /// Writes the property.
        /// </summary>
        /// <param name="parentType">Type of the parent.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        private static void WriteProperty(Type parentType, TypeBuilder builder, string name, Type type)
        {
            const BindingFlags BindingFlag = BindingFlags.Public | BindingFlags.Instance;
            const MethodAttributes MethodAttribute = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot;

            string getName = "get_" + name;
            string setName = "set_" + name;

            MethodInfo parentGetMethod = parentType.GetMethod(getName, BindingFlag);
            MethodBuilder implementedGetMethod = builder.DefineMethod(getName, MethodAttribute, type, Type.EmptyTypes);
            ILGenerator getIl = implementedGetMethod.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Call, parentGetMethod);
            getIl.Emit(OpCodes.Ret);

            MethodInfo interfaceGetMethod = AuthorizeAttributeType.GetMethod(getName, BindingFlag);
            builder.DefineMethodOverride(implementedGetMethod, interfaceGetMethod);

            MethodInfo parentSetMethod = parentType.GetMethod(setName, BindingFlag);
            MethodBuilder implementedSetMethod = builder.DefineMethod(setName, MethodAttribute, typeof(void), new[] { type });
            ILGenerator setIl = implementedSetMethod.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Call, parentSetMethod);
            setIl.Emit(OpCodes.Ret);

            MethodInfo interfaceSetMethod = AuthorizeAttributeType.GetMethod(setName, BindingFlag);
            builder.DefineMethodOverride(implementedSetMethod, interfaceSetMethod);
        }

        /// <summary>
        /// Writes the is authorized.
        /// </summary>
        /// <param name="parentType">Type of the parent.</param>
        /// <param name="builder">The builder.</param>
        private static void WriteIsAuthorized(Type parentType, TypeBuilder builder)
        {
            var protectedMethod = parentType.GetMethod("AuthorizeCore", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            var implementedMethod = builder.DefineMethod("IsAuthorized", MethodAttributes.Public | MethodAttributes.Virtual, typeof(bool), new[] { typeof(HttpContextBase) });
            var il = implementedMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, protectedMethod);
            il.Emit(OpCodes.Ret);

            var interfaceMethod = AuthorizeAttributeType.GetMethod("IsAuthorized", BindingFlags.Public | BindingFlags.Instance);
            builder.DefineMethodOverride(implementedMethod, interfaceMethod);
        }

        /// <summary>
        /// Creates the module builder.
        /// </summary>
        /// <returns>
        /// A module builder represented as a <see cref="ModuleBuilder"/> instance 
        /// </returns>
        private static ModuleBuilder CreateModuleBuilder()
        {
            const string name = "InheritedAuthorizeAttributes";

            var assemblyName = new AssemblyName(name + "Assembly")
            {
                Version = typeof(AuthorizeAttributeBuilder).Assembly.GetName().Version
            };

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name + "Module");

            return moduleBuilder;
        }
    }
}

