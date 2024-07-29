using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.RemotingRef.NetCore.Criteria;

public class GetTeam3rdCriteria : CriteriaBase
{
    public GetTeam3rdCriteria(long timestamp)
    {
        Timestamp = timestamp;
    }

    [SpParam(SqlParamName = "MinTstamp")]
    public long Timestamp { get; set; }

    public override string GetSettingKey()
    => "API_Ref_GetTeam3rd";

    public override bool IsValid() => true;
}