namespace BankSoftware.Domain.Factories
{
    public class NumberFactory
    {
        public static string Generate()
        {
            return $"ln-{DateTime.UtcNow:dd/MM/yyyy}-{Guid.NewGuid().ToString("N")}";
        }
    }
}