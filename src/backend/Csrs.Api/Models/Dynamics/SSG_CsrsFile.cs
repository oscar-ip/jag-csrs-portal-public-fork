using Csrs.Api.Services;

namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_CsrsFile
    {
        public const string Active = "Active";
        public const string Draft = "Draft";

        public sealed class StatusCodes
        {
            public static bool Initialized { get; private set; } = false;
            private static readonly List<StatusCodes> _items = new List<StatusCodes>();

            public string Name { get; init; }
            public int Value { get; init; }


            private StatusCodes(string name, int value)
            {
                Name = name ?? throw new ArgumentNullException(nameof(name));
                Value = value;
            }

            public static StatusCodes? FromName(string name)
            {
                return _items.FirstOrDefault(_ => _.Name == name);
            }
            public static StatusCodes? FromValue(int value)
            {
                return _items.FirstOrDefault(_ => _.Value == value);
            }

            public static async Task InitializeAsync(IOptionSetRepository optionSetRepository, CancellationToken cancellationToken)
            {
                var values = await optionSetRepository.GetStatusCodesAsync(EntityLogicalName, cancellationToken);
                foreach (var value in values.Where(_ => _.Value == Active || _.Value == Draft))
                {
                    _items.Add(new StatusCodes(value.Value, value.Id));
                }

                Initialized = true;
            }
        }
    }
}
