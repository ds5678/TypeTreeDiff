using System;

namespace TypeTreeDiff.Core.Version
{
	public enum UnityVersionType
	{
		Alpha = 0,
		Beta,
		Base,
		Final,
		Patch,

		MaxValue = Patch,
	}

	public static class UnityVersionTypeExtentions
	{
		public static string ToLiteral(this UnityVersionType _this)
		{
			switch(_this)
			{
				case UnityVersionType.Alpha:
					return "a";

				case UnityVersionType.Beta:
					return "b";

				case UnityVersionType.Base:
					return string.Empty;

				case UnityVersionType.Final:
					return "f";

				case UnityVersionType.Patch:
					return "p";

				default:
					throw new Exception($"Unsupported vertion type {_this}");
			}
		}
	}
}
