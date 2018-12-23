using StudentsKingdom.Data.Common.Contracts.Stats;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Data.Common.Contracts
{
    public interface IStats: IHealth, IDamage, IDefence, IVitality, IStrength, IAgility, IIntellect, IEndurance
    {
    }
}
