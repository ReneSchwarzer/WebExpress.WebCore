using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting category for testing purposes.
    /// </summary>
    [WebIcon<TestIconProfile>]
    [Name("SettingCategory B")]
    [Description("Description of category b.")]
    [SettingSection(SettingSection.Primary)]
    public sealed class TestSettingCategoryB : ISettingCategory
    {
    }
}
