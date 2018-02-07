using System.Web.Optimization;

namespace Survey.Web.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			#region Scripts Layout Main Page

			bundles.Add(new ScriptBundle("~/bundles/baseScripts").Include(
				"~/Areas/Admin/Assets/js/vendor/jquery/jquery.min.js",
				"~/Areas/Admin/Assets/js/vendor/jquery/jquery-ui.min.js",
				"~/Areas/Admin/Assets/js/vendor/bootstrap-rtl.min.js",
				"~/Areas/Admin/Assets/js/vendor/moment/moment.min.js",
				"~/Areas/Admin/Assets/js/vendor/customscrollbar/jquery.mCustomScrollbar.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/appScripts").Include(
				"~/Areas/Admin/Assets/js/app.js",
				"~/Areas/Admin/Assets/js/app_plugins.js"));
			#endregion

			#region Style Layout Main Page

			bundles.Add(new StyleBundle("~/bundles/baseStyles").Include(
				"~/Areas/Admin/Assets/css/styles.css",
				"~/Areas/Admin/Assets/css/vendor/bootstrap-rtl.min.css",
				"~/Areas/Admin/Assets/css/custom.css"));

			#endregion

			#region Pages

			bundles.Add(new ScriptBundle("~/bundles/mainPage").Include(
					"~/Areas/Admin/Assets/js/vendor/bootstrap-datetimepicker/bootstrap-datetimepicker.js",
					"~/Areas/Admin/Assets/js/vendor/jvectormap/jquery-jvectormap.min.js",
					"~/Areas/Admin/Assets/js/vendor/jvectormap/jquery-jvectormap-world-mill-en.js",
					"~/Areas/Admin/Assets/js/vendor/jvectormap/jquery-jvectormap-us-aea-en.js",
					"~/Areas/Admin/Assets/js/vendor/rickshaw/d3.v3.js",
					"~/Areas/Admin/Assets/js/vendor/rickshaw/rickshaw.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/userPage").Include(
				"~/Areas/Admin/Assets/js/vendor/bootstrap-select/bootstrap-select.js",
				"~/Areas/Admin/Assets/js/vendor/select2/select2.full.min.js"));

			#endregion
		}
	}
}