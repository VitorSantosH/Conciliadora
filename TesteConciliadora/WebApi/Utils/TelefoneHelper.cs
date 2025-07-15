namespace TesteConciliadora.WebApi.Utils;

public static class TelefoneHelper
{
    public static bool IsValid(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            return false;

        // Remove tudo que não é número
        var numeros = System.Text.RegularExpressions.Regex.Replace(telefone, @"\D", "");

        // Valida: DDD + número com 9 dígitos (ex: 11999990000)
        return System.Text.RegularExpressions.Regex.IsMatch(numeros, @"^[1-9]{2}9[0-9]{8}$");
    }
}