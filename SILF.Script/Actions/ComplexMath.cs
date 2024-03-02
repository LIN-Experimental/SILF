namespace SILF.Script.Actions;

internal class ComplexMath
{


    public static string MultiplicarNumeros(string num1, string num2)
    {
        // Convertir las cadenas a arreglos de caracteres y encontrar la posición de la coma
        char[] num1Array = num1.Replace(",", "").ToCharArray();
        char[] num2Array = num2.Replace(",", "").ToCharArray();
        int coma1Index = num1.IndexOf(',');
        int coma2Index = num2.IndexOf(',');

        // Calcular la longitud del resultado y crear un arreglo para almacenarlo
        int maxLength = num1Array.Length + num2Array.Length;
        int[] resultado = new int[maxLength];

        // Realizar la multiplicación cifra a cifra
        for (int i = num1Array.Length - 1; i >= 0; i--)
        {
            for (int j = num2Array.Length - 1; j >= 0; j--)
            {
                int digito1 = num1Array[i] - '0';
                int digito2 = num2Array[j] - '0';
                int producto = digito1 * digito2;
                int posicion1 = i + j;
                int posicion2 = i + j + 1;
                int suma = producto + resultado[posicion2];

                resultado[posicion1] += suma / 10;
                resultado[posicion2] = suma % 10;
            }
        }

        // Construir la cadena de resultado, incluyendo la coma decimal
        string resultadoString = "";
        for (int i = 0; i < maxLength; i++)
        {
            if (i == maxLength - (num1Array.Length + num2Array.Length - Math.Max(coma1Index, coma2Index)))
            {
                resultadoString += ",";
            }
            resultadoString += resultado[i];
        }

        // Eliminar ceros a la izquierda
        resultadoString = resultadoString.TrimStart('0');

        // Añadir un 0 delante del punto decimal si es necesario
        if (resultadoString[0] == ',')
        {
            resultadoString = "0" + resultadoString;
        }

        return resultadoString;
    }


    public static string RestarNumeros(string num1, string num2)
    {
        // Determinar si los números son negativos
        bool negativo1 = num1[0] == '-';
        bool negativo2 = num2[0] == '-';

        // Si solo uno de los números es negativo, devolver la suma de ambos con el signo del más grande
        if (negativo1 ^ negativo2)
        {
            if (negativo1)
            {
                num1 = num1.Substring(1); // Eliminar el signo menos del primer número
                return SumarNumeros(num1, num2);
            }
            else
            {
                num2 = num2.Substring(1); // Eliminar el signo menos del segundo número
                return SumarNumeros(num1, num2);
            }
        }

        // Si ambos son negativos, restar normalmente pero cambiar el signo del resultado
        if (negativo1 && negativo2)
        {
            num1 = num1.Substring(1); // Eliminar el signo menos del primer número
            num2 = num2.Substring(1); // Eliminar el signo menos del segundo número
            return "-" + RestarNumeros(num2, num1);
        }

        // Resta normal si ambos son positivos
        return RestarPositivos(num1, num2);
    }



    static string RestarPositivos(string num1, string num2)
    {

        bool result = false;
        // Asegurarse de que num1 sea mayor o igual que num2 en longitud
        if (num1.Length < num2.Length || (num1.Length == num2.Length && string.Compare(num1, num2) < 0))
        {
            result = true;
            string temp = num1;
            num1 = num2;
            num2 = temp;
        }

        StringBuilder resultado = new StringBuilder();

        int carry = 0;

        for (int i = 0; i < num1.Length; i++)
        {
            int d1 = i < num1.Length ? num1[num1.Length - 1 - i] - '0' : 0;
            int d2 = i < num2.Length ? num2[num2.Length - 1 - i] - '0' : 0;

            int resta = d1 - d2 - carry;
            carry = 0;

            if (resta < 0)
            {
                resta += 10;
                carry = 1;
            }

            resultado.Insert(0, resta);
        }

        // Eliminar ceros no significativos en la parte delantera
        while (resultado.Length > 1 && resultado[0] == '0')
        {
            resultado.Remove(0, 1);
        }

        if (result)
            return "-" + resultado.ToString();

        return resultado.ToString();
    }


    public static string SumarNumeros(string num1, string num2)
    {
        bool negativo1 = num1[0] == '-';
        bool negativo2 = num2[0] == '-';

        // Eliminar los signos de los números
        if (negativo1)
            num1 = num1.Substring(1);
        if (negativo2)
            num2 = num2.Substring(1);

        // Convertir las cadenas a arreglos de caracteres y encontrar la posición de la coma
        char[] num1Array = num1.Replace(",", "").ToCharArray();
        char[] num2Array = num2.Replace(",", "").ToCharArray();
        int coma1Index = num1.IndexOf(',');
        int coma2Index = num2.IndexOf(',');

        // Calcular la longitud del resultado y crear un arreglo para almacenarlo
        int maxLength = Math.Max(num1Array.Length, num2Array.Length) + 1;
        int[] resultado = new int[maxLength];

        int carry = 0;

        // Realizar la suma cifra a cifra
        for (int i = 0; i < maxLength; i++)
        {
            int digito1 = (i < num1Array.Length) ? num1Array[num1Array.Length - 1 - i] - '0' : 0;
            int digito2 = (i < num2Array.Length) ? num2Array[num2Array.Length - 1 - i] - '0' : 0;

            if (negativo1)
                digito1 *= -1;
            if (negativo2)
                digito2 *= -1;

            int suma = digito1 + digito2 + carry;
            carry = suma / 10;
            resultado[maxLength - 1 - i] = Math.Abs(suma % 10);
        }

        // Construir la cadena de resultado, incluyendo la coma decimal
        string resultadoString = "";
        for (int i = 0; i < maxLength; i++)
        {
            if (i == maxLength - (Math.Max(coma1Index, coma2Index)))
            {
                resultadoString += ",";
            }
            resultadoString += resultado[i];
        }

        // Eliminar ceros a la izquierda
        resultadoString = resultadoString.TrimStart('0');

        // Añadir un 0 delante del punto decimal si es necesario
        if (resultadoString[0] == ',')
        {
            resultadoString = "0" + resultadoString;
        }

        return resultadoString;
    }

}
