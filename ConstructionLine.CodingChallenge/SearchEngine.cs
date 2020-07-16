using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    using System;
    using System.Data;
    using System.Linq;

    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // No indices used
        }

        public SearchResults Search(SearchOptions options)
        {
            var colors = options.Colors != null ? options.Colors.Select(c => c.Id).ToList() : new List<Guid>();
            var sizes = options.Sizes != null ? options.Sizes.Select(c => c.Id).ToList() : new List<Guid>();

            return Search(colors, sizes);
        }

        private SearchResults Search(ICollection<Guid> colors, ICollection<Guid> sizes)
        {
            // Approach: Keep it simple and pass the tests

            // Cache options to speed up matching
            int colorCount = colors.Count;
            int sizeCount = sizes.Count;

            // Do a simple linear scan for matching shirts using LINQ
            var shirts = _shirts.Where(s => colorCount == 0 || colors.Contains(s.Color.Id) && 
                                                                 sizeCount == 0  || sizes.Contains(s.Size.Id));

            // Count the shirts
            // NOTE: This passes the tests, but this still makes no sense to me and cost me loads of time.
            // This should surely count the matching shirts in the return data set, rather than in the original data set?
            // This is ambiguous in the original question and is pretty confusing.
            var sizeCounts = new List<SizeCount>();
            foreach (var size in Size.All)
            {
                int count = _shirts.Count(c => c.Size.Id == size.Id && (colorCount == 0 || colors.Contains(c.Color.Id)));
                sizeCounts.Add(new SizeCount { Size = size, Count = count });
            }

            var colorCounts = new List<ColorCount>();
            foreach (var color in Color.All)
            {
                int count = _shirts.Count(c => c.Color.Id == color.Id && (sizeCount == 0 || sizes.Contains(c.Size.Id)));
                colorCounts.Add(new ColorCount { Color = color, Count = count });
            }

            return new SearchResults
            {
                Shirts = shirts.ToList(),
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };
        }
    }
}