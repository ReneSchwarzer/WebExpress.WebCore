using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting group for testing purposes.
    /// </summary>
    [WebIcon<TestIconShild>()]
    [Name("SettingGroup B")]
    [Description("Description of group b.")]
    [SettingCategory<TestSettingCategoryA>()]
    [SettingSection(SettingSection.Primary)]
    public sealed class TestSettingGroupB : ISettingGroup
    {
    }
}
