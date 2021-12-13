using AutoFixture.Xunit2;
using Csrs.Api.Models;
using Csrs.Api.Models.Dynamics;
using Csrs.Api.Repositories;
using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace Csrs.Test.Repositories
{
    public class PartyInsertOrUpdateFieldMapperTest
    {
        [Theory]
        [AutoData]
        public void on_insert_bceid_guid_should_not_be_set_if_empty(Party expected)
        {
            expected.BCeIDGuid = Guid.Empty;

            PartyInsertOrUpdateFieldMapper sut = new();
            Dictionary<string, object?> actual = sut.GetFieldsForInsert(expected);

            actual.Should().NotBeNull();
            actual.Should().NotContainKey(SSG_CsrsParty.Attributes.ssg_bceid_guid);
            actual.Should().NotContainKey(SSG_CsrsParty.Attributes.ssg_bceid_last_update);

            AssertCommon(expected, actual);
        }

        [Theory]
        [AutoData]
        public void all_fields_should_map_to_correct_keys(Party expected)
        {
            PartyInsertOrUpdateFieldMapper sut = new();
            Dictionary<string, object?> actual = sut.GetFieldsForInsert(expected);

            actual.Should().NotBeNull();
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_bceid_guid, expected.BCeIDGuid.ToString("d"));
            actual.Should().ContainKey(SSG_CsrsParty.Attributes.ssg_bceid_last_update);

            // bceid_last_update should be a local time at the time of insertion
            actual[SSG_CsrsParty.Attributes.ssg_bceid_last_update].Should().NotBeNull();
#pragma warning disable CS8605 // Unboxing a possibly null value.
            var ssg_bceid_last_update = (DateTime)actual[SSG_CsrsParty.Attributes.ssg_bceid_last_update];
#pragma warning restore CS8605 // Unboxing a possibly null value.
            ssg_bceid_last_update.Should().BeCloseTo(DateTime.Now, 1.Seconds());
            ssg_bceid_last_update.Kind.Should().Be(DateTimeKind.Local);

            AssertCommon(expected, actual);

            // test if each of these null, it doesn't get mapped
            expected.Gender = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsParty.Attributes.ssg_gender);

            expected.Identity = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsParty.Attributes.ssg_identity);

            expected.Referral = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsParty.Attributes.ssg_referral);

            expected.Province = null;
            actual = sut.GetFieldsForInsert(expected);
            actual.Should().NotContainKey(SSG_CsrsParty.Attributes.ssg_provinceterritory);
        }

        private void AssertCommon(Party expected, Dictionary<string, object?> actual)
        {
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_firstname, expected.FirstName);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_lastname, expected.LastName);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_middlename, expected.MiddleName);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_dateofbirth, expected.DateOfBirth);

            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_gender, expected.Gender?.Id);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_identity, expected.Identity?.Id);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_referral, expected.Referral?.Id);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_provinceterritory, expected.Province?.Id);

            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_dateofbirth, expected.DateOfBirth);

            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_email, expected.Email);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_homephone, expected.HomePhone);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_workphone, expected.WorkPhone);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_cellphone, expected.CellPhone);

            // address
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_street1, expected.AddressStreet1);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_street2, expected.AddressStreet2);
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_city, expected.City);
            // province
            actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_areapostalcode, expected.PostalCode);

            //actual.Should().Contain(SSG_CsrsParty.Attributes.ssg_csrsoptoutedocuments, expected.OptOutElectronicDocuments);

            actual.Should().Contain(SSG_CsrsParty.Attributes.statecode, 0);
            actual.Should().Contain(SSG_CsrsParty.Attributes.statuscode, 1);
        }
    }
}
