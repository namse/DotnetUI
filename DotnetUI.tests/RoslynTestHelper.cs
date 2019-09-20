namespace DotnetUI.tests
{
    public static class RoslynTestHelper
    {
        public static string GenerateCodeForExpression(string expression)
        {
            return $@"
class C
{{
    void Func() \{{
        var a = ({expression});
    }}
}}";
        }
    }
}