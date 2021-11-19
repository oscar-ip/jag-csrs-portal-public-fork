using AutoFixture;
using AutoMapper;
using Xunit;

namespace Csrs.Test.Models.Mapping
{

    public abstract class MappingProfileTest<TProfile, TSource, TDest> where TProfile : Profile, new()
    {
        protected readonly Fixture _fixture = new Fixture();

        protected MapperConfiguration CreateMapperConfiguration()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile<TProfile>();
            });
        }

        protected IMapper CreateMapper()
        {
            var mappingConfig = CreateMapperConfiguration();
            return mappingConfig.CreateMapper();
        }

        [Fact(Skip = "Mapping Configuration is not complete yet")]
        public void ProfileConfigurationIsValid()
        {
            var mappingConfig = CreateMapperConfiguration();
            mappingConfig.AssertConfigurationIsValid();
        }

        [Fact]
        public void MappingDoesNotThrow()
        {
            IMapper mapper = CreateMapper();

            var expected = _fixture.Create<TSource>();
            var actual = mapper.Map<TDest>(expected);
        }
    }
}
