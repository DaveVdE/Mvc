﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.FunctionalTests
{
    public class RazorPagesTest : IClassFixture<MvcTestFixture<RazorPagesWebSite.Startup>>
    {
        public RazorPagesTest(MvcTestFixture<RazorPagesWebSite.Startup> fixture)
        {
            Client = fixture.Client;
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task HelloWorld_CanGetContent()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/HelloWorld");

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello, World!", content.Trim());
        }

        [Fact]
        public async Task HelloWorldWithRoute_CanGetContent()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/HelloWorldWithRoute/Some/Path/route");

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello, route!", content.Trim());
        }

        [Fact]
        public async Task HelloWorldWithHandler_CanGetContent()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/HelloWorldWithHandler?message=handler");

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello, handler!", content.Trim());
        }

        [Fact]
        public async Task HelloWorldWithPageModelHandler_CanGetContent()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/HelloWorldWithPageModelHandler?message=pagemodel");

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello, pagemodel!", content.Trim());
        }

        [Fact]
        public async Task TempData_SetTempDataInPage_CanReadValue()
        {
            // Arrange 1
            var url = "http://localhost/TempData/SetTempDataOnPageAndRedirect?message=Hi1";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            // Act 1
            var response = await Client.SendAsync(request);
            
            // Assert 1
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            // Arrange 2
            request = new HttpRequestMessage(HttpMethod.Get, response.Headers.Location);
            request.Headers.Add("Cookie", GetCookie(response));

            // Act2
            response = await Client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hi1", content.Trim());
        }

        [Fact]
        public async Task TempData_SetTempDataInPageModel_CanReadValue()
        {
            // Arrange 1
            var url = "http://localhost/TempData/SetTempDataOnPageModelAndRedirect?message=Hi2";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Act 1
            var response = await Client.SendAsync(request);

            // Assert 1
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            // Arrange 2
            request = new HttpRequestMessage(HttpMethod.Get, response.Headers.Location);
            request.Headers.Add("Cookie", GetCookie(response));

            // Act2
            response = await Client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hi2", content.Trim());
        }

        private static string GetCookie(HttpResponseMessage response)
        {
            var setCookie = response.Headers.GetValues("Set-Cookie").ToArray();
            return setCookie[0].Split(';').First();
        }

        public class CookieMetadata
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }
    }
}
