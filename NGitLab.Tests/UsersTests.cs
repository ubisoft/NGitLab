using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class UsersTests
{
    [Test]
    [NGitLabRetry]
    public async Task GetUsers()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.Client.Users.All.Take(100).ToArray(); // We don't want to enumerate all users
        Assert.That(users, Is.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetUser()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var current = context.Client.Users.Current;

        var user = context.Client.Users[current.Id];
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Username, Is.EqualTo(current.Username));
    }

    [Test]
    [NGitLabRetry]
    public async Task CreateUpdateDelete()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.AdminClient.Users;

        var addedUser = CreateNewUser(context);
        Assert.That(addedUser.Bio, Is.EqualTo("bio"));

        var updatedUser = users.Update(addedUser.Id, new UserUpsert { Bio = "Bio2" });
        Assert.That(updatedUser.Bio, Is.EqualTo("Bio2"));

        users.Delete(addedUser.Id);

        await GitLabTestContext.RetryUntilAsync(() => users.Get(addedUser.Username).ToList(), users => users.Count == 0, TimeSpan.FromMinutes(2));
    }

    [Test]
    [NGitLabRetry]
    public async Task CreateAsync()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.AdminClient.Users;

        var addedUser = await CreateNewUserAsync(context);
        Assert.That(addedUser.Bio, Is.EqualTo("bio"));

        await GitLabTestContext.RetryUntilAsync(() => users.Get(addedUser.Username).ToList(), users => users.Count != 0, TimeSpan.FromMinutes(2));
    }

    [Test]
    [NGitLabRetry]
    public async Task GetUserByEmailDoesNotWorkOnNonAdminClient()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var addedUser = CreateNewUser(context);

        var searchedUsers = context.Client.Users.Search(addedUser.Email);
        Assert.That(!searchedUsers.Any());
    }

    [Test]
    [NGitLabRetry]
    public async Task GetUserByEmailWorksOnAdminClient()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var addedUser = CreateNewUser(context);

        var searchedUsers = context.AdminClient.Users.Search(addedUser.Email);
        Assert.That(searchedUsers.SingleOrDefault(u => u.Email.Equals(addedUser.Email, StringComparison.OrdinalIgnoreCase)) is not null);
    }

    [Test]
    [NGitLabRetry]
    public async Task DeactivatedAccountShouldBeAbleToActivate()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.AdminClient.Users;

        var addedUser = CreateNewUser(context);

        users.Deactivate(addedUser.Id);

        Assert.That(users[addedUser.Id].State, Is.EqualTo("deactivated"));

        users.Activate(addedUser.Id);

        Assert.That(users[addedUser.Id].State, Is.EqualTo("active"));

        users.Delete(addedUser.Id);

        await GitLabTestContext.RetryUntilAsync(() => users.Get(addedUser.Username).ToList(), users => users.Count == 0, TimeSpan.FromMinutes(2));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_add_an_ssh_key_to_the_gitlab_profile()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var keys = context.Client.Users.CurrentUserSShKeys;

        var generatedKey = RsaSshKey.GenerateQuickest();

        var result = keys.Add(new SshKeyCreate
        {
            Key = generatedKey.PublicKey,
            Title = "test key",
        });

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Key, Does.StartWith(generatedKey.PublicKey));

        var newKey = keys.All.FirstOrDefault(k => k.Id == result.Id);

        Assert.That(newKey, Is.Not.Null);
        Assert.That(newKey.Key, Is.EqualTo(result.Key));

        keys.Remove(result.Id);

        Assert.That(keys.All.FirstOrDefault(k => k.Id == result.Id), Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task CreateTokenAsAdmin_ReturnsUserToken()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.AdminClient.Users;

        var tokenRequest = new UserTokenCreate
        {
            UserId = users.Current.Id,
            Name = FormattableString.Invariant($"Test_Create_{DateTime.UtcNow:yyyyMMddHHmmss}"),
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            Scopes = new[] { "api", "read_user" },
        };

        var tokenResult = users.CreateToken(tokenRequest);

        Assert.That(tokenResult.Token, Is.Not.Empty);
        Assert.That(tokenResult.Name, Is.EqualTo(tokenRequest.Name));
    }

    [Test]
    [NGitLabRetry]
    public async Task CreateTokenAsyncAsAdmin_ReturnsUserToken()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.AdminClient.Users;

        var tokenRequest = new UserTokenCreate
        {
            UserId = users.Current.Id,
            Name = FormattableString.Invariant($"Test_Create_{DateTime.UtcNow:yyyyMMddHHmmss}"),
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            Scopes = new[] { "api", "read_user" },
        };

        var tokenResult = await users.CreateTokenAsync(tokenRequest);

        Assert.That(tokenResult.Token, Is.Not.Empty);
        Assert.That(tokenResult.Name, Is.EqualTo(tokenRequest.Name));
    }

    [Test]
    [NGitLabRetry]
    public async Task CreateTokenAsyncAsAdmin_WhenUserNotFound_ItThrowsBadRequest()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var users = context.AdminClient.Users;

        var tokenRequest = new UserTokenCreate
        {
            UserId = int.MaxValue,
            Name = "foo",
            ExpiresAt = DateTime.UtcNow.Date.AddDays(7),
            Scopes = new[] { "write_repository" },
        };

        var ex = Assert.ThrowsAsync<GitLabException>(() => users.CreateTokenAsync(tokenRequest));
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(ex.ErrorMessage, Is.EqualTo("404 User Not Found"));
    }

    [Test]
    [NGitLabRetry]
    public async Task GetLastActivityDates()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var activities = context.AdminClient.Users.GetLastActivityDatesAsync().ToArray();
        Assert.That(activities.Where(a => string.Equals(a.Username, context.AdminClient.Users.Current.Username, StringComparison.Ordinal)), Is.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetLastActivityDatesSinceYesterday()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var yesterday = DateTimeOffset.UtcNow.AddDays(-1);
        var activities = context.AdminClient.Users.GetLastActivityDatesAsync(from: yesterday).ToArray();
        Assert.That(activities, Is.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetLastActivityDatesFromTheFuture()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var tomorrow = DateTimeOffset.UtcNow.AddDays(1);
        var activities = context.AdminClient.Users.GetLastActivityDatesAsync(from: tomorrow).ToArray();
        Assert.That(activities, Is.Empty);
    }

    [Test]
    public async Task GetLastActivityDates_UsingNonAdminCredentials()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var privateProfileUsers = context.AdminClient.Users.All.Where(u => u.PrivateProfile).Select(ppu => ppu.Username).ToList();
        Assert.That(privateProfileUsers, Is.Not.Empty);

        if (context.IsGitLabMajorVersion(15))
        {
            var exception = Assert.Throws<GitLabException>(() => context.Client.Users.GetLastActivityDatesAsync().ToArray());
            Assert.That(exception?.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        else
        {
            var lastActivityDates = context.Client.Users.GetLastActivityDatesAsync().ToList();
            Assert.That(lastActivityDates, Is.Not.Empty);

            var lastActivityDatesOfPrivateProfileUsers = lastActivityDates.Where(lad => privateProfileUsers.Contains(lad.Username, StringComparer.Ordinal)).ToList();
            Assert.That(lastActivityDatesOfPrivateProfileUsers, Is.Empty);
        }
    }

    private static UserUpsert CreateNewUserUpsert(GitLabTestContext context)
    {
        var randomNumber = context.GetRandomNumber().ToString(CultureInfo.InvariantCulture);
        return new UserUpsert
        {
            Email = $"test{randomNumber}@test.pl",
            Bio = "bio",
            CanCreateGroup = true,
            IsAdmin = true,
            Linkedin = null,
            Name = $"NGitLab Test User {randomNumber}",
            Password = "!@#$QWDRQW@",
            ProjectsLimit = 1000,
            Provider = "provider",
            ExternalUid = $"external_uid_{randomNumber}",
            Skype = "skype",
            Twitter = "twitter",
            Username = $"ngitlabtestuser{randomNumber}",
            WebsiteURL = "https://www.example.com",
        };
    }

    private static User CreateNewUser(GitLabTestContext context)
    {
        return context.AdminClient.Users.Create(CreateNewUserUpsert(context));
    }

    private static Task<User> CreateNewUserAsync(GitLabTestContext context)
    {
        return context.AdminClient.Users.CreateAsync(CreateNewUserUpsert(context));
    }

    // Comes from https://github.com/meziantou/Meziantou.GitLabClient/blob/main/test/Meziantou.GitLabClient.Tests/Internals/RsaSshKey.cs
    private sealed class RsaSshKey
    {
        private const int PrefixSize = 4;
        private const int PaddedPrefixSize = PrefixSize + 1;
        private const string KeyType = "ssh-rsa";

        public string PublicKey { get; }

        public string PrivateKey { get; }

        public RsaSshKey(string publicKey, string privateKey)
        {
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            PrivateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
        }

        public static RsaSshKey GenerateQuickest()
        {
            using var rsa = RSA.Create();
            var size = rsa.LegalKeySizes.Min();
            rsa.KeySize = size.MinSize;

            return Generate(rsa);
        }

        public static RsaSshKey Generate(int keyLength)
        {
            using var rsa = RSA.Create();
            rsa.KeySize = keyLength;
            return Generate(rsa);
        }

        public static RsaSshKey Generate(RSA cryptoServiceProvider)
        {
            var keyParameters = cryptoServiceProvider.ExportParameters(includePrivateParameters: true);

            var publicBuffer = new byte[3 + KeyType.Length + PaddedPrefixSize + keyParameters.Exponent.Length + PaddedPrefixSize + keyParameters.Modulus.Length + 1];
            using (var writer = new BinaryWriter(new MemoryStream(publicBuffer)))
            {
                writer.Write(new byte[] { 0x00, 0x00, 0x00 });
                writer.Write(KeyType);
                WritePrefixed(writer, keyParameters.Exponent, addLeadingNull: true);
                WritePrefixed(writer, keyParameters.Modulus, addLeadingNull: true);
            }

            var privateBuffer = new byte[PaddedPrefixSize + keyParameters.D.Length + PaddedPrefixSize + keyParameters.P.Length + PaddedPrefixSize + keyParameters.Q.Length + PaddedPrefixSize + keyParameters.InverseQ.Length];
            using (var writer = new BinaryWriter(new MemoryStream(privateBuffer)))
            {
                WritePrefixed(writer, keyParameters.D, addLeadingNull: true);
                WritePrefixed(writer, keyParameters.P, addLeadingNull: true);
                WritePrefixed(writer, keyParameters.Q, addLeadingNull: true);
                WritePrefixed(writer, keyParameters.InverseQ, addLeadingNull: true);
            }

            var publicBlob = KeyType + " " + Convert.ToBase64String(publicBuffer);
            var privateBlob = Convert.ToBase64String(privateBuffer);
            return new RsaSshKey(publicBlob, privateBlob);
        }

        private static void WritePrefixed(BinaryWriter writer, byte[] bytes, bool addLeadingNull = false)
        {
            var length = bytes.Length;
            if (addLeadingNull)
            {
                length++;
            }

            writer.Write(BitConverter.GetBytes(length).Reverse().ToArray());
            if (addLeadingNull)
            {
                writer.Write((byte)0x00);
            }

            writer.Write(bytes);
        }
    }
}
