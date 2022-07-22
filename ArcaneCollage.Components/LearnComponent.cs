using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Skills.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneCollage.Components
{
    public class LearnComponent : ILearnComponent
    {
        public SkillType skillType { get; set; }

        public double value => baseValue * (1 + details.Sum(x => x.value));
        public double baseValue => 1;
        public IEnumerable<(string desc, double value)> details => _details;


        private List<(string desc, double value)> _details = new List<(string desc, double value)>();

        public void AddDetail(string desc, double efficient)
        {
            _details.Add((desc, efficient));
        }
    }
}
