using Fanex.Data.Repository;

namespace Fanex.RemotingRef.NetCore.Criteria;

public class GetSportCriteria : CriteriaBase
{
    public override string GetSettingKey()
    => "Dot_Member_GetRef_Sports";

    public override bool IsValid() => true;
}