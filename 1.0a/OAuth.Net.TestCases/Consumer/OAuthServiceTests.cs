﻿// Copyright (c) 2008 Madgex
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// OAuth.net uses the Common Service Locator interface, released under the MS-PL
// license. See "CommonServiceLocator License.txt" in the Licenses folder.
// 
// The examples and test cases use the Windsor Container from the Castle Project
// and Common Service Locator Windsor adaptor, released under the Apache License,
// Version 2.0. See "Castle Project License.txt" in the Licenses folder.
// 
// XRDS-Simple.net uses the HTMLAgility Pack. See "HTML Agility Pack License.txt"
// in the Licenses folder.
//
// Authors: Bruce Boughton, Chris Adams
// Website: http://lab.madgex.com/oauth-net/
// Email:   oauth-dot-net@madgex.com

#if DEBUG
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using OAuth.Net.Common;
using OAuth.Net.Consumer;

namespace OAuth.Net.TestCases.Consumer
{
    [TestFixture]
    public class OAuthServiceTests
    {
        [TestFixtureSetUp]
        public void SetLocatorProvider()
        {
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(
                    new WindsorContainer()));
        }

        [Test]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test methods cannot be static")]
        public void TestBuildAuthorizationUriWithoutExistingQueryString()
        {
            OAuthService service = OAuthService.Create(
                new EndPoint("http://example.com/request_token"),
                new Uri("http://example.com/authorize"),
                new EndPoint("http://example.com/access_token"),
                new MockConsumer()
                {
                    Key = "key",
                    Secret = "secret",
                    Status = ConsumerStatus.Valid
                });

            Uri authUri = service.BuildAuthorizationUrl(
                new OAuthToken(TokenType.Request, "token", null, (string)null));

            Assert.That(
                authUri.AbsoluteUri, 
                Is.EqualTo("http://example.com/authorize?oauth_token=token"));
        }

        [Test]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test methods cannot be static")]
        public void TestBuildAuthorizationUriWithExistingQueryString()
        {
            OAuthService service = OAuthService.Create(
                new EndPoint("http://example.com/request_token"),
                new Uri("http://example.com/authorize?readonly=true"),
                new EndPoint("http://example.com/access_token"),
                new MockConsumer()
                {
                    Key = "key",
                    Secret = "secret",
                    Status = ConsumerStatus.Valid
                });

            Uri authUri = service.BuildAuthorizationUrl(
                new OAuthToken(TokenType.Request, "token", null, (string)null));

            Assert.That(
                authUri.AbsoluteUri, 
                Is.EqualTo("http://example.com/authorize?readonly=true&oauth_token=token"));
        }

        [Test]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test methods cannot be static")]
        public void TestBuildAuthorizationUriWithAdditionalParametersWithoutExistingQueryString()
        {
            OAuthService service = OAuthService.Create(
                new EndPoint("http://example.com/request_token"),
                new Uri("http://example.com/authorize"),
                new EndPoint("http://example.com/access_token"),
                new MockConsumer()
                {
                    Key = "key",
                    Secret = "secret",
                    Status = ConsumerStatus.Valid
                });

            Uri authUri = service.BuildAuthorizationUrl(
                new OAuthToken(TokenType.Request, "token", null, (string)null),
                new NameValueCollection
                {
                    { "time", "60" }
                });

            Assert.That(
                authUri.AbsoluteUri,
                Is.EqualTo("http://example.com/authorize?oauth_token=token&time=60"));
        }

        [Test]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test methods cannot be static")]
        public void TestBuildAuthorizationUriWithAdditionalParametersAndExistingQueryString()
        {
            OAuthService service = OAuthService.Create(
                new EndPoint("http://example.com/request_token"),
                new Uri("http://example.com/authorize?readonly=true"),
                new EndPoint("http://example.com/access_token"),
                new MockConsumer()
                {
                    Key = "key",
                    Secret = "secret",
                    Status = ConsumerStatus.Valid
                });

            Uri authUri = service.BuildAuthorizationUrl(
                new OAuthToken(TokenType.Request, "token", null, (string)null),
                new NameValueCollection
                {
                    { "time", "60" }
                });

            Assert.That(
                authUri.AbsoluteUri,
                Is.EqualTo("http://example.com/authorize?readonly=true&oauth_token=token&time=60"));
        }
    }
}
#endif