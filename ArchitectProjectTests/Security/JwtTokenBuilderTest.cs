using Xunit;
using ArchitectProject.Security;

namespace ArchitectProjectTests.Security
{
    public class JwtTokenBuilderTest
    {
        [Fact]
        public void TestBuildStaticToken()
        {
            JwtSecurityKey.UseDynamicToken = false;
            JwtTokenBuilder jwtTokenBuilder = new JwtTokenBuilder();
            JwtToken token = jwtTokenBuilder
                                .AddSecurityKey(JwtSecurityKey.Create("MyTestKeyMustBeAtLeast16CharactesInLength."))
                                .AddSubject("TestUser")
                                .AddIssuer("Issuer")
                                .AddAudience("Audience")
                                .Build();
            Assert.Equal("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJUZXN0VXNlciIsImlzcyI6Iklzc3VlciIsImF1ZCI6IkF1ZGllbmNlIn0._mbabeQ1f2kzE1GS0boKUhemlfWhzdv7cruib0L3er0",
                token.Value);
        }
    }
}
