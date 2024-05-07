using Serializer.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ForestLike.ClientServerLogic;
using Serializer;
using DnsClient;


namespace ForestLike.Services;

public class AuthorizationService
{
    //TODO temporary solution
    static byte[] salt = { 1, 2, 3, 4, 5, 6 };
    Client client;

    public AuthorizationService(Client client)
    {
        this.client = client;
    }
    ~AuthorizationService()
    {
        Logout();
    }

    public async Task<string> Register(string login, string password)
    {
        User user = new User { Login = login, HashPassword = HashPassword(password) };
        string registrationRequest = "/registration " + EntitySerializer.SerializeUser(user);
        await client.SendDataToServer(registrationRequest);
        string response = await client.GetDataFromServer();

        //TODO delete next string
        Console.WriteLine(response);
        return response;
    }
    public async Task<string> Login(string login, string password)
    {
        User user = new User { Login = login, HashPassword = HashPassword(password) };
        string loginRequest = "/login " + EntitySerializer.SerializeUser(user);
        await client.SendDataToServer(loginRequest);
        string response = await client.GetDataFromServer();

        //TODO delete next string
            Console.WriteLine(response);
        return response;
    }

    public async Task<string> Login(User user)
    {
        return Login(user.Login, user.HashPassword).Result;
    }
    public async Task<string> Register(User user)
    {
        return Register(user.Login, user.HashPassword).Result;
    }
    public async Task<string> Logout()
    {
        string logoutRequest = "/logout ";
        await client.SendDataToServer(logoutRequest);
        string response = await client.GetDataFromServer();

        //TODO delete next string
        Console.WriteLine(response);
        return response;
    }

    private string HashPassword(string password)
    {
        //on testing not working
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return password;
    }

}
