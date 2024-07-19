using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.RemotingRef.NetCore.Criteria;

public class GetLeague3rdCriteria : CriteriaBase
{
    public GetLeague3rdCriteria(long timestamp)
    {
        Timestamp = timestamp;
    }
    [SpParam(SqlParamName = "MinTstamp")]
    public long Timestamp { get; set; }

    public override string GetSettingKey()
    => "API_Ref_GetLeague3rd";

    public override bool IsValid() => true;
}