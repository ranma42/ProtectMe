using System;
using System.Collections.Generic;
using Mono.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;

public static class IListExtension
{
	public static void RemoveAll<T>(this IList<T> c, Func<T,bool> cond)
	{
		var newc = c.Where(x => !cond(x)).ToArray();

		c.Clear();
		foreach (var v in newc)
			c.Add(v);
	}

	public static CustomAttribute FindProtectedAttribute(this Collection<CustomAttribute> self)
	{
		return self.FirstOrDefault(x => x.AttributeType.FullName == "ProtectionAttributes.ProtectedAttribute");
	}
}

public class ModuleWeaver
{
	// Will log an informational message to MSBuild
	public Action<string> LogInfo { get; set; }

	// An instance of Mono.Cecil.ModuleDefinition for processing
	public ModuleDefinition ModuleDefinition { get; set; }

	// Init logging delegates to make testing easier
	public ModuleWeaver()
	{
		LogInfo = m => { };
	}

	public void Execute()
	{
		foreach (var type in ModuleDefinition.GetTypes())
		{
			ProcessType(type);
		}

		ModuleDefinition.AssemblyReferences.RemoveAll(x => x.Name == "ProtectionAttributes");
	}

	public void ProcessType(TypeDefinition typeDefinition)
	{
		foreach (var field in typeDefinition.Fields)
		{
			ProcessField(field);
		}

		foreach (var method in typeDefinition.Methods)
		{
			ProcessMethod(method);
		}
	}

	static void ProcessField(FieldDefinition field)
	{
		var attr = field.CustomAttributes.FindProtectedAttribute();
		if (attr == null)
			return;

		field.CustomAttributes.Remove(attr);
		field.IsFamily = true;
	}

	static void ProcessMethod(MethodDefinition method)
	{
		var attr = method.CustomAttributes.FindProtectedAttribute();
		if (attr == null)
			return;

		method.CustomAttributes.Remove(attr);
		method.IsFamily = true;
	}
}
