using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Csrs.Api.Repositories;
using Csrs.Interfaces.Dynamics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Csrs.Interfaces.Dynamics.Models;
using System;
using System.Globalization;
//using Microsoft.OData;
using PickLists = Csrs.Api.Models.PickupLists;

namespace Csrs.TntegrationTest
{
    public class GetPartyTest : DynamicsClientTestBase
    {
        [DebugOnlyFact]
        public async Task get_party()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"ssg_bceid_userid ne null";
            //string filter = $"ssg_csrspartyid eq 'fc7702c0-0d89-ec11-b831-00505683fbf4'";
            List<string> expand = new List<string> { "createdby" };
            var actual = await dynamicsClient.Ssgcsrsparties.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
        }

        [DebugOnlyFact]
        public async Task get_party_by_bceid()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();
            //var actual = await dynamicsClient.GetPartyByBCeIdAsync("f418274f-b10f-ea11-b814-00505683fbf4", cancellationToken: CancellationToken.None);
            var actual = await dynamicsClient.GetPartyByBCeIdAsync("5beb7384-eb6d-4fd4-8918-c3bb1a5c", cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);
        }

        [DebugOnlyFact]
        public async Task update_party_by_dateofbirh()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"ssg_bceid_guid eq '5beb7384-eb6d-4fd4-8918-c3bb1a5c'";
            List<string> expand = new List<string> { "createdby" };
            var actual = await dynamicsClient.Ssgcsrsparties.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            //Assert.NotNull(actual);

            //actual.Value[0].SsgDateofbirth = new System.DateTimeOffset(new System.DateTime(2022, 02, 04), new System.TimeSpan(0,0,0));
            string ssgCsrspartyid = actual.Value[0].SsgCsrspartyid;
            Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrsparty party = actual.Value[0];
            await dynamicsClient.Ssgcsrsparties.UpdateAsync(ssgCsrspartyid, body: party, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);


            actual = await dynamicsClient.Ssgcsrsparties.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            Assert.NotNull(actual);

        }

        [DebugOnlyFact]
        public async Task create_child()
        {
            IDynamicsClient _dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string userId = "WWHITE";
            string filter = $"ssg_bceid_userid eq '{userId}'";

            List<string> expand = new List<string>
            {
                /*"createdby",*/
                "ssg_csrsparty_ssg_csrschild_ChildsFather"/*,
                "ssg_csrsparty_ssg_csrschild_Mother",
                "ssg_csrsparty_ssg_csrsfile_Recipient",
                "ssg_csrsparty_ssg_csrsfile_Payor",
                "ssg_csrsparty_ssg_csrsarchivedpartycontact_PartyName",
                "ssg_csrsparty_ssg_csrsrecalculation_Payor",
                "ssg_csrsparty_SharePointDocumentLocations",
                "ssg_ssg_csrsparty_ssg_csrsportalaudithistory_Party",
                "ssg_ssg_csrsparty_ssg_csrsfeedback_CSRSParty",
                "ssg_DuplicatedParty",
                "ssg_csrsparty_ssg_csrsparty_MasterPartyRecord"*/
            };

            MicrosoftDynamicsCRMssgCsrspartyCollection dads = null;
            try
            {
                dads = await _dynamicsClient.Ssgcsrsparties.GetAsync(top: 10, filter: filter, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }

            Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrsparty? father = null;
            if (dads is not null && dads.Value is not null && dads.Value.Count > 0)
            {
                father = dads.Value[0];
                if (father.SsgCsrspartySsgCsrschildChildsFather is not null &&
                    father.SsgCsrspartySsgCsrschildChildsFather.Count > 0)
                {
                }
            }

            MicrosoftDynamicsCRMssgCsrschild csrsChild = new MicrosoftDynamicsCRMssgCsrschild
            {
                Statecode = 0,
                Statuscode = 1,
                SsgFirstname = "Alex",
                SsgMiddlename = "David",
                SsgLastname = "Kiselev",
                SsgDateofbirth = DateTimeOffset.Parse("2008-02-02"),
                SsgChildisadependent = (int)ChildIsDependent.Yes,
                SsgChildsFatherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", father?.SsgCsrspartyid), 
                //SsgChildsFather = //fatherExt,
                //SsgChildsMother = fatherExt
                //   new Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = fatherId },
                //SsgChildsFather = father
                //SsgFileNumber = ssgFileNumber;
                //_ssgChildsfatherValue = fatherId,
                //_ssgChildsmotherValue = fatherId

                //ssgCsrspartySsgCsrschildChildsFather = father,
                //SsgChildsFather = 
                //    new Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = father?.SsgCsrspartyid }
                //SsgChildsFather = father
                //    new Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrsparty { SsgCsrspartyid = father?.SsgCsrspartyid }
            };

            //csrsChild._ssgFilenumberValue = csrsFile.SsgCsrsfileid;
            try
            {
                csrsChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(body: csrsChild, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }

            //string childid = csrsChild.SsgCsrschildid;
            ///*string*/
            //filter = $"ssg_csrschildid eq '{childid}'";
            ////List<string> expand = new List<string> { "createdby" };


            //if (father.SsgCsrspartySsgCsrschildChildsFather is null ||
            //        father.SsgCsrspartySsgCsrschildChildsFather.Count == 0)
            //{
            //    father.SsgCsrspartySsgCsrschildChildsFather = new List<MicrosoftDynamicsCRMssgCsrschild>();


            //}

            //father.SsgCsrspartySsgCsrschildChildsFather.Add(csrsChild);

            //try
            //{
            //    await _dynamicsClient.Ssgcsrsparties.UpdateAsync(fatherId, body: father, cancellationToken: CancellationToken.None);
            //}
            //catch (Exception ex)
            //{
            //}


            //try
            //{
            //    csrsChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(body: csrsChild, cancellationToken: CancellationToken.None);
            //}
            //catch (Exception ex)
            //{
            //}




            try
            {
                var getChild = await _dynamicsClient.Ssgcsrschildren.GetAsync(top: 5, filter: filter, expand: expand, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }
            //Assert.NotNull(getChild);

        }

        [DebugOnlyFact]
        public async Task get_ssg_csrsparty_ssg_csrschild_Mother()
        {
            IDynamicsClient dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string filter = $"count(ssg_csrsparty_ssg_csrsfile_Recipient) qe 0";
            List<string> expand = new List<string> { "createdby" };
            /*
            List<string> expand = new List<string>
            {
                "ssg_csrsparty_ssg_csrsfile_Recipient",
                "ssg_csrsparty_ssg_csrsfile_Payor"
            };*/

            Interfaces.Dynamics.Models.MicrosoftDynamicsCRMssgCsrspartyCollection actual = null;
            try
            {
                actual = await dynamicsClient.Ssgcsrsparties.GetAsync(filter:filter, expand: expand, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }

           

            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Value);
        }


        [DebugOnlyFact]
        public async Task submit_application()
        {
            IDynamicsClient _dynamicsClient = _serviceProvider.GetRequiredService<IDynamicsClient>();

            string userId = "26336072-cba4-4b6e-871b-5355d27df9b3";
            //-- main party ---
            MicrosoftDynamicsCRMssgCsrsparty dynamicsParty = new MicrosoftDynamicsCRMssgCsrsparty
            {
                Statecode = 0,
                //SsgStagingfilenumber = ssgStagingfilenumber;
                //SsgIncomeassistance = ssgIncomeassistance;
                SsgReferral = 867670000,
                SsgCellphone = "403-7002156",//party.CellPhone,
                SsgFirstname = "Serrr",//party.FirstName,
                SsgDateofbirth = new DateTimeOffset(new DateTime(1976,1,25),TimeSpan.Zero),
                SsgLastname = "Kiiiiii",//party.LastName,
                //SsgReferencenumber = ssgReferencenumber,
                SsgPartygender = 867670000,
                //SsgBceidUserid = "26336072-cba4-4b6e-871b-5355d27df9b3",//ssgBceidUserid,
                SsgPreferredcontactmethod = 867670000,
                SsgCity = "Calgary",//party.City,
                SsgWorkphone = "403-1234567",//party.WorkPhone,
                SsgProvinceterritory = 867670000,
                SsgMiddlename = "Michael",//party.MiddleName,
                //SsgFullname = party.LastName + ", " + party.FirstName,
                SsgGender = 867670000,
                SsgHomephone = "403-456789",//party.HomePhone,
                Statuscode = 1,
                SsgStreet2 = String.Empty,
                //SsgStagingid = ssgStagingid,
                SsgAreapostalcode = "T3L 1J8",//party.PostalCode,
                SsgEmail = "sk@telus.net",//party.Email,
                //SsgBceidLastUpdate = ssgBceidLastUpdate,
                SsgPreferredname = "Ssss",//party.PreferredName,
                //SsgPortalaccess = ssgPortalaccess,
                SsgIdentity = 867670000,
                //SsgBceidDisplayname = (party.FirstName[0] + party.LastName).ToUpper(),
                //SsgIdentityotherdetails = ssgIdentityotherdetails,
                SsgStreet1 = "103 Scenic Public NW"//party.AddressStreet1
            };
            dynamicsParty.SsgBceidGuid = userId;

            try
            { 
                dynamicsParty = await _dynamicsClient.Ssgcsrsparties.CreateAsync(body: dynamicsParty, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }
            string partyId = dynamicsParty.SsgCsrspartyid;

            //-- other party --
            MicrosoftDynamicsCRMssgCsrsparty otherParty = new MicrosoftDynamicsCRMssgCsrsparty
            {
                Statecode = 0,
                SsgReferral = 867670000,
                SsgCellphone = "500-71111",
                SsgFirstname = "Tan",
                SsgDateofbirth = DateTimeOffset.Parse("1950-03-17"),
                SsgLastname = "Kha",
                SsgPartygender = 867670001,
                SsgPreferredcontactmethod = 867670000,
                SsgCity = "Calgary",
                SsgWorkphone = "403-1234567",
                SsgProvinceterritory = 867670000,
                SsgMiddlename = "Ana",
                SsgGender = 867670001,
                SsgHomephone = "403-3381907",
                Statuscode = 1,
                SsgStreet2 = String.Empty,
                SsgAreapostalcode = "T3L 1J8",
                SsgEmail = "tk@gmail.com",
                SsgPreferredname = "Ta",
                SsgIdentity = 867670000,
                SsgStreet1 = "123 Scenic School NW"
            };

            try
            {
                otherParty = await _dynamicsClient.Ssgcsrsparties.CreateAsync(body: otherParty, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }
            string otherPartyId = otherParty.SsgCsrspartyid;

            //-- file 

            MicrosoftDynamicsCRMssgCsrsfile csrsFile = new MicrosoftDynamicsCRMssgCsrsfile();
            csrsFile.SsgCourtfiletype = PickLists.GetCourtFileTypes("Court Order");
            csrsFile.SsgAct = PickLists.GetCourtFileTypes("Court Order");
            csrsFile.SsgFmepfileactive = true;
            csrsFile.SsgSafetyalert = true;
            csrsFile.SsgSafetyalertpayor = true;
            csrsFile.SsgSafetyconcerndescription = "Safety Concern description";
            csrsFile.SsgSharedparenting = true;
            csrsFile.SsgSplitparentingarrangement = true;

            csrsFile.SsgSection7expenses = 867670000;
            //csrsFile.SsgSection7payorsamount = 1000.00m;
            //csrsFile.SsgSection7recipientsamount = 400.00m;
            //csrsFile.SsgSection7payorsproportion = 20;

            csrsFile.SsgRegistrationdate = DateTimeOffset.Parse("2008-02-02");
            csrsFile.SsgDateoforderorwa = DateTimeOffset.Parse("2008-02-02");

            csrsFile.SsgStyleofcauseapplicant = "Applicant";
            csrsFile.SsgStyleofcauserespondent = "Respondent";
            csrsFile.SsgIncomeonorder = 20000.00m;
            csrsFile.SsgRecalculationorderedbythecourt = true;

            csrsFile.SsgBCCourtLevel = new MicrosoftDynamicsCRMssgCsrsbccourtlevel { SsgCourtlevellabel = "Provincial" };
            csrsFile.SsgBCCourtLocationODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", "99e9c19d-0af5-e911-b811-00505683fbf4");

            csrsFile.SsgPartyenrolled = PickLists.GetPartyEnrolled("Recipient");
            csrsFile.SsgCourtfilenumber = "123-ABC";
            csrsFile.SsgRecipientODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", dynamicsParty.SsgCsrspartyid);
            csrsFile.SsgPayorODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", otherParty.SsgCsrspartyid);

            try
            {
                csrsFile = await _dynamicsClient.Ssgcsrsfiles.CreateAsync(body: csrsFile, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }

            // --- chidren part

            MicrosoftDynamicsCRMssgCsrschild alexChild = new MicrosoftDynamicsCRMssgCsrschild
            {
                SsgFirstname = "Aaaa",
                SsgMiddlename = "Dddd",
                SsgLastname = "Kkkk",
                SsgDateofbirth = DateTimeOffset.Parse("2010-01-01"),
                SsgChildisadependent = 867670000
            };

            alexChild.SsgChildsFatherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", dynamicsParty.SsgCsrspartyid);
            alexChild.SsgChildsMotherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", otherParty.SsgCsrspartyid);
            alexChild.SsgFileNumberODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", csrsFile.SsgCsrsfileid);

            try
            {

                alexChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(body: alexChild, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }

            MicrosoftDynamicsCRMssgCsrschild katChild = new MicrosoftDynamicsCRMssgCsrschild
            {
                SsgFirstname = "Katt",
                SsgMiddlename = "Kaaa",
                SsgLastname = "Kgggg",
                SsgDateofbirth = DateTimeOffset.Parse("2002-11-03"),
                SsgChildisadependent = 867670001
            };

            katChild.SsgChildsFatherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", dynamicsParty.SsgCsrspartyid);
            katChild.SsgChildsMotherODataBind = _dynamicsClient.GetEntityURI("ssg_csrsparties", otherParty.SsgCsrspartyid);
            katChild.SsgFileNumberODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", csrsFile.SsgCsrsfileid);

            try
            {
                katChild = await _dynamicsClient.Ssgcsrschildren.CreateAsync(body: katChild, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
            }


        }

    }
}
