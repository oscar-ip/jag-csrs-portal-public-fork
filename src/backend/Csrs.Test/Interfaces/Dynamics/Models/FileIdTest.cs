using Csrs.Interfaces.Dynamics.Models;
using System;
using Xunit;


namespace Csrs.Test.Interfaces.Dynamics.Models
{
    public class FileIdTest
    {
        [Fact]
        public void should_format_with_d_format_specifier_using_new()
        {
            FileId sut = FileId.New();

            var expected = sut.Value.ToString("d");
            var actual = sut.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void should_format_with_d_format_specifier_using_guid()
        {
            var id = Guid.NewGuid();
            FileId sut = new FileId(id);

            var expected = id.ToString("d");
            var actual = sut.ToString();

            Assert.Equal(expected, actual);
        }
    }
}
