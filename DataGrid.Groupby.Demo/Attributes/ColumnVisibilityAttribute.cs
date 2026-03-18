using System.Windows;


namespace DataGrid.Groupby.Demo.Attributes
{
    /// <summary>
    /// 业务场景特性：标记属性在指定场景下的可见性和默认值
    /// </summary>
    /// <remarks>
    /// 初始化业务场景特性
    /// </remarks>
    /// <param name="scenario">场景名称</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ColumnVisibilityAttribute(string scenario) : Attribute
    {
        /// <summary>
        /// 场景名称（如"Position"、"Order"）
        /// </summary>
        public string Scenario { get; } = scenario ?? throw new ArgumentNullException(nameof(scenario));

        /// <summary>
        /// 是否可见
        /// </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;
    }

}