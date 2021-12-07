namespace Csrs.Api.Models.Dynamics
{
    public class StatusCode<TEnum> where TEnum : struct
    {
        public StatusCode(int id, string name, TEnum value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public int Id { get; }
        public string Name { get; }
        public TEnum Value { get; }
    }
}
