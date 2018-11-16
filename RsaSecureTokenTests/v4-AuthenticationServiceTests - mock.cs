using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RsaSecureToken;

namespace RsaSecureToken.Tests
{
    [TestClass()]
    public class AuthenticationServiceTests
    {
        [TestMethod()]
        public void IsValidTest_只有驗證Authentication合法或非法()
        {
            //arrange
            IProfile profile = Substitute.For<IProfile>();
            profile.GetPassword("Joey").Returns("91");

            IToken token = Substitute.For<IToken>();
            token.GetRandom("Joey").Returns("abc");

            ILog log = Substitute.For<ILog>();
            AuthenticationService target = new AuthenticationService(profile, token, log);
            string account = "Joey";
            string password = "wrong password";
            // 正確的 password 應為 "91abc"

            //act
            bool actual;
            actual = target.IsValid(account, password);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidTest_如何驗證當非法登入時有正確紀錄log()
        {
	        //arrange
	        IProfile profile = Substitute.For<IProfile>();
	        profile.GetPassword("Joey").Returns("91");

	        IToken token = Substitute.For<IToken>();
	        token.GetRandom("Joey").Returns("abc");

	        ILog log = Substitute.For<ILog>();
	        AuthenticationService target = new AuthenticationService(profile, token, log);
	        string account = "Joey";
	        string password = "wrong password";
	        // 正確的 password 應為 "91abc"

	        //act
	        bool actual;
	        target.IsValid(account, password);
			
			log.Received().Save(Arg.Is<string>( x=>x.Contains("Joey") && x.Contains("login failed")));
        }        
    }
}