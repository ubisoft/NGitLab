using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using NGitLab.Models;
using NUnit.Framework;

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
                Assert.Inconclusive("Cannot test the creation of users since the current user is not admin");
            }

            var userUpsert = new UserUpsert
            {
                Email = "test@test.pl",
                Bio = "bio",
                CanCreateGroup = true,
                IsAdmin = true,
                Linkedin = null,
                Name = "sadfasdf",
                Password = "!@#$QWDRQW@",
                ProjectsLimit = 1000,
                Provider = "provider",
                Skype = "skype",
                Twitter = "twitter",
                Username = "username",
                WebsiteURL = "wp.pl"
            };

            var addedUser = _users.Create(userUpsert);
            Assert.That(addedUser.Bio, Is.EqualTo(userUpsert.Bio));

            userUpsert.Bio = "Bio2";
            userUpsert.Email = "test@test.pl";

            var updatedUser = _users.Update(addedUser.Id, userUpsert);
            Assert.That(updatedUser.Bio, Is.EqualTo(userUpsert.Bio));

            _users.Delete(addedUser.Id);

            TestCurrent(userUpsert);
        }

        public void TestCurrent(UserUpsert user)
        {
            var client = new GitLabClient(Initialize.GitLabHost, user.Username, user.Password).Users;

            var session = client.Current;
            Assert.That(session, Is.Not.Null);
            Assert.That(session.CreatedAt.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(session.Email, Is.EqualTo(user.Email));
            Assert.That(session.Name, Is.EqualTo(user.Name));
            Assert.That(session.PrivateToken, Is.Not.Null);
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

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Key, generatedKey.PublicKey);

            var newKey = keys.All.FirstOrDefault(k => k.Id == result.Id);

            Assert.IsNotNull(newKey);
            Assert.AreEqual(newKey.Key, generatedKey.PublicKey);

            keys.Remove(result.Id);

            Assert.IsNull(keys.All.FirstOrDefault(k => k.Id == result.Id));
        }

        [Test]
        public void CreateTokenAsAdmin_ReturnsUserToken()
        {
            if (!Initialize.IsAdmin)
            {
                Assert.Inconclusive("Cannot test the creation of users since the current user is not admin");
            }

            var tokenRequest = new UserTokenCreate
            {
                UserId = _users.Current.Id,
                Name = $"Test_Create_{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                ExpiresAt = DateTime.Now.AddDays(1),
                Scopes = new[] { "api", "read_user" }
            };

            var tokenResult = _users.CreateToken(tokenRequest);

            Assert.IsNotEmpty(tokenResult.Token);
            Assert.AreEqual(tokenRequest.Name, tokenResult.Name);
        }

        //Comes from https://github.com/meziantou/Meziantou.GitLabClient/blob/master/Meziantou.GitLabClient.Tests/Internals/RsaSshKey.cs
        private class RsaSshKey
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
                using (var rsa = RSA.Create())
                {
                    var size = rsa.LegalKeySizes.Min();
                    rsa.KeySize = size.MinSize;

                    return Generate(rsa);
                }
            }

            public static RsaSshKey Generate(int keyLength)
            {
                using (var rsa = RSA.Create())
                {
                    rsa.KeySize = keyLength;
                    return Generate(rsa);
                }
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
