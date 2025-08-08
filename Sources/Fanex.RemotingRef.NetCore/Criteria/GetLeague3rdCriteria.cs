namespace Fanex.RemotingRef.NetCore.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    public class GetLeague3rdCriteria(long timestamp = 0) : CriteriaBase
    {
        [SpParam(SqlParamName = "MinTstamp")]
        public long Timestamp { get; set; }

        public override string GetSettingKey()
        => "API_Ref_GetLeague3rd";

        public override bool IsValid() => true;
    }
}