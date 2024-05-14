using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PredPrak;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject1
{    
    [TestClass]
    public class ValidationPasswordTests
    {
        [TestMethod]
        public void PassTestDigit()
        {
            string pass = "zxc";
            int actual = 0;
            int extended =  RegistrationOfJury.ValidatePassword(pass);
            Assert.AreEqual(extended, actual);
        }
        [TestMethod]
        public void PassTestLower()
        {
            string pass = "ZXC1";
            int actual = 1;
            int extended = RegistrationOfJury.ValidatePassword(pass);
            Assert.AreEqual(extended, actual);
        }
        [TestMethod]
       
        public void PassTestUpper()
        {
            string pass = "zxc123";
            int actual = 2;
            int extended = RegistrationOfJury.ValidatePassword(pass);
            Assert.AreEqual(extended, actual);
        }
        [TestMethod]
        public void PassTestSpec()
        {
            string pass = "ZXzz125445";
            int actual = 3;
            int extended = RegistrationOfJury.ValidatePassword(pass);
            Assert.AreEqual(extended, actual);
        }
        [TestMethod]
        public void PassTestKol()
        {
            string pass = "!1zZ";
            int actual = 4;
            int extended = RegistrationOfJury.ValidatePassword(pass);
            Assert.AreEqual(extended, actual);
        }


    }
}
