#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using System.Text;
using ArgEx = System.ArgumentException;
//using UnityEngine;
using S = Active.Core.Strings;

namespace Active.Core.Details{
public class StatusFormat{

	const bool UseLineNumbers = false;
	const char ACCESS = '.', NL = '\n', COLON = ':', CONT = '+', SPACE = ' ';
	const int  Tabs = 2;
    static char[] statusChars = {'✗', '→', '✓'};

	public static void UseASCII() => statusChars = new []{' ', '+', '*'};

	public static string Decorator(object x) => $"<{x.GetType().Name[0]}>";

  #if !AL_OPTIMIZE

	public static string CallTree(in status status){
		var builder = new StringBuilder();
		Hierarchy(status, builder, tabs: Tabs);
		return builder.ToString();
	}

	public static string Status(in status s)
	=> $"{Symbol(s)} {TraceFormat.LogTrace(s.trace)}";

	public static string Name(in status s) => S.status.names[RepIndex(s.raw)];

	public static char Symbol(in status s) => statusChars[RepIndex(s.raw)];

	public static string SysTrace(string path, string member, int line)
	=> LastPathComponent(path)
	   + (member.ToLower() != "action" ? ACCESS + member          : "")
	   + (UseLineNumbers               ? COLON  + line.ToString() : "");

	public static string ToString(in status s)
	=> $"{Name(s)} ({TraceFormat.Scope(s.trace)})";

	static void Hierarchy(in status s, StringBuilder @out, int depth=0,
														   int tabs=2){
		@out.Append(SPACE, depth*tabs).Append(Status(s) + NL);
		var C = s.meta.components;
		if(C != null) foreach(var c in C) Hierarchy(c, @out, depth + 1, tabs);
	}

	public static string LastPathComponent(string path) => path.Substring(
		path.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1
	).Replace(".cs", null);

	static int RepIndex(int sval) => sval > 0 ? 2 : sval < 0 ? 0 : 1;

  #endif

}}
