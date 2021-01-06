using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using NGitLab.Models;
using NUnit.Framework;
using static NGitLab.Tests.Initialize;

namespace NGitLab.Tests
{
    public class UsersTests
    {
        private readonly IUserClient _users;

        public UsersTests()
        {
            _users = Initialize.GitLabClient.Users;
        }

        [Test]
        public void GetUsers()
        {
            var users = _users.All.ToArray();
            CollectionAssert.IsNotEmpty(users);
        }

        [Test]
        public void GetUser()
        {
            var user = _users[_users.Current.Id];
            Assert.IsNotNull(user);
            Assert.That(user.Username, Is.EqualTo(_users.Current.Username));
        }

        [Test]
        public void CreateUpdateDelete()
        {
            if (!Initialize.IsAdmin)
            {
                Utils.FailInCiEnvironment("Cannot test the creation of users since the current user is not admin");
            }

            var userUpsert = GetNewUser();

            var addedUser = _users.Create(userUpsert);
            Assert.That(addedUser.Bio, Is.EqualTo(userUpsert.Bio));

            userUpsert.Bio = "Bio2";

            var updatedUser = _users.Update(addedUser.Id, userUpsert);
            Assert.That(updatedUser.Bio, Is.EqualTo(userUpsert.Bio));

            _users.Delete(addedUser.Id);

            WaitWithTimeoutUntil(() => !_users.Get(addedUser.Username).Any());

            Assert.IsFalse(_users.Get(addedUser.Username).Any());
        }

        [Test]
        public void DeactivatedAccountShouldBeAbleToActivate()
        {
            if (!Initialize.IsAdmin)
            {
                Utils.FailInCiEnvironment("Cannot test the creation of users since the current user is not admin");
            }

            var userUpsert = GetNewUser();

            var addedUser = _users.Create(userUpsert);

            _users.Deactivate(addedUser.Id);

            Assert.That(_users[addedUser.Id].State, Is.EqualTo("deactivated"));

            _users.Activate(addedUser.Id);

            Assert.That(_users[addedUser.Id].State, Is.EqualTo("active"));

            _users.Delete(addedUser.Id);

            WaitWithTimeoutUntil(() => !_users.Get(addedUser.Username).Any());
            Assert.IsFalse(_users.Get(addedUser.Username).Any());
        }

        [Test]
        public void Test_can_add_an_ssh_key_to_the_gitlab_profile()
        {
            var users = _users;
            var keys = users.CurrentUserSShKeys;

            var generatedKey = RsaSshKey.GenerateQuickest();

            var result = keys.Add(new SshKeyCreate
            {
                Key = generatedKey.PublicKey,
                Title = "test key",
            });

            // result.Key = generatedKey.PublicKey + " robot (gitlab.example.com)"
            Assert.IsNotNull(result);
            StringAssert.StartsWith(generatedKey.PublicKey, result.Key);

            var newKey = keys.All.FirstOrDefault(k => k.Id == result.Id);

            Assert.IsNotNull(newKey);
            Assert.AreEqual(result.Key, newKey.Key);

            keys.Remove(result.Id);

            Assert.IsNull(keys.All.FirstOrDefault(k => k.Id == result.Id));
        }

        [Test]
        public void CreateTokenAsAdmin_ReturnsUserToken()
        {
            if (!Initialize.IsAdmin)
            {
                Utils.FailInCiEnvironment("Cannot test the creation of users since the current user is not admin");
            }

            var tokenRequest = new UserTokenCreate
            {
                UserId = _users.Current.Id,
                Name = $"Test_Create_{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                ExpiresAt = DateTime.Now.AddDays(1),
                Scopes = new[] { "api", "read_user" },
            };

            var tokenResult = _users.CreateToken(tokenRequest);

            Assert.IsNotEmpty(tokenResult.Token);
            Assert.AreEqual(tokenRequest.Name, tokenResult.Name);
        }

        private static UserUpsert GetNewUser()
        {
            var randomNumber = Initialize.GetRandomNumber();

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
                WebsiteURL = "wp.pl",
            };
        }

        // Comes from https://github.com/meziantou/Meziantou.GitLabClient/blob/master/Meziantou.GitLabClient.Tests/Internals/RsaSshKey.cs
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
}
