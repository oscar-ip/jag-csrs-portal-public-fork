using AutoFixture.Xunit2;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace Csrs.Test.Repositories
{
    public class ChildInsertOrUpdateFieldMapperTest
    {
        [Theory]
        [AutoData]
        public void on_insert_required_fields_should_be_map(Child expected)
        {
            var sut = new ChildInsertOrUpdateFieldMapper();

            Dictionary<string, object>? actual = sut.GetFieldsForInsert(expected);

            actual.Should().NotBeNull();
#pragma warning disable CS8604 // Possible null reference argument.
            actual.Should().Contain(SSG_CsrsChild.Attributes.ssg_firstname, expected.FirstName);
            actual.Should().Contain(SSG_CsrsChild.Attributes.ssg_lastname, expected.LastName);
            actual.Should().Contain(SSG_CsrsChild.Attributes.ssg_middlename, expected.MiddleName);
            actual.Should().Contain(SSG_CsrsChild.Attributes.ssg_dateofbirth, expected.DateOfBirth);
#pragma warning restore CS8604 // Possible null reference argument.

            expected.FirstName = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsChild.Attributes.ssg_firstname);

            expected.LastName = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsChild.Attributes.ssg_lastname);

            expected.MiddleName = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsChild.Attributes.ssg_middlename);

            expected.DateOfBirth = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsChild.Attributes.ssg_dateofbirth);
        }
    }
}
