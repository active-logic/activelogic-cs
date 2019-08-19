using System;
using ArgEx = System.ArgumentException;

namespace Active.Core.Details{

public static class TraceFormat{

	const char LPARENS = '(', NOT = '!';
	const int MinCharCountForScope = 2;

	// LogTrace uses this to format its `reason` field.
	public static string ReasonField(string reason)
		=> reason==null || reason.Length == 0 ? null :
		   reason[0] == LPARENS ? throw new ArgEx(Strings.AvoidParens) :
		   reason;

	// StatusFormat uses this to format the trace attached to a status.
	public static string LogTrace(LogTrace trace){
		if(trace == null) return "?trace";
		var @out = string.Format(
			"{0}{1} {2}", trace.prefix ?? null,
						  trace.scope,
						  Reason(trace.reason, trace.isDecorator)
		).Trim();
		return trace.next == null ? @out : $"{@out} -> {LogTrace(trace.next)}";
	}

	public static string DecoratorReason(object target, string reason)
		=> ( (string)target == "."
			 ? $"{ReasonField(reason)}"
			 : $"{ReasonField(reason)} {DecoratorTarget(target)}" ).Trim();

	public static string Scope(LogTrace t) => t?.scope.ToString();

	static string DecoratorTarget(object target) => target?.ToString() ?? "?";

	/* Used by TraceFormat.LogTrace */
	static string Reason(string reason, bool isDecorator)
		=> isDecorator ? reason : reason == null ? null : $"({reason})";

}}
