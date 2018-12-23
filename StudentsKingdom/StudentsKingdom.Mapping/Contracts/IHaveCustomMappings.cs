using AutoMapper;

namespace StudentsKingdom.Mapping.Contracts
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
