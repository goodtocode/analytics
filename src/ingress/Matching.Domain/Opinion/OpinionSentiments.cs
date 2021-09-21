﻿using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace GoodToCode.Matching.Domain
{
    public class OpinionSentiments : RowEntity, IOpinionSentiments
    {
        [JsonInclude]
        public string AnalyzedText { get; private set; }
        [JsonInclude]
        public string Sentiment { get; private set; }
        [JsonInclude]
        public double Negative { get; private set; }
        [JsonInclude]
        public double Neutral { get; private set; }
        [JsonInclude]
        public double Positive { get; private set; }
        [JsonInclude]
        public string LanguageIso { get; private set; }

        public OpinionSentiments() { }

        public OpinionSentiments(ICellData cell, ISentimentResult result) : base(Guid.NewGuid().ToString(), cell)
        {
            AnalyzedText = result.AnalyzedText;
            Sentiment = result.Sentiment.ToString();
            Negative = result.Negative;
            Neutral = result.Neutral;
            Positive = result.Positive;
        }
    }
}