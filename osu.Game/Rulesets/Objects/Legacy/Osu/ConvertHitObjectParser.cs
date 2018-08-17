﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using OpenTK;
using osu.Game.Rulesets.Objects.Types;
using System.Collections.Generic;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Objects.Legacy.Osu
{
    /// <summary>
    /// A HitObjectParser to parse legacy osu! Beatmaps.
    /// </summary>
    public class ConvertHitObjectParser : Legacy.ConvertHitObjectParser
    {
        public ConvertHitObjectParser(double offset, int formatVersion)
            : base(offset, formatVersion)
        {
        }

        private bool forceNewCombo;
        private int extraComboOffset;

        protected override HitObject CreateHit(Vector2 position, bool newCombo, int comboOffset)
        {
            newCombo |= forceNewCombo;
            comboOffset += extraComboOffset;

            forceNewCombo = false;
            extraComboOffset = 0;

            return new ConvertHit
            {
                Position = position,
                NewCombo = FirstObject || newCombo,
                ComboOffset = comboOffset
            };
        }

        protected override HitObject CreateSlider(Vector2 position, bool newCombo, int comboOffset, List<Vector2> controlPoints, double length, CurveType curveType, int repeatCount, List<List<SampleInfo>> repeatSamples)
        {
            newCombo |= forceNewCombo;
            comboOffset += extraComboOffset;

            forceNewCombo = false;
            extraComboOffset = 0;

            return new ConvertSlider
            {
                Position = position,
                NewCombo = FirstObject || newCombo,
                ComboOffset = comboOffset,
                ControlPoints = controlPoints,
                Distance = Math.Max(0, length),
                CurveType = curveType,
                RepeatSamples = repeatSamples,
                RepeatCount = repeatCount
            };
        }

        protected override HitObject CreateSpinner(Vector2 position, bool newCombo, int comboOffset, double endTime)
        {
            forceNewCombo |= FormatVersion <= 8 || newCombo;
            extraComboOffset += comboOffset;

            return new ConvertSpinner
            {
                Position = position,
                EndTime = endTime
            };
        }

        protected override HitObject CreateHold(Vector2 position, bool newCombo, int comboOffset, double endTime)
        {
            return null;
        }
    }
}
