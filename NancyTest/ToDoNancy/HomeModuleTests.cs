using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoNancyTests
{
    using Nancy;
    using Nancy.Testing;
    using Xunit;
    public class HomeModuleTests
    {
        [Fact]
        public void Should_answer_200_on_root_path()
        {
            // sut = system under test
            var sut = new Browser(new DefaultNancyBootstrapper());
            var actual = sut.Get("/");
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }
    }
}
