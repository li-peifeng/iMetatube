using System.ComponentModel;

namespace Jellyfin.Plugin.MetaTube.Translation;

public enum TranslationMode
{
    [Description("关闭")]
    Disabled,

    [Description("标题")]
    Title,

    [Description("摘要")]
    Summary,

    [Description("标题和摘要")]
    Both
}