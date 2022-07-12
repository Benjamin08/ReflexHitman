using System;

public class MonoPInvokeCallbackAttribute : Attribute
{
	private readonly Type type;
	public MonoPInvokeCallbackAttribute( Type t ) { type = t; }
}

