using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.RemotingRef.NetCore.Criteria;

public class GetRefResourceCriteria : CriteriaBase
{
    public GetRefResourceCriteria()
    {
    }
    
    public override string GetSettingKey()
    => "Dot_Member_GetRef_AllRefResourceData";

    public override bool IsValid() => true;
}