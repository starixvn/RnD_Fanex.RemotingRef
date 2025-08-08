using Fanex.Data.Repository;

namespace Fanex.RemotingRef.NetCore.Criteria;

public class GetBettypeCriteria : CriteriaBase
{
    public override string GetSettingKey()
    => "Dot_Member_GetRef_Bettype";

    public override bool IsValid() => true;
}