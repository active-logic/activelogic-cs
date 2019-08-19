using Active.Core.Details;
using Active.Editors;
using System.Diagnostics;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using V  = Active.Core.Details.ValidString;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using S  = System.String;

namespace Active.Core{
partial class UGig{

[Conditional("DEBUG")]
protected void Trace([P]S p="", [M]S m="", [L]int l=0){
    var file = StatusFormat.LastPathComponent(p);
    ActiveConsole.instance?.Log($"{file}::{m} [{l}]\n",
                                this.gameObject);
}

[Conditional("DEBUG")]
protected void Log(S message = null, [P]S p="", [M]S m="", [L]int l=0){
    var file = StatusFormat.LastPathComponent(p);
    ActiveConsole.instance?.Log($"{file}::{m} [{l}] {message}\n",
                                this.gameObject);
}

[Conditional("DEBUG")]
protected void Log(V message = null, [P]S p="", [M]S m="", [L]int l=0){
    var file = StatusFormat.LastPathComponent(p);
    ActiveConsole.instance?.Log($"{file}::{m} [{l}] {message}\n",
                                this.gameObject);
}

}}
