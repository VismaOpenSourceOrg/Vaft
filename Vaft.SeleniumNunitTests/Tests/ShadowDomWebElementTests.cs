using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.PageObjects.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    public class ShadowDomWebElementTests:TestBase
    {
        private readonly string DemoPage = "Resources\\WebComponentsDemoPage.html";
        private WebComponentsDemoPage _demoPage;

        
        [SetUp]
        public void SetUp()
        {
           _demoPage= new WebComponentsDemoPage(Driver);
            Driver.Navigate().GoToUrl("file:///"+Path.Combine(GetExecutingAssemblyPath(), DemoPage));
        }
        
        
        [Test]
        public void VerifyPageValues()
        {
         string text1 = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
         string text2 ="Lorem Ipsum is simply dummy text of the printing and typesetting industry.Lorem Ipsum is simply dummy text of the printing and typesetting industry.Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
         
         Assert.AreEqual( "987777",_demoPage.GetShadow1InputTwoFieldValue());
         Assert.AreEqual(text1,_demoPage.GetShadow2DummyTextValue());
         Assert.AreEqual("2133546",_demoPage.GetShadow3InputFieldSub3TwoValue());
         Assert.AreEqual(text1,_demoPage.GetShadow3DummyTextValue());
         Assert.AreEqual("5",_demoPage.GetShadow22InputFieldSub2TwoValue());
         Assert.AreEqual(text2,_demoPage.GetShadow22DummyTextValue());
        }
        
        private  string GetExecutingAssemblyPath()
        {
            var assemblyFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
            var path = Path.GetDirectoryName(assemblyFile);
            return Uri.UnescapeDataString(path);
        }
    }
}