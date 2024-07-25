﻿using Libplanet.Crypto;
using Nekoyume.Model.EnumType;
using Nekoyume.Model.State;

namespace Lib9c.Models.Runes;

public record RuneSlots(
    Address Address,
    BattleType BattleType,
    IEnumerable<RuneSlot> Slots)
{
    public RuneSlots(
        Address address,
        RuneSlotState runeSlotState)
        : this(
            address,
            runeSlotState.BattleType,
            runeSlotState.GetRuneSlot().Select(runeSlot => new RuneSlot(runeSlot)))
    {
    }
}