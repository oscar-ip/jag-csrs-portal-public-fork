namespace Csrs.Api.Models
{
    public class PickupLists
    {
        private static List<LookupValue> _genders = new List<LookupValue>();
        private static List<LookupValue> _provinces = new List<LookupValue>();
        private static List<LookupValue> _identities = new List<LookupValue>();
        private static List<LookupValue> _referals = new List<LookupValue>();
        private static List<LookupValue> _preferableContactMethods = new List<LookupValue>();
        private static List<LookupValue> _partyEnrolled = new List<LookupValue>();
        private static List<LookupValue> _section7Expenses = new List<LookupValue>();
        private static List<LookupValue> _courtFileTypes = new List<LookupValue>();
        private static List<LookupValue> _childADependents = new List<LookupValue>();

        public static List<LookupValue> GetGenders()
        {
            _genders.Add(new LookupValue { Id = 867670000, Value = "Male" });
            _genders.Add(new LookupValue { Id = 867670001, Value = "Female" });
            _genders.Add(new LookupValue { Id = 867670002, Value = "Gender-Diverse" });
            _genders.Add(new LookupValue { Id = 867670003, Value = "Unknown" });
            return _genders;
        }

        public static int GetGenders(string value)
        {
            int id = 0;
            if (_genders.Count == 0) GetGenders();
            LookupValue item = _genders.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }

        public static List<LookupValue> GetProvinces()
        {
            _provinces.Add(new LookupValue { Id = 867670000, Value = "Alberta" });
            _provinces.Add(new LookupValue { Id = 867670001, Value = "British Columbia" });
            _provinces.Add(new LookupValue { Id = 867670002, Value = "Manitoba" });
            _provinces.Add(new LookupValue { Id = 867670003, Value = "New Brunswick" });
            _provinces.Add(new LookupValue { Id = 867670004, Value = "Newfoundland and Labrador" });
            _provinces.Add(new LookupValue { Id = 867670005, Value = "Nova Scotia" });
            _provinces.Add(new LookupValue { Id = 867670006, Value = "Northwest Territories" });
            _provinces.Add(new LookupValue { Id = 867670007, Value = "Nunavut" });
            _provinces.Add(new LookupValue { Id = 867670008, Value = "Ontario" });
            _provinces.Add(new LookupValue { Id = 867670009, Value = "Prince Edward Island" });
            _provinces.Add(new LookupValue { Id = 867670010, Value = "Quebec" });
            _provinces.Add(new LookupValue { Id = 867670011, Value = "Saskatchewan" });
            _provinces.Add(new LookupValue { Id = 867670012, Value = "Yukon" });
            return _provinces;
        }

        public static int GetProvinces(string value)
        {
            int id = 0;
            if (_provinces.Count == 0) GetProvinces();
            LookupValue item = _provinces.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }


        public static List<LookupValue> GetIdentities()
        {
            _identities.Add(new LookupValue { Id = 867670000, Value = "Indigenous (e.g. Inuit, First Nations, Métis)" });
            _identities.Add(new LookupValue { Id = 867670001, Value = "White (e.g. German, Irish, English, Italian, Polish, French Canadian)" });
            _identities.Add(new LookupValue { Id = 867670002, Value = "Hispanic or Latin(e.g.Mexican, Puerto Rican, Cuban, Salvadoran, Dominican)" });
            _identities.Add(new LookupValue { Id = 867670003, Value = "Black or Caribbean (e.g. Black, Jamaican, Haitian, Nigerian, Somalian)" });
            _identities.Add(new LookupValue { Id = 867670004, Value = "Asian (e.g. Chinese, Filipino, Asian Indian, Korean, Japanese)" });
            _identities.Add(new LookupValue { Id = 867670005, Value = "Middle Eastern or North African (e.g. Lebanese, Egyptian, Syrian, Moroccan, Algerian)" });
            _identities.Add(new LookupValue { Id = 867670006, Value = "Pacific Islander (e.g. Samoan, Native Hawaiian, Tongan)" });
            _identities.Add(new LookupValue { Id = 867670007, Value = "Prefer not to answer" });
            _identities.Add(new LookupValue { Id = 867670008, Value = "Other" });
            return _identities;
        }

        public static int GetIdentities(string value)
        {
            int id = 0;
            if (_identities.Count == 0) GetIdentities();
            LookupValue item = _identities.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }

        public static List<LookupValue> GetReferals()
        {
            _referals.Add(new LookupValue { Id = 867670000, Value = "Family Maintenance Enforcement Program (FMEP)" });
            _referals.Add(new LookupValue { Id = 867670001, Value = "Family Justice Centre or Justice Access Centre (including a Family Justice Counsellor)" });
            _referals.Add(new LookupValue { Id = 867670002, Value = "Former spouse/partner" });
            _referals.Add(new LookupValue { Id = 867670003, Value = "Child Support Recalculation Service (CSRS)" });
            _referals.Add(new LookupValue { Id = 867670004, Value = "Courthouse" });
            _referals.Add(new LookupValue { Id = 867670005, Value = "Legal community (ex. lawyer, family advocate)" });
            _referals.Add(new LookupValue { Id = 867670006, Value = "Community resource" });
            _referals.Add(new LookupValue { Id = 867670007, Value = "A friend or family member" });
            _referals.Add(new LookupValue { Id = 867670008, Value = "Internet" });
            _referals.Add(new LookupValue { Id = 867670009, Value = "Other" });
            return _referals;
        }
        public static int GetReferals(string value)
        {
            int id = 0;
            if (_referals.Count == 0) GetReferals();
            LookupValue item = _referals.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }
        public static List<LookupValue> GetPreferableContactMethods()
        {
            _preferableContactMethods.Add(new LookupValue { Id = 867670000, Value = "Email" });
            _preferableContactMethods.Add(new LookupValue { Id = 867670001, Value = "Paper Mail" });

            return _preferableContactMethods;
        }

        public static int GetPreferableContactMethods(string value)
        {
            int id = 0;
            if (_preferableContactMethods.Count == 0) GetPreferableContactMethods();
            LookupValue item = _preferableContactMethods.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }


        public static List<LookupValue> GetPartyEnrolled()
        {
            _partyEnrolled.Add(new LookupValue { Id = 867670000, Value = "Payor" });
            _partyEnrolled.Add(new LookupValue { Id = 867670001, Value = "Recipient" });
            return _partyEnrolled;
        }
        public static int GetPartyEnrolled(string value)
        {
            int id = 0;
            if (_partyEnrolled.Count == 0) GetPartyEnrolled();
            LookupValue item = _partyEnrolled.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }

        public static List<LookupValue> GetSection7Expenses()
        {
            _section7Expenses.Add(new LookupValue { Id = 867670000, Value = "Yes" });
            _section7Expenses.Add(new LookupValue { Id = 867670001, Value = "No" });
            _section7Expenses.Add(new LookupValue { Id = 867670002, Value = "I do not know" });
            return _section7Expenses;
        }
        public static int GetSection7Expenses(string value)
        {
            int id = 0;
            if (_section7Expenses.Count == 0) GetSection7Expenses();
            LookupValue item = _section7Expenses.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }

        public static List<LookupValue> GetCourtFileTypes()
        {
            _courtFileTypes.Add(new LookupValue { Id = 867670000, Value = "Court Order" });
            _courtFileTypes.Add(new LookupValue { Id = 867670001, Value = "Written Agreement" });
            return _courtFileTypes;
        }

        public static int GetCourtFileTypes(string value)
        {
            int id = 0;
            if (_courtFileTypes.Count == 0) GetCourtFileTypes();
            LookupValue item = _courtFileTypes.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }

        public static List<LookupValue> GetChildADependents()
        {
            _childADependents.Add(new LookupValue { Id = 867670000, Value = "Yes" });
            _childADependents.Add(new LookupValue { Id = 867670001, Value = "No" });
            _childADependents.Add(new LookupValue { Id = 867670002, Value = "I do not know" });
            return _courtFileTypes;
        }

        public static int GetChildADependents(string value)
        {
            int id = 0;
            if (_childADependents.Count == 0) GetChildADependents();
            LookupValue item = _childADependents.Find(x => x.Value.Equals(value));
            if (item is not null)
            {
                id = item.Id;
            }
            return id;
        }
    }
}

