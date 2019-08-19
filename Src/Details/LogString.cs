using Ex = System.Exception;

namespace Active.Core.Details{
public class LogString{

    public string str;
    public bool valid{ get; private set; }

    LogString(string str){ this.str = str; }

    LogString(string str, bool valid) { this.str = str; this.valid = valid; }

    override public string ToString(){
        return valid ? str : throw new Ex("Can't extract invalid string");
    }

    public static bool Invalid(LogString s) => s != null && !s.valid;

    public static bool operator true (LogString s)  => throw new Ex("N/A");

    public static bool operator false (LogString s) => !status.log;

    // This is used when RHS of x && y is evaluated, and it has to be
    // implicit otherwise we'd have to cast manually.
    // But this can't be used to create a 'valid' string so a naive cast
    // of an un-guarded string will fail at runtime.
    public static implicit operator LogString(string str) => new LogString(str);

    // Only needed if status allows string as direct logtrace input
    //public static implicit operator string(LogString self){
    //    return self.str;
    //}

    public static LogString operator & (LogString x, LogString y)
        => new LogString(y.str, true);

    public static LogString operator | (LogString x, LogString y)
        => throw new Ex("N/A");

}}
