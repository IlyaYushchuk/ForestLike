using ForestLike.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace ForestLike.Services;

public class AuthorizationService
{
    IStorageController storageController;
    //Better not to touch
    byte[] salt = { 1, 2, 3, 4, 5, 6 };

    public AuthorizationService(IStorageController storageController)
    {
        this.storageController = storageController;
    }

    private string HashPassword(string password)
    {
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return hashedPassword;
    }

    public User Registration(string login, string password)
    {
        User user = new User { Login = login, HashPassword = HashPassword(password) } ;
        storageController.UploadNewUser(user);
        return storageController.GetUserInfo(user);
    }
    public User Login(string login, string password)
    {
        User user = new User { Login = login, HashPassword = HashPassword(password) };
        return storageController.GetUserInfo(user);
    }
}
