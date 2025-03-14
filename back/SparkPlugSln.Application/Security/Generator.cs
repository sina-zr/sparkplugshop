namespace SparkPlugSln.Application.Security;

public static class Generator
{
    public static int GenerateVerificationCode()
    {
        Random random = new Random();
        var code = random.Next(100000, 1000000);
        return code;
    }
}