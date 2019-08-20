using ArgEx = System.ArgumentException;

namespace Active.Core.Details{
public class ValidString{

    readonly string value;

    ValidString(string s) => value = s;

    public static implicit operator ValidString(LogString s)
    => (s == null) ? null                    :
       s.valid     ? new ValidString(s.str)  :
       throw new ArgEx("Unchecked logstring");

    public static implicit operator string(ValidString s) => s?.value;

    #if !AL_STRICT
      public static implicit operator ValidString(string that)
      => new ValidString(that);
    #endif

}}
