﻿using TCC.TeraCommon.Game.Messages;
using TCC.TeraCommon.Game.Services;

namespace TCC.Parsing.Messages
{
    public class S_LOGOUT_PARTY_MEMBER : ParsedMessage
    {
        private uint serverId;
        public uint ServerId => serverId;

        private uint playerId;
        public uint PlayerId => playerId;

        public S_LOGOUT_PARTY_MEMBER(TeraMessageReader reader) : base(reader)
        {
            serverId = reader.ReadUInt32();
            playerId = reader.ReadUInt32();
        }
    }
}