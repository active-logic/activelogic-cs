using Active.Core;

public class CoreTest : TestBase{

protected static readonly status done = status.@unchecked(+1),
                                 fail = status.@unchecked(-1),
                                 cont = status.@unchecked( 0);
                                 
}
