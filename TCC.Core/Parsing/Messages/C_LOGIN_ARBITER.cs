﻿using TCC.TeraCommon.Game.Messages;
using TCC.TeraCommon.Game.Services;

namespace TCC.Parsing.Messages
{
    public class C_LOGIN_ARBITER : ParsedMessage
    {
        internal C_LOGIN_ARBITER(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(15);
            //Language = (LangEnum)reader.ReadUInt32();
            Version = reader.ReadInt32();
            reader.Factory.ReleaseVersion = Version;
        }

        public int Version { get; set; }
    }

}
