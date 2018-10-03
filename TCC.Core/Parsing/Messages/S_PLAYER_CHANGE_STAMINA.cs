﻿using TCC.Annotations;
using TCC.TeraCommon.Game.Messages;
using TCC.TeraCommon.Game.Services;

namespace TCC.Parsing.Messages
{
    public class S_PLAYER_CHANGE_STAMINA : ParsedMessage
    {

        public int CurrentST { get; }

        public int MaxST { [UsedImplicitly] get; }

        public S_PLAYER_CHANGE_STAMINA(TeraMessageReader reader) : base(reader)
        {
            CurrentST = reader.ReadInt32();
            MaxST = reader.ReadInt32();
            reader.Skip(12);
            //unk1 = reader.ReadInt32();
            //unk2 = reader.ReadInt32();
            //unk3 = reader.ReadInt32();
        }
    }
}