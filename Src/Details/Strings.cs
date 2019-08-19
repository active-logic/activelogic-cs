namespace Active.Core{
static class Strings{

    public class status{
        public static readonly string[] names = {"failing", "running", "done"};
    }

    public const string Bool = "bool",
        MinCharCountError = "Scope label should contain at least {0} chars",
        AvoidParens = "Do not parenthesize reasons",
        UnexpectedValue = "Unexpected left arg; using &, | instead of &&, ||?";

}}
