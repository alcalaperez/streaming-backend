﻿using System;
using System.Linq;
using System.Security.Cryptography;
/// <summary>
/// https://medium.com/dealeron-dev/storing-passwords-in-net-core-3de29a3da4d2
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);

    (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
}


public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32; // 256 bit
    private const int topIterations = 1000; // How many iterations

    public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
    {
        var parts = hash.Split('.', 3);

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. " +
              "Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        var needsUpgrade = iterations != topIterations;

        using (var algorithm = new Rfc2898DeriveBytes(
          password,
          salt,
          iterations,
          HashAlgorithmName.SHA256))
        {
            var keyToCheck = algorithm.GetBytes(KeySize);

            var verified = keyToCheck.SequenceEqual(key);

            return (verified, needsUpgrade);
        }
    }

    public string Hash(string password)
    {
        using (var algorithm = new Rfc2898DeriveBytes(
          password,
          SaltSize,
          topIterations,
          HashAlgorithmName.SHA256))
        {
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{topIterations}.{salt}.{key}";
        }
    }
}