using Newtonsoft.Json.Linq;
using System.Text;
namespace CalcAPI.Data
{
    public class ComplexExpression
    {
        private List<string> parsedExpression;
        public double Result { get; }

        public ComplexExpression(string str)
        {
            var expression = MarkSpecialSigns(str);

            parsedExpression = expression.Split(' ').ToList();
            parsedExpression.RemoveAll(x => x == "");
        }
        public ComplexExpression(List<string> parsed)
        {
            parsedExpression = parsed;
        }


        private string MarkSpecialSigns(string expression)
        {
            StringBuilder stringBuilder = new StringBuilder(expression);

            stringBuilder.Replace("+", " + ");
            stringBuilder.Replace("–", " – ");
            stringBuilder.Replace("-", " - ");
            stringBuilder.Replace("*", " * ");
            stringBuilder.Replace("/", " / ");
            stringBuilder.Replace("^", " ^ ");
            stringBuilder.Replace("√", " √ ");
            stringBuilder.Replace("(", " ( ");
            stringBuilder.Replace(")", " ) ");

            return stringBuilder.ToString();
        }

        public double CalculateResult()
        {
            CheckForExceptionsInExpression(parsedExpression);

            var buffer = parsedExpression;
            //Скобки
            buffer = CalculateExpressionsInBrackets(buffer);
            // Степени
            buffer = CalculatePowersAndRoots(buffer);
            // Умножения и Деления
            buffer = CalculateMultiplicationsAndDivision(buffer);
            // Сложение вычитания
            buffer = CalculateAdditionsAndSubstraction(buffer);
            

            //В конце выражение должно сократиться до одного элемента
            if (buffer.Count == 1)
                return Convert.ToDouble(buffer[0]);
            else
                throw new Exception("Непредвиденная ошибка ");
        }

        private void CheckForExceptionsInExpression(List<string> expression)
        {
            List<string> allowedMathematicalSigns = new List<string> { "+", "–", "-", "*", "/", "^", "√" };
            bool numberExpected = true;
            foreach (var item in expression)
            {
                if (numberExpected)
                {
                    if (item == "(")
                        numberExpected = true;
                    else
                    {
                        //Провека на записи числа 
                        try
                        {
                            Convert.ToDouble(item);
                            numberExpected = false;
                        }
                        catch (InvalidCastException)
                        {
                            throw new Exception($"Ошибка записи : В элементе |{item}| Ожидается число или открывающаяся скобка");
                        }
                    }
                }
                else
                {
                    if (item == ")")
                        numberExpected = false;
                    // Проверка на Математический знак
                    else if (allowedMathematicalSigns.Contains(item)) 
                        numberExpected = true;
                    else
                        throw new Exception($"Ошибка записи : В элементе |{item}| Ожидается математический знак или закрывающаяся скобка");
                }
            }
        }
        private List<string> CalculateExpressionsInBrackets(List<string> buffer)
        {
            while (buffer.Contains("(") && buffer.Contains(")"))
            {
                int start = buffer.LastIndexOf("(");
                int end = buffer.IndexOf(")");
                var expression = new ComplexExpression(buffer.GetRange(start + 1, end - start - 1));
                double result = expression.CalculateResult();

                buffer[start] = result.ToString();
                for (int i = start + 1; i <= end;)
                {
                    buffer.RemoveAt(i);
                    end--;
                }
            }
            return buffer;
        }
        private List<string> CalculatePowersAndRoots(List<string> buffer)
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] == "^")
                {
                    double a = Convert.ToDouble(buffer[i - 1]);
                    double b = Convert.ToDouble(buffer[i + 1]);
                    double result = Math.Pow(a, b);

                    i--;
                    buffer[i] = result.ToString();
                    buffer.RemoveAt(i + 1);
                    buffer.RemoveAt(i + 1);
                }
                if (buffer[i] == "√")
                {
                    double a = Convert.ToDouble(buffer[i - 1]);
                    double b = Convert.ToDouble(buffer[i + 1]);
                    double result = Math.Pow(a, 1 / b);

                    i--;
                    buffer[i] = result.ToString();
                    buffer.RemoveAt(i + 1);
                    buffer.RemoveAt(i + 1);
                }
            }
            return buffer;
        }
        private List<string> CalculateMultiplicationsAndDivision(List<string> buffer)
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] == "*")
                {
                    double a = Convert.ToDouble(buffer[i - 1]);
                    double b = Convert.ToDouble(buffer[i + 1]);
                    double result = a * b;

                    i--;
                    buffer[i] = result.ToString();
                    buffer.RemoveAt(i + 1);
                    buffer.RemoveAt(i + 1);
                }
                if (buffer[i] == "/")
                {
                    double a = Convert.ToDouble(buffer[i - 1]);
                    double b = Convert.ToDouble(buffer[i + 1]);
                    double result = a / b;

                    i--;
                    buffer[i] = result.ToString();
                    buffer.RemoveAt(i + 1);
                    buffer.RemoveAt(i + 1);
                }
            }
            return buffer;
        }
        private List<string> CalculateAdditionsAndSubstraction(List<string> buffer)
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] == "+")
                {
                    double a = Convert.ToDouble(buffer[i - 1]);
                    double b = Convert.ToDouble(buffer[i + 1]);
                    double result = a + b;

                    i--;
                    buffer[i] = result.ToString();
                    buffer.RemoveAt(i + 1);
                    buffer.RemoveAt(i + 1);
                }
                if ((buffer[i] == "–") || (buffer[i] == "-"))       // ЭТО ДВА РАЗНЫХ СИМВОЛА (один из них тире, а другой дефис)
                {
                    double a = Convert.ToDouble(buffer[i - 1]);
                    double b = Convert.ToDouble(buffer[i + 1]);
                    double result = a - b;

                    i--;
                    buffer[i] = result.ToString();
                    buffer.RemoveAt(i + 1);
                    buffer.RemoveAt(i + 1);
                }
            }
            return buffer;
        }

    }
}
