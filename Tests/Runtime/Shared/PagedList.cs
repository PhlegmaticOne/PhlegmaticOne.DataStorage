using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Contracts;

namespace Phlegmaticone.DataStorage.Tests.Runtime.Shared
{
    [Serializable]
    [DataContract]
    internal class PagedList : IModel
    {
        [JsonProperty] [DataMember] public int PageIndex { get; set; }

        [JsonProperty] [DataMember] public int PageSize { get; set; }

        [JsonProperty] [DataMember] public int TotalCount { get; set; }

        [JsonProperty] [DataMember] public int TotalPages { get; set; }

        [JsonProperty] [DataMember] public int IndexFrom { get; set; }

        [JsonProperty] [DataMember] public IList<string> Items { get; set; }

        [JsonIgnore] [IgnoreDataMember] public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        [JsonIgnore] [IgnoreDataMember] public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

        public static PagedList OnePage(IEnumerable<string> values)
        {
            var items = values.ToList();
            return new PagedList
            {
                Items = items,
                IndexFrom = 0,
                PageIndex = 0,
                PageSize = items.Count,
                TotalCount = items.Count,
                TotalPages = 1
            };
        }
    }
}