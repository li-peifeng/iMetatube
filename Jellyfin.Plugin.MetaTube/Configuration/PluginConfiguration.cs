using Jellyfin.Plugin.MetaTube.Helpers;
using Jellyfin.Plugin.MetaTube.Translation;
#if __EMBY__
using System.ComponentModel;
using Emby.Web.GenericEdit;
using MediaBrowser.Model.Attributes;

#else
using MediaBrowser.Model.Plugins;
#endif

namespace Jellyfin.Plugin.MetaTube.Configuration;

#if __EMBY__
public class PluginConfiguration : EditableOptionsBase
{
    public override string EditorTitle => Plugin.Instance.Name;
#else
public class PluginConfiguration : BasePluginConfiguration
{
#endif

#if __EMBY__
    [DisplayName("后端服务器")]
    [Description("后端服务器的完整URL地址，推荐使用HTTPS。")]
    [Required]
#endif
    public string Server { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("令牌")]
    [Description("后端服务器的访问令牌，如果后端没有设置令牌，请留空。")]
#endif
    public string Token { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("启用合集")]
    [Description("按系列自动创建合集。")]
#endif
    public bool EnableCollections { get; set; } = false;

#if __EMBY__
    [DisplayName("启用导演")]
    [Description("将导演添加到相应的视频元数据中。")]
#endif
    public bool EnableDirectors { get; set; } = true;

#if __EMBY__
    [DisplayName("启用评级")]
    [Description("显示原始网站的社区评级。")]
#endif
    public bool EnableRatings { get; set; } = true;

#if __EMBY__
    [DisplayName("启用预告片")]
    [Description("生成strm格式的在线视频预告片。")]
#endif
    public bool EnableTrailers { get; set; } = false;

#if __EMBY__
    [DisplayName("启用真实演员的名字")]
    [Description("搜索并替换来自AVBASE的真实演员姓名。")]
#endif
    public bool EnableRealActorNames { get; set; } = false;

#if __EMBY__
    [DisplayName("启用角标")]
    [Description("在海报图中添加中文字幕角标。")]
#endif
    public bool EnableBadges { get; set; } = false;

#if __EMBY__
    [DisplayName("角标网址")]
    [Description("自定义角标网址，推荐使用PNG格式。（默认：zimu.png）")]
#endif
    public string BadgeUrl { get; set; } = "zimu.png";

#if __EMBY__
    [DisplayName("海报图比例")]
    [Description("海报图的长宽比，设置负值以使用默认值。")]
#endif
    public double PrimaryImageRatio { get; set; } = -1;

#if __EMBY__
    [DisplayName("默认图像质量")]
    [Description("JPEG图像的默认压缩质量，设置在0到100之间。（默认：90）")]
    [MinValue(0)]
    [MaxValue(100)]
    [Required]
#endif
    public int DefaultImageQuality { get; set; } = 90;

#if __EMBY__
    [DisplayName("启用电影提供商过滤器")]
    [Description("过滤和重新排序电影提供商的搜索结果。")]
#endif
    public bool EnableMovieProviderFilter { get; set; } = false;

#if __EMBY__
    [DisplayName("电影提供商过滤器")]
    [Description(
        "提供商名称不区分大小写，优先级从左到右递减，用逗号分隔。")]
#endif
    public string RawMovieProviderFilter
    {
        get => _movieProviderFilter?.Any() == true ? string.Join(',', _movieProviderFilter) : string.Empty;
        set => _movieProviderFilter = value?.Split(',').Select(s => s.Trim()).Where(s => s.Any())
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    public List<string> GetMovieProviderFilter()
    {
        return _movieProviderFilter;
    }

    private List<string> _movieProviderFilter;

#if __EMBY__
    [DisplayName("启用模板")]
#endif
    public bool EnableTemplate { get; set; } = false;

#if __EMBY__
    [DisplayName("影片名称")]
#endif
    public string NameTemplate { get; set; } = DefaultNameTemplate;

#if __EMBY__
    [DisplayName("标签模版")]
#endif
    public string TaglineTemplate { get; set; } = DefaultTaglineTemplate;

    public static string DefaultNameTemplate => "{number} {title}";

    public static string DefaultTaglineTemplate => "发行日期 {date}";

#if __EMBY__
    [DisplayName("翻译模式")]
#endif
    public TranslationMode TranslationMode { get; set; } = TranslationMode.Disabled;

#if __EMBY__
    [DisplayName("翻译引擎")]
#endif
    public TranslationEngine TranslationEngine { get; set; } = TranslationEngine.Baidu;

#if __EMBY__
    [DisplayName("百度 app id")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.Baidu)]
#endif
    public string BaiduAppId { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("百度 app key")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.Baidu)]
#endif
    public string BaiduAppKey { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("谷歌 api key")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.Google)]
#endif
    public string GoogleApiKey { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("Google api url")]
    [Description("Custom Google translate api url. (optional)")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.Google)]
#endif
    public string GoogleApiUrl { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("DeepL api key")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.DeepL)]
#endif
    public string DeepLApiKey { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("DeepL 其它 url (可选)")]
    [Description("自定义 DeepL 兼容 API URL。（可选）")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.DeepL)]
#endif
    public string DeepLApiUrl { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("OpenAI api key")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.OpenAi)]
#endif
    public string OpenAiApiKey { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("OpenAI api url")]
    [Description("Custom OpenAI-compatible api url. (optional)")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.OpenAi)]
#endif
    public string OpenAiApiUrl { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("OpenAI model")]
    [Description("Custom OpenAI-compatible api model. (optional)")]
    [VisibleCondition(nameof(TranslationEngine), ValueCondition.IsEqual, TranslationEngine.OpenAi)]
#endif
    public string OpenAiModel { get; set; } = string.Empty;

#if __EMBY__
    [DisplayName("Enable title substitution")]
#endif
    public bool EnableTitleSubstitution { get; set; } = false;

#if __EMBY__
    [DisplayName("标题替换表")]
    [Description(
        "每行一条记录，用等号隔开。将目标标题留空以删除源标题。")]
    [EditMultiline(5)]
#endif
    public string TitleRawSubstitutionTable
    {
        get => _titleSubstitutionTable?.ToString();
        set => _titleSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetTitleSubstitutionTable()
    {
        return _titleSubstitutionTable;
    }

    private SubstitutionTable _titleSubstitutionTable;

#if __EMBY__
    [DisplayName("启用演员替换")]
#endif
    public bool EnableActorSubstitution { get; set; } = false;

#if __EMBY__
    [DisplayName("演员替换表")]
    [Description(
        "每行一条记录，用等号隔开。将目标演员留空以删除源演员。")]
    [EditMultiline(5)]
#endif
    public string ActorRawSubstitutionTable
    {
        get => _actorSubstitutionTable?.ToString();
        set => _actorSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetActorSubstitutionTable()
    {
        return _actorSubstitutionTable;
    }

    private SubstitutionTable _actorSubstitutionTable;

#if __EMBY__
    [DisplayName("启用类型替换")]
#endif
    public bool EnableGenreSubstitution { get; set; } = false;

#if __EMBY__
    [DisplayName("类型替换表")]
    [Description(
        "每行一条记录，用等号隔开。将目标类型留空以删除源类型。")]
    [EditMultiline(5)]
#endif
    public string GenreRawSubstitutionTable
    {
        get => _genreSubstitutionTable?.ToString();
        set => _genreSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetGenreSubstitutionTable()
    {
        return _genreSubstitutionTable;
    }

    private SubstitutionTable _genreSubstitutionTable;
}