using System;
using System.Collections.Generic;
using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
	internal sealed class RecordSettings
	{
		public RecordSettings(IList<IOrderedMemberInfo> orderedMemberInfos, FormatOptions options)
		{
			OrderedMembers = orderedMemberInfos;
			Options = options;
		}

		public int RowIndex { get; set; }
	    public Type RecordType { get; set; }
	    public IList<IOrderedMemberInfo> OrderedMembers { get; internal set; }
	    public FormatOptions Options { get; }
	}
}