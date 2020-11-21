using ArgEx = System.ArgumentException;
using InvOp = System.InvalidOperationException;

﻿namespace Active.Core.Details{
/* NOTE: LogTrace is not allocated in logless mode */
public class LogTrace{

	const string PARENTHESIZED_REASON_ERR = "Do not parenthesize reasons";

	public readonly string scope;
	public readonly bool isDecorator;
	public readonly string reason;
	public readonly LogTrace next;

	public LogTrace(object scope, string reason=null){
		if(!status.log) throw new InvOp("Logging is disabled");
		this.scope  = scope.ToString();
		this.isDecorator = scope is IDecorator;
		this.next   = null;
		this.reason = TraceFormat.ReasonField(reason);
	}

	public LogTrace(object scope, LogTrace next, string reason){
		if(!status.log) throw new InvOp("Logging is disabled");
		this.scope  = scope.ToString();
		this.isDecorator = scope is IDecorator;
		this.next   = next;
		this.reason = TraceFormat.ReasonField(reason);
	}

	public string prefix { get; private set; }

	public void Prefix(char c) => prefix = c + prefix;

	public bool Matches(object scope, string reason)
		=> (this.scope.ToString()).Equals(scope)
		   && this.reason == TraceFormat.ReasonField(reason);

}}
