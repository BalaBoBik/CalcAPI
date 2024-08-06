using System.Text;
namespace CalcAPI.Data
{
    public class ComplexExpression
    {
        private string expression;
        private List<string> parsedExpression;

        public ComplexExpression(string str)
        {
            expression = MarkSpecialSigns(str);

            parsedExpression = expression.Split(' ').ToList();
            parsedExpression.RemoveAll(x => x == "");
        }

        private string MarkSpecialSigns(string expression)
        {
            StringBuilder stringBuilder = new StringBuilder(expression);

            stringBuilder.Replace("+", " + ");
            stringBuilder.Replace("-", " - ");
            stringBuilder.Replace("*", " * ");
            stringBuilder.Replace("/", " / ");
            stringBuilder.Replace("^", " ^ ");
            stringBuilder.Replace("(", " ( ");
            stringBuilder.Replace(")", " ) ");

            return stringBuilder.ToString();
        }
        public List<string> GetParsedExpression()
        {
            return parsedExpression;
        }
    }
}
