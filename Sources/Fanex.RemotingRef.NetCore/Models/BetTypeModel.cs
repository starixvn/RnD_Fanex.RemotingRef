using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Models;
public class BetTypeModel
{
    public int TypeId { get; set; }

    [Language(LanguageEnum.En)]
    public string TypeName { get; set; }

    [Language(LanguageEnum.Th)]
    public string TYpeNameTh { get; set; }

    [Language(LanguageEnum.Vn)]
    public string TypeNameVn { get; set; }

    [Language(LanguageEnum.Cn)]
    public string TypeNameCn { get; set; }

    [Language(LanguageEnum.Ch)]
    public string TypeNameC { get; set; }

    [Language(LanguageEnum.Sc2)]
    public string TypeNameSc2 => TypeNameCn;

    [Language(LanguageEnum.Id)]
    public string TypeNameID { get; set; }

    [Language(LanguageEnum.Jp)]
    public string TypeNameJ { get; set; }

    [Language(LanguageEnum.Ko)]
    public string TypeNameKo { get; set; }

    public long TimeSpan { get; set; }
}